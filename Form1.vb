Imports System.Net.NetworkInformation

Public Class Form1

    Protected ReadOnly ping As New System.Net.NetworkInformation.Ping
    Protected latency As String
    Protected address As String = "8.8.8.8"
    Protected niFont As New Font("Tahoma", 16, FontStyle.Regular, GraphicsUnit.Pixel)
    Protected niBrush As New SolidBrush(Color.White)

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        latency = GetLatency(address)
        NotifyIcon1.Icon = CreateTextIcon(latency)
        NotifyIcon1.Text = latency
    End Sub

    Protected Function GetLatency(ByRef hostNameOrAddress As String)
        Return ping.Send(hostNameOrAddress).RoundtripTime
    End Function

    Protected Function CreateTextIcon(ByRef msLatency As String)
        If msLatency >= 100 Then
            msLatency = 99
        End If
        If msLatency <= 60 Then
            niBrush = New SolidBrush(Color.Lime)
        ElseIf msLatency > 60 AndAlso msLatency <= 80 Then
            niBrush = New SolidBrush(Color.Orange)
        ElseIf msLatency > 80 AndAlso msLatency < 100 Then
            niBrush = New SolidBrush(Color.Red)
        End If
        Dim bitmapText = New Bitmap(20, 20)
        Dim g = Graphics.FromImage(bitmapText)
        Dim hIcon
        g.Clear(Color.Transparent)
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit
        g.DrawString(msLatency, niFont, niBrush, 0, 0)
        hIcon = bitmapText.GetHicon()
        Return Icon.FromHandle(hIcon)
    End Function
End Class
