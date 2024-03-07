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
End Class