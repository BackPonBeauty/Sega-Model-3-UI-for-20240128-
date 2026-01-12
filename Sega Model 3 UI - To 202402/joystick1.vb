Imports Sega_Model_3_UI_for_20240128.Form1
Imports SharpDX.DirectInput
Imports System.Runtime.InteropServices
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Tab

Public Class joystick1

    Private directInput As DirectInput
    Private joysticks As New List(Of Joystick)
    Private timer As Timer


    <StructLayout(LayoutKind.Sequential)>
    Private Structure XINPUT_STATE
        Public dwPacketNumber As UInteger
        Public Gamepad As XINPUT_GAMEPAD
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Private Structure XINPUT_GAMEPAD
        Public wButtons As UShort
        Public bLeftTrigger As Byte
        Public bRightTrigger As Byte
        Public sThumbLX As Short
        Public sThumbLY As Short
        Public sThumbRX As Short
        Public sThumbRY As Short
    End Structure

    <DllImport("xinput1_4.dll", EntryPoint:="XInputGetState")>
    Private Shared Function XInputGetState(dwUserIndex As Integer, ByRef pState As XINPUT_STATE) As Integer
    End Function

    Private previousButtons(3) As UShort

    Private Sub joystick_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        directInput = New DirectInput()

        ' ゲームパッド/ジョイスティックを列挙（最大4台）
        Dim devices = directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AttachedOnly).ToList()
        devices.AddRange(directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AttachedOnly))

        For i = 0 To Math.Min(3, devices.Count - 1)
            Dim js = New Joystick(directInput, devices(i).InstanceGuid)
            js.Acquire()
            joysticks.Add(js)
            Console.WriteLine($"Controller {i} connected: {devices(i).InstanceName}")
        Next

        Timer2.Start()
    End Sub

    Private Sub joystick_Close(sender As Object, e As EventArgs) Handles MyBase.Closed
        Timer1.Enabled = False
        Timer2.Enabled = False
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        For i = 0 To joysticks.Count - 1
            Dim state = joysticks(i).GetCurrentState()

            ' ボタン
            Dim buttons = state.Buttons
            For b = 0 To buttons.Length - 1
                If buttons(b) Then
                    Console.WriteLine($"Controller {i} Button {b} pressed")
                End If
            Next

            ' アナログ軸（0～65535 の範囲で返る）
            Dim lx As Double = NormalizeAxis(state.X)
            Dim ly As Double = NormalizeAxis(state.Y)
            Dim rx As Double = NormalizeAxis(state.RotationX)
            Dim ry As Double = NormalizeAxis(state.RotationY)

            If Math.Abs(lx) > 0.2 OrElse Math.Abs(ly) > 0.2 Then
                Console.WriteLine($"Controller {i} LeftStick: X={lx:F2}, Y={ly:F2}")
            End If
            If Math.Abs(rx) > 0.2 OrElse Math.Abs(ry) > 0.2 Then
                Console.WriteLine($"Controller {i} RightStick: X={rx:F2}, Y={ry:F2}")
            End If

            ' POV (十字キー)
            If state.PointOfViewControllers.Length > 0 Then
                Dim pov = state.PointOfViewControllers(0)
                If pov <> -1 Then
                    Console.WriteLine($"Controller {i} POV: {pov}")
                End If
            End If
        Next
    End Sub

    ' 0～65535 を -1.0～1.0 に正規化
    Private Function NormalizeAxis(value As Integer) As Double
        Return (value - 32767.5) / 32767.5
    End Function


    Dim LastButton = ""
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        For i As Integer = 0 To 3 ' 最大4台
            Dim state As New XINPUT_STATE()
            If XInputGetState(i, state) = 0 Then ' 接続あり

                ' ===== ボタン =====
                Dim buttons As UShort = state.Gamepad.wButtons
                If buttons <> 0 Then
                    If LastButton <> ButtonToString(buttons) Then
                        Console.WriteLine($"JOY{i}_Buttons: {ButtonToString(buttons)}")
                        LastButton = ButtonToString(buttons)
                    End If
                End If

                ' ===== アナログ入力 =====
                Dim lt As Byte = state.Gamepad.bLeftTrigger
                Dim rt As Byte = state.Gamepad.bRightTrigger
                Dim lx As Short = state.Gamepad.sThumbLX
                Dim ly As Short = state.Gamepad.sThumbLY
                Dim rx As Short = state.Gamepad.sThumbRX
                Dim ry As Short = state.Gamepad.sThumbRY

                ' トリガー（0～255）
                If lt > 10 Then Console.WriteLine($"JOY{i + 1}_ZAXIS_POS")
                If rt > 10 Then Console.WriteLine($"JOY{i + 1}_RZAXIS_POS")

                Dim lxNorm As String = ""
                Dim lyNorm As String = ""
                Dim rxNorm As String = ""
                Dim ryNorm As String = ""

                'Console.WriteLine($"Controller {i}  LeftStick: X={lx:F3}")
                'ly = ApplyDeadZone(ly, 0.2)
                'rx = ApplyDeadZone(rx, 0.2)
                'ry = ApplyDeadZone(ry, 0.2)

                If lx <> 0 And lx > 10000 Or lx < -10000 Then
                    If lx < -10000 Then
                        lxNorm = "NEG"
                    ElseIf lx > 10000 Then
                        lxNorm = "POS"
                    End If
                    If LastButton <> ($"JOY{i + 1}_XAXIS_{lxNorm}") Then
                        Console.WriteLine($"JOY{i + 1}_XAXIS_{lxNorm}")
                        LastButton = ($"JOY{i + 1}_XAXIS_{lxNorm}")
                    End If
                End If
                If ly <> 0 And ly > 10000 Or ly < -10000 Then
                    If ly < -10000 Then
                        lyNorm = "NEG"
                    ElseIf ly > 10000 Then
                        lyNorm = "POS"
                    End If
                    If LastButton <> ($"JOY{i + 1 }_YAXIS_{lyNorm}") Then
                        Console.WriteLine($"JOY{i + 1 }_YAXIS_{lyNorm}")
                        LastButton = ($"JOY{i + 1 }_YAXIS_{lyNorm}")
                    End If
                End If
                If rx <> 0 And rx > 10000 Or rx < -10000 Then
                    If rx < -10000 Then
                        rxNorm = "NEG"
                    ElseIf rx > 10000 Then
                        rxNorm = "POS"
                    End If
                    If LastButton <> ($"JOY{i + 1 }_RXAXIS_{rxNorm}") Then
                        Console.WriteLine($"JOY{i + 1 }_RXAXIS_{rxNorm}")
                        LastButton = ($"JOY{i + 1 }_RXAXIS_{rxNorm}")
                    End If
                End If
                If ry <> 0 And ry > 10000 Or ry < -10000 Then
                    If ry < -10000 Then
                        ryNorm = "NEG"
                    ElseIf ry > 10000 Then
                        ryNorm = "POS"
                    End If
                    If LastButton <> ($"JOY{i + 1 }_RYAXIS_{ryNorm}") Then
                        Console.WriteLine($"JOY{i + 1 }_RYAXIS_{ryNorm}")
                        LastButton = ($"JOY{i + 1 }_RYAXIS_{ryNorm}")
                    End If
                End If
            End If
        Next
    End Sub

    Private Function ApplyDeadZone(value As Double, threshold As Double) As Double
        If Math.Abs(value) < threshold Then
            Return 0
        End If
        Return value
    End Function

    Private Function ButtonToString(buttons As UShort) As String
        Dim names As New List(Of String)()
        If (buttons And &H1000) <> 0 Then
            names.Add("A")
        End If
        If (buttons And &H2000) <> 0 Then
            names.Add("B")
        End If
        If (buttons And &H4000) <> 0 Then
            names.Add("X")
        End If
        If (buttons And &H8000) <> 0 Then
            names.Add("Y")
        End If
        If (buttons And &H1) <> 0 Then
            names.Add("DPAD_UP")
        End If
        If (buttons And &H2) <> 0 Then
            names.Add("DPAD_DOWN")
        End If
        If (buttons And &H4) <> 0 Then
            names.Add("DPAD_LEFT")
        End If
        If (buttons And &H8) <> 0 Then
            names.Add("DPAD_RIGHT")
        End If
        If (buttons And &H10) <> 0 Then
            names.Add("START")
        End If
        If (buttons And &H20) <> 0 Then
            names.Add("BACK")
        End If
        If (buttons And &H40) <> 0 Then
            names.Add("LEFT_THUMB")
        End If
        If (buttons And &H80) <> 0 Then
            names.Add("RIGHT_THUMB")
        End If
        If (buttons And &H100) <> 0 Then
            names.Add("LEFT_SHOULDER")
        End If
        If (buttons And &H200) <> 0 Then
            names.Add("RIGHT_SHOULDER")
        End If
        Return String.Join(",", names)
    End Function



End Class
