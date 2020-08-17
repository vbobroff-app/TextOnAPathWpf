# TextOnAPathWpf
Positioning vector text characters along a curved line
![text](https://raw.githubusercontent.com/vbobroff-app/TextOnAPathWpf/master/images/text_on_a_path.png)
# VectorTextBlock      [![NuGet](https://img.shields.io/nuget/v/VectorTextBlock.svg)](https://www.nuget.org/packages/VectorTextBlock)
## How to Install
*CLI  Nuget Pack command*
```
Install-Package VextorTextBlock
```
https://www.nuget.org/packages/VectorTextBlock
## How to Use
```xml
xmlns:vtb="clr-namespace:VectorTextBlock;assembly=VectorTextBlock"
...
<vtb:VectorTextBlock VerticalAlignment="Top" Height="185" Foreground="Blue"
    FontSize="54"
    FontWeight="Bold"
    Text="How to draw text on a path"
    ContentAlignment="Left" 
    AutoScalePath="True"  
    Stroke="Gray"
    StrokeThickness="2"
    Fill="Blue"
    ShowPath="True"
    Shift="12" 
    Padding="54">
   <vtb:VectorTextBlock.TextPath>
       <PathGeometry Figures="M 0 50 Q 25 60 50 50 Q 75 40 100 50 M 100 50 Z" />
   </vtb:VectorTextBlock.TextPath>
</vtb:VectorTextBlock>

```
#### Object

```cs
public class VectorTextBlock : Control
```

#### Properties 
*VektorTextBlock*
|Name|Type|Category|Default|Description|
|-----|-----|-------|-----|-----|
|Text|string|DependencyPropertyy|null| Text
|ContentAlignment|HorizontalAlignment|DependencyProperty|Left| Text HorizontalAlignment
|Fill|Brush|DependencyProperty|null| Fill in Text Gometry object
|Stroke|Brush|DependencyProperty|null| Stroke in Text Gometry object
|StrokeThickness|double|DependencyProperty|0| Stroke Thickness
|TextPath|Geometry|DependencyProperty|null| Curve path as Geomery object
|PathFigure|PathFigure|DependencyProperty|null| Curve path as PathFigure object
|ShowPath|bool|DependencyProperty|false| Show Curve path, stroke equals Foreground
|AutoScalePath|bool|DependencyProperty|false| If true the path bounds size is maximaed
|Shift|doble|DependencyProperty|0| Horzontal shift the text to the path curve, if value is negative, the text will be under the curve
|TextTrimming|TextTrimming|DependencyProperty|0| Text Trimming 
|TextDecorations|TextDecorationCollection|DependencyProperty|0| Text Decorations
|TextWrapping|TextWrapping|DependencyProperty|0| Text TextWrapping

### Resources
* [Render Text On A Path With WPF][3] - by Charles Petzold on 09/10/2019
* [Text On A Path With Alignment][2] - by Jason Ware on December 31, 2013
* [Text on a path in WPF][1] - by lneir, 30 Oct 2008

[1]: https://www.codeproject.com/Articles/30090/Text-On-A-Path-in-WPF
[2]: http://blogs.interknowlogy.com/2013/12/31/4575/
[3]: https://docs.microsoft.com/en-us/archive/msdn-magazine/2008/december/foundations-render-text-on-a-path-with-wpf





