Public Interface IWeatherService

    Function GetClosestWeatherSourceByZipCodeAsync(zipcode As String) As Task(Of WeatherSource)

End Interface
