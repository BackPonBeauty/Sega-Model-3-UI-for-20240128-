' Copyright (C) 2026 BackPonBeauty
'
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with this program.  If not, see <https://www.gnu.org/licenses/>.

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
                childControl.Font = New Font("Cascadia Code", size, FontStyle.Regular)
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
            Form1.REC_F = False
            Form1.REP_F = False
            Form1.Button15.ForeColor = Color.Black
            Form1.Button16.ForeColor = Color.Black
        End If

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick

        Me.Close()
    End Sub
End Class