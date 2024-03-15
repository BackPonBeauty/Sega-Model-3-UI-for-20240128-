Public Class About
    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start("https://www.supermodel3.com/Forum/viewtopic.php?t=15")
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        System.Diagnostics.Process.Start("https://insertmorecoins.es/foro/supermodel/(hilo-oficial)-sega-model-3ui/")
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        System.Diagnostics.Process.Start("https://www.youtube.com/channel/UClAD0SfYiBMRdK_IieDXfaA")
    End Sub

    Private Sub About_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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