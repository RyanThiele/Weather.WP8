Option Explicit On
Option Strict On

Imports System.Xml
Imports System.Xml.Linq
Imports System.Threading
Imports System.ComponentModel
Imports Dynamensions.Weather.Entities
Imports Dynamensions.Weather.Models

Namespace Services

    Public Class WeatherService
        Implements IWeatherService


        ' TODO: Check if needed.
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

        ' TODO: Check if needed.
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


        Public Async Function GetCurrentObservationByIcaoAsync(icao As String, token As CancellationToken) As Task(Of CurrentObservations) Implements IWeatherService.GetCurrentObservationByIcaoAsync
            Dim url As String = "https://w1.weather.gov/xml/current_obs/" & icao & ".xml"
            Dim client As New WebClient

            token.Register(AddressOf client.CancelAsync)
            Dim feed As String = Await client.DownloadStringTaskAsync(url)

            If String.IsNullOrWhiteSpace(feed) Then Return Nothing

            Dim doc As XDocument = XDocument.Parse(feed)
            Dim root As XElement = doc.<current_observation>.SingleOrDefault

            If root Is Nothing Then Return Nothing

            Dim model As New CurrentObservations

            Dim refreshInterval As Integer
            Dim latitude, longitude As Decimal
            Integer.TryParse(root.<suggested_pickup_period>.Value, refreshInterval)
            Decimal.TryParse(root.<latitude>.Value, latitude)
            Decimal.TryParse(root.<longitude>.Value, longitude)

            model.RefreshTime = TimeSpan.FromMinutes(refreshInterval)
            model.Location = root.<location>.SingleOrDefault.ValueIfExists
            model.StationId = root.<station_id>.SingleOrDefault.ValueIfExists
            model.Latitude = root.<latitude>.SingleOrDefault.ValueIfExists.ToDecimal
            model.Longitude = root.<longitude>.SingleOrDefault.ValueIfExists.ToDecimal
            model.UpdatedTime = root.<observation_time_rfc822>.SingleOrDefault.ValueIfExists.FromRfc22StringToDateTime
            model.UpdatedTimeString = root.<observation_time>.SingleOrDefault.ValueIfExists
            model.Weather = root.<weather>.SingleOrDefault.ValueIfExists
            model.TemperatureString = root.<temperature_string>.SingleOrDefault.ValueIfExists
            model.TemperatureF = root.<temp_f>.SingleOrDefault.ValueIfExists.ToDecimal
            model.TemperatureC = root.<temp_c>.SingleOrDefault.ValueIfExists.ToDecimal
            model.RelativeHumidity = root.<relative_humidity>.SingleOrDefault.ValueIfExists.ToInteger
            model.WindString = root.<wind_string>.SingleOrDefault.ValueIfExists
            model.WindDirection = root.<wind_dir>.SingleOrDefault.ValueIfExists
            model.WindDegrees = root.<wind_degrees>.SingleOrDefault.ValueIfExists.ToInteger
            model.WindMph = root.<wind_mph>.SingleOrDefault.ValueIfExists.ToDecimal
            model.WindKnots = root.<wind_kt>.SingleOrDefault.ValueIfExists.ToDecimal
            model.PressureIn = root.<pressure_in>.SingleOrDefault.ValueIfExists.ToDecimal
            model.DewPointString = root.<dewpoint_string>.SingleOrDefault.ValueIfExists
            model.DewPointF = root.<dewpoint_f>.SingleOrDefault.ValueIfExists.ToDecimal
            model.DewPointC = root.<dewpoint_c>.SingleOrDefault.ValueIfExists.ToDecimal
            model.HeatIndexString = root.<head_index_string>.SingleOrDefault.ValueIfExists
            model.HeatIndexF = root.<head_index_f>.SingleOrDefault.ValueIfExists.ToDecimal
            model.HeatIndexC = root.<head_index_c>.SingleOrDefault.ValueIfExists.ToDecimal
            model.VisibilityMi = root.<visibility_mi>.SingleOrDefault.ValueIfExists.ToDecimal
            model.Icon = New Uri(root.<image>.<url>.SingleOrDefault.ValueIfExists)


            Return model
        End Function

        Public Async Function GetLocationByLatitudeLongitudeAsync(latitude As Decimal, longitude As Decimal, token As CancellationToken) As Task(Of Models.Location) Implements IWeatherService.GetLocationByLatitudeLongitudeAsync
            Dim closestPostalcode As String = Await DatabaseHelper.GetClosestPostalCodeByLatLongAsync(latitude, longitude)
            Return Await GetLocationByPostalCodeAsync(closestPostalcode, token)
        End Function
    End Class

End Namespace
