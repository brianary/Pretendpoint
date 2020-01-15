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

    /// Executes the cmdlet.
    static member internal Invoke (cmdlet:PSCmdlet) (listener:HttpListener) =
        listener.Close ()
        (listener :> IDisposable).Dispose ()
        sprintf "%A" listener |> cmdlet.WriteVerbose

    override x.ProcessRecord () =
        base.ProcessRecord ()
        StopHttpListenerCommand.Invoke x x.Listener
