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

Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports SharpDX.XInput

Public Class KeyConfigForm
    Inherits Form

    Private _tempKeyMapping As Dictionary(Of String, Keys)
    Private _tempPadMapping As Dictionary(Of String, String)
    
    Private _waitingKeyAction As String = ""
    Private _waitingPadAction As String = ""

    Private _keyButtons As New Dictionary(Of String, CyberButton)()
    Private _padButtons As New Dictionary(Of String, CyberButton)()

    Private pnlScroll As Panel
    Private btnSave As CyberButton
    Private btnCancel As CyberButton

    Private _controller As Controller
    Private _pollTimer As Timer

    Public Sub New()
        ' Window setup
        Me.Text = "KEY & CONTROLLER CONFIG"
        Me.Size = New Size(580, 600)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.BackColor = Color.FromArgb(10, 14, 26)
        Me.ForeColor = Color.FromArgb(0, 238, 255)
        Me.Font = New Font("Consolas", 9.0!)
        Me.KeyPreview = True

        ' Load current mappings
        _tempKeyMapping = New Dictionary(Of String, Keys)(XInputSender.KeyMapping)
        _tempPadMapping = New Dictionary(Of String, String)(XInputSender.PadMapping)

        ' Initialize XInput controller for polling configuration inputs
        _controller = New Controller(UserIndex.One)

        BuildUI()

        ' Start polling timer for controller configuration input detection
        _pollTimer = New Timer()
        _pollTimer.Interval = 50
        AddHandler _pollTimer.Tick, AddressOf PollTimer_Tick
        _pollTimer.Start()
    End Sub

    Private Sub BuildUI()
        Dim rowHeight = 32
        Dim labelWidth = 140
        Dim colWidth = 160

        ' Headers (placed statically at the top of the form)
        Dim lblActionHeader As New Label() With {
            .Text = "ACTION",
            .Location = New Point(20, 20),
            .Size = New Size(labelWidth, 24),
            .ForeColor = Color.White,
            .Font = New Font("Consolas", 9, FontStyle.Bold)
        }
        Me.Controls.Add(lblActionHeader)

        Dim lblKeyHeader As New Label() With {
            .Text = "KEYBOARD KEY",
            .Location = New Point(180, 20),
            .Size = New Size(colWidth, 24),
            .ForeColor = Color.White,
            .Font = New Font("Consolas", 9, FontStyle.Bold)
        }
        Me.Controls.Add(lblKeyHeader)

        Dim lblPadHeader As New Label() With {
            .Text = "PHYSICAL CONTROLLER",
            .Location = New Point(350, 20),
            .Size = New Size(180, 24),
            .ForeColor = Color.White,
            .Font = New Font("Consolas", 9, FontStyle.Bold)
        }
        Me.Controls.Add(lblPadHeader)

        ' Scrollable Panel for actions
        pnlScroll = New Panel() With {
            .Location = New Point(10, 50),
            .Size = New Size(545, 430),
            .AutoScroll = True,
            .BackColor = Color.FromArgb(12, 17, 32)
        }
        Me.Controls.Add(pnlScroll)

        Dim actions As String() = {
            "DpadUp", "DpadDown", "DpadLeft", "DpadRight",
            "A", "B", "X", "Y",
            "LB", "RB", "LTrigger", "RTrigger",
            "LThumb", "RThumb", "Start", "Back",
            "LStickUp", "LStickDown", "LStickLeft", "LStickRight",
            "RStickUp", "RStickDown", "RStickLeft", "RStickRight"
        }
        Dim displayNames As String() = {
            "DPAD UP", "DPAD DOWN", "DPAD LEFT", "DPAD RIGHT",
            "A BUTTON", "B BUTTON", "X BUTTON", "Y BUTTON",
            "L SHOULDER (LB)", "R SHOULDER (RB)", "L TRIGGER (LT)", "R TRIGGER (RT)",
            "L STICK CLICK", "R STICK CLICK", "START", "BACK (COIN)",
            "L STICK UP", "L STICK DOWN", "L STICK LEFT", "L STICK RIGHT",
            "R STICK UP", "R STICK DOWN", "R STICK LEFT", "R STICK RIGHT"
        }

        For i = 0 To actions.Length - 1
            Dim action = actions(i)
            Dim displayName = displayNames(i)

            ' Action Label
            Dim lbl As New Label() With {
                .Text = displayName & " :",
                .Location = New Point(10, i * rowHeight),
                .Size = New Size(labelWidth, 24),
                .ForeColor = Color.FromArgb(0, 180, 220),
                .TextAlign = ContentAlignment.MiddleLeft
            }
            pnlScroll.Controls.Add(lbl)

            ' Keyboard Config Button
            Dim btnKey As New CyberButton() With {
                .Text = If(_tempKeyMapping(action) = Keys.None, "NONE", _tempKeyMapping(action).ToString()),
                .Location = New Point(170, i * rowHeight - 2),
                .Size = New Size(colWidth - 10, 26),
                .GlowColor = Color.FromArgb(0, 180, 220),
                .Tag = action
            }
            AddHandler btnKey.Click, AddressOf btnKey_Click
            pnlScroll.Controls.Add(btnKey)
            _keyButtons(action) = btnKey

            ' Controller Config Button
            Dim btnPad As New CyberButton() With {
                .Text = If(String.IsNullOrEmpty(_tempPadMapping(action)) OrElse _tempPadMapping(action) = "NONE", "NONE", _tempPadMapping(action)),
                .Location = New Point(340, i * rowHeight - 2),
                .Size = New Size(160, 26),
                .GlowColor = Color.FromArgb(0, 180, 220),
                .Tag = action
            }
            AddHandler btnPad.Click, AddressOf btnPad_Click
            pnlScroll.Controls.Add(btnPad)
            _padButtons(action) = btnPad
        Next

        ' Save Button (placed statically at the bottom)
        btnSave = New CyberButton() With {
            .Text = "SAVE",
            .Location = New Point(140, 500),
            .Size = New Size(130, 30),
            .GlowColor = Color.FromArgb(0, 220, 100),
            .ForeColor = Color.FromArgb(0, 220, 100)
        }
        AddHandler btnSave.Click, AddressOf btnSave_Click
        Me.Controls.Add(btnSave)

        ' Cancel Button (placed statically at the bottom)
        btnCancel = New CyberButton() With {
            .Text = "CANCEL",
            .Location = New Point(310, 500),
            .Size = New Size(130, 30),
            .GlowColor = Color.FromArgb(220, 60, 60),
            .ForeColor = Color.FromArgb(220, 60, 60)
        }
        AddHandler btnCancel.Click, AddressOf btnCancel_Click
        Me.Controls.Add(btnCancel)
    End Sub

    Private Sub btnKey_Click(sender As Object, e As EventArgs)
        Dim btn = DirectCast(sender, CyberButton)
        Dim action = btn.Tag.ToString()

        ResetWaitingStates()

        _waitingKeyAction = action
        btn.Text = "PRESS KEY..."
        btn.GlowColor = Color.FromArgb(255, 160, 0)
    End Sub

    Private Sub btnPad_Click(sender As Object, e As EventArgs)
        Dim btn = DirectCast(sender, CyberButton)
        Dim action = btn.Tag.ToString()

        ResetWaitingStates()

        _waitingPadAction = action
        btn.Text = "PRESS PAD INPUT..."
        btn.GlowColor = Color.FromArgb(255, 160, 0)
    End Sub

    Private Sub ResetWaitingStates()
        If Not String.IsNullOrEmpty(_waitingKeyAction) Then
            _keyButtons(_waitingKeyAction).Text = If(_tempKeyMapping(_waitingKeyAction) = Keys.None, "NONE", _tempKeyMapping(_waitingKeyAction).ToString())
            _keyButtons(_waitingKeyAction).GlowColor = Color.FromArgb(0, 180, 220)
            _waitingKeyAction = ""
        End If
        If Not String.IsNullOrEmpty(_waitingPadAction) Then
            _padButtons(_waitingPadAction).Text = If(String.IsNullOrEmpty(_tempPadMapping(_waitingPadAction)) OrElse _tempPadMapping(_waitingPadAction) = "NONE", "NONE", _tempPadMapping(_waitingPadAction))
            _padButtons(_waitingPadAction).GlowColor = Color.FromArgb(0, 180, 220)
            _waitingPadAction = ""
        End If
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        If Not String.IsNullOrEmpty(_waitingKeyAction) Then
            Dim pressedKey = e.KeyCode
            e.Handled = True
            e.SuppressKeyPress = True

            ' Escape resets or clears key assignment
            If pressedKey = Keys.Escape Then
                _tempKeyMapping(_waitingKeyAction) = Keys.None
                _keyButtons(_waitingKeyAction).Text = "NONE"
                _keyButtons(_waitingKeyAction).GlowColor = Color.FromArgb(0, 180, 220)
                _waitingKeyAction = ""
                Return
            End If

            ' Duplicate check and override logic
            Dim duplicateAction As String = ""
            For Each kvp In _tempKeyMapping
                If kvp.Key <> _waitingKeyAction AndAlso kvp.Value <> Keys.None AndAlso kvp.Value = pressedKey Then
                    duplicateAction = kvp.Key
                    Exit For
                End If
            Next

            If Not String.IsNullOrEmpty(duplicateAction) Then
                ' Clear the duplicated key assignment (override)
                _tempKeyMapping(duplicateAction) = Keys.None
                If _keyButtons.ContainsKey(duplicateAction) Then
                    _keyButtons(duplicateAction).Text = "NONE"
                End If
            End If

            ' Assign the new key
            _tempKeyMapping(_waitingKeyAction) = pressedKey
            _keyButtons(_waitingKeyAction).Text = pressedKey.ToString()
            _keyButtons(_waitingKeyAction).GlowColor = Color.FromArgb(0, 180, 220)
            _waitingKeyAction = ""
            Return
        End If

        MyBase.OnKeyDown(e)
    End Sub

    Private Sub PollTimer_Tick(sender As Object, e As EventArgs)
        If String.IsNullOrEmpty(_waitingPadAction) Then Return
        If Not _controller.IsConnected Then Return

        Try
            Dim state As State
            _controller.GetState(state)

            Dim activeInput = XInputSender.ScanActiveControllerInput(state.Gamepad)
            If Not String.IsNullOrEmpty(activeInput) Then
                ' Duplicate check and override logic
                Dim duplicateAction As String = ""
                For Each kvp In _tempPadMapping
                    If kvp.Key <> _waitingPadAction AndAlso kvp.Value = activeInput Then
                        duplicateAction = kvp.Key
                        Exit For
                    End If
                Next

                If Not String.IsNullOrEmpty(duplicateAction) Then
                    ' Clear the duplicated pad assignment (override)
                    _tempPadMapping(duplicateAction) = "NONE"
                    If _padButtons.ContainsKey(duplicateAction) Then
                        _padButtons(duplicateAction).Text = "NONE"
                    End If
                End If

                ' Assign pad input
                _tempPadMapping(_waitingPadAction) = activeInput
                _padButtons(_waitingPadAction).Text = activeInput
                _padButtons(_waitingPadAction).GlowColor = Color.FromArgb(0, 180, 220)
                _waitingPadAction = ""
            End If
        Catch
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs)
        _pollTimer?.Stop()
        XInputSender.KeyMapping = New Dictionary(Of String, Keys)(_tempKeyMapping)
        XInputSender.PadMapping = New Dictionary(Of String, String)(_tempPadMapping)
        XInputSender.SaveConfig()
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs)
        _pollTimer?.Stop()
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        _pollTimer?.Stop()
        MyBase.OnFormClosing(e)
    End Sub
End Class
