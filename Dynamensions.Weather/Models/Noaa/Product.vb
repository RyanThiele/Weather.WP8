Namespace Models.Noaa

    Public NotInheritable Class Product

        Private _operationalMode As OperationalModes
        ''' <summary>
        ''' The operational-mode defines the status of the product.
        ''' Applications can review the content of this element to determine if they should perform further processing.
        ''' Sample values include “test”, “developmental”, “experimental”, and “official” product. (R2.1.4)
        ''' </summary>
        Public Property OperationalMode() As OperationalModes
            Get
                Return _operationalMode
            End Get
            Friend Set(value As OperationalModes)
                _operationalMode = value
            End Set
        End Property

        Private _conciseName As String
        ''' <summary>
        ''' The concise-name represents a name or code that describes this product. 
        ''' The concise-nameType will have a list of names that is extensible to support secondary developer additions.
        ''' Sample values include "glance", "digital-tabular", "digital-zone" (Derived From R2.1.1).
        ''' </summary>
        Public Property ConciseName() As String
            Get
                Return _conciseName
            End Get
            Friend Set(value As String)
                _conciseName = value
            End Set
        End Property

        Private _srsName As String
        ''' <summary>
        ''' The srsName communicates the spatial reference system used by NDFD. 
        ''' The NDFD spatial reference system is the "WGS 1984".
        ''' </summary>
        Public Property SrsName() As String
            Get
                Return _srsName
            End Get
            Friend Set(value As String)
                _srsName = value
            End Set
        End Property

        Private _title As String
        ''' <summary>
        ''' The title provides a concise summarization of what this DWML product contains (R2.1.1).
        ''' </summary>
        Public Property Title() As String
            Get
                Return _title
            End Get
            Friend Set(value As String)
                _title = value
            End Set
        End Property

        Private _field As String
        ''' <summary>
        ''' The field specifies the general area within the environmental sciences that the data 
        ''' contained in the DWML instance is from. Example values include "meteorological", 
        ''' "hydrological", "oceanographical", "land surface", and "space" (R2.1.5).
        ''' </summary>
        Public Property Field() As String
            Get
                Return _field
            End Get
            Friend Set(value As String)
                _field = value
            End Set
        End Property

        Private _category As String
        ''' <summary>
        ''' The category defines the specific category that the product belongs to. 
        ''' Example values include "observation", "forecast", "analysis", and "statistic" (R2.1.6).
        ''' </summary>
        Public Property Category() As String
            Get
                Return _category
            End Get
            Friend Set(value As String)
                _category = value
            End Set
        End Property

        Private _creationDateTime As DateTime
        ''' <summary>
        ''' The creation-date/time contains the date and time that the product was prepared (R2.1.2).
        ''' </summary>
        Public Property CreationDateTime() As DateTime
            Get
                Return _creationDateTime
            End Get
            Set(ByVal value As DateTime)
                _creationDateTime = value
            End Set
        End Property

        Private _refreshFequency As TimeSpan
        ''' <summary>
        ''' The refresh frequency is used by the production center to help users know how often 
        ''' to return for updated data. In the case of the NDFD, the data is updated on an as 
        ''' needed basis. As a result the frequency provided may not always ensure users update 
        ''' as soon as new data is available. The frequency will also not guarantee that that 
        ''' when updates are done that the retrieved data is new. Still, the suggested refresh 
        ''' frequency will help well mannered users know what the provider believes is a 
        ''' reasonable time between repeated accesses of the system (R2.1.14).
        ''' </summary>
        Public Property RefreshFequency() As TimeSpan
            Get
                Return _refreshFequency
            End Get
            Set(ByVal value As TimeSpan)
                _refreshFequency = value
            End Set
        End Property

    End Class

End Namespace
