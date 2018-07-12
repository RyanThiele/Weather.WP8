Imports System.Threading
Imports Dynamensions.Weather.Models

Namespace Services

    Public Enum NoaaWeatherFormats
        Every12Hours
        Every24Hours
    End Enum

    Public Interface IWeatherService
        Function GetCurrentObservationByIcaoAsync(icao As String, token As CancellationToken) As Task(Of CurrentObservations)
        Function GetWeatherStationsByPostalCodeAsync(postalCode As String, numberOfStations As Integer, token As CancellationToken) As Task(Of IEnumerable(Of Models.WeatherStation))
    End Interface

End Namespace
