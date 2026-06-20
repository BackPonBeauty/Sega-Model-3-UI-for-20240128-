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

Imports System.Drawing
Imports System.Windows.Forms

' ========================================
' CyberButton
' ========================================
Public Class CyberButton
    Inherits Button

    Public Property GlowColor As Color = Color.FromArgb(0, 238, 255)
    Private _isHover As Boolean = False

    Public Sub New()
        ' DoubleBufferも有効にしてちらつき防止
        SetStyle(ControlStyles.UserPaint Or
                 ControlStyles.AllPaintingInWmPaint Or
                 ControlStyles.OptimizedDoubleBuffer, True)
        FlatStyle = FlatStyle.Flat
        FlatAppearance.BorderSize = 0
        ' TransparentはUserPaintと相性が悪いので親の背景色に合わせる
        BackColor = Color.FromArgb(10, 14, 26)
        ForeColor = Color.FromArgb(0, 238, 255)
        Font = New Font("Consolas", 9, FontStyle.Bold)
        Cursor = Cursors.Hand
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        ' 背景を確実に塗りつぶす（Transparent廃止）
        Using br As New SolidBrush(BackColor)
            g.FillRectangle(br, ClientRectangle)
        End Using

        ' ホバー時のグロー背景
        If _isHover AndAlso Me.Enabled Then
            Using br As New SolidBrush(Color.FromArgb(40, GlowColor.R, GlowColor.G, GlowColor.B))
                g.FillRectangle(br, ClientRectangle)
            End Using
        End If

        ' 枠線
        Using pen As New Pen(If(Me.Enabled, GlowColor, Color.FromArgb(60, 60, 60)), 1.5F)
            g.DrawRectangle(pen, 1, 1, Width - 3, Height - 3)
        End Using

        ' 四隅ブラケット
        Dim s = 7
        Using pen As New Pen(If(Me.Enabled, Color.White, Color.FromArgb(60, 60, 60)), 1.2F)
            g.DrawLine(pen, 1, 1, 1 + s, 1) : g.DrawLine(pen, 1, 1, 1, 1 + s)
            g.DrawLine(pen, Width - 2, 1, Width - 2 - s, 1) : g.DrawLine(pen, Width - 2, 1, Width - 2, 1 + s)
            g.DrawLine(pen, 1, Height - 2, 1 + s, Height - 2) : g.DrawLine(pen, 1, Height - 2, 1, Height - 2 - s)
            g.DrawLine(pen, Width - 2, Height - 2, Width - 2 - s, Height - 2) : g.DrawLine(pen, Width - 2, Height - 2, Width - 2, Height - 2 - s)
        End Using

        ' テキスト
        Dim sf As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
        Using br As New SolidBrush(If(Me.Enabled, ForeColor, Color.FromArgb(80, 80, 80)))
            g.DrawString(Text, Font, br, New RectangleF(0, 0, Width, Height), sf)
        End Using
    End Sub

    Protected Overrides Sub OnEnabledChanged(e As EventArgs)
        MyBase.OnEnabledChanged(e) : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        _isHover = True
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        _isHover = False
        Invalidate()
    End Sub
End Class

' ========================================
' CyberPanel
' ========================================
Public Class CyberPanel
    Inherits Panel
    Public Property Title As String = ""
    Public Property BorderColor As Color = Color.FromArgb(0, 180, 220)
    Public Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw, True)
        BackColor = Color.FromArgb(5, 15, 30)
    End Sub
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Using pen As New Pen(BorderColor, 1.5F)
            g.DrawRectangle(pen, 1, 1, Width - 3, Height - 3)
        End Using
        Dim s = 14
        Using pen As New Pen(Color.FromArgb(0, 238, 255), 2)
            g.DrawLine(pen, 1, 1, 1 + s, 1) : g.DrawLine(pen, 1, 1, 1, 1 + s)
            g.DrawLine(pen, Width - 2, 1, Width - 2 - s, 1) : g.DrawLine(pen, Width - 2, 1, Width - 2, 1 + s)
            g.DrawLine(pen, 1, Height - 2, 1 + s, Height - 2) : g.DrawLine(pen, 1, Height - 2, 1, Height - 2 - s)
            g.DrawLine(pen, Width - 2, Height - 2, Width - 2 - s, Height - 2) : g.DrawLine(pen, Width - 2, Height - 2, Width - 2, Height - 2 - s)
        End Using
        If Title <> "" Then
            Dim tf As New Font("Consolas", 8, FontStyle.Bold)
            Dim titleText = $"[ {Title} ]"
            Dim sz = g.MeasureString(titleText, tf)
            Dim tx = (Width - sz.Width) / 2
            Using br As New SolidBrush(Color.FromArgb(5, 15, 30))
                g.FillRectangle(br, tx - 4, -1, sz.Width + 8, sz.Height + 2)
            End Using
            Using br As New SolidBrush(Color.FromArgb(0, 238, 255))
                g.DrawString(titleText, tf, br, tx, 0)
            End Using
        End If
    End Sub
End Class

' ========================================
' CyberListView
' ========================================
Public Class CyberListView
    Inherits ListView

    Public Event SlotClicked(slotIndex As Integer, item As ListViewItem)
    Public Event RowClicked(item As ListViewItem)

    Public Sub New()
        View = View.Details
        FullRowSelect = False
        GridLines = False
        BackColor = Color.FromArgb(3, 10, 22)
        ForeColor = Color.FromArgb(0, 238, 255)
        Font = New Font("Consolas", 9)
        BorderStyle = BorderStyle.None
        OwnerDraw = True
        HeaderStyle = ColumnHeaderStyle.Nonclickable
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button <> MouseButtons.Left Then Return

        Dim hit = Me.HitTest(e.X, e.Y)
        If hit.Item Is Nothing Then Return

        ' X座標から列インデックスを手動計算
        Dim colIndex As Integer = 0
        Dim cumX As Integer = 0
        For i = 0 To Me.Columns.Count - 1
            cumX += Me.Columns(i).Width
            If e.X < cumX Then
                colIndex = i
                Exit For
            End If
        Next

        If colIndex >= 1 AndAlso colIndex <= 4 Then
            ' P1〜P4列クリック
            If hit.Item.SubItems.Count > colIndex Then
                Dim subText = hit.Item.SubItems(colIndex).Text
                If subText.StartsWith("●") Then
                    RaiseEvent SlotClicked(colIndex, hit.Item)
                End If
            End If
        Else
            ' HOST IP列クリック → 行選択イベント
            RaiseEvent RowClicked(hit.Item)
        End If
    End Sub

    Protected Overrides Sub OnDrawColumnHeader(e As DrawListViewColumnHeaderEventArgs)
        Using br As New SolidBrush(Color.FromArgb(0, 25, 45))
            e.Graphics.FillRectangle(br, e.Bounds)
        End Using
        Using pen As New Pen(Color.FromArgb(0, 180, 220), 1)
            e.Graphics.DrawLine(pen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1)
        End Using
        Dim sf As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
        Using br As New SolidBrush(Color.FromArgb(0, 210, 255))
            e.Graphics.DrawString(e.Header.Text, New Font("Consolas", 8, FontStyle.Bold), br, e.Bounds, sf)
        End Using
    End Sub

    Protected Overrides Sub OnDrawItem(e As DrawListViewItemEventArgs)
        e.DrawDefault = False
    End Sub

    Protected Overrides Sub OnDrawSubItem(e As DrawListViewSubItemEventArgs)
        Dim g = e.Graphics
        Dim b = e.Bounds
        If e.Item.Selected Then
            Using br As New SolidBrush(Color.FromArgb(0, 50, 80))
                g.FillRectangle(br, b)
            End Using
        End If
        Using pen As New Pen(Color.FromArgb(0, 40, 65), 1)
            g.DrawLine(pen, b.Left, b.Bottom - 1, b.Right, b.Bottom - 1)
        End Using
        If e.ColumnIndex = 0 Then
            Using br As New SolidBrush(Color.FromArgb(160, 230, 255))
                Dim rect = b
                rect.X += 8
                Dim sf As New StringFormat() With {.LineAlignment = StringAlignment.Center}
                g.DrawString(e.SubItem.Text, e.Item.Font, br, rect, sf)
            End Using
        Else
            Dim txt = e.SubItem.Text
            Dim available = txt.StartsWith("●")
            Dim cx = b.Left + b.Width \ 2
            Dim cy = b.Top + b.Height \ 2
            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            If available Then
                Dim userCount = 0
                If txt.Length > 1 Then
                    Integer.TryParse(txt.Substring(1), userCount)
                End If

                Dim glowColor As Color
                Dim dotColor As Color

                If userCount = 1 Then
                    glowColor = Color.FromArgb(35, 255, 238, 0)
                    dotColor = Color.FromArgb(255, 238, 0)
                ElseIf userCount >= 2 Then
                    glowColor = Color.FromArgb(35, 255, 60, 60)
                    dotColor = Color.FromArgb(255, 60, 60)
                Else
                    glowColor = Color.FromArgb(35, 0, 238, 255)
                    dotColor = Color.FromArgb(0, 238, 255)
                End If

                Using br As New SolidBrush(glowColor)
                    g.FillEllipse(br, cx - 9, cy - 9, 18, 18)
                End Using
                Using br As New SolidBrush(dotColor)
                    g.FillEllipse(br, cx - 5, cy - 5, 10, 10)
                End Using
            Else
                Dim dotColor = If(txt = "×", Color.FromArgb(160, 40, 40), Color.FromArgb(0, 50, 70))
                Using br As New SolidBrush(dotColor)
                    g.FillEllipse(br, cx - 4, cy - 4, 8, 8)
                End Using
            End If
        End If
    End Sub
End Class

Public Class TransparentChatLog
    Inherits Control

    Private _messages As New List(Of String)()
    Private ReadOnly _maxLines As Integer = 6

    Public Sub New()
        SetStyle(ControlStyles.SupportsTransparentBackColor Or
                 ControlStyles.UserPaint Or
                 ControlStyles.AllPaintingInWmPaint Or
                 ControlStyles.OptimizedDoubleBuffer, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(220, 220, 220)
        Font = New Font("MS Gothic", 9.5F, FontStyle.Bold)
    End Sub

    Public Sub AppendChat(msg As String)
        _messages.Add(msg)
        If _messages.Count > _maxLines Then
            _messages.RemoveAt(0)
        End If
        Invalidate()
    End Sub

    Public Sub Clear()
        _messages.Clear()
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit
        
        Dim fontHeight = Font.Height
        Dim lineSpacing = 4
        Dim startY = Height - 5
        
        ' Draw messages from bottom to top
        For i = _messages.Count - 1 To 0 Step -1
            Dim msg = _messages(i)
            Dim sz = g.MeasureString(msg, Font, Width)
            Dim rectHeight = CInt(Math.Ceiling(sz.Height))
            startY -= rectHeight
            
            Dim rect As New Rectangle(5, startY, Width - 10, rectHeight)
            
            ' Draw text shadow/outline (black) for high contrast on video background
            For dx = -1 To 1
                For dy = -1 To 1
                    If dx <> 0 OrElse dy <> 0 Then
                        Using shadowBrush As New SolidBrush(Color.FromArgb(160, 0, 0, 0))
                            Dim shadowRect = rect
                            shadowRect.Offset(dx, dy)
                            g.DrawString(msg, Font, shadowBrush, shadowRect)
                        End Using
                    End If
                Next
            Next
            
            ' Draw main text
            Using textBrush As New SolidBrush(ForeColor)
                g.DrawString(msg, Font, textBrush, rect)
            End Using
            
            startY -= lineSpacing
            If startY < 0 Then Exit For
        Next
    End Sub
End Class