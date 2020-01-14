namespace Pretendpoint

open System
open System.Management.Automation
open System.Net

/// Closes an HTTP listener.
[<Cmdlet(VerbsLifecycle.Stop, "HttpListener")>]
type StopHttpListenerCommand () =
    inherit PSCmdlet ()

    /// The HTTP listener to close.
    [<Parameter(Position=0,Mandatory=true,ValueFromPipeline=true)>]
    [<ValidateNotNullOrEmpty>]
    member val Listener : HttpListener = null with get, set

    override x.ProcessRecord () =
        base.ProcessRecord ()
        x.Listener.Close ()
        (x.Listener :> IDisposable).Dispose ()
        sprintf "%A" x.Listener |> x.WriteVerbose
