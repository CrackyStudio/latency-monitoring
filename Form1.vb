Imports System.Net.NetworkInformation

Public Class Form1

    Protected ReadOnly ping As New Ping
    Protected latency As Long
    Protected address As String = "8.8.8.8"
    Protected niFont As New Font("Tahoma", 16, FontStyle.Regular, GraphicsUnit.Pixel)
    Protected niBrushColor As Color = Color.White
    Protected niBrush As New SolidBrush(niBrushColor)
    Protected bitmapText As Bitmap = New Bitmap(20, 20)
    Protected niGraphics As Graphics = Graphics.FromImage(bitmapText)
    Protected niIcon As IntPtr

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = FormBorderStyle.SizableToolWindow
        Me.ShowInTaskbar = False
    End Sub

    Private Sub NotifyIcon_Click(sender As Object, e As EventArgs) Handles NotifyIcon.Click
        Application.Exit()
    End Sub

    Private Sub Timer_Tick(sender As Object, e As EventArgs) Handles Timer.Tick
        latency = GetLatency(address)
        If latency > 0 Then
            NotifyIcon.Icon = CreateTextIcon(latency)
            NotifyIcon.Text = ""
        Else
            NotifyIcon.Icon = My.Resources.offline
            NotifyIcon.Text = "No connection"
        End If
    End Sub

    Protected Function GetLatency(ByRef hostNameOrAddress As String)
        Try
            Return ping.Send(hostNameOrAddress).RoundtripTime
        Catch ex As Exception
            Return -1
        End Try

    End Function

    Protected Function CreateTextIcon(ByRef msLatency As Long)
        PrepareNotifyIcon(msLatency)
        Return Icon.FromHandle(niIcon)
    End Function

    Protected Sub PrepareNotifyIcon(ByRef msLatency As Long)
        niBrushColor = SetBrushColor(msLatency)
        niBrush = New SolidBrush(niBrushColor)
        niGraphics.Clear(Color.Transparent)
        niGraphics.TextRenderingHint = Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit
        niGraphics.DrawString(Format(msLatency, "00"), niFont, niBrush, 0, 0)
        niIcon = bitmapText.GetHicon()
    End Sub

    Protected Function SetBrushColor(ByRef msLatency As Long)
        msLatency = SetMax(msLatency)
        Select Case msLatency
            Case <= 60
                Return Color.Lime
            Case > 60
                Return Color.Orange
            Case > 80
                Return Color.Red
            Case Else
                Return Color.White
        End Select
    End Function

    Protected Function SetMax(ByRef msLatency As Long)
        Select Case msLatency
            Case > 100
                Return 99
            Case Else
                Return msLatency
        End Select
    End Function
End Class
