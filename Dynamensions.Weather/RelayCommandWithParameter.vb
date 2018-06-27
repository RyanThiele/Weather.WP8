Imports System.Diagnostics

Namespace Commands

    Public Class RelayCommandWithParameter
        Implements ICommand


#Region "Fields"

        Private ReadOnly _execute As Action(Of Object) = Nothing
        Private ReadOnly _canExecute As Func(Of Boolean) = Nothing

#End Region

#Region "Constructors"

        Public Sub New(execute As Action(Of Object))
            MyClass.New(execute, Nothing)
        End Sub

        ''' <summary>
        ''' Creates a new command.
        ''' </summary>
        ''' <param name="execute">The execution logic.</param>
        ''' <param name="canExecute">The execution status logic.</param>
        Public Sub New(execute As Action(Of Object), canExecute As Func(Of Boolean))

            If execute Is Nothing Then Throw New ArgumentNullException("execute")

            _execute = execute
            _canExecute = canExecute
        End Sub

#End Region

#Region "ICommand Members"

        <DebuggerStepThrough()>
        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Return If(_canExecute Is Nothing, True, _canExecute())
        End Function

        Public Event CanExecuteChanged(sender As Object, e As EventArgs) Implements ICommand.CanExecuteChanged

        Protected Sub OnCanExecuteChanged()
            RaiseEvent CanExecuteChanged(Me, New EventArgs)
        End Sub

        Public Sub Execute(parameter As Object) Implements ICommand.Execute
            _execute(parameter)
        End Sub

#End Region

    End Class

End Namespace