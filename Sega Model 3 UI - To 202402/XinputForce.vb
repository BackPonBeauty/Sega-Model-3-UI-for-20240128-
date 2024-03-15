Public Class XinputForce
    Private Sub Form4_Close(sender As Object, e As EventArgs) Handles MyBase.Closing
        Form1.XThreshold.Text = Label_XThreshold.Text
        Form1.XConst.Text = Label_XConstMax.Text
        Form1.XViblate.Text = Label_XViblate.Text
    End Sub

    Private Sub ThresholdBar_Scroll(sender As Object, e As EventArgs) Handles ThresholdBar.Scroll
        Label_XThreshold.Text = ThresholdBar.Value.ToString
    End Sub

    Private Sub ConstMaxBar_Scroll(sender As Object, e As EventArgs) Handles ConstMaxBar.Scroll
        Label_XConstMax.Text = ConstMaxBar.Value.ToString
    End Sub

    Private Sub ViblateBar_Scroll(sender As Object, e As EventArgs) Handles ViblateBar.Scroll
        Label_XViblate.Text = ViblateBar.Value.ToString
    End Sub

    Private Sub XinputForce_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
End Class