Pretendpoint
============

<!-- To publish to PowerShell Gallery: dotnet build -t:PublishModule -c Release -->
<!-- img src="Pretendpoint.svg" alt="Pretendpoint icon" align="right" / -->

[![Actions Status](https://github.com/brianary/Pretendpoint/workflows/.NET%20Core/badge.svg)](https://github.com/brianary/Pretendpoint/actions)

A minimal set of PowerShell cmdlets to use an HTTP test server/endpoint for inspecting or debugging client requests.

Cmdlets
-------

Documentation is automatically generated using [platyPS](https://github.com/PowerShell/platyPS).

- [Get-WebRequestBody](docs/Get-WebRequestBody.md)
- [Read-WebRequest](docs/Read-WebRequest.md)
- [Receive-HttpContext](docs/Receive-HttpContext.md)
- [Restart-HttpListener](docs/Restart-HttpListener.md)
- [Start-HttpListener](docs/Start-HttpListener.md)
- [Stop-HttpListener](docs/Stop-HttpListener.md)
- [Suspend-HttpListener](docs/Suspend-HttpListener.md)
- [Write-WebResponse](docs/Write-WebResponse.md)

Tests
-----

Tests are written for [Pester](https://github.com/Pester/Pester).

To run the tests, run `dotnet build -t:pester`.
