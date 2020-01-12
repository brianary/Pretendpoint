# Pester tests, see https://github.com/Pester/Pester/wiki
$envPath = $env:Path # avoid testingc the wrong cmdlets
Import-Module (Resolve-Path ./src/*/bin/Debug/*/*.psd1) -vb
$notAdmin = !([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).`
    IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
Describe 'Pretendpoint' {
    Context 'Pretendpoint module' {
        It "Given the Pretendpoint module, it should have a nonzero version" {
            $m = Get-Module Pretendpoint
            $m.Version |Should -Not -Be $null
            $m.Version.Major |Should -BeGreaterThan 0
        }
    }
    $http = @{}
    Context 'Start-HttpListener cmdlet' {
        It "Given a port <Port>, an HTTP listener is listening and a TCP socket may be established to that port." -TestCases @(
            7777,8080 |foreach {@{ Port = $_ }}
        ) {
            Param($Port)
            $http[$Port] = Start-HttpListener $Port
            $http[$Port].IsListening |Should -BeTrue
            $socket = New-Object Net.Sockets.Socket InterNetwork,Stream,Tcp
            $socket.Connect(0x7F000001,$Port)
            $socket.Connected |Should -BeTrue
            $socket.Close()
            $socket.Dispose()
        } -Skip:$notAdmin
     }
     Context 'Stop-HttpListener cmdlet' {
       It "Given an HTTP Listener bound to port <Port>, it should stop listening." -TestCases @(
           $http.Keys |foreach {@{ Port = $_; Listener = $http[$_] }}
       ) {
           Param($Port,$Listener)
           if(!$Listener.IsListening) {Set-ItResult -Inconclusive -Because "the HTTP listener isn't listening"}
           $Listener |Stop-HttpListener
           $Listener.IsListening |Should -BeFalse
       } -Skip:$notAdmin
    }
}
$env:Path = $envPath
