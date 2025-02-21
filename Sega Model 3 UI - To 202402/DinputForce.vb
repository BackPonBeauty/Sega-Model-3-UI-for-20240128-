Public Class DinputForce
    Private Sub ConstLeftBar_Scroll(sender As Object, e As EventArgs) Handles ConstLeftBar.Scroll
        Label_ConstLeft.Text = ConstLeftBar.Value.ToString
    End Sub

    Private Sub ConsRightBar_Scroll(sender As Object, e As EventArgs) Handles ConsRightBar.Scroll
        Label_ConstRight.Text = ConsRightBar.Value.ToString
    End Sub

    Private Sub SelfBar_Scroll(sender As Object, e As EventArgs) Handles SelfBar.Scroll
        Label_Center.Text = SelfBar.Value.ToString
    End Sub

    Private Sub FrictionBar_Scroll(sender As Object, e As EventArgs) Handles FrictionBar.Scroll
        Label_Friction.Text = FrictionBar.Value.ToString
    End Sub

    Private Sub ViblateBar_Scroll(sender As Object, e As EventArgs) Handles ViblateBar.Scroll
        Label_Viblate.Text = ViblateBar.Value.ToString
    End Sub

    Private Sub Form3_Close(sender As Object, e As EventArgs) Handles MyBase.Closing
        Form1.DConstRight.Text = Label_ConstLeft.Text
        Form1.DConstLeft.Text = Label_ConstRight.Text
        Form1.DCenter.Text = Label_Center.Text
        Form1.DFriction.Text = Label_Friction.Text
        Form1.DViblate.Text = Label_Viblate.Text
    End Sub

    Private Sub DinputForce_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackColor = Color.FromArgb(255, Form1.Bgcolor_R, Form1.Bgcolor_G, Form1.Bgcolor_B)
        For Each c In Me.Controls
            c.ForeColor = Form1.Pub_Forecolor_s
        Next
        GetAllControls(Me, Integer.Parse(Form1.FontSize_bin))
    End Sub
    Private Sub GetAllControls(ByVal control As Control, size As Integer)
        If control.HasChildren Then
            For Each childControl As Control In control.Controls
                GetAllControls(childControl, size)
                'テキストボックスだけにアクセスしたい場合
                ' If TypeOf childControl Is TextBox Then
                childControl.Font = New Font("Cascadia Code", size, FontStyle.Regular)
                ' End If
            Next childControl
        End If
    End Sub
End Class