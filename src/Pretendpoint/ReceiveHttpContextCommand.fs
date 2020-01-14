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

    override x.ProcessRecord () =
        base.ProcessRecord ()
        if not x.Listener.IsListening then
            ErrorRecord (InvalidOperationException "The HTTP listener isn't listening.", "NOLISTEN",
                ErrorCategory.InvalidOperation, x.Listener) |> x.ThrowTerminatingError
        x.Listener.GetContext () |> x.WriteObject
