# Pester tests, see https://github.com/Pester/Pester/wiki
Import-LocalizedData -BindingVariable manifest -BaseDirectory ./src/* -FileName (Split-Path $PWD -Leaf)
$psd1 = Resolve-Path ./src/*/bin/Debug/*/*.psd1
if(1 -lt ($psd1 |Measure-Object).Count) {throw "Too many module binaries found: $psd1"}
$module = Import-Module "$psd1" -PassThru -vb

$notAdmin = !([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).`
    IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

Describe $module.Name {
    Context "$($module.Name) module" -Tag Module {
        It "Given the module, the version should match the manifest version" {
            $module.Version |Should -BeExactly $manifest.ModuleVersion
        }
		It "Given the module, the DLL file version should match the manifest version" {
            (Get-Item "$($module.ModuleBase)\$($module.Name).dll").VersionInfo.FileVersionRaw |
                Should -BeLike "$($manifest.ModuleVersion)*"
		}
		It "Given the module, the DLL product version should match the manifest version" {
            (Get-Item "$($module.ModuleBase)\$($module.Name).dll").VersionInfo.ProductVersion |
                Should -BeLike "$($manifest.ModuleVersion)*"
		}
		It "Given the module, the raw DLL product version should match the manifest version" {
            (Get-Item "$($module.ModuleBase)\$($module.Name).dll").VersionInfo.ProductVersionRaw |
                Should -BeLike "$($manifest.ModuleVersion)*"
		} -Pending
		It "Given the module, the DLL should have a valid semantic product version" {
			$v = (Get-Item "$($module.ModuleBase)\$($module.Name).dll").VersionInfo.ProductVersion
			[semver]::TryParse($v, [ref]$null) |Should -BeTrue
		} -Pending
    }
    $http = @{}
    Context 'Start-HttpListener cmdlet' {
        It "Given a port <Port>, an HTTP listener is listening and a TCP socket may be established to that port" -TestCases @(
            7777,8080 |foreach {@{ Port = $_ }}
        ) {
            Param($Port)
            $http[$Port] = Pretendpoint\Start-HttpListener $Port
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
        It "Given HTTP Listener port <Port> and a request with 'User-Agent: <Data>', an HTTP context is returned" -TestCases $tests {
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
        It "Stops listening" -TestCases $tests {
            Param($Port,$Listener,$Data)
            if(!$Listener.IsListening) {Set-ItResult -Inconclusive -Because "the HTTP listener isn't listening"}
            $Listener |Suspend-HttpListener
            $Listener.IsListening |Should -BeFalse
        } -Skip:$notAdmin
    }
    Context 'Restart-HttpListener cmdlet' {
        It "Given a suspended HTTP Listener bound to port <Port>, it should start listening again" -TestCases $tests {
            Param($Port,$Listener,$Data)
            if($Listener.IsListening) {Set-ItResult -Inconclusive -Because "the HTTP listener is still listening"}
            $Listener |Restart-HttpListener
            $Listener.IsListening |Should -BeTrue
        } -Skip:$notAdmin
    }
    Context 'Read-WebRequest cmdlet' {
        It "Given an HTTP Listener bound to port <Port>, it should receive HTTP POST data '<Data>'" -TestCases $tests {
            Param($Port,$Listener,$Data)
            if(!$Listener.IsListening) {Set-ItResult -Inconclusive -Because "the HTTP listener isn't listening"}
            $iwr = Start-Process (Get-Process -Id $PID).Path '-nol','-noni','-nop','-c',
                "& { Invoke-WebRequest http://localhost:$Port/ -Method Post -ContentType text/plain -Body '$Data' }" -WindowStyle Hidden -PassThru
            $context = $Listener |Receive-HttpContext
            $context |Should -Not -BeNullOrEmpty
            $context |Read-WebRequest |Should -BeExactly $Data
            $context.Response.StatusCode = 204
            $context.Response.Close()
            Wait-Process -Id $iwr.Id -ErrorAction SilentlyContinue
        } -Skip:$notAdmin
        It "Given an HTTP Listener bound to port <Port>, it should receive HTTP POST data '<Data>' (binary)" -TestCases $tests {
            Param($Port,$Listener,$Data)
            if(!$Listener.IsListening) {Set-ItResult -Inconclusive -Because "the HTTP listener isn't listening"}
            $binData = ([guid]$Data).ToByteArray()
            $iwr = Start-Process (Get-Process -Id $PID).Path '-nol','-noni','-nop','-c',
                "& { Invoke-WebRequest http://localhost:$Port/ -Method Post -ContentType application/octet-stream -Body ([byte[]]($($binData -join ','))) }" -WindowStyle Hidden -PassThru
            $context = $Listener |Receive-HttpContext
            $context |Should -Not -BeNullOrEmpty
            [bool](compare $binData ($context |Read-WebRequest)) |Should -BeFalse
            $context.Response.StatusCode = 204
            $context.Response.Close()
            Wait-Process -Id $iwr.Id -ErrorAction SilentlyContinue
        } -Skip:$notAdmin
    }
    Context 'Write-WebResponse cmdlet' {
        It "Sending HTTP response data '<Data>'" -TestCases $tests {
            Param($Port,$Listener,$Data)
            if(!$Listener.IsListening) {Set-ItResult -Inconclusive -Because "the HTTP listener isn't listening"}
            $testdrive = (Get-PSDrive TestDrive).Root
            $iwr = Start-Process (Get-Process -Id $PID).Path '-nol','-noni','-nop','-c',
                "& { Invoke-WebRequest http://localhost:$Port/ -OutFile '$testdrive\testbody.txt' }" -WindowStyle Hidden -PassThru
            $context = $Listener |Receive-HttpContext
            $context |Should -Not -BeNullOrEmpty
            $Data |Write-WebResponse $context.Response
            Wait-Process -Id $iwr.Id -ErrorAction SilentlyContinue
            'TestDrive:\testbody.txt' |Should -FileContentMatch $Data
        } -Skip:$notAdmin
        It "Sending HTTP response data '<Data>' (binary)" -TestCases $tests {
            Param($Port,$Listener,$Data)
            if(!$Listener.IsListening) {Set-ItResult -Inconclusive -Because "the HTTP listener isn't listening"}
            $testdrive = (Get-PSDrive TestDrive).Root
            [byte[]] $binData = ([guid]$Data).ToByteArray()
            $iwr = Start-Process (Get-Process -Id $PID).Path '-nol','-noni','-nop','-c',
                "& { Invoke-WebRequest http://localhost:$Port/ -OutFile '$testdrive\testbody.dat' }" -WindowStyle Hidden -PassThru
            $context = $Listener |Receive-HttpContext
            $context |Should -Not -BeNullOrEmpty
            ,$binData |Write-WebResponse $context.Response
            Wait-Process -Id $iwr.Id -ErrorAction SilentlyContinue
            [byte[]] $response = Get-Content TestDrive:\testbody.dat -AsByteStream
            [bool](compare $binData $response) |Should -BeFalse
        } -Skip:$notAdmin
    }
    Context 'Stop-HttpListener cmdlet' {
        It "Stops listening, and not be startable" -TestCases $tests {
            Param($Port,$Listener,$Data)
            if(!$Listener.IsListening) {Set-ItResult -Inconclusive -Because "the HTTP listener isn't listening"}
            $Listener |Stop-HttpListener
            $Listener.IsListening |Should -BeFalse
            { $Listener.Start() } |Should -Throw 'Cannot access a disposed object.'
        } -Skip:$notAdmin
    }
    Context 'Get-WebRequestBody cmdlet' {
        It "Getting a request via port <Port> should receive HTTP POST data '<Data>'" -TestCases $tests {
            Param($Port,$Listener,$Data)
            $iwr = Start-Process (Get-Process -Id $PID).Path '-nol','-noni','-nop','-c',
                "& { Invoke-WebRequest http://localhost:$Port/ -Method Post -ContentType text/plain -Body '$Data' }" -WindowStyle Hidden -PassThru
            Get-WebRequestBody -Port $Port |Should -BeExactly $Data
            Wait-Process -Id $iwr.Id -ErrorAction SilentlyContinue
        } -Skip:$notAdmin
        It "Getting a request via port <Port> should receive HTTP POST data '<Data>' (binary)" -TestCases $tests {
            Param($Port,$Listener,$Data)
            $binData = ([guid]$Data).ToByteArray()
            $iwr = Start-Process (Get-Process -Id $PID).Path '-nol','-noni','-nop','-c',
                "& { Invoke-WebRequest http://localhost:$Port/ -Method Post -ContentType application/octet-stream -Body ([byte[]]($($binData -join ','))) }" -WindowStyle Hidden -PassThru
            [bool](compare $binData (Get-WebRequestBody -Port $Port)) |Should -BeFalse
            Wait-Process -Id $iwr.Id -ErrorAction SilentlyContinue
        } -Skip:$notAdmin
        It "Sending HTTP response data '<Data>' over port <Port> should be received" -TestCases $tests {
            Param($Port,$Listener,$Data)
            $iwr = Start-Process (Get-Process -Id $PID).Path '-nol','-noni','-nop','-c',
               "& { Import-Module '$(Resolve-Path ./src/*/bin/Debug/*/*.psd1)'; Get-WebRequestBody -Port $Port -StatusCode 200 -Body '$Data' }" -WindowStyle Hidden -PassThru
            Invoke-WebRequest http://localhost:$Port/ |Should -BeExactly $Data
            Wait-Process -Id $iwr.Id -ErrorAction SilentlyContinue
        } -Skip:$notAdmin
        # For some reason, these tests don't work, despite being functionally equivalent to tests above.
        # "SocketException: An existing connection was forcibly closed by the remote host"
        It "Sending HTTP response data '<Data>' (binary) over port <Port> should be received" -TestCases $tests {
            Param($Port,$Listener,$Data)
            [byte[]] $binData = ([guid]$Data).ToByteArray()
            $iwr = Start-Process (Get-Process -Id $PID).Path '-nol','-noni','-nop','-c',
               "& { Import-Module '$(Resolve-Path ./src/*/bin/Debug/*/*.psd1)'; [byte[]]@($($binData -join ',')) |Get-WebRequestBody -Port $Port -StatusCode 200; sleep 30; }" -WindowStyle Normal -PassThru
            Invoke-WebRequest http://localhost:$Port/ |Should -BeExactly $Data
            Wait-Process -Id $iwr.Id -ErrorAction SilentlyContinue
        } -Pending
        # "Unable to read data from the transport connection: An existing connection was forcibly closed by the remote host."
        It "Sending HTTP response data '<Data>' to port <Port> should be received" -TestCases $tests {
            Param($Port,$Listener,$Data)
            $iwr = Start-Process (Get-Process -Id $PID).Path '-nol','-noni','-nop','-c',
               "& { Invoke-WebRequest http://localhost:$Port/ -OutFile '$testdrive\testbody.txt' }" -WindowStyle Hidden -PassThru
            Get-WebRequestBody -Port $Port -StatusCode 200 -Body $Data
            Wait-Process -Id $iwr.Id -ErrorAction SilentlyContinue
            'TestDrive:\testbody.txt' |Should -FileContentMatch $Data
        } -Pending
        It "Sending HTTP response data '<Data>' (binary) to port <Port> should be received" -TestCases $tests {
            Param($Port,$Listener,$Data)
            [byte[]] $binData = ([guid]$Data).ToByteArray()
            $iwr = Start-Process (Get-Process -Id $PID).Path '-nol','-noni','-nop','-c',
                "& { Invoke-WebRequest http://localhost:$Port/ -OutFile '$testdrive\testbody.dat'; sleep 30; }" -WindowStyle Normal -PassThru
            ,$binData |Get-WebRequestBody -Port $Port -StatusCode 200
            Wait-Process -Id $iwr.Id -ErrorAction SilentlyContinue
            [byte[]] $response = Get-Content TestDrive:\testbody.dat -AsByteStream
            [bool](compare $binData $response) |Should -BeFalse
        } -Pending
    }
}.GetNewClosure()
$env:Path = $envPath
