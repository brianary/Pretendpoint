---
external help file: Pretendpoint.dll-Help.xml
Module Name: Pretendpoint
online version:
schema: 2.0.0
---

# Get-WebRequestBody

## SYNOPSIS
Starts an HTTP listener to receive a single request, whose body is returned, supporting static or dynamic respnoses.

## SYNTAX

### StaticResponse
```
Get-WebRequestBody [-Encoding <String>] [-Body <Object>] [-ResponseEncoding <String>] [-ContentType <String>]
 [-StatusCode <HttpStatusCode>] [-IncludeHeaders] [-Port] <Int32[]>
 [-AuthenticationSchemes <AuthenticationSchemes>] [-Realm <String>] [-IgnoreWriteExceptions]
 [<CommonParameters>]
```

### DynamicResponse
```
Get-WebRequestBody [-Encoding <String>] -Response <ScriptBlock> [-IncludeHeaders] [-Port] <Int32[]>
 [-AuthenticationSchemes <AuthenticationSchemes>] [-Realm <String>] [-IgnoreWriteExceptions]
 [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-WebRequestBody -Port 8080
```

Waits for a request to http://localhost:8080/ , prints out the request body, and returns an HTTP 204.

## PARAMETERS

### -Port
Ports on the localhost to bind to.

```yaml
Type: Int32[]
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -AuthenticationSchemes
Client authentication methods to support.

```yaml
Type: AuthenticationSchemes
Parameter Sets: (All)
Aliases:
Accepted values: None, Digest, Negotiate, Ntlm, IntegratedWindowsAuthentication, Basic, Anonymous

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Body
Data to send as a response body.

```yaml
Type: Object
Parameter Sets: StaticResponse
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -ContentType
Sets the content type of the response body.

```yaml
Type: String
Parameter Sets: StaticResponse
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Encoding
Forces an encoding for the request body; byte or hex for binary, others for text.

```yaml
Type: String
Parameter Sets: (All)
Aliases: Incoding
Accepted values: ascii, byte, utf16, utf16BE, utf32, utf32BE, utf7, utf8

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -IgnoreWriteExceptions
Indicates that response writes shouldn't generate exceptions.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Realm
The RFC2617 authentication realm.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Response
A script block that accepts an HttpListenerContext parameter to respond with.

```yaml
Type: ScriptBlock
Parameter Sets: DynamicResponse
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ResponseEncoding
Sets an encoding for the response body.

```yaml
Type: String
Parameter Sets: StaticResponse
Aliases: Outcoding
Accepted values: ascii, utf16, utf16BE, utf32, utf32BE, utf7, utf8

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -StatusCode
Sets the HTTP response status code.

```yaml
Type: HttpStatusCode
Parameter Sets: StaticResponse
Aliases:
Accepted values: Continue, SwitchingProtocols, Processing, EarlyHints, OK, Created, Accepted, NonAuthoritativeInformation, NoContent, ResetContent, PartialContent, MultiStatus, AlreadyReported, IMUsed, MultipleChoices, Ambiguous, MovedPermanently, Moved, Found, Redirect, SeeOther, RedirectMethod, NotModified, UseProxy, Unused, TemporaryRedirect, RedirectKeepVerb, PermanentRedirect, BadRequest, Unauthorized, PaymentRequired, Forbidden, NotFound, MethodNotAllowed, NotAcceptable, ProxyAuthenticationRequired, RequestTimeout, Conflict, Gone, LengthRequired, PreconditionFailed, RequestEntityTooLarge, RequestUriTooLong, UnsupportedMediaType, RequestedRangeNotSatisfiable, ExpectationFailed, MisdirectedRequest, UnprocessableEntity, Locked, FailedDependency, UpgradeRequired, PreconditionRequired, TooManyRequests, RequestHeaderFieldsTooLarge, UnavailableForLegalReasons, InternalServerError, NotImplemented, BadGateway, ServiceUnavailable, GatewayTimeout, HttpVersionNotSupported, VariantAlsoNegotiates, InsufficientStorage, LoopDetected, NotExtended, NetworkAuthenticationRequired

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -IncludeHeaders
Indicates that the HTTP request headers should be output.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Object

### System.Int32[]

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
