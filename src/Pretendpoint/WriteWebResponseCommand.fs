namespace Pretendpoint

open System
open System.Management.Automation
open System.Net

/// Sends a text or binary response body to the HTTP listener client.
[<Cmdlet(VerbsCommunications.Write, "WebResponse")>]
type WriteWebResponseCommand () =
    inherit PSCmdlet ()

    /// The HTTP response to send through.
    [<Parameter(Position=0,Mandatory=true)>]
    [<ValidateNotNullOrEmpty>]
    member val Response : HttpListenerResponse = null with get, set

    /// Data to send as a response body.
    [<Parameter(ValueFromPipeline=true)>]
    [<ValidateNotNullOrEmptyAttribute>]
    member val Body : obj = null with get, set

    /// Sets an encoding for the response body.
    [<Parameter>]
    [<ValidateSet("ascii","utf16","utf16BE","utf32","utf32BE","utf7","utf8")>]
    member val Encoding : string = "utf8" with get, set

    /// Sets the content type of the response body.
    [<Parameter>]
    [<ValidateNotNullOrEmptyAttribute>]
    member val ContentType : string = null with get, set

    /// Sets the HTTP response status code.
    [<Parameter>]
    member val StatusCode : HttpStatusCode = HttpStatusCode.OK with get, set

    /// Executes the cmdlet.
    static member internal Invoke (cmdlet:PSCmdlet) (response:HttpListenerResponse)
        (body:obj) (encoding:string) (contentType:string) (statusCode:HttpStatusCode) =
        let data =
            match body with
            | :? PSObject as o ->
                match o.BaseObject with
                | :? array<byte> as b ->
                    if isNull contentType then response.ContentType <- "application/octet-stream"
                    else response.ContentType <- contentType
                    b
                | :? string as s ->
                    if isNull contentType then response.ContentType <- "text/plain"
                    else response.ContentType <- contentType
                    response.ContentEncoding <- ReadWebRequestCommand.GetEncoding encoding
                    response.ContentEncoding.GetBytes s
                | _ -> ErrorRecord (ArgumentException (sprintf "The response body may only be a string or byte array, got '%A'." (o.GetType()), "Body"),
                                    "BADTYPE", ErrorCategory.InvalidArgument, body) |> cmdlet.ThrowTerminatingError; [||]
            | _ -> ErrorRecord (ArgumentException (sprintf "The response body may only be a string or byte array, got '%A'." (body.GetType()), "Body"),
                                "BADTYPE", ErrorCategory.InvalidArgument, body) |> cmdlet.ThrowTerminatingError; [||]
        response.StatusCode <- int statusCode
        response.OutputStream.Write (ReadOnlySpan<byte> data)
        response.OutputStream.Close ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        WriteWebResponseCommand.Invoke x x.Response x.Body x.Encoding x.ContentType x.StatusCode
