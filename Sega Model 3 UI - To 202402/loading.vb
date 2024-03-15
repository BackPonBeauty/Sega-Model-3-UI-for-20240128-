﻿Public Class loading
    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.TopMost = True
        Timer1.Enabled = True
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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        If Process.GetProcessesByName("Supermodel").Count = 0 Then
            Label1.Text = "Failed"
            Button1.Left = 28
        Else
            Label1.Text = "Success!"
            timer2.enabled = True
        End If

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Me.Close()
    End Sub
End Class