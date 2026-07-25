namespace Pretendpoint

open System
open System.Management.Automation
open System.Net

/// Restarts an HTTP listener.
[<Cmdlet(VerbsLifecycle.Restart, "HttpListener")>]
type RestartHttpListenerCommand () =
    inherit PSCmdlet ()

    /// The HTTP listener to restart.
    [<Parameter(Position=0,Mandatory=true,ValueFromPipeline=true)>]
    [<ValidateNotNullOrEmpty>]
    member val Listener : HttpListener = null with get, set

    /// Executes the cmdlet.
    static member internal Invoke (cmdlet:PSCmdlet) (listener:HttpListener) =
        if listener.IsListening then listener.Stop ()
        listener.Start ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        RestartHttpListenerCommand.Invoke x x.Listener
