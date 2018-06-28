Imports System.IO.IsolatedStorage
Imports System.ComponentModel

Public Class DatabaseHelper

    Public Shared Sub MoveReferenceDatabase(databaseFileName As String)

        Dim iso = IsolatedStorageFile.GetUserStoreForApplication()

        Using input = Application.GetResourceStream(New Uri(databaseFileName, UriKind.Relative)).Stream
            Using output As IsolatedStorageFileStream = iso.CreateFile(databaseFileName)
                Dim readBuffer As Byte() = New Byte(4096) {}
                Dim bytesRead As Integer = -1

                bytesRead = input.Read(readBuffer, 0, readBuffer.Length)
                While bytesRead > 0
                    output.Write(readBuffer, 0, bytesRead)
                    bytesRead = input.Read(readBuffer, 0, readBuffer.Length)
                End While
            End Using
        End Using
    End Sub


    Public Shared Async Function MoveReferenceDatabaseAsync(databaseFileName As String) As Task
        Dim iso = IsolatedStorageFile.GetUserStoreForApplication()
        Using input = Application.GetResourceStream(New Uri(databaseFileName, UriKind.Relative)).Stream
            Using output As IsolatedStorageFileStream = iso.CreateFile(databaseFileName)
                Dim readBuffer As Byte() = New Byte(4096) {}
                Dim bytesRead As Integer = -1
                input.Seek(0, IO.SeekOrigin.Begin)

                bytesRead = Await input.ReadAsync(readBuffer, 0, readBuffer.Length)
                While bytesRead > 0
                    Await output.WriteAsync(readBuffer, 0, bytesRead)
                    bytesRead = Await input.ReadAsync(readBuffer, 0, readBuffer.Length)
                End While
            End Using
        End Using
    End Function

End Class
