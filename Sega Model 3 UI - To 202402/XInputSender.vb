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

Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports SharpDX.XInput
Imports State = SharpDX.XInput.State
Imports System.Windows.Forms
Imports System.Collections.Generic

Public Class XInputSender

    Public Event ControllerDisconnected()

    Private _controller As Controller
    Private _udpClient As UdpClient
    Private _endPoint As IPEndPoint
    Private _running As Boolean
    Private _thread As Thread

    ' Keyboard fallback state
    Private Shared _pressedKeys As New HashSet(Of Keys)()
    Private Shared _keysLock As New Object()

    Public Shared KeyMapping As New Dictionary(Of String, Keys) From {
        {"DpadUp", Keys.None},
        {"DpadDown", Keys.None},
        {"DpadLeft", Keys.None},
        {"DpadRight", Keys.None},
        {"A", Keys.J},
        {"B", Keys.K},
        {"X", Keys.U},
        {"Y", Keys.I},
        {"LB", Keys.None},
        {"RB", Keys.None},
        {"LTrigger", Keys.None},
        {"RTrigger", Keys.None},
        {"LThumb", Keys.None},
        {"RThumb", Keys.None},
        {"Start", Keys.Enter},
        {"Back", Keys.Space},
        {"LStickUp", Keys.W},
        {"LStickDown", Keys.S},
        {"LStickLeft", Keys.A},
        {"LStickRight", Keys.D},
        {"RStickUp", Keys.None},
        {"RStickDown", Keys.None},
        {"RStickLeft", Keys.None},
        {"RStickRight", Keys.None}
    }

    ' Controller input mapping (Action -> Physical Input Name)
    Public Shared PadMapping As New Dictionary(Of String, String) From {
        {"DpadUp", "DpadUp"},
        {"DpadDown", "DpadDown"},
        {"DpadLeft", "DpadLeft"},
        {"DpadRight", "DpadRight"},
        {"A", "A"},
        {"B", "B"},
        {"X", "X"},
        {"Y", "Y"},
        {"LB", "LB"},
        {"RB", "RB"},
        {"LTrigger", "LTrigger"},
        {"RTrigger", "RTrigger"},
        {"LThumb", "LThumb"},
        {"RThumb", "RThumb"},
        {"Start", "Start"},
        {"Back", "Back"},
        {"LStickUp", "LStickUp"},
        {"LStickDown", "LStickDown"},
        {"LStickLeft", "LStickLeft"},
        {"LStickRight", "LStickRight"},
        {"RStickUp", "RStickUp"},
        {"RStickDown", "RStickDown"},
        {"RStickLeft", "RStickLeft"},
        {"RStickRight", "RStickRight"}
    }

    Public Shared Sub UpdateKeyState(key As Keys, isPressed As Boolean)
        SyncLock _keysLock
            If isPressed Then
                _pressedKeys.Add(key)
            Else
                _pressedKeys.Remove(key)
            End If
        End SyncLock
    End Sub

    Public Shared Sub LoadConfig()
        Dim path = IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.cfg")
        If Not IO.File.Exists(path) Then
            SaveConfig()
            Return
        End If

        Try
            ' Pre-populate defaults in case config is partial
            Dim lines = IO.File.ReadAllLines(path)
            For Each line In lines
                Dim parts = line.Split("="c)
                If parts.Length = 2 Then
                    Dim keyName = parts(0).Trim()
                    Dim val = parts(1).Trim()

                    If keyName.StartsWith("Kb_") Then
                        Dim action = keyName.Substring(3)
                        Dim keyVal As Keys
                        If [Enum].TryParse(Of Keys)(val, True, keyVal) Then
                            If KeyMapping.ContainsKey(action) Then
                                KeyMapping(action) = keyVal
                            End If
                        End If
                    ElseIf keyName.StartsWith("Pad_") Then
                        Dim action = keyName.Substring(4)
                        If PadMapping.ContainsKey(action) Then
                            PadMapping(action) = val
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Config] Load error: " & ex.Message)
        End Try
    End Sub

    Public Shared Sub SaveConfig()
        Dim path = IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.cfg")
        Try
            Dim lines As New List(Of String)()
            For Each kvp In KeyMapping
                lines.Add($"Kb_{kvp.Key}={kvp.Value.ToString()}")
            Next
            For Each kvp In PadMapping
                lines.Add($"Pad_{kvp.Key}={kvp.Value}")
            Next
            IO.File.WriteAllLines(path, lines.ToArray())
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Config] Save error: " & ex.Message)
        End Try
    End Sub

    Public Sub Start(destIP As String, destPort As Integer, playerIndex As UserIndex)
        _controller = New Controller(playerIndex)
        _udpClient = New UdpClient()
        _endPoint = New IPEndPoint(IPAddress.Parse(destIP), destPort)
        _running = True
        _thread = New Thread(AddressOf SendLoop)
        _thread.IsBackground = True
        _thread.Start()
    End Sub

    Private Sub SendLoop()
        While _running
            Dim pkt() As Byte
            If _controller.IsConnected Then
                Try
                    Dim state As State
                    _controller.GetState(state)
                    pkt = GetMappedGamepadPacket(state.Gamepad)
                Catch ex As Exception
                    pkt = GetKeyboardStatePacket()
                End Try
            Else
                pkt = GetKeyboardStatePacket()
            End If
            _udpClient.Send(pkt, pkt.Length, _endPoint)

            Thread.Sleep(16)
        End While
    End Sub


    ' Detect active controls on a controller (used for configuration screen)
    Public Shared Function ScanActiveControllerInput(g As Gamepad) As String
        ' Thresholds
        Const TriggerThreshold As Byte = 100
        Const StickThreshold As Short = 15000

        ' Check normal buttons
        Dim b = g.Buttons
        If (b And GamepadButtonFlags.A) <> 0 Then Return "A"
        If (b And GamepadButtonFlags.B) <> 0 Then Return "B"
        If (b And GamepadButtonFlags.X) <> 0 Then Return "X"
        If (b And GamepadButtonFlags.Y) <> 0 Then Return "Y"
        If (b And GamepadButtonFlags.Start) <> 0 Then Return "Start"
        If (b And GamepadButtonFlags.Back) <> 0 Then Return "Back"
        If (b And GamepadButtonFlags.LeftShoulder) <> 0 Then Return "LB"
        If (b And GamepadButtonFlags.RightShoulder) <> 0 Then Return "RB"
        If (b And GamepadButtonFlags.LeftThumb) <> 0 Then Return "LThumb"
        If (b And GamepadButtonFlags.RightThumb) <> 0 Then Return "RThumb"
        If (b And GamepadButtonFlags.DPadUp) <> 0 Then Return "DpadUp"
        If (b And GamepadButtonFlags.DPadDown) <> 0 Then Return "DpadDown"
        If (b And GamepadButtonFlags.DPadLeft) <> 0 Then Return "DpadLeft"
        If (b And GamepadButtonFlags.DPadRight) <> 0 Then Return "DpadRight"

        ' Check triggers
        If g.LeftTrigger > TriggerThreshold Then Return "LTrigger"
        If g.RightTrigger > TriggerThreshold Then Return "RTrigger"

        ' Check sticks
        If g.LeftThumbY > StickThreshold Then Return "LStickUp"
        If g.LeftThumbY < -StickThreshold Then Return "LStickDown"
        If g.LeftThumbX < -StickThreshold Then Return "LStickLeft"
        If g.LeftThumbX > StickThreshold Then Return "LStickRight"

        If g.RightThumbY > StickThreshold Then Return "RStickUp"
        If g.RightThumbY < -StickThreshold Then Return "RStickDown"
        If g.RightThumbX < -StickThreshold Then Return "RStickLeft"
        If g.RightThumbX > StickThreshold Then Return "RStickRight"

        Return Nothing
    End Function

    Private Function GetMappedGamepadPacket(g As Gamepad) As Byte()
        Dim buttons As UShort = 0
        Dim leftTrigger As Byte = 0
        Dim rightTrigger As Byte = 0
        Dim thumbLX As Short = 0
        Dim thumbLY As Short = 0
        Dim thumbRX As Short = 0
        Dim thumbRY As Short = 0

        If IsPadActionActive(g, "DpadUp") Then buttons = buttons Or &H1
        If IsPadActionActive(g, "DpadDown") Then buttons = buttons Or &H2
        If IsPadActionActive(g, "DpadLeft") Then buttons = buttons Or &H4
        If IsPadActionActive(g, "DpadRight") Then buttons = buttons Or &H8
        If IsPadActionActive(g, "Start") Then buttons = buttons Or &H10
        If IsPadActionActive(g, "Back") Then buttons = buttons Or &H20
        If IsPadActionActive(g, "LThumb") Then buttons = buttons Or &H40
        If IsPadActionActive(g, "RThumb") Then buttons = buttons Or &H80
        If IsPadActionActive(g, "LB") Then buttons = buttons Or &H100
        If IsPadActionActive(g, "RB") Then buttons = buttons Or &H200
        If IsPadActionActive(g, "A") Then buttons = buttons Or &H1000
        If IsPadActionActive(g, "B") Then buttons = buttons Or &H2000
        If IsPadActionActive(g, "X") Then buttons = buttons Or &H4000
        If IsPadActionActive(g, "Y") Then buttons = buttons Or &H8000

        If IsPadActionActive(g, "LTrigger") Then leftTrigger = 255
        If IsPadActionActive(g, "RTrigger") Then rightTrigger = 255

        If IsPadActionActive(g, "LStickUp") Then thumbLY = 32767
        If IsPadActionActive(g, "LStickDown") Then thumbLY = -32768
        If IsPadActionActive(g, "LStickLeft") Then thumbLX = -32768
        If IsPadActionActive(g, "LStickRight") Then thumbLX = 32767

        If IsPadActionActive(g, "RStickUp") Then thumbRY = 32767
        If IsPadActionActive(g, "RStickDown") Then thumbRY = -32768
        If IsPadActionActive(g, "RStickLeft") Then thumbRX = -32768
        If IsPadActionActive(g, "RStickRight") Then thumbRX = 32767

        Dim buf(19) As Byte
        buf(0) = CByte(buttons And &HFF)
        buf(1) = CByte((buttons >> 8) And &HFF)
        buf(2) = leftTrigger
        buf(3) = rightTrigger
        BitConverter.GetBytes(thumbLX).CopyTo(buf, 4)
        BitConverter.GetBytes(thumbLY).CopyTo(buf, 6)
        BitConverter.GetBytes(thumbRX).CopyTo(buf, 8)
        BitConverter.GetBytes(thumbRY).CopyTo(buf, 10)
        Return buf
    End Function

    Private Function IsPadActionActive(g As Gamepad, action As String) As Boolean
        If Not PadMapping.ContainsKey(action) Then Return False
        Dim physicalInput = PadMapping(action)

        ' Check normal buttons
        Dim b = g.Buttons
        Select Case physicalInput
            Case "A" : Return (b And GamepadButtonFlags.A) <> 0
            Case "B" : Return (b And GamepadButtonFlags.B) <> 0
            Case "X" : Return (b And GamepadButtonFlags.X) <> 0
            Case "Y" : Return (b And GamepadButtonFlags.Y) <> 0
            Case "Start" : Return (b And GamepadButtonFlags.Start) <> 0
            Case "Back" : Return (b And GamepadButtonFlags.Back) <> 0
            Case "LB" : Return (b And GamepadButtonFlags.LeftShoulder) <> 0
            Case "RB" : Return (b And GamepadButtonFlags.RightShoulder) <> 0
            Case "LThumb" : Return (b And GamepadButtonFlags.LeftThumb) <> 0
            Case "RThumb" : Return (b And GamepadButtonFlags.RightThumb) <> 0
            Case "DpadUp" : Return (b And GamepadButtonFlags.DPadUp) <> 0
            Case "DpadDown" : Return (b And GamepadButtonFlags.DPadDown) <> 0
            Case "DpadLeft" : Return (b And GamepadButtonFlags.DPadLeft) <> 0
            Case "DpadRight" : Return (b And GamepadButtonFlags.DPadRight) <> 0
        End Select

        ' Check triggers (threshold 80)
        If physicalInput = "LTrigger" AndAlso g.LeftTrigger > 80 Then Return True
        If physicalInput = "RTrigger" AndAlso g.RightTrigger > 80 Then Return True

        ' Check sticks (threshold 12000)
        Select Case physicalInput
            Case "LStickUp" : Return g.LeftThumbY > 12000
            Case "LStickDown" : Return g.LeftThumbY < -12000
            Case "LStickLeft" : Return g.LeftThumbX < -12000
            Case "LStickRight" : Return g.LeftThumbX > 12000
            Case "RStickUp" : Return g.RightThumbY > 12000
            Case "RStickDown" : Return g.RightThumbY < -12000
            Case "RStickLeft" : Return g.RightThumbX < -12000
            Case "RStickRight" : Return g.RightThumbX > 12000
        End Select

        Return False
    End Function

    Private Function GetKeyboardStatePacket() As Byte()
        Dim buttons As UShort = 0
        Dim leftTrigger As Byte = 0
        Dim rightTrigger As Byte = 0
        Dim thumbLX As Short = 0
        Dim thumbLY As Short = 0
        Dim thumbRX As Short = 0
        Dim thumbRY As Short = 0

        SyncLock _keysLock
            If IsKeyPressed("DpadUp") Then buttons = buttons Or &H1
            If IsKeyPressed("DpadDown") Then buttons = buttons Or &H2
            If IsKeyPressed("DpadLeft") Then buttons = buttons Or &H4
            If IsKeyPressed("DpadRight") Then buttons = buttons Or &H8
            If IsKeyPressed("Start") Then buttons = buttons Or &H10
            If IsKeyPressed("Back") Then buttons = buttons Or &H20
            If IsKeyPressed("LThumb") Then buttons = buttons Or &H40
            If IsKeyPressed("RThumb") Then buttons = buttons Or &H80
            If IsKeyPressed("LB") Then buttons = buttons Or &H100
            If IsKeyPressed("RB") Then buttons = buttons Or &H200
            If IsKeyPressed("A") Then buttons = buttons Or &H1000
            If IsKeyPressed("B") Then buttons = buttons Or &H2000
            If IsKeyPressed("X") Then buttons = buttons Or &H4000
            If IsKeyPressed("Y") Then buttons = buttons Or &H8000

            If IsKeyPressed("LTrigger") Then leftTrigger = 255
            If IsKeyPressed("RTrigger") Then rightTrigger = 255

            If IsKeyPressed("LStickUp") Then thumbLY = 32767
            If IsKeyPressed("LStickDown") Then thumbLY = -32768
            If IsKeyPressed("LStickLeft") Then thumbLX = -32768
            If IsKeyPressed("LStickRight") Then thumbLX = 32767

            If IsKeyPressed("RStickUp") Then thumbRY = 32767
            If IsKeyPressed("RStickDown") Then thumbRY = -32768
            If IsKeyPressed("RStickLeft") Then thumbRX = -32768
            If IsKeyPressed("RStickRight") Then thumbRX = 32767
        End SyncLock

        Dim buf(19) As Byte
        buf(0) = CByte(buttons And &HFF)
        buf(1) = CByte((buttons >> 8) And &HFF)
        buf(2) = leftTrigger
        buf(3) = rightTrigger
        BitConverter.GetBytes(thumbLX).CopyTo(buf, 4)
        BitConverter.GetBytes(thumbLY).CopyTo(buf, 6)
        BitConverter.GetBytes(thumbRX).CopyTo(buf, 8)
        BitConverter.GetBytes(thumbRY).CopyTo(buf, 10)
        Return buf
    End Function

    Private Function IsKeyPressed(action As String) As Boolean
        If KeyMapping.ContainsKey(action) Then
            Dim k = KeyMapping(action)
            Return _pressedKeys.Contains(k)
        End If
        Return False
    End Function

    Public Sub [Stop]()
        _running = False
        _udpClient?.Close()
    End Sub

End Class