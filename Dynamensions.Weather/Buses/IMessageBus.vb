Public Interface IMessageBus
    Sub Subscribe(Of TMessage)(handler As Action(Of TMessage))
    Sub Unsubscribe(Of TMessage)(handler As Action(Of TMessage))
    Sub Publish(Of TMessage)(message As TMessage)
End Interface
