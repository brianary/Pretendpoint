---
external help file: Pretendpoint.dll-Help.xml
Module Name: Pretendpoint
online version:
schema: 2.0.0
---

# Restart-HttpListener

## SYNOPSIS
Stops and restarts an HTTP listener.

## SYNTAX

```
Restart-HttpListener [-Listener] <HttpListener> [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### EXAMPLE 1
```
PS C:\> Restart-HttpListener $http
```

The $http listener is stopped and restarted.

## PARAMETERS

### -Listener
The HTTP listener to stop and restart.

```yaml
Type: HttpListener
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Net.HttpListener to stop and restart.
### System.Net.HttpListener

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS

[https://docs.microsoft.com/dotnet/api/system.net.httplistener](https://docs.microsoft.com/dotnet/api/system.net.httplistener)

