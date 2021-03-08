Imports System.Threading

Module Module1

    Private myProcess As Process = New Process()
    Private elapsedTime As Integer
    Private eventHandled As Boolean

    Sub Main()
        Dim fileName As String = "BrowserHost.exe"
        Dim arguments As String = String.Empty

        elapsedTime = 0
        eventHandled = False

        Try
            myProcess.StartInfo.FileName = fileName
            myProcess.StartInfo.Arguments = arguments
            myProcess.StartInfo.CreateNoWindow = True
            myProcess.EnableRaisingEvents = True
            AddHandler myProcess.Exited, AddressOf MyProcess_Exited
            myProcess.Start()
        Catch ex As Exception
            Console.WriteLine($"An error occurred trying to print ""{fileName}"":\n" & ex.Message)
            Return
        End Try

        Const SLEEP_AMOUNT As Integer = 100
        Const TIMEOUT As Integer = 30000

        While (Not eventHandled)
            elapsedTime += SLEEP_AMOUNT
            If (elapsedTime > TIMEOUT) Then
                Console.WriteLine($"Exiting due to exceeding timeout of {TIMEOUT} ms.")
                Exit While
            End If
            Thread.Sleep(SLEEP_AMOUNT)
        End While

        Console.WriteLine("Press enter to exit")
        Console.ReadLine()
    End Sub

    Private Sub MyProcess_Exited(sender As Object, e As System.EventArgs)
        eventHandled = True
        Console.WriteLine($"Exit time:    {myProcess.ExitTime}")
        Console.WriteLine($"Exit code:    {myProcess.ExitCode}")
        Console.WriteLine($"Elapsed time: {elapsedTime}")
    End Sub
End Module
