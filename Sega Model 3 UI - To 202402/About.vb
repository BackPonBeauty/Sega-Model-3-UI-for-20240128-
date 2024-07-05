Public Class About

    Private Sub About_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackColor = Color.FromArgb(255, Form1.Bgcolor_R, Form1.Bgcolor_G, Form1.Bgcolor_B)
        Me.ForeColor = Form1.Pub_Forecolor_s
        GetAllControls(Me, Integer.Parse(Form1.FontSize_bin))
    End Sub
    Private Sub GetAllControls(ByVal control As Control, size As Integer)
        If control.HasChildren Then
            For Each childControl As Control In control.Controls
                GetAllControls(childControl, size)
                'テキストボックスだけにアクセスしたい場合
                ' If TypeOf childControl Is TextBox Then
                childControl.Font = New Font("Arial", size, FontStyle.Regular)
                ' End If
            Next childControl
        End If
    End Sub

    Private Sub PictureBox5_Click(sender As Object, e As EventArgs)
        System.Diagnostics.Process.Start("https://discord.gg/TH6VHSXBUy")
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        System.Diagnostics.Process.Start("https://discord.gg/ajvbpQSs5g")
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        System.Diagnostics.Process.Start("https://twitter.com/back_pon_beauty")
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        System.Diagnostics.Process.Start("https://www.youtube.com/@backponbeauty")
    End Sub

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        System.Diagnostics.Process.Start("https://www.twitch.tv/back_pon_beauty")
    End Sub

    Private Sub PictureBox6_Click(sender As Object, e As EventArgs) Handles PictureBox6.Click
        System.Diagnostics.Process.Start("https://github.com/BackPonBeauty")
    End Sub

    Private Sub PictureBox7_Click(sender As Object, e As EventArgs)
        System.Diagnostics.Process.Start("https://github.com/trzy/Supermodel")
    End Sub

    Private Sub PictureBox8_Click(sender As Object, e As EventArgs) Handles PictureBox8.Click
        If My.Application.OpenForms("ponmi") IsNot Nothing Then
        Else
            ponmi.Show()
        End If
    End Sub

    Private Sub PictureBox9_Click(sender As Object, e As EventArgs)
        System.Diagnostics.Process.Start("https://supermodel3.com")
    End Sub
End Class