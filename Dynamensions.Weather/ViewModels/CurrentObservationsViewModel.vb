Imports Dynamensions.Weather.Services
Imports Dynamensions.Weather.Models

Public Enum TemerpatureDiplayUnits
    Celsius
    Fahrenheit
End Enum

Public Class CurrentObservationsViewModel
    Inherits ObservableObject

#Region "Properties"

#Region "CurrentObservations"

#Region "CurrentObservations"

    Dim _CurrentObservations As CurrentObservations
    Public Property CurrentObservations As CurrentObservations
        Get
            Return _CurrentObservations
        End Get
        Set(value As CurrentObservations)
            _CurrentObservations = value
            OnPropertyChanged("CurrentObservations")
        End Set
    End Property

#End Region


#End Region

#Region "IsUsingImperial"

    Dim _IsUsingImperial As Boolean
    Public Property IsUsingImperial As String
        Get
            Return _IsUsingImperial
        End Get
        Set(value As String)
            _IsUsingImperial = value
            OnPropertyChanged("IsUsingImperial")
        End Set
    End Property

#End Region

#End Region

#Region "Commands"
#End Region

End Class
