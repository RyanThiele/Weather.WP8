Imports Dynamensions.Weather.Models

Namespace Services

    Public Interface IGeocodeService

        Function GetLocationByPostalCodeAsync(postalCode As String) As Task(Of Location)

    End Interface

End Namespace
