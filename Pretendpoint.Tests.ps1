# Pester tests, see https://github.com/Pester/Pester/wiki
$envPath = $env:Path # avoid testing the wrong cmdlets
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
    $tests = $http.Keys |foreach {@{ Port = $_; Listener = $http[$_]; Data = "$(New-Guid)" }}
    Context 'Receive-HttpContext cmdlet' {
        It "Given an HTTP Listener bound to port <Port>, and a request with a user agent of '<Data>', an HTTP context for that request is returned." -TestCases $tests {
            Param($Port,$Listener,$Data)
            if(!$Listener.IsListening) {Set-ItResult -Inconclusive -Because "the HTTP listener isn't listening"}
            Start-Process (Get-Process -Id $PID).Path '-nol','-noni','-nop','-c',
                "& { Invoke-WebRequest http://localhost:$Port/ -UserAgent '$Data' }" -WindowStyle Hidden
            $context = $Listener |Receive-HttpContext
            $context |Should -Not -BeNullOrEmpty
            $context.Request.UserAgent |Should -BeExactly $Data
            $context.Response.StatusCode = 204
            $context.Response.Close()
        } -Skip:$notAdmin
    }
    Context 'Suspend-HttpListener cmdlet' {
        It "Given an HTTP Listener bound to port <Port>, it should stop listening." -TestCases $tests {
            Param($Port,$Listener,$Data)
            if(!$Listener.IsListening) {Set-ItResult -Inconclusive -Because "the HTTP listener isn't listening"}
            $Listener |Suspend-HttpListener
            $Listener.IsListening |Should -BeFalse
        } -Skip:$notAdmin
    }
    Context 'Restart-HttpListener cmdlet' {
        It "Given a suspended HTTP Listener bound to port <Port>, it should start listening again." -TestCases $tests {
            Param($Port,$Listener,$Data)
            if($Listener.IsListening) {Set-ItResult -Inconclusive -Because "the HTTP listener is still listening"}
            $Listener |Restart-HttpListener
            $Listener.IsListening |Should -BeTrue
        } -Skip:$notAdmin
    }
    Context 'Read-WebRequest cmdlet' {
        It "Given an HTTP Listener bound to port <Port>, it should receive HTTP POST data '<Data>'." -TestCases $tests {
            Param($Port,$Listener,$Data)
            if(!$Listener.IsListening) {Set-ItResult -Inconclusive -Because "the HTTP listener isn't listening"}
            Start-Process (Get-Process -Id $PID).Path '-nol','-noni','-nop','-c',
                "& { Invoke-WebRequest http://localhost:$Port/ -Body '$Data' -ContentType text/plain }"  -WindowStyle Hidden
            $context = $Listener |Receive-HttpContext
            $context |Should -Not -BeNullOrEmpty
            $context |Read-WebRequest |Should -BeExactly $Data
            $context.Response.StatusCode = 204
            $context.Response.Close()
        } -Skip:$notAdmin
        It "Given an HTTP Listener bound to port <Port>, it should receive HTTP POST data '<Data>' (binary)." -TestCases $tests {
            Param($Port,$Listener,$Data)
            if(!$Listener.IsListening) {Set-ItResult -Inconclusive -Because "the HTTP listener isn't listening"}
            $binData = ([guid]$Data).ToByteArray()
            Start-Process (Get-Process -Id $PID).Path '-nol','-noni','-nop','-c',
                "& { Invoke-WebRequest http://localhost:$Port/ -Body ([byte[]]($($binData -join ','))) -ContentType application/octet-stream }" -WindowStyle Hidden
            $context = $Listener |Receive-HttpContext
            $context |Should -Not -BeNullOrEmpty
            [bool](compare $binData ($context |Read-WebRequest)) |Should -BeFalse
            $context.Response.StatusCode = 204
            $context.Response.Close()
        } -Skip:$notAdmin
    }
    Context 'Stop-HttpListener cmdlet' {
        It "Given an HTTP Listener bound to port <Port>, it should stop listening, and not be startable." -TestCases $tests {
            Param($Port,$Listener,$Data)
            if(!$Listener.IsListening) {Set-ItResult -Inconclusive -Because "the HTTP listener isn't listening"}
            $Listener |Stop-HttpListener
            $Listener.IsListening |Should -BeFalse
            { $Listener.Start() } |Should -Throw 'Cannot access a disposed object.'
        } -Skip:$notAdmin
    }
}
$env:Path = $envPath
