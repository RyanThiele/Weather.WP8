Imports System.Xml
Imports System.Xml.Linq
Imports System.Threading

Namespace Services

    Public Class WeatherService
        Implements IWeatherService


        Public Async Function GetClosestWeatherSourceByZipCodeAsync(zipcode As String) As Task(Of WeatherSource) Implements IWeatherService.GetClosestWeatherSourceByZipCodeAsync

            Dim client As New WebClient
            Dim response As String = Await client.DownloadStringTaskAsync("https://graphical.weather.gov/xml/sample_products/browser_interface/ndfdXMLclient.php?listZipCodeList=" & String.Join("+", New List(Of String)({zipcode})))
            Dim document As XDocument = XDocument.Parse(response)

            Dim latLong As String = document.<dwml>.<latLonList>.Value

            Return New WeatherSource
        End Function


        Public Function GetWeatherByDay(latitude As Decimal, longitude As Decimal, startDate As Date, numberOfDays As Integer, format As NoaaWeatherFormats, token As CancellationToken) As Object Implements IWeatherService.GetWeatherByDay

        End Function
    End Class

End Namespace
