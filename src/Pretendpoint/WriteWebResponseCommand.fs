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

    override x.ProcessRecord () =
        base.ProcessRecord ()
        let data =
            match x.Body with
            | :? PSObject as o ->
                match o.BaseObject with
                | :? array<byte> as b ->
                    if isNull x.ContentType then x.Response.ContentType <- "application/octet-stream"
                    else x.Response.ContentType <- x.ContentType
                    b
                | :? string as s ->
                    if isNull x.ContentType then x.Response.ContentType <- "text/plain"
                    else x.Response.ContentType <- x.ContentType
                    x.Response.ContentEncoding <- ReadWebRequestCommand.GetEncoding x.Encoding
                    x.Response.ContentEncoding.GetBytes s
                | _ -> ErrorRecord (ArgumentException (sprintf "The response body may only be a string or byte array, got '%A'." (o.GetType()), "Body"),
                                    "BADTYPE", ErrorCategory.InvalidArgument, x.Body) |> x.ThrowTerminatingError; [||]
            | _ -> ErrorRecord (ArgumentException (sprintf "The response body may only be a string or byte array, got '%A'." (x.Body.GetType()), "Body"),
                                "BADTYPE", ErrorCategory.InvalidArgument, x.Body) |> x.ThrowTerminatingError; [||]
        x.Response.StatusCode <- int x.StatusCode
        x.Response.OutputStream.Write (ReadOnlySpan<byte> data)
        x.Response.OutputStream.Close ()
