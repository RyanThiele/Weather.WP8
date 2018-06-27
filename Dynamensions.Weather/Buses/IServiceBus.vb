Public Interface IServiceBus
    Function GetService(Of T)() As T
    Function RegisterService(Of T)(ByVal service As T) As Boolean
End Interface
