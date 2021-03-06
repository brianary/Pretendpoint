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
    [<Alias("Outcoding")>] // ;)
    member val Encoding : string = "utf8" with get, set

    /// Sets the content type of the response body.
    [<Parameter>]
    [<ValidateNotNullOrEmptyAttribute>]
    member val ContentType : string = null with get, set

    /// Sets the HTTP response status code.
    [<Parameter>]
    member val StatusCode : HttpStatusCode = enum 0 with get, set

    /// Get the Body data.
    static member internal GetData (contentType:string) (encoding:string) (body:obj) =
        match body with
        | null -> null, null, [||]
        | :? array<byte> as b -> (if isNull contentType then "application/octet-stream" else contentType), null, b
        | :? string as s ->
            let enc = ReadWebRequestCommand.GetEncoding encoding
            (if isNull contentType then "text/plain" else contentType), enc, (enc.GetBytes s)
        | :? PSObject as o -> WriteWebResponseCommand.GetData contentType encoding o.BaseObject
        | _ -> sprintf "The response body may only be a string or byte array, got '%A'." (body.GetType()) |> invalidArg "Body"

    /// Executes the cmdlet.
    static member internal Invoke (cmdlet:PSCmdlet) (response:HttpListenerResponse)
        (body:obj) (encoding:string) (contentType:string) (statusCode:HttpStatusCode) =
        cmdlet.WriteVerbose "Response:"
        let ctype, enc, data =
            try WriteWebResponseCommand.GetData contentType encoding body with
            | :? InvalidOperationException as ex ->
                ErrorRecord (ex, "BADBODY", ErrorCategory.InvalidArgument, body) |> cmdlet.ThrowTerminatingError
                null, null, null
        response.ContentType <- ctype
        response.ContentEncoding <- enc
        response.StatusCode <-
            if (int statusCode) = 0 then if data.Length = 0 then 204 else 200
            else int statusCode
        if response.StatusCode <> 204 then
            response.ContentLength64 <- int64 data.Length
            response.OutputStream.Write (ReadOnlySpan<byte> data)
        response.OutputStream.Close ()
        sprintf "HTTP/%A %d %s" response.ProtocolVersion response.StatusCode response.StatusDescription
            |> cmdlet.WriteVerbose
        Seq.iter cmdlet.WriteVerbose [for h in response.Headers -> sprintf "%s: %s" h response.Headers.[h]]

    override x.ProcessRecord () =
        base.ProcessRecord ()
        WriteWebResponseCommand.Invoke x x.Response x.Body x.Encoding x.ContentType x.StatusCode
