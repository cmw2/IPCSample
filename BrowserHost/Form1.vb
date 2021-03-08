'' This Sample Code is provided for the purpose of illustration only 
'' And Is Not intended to be used in a production environment.   

Imports System.IO
Imports System.IO.Pipes

Public Class Form1
    Private Sub WebBrowser1_Navigating(sender As Object, e As WebBrowserNavigatingEventArgs) Handles WebBrowser1.Navigating
        Const BEGINSYNC As String = "SYNC-BEGIN"
        Const ENDSYNC As String = "SYNC-END"

        If (WebBrowser1.Document IsNot Nothing) Then
            If (e.Url.ToString().EndsWith("#")) Then
                Dim userName As String = WebBrowser1.Document.GetElementById("username").GetAttribute("value")
                Dim pwd As String = WebBrowser1.Document.GetElementById("pwd").GetAttribute("value")
                Console.WriteLine($"userName: {userName}")
                Try
                    Dim pipeHandle As String = Environment.GetCommandLineArgs(2)
                    Using pipeClient As PipeStream =
                        New AnonymousPipeClientStream(PipeDirection.Out, pipeHandle)
                        Using sw As StreamWriter = New StreamWriter(pipeClient)
                            sw.AutoFlush = True
                            'Send a 'sync message' and wait for client to receive it.
                            Console.WriteLine("[CLIENT] Sending Begin Sync message")
                            sw.WriteLine(BEGINSYNC)
                            Console.WriteLine("[CLIENT] Waiting for pipe drain")
                            pipeClient.WaitForPipeDrain()
                            Console.WriteLine("[CLIENT] Sending User message")
                            sw.WriteLine($"User:{userName}")
                            Console.WriteLine("[CLIENT] Sending Password message")
                            sw.WriteLine($"Password:{pwd}")
                            Console.WriteLine("[CLIENT] Sending End Sync message")
                            sw.WriteLine(ENDSYNC)
                            Console.WriteLine("[CLIENT] Waiting for pipe drain")
                            pipeClient.WaitForPipeDrain()
                        End Using
                    End Using
                Catch ex As Exception
                    Console.WriteLine("Error opening PipeStream: " & ex.Message)
                End Try
            End If
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim curDir As String = Directory.GetCurrentDirectory()
        Dim htmlFile As String = Environment.GetCommandLineArgs(1)
        Dim url = $"file:///{curDir}/{htmlFile}"
        WebBrowser1.Url = New Uri(url)


    End Sub
End Class
