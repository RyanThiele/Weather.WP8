Imports System.Data.Linq
Imports System.Data.Linq.Mapping
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Threading
Imports System


Namespace Entities

    Public Class DbDataContext
        Inherits DataContext

        Public Shared DBConnectionString As String = "Data Source=isostore:/Weather.sdf"

        Public Sub New()
            MyClass.New(DBConnectionString)
        End Sub

        Public Sub New(connectionString As String)
            MyBase.New(connectionString)
        End Sub

        ' Specify tables.
        Public Locations As Table(Of Location)
        Public Stations As Table(Of Station)

        Public Function SubmitChangesAsync() As Task
            Dim tcs As New TaskCompletionSource(Of Object)
            Dim worker As New BackgroundWorker

            AddHandler worker.DoWork, Sub(s, e)
                                          SubmitChanges()
                                      End Sub

            AddHandler worker.RunWorkerCompleted, Sub(s, e)
                                                      tcs.SetResult(Nothing)
                                                  End Sub

            worker.RunWorkerAsync()
            Return tcs.Task
        End Function

        Friend Function DeleteDatabaseAsync() As Task
            Dim tcs As New TaskCompletionSource(Of Object)
            Dim worker As New BackgroundWorker

            AddHandler worker.DoWork, Sub(s, e)
                                          DeleteDatabase()
                                      End Sub

            AddHandler worker.RunWorkerCompleted, Sub(s, e)
                                                      tcs.SetResult(Nothing)
                                                  End Sub

            worker.RunWorkerAsync()
            Return tcs.Task
        End Function

        Friend Function CreateDatabaseAsync() As Task
            Dim tcs As New TaskCompletionSource(Of Object)
            Dim worker As New BackgroundWorker

            AddHandler worker.DoWork, Sub(s, e)
                                          CreateDatabase()
                                      End Sub

            AddHandler worker.RunWorkerCompleted, Sub(s, e)
                                                      tcs.SetResult(Nothing)
                                                  End Sub

            worker.RunWorkerAsync()
            Return tcs.Task
        End Function

    End Class

End Namespace
