'' This Sample Code is provided for the purpose of illustration only 
'' And Is Not intended to be used in a production environment.   

Imports System.IO
Imports System.IO.Pipes

Module Module1

    Sub Main()
        Const browserHostFileName As String = "BrowserHost.exe"
        Const htmlFileName = "HTMLPage1.html"
        Const BEGINSYNC As String = "SYNC-BEGIN"
        Const ENDSYNC As String = "SYNC-END"

        Using pipeServer As AnonymousPipeServerStream = New AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.Inheritable)
            Dim arguments = htmlFileName & " " & pipeServer.GetClientHandleAsString()
            Dim startInfo As ProcessStartInfo = New ProcessStartInfo(browserHostFileName, arguments)
            startInfo.UseShellExecute = False
            Using browserHostProcess As Process = Process.Start(startInfo)
                browserHostProcess.EnableRaisingEvents = True
                pipeServer.DisposeLocalCopyOfClientHandle()
                Using sr As StreamReader = New StreamReader(pipeServer)
                    Dim temp As String
                    Console.WriteLine("[SERVER] Waiting for stream to start")
                    sr.Peek()

                    Console.WriteLine("[SERVER] Waiting for begin sync message")
                    Do
                        temp = sr.ReadLine()
                    Loop While (Not temp.StartsWith(BEGINSYNC))
                    Console.WriteLine("[SERVER] Found BEGINSYNC")

                    'Read the data
                    'Stop when find endsync
                    Do
                        temp = sr.ReadLine()
                        Console.WriteLine("[SERVER] Echo: " + temp)
                    Loop While (Not temp.StartsWith(ENDSYNC))
                    Console.WriteLine("[SERVER] Found ENDSYNC")
                    browserHostProcess.CloseMainWindow()
                End Using

            End Using
        End Using

        Console.WriteLine("Press enter to exit")
        Console.ReadLine()
    End Sub

End Module
