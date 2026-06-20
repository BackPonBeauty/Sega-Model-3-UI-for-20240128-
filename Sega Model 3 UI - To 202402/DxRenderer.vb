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

Imports SharpDX
Imports SharpDX.Direct2D1
Imports SharpDX.DXGI
Imports SharpDX.Mathematics.Interop
Imports System.Runtime.InteropServices

Public Class DxRenderer
    Implements IDisposable

    Private _factory = New SharpDX.Direct2D1.Factory(FactoryType.MultiThreaded)
    Private _renderTarget As WindowRenderTarget
    Private _bitmap As SharpDX.Direct2D1.Bitmap

    Private ReadOnly _width As Integer
    Private ReadOnly _height As Integer
    Private ReadOnly _handle As IntPtr

    Public Sub New(hwnd As IntPtr, w As Integer, h As Integer)
        _handle = hwnd
        _width = w
        _height = h

        _factory = New SharpDX.Direct2D1.Factory(FactoryType.SingleThreaded)

        Dim rtp = New HwndRenderTargetProperties()
        rtp.Hwnd = hwnd
        rtp.PixelSize = New Size2(_width, _height)
        rtp.PresentOptions = PresentOptions.Immediately  ' VSyncなし

        Dim rp = New RenderTargetProperties()
        rp.PixelFormat = New PixelFormat(Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Ignore)

        _renderTarget = New WindowRenderTarget(_factory, rp, rtp)

        Dim bp = New BitmapProperties()
        bp.PixelFormat = New PixelFormat(Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Ignore)
        _bitmap = New SharpDX.Direct2D1.Bitmap(_renderTarget,
                                               New Size2(_width, _height), bp)
    End Sub

    ''' <summary>RGBAフレームをBitmapに転送して描画</summary>
    Public Sub DrawFrame(bgraData() As Byte, Optional mode As Integer = 0)
        Dim stride = _width * 4
        _bitmap.CopyFromMemory(bgraData, stride)

        Dim rtSize = _renderTarget.Size

        _renderTarget.BeginDraw()
        _renderTarget.Clear(New RawColor4(0, 0, 0, 1))

        Dim srcRect As RawRectangleF
        Select Case mode
            Case 1 ' Top-Left
                srcRect = New RawRectangleF(0, 0, _width / 2.0F, _height / 2.0F)
            Case 2 ' Top-Right
                srcRect = New RawRectangleF(_width / 2.0F, 0, _width, _height / 2.0F)
            Case 3 ' Bottom-Left
                srcRect = New RawRectangleF(0, _height / 2.0F, _width / 2.0F, _height)
            Case 4 ' Bottom-Right
                srcRect = New RawRectangleF(_width / 2.0F, _height / 2.0F, _width, _height)
            Case Else ' Normal
                srcRect = New RawRectangleF(0, 0, _width, _height)
        End Select

        _renderTarget.DrawBitmap(_bitmap,
                             New RawRectangleF(0, 0, rtSize.Width, rtSize.Height),
                             1.0F,
                             BitmapInterpolationMode.Linear,
                             srcRect)
        _renderTarget.EndDraw()
    End Sub

    Private Sub SwapRGBAtoBGRA(data() As Byte)
        Dim i As Integer = 0
        Do While i < data.Length
            Dim r = data(i)
            data(i) = data(i + 2)   ' R ← B
            data(i + 2) = r          ' B ← R
            i += 4
        Loop
    End Sub



    Public Sub Dispose() Implements IDisposable.Dispose
        _bitmap?.Dispose()
        _renderTarget?.Dispose()
        _factory?.Dispose()
    End Sub
End Class
