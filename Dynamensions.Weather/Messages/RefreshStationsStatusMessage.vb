Public Class RefreshStationsStatusMessage
    Inherits StatusMessage

    Public Sub New(statusMessage As StatusMessage)
        Status = statusMessage.Status
        SubStatus = statusMessage.SubStatus
        Progress = statusMessage.Progress
    End Sub

End Class
