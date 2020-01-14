namespace Pretendpoint

open System
open System.Management.Automation
open System.Net

/// Creates and starts an HTTP listener, for testing HTTP clients.
[<Cmdlet(VerbsLifecycle.Start, "HttpListener")>]
[<OutputType(typeof<HttpListener>)>]
type StartHttpListenerCommand () =
    inherit PSCmdlet ()

    /// Ports on the localhost to bind to.
    [<Parameter(Position=0,Mandatory=true,ValueFromRemainingArguments=true)>]
    [<ValidateCount(1,2147483647)>]
    member val Port : int[] = [||] with get, set

    /// Client authentication methods to support.
    [<Parameter>]
    member val AuthenticationSchemes : AuthenticationSchemes = AuthenticationSchemes.Anonymous with get, set

    /// The RFC2617 authentication realm.
    [<Parameter>]
    member val Realm : string = null with get, set

    /// Indicates that response writes shouldn't generate exceptions.
    [<Parameter>]
    member val IgnoreWriteExceptions : SwitchParameter = (SwitchParameter false) with get, set

    override x.ProcessRecord () =
        base.ProcessRecord ()
        if not HttpListener.IsSupported then
            let msg = sprintf "HTTP listeners are not supported on this OS (%s)" Environment.OSVersion.VersionString
            ErrorRecord (InvalidOperationException msg, "NOHTTP", ErrorCategory.InvalidOperation, Environment.OSVersion)
                |> x.ThrowTerminatingError
        let listener = new HttpListener (AuthenticationSchemes=x.AuthenticationSchemes)
        Seq.iter (sprintf "http://*:%d/" >> listener.Prefixes.Add) x.Port
        listener.Start ()
        sprintf "%A" listener |> x.WriteVerbose
        x.WriteObject listener
