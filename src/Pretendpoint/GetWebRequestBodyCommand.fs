namespace Pretendpoint

open System
open System.Management.Automation
open System.Net

/// Starts an HTTP listener to receive a single request, whose body is returned, supporting static or dynamic respnoses.
[<Cmdlet(VerbsCommon.Get, "WebRequestBody")>]
type GetWebRequestBodyCommand () =
    inherit StartHttpListenerCommand ()

    /// Forces an encoding for the request body; Byte for binary, others for text.
    [<Parameter>]
    [<ValidateSet("ascii","byte","utf16","utf16BE","utf32","utf32BE","utf7","utf8")>]
    [<Alias("Incoding")>]
    member val Encoding : string = null with get, set

    /// Data to send as a response body.
    [<Parameter(ParameterSetName="StaticResponse",ValueFromPipeline=true)>]
    [<ValidateNotNullOrEmptyAttribute>]
    member val Body : obj = null with get, set

    /// Sets an encoding for the response body.
    [<Parameter(ParameterSetName="StaticResponse")>]
    [<ValidateSet("ascii","utf16","utf16BE","utf32","utf32BE","utf7","utf8")>]
    [<Alias("Outcoding")>] // ;)
    member val ResponseEncoding : string = "utf8" with get, set

    /// Sets the content type of the response body.
    [<Parameter(ParameterSetName="StaticResponse")>]
    [<ValidateNotNullOrEmptyAttribute>]
    member val ContentType : string = null with get, set

    /// Sets the HTTP response status code.
    [<Parameter(ParameterSetName="StaticResponse")>]
    member val StatusCode : HttpStatusCode = HttpStatusCode.NoContent with get, set

    /// A script block that accepts an HttpListenerContext parameter to respond with.
    [<Parameter(ParameterSetName="DynamicResponse", Mandatory=true)>]
    member val Response : ScriptBlock = null with get, set

    override x.ProcessRecord () =
        x.PSCmdletProcessRecord ()
        let http = StartHttpListenerCommand.Invoke x x.Port x.AuthenticationSchemes x.Realm x.IgnoreWriteExceptions.IsPresent
        sprintf "Listening: %A" http |> x.WriteVerbose
        let context = ReceieveHttpContextCommand.Invoke x http
        ReadWebRequestCommand.Invoke x context.Request x.Encoding |> x.WriteObject
        if x.ParameterSetName = "DynamicResponse" then
            x.Response.Invoke(context) |> ignore
            if context.Response.OutputStream.CanWrite then
                context.Response.OutputStream.Close ()
        else
            WriteWebResponseCommand.Invoke x context.Response x.Body x.ResponseEncoding x.ContentType x.StatusCode
        StopHttpListenerCommand.Invoke x http
