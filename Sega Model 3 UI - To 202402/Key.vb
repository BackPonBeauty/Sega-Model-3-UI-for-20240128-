Imports System.Runtime.InteropServices
Public Delegate Function CallBack(ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer

Public Class Key

    Const WM_KEYDOWN As Integer = &H100
    Const WM_KEYUP As Integer = &H101

    Public ReadOnly Property Hooked As Boolean
        Get
            Return If(hHook = 0, False, True)
        End Get
    End Property


    Public Function MouseHookStart() As Boolean
        If hHook.Equals(0) Then
            hookproc = AddressOf KeybordHookProc
            hHook = SetWindowsHookEx(WH_KEYBOARD_LL, hookproc, GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function MouseHookEnd() As Boolean
        If hHook.Equals(0) Then
            'マウスフックが開始されていない
            Return False
        Else
            'マウスフックを終了する
            Dim ret As Boolean = UnhookWindowsHookEx(hHook)

            If ret.Equals(False) Then
                Return False
            Else
                hHook = 0
                Return True
            End If
        End If

    End Function

    Dim WH_KEYBOARD_LL As Integer = 13
    Shared hHook As Integer = 0

    Private hookproc As CallBack

    Public Delegate Function CallBack(
        ByVal nCode As Integer,
        ByVal wParam As IntPtr,
        ByVal lParam As IntPtr) As Integer

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function SetWindowsHookEx _
          (ByVal idHook As Integer, ByVal HookProc As CallBack,
    ByVal hInstance As IntPtr, ByVal wParam As Integer) As Integer
    End Function

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function GetModuleHandle _
    (ByVal lpModuleName As String) As IntPtr
    End Function

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function CallNextHookEx _
          (ByVal idHook As Integer, ByVal nCode As Integer,
    ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
    End Function

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function UnhookWindowsHookEx _
    (ByVal idHook As Integer) As Boolean
    End Function

    <StructLayout(LayoutKind.Sequential)> Public Structure KeyboardLLHookStruct
        Public vkCode As Integer
        Public scanCode As Integer
        Public flags As Integer
        Public time As Integer
        Public dwExtraInfo As Integer
    End Structure


    Public Function KeybordHookProc(
        ByVal nCode As Integer,
        ByVal wParam As IntPtr,
        ByVal lParam As IntPtr) As Integer

        If (nCode < 0) Then
            Return CallNextHookEx(hHook, nCode, wParam, lParam)
        End If

        Dim hookStruct As New KeyboardLLHookStruct()
        hookStruct = CType(Marshal.PtrToStructure(lParam, hookStruct.GetType()), KeyboardLLHookStruct)

        If wParam = New IntPtr(WM_KEYDOWN) Then
            Dim e As New KeyBoardHookerEventArgs
            e.vkCode = hookStruct.vkCode
            RaiseEvent KeyDown1(Me, e)
            Return 0
        End If

        If wParam = New IntPtr(WM_KEYUP) Then
            Dim e As New KeyBoardHookerEventArgs
            e.vkCode = hookStruct.vkCode
            RaiseEvent KeyUp1(Me, e)
            Return 0
        End If

        Return CallNextHookEx(hHook, nCode, wParam, lParam)
    End Function

    Public Event KeyDown1(ByVal sender As Object, ByVal EventArgs As KeyBoardHookerEventArgs)
    Public Event KeyUp1(ByVal sender As Object, ByVal EventArgs As KeyBoardHookerEventArgs)

    Public Sub Dispose1()
        Dim ret As Boolean = UnhookWindowsHookEx(hHook)
        If ret.Equals(False) Then
        End If
    End Sub

    Private Sub InitializeComponents()
        Me.SuspendLayout()
        '
        'Key
        '
        Me.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.Name = "Key"
        Me.ResumeLayout(False)

    End Sub
End Class


Public Class KeyBoardHookerEventArgs
    Inherits EventArgs

    Dim _vkCode As Integer

    Public Property vkCode() As Integer
        Get
            Return _vkCode
        End Get
        Set(ByVal value As Integer)
            _vkCode = value
        End Set
    End Property

End Class

Public Class MouseHookClass

    Dim WH_MOUSE_LL As Integer = 14
    Shared hHook As Integer = 0

    Private hookproc As CallBack


    Public Delegate Function CallBack(
        ByVal nCode As Integer,
        ByVal wParam As IntPtr,
        ByVal lParam As IntPtr) As Integer

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function GetModuleHandle(lpModuleName As String) As IntPtr
    End Function

    'Import for the SetWindowsHookEx function.
    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function SetWindowsHookEx(ByVal idHook As Integer, ByVal HookProc As CallBack, ByVal hInstance As IntPtr, ByVal wParam As Integer) As Integer
    End Function

    'Import for the CallNextHookEx function.
    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function CallNextHookEx(ByVal idHook As Integer, ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
    End Function
    'Import for the UnhookWindowsHookEx function.
    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function UnhookWindowsHookEx(ByVal idHook As Integer) As Boolean
    End Function




    'Point structure declaration.
    <StructLayout(LayoutKind.Sequential)> Public Structure Point
        Public x As Integer
        Public y As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Class MouseLLHookStruct
        Public pt As Point
        Public mouseData As Integer
        Public flags As Integer
        Public time As Integer
        Public dwExtraInfo As Integer
    End Class

    'マウス操作の種類を表す。
    Public Enum MouseMessage
        'マウスカーソルが移動した。
        Move = &H200
        '左ボタンが押された。
        LDown = &H201
        '左ボタンが解放された。
        LUp = &H202
        '右ボタンが押された。
        RDown = &H204
        '左ボタンが解放された。
        RUp = &H205
        '中ボタンが押された。
        MDown = &H207
        '中ボタンが解放された。
        MUp = &H208
        'ホイールが回転した。
        Wheel = &H20A
        'Xボタンが押された。
        XDown = &H20B
        'Xボタンが解放された。
        XUp = &H20C
    End Enum


    Public Event MouseHook(sender As Object, e As MouseHookEventArgs)
    Public Class MouseHookEventArgs
        Inherits EventArgs

        Private _mousestatus As MouseLLHookStruct
        Private _mousemessage As MouseMessage
        Public Sub New(mousemessage As MouseMessage, mousestatus As MouseLLHookStruct)
            _mousemessage = mousemessage
            _mousestatus = mousestatus
        End Sub

        ''' <summary>
        ''' マウスカーソルの位置（スクリーン座標）
        ''' </summary>
        Public ReadOnly Property Point As Point
            Get
                Return _mousestatus.pt
            End Get
        End Property

        ''' <summary>
        ''' マウスの状態
        ''' </summary>
        Public ReadOnly Property Message As MouseMessage
            Get
                Return _mousemessage
            End Get
        End Property
    End Class


    ''' <summary>
    ''' 現在マウスをフックしているか返す
    ''' </summary>
    ''' <returns>False:フックしていない  True:フックしている</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Hooked As Boolean
        Get
            Return If(hHook = 0, False, True)
        End Get
    End Property

    ''' <summary>
    ''' マウスフックを開始する
    ''' </summary>
    ''' <returns>False:フックに失敗もしくはフック済み True:フックに成功</returns>
    ''' <remarks></remarks>
    Public Function MouseHookStart() As Boolean
        If hHook.Equals(0) Then
            'マウスフックを開始する
            hookproc = AddressOf MouseLLHookProc
            hHook = SetWindowsHookEx(WH_MOUSE_LL, hookproc, GetModuleHandle(IntPtr.Zero), 0)
            If hHook.Equals(0) Then
                Return False
            Else
                Return True
            End If
        Else
            'マウスフックがすでに開始されている
            Return False
        End If

    End Function

    ''' <summary>
    ''' マウスフックを終了する
    ''' </summary>
    ''' <returns>False:フック解除に失敗もしくはフックしていない True:フック解除に成功</returns>
    ''' <remarks></remarks>
    Public Function MouseHookEnd() As Boolean
        If hHook.Equals(0) Then
            'マウスフックが開始されていない
            Return False
        Else
            'マウスフックを終了する
            Dim ret As Boolean = UnhookWindowsHookEx(hHook)

            If ret.Equals(False) Then
                Return False
            Else
                hHook = 0
                Return True
            End If
        End If

    End Function

    Private Function MouseLLHookProc(ByVal nCode As Integer, ByVal wParam As MouseMessage, ByVal lParam As IntPtr) As Integer
        Dim MyMouseHookStruct As New MouseLLHookStruct()

        If nCode = 0 Then
            MyMouseHookStruct = CType(Marshal.PtrToStructure(lParam, MyMouseHookStruct.GetType()), MouseLLHookStruct)
            'イベントを発生させる
            RaiseEvent MouseHook(Nothing, New MouseHookEventArgs(wParam, MyMouseHookStruct))
        End If

        Return CallNextHookEx(hHook, nCode, wParam, lParam)
    End Function

End Class