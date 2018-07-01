Imports System.Threading

Namespace Services

    Public Enum NoaaWeatherFormats
        Every12Hours
        Every24Hours
    End Enum

    Public Interface IWeatherService

        Function GetWeatherByDay(point As Point, startDate As DateTime, numberOfDays As Integer, format As NoaaWeatherFormats, token As CancellationToken)

    End Interface

End Namespace
