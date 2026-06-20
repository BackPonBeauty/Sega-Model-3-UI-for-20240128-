' Copyright (C) 2026 BackPonBeauty
'
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with this program. If not, see <https://www.gnu.org/licenses/>.

Imports Firebase.Auth
Imports Firebase.Auth.Providers
Imports Firebase.Database
Imports Firebase.Database.Query
Imports Firebase.Database.Streaming
Imports Newtonsoft.Json
Imports System.Reactive.Linq

Public Class SlotInfo
    <JsonProperty("xinput")>
    Public Property XInput As Integer

    <JsonProperty("video")>
    Public Property Video As Integer

    <JsonProperty("audio")>
    Public Property Audio As Integer

    <JsonProperty("available")>
    Public Property Available As Boolean

    <JsonProperty("user")>
    Public Property User As String
End Class

Public Class HostInfo
    <JsonProperty("timestamp")>
    Public Property Timestamp As Long

    <JsonProperty("ip")>
    Public Property Ip As String

    <JsonProperty("gametitle")>
    Public Property GameTitle As String

    <JsonProperty("servername")>
    Public Property ServerName As String

    <JsonProperty("slot1")>
    Public Property Slot1 As SlotInfo

    <JsonProperty("slot2")>
    Public Property Slot2 As SlotInfo

    <JsonProperty("slot3")>
    Public Property Slot3 As SlotInfo

    <JsonProperty("slot4")>
    Public Property Slot4 As SlotInfo

    ' 既存コードとの互換性のためSlots辞書を動的生成
    Public ReadOnly Property Slots As Dictionary(Of String, SlotInfo)
        Get
            Dim d As New Dictionary(Of String, SlotInfo)
            If Slot1 IsNot Nothing Then d("slot1") = Slot1
            If Slot2 IsNot Nothing Then d("slot2") = Slot2
            If Slot3 IsNot Nothing Then d("slot3") = Slot3
            If Slot4 IsNot Nothing Then d("slot4") = Slot4
            Return d
        End Get
    End Property
End Class

Public Class ChatMessage
    <JsonProperty("username")>
    Public Property Username As String

    <JsonProperty("message")>
    Public Property Message As String

    <JsonProperty("badge")>
    Public Property Badge As String

    <JsonProperty("timestamp")>
    Public Property Timestamp As Long
End Class

Public Class FirebaseMatchingClient
    Private Const ApiKey As String = "YourApiKey"
    Private Const DbUrl As String = "https://YourDbUrl.firebasedatabase.app"
    Private Const AuthDomain As String = "YourAuthDomain"
    Private Const TimeoutMinutes As Integer = 10

    Private dbClient As FirebaseClient
    Private authClient As FirebaseAuthClient

    ' -------------------------------------------------------
    ' 初期化（匿名認証）
    ' -------------------------------------------------------
    Public Async Function InitializeAsync() As Task
        Try
            Dim config = New FirebaseAuthConfig() With {
                .ApiKey = ApiKey,
                .AuthDomain = AuthDomain,
                .Providers = New FirebaseAuthProvider() {}
            }
            authClient = New FirebaseAuthClient(config)
            Dim userCredential = Await authClient.SignInAnonymouslyAsync()

            dbClient = New FirebaseClient(DbUrl, New FirebaseOptions With {
                .AuthTokenAsyncFactory = Async Function() Await userCredential.User.GetIdTokenAsync()
            })

            Debug.WriteLine("[Firebase] Login Success UID: " & userCredential.User.Uid)
        Catch ex As Exception
            Debug.WriteLine("[ERROR] Firebase Init Failed: " & ex.Message)
        End Try
    End Function

    ' -------------------------------------------------------
    ' ホスト一覧取得（タイムアウト除外済み）
    ' -------------------------------------------------------
    Public Async Function GetActiveHostsAsync() As Task(Of Dictionary(Of String, HostInfo))
        Dim result = New Dictionary(Of String, HostInfo)
        Try
            Dim hosts = Await dbClient.Child("hosts").OnceAsync(Of HostInfo)()
            Dim nowUnix = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            Dim timeoutMs = TimeoutMinutes * 60 * 1000L

            For Each host In hosts
                If host.Object IsNot Nothing Then
                    Dim elapsed = nowUnix - host.Object.Timestamp
                    If elapsed <= timeoutMs Then
                        result(host.Key) = host.Object
                    End If
                End If
            Next
        Catch ex As Exception
            Debug.WriteLine("[ERROR] Failed to fetch host list: " & ex.Message)
        End Try
        Return result
    End Function

    ' -------------------------------------------------------
    ' スロットの接続ユーザー情報の更新/クリア
    ' -------------------------------------------------------
    Public Async Function UpdateSlotUserAsync(hostId As String, slotIndex As Integer, username As String, isJoining As Boolean) As Task
        Try
            If String.IsNullOrEmpty(username) Then Return

            Dim slotRef = dbClient.Child("hosts").Child(hostId).Child("slot" & slotIndex.ToString()).Child("user")
            Dim currentVal = Await slotRef.OnceSingleAsync(Of String)()

            If isJoining Then
                If String.IsNullOrEmpty(currentVal) Then
                    Await slotRef.PutAsync(Of String)(username)
                Else
                    Dim users = currentVal.Split(New String() {", "}, StringSplitOptions.RemoveEmptyEntries)
                    If Not users.Contains(username) Then
                        Dim newVal = currentVal & ", " & username
                        Await slotRef.PutAsync(Of String)(newVal)
                    End If
                End If
            Else
                If Not String.IsNullOrEmpty(currentVal) Then
                    Dim users = currentVal.Split(New String() {", "}, StringSplitOptions.RemoveEmptyEntries).ToList()
                    If users.Contains(username) Then
                        users.Remove(username)
                        If users.Count = 0 Then
                            Await slotRef.PutAsync(Of String)(Nothing)
                        Else
                            Dim newVal = String.Join(", ", users)
                            Await slotRef.PutAsync(Of String)(newVal)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Debug.WriteLine("[ERROR] UpdateSlotUserAsync: " & ex.Message)
        End Try
    End Function

    ' -------------------------------------------------------
    ' チャットメッセージ送信
    ' -------------------------------------------------------
    Public Async Function SendChatMessageAsync(hostId As String, username As String, message As String) As Task
        Try
            Dim hostObj = Await dbClient.Child("hosts").Child(hostId).OnceSingleAsync(Of HostInfo)()
            If hostObj Is Nothing Then
                Debug.WriteLine("[SendChatMessageAsync] Host node does not exist. Skipping chat message.")
                Return
            End If

            Dim chatMsg As New ChatMessage() With {
                .Username = username,
                .Message = message,
                .Badge = "",
                .Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            }
            Await dbClient.Child("hosts").Child(hostId).Child("chat").PostAsync(chatMsg)
        Catch ex As Exception
            Debug.WriteLine("[ERROR] SendChatMessageAsync: " & ex.Message)
        End Try
    End Function

    ' -------------------------------------------------------
    ' チャット監視のストリーム取得
    ' -------------------------------------------------------
    Public Function GetChatObservable(hostId As String) As IObservable(Of FirebaseEvent(Of ChatMessage))
        Return dbClient.Child("hosts").Child(hostId).Child("chat").AsObservable(Of ChatMessage)()
    End Function

    ' -------------------------------------------------------
    ' ホスト監視のストリーム取得
    ' -------------------------------------------------------
    Public Function GetHostsObservable() As IObservable(Of FirebaseEvent(Of HostInfo))
        Return dbClient.Child("hosts").AsObservable(Of HostInfo)()
    End Function

    Public Async Function GetRemoteVersionAsync(appKey As String) As Task(Of String)
        Try
            If dbClient Is Nothing Then Return Nothing
            Return Await dbClient.Child(appKey).OnceSingleAsync(Of String)()
        Catch ex As Exception
            Debug.WriteLine("[ERROR] GetRemoteVersionAsync: " & ex.Message)
            Return Nothing
        End Try
    End Function
End Class