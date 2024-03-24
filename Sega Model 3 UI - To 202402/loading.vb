Public Class loading
    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.TopMost = True
        Me.BackColor = Color.FromArgb(255, Form1.Bgcolor_R, Form1.Bgcolor_G, Form1.Bgcolor_B)
        Me.ForeColor = Form1.Pub_Forecolor_s
        Timer1.Enabled = True
        Button1.Visible = False
        GetAllControls(Me, Integer.Parse(Form1.FontSize_bin))
    End Sub
    Private Sub GetAllControls(ByVal control As Control, size As Integer)
        If control.HasChildren Then
            For Each childControl As Control In control.Controls
                GetAllControls(childControl, size)
                childControl.Font = New Font("Arial", size, FontStyle.Regular)
                ' End If
            Next childControl
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        Form1.BringToFront()
        If Process.GetProcessesByName("Supermodel").Count = 0 Then
            Label1.Text = "Failed"
            Button1.Visible = True
        Else
            Label1.Text = "Success!"
            Timer2.Enabled = True
        End If

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick

        Me.Close()
    End Sub
End Class