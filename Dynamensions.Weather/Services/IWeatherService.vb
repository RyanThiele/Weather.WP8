Imports System.Threading

Namespace Services

    Public Enum NoaaWeatherFormats
        Every12Hours
        Every24Hours
    End Enum

    Public Interface IWeatherService

        Function GetClosestWeatherSourceByZipCodeAsync(zipcode As String) As Task(Of WeatherSource)

        Function GetWeatherByDay(latitude As Decimal, longitude As Decimal, startDate As DateTime, numberOfDays As Integer, format As NoaaWeatherFormats, token As CancellationToken)

    End Interface

End Namespace
