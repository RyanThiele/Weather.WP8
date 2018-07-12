Imports System.Threading

Namespace Services

    Public Interface ILocationService
        Function GetLocationByPostalCodeAsync(postalCode As String, numberOfStations As Integer, token As CancellationToken) As Task(Of Models.Location)
        Function GetLocationByLatitudeLongitudeAsync(latitude As Decimal, longitude As Decimal, numberOfStations As Integer, token As CancellationToken) As Task(Of Models.Location)
    End Interface

End Namespace
