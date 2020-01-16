---
external help file: Pretendpoint.dll-Help.xml
Module Name: Pretendpoint
online version: https://docs.microsoft.com/dotnet/api/system.net.httplistener
schema: 2.0.0
---

# Write-WebResponse

## SYNOPSIS
Sends a text or binary response body to the HTTP listener client.

## SYNTAX

```
Write-WebResponse [-Response] <HttpListenerResponse> [-Body <Object>] [-Encoding <String>]
 [-ContentType <String>] [-StatusCode <HttpStatusCode>] [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### EXAMPLE 1
```
'Success' |Write-WebResponse $httpContext.Response
```

Sends the string "Success" to the HTTP listener client as text/plain.

### EXAMPLE 2
```
ConvertTo-Json $data |Write-WebResponse $httpContext.Response -ContentType application/json
```

Sends the JSON data to the HTTP listener client.

## PARAMETERS

### -Response
The HTTP response to send through.

```yaml
Type: HttpListenerResponse
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ContentType
Sets the content type of the response body.

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

### -Encoding
Sets an encoding for the response body.

```yaml
Type: String
Parameter Sets: (All)
Aliases: Outcoding
Accepted values: ascii, utf16, utf16BE, utf32, utf32BE, utf7, utf8

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
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -StatusCode
Sets the HTTP response status code.

```yaml
Type: HttpStatusCode
Parameter Sets: (All)
Aliases:
Accepted values: Continue, SwitchingProtocols, Processing, EarlyHints, OK, Created, Accepted, NonAuthoritativeInformation, NoContent, ResetContent, PartialContent, MultiStatus, AlreadyReported, IMUsed, MultipleChoices, Ambiguous, MovedPermanently, Moved, Found, Redirect, SeeOther, RedirectMethod, NotModified, UseProxy, Unused, TemporaryRedirect, RedirectKeepVerb, PermanentRedirect, BadRequest, Unauthorized, PaymentRequired, Forbidden, NotFound, MethodNotAllowed, NotAcceptable, ProxyAuthenticationRequired, RequestTimeout, Conflict, Gone, LengthRequired, PreconditionFailed, RequestEntityTooLarge, RequestUriTooLong, UnsupportedMediaType, RequestedRangeNotSatisfiable, ExpectationFailed, MisdirectedRequest, UnprocessableEntity, Locked, FailedDependency, UpgradeRequired, PreconditionRequired, TooManyRequests, RequestHeaderFieldsTooLarge, UnavailableForLegalReasons, InternalServerError, NotImplemented, BadGateway, ServiceUnavailable, GatewayTimeout, HttpVersionNotSupported, VariantAlsoNegotiates, InsufficientStorage, LoopDetected, NotExtended, NetworkAuthenticationRequired

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String containing the text to return to the HTTP client.
### System.Object
## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
