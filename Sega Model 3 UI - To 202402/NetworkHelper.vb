' Copyright (C) 2026 BackPonBeauty
'
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with this program. If not, see <https://www.gnu.org/licenses/>.

Imports System.Net
Imports System.Net.Sockets

Public Class NetworkHelper
    Public Shared Function PunchHole(localPort As Integer, hostIP As String, hostPort As Integer) As UdpClient
        Try
            Dim client As New UdpClient(localPort)
            Dim hostEndpoint As New IPEndPoint(IPAddress.Parse(hostIP), hostPort)

            Dim dummyData As Byte() = {0, 0, 0, 0}
            For i As Integer = 1 To 3
                client.Send(dummyData, dummyData.Length, hostEndpoint)
            Next

            Debug.WriteLine($"[PunchHole] Port {localPort} → {hostIP}:{hostPort} Completed")
            Return client  ' Closeしない

        Catch ex As Exception
            Debug.WriteLine("[PunchHole] Failed: " & ex.Message)
            Return Nothing
        End Try
    End Function
End Class