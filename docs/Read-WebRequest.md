---
external help file: Pretendpoint.dll-Help.xml
Module Name: Pretendpoint
online version: https://docs.microsoft.com/dotnet/api/system.net.httplistener
schema: 2.0.0
---

# Read-WebRequest

## SYNOPSIS
Parses an HTTP listener request.

## SYNTAX

```
Read-WebRequest [-Request] <HttpListenerRequest> [-Encoding <String>] [-IncludeHeaders] [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### EXAMPLE 1
```
PS C:\> Read-WebRequest $httpContext.Request
```

Parses the request body as a string or byte array.

## PARAMETERS

### -Request
The HTTP listener to receive the request through.

```yaml
Type: HttpListenerRequest
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Encoding
Forces an encoding for the request body; byte or hex for binary, others for text.

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

### System.Net.HttpListenerRequest

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS

[https://docs.microsoft.com/dotnet/api/system.net.httplistener](https://docs.microsoft.com/dotnet/api/system.net.httplistener)

