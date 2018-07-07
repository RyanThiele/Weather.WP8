Imports System.Xml
Imports System.Xml.Linq
Imports System.Threading
Imports System.ComponentModel
Imports Dynamensions.Weather.Entities

Namespace Services

    Public Class WeatherService
        Implements IWeatherService


        Public Function GetLocationByPostalCodeAsync(postalCode As String, token As CancellationToken) As Task(Of Models.Location) Implements IWeatherService.GetLocationByPostalCodeAsync
            Dim tcs As New TaskCompletionSource(Of Models.Location)
            Dim model As Models.Location = Nothing
            Dim worker As New BackgroundWorker

            token.Register(AddressOf worker.CancelAsync)
            AddHandler worker.DoWork, Sub(s, e)
                                          Using db As New Entities.DbDataContext()
                                              Dim query = From location In db.Locations
                                                          Join station In db.Stations On station.Id Equals location.StationId
                                                          Where location.PostalCode.Equals(postalCode)
                                                          Select New Models.Location With {
                                                              .Address = New Models.Address With {
                                                                  .City = location.City,
                                                                  .Country = location.County,
                                                                  .County = location.County,
                                                                  .PostalCode = location.PostalCode,
                                                                  .StateOrRegion = location.StateOrProvince},
                                                              .WeatherStation = New Models.WeatherStation With {
                                                                  .Name = station.Station,
                                                                  .ICAO = station.ICAO,
                                                                  .IATA = station.IATA,
                                                                  .Latitude = station.Latitude,
                                                                  .Longitude = station.Longitude}}

                                              model = query.SingleOrDefault
                                          End Using
                                      End Sub

            AddHandler worker.RunWorkerCompleted, Sub(s, e)
                                                      tcs.TrySetResult(model)
                                                  End Sub

            worker.RunWorkerAsync()
            Return tcs.Task
        End Function

        Public Function GetStationByPostalCodeAsync(postalcode As String, token As CancellationToken) As Task(Of Station) Implements IWeatherService.GetStationByPostalCodeAsync
            Dim tcs As New TaskCompletionSource(Of Station)
            Dim worker As New BackgroundWorker
            Dim entity As Station = Nothing
            token.Register(AddressOf worker.CancelAsync)

            AddHandler worker.DoWork, Sub(s, e)
                                          Using db As New DbDataContext
                                              Dim query = From location In db.Locations
                                                          Join station In db.Stations On station.Id Equals location.StationId
                                                          Where location.PostalCode.Equals(postalcode)
                                                          Select station

                                              entity = query.SingleOrDefault
                                          End Using
                                      End Sub

            AddHandler worker.RunWorkerCompleted, Sub(s, e)
                                                      tcs.SetResult(entity)
                                                  End Sub

            worker.RunWorkerAsync()
            Return tcs.Task
        End Function
    End Class

End Namespace
