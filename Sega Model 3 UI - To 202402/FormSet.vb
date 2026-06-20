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

Public Class FormSet
    Private Sub FormSet_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.TopMost = True
        Me.BackColor = Color.FromArgb(255, Form1.Bgcolor_R, Form1.Bgcolor_G, Form1.Bgcolor_B)
        Me.Label1.ForeColor = Form1.Pub_Forecolor_s
        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        Me.Close()
    End Sub
End Class