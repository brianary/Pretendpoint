namespace Pretendpoint

open System
open System.Management.Automation
open System.Net

/// Listens for an HTTP request and returns an HTTP request & response.
[<Cmdlet(VerbsCommunications.Receive, "HttpContext")>]
type ReceieveHttpContextCommand () =
    inherit PSCmdlet ()

    /// The HTTP listener to receive the request through.
    [<Parameter(Position=0,Mandatory=true,ValueFromPipeline=true)>]
    [<ValidateNotNullOrEmpty>]
    member val Listener : HttpListener = null with get, set

    /// Executes the cmdlet.
    static member internal Invoke (cmdlet:PSCmdlet) (listener:HttpListener) =
        if not listener.IsListening then
            ErrorRecord (InvalidOperationException "The HTTP listener isn't listening.", "NOLISTEN",
                ErrorCategory.InvalidOperation, listener) |> cmdlet.ThrowTerminatingError
        listener.GetContext ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        ReceieveHttpContextCommand.Invoke x x.Listener
            |> x.WriteObject
