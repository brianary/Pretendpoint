---
external help file: Pretendpoint.dll-Help.xml
Module Name: Pretendpoint
online version: https://docs.microsoft.com/dotnet/api/system.net.httplistener
schema: 2.0.0
---

# Start-HttpListener

## SYNOPSIS
Creates and starts an HTTP listener, for testing HTTP clients.

## SYNTAX

```
Start-HttpListener [-Port] <Int32[]> [-AuthenticationSchemes <AuthenticationSchemes>] [-Realm <String>]
 [-IgnoreWriteExceptions] [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### EXAMPLE 1
```
$Listener = Start-HttpListener 8080
```

Creates and starts an HTTP listener at http://localhost:8080/

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32[]

## OUTPUTS

### System.Web.HttpListener to receive requests.
### System.Net.HttpListener

## NOTES

## RELATED LINKS

[https://docs.microsoft.com/dotnet/api/system.net.httplistener](https://docs.microsoft.com/dotnet/api/system.net.httplistener)

[https://tools.ietf.org/html/rfc2617#section-1.2](https://tools.ietf.org/html/rfc2617#section-1.2)

