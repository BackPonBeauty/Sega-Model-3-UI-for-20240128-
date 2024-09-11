Imports System.IO
Imports System.Text

Public Class PosResWindow
    Dim Last_index As Integer
    Private Sub Me_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For i As Integer = 1 To 3
            Me.Panel1.Controls("Button" & i).BackColor = Color.FromArgb(255, Form1.Bgcolor_R, Form1.Bgcolor_G, Form1.Bgcolor_B)
            Me.Panel1.Controls("Button" & i).ForeColor = Form1.Pub_Forecolor_s
        Next
        Me.Width = Integer.Parse(Form1.Label_xRes.Text)
        Me.Height = Integer.Parse(Form1.Label_yRes.Text)
        Dim s As System.Windows.Forms.Screen = System.Windows.Forms.Screen.FromControl(Me)

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
        Dim canvas As New Bitmap(Integer.Parse(Form1.Label_xRes.Text), Integer.Parse(Form1.Label_yRes.Text))
        Dim g As Graphics = Graphics.FromImage(canvas)
        Dim img As Image = Image.FromFile("Snaps\" & Form1.Roms & ".jpg")
        Dim cm As New System.Drawing.Imaging.ColorMatrix()
        cm.Matrix00 = 1
        cm.Matrix11 = 1
        cm.Matrix22 = 1
        cm.Matrix33 = 0.25F
        cm.Matrix44 = 1
        Dim ia As New System.Drawing.Imaging.ImageAttributes()
        ia.SetColorMatrix(cm)
        g.DrawImage(img, New Rectangle(0, 0, Me.Width, Me.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia)
        img.Dispose()
        g.Dispose()

        Me.BackgroundImageLayout = ImageLayout.Stretch
        Me.BackgroundImage = canvas

        GetAllControls(Me.Panel1, Integer.Parse(Form1.FontSize_bin))
        LoadResolution()
    End Sub
    Private Sub GetAllControls(ByVal control As Control, size As Integer)
        If control.HasChildren Then
            For Each childControl As Control In control.Controls
                GetAllControls(childControl, size)
                childControl.Font = New Font("Arial", size, FontStyle.Regular)
            Next childControl
        End If
    End Sub

    Private Sub LoadResolution()
        Dim line As String = ""
        Dim al As New ArrayList

        Using sr As StreamReader = New StreamReader(
          "Resolution.txt", Encoding.GetEncoding("UTF-8"))

            line = sr.ReadLine()
            Do Until line Is Nothing
                ComboBox_resolution.Items.Add(line)
                line = sr.ReadLine()
            Loop
            ComboBox_resolution.SelectedIndex = Form1.ComboBox_resolution.SelectedIndex
            Last_index = ComboBox_resolution.SelectedIndex
        End Using
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
            Case Keys.R
                Me.Top = 0
                Me.Left = 0
        End Select
        Label1.Text = "( " & Me.Left & " , " & Me.Top & " )"
        e.IsInputKey = True
    End Sub

    Private Sub Form2_Show(sender As Object, e As EventArgs) Handles MyBase.Shown
        Form2_Shown()
    End Sub

    Private Sub Form2_Shown()
        Panel1.Top = (Me.Height / 2) - (Panel1.Height / 2)
        Panel1.Left = (Me.Width / 2) - (Panel1.Width / 2)
        Dim s As System.Windows.Forms.Screen = System.Windows.Forms.Screen.FromControl(Me)

        Dim x As Integer = s.Bounds.X
        Dim y As Integer = s.Bounds.Y
        Dim h As Integer = s.Bounds.Height
        Dim w As Integer = s.Bounds.Width
        Center(y, h, x, w)
        Label1.Text = "( " & Me.Left & " , " & Me.Top & " )"
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
            Label1.Text = "( " & Me.Left & " , " & Me.Top & " )"
        End If
        Dim s As System.Windows.Forms.Screen = System.Windows.Forms.Screen.FromControl(Me)
        Dim x As Integer = s.Bounds.X
        Dim y As Integer = s.Bounds.Y
        Form1.Label_hScreenRes.Text = s.Bounds.Height
        Form1.Label_wScreenRes.Text = s.Bounds.Width
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
        Next
        Center(Form1.By(N), Form1.Bh(N), Form1.Bx(N), Form1.Bw(N))
    End Sub

    Private Sub Center(By As Integer, Bh As Integer, Bx As Integer, Bw As Integer)
        Me.Top = ((Bh / 2) + By) - (Me.Height / 2)
        Me.Left = ((Bw / 2) + Bx) - (Me.Width / 2)

        Label1.Text = "( " & Me.Left & " , " & Me.Top & " )"
    End Sub

    Private Sub Button_Set_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form1.Label_xPos.Text = Me.Left
        Form1.Label_yPos.Text = Me.Top
        Form1.Label_xRes.Text = Me.Width
        Form1.Label_yRes.Text = Me.Height
        FormSet.Show()
        FormSet.Label1.Text = "Set screen size and position."
    End Sub

    Private Sub ComboBox_resolution_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_resolution.SelectedIndexChanged

        Dim S_Select As String = ComboBox_resolution.SelectedItem
        Dim S_Split() As String = Split(S_Select, "x")
        Form1.Label_xRes.Text = S_Split(0)
        Form1.Label_yRes.Text = S_Split(1)
        If Integer.Parse(Form1.Label_wScreenRes.Text) < Integer.Parse(Form1.Label_xRes.Text) Or Integer.Parse(Form1.Label_hScreenRes.Text) < Integer.Parse(Form1.Label_yRes.Text) Then
            Dim result As DialogResult = MessageBox.Show("It's bigger than the screen size.",
                                             "confirmation",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Exclamation,
                                            MessageBoxDefaultButton.Button2)
            If result = DialogResult.No Then
                ComboBox_resolution.SelectedIndex = Last_index
                Form1.ComboBox_resolution.SelectedIndex = ComboBox_resolution.SelectedIndex
                Exit Sub
            Else
                Form1.ComboBox_resolution.SelectedIndex = ComboBox_resolution.SelectedIndex
                Last_index = ComboBox_resolution.SelectedIndex
                Me.Width = Integer.Parse(Form1.Label_xRes.Text)
                Me.Height = Integer.Parse(Form1.Label_yRes.Text)
                Form2_Shown()
            End If
        Else
            Form1.ComboBox_resolution.SelectedIndex = ComboBox_resolution.SelectedIndex
            Last_index = ComboBox_resolution.SelectedIndex
            Me.Width = Integer.Parse(Form1.Label_xRes.Text)
            Me.Height = Integer.Parse(Form1.Label_yRes.Text)
            Form2_Shown()
        End If

    End Sub

    Private Sub ComboBox_resolution_Selected(sender As Object, e As EventArgs) Handles ComboBox_resolution.DropDownClosed
        Button3.Select()
    End Sub


    Private Sub ComboBox__MouseWheel(ByVal sender As Object, ByVal e As Windows.Forms.MouseEventArgs) Handles ComboBox_resolution.MouseWheel
        Dim eventArgs As HandledMouseEventArgs = DirectCast(e, HandledMouseEventArgs)
        eventArgs.Handled = True
    End Sub
End Class