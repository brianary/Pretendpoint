namespace Pretendpoint

open System
open System.IO
open System.Management.Automation
open System.Net
open System.Net.Mime
open System.Text
open System.Text.RegularExpressions

/// Parses an HTTP listener request.
[<Cmdlet(VerbsCommunications.Read, "WebRequest")>]
type ReadWebRequestCommand () =
    inherit PSCmdlet ()

    /// The HTTP listener to receive the request through.
    [<Parameter(Position=0,Mandatory=true,ValueFromPipelineByPropertyName=true)>]
    [<ValidateNotNullOrEmpty>]
    member val Request : HttpListenerRequest = null with get, set

    /// Forces an encoding for the request body; Byte for binary, others for text.
    [<ValidateSet("ascii","byte","utf16","utf16BE","utf32","utf32BE","utf7","utf8")>]
    member val Encoding : string = null with get, set

    /// Reads a binary body from a request, with cmdlet integration.
    static member internal ReadBinaryData (cmdlet:PSCmdlet) (request:HttpListenerRequest) =
        if not request.HasEntityBody then [||]
        else
            if not request.InputStream.CanRead then
                ErrorRecord (InvalidOperationException "Unable to read HTTP request.", "NOREAD",
                    ErrorCategory.InvalidOperation, request) |> cmdlet.ThrowTerminatingError
            if request.ContentLength64 < 1L then
                cmdlet.WriteVerbose "Reading 1KB blocks"
                let readBlock (stream:Stream) =
                    cmdlet.WriteVerbose "Reading 1KB"
                    let block = Array.create 1024 0uy
                    if stream.Read(Span<byte> block) = 0 then None
                    else Some(block, stream)
                Seq.unfold readBlock request.InputStream |> Array.concat
            else
                sprintf "Reading %d bytes" request.ContentLength64 |> cmdlet.WriteVerbose
                let data = Array.create (int request.ContentLength64) 0uy
                if request.InputStream.Read(Span<byte> data) = 0 then
                    ErrorRecord (InvalidOperationException "No data read from HTTP request.", "ZEROREAD",
                        ErrorCategory.InvalidOperation, request) |> cmdlet.ThrowTerminatingError
                data

    /// Reads a text body from a request.
    static member internal ReadTextData (cmdlet:PSCmdlet) (request:HttpListenerRequest) (encoding:Encoding) =
        encoding.GetString(ReadWebRequestCommand.ReadBinaryData cmdlet request)

    /// Returns an encoding, given a PowerShell-idiomatic, simplified encoding name.
    static member internal GetEncoding name =
        Regex.Replace (name, @"\Autf", "utf-") |> Encoding.GetEncoding

    /// Executes the cmdlet.
    static member internal Invoke (cmdlet:PSCmdlet) (request:HttpListenerRequest) (encoding:string) =
        Seq.iter cmdlet.WriteVerbose [for h in request.Headers -> sprintf "%s: %s" h request.Headers.[h]]
        if isNull encoding then
            //TODO: multipart/alternative, multipart/parallel, multipart/related, multipart/form-data, multipart/*
            // https://stackoverflow.com/a/21689347/54323
            // https://docs.microsoft.com/dotnet/api/system.net.http.streamcontent
            let texty = Regex @"\A(?:(?:text|message)/.*|application/(?:json|(?:.*\+)xml))\z"
            let contentType = (ContentType request.ContentType).MediaType
            if texty.IsMatch contentType then
                ReadWebRequestCommand.ReadTextData cmdlet request request.ContentEncoding |> cmdlet.WriteObject
            else
                ReadWebRequestCommand.ReadBinaryData cmdlet request |> cmdlet.WriteObject
        else
            if encoding = "byte" then ReadWebRequestCommand.ReadBinaryData cmdlet request |> cmdlet.WriteObject
            else
                ReadWebRequestCommand.GetEncoding encoding
                    |> ReadWebRequestCommand.ReadTextData cmdlet request
                    |> cmdlet.WriteObject

    override x.ProcessRecord () =
        base.ProcessRecord ()
        ReadWebRequestCommand.Invoke x x.Request x.Encoding
