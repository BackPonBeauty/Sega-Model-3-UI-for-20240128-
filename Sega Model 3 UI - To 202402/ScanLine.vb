Imports System.Reflection.Emit
Imports System.Threading
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar

Public Class ScanLine


    Private Sub ScanLine_DoubleClick(sender As Object, e As EventArgs) Handles MyBase.DoubleClick
        Me.Close()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.DoubleClick
        Me.Close()
    End Sub

    Private Sub ScanLine_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Enabled = True
    End Sub

    Public Sub Draw_Scanline(scanline_type As String)
        PictureBox1.BackgroundImage = Nothing
        PictureBox1.Image = Nothing
        If Form1.scanline_type = "LINE1" Then
            Me.BackColor = Color.White
            PictureBox1.BackColor = Color.White
            Me.TransparencyKey = Color.White
            Dim canvas As New Bitmap(PictureBox1.Width, PictureBox1.Height)

            Dim g As Graphics = Graphics.FromImage(canvas)
            Dim p_bin_1 As Double = (Integer.Parse(Form1.Label_yRes.Text) / 480)
            Dim p_bin As Integer = Math.Round(p_bin_1)
            'Form1.Debug(p_bin)
            Dim w As Integer = Integer.Parse(Form1.Label_xRes.Text)
            Dim h As Integer = Integer.Parse(Form1.Label_yRes.Text)
            Dim blackPen As Pen = New Pen(Color.FromArgb(255, 0, 0, 0), p_bin)

            For i As Integer = 0 To h Step p_bin + p_bin
                g.DrawLine(blackPen, 0, i, w, i)
            Next
            g.Dispose()
            PictureBox1.Image = canvas
        ElseIf Form1.scanline_type = "LINE2" Then
            Me.BackColor = Color.White
            PictureBox1.BackColor = Color.White
            Me.TransparencyKey = Color.White
            Dim canvas As New Bitmap(PictureBox1.Width, PictureBox1.Height)

            Dim g As Graphics = Graphics.FromImage(canvas)
            Dim p_bin_1 As Double = (Integer.Parse(Form1.Label_yRes.Text) / 480)
            Dim p_bin As Integer = Math.Round(p_bin_1)
            'Form1.Debug(p_bin)
            Dim w As Integer = Integer.Parse(Form1.Label_xRes.Text)
            Dim h As Integer = Integer.Parse(Form1.Label_yRes.Text)
            Dim blackPen As Pen = New Pen(Color.FromArgb(255, 0, 0, 0), p_bin / 2)

            For i As Integer = 0 To h Step p_bin + p_bin
                g.DrawLine(blackPen, 0, i, w, i)
            Next
            g.Dispose()
            PictureBox1.Image = canvas
        Else
            PictureBox1.BackgroundImage = My.Resources.scanline
            PictureBox1.BackgroundImageLayout = ImageLayout.Tile
        End If
        Me.Opacity = Form1.Opacity_D / 10
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Process.GetProcessesByName("Supermodel").Count <> 0 Then
        Else
            Timer1.Enabled = False
            Form1.ScanLine_F = False
            Me.Close()
        End If

    End Sub
End Class