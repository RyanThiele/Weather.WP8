Imports System.Threading.Tasks

Public Interface ISettingsService
    Function LoadZipCodesAsync() As Task(Of IEnumerable(Of String))
    Function SaveZipCodesAsync(zipCodes As IEnumerable(Of String)) As Task
End Interface
