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
Imports NAudio.Wave
Imports Concentus.Structs

Public Class AudioReceiver
    Private _udpClient As UdpClient
    Private _thread As Thread
    Private _running As Boolean = False
    Private _waveOut As WaveOutEvent
    Private _bufferedProvider As BufferedWaveProvider
    Private _decoder As OpusDecoder

    Private ReadOnly SampleRate As Integer = 48000
    Private ReadOnly Channels As Integer = 2

    Public Sub New(udpClient As UdpClient)
        _udpClient = udpClient  ' 外から受け取る
    End Sub
    Public Sub Start()

        _running = True
        ' Opusデコーダー初期化
        _decoder = New OpusDecoder(SampleRate, Channels)

        Dim fmt = New WaveFormat(SampleRate, 16, Channels)
        _bufferedProvider = New BufferedWaveProvider(fmt)
        _bufferedProvider.BufferDuration = TimeSpan.FromMilliseconds(100)
        _bufferedProvider.DiscardOnBufferOverflow = True

        _waveOut = New WaveOutEvent()
        _waveOut.DesiredLatency = 100
        _waveOut.NumberOfBuffers = 3
        _waveOut.Init(_bufferedProvider)
        _waveOut.Play()

        '_udpClient = New UdpClient(5005)
        _running = True

        _thread = New Thread(AddressOf ReceiveLoop)
        _thread.IsBackground = True
        _thread.Priority = ThreadPriority.AboveNormal
        _thread.Start()
    End Sub

    Private Sub ReceiveLoop()
        Dim ep As New IPEndPoint(IPAddress.Any, 0)
        Dim lastTimestamp As UInteger = UInteger.MaxValue

        Do While _running
            Try
                Dim data() As Byte = _udpClient.Receive(ep)
                If data.Length <= 4 Then Continue Do

                ' ビッグエンディアンで読む
                Dim ts As UInteger = (CUInt(data(0)) << 24) Or
                                 (CUInt(data(1)) << 16) Or
                                 (CUInt(data(2)) << 8) Or
                                  CUInt(data(3))

                If lastTimestamp <> UInteger.MaxValue Then
                    Dim diff = CInt(ts) - CInt(lastTimestamp)
                    If diff <> 960 Then
                        Debug.WriteLine($"[Audio] 異常: ts差={diff} (期待値=960)")
                    End If
                End If
                lastTimestamp = ts

                Dim pcmBuf(960 * Channels - 1) As Short
#Disable Warning BC40000
                Dim decoded = _decoder.Decode(data, 4, data.Length - 4,
                                          pcmBuf, 0, 960, False)
#Enable Warning BC40000
                If decoded > 0 Then
                    Dim bytes(decoded * Channels * 2 - 1) As Byte
                    Buffer.BlockCopy(pcmBuf, 0, bytes, 0, bytes.Length)
                    'Debug.WriteLine($"[Audio] buffer={_bufferedProvider.BufferedDuration.TotalMilliseconds:F0}ms")
                    _bufferedProvider.AddSamples(bytes, 0, bytes.Length)
                End If
            Catch ex As Exception
                If _running Then Debug.WriteLine("[AudioUDP] Error: " & ex.Message)
            End Try
        Loop
    End Sub

    Public Sub SendDummy(ip As String)
        Try
            Dim ep As New IPEndPoint(IPAddress.Parse(ip), 55003)
            _udpClient.Send(New Byte() {0}, 1, ep)
            Debug.WriteLine("[NAT] Dummy sent to " & ip & ":55003")
        Catch ex As Exception
            Debug.WriteLine("[NAT] SendDummy error: " & ex.Message)
        End Try
    End Sub
    Public Sub [Stop]()
        _running = False
        _waveOut?.Stop()
        _waveOut?.Dispose()
        Try : _udpClient?.Close() : Catch : End Try
    End Sub
End Class