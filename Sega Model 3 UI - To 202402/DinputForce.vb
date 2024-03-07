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
End Class