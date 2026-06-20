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

Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms

Public Class DiscordAuth
    Private Shared ReadOnly HttpClient As New HttpClient()
    Private Const SessionFileName As String = "discord_session.dat"

    ' =========================================================================================
    ' ⚠️ Discord Credentials Embedding Area
    ' * When building the production binary, write the actual credentials here.
    ' * When committing to GitHub, always replace this part with dummy strings.
    ' =========================================================================================
    Private Const DiscordClientId As String = "YourClientId"
    Private Const DiscordClientSecret As String = "YourClientSecret"
    Private Const DiscordBotToken As String = "YourBotToken"
    Private Const DiscordGuildId As String = "YourGuildId"
    Private Const DiscordInviteUrl As String = "https://discord.gg/mNjPJHTTen"
    ' =========================================================================================

    Public Class AuthResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
        Public Property UserJoined As Boolean
        Public Property UserId As String
        Public Property Username As String
    End Class

    ' Session storage structure
    Private Class SessionData
        Public Property RefreshToken As String
        Public Property SavedAt As DateTime
    End Class

    ''' <summary>
    ''' Performs OAuth2 authentication with Discord and verifies server membership. Automatically refreshes cached token if within 30 days.
    ''' </summary>
    Public Shared Async Function AuthenticateAndCheckMembershipAsync(statusCallback As Action(Of String)) As Task(Of AuthResult)
        Dim result As New AuthResult()

        ' Simple check
        If String.IsNullOrEmpty(DiscordClientId) OrElse DiscordClientId = "YOUR_CLIENT_ID" OrElse
           String.IsNullOrEmpty(DiscordClientSecret) OrElse DiscordClientSecret = "YOUR_CLIENT_SECRET" OrElse
           String.IsNullOrEmpty(DiscordBotToken) OrElse DiscordBotToken = "YOUR_BOT_TOKEN" OrElse
           String.IsNullOrEmpty(DiscordGuildId) OrElse DiscordGuildId = "YOUR_GUILD_ID" Then
            result.Success = False
            result.ErrorMessage = "Discord credentials have not been embedded. Please check the constants in the DiscordAuth.vb code."
            Return result
        End If

        Dim accessToken As String = ""
        Dim newRefreshToken As String = ""

        ' 1. Load and verify saved session locally
        Dim session = LoadSession()
        If session IsNot Nothing AndAlso (DateTime.Now - session.SavedAt).TotalDays < 30 Then
            statusCallback("Updating previous session automatically...")
            ' Refresh the token using the refresh token
            Dim tokenJson = Await RefreshAccessTokenAsync(DiscordClientId, DiscordClientSecret, session.RefreshToken)
            If Not String.IsNullOrEmpty(tokenJson) Then
                accessToken = ParseJsonValue(tokenJson, "access_token")
                newRefreshToken = ParseJsonValue(tokenJson, "refresh_token")
                If Not String.IsNullOrEmpty(accessToken) AndAlso Not String.IsNullOrEmpty(newRefreshToken) Then
                    SaveSession(newRefreshToken)
                End If
            End If
        End If

        ' 2. Perform browser-based OAuth authentication if automatic refresh failed
        If String.IsNullOrEmpty(accessToken) Then
            Dim redirectUri As String = "http://localhost:5050/"
            Dim state As String = Guid.NewGuid().ToString("N")

            ' Start HttpListener
            Dim listener As HttpListener = Nothing
            Try
                listener = New HttpListener()
                listener.Prefixes.Add(redirectUri)
                listener.Start()
            Catch ex As Exception
                result.Success = False
                result.ErrorMessage = "Failed to start listener on local port 5050: " & ex.Message
                Return result
            End Try

            Try
                ' Open OAuth authentication page in browser
                Dim authUrl As String = $"https://discord.com/api/oauth2/authorize?client_id={DiscordClientId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&response_type=code&scope=identify&state={state}"
                statusCallback("Please perform Discord authentication in your browser...")
                System.Diagnostics.Process.Start(authUrl)

                ' 3. Wait for authorization code (60 seconds timeout)
                Dim listenTask = listener.GetContextAsync()
                Dim delayTask = Task.Delay(60000)
                Dim completedTask = Await Task.WhenAny(listenTask, delayTask)

                If completedTask Is delayTask Then
                    result.Success = False
                    result.ErrorMessage = "Discord authentication timed out (60 seconds)."
                    Return result
                End If

                Dim context = Await listenTask
                Dim request = context.Request
                Dim response = context.Response

                Dim code As String = request.QueryString("code")
                Dim rcvState As String = request.QueryString("state")

                ' Return response screen
                Dim responseString As String = ""
                If String.IsNullOrEmpty(code) OrElse rcvState <> state Then
                    responseString = "<html><body><h2>Authentication Failed</h2><p>Invalid state or missing code.</p></body></html>"
                    result.Success = False
                    result.ErrorMessage = "Authentication failed (State mismatch or code missing)"
                Else
                    responseString = "<html><head><meta charset='utf-8'></head><body><h2>Authentication Complete</h2><p>Authentication was successful. Please close this window and return to the application.</p></body></html>"
                End If

                Dim buffer As Byte() = Encoding.UTF8.GetBytes(responseString)
                response.ContentLength64 = buffer.Length
                Using output = response.OutputStream
                    Await output.WriteAsync(buffer, 0, buffer.Length)
                End Using
                response.Close()

                If Not result.Success AndAlso Not String.IsNullOrEmpty(result.ErrorMessage) Then
                    Return result
                End If

                statusCallback("Obtaining token...")

                ' Obtain access token
                Dim tokenParams As New Dictionary(Of String, String) From {
                    {"client_id", DiscordClientId},
                    {"client_secret", DiscordClientSecret},
                    {"grant_type", "authorization_code"},
                    {"code", code},
                    {"redirect_uri", redirectUri}
                }
                Dim tokenContent As New FormUrlEncodedContent(tokenParams)
                Dim tokenResponse = Await HttpClient.PostAsync("https://discord.com/api/v10/oauth2/token", tokenContent)
                If Not tokenResponse.IsSuccessStatusCode Then
                    result.Success = False
                    result.ErrorMessage = "Failed to obtain token: " & tokenResponse.StatusCode.ToString()
                    Return result
                End If

                Dim tokenJson As String = Await tokenResponse.Content.ReadAsStringAsync()
                accessToken = ParseJsonValue(tokenJson, "access_token")
                newRefreshToken = ParseJsonValue(tokenJson, "refresh_token")

                If Not String.IsNullOrEmpty(accessToken) AndAlso Not String.IsNullOrEmpty(newRefreshToken) Then
                    SaveSession(newRefreshToken)
                Else
                    result.Success = False
                    result.ErrorMessage = "Failed to parse access token."
                    Return result
                End If

            Catch ex As Exception
                result.Success = False
                result.ErrorMessage = "An exception occurred during authentication: " & ex.Message
                Return result
            Finally
                If listener IsNot Nothing Then
                    Try
                        listener.Stop()
                        listener.Close()
                    Catch
                    End Try
                End If
            End Try
        End If

        Try
            statusCallback("Retrieving user info...")

            ' 3. Retrieve user profile (@me)
            Dim userRequest As New HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/v10/users/@me")
            userRequest.Headers.Authorization = New System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken)
            Dim userResponse = Await HttpClient.SendAsync(userRequest)
            If Not userResponse.IsSuccessStatusCode Then
                ' Session might be stale, delete session
                DeleteSession()
                result.Success = False
                result.ErrorMessage = "Failed to retrieve user info: " & userResponse.StatusCode.ToString()
                Return result
            End If

            Dim userJson As String = Await userResponse.Content.ReadAsStringAsync()
            Dim userId As String = ParseJsonValue(userJson, "id")
            Dim username As String = ParseJsonValue(userJson, "username")

            result.UserId = userId
            result.Username = username

            statusCallback("Verifying server membership...")

            ' 4. Use Bot Token to verify membership in specified guild
            Dim memberRequest As New HttpRequestMessage(HttpMethod.Get, $"https://discord.com/api/v10/guilds/{DiscordGuildId}/members/{userId}")
            memberRequest.Headers.Authorization = New System.Net.Http.Headers.AuthenticationHeaderValue("Bot", DiscordBotToken)
            Dim memberResponse = Await HttpClient.SendAsync(memberRequest)

            If memberResponse.StatusCode = HttpStatusCode.OK Then
                result.Success = True
                result.UserJoined = True
            ElseIf memberResponse.StatusCode = HttpStatusCode.NotFound Then
                result.Success = True
                result.UserJoined = False
                result.ErrorMessage = "You are not a member of the designated Discord server."
            Else
                result.Success = False
                result.ErrorMessage = "Failed to verify server membership: " & memberResponse.StatusCode.ToString()
            End If

        Catch ex As Exception
            result.Success = False
            result.ErrorMessage = "An exception occurred during authentication: " & ex.Message
        End Try

        Return result
    End Function

    ''' <summary>
    ''' Retrieves access token again using a refresh token.
    ''' </summary>
    Private Shared Async Function RefreshAccessTokenAsync(clientId As String, clientSecret As String, refreshToken As String) As Task(Of String)
        Try
            Dim tokenParams As New Dictionary(Of String, String) From {
                {"client_id", clientId},
                {"client_secret", clientSecret},
                {"grant_type", "refresh_token"},
                {"refresh_token", refreshToken}
            }
            Dim tokenContent As New FormUrlEncodedContent(tokenParams)
            Dim tokenResponse = Await HttpClient.PostAsync("https://discord.com/api/v10/oauth2/token", tokenContent)
            If tokenResponse.IsSuccessStatusCode Then
                Return Await tokenResponse.Content.ReadAsStringAsync()
            End If
        Catch
        End Try
        Return ""
    End Function

    ''' <summary>
    ''' Saves session information.
    ''' </summary>
    Private Shared Sub SaveSession(refreshToken As String)
        Try
            Dim path = GetSessionPath()
            Using writer As New BinaryWriter(File.Open(path, FileMode.Create))
                writer.Write(refreshToken)
                writer.Write(DateTime.Now.ToBinary())
            End Using
        Catch ex As Exception
            Debug.WriteLine("Failed to save session: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Loads session information.
    ''' </summary>
    Private Shared Function LoadSession() As SessionData
        Try
            Dim path = GetSessionPath()
            If Not File.Exists(path) Then Return Nothing

            Using reader As New BinaryReader(File.Open(path, FileMode.Open))
                Dim token = reader.ReadString()
                Dim binaryDate = reader.ReadInt64()
                Dim savedAt = DateTime.FromBinary(binaryDate)
                Return New SessionData() With {.RefreshToken = token, .SavedAt = savedAt}
            End Using
        Catch
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Deletes session information.
    ''' </summary>
    Private Shared Sub DeleteSession()
        Try
            Dim path = GetSessionPath()
            If File.Exists(path) Then
                File.Delete(path)
            End If
        Catch
        End Try
    End Sub

    Private Shared Function GetSessionPath() As String
        Return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SessionFileName)
    End Function

    ''' <summary>
    ''' Simple helper to extract string values for a specified key from JSON.
    ''' </summary>
    Private Shared Function ParseJsonValue(json As String, key As String) As String
        Try
            Dim searchKey As String = $"""{key}"""
            Dim index As Integer = json.IndexOf(searchKey)
            If index < 0 Then Return ""

            ' Find ":" after the key
            Dim colonIndex As Integer = json.IndexOf(":", index + searchKey.Length)
            If colonIndex < 0 Then Return ""

            ' Find start of value (double quote)
            Dim quoteStart As Integer = json.IndexOf("""", colonIndex)
            If quoteStart < 0 Then Return ""

            ' Find end of value
            Dim quoteEnd As Integer = json.IndexOf("""", quoteStart + 1)
            If quoteEnd < 0 Then Return ""

            Return json.Substring(quoteStart + 1, quoteEnd - quoteStart - 1)
        Catch
            Return ""
        End Try
    End Function

    Public Shared Function GetInviteUrl() As String
        Return DiscordInviteUrl
    End Function
End Class
