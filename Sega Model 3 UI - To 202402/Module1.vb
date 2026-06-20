' Copyright (C) 2026 BackPonBeauty
'
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with this program.  If not, see <https://www.gnu.org/licenses/>.

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

