Imports System.Reflection

Public Class ServiceBus
    Implements IServiceBus

    Private services As New Dictionary(Of Type, Object)()

    Public Function GetService(Of T)() As T Implements IServiceBus.GetService
        SyncLock services
            If services.ContainsKey(GetType(T)) Then
                Return services(GetType(T))
            End If
        End SyncLock
        Return Nothing
    End Function

    Public Function RegisterService(Of T)(service As T) As Boolean Implements IServiceBus.RegisterService
        SyncLock services
            If Not services.ContainsKey(GetType(T)) Then
                services.Add(GetType(T), service)
                Return True
            End If
        End SyncLock
        Return False
    End Function
End Class