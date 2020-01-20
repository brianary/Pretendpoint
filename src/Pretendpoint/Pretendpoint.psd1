# see https://docs.microsoft.com/powershell/scripting/developer/module/how-to-write-a-powershell-module-manifest
# and https://docs.microsoft.com/powershell/module/microsoft.powershell.core/new-modulemanifest
@{
RootModule = 'Pretendpoint.dll'
ModuleVersion = '1.0.4'
CompatiblePSEditions = ,'Core'
GUID = '99dd8a26-0a66-4c4f-99e2-0a7e01e3af51'
Author = 'Brian Lalonde'
#CompanyName = 'Unknown'
Copyright = 'Copyright © 2020 Brian Lalonde'
Description = @'
Pretend Endpoint, the disposable web server.
A set of PowerShell cmdlets to create an HTTP test server/endpoint for inspecting or debugging client requests.
'@
PowerShellVersion = '6.0'
FunctionsToExport = @()
CmdletsToExport = @(
    'Get-WebRequestBody'
    'Read-WebRequest'
    'Receive-HttpContext'
    'Restart-HttpListener'
    'Start-HttpListener'
    'Stop-HttpListener'
    'Suspend-HttpListener'
    'Write-WebResponse'
)
VariablesToExport = @()
AliasesToExport = @()
FileList = @('Pretendpoint.dll','Pretendpoint.dll-Help.xml')
PrivateData = @{
    PSData = @{
        Tags = @('endpoint','webserver','webrequest','webresponse','web','server','http')
        LicenseUri = 'https://github.com/brianary/Pretendpoint/blob/master/LICENSE'
        ProjectUri = 'https://github.com/brianary/Pretendpoint/'
        IconUri = 'http://webcoder.info/images/Pretendpoint.svg'
        # ReleaseNotes = ''
    }
}
}
