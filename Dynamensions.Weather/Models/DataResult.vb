''' <summary>
''' A call to retrieve or set data in the data service.
''' </summary>
''' <typeparam name="T">A generic type concerning the call.</typeparam>
''' <remarks>
''' Since there is no logic, the structure is used since it takes less resource to instantiate.
''' </remarks>
Public Structure DataResult(Of T)

    Public Sub New(result As T, Optional errorMessage As String = Nothing, Optional exception As Exception = Nothing)
        Me.Result = result
        Me.ErrorMessage = errorMessage
        Me.Exception = exception
        Me.IsSuccessful = String.IsNullOrWhiteSpace(errorMessage)
    End Sub

    ''' <summary>
    ''' The result of the data method.
    ''' </summary>
    Friend ReadOnly Result As T

    ''' <summary>
    ''' A variable dictating whether the call was successful.
    ''' </summary>
    Friend ReadOnly IsSuccessful As Boolean

    ''' <summary>
    ''' A friendly error message.
    ''' </summary>
    Friend ReadOnly ErrorMessage As String

    ''' <summary>
    ''' The exception for debugging purposes.
    ''' </summary>
    ''' <remarks></remarks>
    Friend ReadOnly Exception As Exception
End Structure