Imports System.IO

Public Class Form1
    Private Sub WebBrowser1_Navigating(sender As Object, e As WebBrowserNavigatingEventArgs) Handles WebBrowser1.Navigating
        If (WebBrowser1.Document IsNot Nothing) Then
            If (e.Url.ToString().EndsWith("#")) Then
                Dim userName As String = WebBrowser1.Document.GetElementById("username").GetAttribute("value")
                Dim pwd As String = WebBrowser1.Document.GetElementById("pwd").GetAttribute("value")
                Debug.WriteLine(userName)
                Debug.WriteLine(pwd)
            End If
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim curDir As String = Directory.GetCurrentDirectory()
        WebBrowser1.Url = New Uri(String.Format("file:///{0}/HTMLPage1.html", curDir))
    End Sub
End Class
