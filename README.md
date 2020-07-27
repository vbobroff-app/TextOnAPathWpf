# TextOnAPathWpf
Positioning vector text characters along a curved line
# VectorTextBlock
#### Object

```cs
public class VectorTextBlock : Control
```

#### Properties 
*ChromeBase*
|Name|Type|Category|Default|Description|
|-----|-----|-----|-----|-----|
|Text|string|Attached Property|null| Text
|ContentAlignment|HorizontalAlignment|Attached Property|Left| Text HorizontalAlignment
|Fill|Brush|Attached Property|null| Fill in Text Gometry object
|Stroke|Brush|Attached Property|null| Stroke in Text Gometry object
|StrokeThickness|double|Attached Property|0| Stroke Thickness
|TextPath|Geometry|Attached Property|null| Curve path as Geomery object
|PathFigure|PathFigure|Attached Property|null| Curve path as PathFigure object
|ShowPath|bool|Attached Property|false| Show Curve path, stroke equals Foreground
|AutoScalePath|bool|Attached Property|false| If true the path bounds size is maximaed
|Shift|doble|Attached Property|0| Horzontal shift the text to the path curve, if value is negative, the text will be under the curve
|TextTrimming|TextTrimming|Attached Property|0| Text Trimming, like same TextBlock property 
|TextDecorations|TextDecorationCollection|Attached Property|0| Text Decorations, like same TextBlock property
|TextWrapping|TextWrapping|Attached Property|0| Text TextWrapping, like same TextBlock property





