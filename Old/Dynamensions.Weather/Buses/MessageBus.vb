Public Class MessageBus
    Implements IMessageBus

    Private _subscribers As New Dictionary(Of Type, List(Of Object))

    Public Sub Publish(Of TMessage)(message As TMessage) Implements IMessageBus.Publish
        If Not _subscribers.ContainsKey(GetType(TMessage)) Then Return

        Dim handlers As List(Of Object) = _subscribers(GetType(TMessage))

        For i As Integer = 0 To handlers.Count - 1
            Dim handler As Action(Of TMessage) = CType(handlers(i), Action(Of TMessage))
            handler.Invoke(message)
        Next
    End Sub

    Public Sub Subscribe(Of TMessage)(handler As Action(Of TMessage)) Implements IMessageBus.Subscribe
        If _subscribers.ContainsKey(GetType(TMessage)) Then
            Dim handlers As List(Of Object) = _subscribers(GetType(TMessage))
            handlers.Add(handler)
        Else
            Dim handlers As New List(Of Object)
            handlers.Add(handler)
            _subscribers(GetType(TMessage)) = handlers
        End If
    End Sub

    Public Sub Unsubscribe(Of TMessage)(handler As Action(Of TMessage)) Implements IMessageBus.Unsubscribe
        If Not _subscribers.ContainsKey(GetType(TMessage)) Then Return

        Dim handlers As List(Of Object) = _subscribers(GetType(TMessage))
        handlers.Remove(handler)

        If handlers.Count = 0 Then _subscribers.Remove(GetType(TMessage))
    End Sub
End Class
