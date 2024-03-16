Public Class FormSet
    Private Sub FormSet_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.TopMost = True

        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        Me.Close()
    End Sub
End Class