namespace Pretendpoint

open System
open System.Management.Automation
open System.Net
open System.Security.Principal

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

    /// Determines if the process is running as admin.
    static member internal IsAdministrator () =
        (WindowsIdentity.GetCurrent () |> WindowsPrincipal).IsInRole(WindowsBuiltInRole.Administrator)

    /// Executes the cmdlet.
    static member internal Invoke (cmdlet:PSCmdlet) (port:int array) (auth:AuthenticationSchemes)
        (realm:string) (ignoreWriteExceptions:bool) =
        if not HttpListener.IsSupported then
            let msg = sprintf "HTTP listeners are not supported on this OS (%s)" Environment.OSVersion.VersionString
            ErrorRecord (InvalidOperationException msg, "NOHTTP", ErrorCategory.InvalidOperation, Environment.OSVersion)
                |> cmdlet.ThrowTerminatingError
        if Environment.OSVersion.Platform = PlatformID.Win32NT && (not (StartHttpListenerCommand.IsAdministrator ())) then
            cmdlet.WriteWarning "Without running as admin, binding an HTTP listener to a port will likely fail."
        let listener = new HttpListener (AuthenticationSchemes=auth, IgnoreWriteExceptions=ignoreWriteExceptions)
        Seq.iter (sprintf "http://*:%d/" >> listener.Prefixes.Add) port
        listener.Start ()
        sprintf "%A" listener |> cmdlet.WriteVerbose
        cmdlet.WriteObject listener

    override x.ProcessRecord () =
        base.ProcessRecord ()
        StartHttpListenerCommand.Invoke x x.Port x.AuthenticationSchemes x.Realm x.IgnoreWriteExceptions.IsPresent
