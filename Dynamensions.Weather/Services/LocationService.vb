Imports System.ComponentModel
Imports Dynamensions.Weather.Models

Namespace Services

    Public Class LocationService
        Implements ILocationService


        Public Function GetLocationByPostalCodeAsync(postalCode As String, numberOfStations As Integer, token As System.Threading.CancellationToken) As Task(Of Models.Location) Implements ILocationService.GetLocationByPostalCodeAsync
            Dim tcs As New TaskCompletionSource(Of Models.Location)
            Dim model As Models.Location = Nothing
            Dim worker As New BackgroundWorker

            token.Register(AddressOf worker.CancelAsync)
            AddHandler worker.DoWork, Sub(s, e)
                                          Using db As New Entities.DbDataContext()
                                              Dim locationQuery = From location In db.Locations
                                                          Join station In db.Stations On station.Id Equals location.StationId
                                                          Where location.PostalCode.Equals(postalCode)
                                                          Select New Models.Location With {
                                                              .Address = New Models.Address With {
                                                                  .City = location.City,
                                                                  .Country = location.County,
                                                                  .County = location.County,
                                                                  .PostalCode = location.PostalCode,
                                                                  .StateOrRegion = location.StateOrProvince},
                                                              .Point = New GeoCoordinate With {
                                                                  .Latitude = location.Latitude,
                                                                  .Longitude = location.Longitude}}

                                              model = locationQuery.SingleOrDefault


                                              Dim stationQuery = From station In db.Stations
                                                      Order By Math.Abs(Math.Abs(station.Latitude.Value) - Math.Abs(model.Point.Latitude)) + Math.Abs(Math.Abs(station.Longitude.Value) - Math.Abs(model.Point.Longitude)), station.Priority
                                                      Select New WeatherStation With {
                                                          .IATA = station.IATA,
                                                          .ICAO = station.ICAO,
                                                          .Latitude = station.Latitude,
                                                          .Longitude = station.Longitude,
                                                          .Name = station.Station}

                                              model.WeatherStations = stationQuery.Take(numberOfStations).ToList
                                          End Using
                                      End Sub

            AddHandler worker.RunWorkerCompleted, Sub(s, e)
                                                      tcs.TrySetResult(model)
                                                  End Sub

            worker.RunWorkerAsync()
            Return tcs.Task
        End Function

        Public Async Function GetLocationByLatitudeLongitudeAsync(latitude As Decimal, longitude As Decimal, numberOfStations As Integer, token As System.Threading.CancellationToken) As Task(Of Location) Implements ILocationService.GetLocationByLatitudeLongitudeAsync
            Dim closestPostalcode As String = Await DatabaseHelper.GetClosestPostalCodeByLatLongAsync(latitude, longitude)
            Return Await GetLocationByPostalCodeAsync(closestPostalcode, numberOfStations, token)
        End Function
    End Class
End Namespace
