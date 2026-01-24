Imports SharpDX.DirectInput
Imports System.Runtime.InteropServices

Module SupermodelControllerMapper

    Private directInput As DirectInput

    ' 統合された接続済みデバイスを格納
    Public joysticks As New List(Of Joystick)


    ' 統合リストから Player1～4 に割り当て
    Public Function MapControllersToPlayers() As List(Of String)
        Dim controllerList As New List(Of String)
        directInput = New DirectInput()

        ' ゲームパッド / ジョイスティックを列挙
        Dim devices = directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AttachedOnly).ToList()
        devices.AddRange(directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AttachedOnly))

        Dim index As Integer = 1

        For Each dev In devices
            Try
                ' 接続済みか確認
                Dim js = New Joystick(directInput, dev.InstanceGuid)
                js.Acquire()
                Dim state = js.GetCurrentState() ' 接続チェック
                joysticks.Add(js)
                controllerList.Add($"JOY{index} - {dev.InstanceName} (DirectInput)")
                index += 1
            Catch ex As Exception
                ' 非接続 / ゴーストは無視
            End Try
        Next

        Return controllerList
    End Function

End Module

