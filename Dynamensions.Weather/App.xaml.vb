Imports System.Reflection
Imports Autofac
Imports Dynamensions.Weather.Services

Partial Public Class App
    Inherits Application

    ''' <summary>
    ''' Provides easy access to the root frame of the Phone Application.
    ''' </summary>
    ''' <returns>The root frame of the Phone Application.</returns>
    Public Property RootFrame As PhoneApplicationFrame

    ''' <summary>
    ''' Constructor for the Application object.
    ''' </summary>
    Public Sub New()
        ' Standard Silverlight initialization
        InitializeComponent()

        ' Phone-specific initialization
        InitializePhoneApplication()

        ' Show graphics profiling information while debugging.
        If System.Diagnostics.Debugger.IsAttached Then
            ' Display the current frame rate counters.
            Application.Current.Host.Settings.EnableFrameRateCounter = True

            ' Show the areas of the app that are being redrawn in each frame.
            'Application.Current.Host.Settings.EnableRedrawRegions = True

            ' Enable non-production analysis visualization mode, 
            ' which shows areas of a page that are handed off to GPU with a colored overlay.
            'Application.Current.Host.Settings.EnableCacheVisualization = True


            ' Disable the application idle detection by setting the UserIdleDetectionMode property of the
            ' application's PhoneApplicationService object to Disabled.
            ' Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
            ' and consume battery power when the user is not using the phone.
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled
        End If

        RegisterServices()
        AddHandler RootFrame.Navigated, AddressOf OnNavigated

        'Using db As New DbDataContext(DbDataContext.DBConnectionString)
        '    If Not db.DatabaseExists Then db.CreateDatabase()

        '    'db.Locations.InsertOnSubmit(New Location() With {.ZipCode = 10100, .City = "Test", .StateOrProvince = "Test", .StateOrProvinceAbbreviation = "Test", .County = "Test", .Latitude = 1, .Longitude = 2})
        '    'db.SubmitChanges()
        'End Using

        DatabaseHelper.MoveReferenceDatabase("Weather.sdf", False)

    End Sub

#Region "Navigation"

    Private Sub OnNavigated(sender As Object, e As NavigationEventArgs)
        If RootFrame.CurrentSource IsNot Nothing Then
            ' get the name of the view
            Dim view As Page = CType(RootFrame.Content, Page)
            Dim viewType As Type = e.Content.GetType
            Dim viewTypeName As String = viewType.ToString() & "Model"

            Dim viewModelType = System.Type.GetType(viewTypeName)
            Dim viewModel As ViewModelBase = Container.Resolve(viewModelType)

            view.DataContext = viewModel
            AddHandler view.Loaded, Async Sub(s, lea)
                                        Await viewModel.InitializeAsync()
                                    End Sub

        End If
    End Sub

#End Region

    Private Sub RegisterServices()

        Dim builder As New ContainerBuilder

        ' Services
        builder.RegisterType(Of NavigationService).As(Of INavigationService)()
        builder.RegisterType(Of Services.DialogService).As(Of IDialogService)()
        builder.RegisterType(Of MessageBus).As(Of IMessageBus).SingleInstance()
        builder.RegisterType(Of Services.SettingsService).As(Of ISettingsService)()
        builder.RegisterType(Of Services.WeatherService).As(Of IWeatherService)()

        ' ViewModels
        builder.RegisterType(GetType(MainViewModel))
        builder.RegisterType(GetType(AddWeatherSourceViewModel))
        builder.RegisterType(GetType(SettingsViewModel))
        'builder.RegisterType(GetType(ZipCodeListViewModel))

        Container = builder.Build()

    End Sub


    ' Code to execute when the application is launching (eg, from Start)
    ' This code will not execute when the application is reactivated
    Private Sub Application_Launching(ByVal sender As Object, ByVal e As LaunchingEventArgs)
#If DEBUG Then
        Dim settingService As ISettingsService = Container.Resolve(Of ISettingsService)()
        'Dim weatherSources As New List(Of WeatherSource)
        'For index = 1 To 10
        '    weatherSources.Add(New WeatherSource() With {
        '                       .ZipCode = "ZC" & index})
        'Next
        'settingService.SaveWeatherSourcesAsync(weatherSources)
#End If

        Dim navigationService = Container.Resolve(Of INavigationService)()
        navigationService.NavigateTo(Of MainViewModel)()
    End Sub

    ' Code to execute when the application is activated (brought to foreground)
    ' This code will not execute when the application is first launched
    Private Sub Application_Activated(ByVal sender As Object, ByVal e As ActivatedEventArgs)
    End Sub

    ' Code to execute when the application is deactivated (sent to background)
    ' This code will not execute when the application is closing
    Private Sub Application_Deactivated(ByVal sender As Object, ByVal e As DeactivatedEventArgs)
    End Sub

    ' Code to execute when the application is closing (eg, user hit Back)
    ' This code will not execute when the application is deactivated
    Private Sub Application_Closing(ByVal sender As Object, ByVal e As ClosingEventArgs)
    End Sub

    ' Code to execute if a navigation fails
    Private Sub RootFrame_NavigationFailed(ByVal sender As Object, ByVal e As NavigationFailedEventArgs)
        If Diagnostics.Debugger.IsAttached Then
            ' A navigation has failed; break into the debugger
            Diagnostics.Debugger.Break()
        End If
    End Sub

    Public Sub Application_UnhandledException(ByVal sender As Object, ByVal e As ApplicationUnhandledExceptionEventArgs) Handles Me.UnhandledException

        ' Show graphics profiling information while debugging.
        If Diagnostics.Debugger.IsAttached Then
            Diagnostics.Debugger.Break()
        Else
            e.Handled = True
            MessageBox.Show(e.ExceptionObject.Message & Environment.NewLine & e.ExceptionObject.StackTrace,
                            "Error", MessageBoxButton.OK)
        End If
    End Sub

#Region "Phone application initialization"
    ' Avoid double-initialization
    Private phoneApplicationInitialized As Boolean = False

    ' Do not add any additional code to this method
    Private Sub InitializePhoneApplication()
        If phoneApplicationInitialized Then
            Return
        End If


        ' Create the frame but don't set it as RootVisual yet; this allows the splash
        ' screen to remain active until the application is ready to render.
        RootFrame = New PhoneApplicationFrame()
        AddHandler RootFrame.Navigated, AddressOf CompleteInitializePhoneApplication

        ' Handle navigation failures
        AddHandler RootFrame.NavigationFailed, AddressOf RootFrame_NavigationFailed

        ' Ensure we don't initialize again
        phoneApplicationInitialized = True

    End Sub

    ' Do not add any additional code to this method
    Private Sub CompleteInitializePhoneApplication(ByVal sender As Object, ByVal e As NavigationEventArgs)

        Dim yy = CType(Application.Current, App).RootFrame.Source

        ' Set the root visual to allow the application to render
        If RootVisual IsNot RootFrame Then
            RootVisual = RootFrame
        End If

        ' Remove this handler since it is no longer needed
        RemoveHandler RootFrame.Navigated, AddressOf CompleteInitializePhoneApplication
    End Sub
#End Region

End Class