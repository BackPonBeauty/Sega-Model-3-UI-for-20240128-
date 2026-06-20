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

Imports System
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Reactive.Linq
Imports System.Reactive
Imports Firebase.Database.Streaming

Public Class VideoForm
    Inherits Form

    Private _ip As String
    Private _w As Integer
    Private _h As Integer
    Private _codec As String
    Private _videoUdp As UdpClient
    Private _audioUdp As UdpClient
    Private _slot As Integer
    Private _hostId As String
    Private _firebase As FirebaseMatchingClient
    Private _discordUsername As String
    Private _isLocalMode As Boolean
    Private _portXInput As Integer
    Private _portHS As Integer
    Private _portVideo As Integer
    Private _portAudio As Integer
    Private _parentForm As Form

    Private _video As VideoReceiver
    Private _audio As AudioReceiver
    Private _renderer As DxRenderer
    Private _renderThread As Thread
    Private _running As Boolean = False
    Private _xinput As XInputSender
    Private _handshakeClient As UdpClient

    Private _renderTimer As System.Windows.Forms.Timer

    Private Enum VideoDisplayMode
        Normal = 0
        TopLeft = 1
        TopRight = 2
        BottomLeft = 3
        BottomRight = 4
    End Enum
    Private _displayMode As VideoDisplayMode = VideoDisplayMode.Normal

    Private pnlVideo As Panel
    Private pnlChatOverlay As Panel
    Private rtbChatLog As TransparentChatLog
    Private txtChatInput As TextBox
    Private _telemetryTimer As System.Windows.Forms.Timer
    Private _chatTimer As System.Windows.Forms.Timer
    Private _chatSubscription As IDisposable = Nothing
    Private _processedChatKeys As New HashSet(Of String)()
    Private _isInitialLoading As Boolean = True
    Private _connectionStartTime As Long = 0

    Public Sub New(ip As String, w As Integer, h As Integer, codec As String,
                   videoUdp As UdpClient, audioUdp As UdpClient, slot As Integer,
                   hostId As String, firebase As FirebaseMatchingClient, discordUser As String,
                   localMode As Boolean, portXInput As Integer, portHS As Integer,
                   portVideo As Integer, portAudio As Integer, handshakeClient As UdpClient, parentForm As Form)

        _ip = ip
        _w = w
        _h = h
        _codec = codec
        _videoUdp = videoUdp
        _audioUdp = audioUdp
        _slot = slot
        _hostId = hostId
        _firebase = firebase
        _discordUsername = discordUser
        _isLocalMode = localMode
        _portXInput = portXInput
        _portHS = portHS
        _portVideo = portVideo
        _portAudio = portAudio
        _handshakeClient = handshakeClient
        _parentForm = parentForm

        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Me.SuspendLayout()
        Me.BackColor = Color.Black
        Me.ClientSize = New Size(_w, _h)
        Me.Font = New Font("Consolas", 9.0!)
        Me.ForeColor = Color.FromArgb(0, 238, 255)
        Me.KeyPreview = True
        Me.Name = "VideoForm"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Text = $"STREAM RECEIVER - Slot P{_slot} [{_w}x{_h} {_codec}]"
        Me.ResumeLayout(False)
    End Sub

    Private Sub VideoForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Create UI components
        pnlVideo = New Panel() With {
            .Location = New Point(0, 0),
            .Size = New Size(_w, _h),
            .BackColor = Color.Black,
            .Dock = DockStyle.Fill
        }
        Me.Controls.Add(pnlVideo)

        pnlChatOverlay = New Panel() With {
            .Size = New Size(300, 110),
            .BackColor = Color.Transparent,
            .Visible = True,
            .Height = 0
        }
        pnlVideo.Controls.Add(pnlChatOverlay)
        pnlChatOverlay.BringToFront()

        rtbChatLog = New TransparentChatLog() With {
            .Dock = DockStyle.Fill,
            .ForeColor = Color.FromArgb(220, 220, 220),
            .Font = New Font("MS Gothic", 9.5F, FontStyle.Bold)
        }
        pnlChatOverlay.Controls.Add(rtbChatLog)

        txtChatInput = New TextBox() With {
            .Dock = DockStyle.Bottom,
            .BackColor = Color.FromArgb(20, 30, 50),
            .ForeColor = Color.White,
            .BorderStyle = BorderStyle.FixedSingle,
            .Font = New Font("MS UI Gothic", 9),
            .Visible = False
        }
        AddHandler txtChatInput.KeyDown, AddressOf txtChatInput_KeyDown
        AddHandler txtChatInput.TextChanged, Sub() ResetChatTimer()
        pnlChatOverlay.Controls.Add(txtChatInput)

        _chatTimer = New System.Windows.Forms.Timer()
        _chatTimer.Interval = 7000
        AddHandler _chatTimer.Tick, AddressOf ChatTimer_Tick

        ' Initialize Receivers & Renderer
        _renderer = New DxRenderer(pnlVideo.Handle, _w, _h)
        _video = New VideoReceiver(_w, _h, _videoUdp, _codec)
        _audio = New AudioReceiver(_audioUdp)

        AddHandler _video.ServerDisconnected, Sub()
                                                  Me.BeginInvoke(Sub()
                                                                     MessageBox.Show("Disconnected by host.", "Disconnected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                                                     Me.Close()
                                                                 End Sub)
                                              End Sub

        _video.Start()
        _audio.Start()
        _running = True

        ' ハートビートスレッド
        Dim hbThread As New Thread(Sub()
                                       Do While _running
                                           Try
                                               Dim hb() As Byte = System.Text.Encoding.ASCII.GetBytes("HB")
                                               _handshakeClient.Send(hb, hb.Length)
                                           Catch ex As Exception
                                               Debug.WriteLine("[HB] Error: " & ex.Message)
                                           End Try
                                           Thread.Sleep(2000)
                                       Loop
                                   End Sub)
        hbThread.IsBackground = True
        hbThread.Start()

        ' ハンドシェイク受信スレッド（KICK対応）
        Dim hsReceiveThread As New Thread(Sub()
                                              Dim epRecv As New IPEndPoint(IPAddress.Any, 0)
                                              _handshakeClient.Client.ReceiveTimeout = 2000
                                              Do While _running
                                                  Try
                                                      Dim data() As Byte = _handshakeClient.Receive(epRecv)
                                                      If data IsNot Nothing AndAlso data.Length > 0 Then
                                                          Dim msgRecv = System.Text.Encoding.ASCII.GetString(data)
                                                          If msgRecv.StartsWith("KICK") Then
                                                              Me.BeginInvoke(Sub()
                                                                                 MessageBox.Show("Kicked by host.", "Kicked", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                                                                 Me.Close()
                                                                             End Sub)
                                                              Return
                                                          End If
                                                      End If
                                                  Catch ex As SocketException
                                                      ' Timeout、継続
                                                  Catch ex As Exception
                                                      Exit Do
                                                  End Try
                                              Loop
                                          End Sub)
        hsReceiveThread.IsBackground = True
        hsReceiveThread.Start()
        '' Render Thread
        '_renderThread = New Thread(AddressOf RenderLoop)
        '_renderThread.IsBackground = True
        '_renderThread.Priority = ThreadPriority.AboveNormal
        '_renderThread.Start()
        _renderTimer = New System.Windows.Forms.Timer()
        _renderTimer.Interval = 1
        AddHandler _renderTimer.Tick, Sub()
                                          If _video IsNot Nothing Then
                                              Dim frame(_video.FrameSize - 1) As Byte
                                              If _video.TryGetFrame(frame) Then
                                                  _renderer?.DrawFrame(frame, CInt(_displayMode))
                                              End If
                                          End If
                                      End Sub
        _renderTimer.Start()
        ' Telemetry Timer
        _telemetryTimer = New System.Windows.Forms.Timer()
        _telemetryTimer.Interval = 1500
        AddHandler _telemetryTimer.Tick, AddressOf TelemetryTimer_Tick
        _telemetryTimer.Start()

        ' Key Event mapping
        XInputSender.LoadConfig()
        _xinput = New XInputSender()
        AddHandler _xinput.ControllerDisconnected, AddressOf OnControllerDisconnected
        _xinput.Start(_ip, _portXInput, SharpDX.XInput.UserIndex.One)

        ' Chat setup
        _processedChatKeys.Clear()
        _isInitialLoading = True
        _connectionStartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        If Not String.IsNullOrEmpty(_hostId) AndAlso _firebase IsNot Nothing Then
            StartChatSubscription()
        End If

        ' Start Delay for chat load
        Task.Run(Async Function()
                     Await Task.Delay(2000)
                     _isInitialLoading = False
                 End Function)

        PositionChatOverlay()
    End Sub





    Private Sub PositionChatOverlay()
        If pnlChatOverlay IsNot Nothing AndAlso pnlVideo IsNot Nothing Then
            pnlChatOverlay.Left = pnlVideo.Width - pnlChatOverlay.Width - 10
            pnlChatOverlay.Top = 10
        End If
    End Sub

    Private Sub VideoForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        PositionChatOverlay()
    End Sub

    Private Sub RenderLoop()
        Do While _running
            If _video IsNot Nothing Then
                Dim frame(_video.FrameSize - 1) As Byte
                If _video.TryGetFrame(frame) Then
                    _renderer?.DrawFrame(frame, CInt(_displayMode))
                Else
                    Thread.Sleep(1)
                End If
            End If
        Loop
    End Sub

    Private Sub TelemetryTimer_Tick(sender As Object, e As EventArgs)
        If _video IsNot Nothing AndAlso _handshakeClient IsNot Nothing Then
            Try
                Dim lossRate As Double = _video.GetAndResetLossRate()
                Dim msg As String = String.Format(System.Globalization.CultureInfo.InvariantCulture, "STAT {0:F4}", lossRate)
                Dim data = System.Text.Encoding.ASCII.GetBytes(msg)
                _handshakeClient.Send(data, data.Length)
            Catch ex As Exception
                Debug.WriteLine("[Telemetry] Send error: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub OnControllerDisconnected()
        Me.BeginInvoke(Sub()
                           MessageBox.Show("Disconnected: no controller detected.", "Disconnected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                           Me.Close()
                       End Sub)
    End Sub

    Private Sub StartChatSubscription()
        _chatSubscription?.Dispose()
        Dim onNext As New Action(Of FirebaseEvent(Of ChatMessage))(
            Sub(chatEvent)
                If chatEvent.Object IsNot Nothing AndAlso
                   chatEvent.EventType = Firebase.Database.Streaming.FirebaseEventType.InsertOrUpdate Then
                    Dim key = chatEvent.Key
                    Dim msg = chatEvent.Object
                    If _processedChatKeys.Contains(key) Then Return
                    _processedChatKeys.Add(key)
                    Dim isNewMessage = Not _isInitialLoading
                    Me.BeginInvoke(Sub()
                                       Dim msgText As String
                                       If String.IsNullOrEmpty(msg.Username) OrElse msg.Username = "SYSTEM" Then
                                           msgText = msg.Message
                                       Else
                                           msgText = $"{msg.Username}: {msg.Message}"
                                       End If
                                       rtbChatLog.AppendChat(msgText)
                                       If isNewMessage Then
                                           pnlChatOverlay.Visible = True
                                           pnlChatOverlay.Height = 110
                                           pnlChatOverlay.BringToFront()
                                           ResetChatTimer()
                                       End If
                                   End Sub)
                End If
            End Sub
        )
        _chatSubscription = _firebase.GetChatObservable(_hostId).Subscribe(onNext)
    End Sub

    Private Sub ResetChatTimer()
        _chatTimer?.Stop()
        _chatTimer?.Start()
    End Sub

    Private Sub ChatTimer_Tick(sender As Object, e As EventArgs)
        _chatTimer?.Stop()
        txtChatInput.Visible = False
        txtChatInput.Text = ""
        pnlChatOverlay.Height = 0
        Me.Focus()
    End Sub

    Private Sub txtChatInput_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            e.SuppressKeyPress = True
            Dim text = txtChatInput.Text.Trim()
            If Not String.IsNullOrEmpty(text) AndAlso _firebase IsNot Nothing AndAlso Not String.IsNullOrEmpty(_hostId) Then
                Dim user = If(String.IsNullOrEmpty(_discordUsername), "guest", _discordUsername)
                _firebase.SendChatMessageAsync(_hostId, user, text)
            End If
            txtChatInput.Text = ""
            txtChatInput.Visible = False
            Me.Focus()
        End If
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        Dim handled As Boolean = False
        Select Case keyData
            Case Keys.Alt Or Keys.D1, Keys.Alt Or Keys.NumPad1
                _displayMode = VideoDisplayMode.TopLeft
                handled = True
            Case Keys.Alt Or Keys.D2, Keys.Alt Or Keys.NumPad2
                _displayMode = VideoDisplayMode.TopRight
                handled = True
            Case Keys.Alt Or Keys.D3, Keys.Alt Or Keys.NumPad3
                _displayMode = VideoDisplayMode.BottomRight
                handled = True
            Case Keys.Alt Or Keys.D4, Keys.Alt Or Keys.NumPad4
                _displayMode = VideoDisplayMode.BottomLeft
                handled = True
            Case Keys.Alt Or Keys.D5, Keys.Alt Or Keys.NumPad5
                _displayMode = VideoDisplayMode.Normal
                handled = True
        End Select

        If handled Then Return True
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Private Sub VideoForm_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If _running AndAlso Not txtChatInput.Visible Then
            XInputSender.UpdateKeyState(e.KeyCode, True)
            For Each kvp In XInputSender.KeyMapping
                If kvp.Value = e.KeyCode Then
                    If e.KeyCode <> Keys.Enter Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If
                    Exit For
                End If
            Next
        End If

        If e.KeyCode = Keys.F11 Then
            If Me.WindowState = FormWindowState.Maximized AndAlso Me.FormBorderStyle = FormBorderStyle.None Then
                Me.FormBorderStyle = FormBorderStyle.Sizable
                Me.WindowState = FormWindowState.Normal
            Else
                Me.FormBorderStyle = FormBorderStyle.None
                Me.WindowState = FormWindowState.Maximized
            End If
            Return
        End If

        If e.KeyCode = Keys.Enter Then
            If _running AndAlso Not txtChatInput.Visible Then
                pnlChatOverlay.Visible = True
                pnlChatOverlay.Height = 110
                pnlChatOverlay.BringToFront()
                txtChatInput.Visible = True
                txtChatInput.Focus()
                ResetChatTimer()
                e.Handled = True
                e.SuppressKeyPress = True
                Return
            End If
        End If

        If e.KeyCode = Keys.Escape Then
            If Me.FormBorderStyle = FormBorderStyle.None Then
                Me.FormBorderStyle = FormBorderStyle.Sizable
                Me.WindowState = FormWindowState.Normal
            Else
                Me.Close()
            End If
        End If
    End Sub

    Private Sub VideoForm_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        If _running Then
            XInputSender.UpdateKeyState(e.KeyCode, False)
            For Each kvp In XInputSender.KeyMapping
                If kvp.Value = e.KeyCode Then
                    e.Handled = True
                    e.SuppressKeyPress = True
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Async Sub VideoForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        _running = False
        _renderTimer?.Stop()
        _telemetryTimer?.Stop()
        _chatTimer?.Stop()
        _chatSubscription?.Dispose()

        ' Stop XInput and Receivers
        _xinput?.Stop()
        Thread.Sleep(100)
        _video?.Stop()
        _audio?.Stop()
        _renderer?.Dispose()

        ' Send leave message to Firebase chat
        If Not String.IsNullOrEmpty(_hostId) AndAlso _firebase IsNot Nothing Then
            Dim nameToWrite = If(String.IsNullOrEmpty(_discordUsername), "guest", _discordUsername)
            _firebase.SendChatMessageAsync(_hostId, "SYSTEM", $"<P{_slot}><{nameToWrite}> disconnected")
            ' Clean Firebase slot
            Try
                Await _firebase.UpdateSlotUserAsync(_hostId, _slot, nameToWrite, False)
            Catch ex As Exception
                Debug.WriteLine("[FIREBASE] Slot clean failed: " & ex.Message)
            End Try
        End If

        ' Close Sockets
        Try
            _handshakeClient?.Close()
        Catch
        End Try
        Try
            _videoUdp?.Close()
        Catch
        End Try
        Try
            _audioUdp?.Close()
        Catch
        End Try

        ' Close UPnP Ports
        If Not _isLocalMode Then
            Await UPnPHelper.ClosePorts(_portXInput, _portHS, _portVideo, _portAudio)
        End If

        ' Notify Parent Form to update UI
        If _parentForm IsNot Nothing Then
            _parentForm.BeginInvoke(Sub()
                                        If TypeOf _parentForm Is Form1 Then
                                            CType(_parentForm, Form1).OnStreamingDisconnected()
                                        End If
                                    End Sub)
        End If
    End Sub
End Class
