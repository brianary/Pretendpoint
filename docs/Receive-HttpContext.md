---
external help file: Pretendpoint.dll-Help.xml
Module Name: Pretendpoint
online version:
schema: 2.0.0
---

# Receive-HttpContext

## SYNOPSIS
Listens for an HTTP request and returns an HTTP request & response.

## SYNTAX

```
Receive-HttpContext [-Listener] <HttpListener> [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### Example 1
```powershell
PS C:\> $context = Receive-WebRequest $http
```

Accepts an HTTP request returns it in an HTTP context object.

## PARAMETERS

### -Listener
The HTTP listener to receive the request through.

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

### System.Net.HttpListener

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
