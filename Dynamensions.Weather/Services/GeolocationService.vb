Imports System.Net
Imports System.Xml.Linq
Imports System.IO
Imports <xmlns:ns="http://schemas.microsoft.com/search/local/ws/rest/v1">
Imports Dynamensions.Weather.Models

Namespace Services


    Public Class GeolocationService
        Implements IGeocodeService

        Private ReadOnly _locationQuery As String = "http://dev.virtualearth.net/REST/v1/Locations?CountryRegion={CountryRegion}&postalCode={PostalCode}&o=xml&key={Key}"
        Private ReadOnly _bingKey As String = "RoZKgybYllx3ZynXdmRT~4OZw7NQxOQD12Px6ABtAQw~Aos2iG9os1WvpjQc3tStdn_EC51Htk55kTfH0pGQnMPTDx0RHPn3NXfanttIByG8"


        Public Async Function GetLocationByPostalCodeAsync(postalCode As String) As Task(Of Location) Implements IGeocodeService.GetLocationByPostalCodeAsync
            Dim queryString As String = _locationQuery.Replace("{CountryRegion}", "US").Replace("{PostalCode}", postalCode).Replace("{Key}", _bingKey)
            Dim request As HttpWebRequest = HttpWebRequest.Create(queryString)
            Dim response As HttpWebResponse = Await request.GetResponseAsync()
            Dim location As Location = Nothing

            If response.StatusCode = HttpStatusCode.OK Then
                Dim reader As New StreamReader(response.GetResponseStream())
                Dim doc As XDocument = XDocument.Parse(Await reader.ReadToEndAsync())

                Dim statusCode As HttpStatusCode = CType([Enum].Parse(GetType(HttpStatusCode), doc.<ns:Response>.<ns:StatusCode>.Value, True), HttpStatusCode)
                If statusCode <> HttpStatusCode.OK Then Return Nothing

                ' get the results
                Dim records As Integer = doc.<ns:Response>.<ns:ResourceSets>.Count()
                If records > 0 Then
                    With doc.<ns:Response>.<ns:ResourceSets>.<ns:ResourceSet>.<ns:Resources>.<ns:Location>

                        location = New Location
                        location.Name = .<ns:Name>.Value
                        location.Point.Latitude = .<ns:Point>.<ns:Latitude>.Value.ToDecimal
                        location.Point.Longitude = .<ns:Point>.<ns:Longitude>.Value.ToDecimal
                        location.BoundingBox.SouthLatitude = .<ns:BoundingBox>.<ns:SouthLatitude>.Value.ToDecimal
                        location.BoundingBox.WestLongitude = .<ns:BoundingBox>.<ns:WestLongitude>.Value.ToDecimal
                        location.BoundingBox.NorthLatitude = .<ns:BoundingBox>.<ns:NorthLatitude>.Value.ToDecimal
                        location.BoundingBox.EastLongitude = .<ns:BoundingBox>.<ns:EastLongitude>.Value.ToDecimal
                        location.Address.StateOrRegion = .<ns:Address>.<ns:AdminDistrict>.Value
                        location.Address.County = .<ns:Address>.<ns:AdminDistrict2>.Value
                        location.Address.Country = .<ns:Address>.<ns:CountryRegion>.Value
                        location.Address.City = .<ns:Address>.<ns:Locality>.Value

                    End With
                End If
            End If

            Return location
        End Function

    End Class

End Namespace
