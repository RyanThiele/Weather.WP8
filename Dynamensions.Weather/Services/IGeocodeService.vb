Imports Dynamensions.Weather.Models
Imports System.Threading

Namespace Services

    Public Interface IGeocodeService

        Function GetLocationByPostalCodeAsync(postalCode As String, token As CancellationToken) As Task(Of Location)

    End Interface

End Namespace
