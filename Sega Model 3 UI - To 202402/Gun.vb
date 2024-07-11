Imports System.Runtime.InteropServices
Imports System.Text
Imports Sega_Model_3_UI_for_20240128.Form1

Public Class Gun

    Private mouseDevices As New Dictionary(Of IntPtr, String)
    Dim RawInput_Enabled As Boolean = True
    Dim P1 As Boolean = False
    Dim P2 As Boolean = False
    Private mysound As String = "mysound"
    Private theme As String = "theme"

    <System.Runtime.InteropServices.DllImport("winmm.dll", CharSet:=System.Runtime.InteropServices.CharSet.Auto)>
    Private Shared Function mciSendString(ByVal command As String,
    ByVal buffer As System.Text.StringBuilder,
    ByVal bufferSize As Integer, ByVal hwndCallback As IntPtr) As Integer
    End Function

    Private aliasName As String = "MediaFile"

    <DllImport("user32.dll", SetLastError:=True)>
    Public Shared Function RegisterRawInputDevices(ByVal pRawInputDevices() As RawInputDevice, ByVal uiNumDevices As UInteger, ByVal cbSize As UInteger) As Boolean
    End Function

    <StructLayout(LayoutKind.Sequential)>
    Public Structure RawInputDevice
        Public usUsagePage As UShort
        Public usUsage As UShort
        Public dwFlags As UInteger
        Public hwndTarget As IntPtr
    End Structure
    Private Sub Gun_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackColor = Color.FromArgb(255, Form1.Bgcolor_R, Form1.Bgcolor_G, Form1.Bgcolor_B)
        Me.ForeColor = Form1.Pub_Forecolor_s
        GetAllControls(Me, Integer.Parse(Form1.FontSize_bin))
        RegisterRawInputDevices()
        InitializeMouseDevices()
        Button1.Enabled = False
        'Button1.BackColor = Color.Gray
    End Sub

    Private Sub GetAllControls(ByVal control As Control, size As Integer)
        If control.HasChildren Then
            For Each childControl As Control In control.Controls
                GetAllControls(childControl, size)
                'テキストボックスだけにアクセスしたい場合
                ' If TypeOf childControl Is TextBox Then
                childControl.Font = New Font("Arial", size, FontStyle.Regular)
                If TypeOf childControl Is ButtonBase Then
                    childControl.ForeColor = Form1.Pub_Forecolor_s
                End If
            Next childControl
        End If

    End Sub
    Private Sub InitializeMouseDevices()
        Dim deviceCount As UInteger = 0
        Dim deviceListSize As UInteger = CUInt(Marshal.SizeOf(GetType(RawInput.RAWINPUTDEVICELIST)))

        ' 最初にデバイスの数を取得
        RawInput.GetRawInputDeviceList(IntPtr.Zero, deviceCount, deviceListSize)
        Console.WriteLine($"Number of devices: {deviceCount}")
        ' デバイスリストのバッファを作成
        Dim pRawInputDeviceList As IntPtr = Marshal.AllocHGlobal(CInt(deviceCount) * CInt(deviceListSize))

        ' デバイスリストを取得
        RawInput.GetRawInputDeviceList(pRawInputDeviceList, deviceCount, deviceListSize)

        Dim mouseIndex As Integer = 1

        For i = deviceCount - 1 To 1 Step -1
            Dim rid As RawInput.RAWINPUTDEVICELIST = CType(Marshal.PtrToStructure(New IntPtr(pRawInputDeviceList.ToInt64() + (i * deviceListSize)), GetType(RawInput.RAWINPUTDEVICELIST)), RawInput.RAWINPUTDEVICELIST)

            ' マウスデバイスのみ処理
            If rid.dwType = RawInput.RIM_TYPEMOUSE Then
                Dim size As UInteger = 0
                RawInput.GetRawInputDeviceInfo(rid.hDevice, RawInput.RIDI_DEVICENAME, IntPtr.Zero, size)
                If size <> 0 Then
                    Dim nameBuilder As New StringBuilder(CInt(size))
                    RawInput.GetRawInputDeviceInfo(rid.hDevice, RawInput.RIDI_DEVICENAME, nameBuilder, size)
                    Dim deviceName As String = nameBuilder.ToString()
                    mouseDevices(rid.hDevice) = $"MOUSE{mouseIndex}" '{deviceName}
                    Console.WriteLine(deviceName)
                    mouseIndex += 1
                End If
            End If
        Next

        Marshal.FreeHGlobal(pRawInputDeviceList)
    End Sub

    Private Sub RegisterRawInputDevices()
        Dim rid As New RawInputDevice()
        rid.usUsagePage = &H1
        rid.usUsage = &H2
        rid.dwFlags = RawInput.RIDEV_INPUTSINK
        rid.hwndTarget = Me.Handle

        If Not RegisterRawInputDevices(New RawInputDevice() {rid}, 1, CUInt(Marshal.SizeOf(rid))) Then
            Throw New ApplicationException("Failed to register raw input devices.")
            Console.WriteLine("Failed to register raw input devices.")
        Else
            Console.WriteLine("Success to register raw input devices.")
        End If
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Const WM_INPUT As Integer = &HFF

        If m.Msg = WM_INPUT Then
            Dim dwSize As UInteger = 0
            RawInput.GetRawInputData(m.LParam, RawInput.RID_INPUT, IntPtr.Zero, dwSize, CUInt(Marshal.SizeOf(GetType(RawInput.RAWINPUTHEADER))))

            If dwSize > 0 Then
                Dim buffer As IntPtr = Marshal.AllocHGlobal(CInt(dwSize))
                Try
                    If RawInput.GetRawInputData(m.LParam, RawInput.RID_INPUT, buffer, dwSize, CUInt(Marshal.SizeOf(GetType(RawInput.RAWINPUTHEADER)))) = dwSize Then
                        Dim raw As RawInput.RAWINPUT = Marshal.PtrToStructure(Of RawInput.RAWINPUT)(buffer)

                        If raw.header.dwType = RawInput.RIM_TYPEMOUSE Then
                            Dim mouse As RawInput.RAWMOUSE = raw.mouse
                            Dim deviceName As String = If(mouseDevices.ContainsKey(raw.header.hDevice), mouseDevices(raw.header.hDevice), "Unknown Mouse")

                            'Dim data1 As String = ""
                            'If mouse.usButtonData = 1 Or mouse.usButtonData = 2 Then
                            '    data1 = "LEFT_BUTTON"
                            'End If
                            'If mouse.usButtonData = 4 Or mouse.usButtonData = 8 Then
                            '    data1 = "RIGHT_BUTTON"
                            'End If
                            'If mouse.usButtonData = 16 Or mouse.usButtonData = 32 Then
                            '    data1 = "MIDDLE_BUTTON"
                            'End If
                            'If mouse.usButtonData = 7865344 Then
                            '    data1 = "ZAXIS_POS"
                            'End If
                            'If mouse.usButtonData = -7863296 Then
                            '    data1 = "ZAXIS_NEG"
                            'End If
                            'Dim deltaX As Integer = mouse.lLastX
                            'Dim deltaY As Integer = mouse.lLastY

                            'If deltaY > 0 Then
                            '    data1 = "YAXIS_POS"
                            'End If
                            'If deltaY < 0 Then
                            '    data1 = "YAXIS_NEG"
                            'End If
                            'If deltaX > 0 Then
                            '    data1 = "XAXIS_POS"
                            'End If
                            'If deltaX < 0 Then
                            '    data1 = "XAXIS_NEG"
                            'End If


                            'If (mouse.usButtonFlags And RawInput.RI_MOUSE_LEFT_BUTTON_UP) <> 0 Then
                            '    Console.WriteLine($"{deviceName}: Left button up")
                            'End If
                            'If (mouse.usButtonFlags And RawInput.RI_MOUSE_RIGHT_BUTTON_DOWN) <> 0 Then
                            '    Console.WriteLine($"{deviceName}: Right button down")
                            'End If
                            'If (mouse.usButtonFlags And RawInput.RI_MOUSE_RIGHT_BUTTON_UP) <> 0 Then
                            '    Console.WriteLine($"{deviceName}: Right button up")
                            'End If
                            'If (mouse.usButtonFlags And RawInput.RI_MOUSE_MIDDLE_BUTTON_DOWN) <> 0 Then
                            '    Console.WriteLine($"{deviceName}: Middle button down")
                            'End If
                            'If (mouse.usButtonFlags And RawInput.RI_MOUSE_MIDDLE_BUTTON_UP) <> 0 Then
                            '    Console.WriteLine($"{deviceName}: Middle button up")
                            'End If
                            'If (mouse.usButtonFlags And RawInput.RI_MOUSE_BUTTON_4_DOWN) <> 0 Then
                            '    Console.WriteLine($"{deviceName}: Button 4 down")
                            'End If
                            'If (mouse.usButtonFlags And RawInput.RI_MOUSE_BUTTON_4_UP) <> 0 Then
                            '    Console.WriteLine($"{deviceName}: Button 4 up")
                            'End If
                            'If (mouse.usButtonFlags And RawInput.RI_MOUSE_BUTTON_5_DOWN) <> 0 Then
                            '    Console.WriteLine($"{deviceName}: Button 5 down")
                            'End If
                            'If (mouse.usButtonFlags And RawInput.RI_MOUSE_BUTTON_5_UP) <> 0 Then
                            '    Console.WriteLine($"{deviceName}: Button 5 up")
                            'End If

                            ' マウスの移動量を取得する場合

                            'Dim wheelDelta As Short = BitConverter.ToInt16(BitConverter.GetBytes(mouse.usButtonData), 0)

                            'If wheelDelta > 0 Then
                            'Console.WriteLine($"{deviceName}: Wheel  {wheelDelta}")
                            'ElseIf wheelDelta < 0 Then
                            '    Console.WriteLine($"{deviceName}: Wheel down {wheelDelta}")
                            'End If
                            If RawInput_Enabled = True Then
                                Label4.Text = ($" {deviceName}")
                            End If

                            'Console.WriteLine($"{deviceName} ButtonsState : {mouse.usButtonData} ,Delta X: {deltaX}, Delta Y: {deltaY}" & vbCrLf)
                        End If
                    End If
                Finally
                    Marshal.FreeHGlobal(buffer)
                End Try
            End If
        End If
        MyBase.WndProc(m)
    End Sub

    Private Sub Player1_Click(sender As Object, e As EventArgs) Handles Player1.Click
        Label2.Text = Label4.Text
        Form1.Label39.Text = Label4.Text
        P1 = True
        Check_Select()

        Dim Rnd As String = "bang"
        Dim leng As Integer = 800

        Dim cmd As String
        Dim sond As String = "sound\" & Rnd & ".mp3"
        Dim fileName As String = sond
        cmd = "open """ + fileName + """ type mpegvideo alias " + mysound
        If mciSendString(cmd, Nothing, 0, IntPtr.Zero) <> 0 Then
            Return
        End If
        cmd = "play " + mysound
        mciSendString(cmd, Nothing, 0, IntPtr.Zero)
        themetimer.Interval = leng
        themetimer.Enabled = True
        Player1.BackColor = Color.Red
    End Sub

    Private Sub Player2_Click(sender As Object, e As EventArgs) Handles Player2.Click
        Label3.Text = Label4.Text
        Form1.Label40.Text = Label4.Text
        P2 = True
        Check_Select()

        Dim Rnd As String = "bang2"
        Dim leng As Integer = 800

        Dim cmd As String
        Dim sond As String = "sound\" & Rnd & ".mp3"
        Dim fileName As String = sond
        cmd = "open """ + fileName + """ type mpegvideo alias " + mysound
        If mciSendString(cmd, Nothing, 0, IntPtr.Zero) <> 0 Then
            Return
        End If
        cmd = "play " + mysound
        mciSendString(cmd, Nothing, 0, IntPtr.Zero)
        themetimer.Interval = leng
        themetimer.Enabled = True
        Player2.BackColor = Color.Red
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Check_Select()
        If P1 = True And P2 = True Then
            Button1.Enabled = True
            'Button1.BackColor = Color.White
        End If
    End Sub

    Private Sub Theme_Tick(sender As Object, e As EventArgs) Handles themetimer.Tick
        themetimer.Enabled = False
        'If Tabb = 96 Then
        Dim cmd As String
        '再生しているWAVEを停止する
        cmd = "stop " + mysound
        mciSendString(cmd, Nothing, 0, IntPtr.Zero)
        '  閉じる
        cmd = "close " + mysound
        mciSendString(cmd, Nothing, 0, IntPtr.Zero)
        Player1.BackColor = Color.Silver
        Player2.BackColor = Color.Silver

    End Sub

End Class