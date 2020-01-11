namespace Pretendpoint

open System
open System.Management.Automation // PowerShell attributes come from this namespace
open System.Net

/// Closes an HTTP listener.
[<Cmdlet(VerbsLifecycle.Stop, "HttpListener")>]
type StopHttpListenerCommand () =
    inherit PSCmdlet ()

    /// The HTTP listener to close.
    [<Parameter(Position=0,Mandatory=true,ValueFromPipeline=true)>]
    [<ValidateNotNullOrEmpty>]
    member val Listener : HttpListener = null with get, set

    override x.EndProcessing () =
        base.EndProcessing ()
        x.Listener.Close()
        (x.Listener :> IDisposable).Dispose()
        sprintf "%A" x.Listener |> x.WriteVerbose
