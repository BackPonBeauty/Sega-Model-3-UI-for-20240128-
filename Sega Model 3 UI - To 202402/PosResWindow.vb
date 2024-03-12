Public Class PosResWindow

    Private Sub Me_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim s As System.Windows.Forms.Screen = System.Windows.Forms.Screen.FromControl(Me)
        'ディスプレイの高さと幅を取得
        Dim x As Integer = s.Bounds.X
        Dim y As Integer = s.Bounds.Y
        Dim h As Integer = s.Bounds.Height
        Dim w As Integer = s.Bounds.Width
        Center(y, h, x, w)
        Dim N As Integer
        For i = 0 To Form1.ScreenN.Length - 1
            If Me.Top >= Form1.By(i) And Me.Top <= (Form1.By(i) + Form1.Bh(i)) And Me.Left >= Form1.Bx(i) And Me.Left <= (Form1.Bx(i) + Form1.Bw(i)) Then
                Label2.Text = Form1.ScreenN(i).ToString
                N = i
                Exit For

            End If
        Next
        Me.BackgroundImage = Image.FromFile("Snaps\" & Form1.Roms & ".jpg")
        Me.BackgroundImageLayout = ImageLayout.Stretch
    End Sub


    Private Sub Button1_PreviewKeyDown(ByVal sender As Object, ByVal e As PreviewKeyDownEventArgs) Handles Button1.PreviewKeyDown, Button3.PreviewKeyDown
        Dim n As Integer = 1
        Select Case e.KeyCode
            Case Keys.Up
                Me.Top -= n
            Case Keys.Left
                Me.Left -= n
            Case Keys.Right
                Me.Left += n
            Case Keys.Down
                Me.Top += n
            Case Keys.Escape
                Me.Close()
            Case Keys.Enter
                Button1.PerformClick()
        End Select
        Label1.Text = "( " & Me.Left & " , " & Me.Top & " )"
        e.IsInputKey = True
    End Sub

    Private Sub Form2_Show(sender As Object, e As EventArgs) Handles MyBase.Shown

        Me.Width = Integer.Parse(Form1.Label_xRes.Text)
        Me.Height = Integer.Parse(Form1.Label_yRes.Text)
        Button3.Top = (Me.Height / 2) - (Button3.Height / 2)
        Button3.Left = (Me.Width / 2) - (Button3.Width / 2)
        Dim s As System.Windows.Forms.Screen = System.Windows.Forms.Screen.FromControl(Me)
        'ディスプレイの高さと幅を取得
        Dim x As Integer = s.Bounds.X
        Dim y As Integer = s.Bounds.Y
        Dim h As Integer = s.Bounds.Height
        Dim w As Integer = s.Bounds.Width
        Center(y, h, x, w)
        Label1.Text = "( " & Me.Left & " , " & Me.Top & " )"
        Label5.Text = "( " & Form1.Label_xRes.Text & " , " & Form1.Label_yRes.Text & " )"
    End Sub

    Private mousePoint As Point

    'MouseEventHandler
    'MouseClick
    Private Sub Form1_MouseDown(ByVal sender As Object,
            ByVal e As System.Windows.Forms.MouseEventArgs) _
            Handles MyBase.MouseDown
        If (e.Button And MouseButtons.Left) = MouseButtons.Left Then
            'RecordMousePosition
            mousePoint = New Point(e.X, e.Y)
        End If
    End Sub

    'MouseMoveEventHandler
    'MouseMove
    Private Sub Form1_MouseMove(ByVal sender As Object,
            ByVal e As System.Windows.Forms.MouseEventArgs) _
            Handles MyBase.MouseMove
        If (e.Button And MouseButtons.Left) = MouseButtons.Left Then
            Me.Left += e.X - mousePoint.X
            Me.Top += e.Y - mousePoint.Y
            '/// or
            'Me.Location = New Point( _
            '    Me.Location.X + e.X - mousePoint.X, _
            '    Me.Location.Y + e.Y - mousePoint.Y)
            Label1.Text = "( " & Me.Left & " , " & Me.Top & " )"
        End If
        Dim N As Integer
        For i = 0 To Form1.ScreenN.Length - 1
            If Me.Top >= Form1.By(i) And Me.Top <= (Form1.By(i) + Form1.Bh(i)) And Me.Left >= Form1.Bx(i) And Me.Left <= (Form1.Bx(i) + Form1.Bw(i)) Then
                Label2.Text = Form1.ScreenN(i).ToString
                N = i
                Exit For

            End If
        Next

    End Sub

    Private Sub Center_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim N As Integer
        For i = 0 To Form1.ScreenN.Length - 1
            If Me.Top >= Form1.By(i) And Me.Top <= (Form1.By(i) + Form1.Bh(i)) And Me.Left >= Form1.Bx(i) And Me.Left <= (Form1.Bx(i) + Form1.Bw(i)) Then
                Label2.Text = Form1.ScreenN(i).ToString
                N = i
                Exit For

            End If
            Form1.Debug(i & ":::::" & Form1.ScreenN(i).ToString)
            'Form1.Debug(Form1.Screen(0).ToString)And And (Me.Left > Form1.Bx(0) And Me.Left < Form1.Bw(0)) 
        Next
        Center(Form1.By(N), Form1.Bh(N), Form1.Bx(N), Form1.Bw(N))
    End Sub

    Private Sub Center(By As Integer, Bh As Integer, Bx As Integer, Bw As Integer)
        Me.Top = ((Bh / 2) + By) - (Me.Height / 2)
        Me.Left = ((Bw / 2) + Bx) - (Me.Width / 2)
        'Me.Top = (Form1.Label_hScreenRes.Text / 2) - (Me.Height / 2)
        'Me.Left = (Form1.Label_wScreenRes.Text / 2) - (Me.Width / 2)
        Label1.Text = "( " & Me.Left & " , " & Me.Top & " )"
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form1.Label_xPos.Text = Me.Left
        Form1.Label_yPos.Text = Me.Top
        Form1.Label_xRes.Text = Me.Width
        Form1.Label_yRes.Text = Me.Height
    End Sub


End Class