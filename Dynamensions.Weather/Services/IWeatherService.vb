Imports System.Threading
Imports Dynamensions.Weather.Models

Namespace Services

    Public Enum NoaaWeatherFormats
        Every12Hours
        Every24Hours
    End Enum

    Public Interface IWeatherService

        Function GetStationByPostalCodeAsync(postalcode As String, token As CancellationToken) As Task(Of Entities.Station)

        Function GetLocationByPostalCodeAsync(postalCode As String, token As CancellationToken) As Task(Of Models.Location)
        Function GetCurrentObservationByIataAsync(icao As String, token As CancellationToken) As Task(Of CurrentObserevations)


        'Function GetLocationByPostalCodeAsync(postalCode As String, token As CancellationToken) As Task(Of Location)
        'Function GetWeatherByDayAsync(postalCode As String, startDate As DateTime, numberOfDays As Integer, format As NoaaWeatherFormats, token As CancellationToken) As Noaa.WeatherByDay

    End Interface

End Namespace
