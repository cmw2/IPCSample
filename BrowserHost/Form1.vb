Imports System.IO

Public Class Form1
    Private Sub WebBrowser1_Navigating(sender As Object, e As WebBrowserNavigatingEventArgs) Handles WebBrowser1.Navigating
        Debug.WriteLine(e.Url)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim curDir As String = Directory.GetCurrentDirectory()
        WebBrowser1.Url = New Uri(String.Format("file:///{0}/HTMLPage1.html", curDir))
    End Sub
End Class
