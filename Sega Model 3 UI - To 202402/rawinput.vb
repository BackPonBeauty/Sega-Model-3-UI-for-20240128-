Imports System.Runtime.InteropServices
Imports System.Text

Module RawInput
    <StructLayout(LayoutKind.Sequential)>
    Public Structure RAWINPUTHEADER
        Public dwType As UInteger
        Public dwSize As UInteger
        Public hDevice As IntPtr
        Public wParam As IntPtr
    End Structure

    <StructLayout(LayoutKind.Explicit)>
    Public Structure RAWMOUSE
        <FieldOffset(0)> Public usFlags As UShort
        <FieldOffset(2)> Public usButtonFlags As UShort
        <FieldOffset(4)> Public usButtonData As UShort
        <FieldOffset(6)> Public ulRawButtons As UInteger
        <FieldOffset(10)> Public lLastX As Integer
        <FieldOffset(14)> Public lLastY As Integer
        <FieldOffset(18)> Public ulExtraInformation As UInteger
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure RAWINPUT
        Public header As RAWINPUTHEADER
        Public mouse As RAWMOUSE
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure RAWINPUTDEVICELIST
        Public hDevice As IntPtr
        Public dwType As UInteger
    End Structure

    <DllImport("user32.dll", SetLastError:=True)>
    Public Function GetRawInputDeviceList(ByVal pRawInputDeviceList As IntPtr, ByRef puiNumDevices As UInteger, ByVal cbSize As UInteger) As UInteger
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Function GetRawInputDeviceInfo(ByVal hDevice As IntPtr, ByVal uiCommand As UInteger, ByVal pData As IntPtr, ByRef pcbSize As UInteger) As UInteger
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Function GetRawInputDeviceInfo(ByVal hDevice As IntPtr, ByVal uiCommand As UInteger, ByVal pData As StringBuilder, ByRef pcbSize As UInteger) As UInteger
    End Function

    <DllImport("user32.dll")>
    Public Function GetRawInputData(ByVal hRawInput As IntPtr, ByVal uiCommand As UInteger, ByVal pData As IntPtr, ByRef pcbSize As UInteger, ByVal cbSizeHeader As UInteger) As UInteger
    End Function

    Public Const RIM_TYPEMOUSE As UInteger = 0
    Public Const RIDI_DEVICENAME As UInteger = &H20000007
    Public Const RID_INPUT As UInteger = &H10000003
    Public Const RIDEV_INPUTSINK As UInteger = &H100 ' This is the missing constant

    Public Const RI_MOUSE_LEFT_BUTTON_DOWN As UShort = &H1
    Public Const RI_MOUSE_LEFT_BUTTON_UP As UShort = &H2
    Public Const RI_MOUSE_RIGHT_BUTTON_DOWN As UShort = &H4
    Public Const RI_MOUSE_RIGHT_BUTTON_UP As UShort = &H8
    Public Const RI_MOUSE_MIDDLE_BUTTON_DOWN As UShort = &H10
    Public Const RI_MOUSE_MIDDLE_BUTTON_UP As UShort = &H20
    Public Const RI_MOUSE_BUTTON_4_DOWN As UShort = &H40
    Public Const RI_MOUSE_BUTTON_4_UP As UShort = &H80
    Public Const RI_MOUSE_BUTTON_5_DOWN As UShort = &H100
    Public Const RI_MOUSE_BUTTON_5_UP As UShort = &H200
End Module
