namespace Pretendpoint

open System
open System.Management.Automation
open System.Net

/// Restarts an HTTP listener.
[<Cmdlet(VerbsLifecycle.Suspend, "HttpListener")>]
type SuspendHttpListenerCommand () =
    inherit PSCmdlet ()

    /// The HTTP listener to restart.
    [<Parameter(Position=0,Mandatory=true,ValueFromPipeline=true)>]
    [<ValidateNotNullOrEmpty>]
    member val Listener : HttpListener = null with get, set

    override x.ProcessRecord () =
        base.ProcessRecord ()
        x.Listener.Stop ()
