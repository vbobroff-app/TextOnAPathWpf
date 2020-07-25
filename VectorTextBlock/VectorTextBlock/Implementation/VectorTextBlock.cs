using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VectorTextBlock
{
    [TemplatePart(Name = "LayoutPanel", Type = typeof(Canvas))]
    public class VectorTextBlock : Control
    {
        private static readonly Type OwnerType = typeof(VectorTextBlock);

        #region Fill
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
          "Fill",
          typeof(Brush),
          OwnerType,
          new FrameworkPropertyMetadata(Brushes.Transparent, OnChanged));

        public Brush Fill
        {
            get => (Brush)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        #endregion

        #region Stroke

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
          "Stroke",
          typeof(Brush),
          OwnerType,
          new FrameworkPropertyMetadata(Brushes.Black, OnChanged));

        public Brush Stroke
        {
            get => (Brush)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        #endregion

        #region StrokeThicjness
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness",
            typeof(double),
            OwnerType,
            new FrameworkPropertyMetadata(0d, OnChanged));

        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        #endregion

        #region Shift
        public static readonly DependencyProperty ShiftProperty = DependencyProperty.Register(
          "Shift",
          typeof(double),
          OwnerType,
          new FrameworkPropertyMetadata(0.0, OnShiftPropertyChanged));

        public double Shift
        {
            get => (double)GetValue(ShiftProperty);
            set => SetValue(ShiftProperty, value);
        }


        private static void OnShiftPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is VectorTextBlock vectorTextBlock)) return;

            vectorTextBlock.Refresh();
        }
        #endregion

        #region Text
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
          "Text",
          typeof(string),
          OwnerType,
          new FrameworkPropertyMetadata(OnFormattedTextInvalidated));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private static void OnFormattedTextInvalidated(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(d is VectorTextBlock vectorTextBlock)) return;

            vectorTextBlock.UpdateText();
            vectorTextBlock.Refresh();
        }
        #endregion

        #region TextPath


        public static readonly DependencyProperty TextPathProperty =
            DependencyProperty.Register("TextPath", typeof(Geometry), OwnerType,
                new FrameworkPropertyMetadata(null, OnTextPathPropertyChanged));

        public Geometry TextPath
        {
            get => (Geometry)GetValue(TextPathProperty);
            set => SetValue(TextPathProperty, value);
        }

        private static void OnTextPathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is VectorTextBlock vectorTextBlock)) return;


            if (e.NewValue.IsEqual(e.OldValue))
                return;

            if (e.NewValue == null)
            {
                if (vectorTextBlock.PathFigure != null) //try to generate from figure
                {
                    var figure = vectorTextBlock.PathFigure;
                    vectorTextBlock.PathFigure = null; //switch
                    vectorTextBlock.PathFigure = figure;
                }
            }

            vectorTextBlock.Refresh();
            vectorTextBlock.Update();

            //vectorTextBlock.EnsureGeometry();
            //vectorTextBlock.InvalidateMeasure();
            //vectorTextBlock.InvalidateVisual();

            //vectorTextBlock.UpdateSize();

        }

        #endregion

        #region PathFigure

        public static readonly DependencyProperty PathFigureProperty =
            DependencyProperty.Register("PathFigure", typeof(PathFigure), OwnerType, new FrameworkPropertyMetadata(OnPathFigureChanged));

        public PathFigure PathFigure
        {
            get => (PathFigure)GetValue(PathFigureProperty);
            set => SetValue(PathFigureProperty, value);
        }
        private static void OnPathFigureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is VectorTextBlock vectorTextBlock)) return;

            if (!(e.NewValue is PathFigure pathFigure))
            {
                if (vectorTextBlock.TextPath != null && vectorTextBlock.TextPath.GetHashCode() == vectorTextBlock._pathHash)
                {
                    vectorTextBlock.TextPath = null;
                }
                vectorTextBlock.Refresh();
                return;
            }

            if (e.NewValue.IsEqual(e.OldValue))
                return;

            // vectorTextBlock._pathLength = vectorTextBlock.GetPathFigureLength(pathFigure);
            //element.TransformVisualChildren();
            if (vectorTextBlock.TextPath == null)
            {
                vectorTextBlock.TextPath = new PathGeometry(new[] { pathFigure });
                vectorTextBlock._pathHash = vectorTextBlock.TextPath.GetHashCode();//save hash to Refresh in designer mode
            }

        }

        #endregion

        #region AutoScaletPath 

        public static readonly DependencyProperty AutoScalePathProperty =
            DependencyProperty.Register("AutoScalePath", typeof(bool), OwnerType,
                new PropertyMetadata(OnAutoScaleTextPathPropertyChanged));

        /// <summary>
        /// If set to True (default) then the geometry defined by TextPath automatically gets scaled to fit the width/height of the control
        /// </summary>
        public bool AutoScalePath
        {
            get => (bool)GetValue(AutoScalePathProperty);
            set => SetValue(AutoScalePathProperty, value);
        }

        static void OnAutoScaleTextPathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is VectorTextBlock vectorTextBlock)) return;

            if (e.NewValue.IsEqual(e.OldValue) || e.NewValue == null)
                return;


            var value = (bool)e.NewValue;

            if (value == false && vectorTextBlock.TextPath != null)
                vectorTextBlock.TextPath.Transform = null;

            vectorTextBlock.Refresh();
            vectorTextBlock.Update();

        }

        #endregion

        #region ShowPath 

        public static readonly DependencyProperty ShowPathProperty =
            DependencyProperty.Register("ShowPath", typeof(bool), OwnerType,
                new PropertyMetadata(false, ShowPathPropertyChanged));

        /// <summary>
        /// If set to True (default) then the geometry defined by TextPath automatically gets scaled to fit the width/height of the control
        /// </summary>
        public bool ShowPath
        {
            get => (bool)GetValue(ShowPathProperty);
            set => SetValue(ShowPathProperty, value);
        }

        static void ShowPathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is VectorTextBlock vectorTextBlock)) return;

            if (e.NewValue.IsEqual(e.OldValue) || e.NewValue == null)
                return;

            vectorTextBlock.Refresh();
            vectorTextBlock.Update();

        }

        #endregion

        #region ContentAlignment

        public static readonly DependencyProperty ContentAlignmentProperty =
            DependencyProperty.Register("ContentAlignment", typeof(HorizontalAlignment), OwnerType, new FrameworkPropertyMetadata(OnContentAlignmentChanged));

        public HorizontalAlignment ContentAlignment
        {
            get => (HorizontalAlignment)GetValue(ContentAlignmentProperty);
            set => SetValue(ContentAlignmentProperty, value);
        }
        private static void OnContentAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is VectorTextBlock vectorTextBlock)) return;
            vectorTextBlock.UpdateText();
            vectorTextBlock.Refresh();
        }

        #endregion


        #region TextDecorations
        public static readonly DependencyProperty TextDecorationsProperty = DependencyProperty.Register(
            "TextDecorations",
            typeof(TextDecorationCollection),
            OwnerType,
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));
        public TextDecorationCollection TextDecorations
        {
            get => (TextDecorationCollection)GetValue(TextDecorationsProperty);
            set => SetValue(TextDecorationsProperty, value);
        }

        #endregion

        #region TextTrimming
        public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register(
            "TextTrimming",
            typeof(TextTrimming),
            OwnerType,
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public TextTrimming TextTrimming
        {
            get => (TextTrimming)GetValue(TextTrimmingProperty);
            set => SetValue(TextTrimmingProperty, value);
        }
        #endregion

        #region TextWrapping
        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register(
            "TextWrapping",
            typeof(TextWrapping),
            OwnerType,
            new FrameworkPropertyMetadata(TextWrapping.NoWrap, OnFormattedTextUpdated));

        public TextWrapping TextWrapping
        {
            get => (TextWrapping)GetValue(TextWrappingProperty);
            set => SetValue(TextWrappingProperty, value);
        }
        #endregion

        private int _pathHash;
        private Size _newSize;

        private Canvas _layoutPanel;

        private bool _isDrawn;

        private FormattedText[] _formattedChars;
        private double _textLength;

        static VectorTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(OwnerType, new FrameworkPropertyMetadata(OwnerType));

            FontSizeProperty.OverrideMetadata(OwnerType, new FrameworkPropertyMetadata(OnFormattedTextUpdated));
            FontFamilyProperty.OverrideMetadata(OwnerType, new FrameworkPropertyMetadata(OnFormattedTextUpdated));
            FontStretchProperty.OverrideMetadata(OwnerType, new FrameworkPropertyMetadata(OnFormattedTextUpdated));
            FontStyleProperty.OverrideMetadata(OwnerType, new FrameworkPropertyMetadata(OnFormattedTextUpdated));
            FontWeightProperty.OverrideMetadata(OwnerType, new FrameworkPropertyMetadata(OnFormattedTextUpdated));
            PaddingProperty.OverrideMetadata(OwnerType, new FrameworkPropertyMetadata(OnChanged));
            ForegroundProperty.OverrideMetadata(OwnerType, new FrameworkPropertyMetadata(OnChanged));
        }
        public VectorTextBlock()
        {
            TextDecorations = new TextDecorationCollection();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _layoutPanel = GetTemplateChild("LayoutPanel") as Canvas;
            if (_layoutPanel == null) return;

            _layoutPanel.SizeChanged -= VectorTextBlock_SizeChanged;
            _layoutPanel.SizeChanged += VectorTextBlock_SizeChanged;

            if (_isDrawn) return;
            Refresh();
            Update();
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            UpdateText();

            var width = Width > 0 ? Width : _textLength;
            var height = Height > 0 ? Height : FontSize * 1.1;

            return new Size(Math.Ceiling(width), Math.Ceiling(height));
        }
        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is VectorTextBlock vectorTextBlock)) return;

            if (e.NewValue.IsEqual(e.OldValue))
                return;

            vectorTextBlock.Refresh();
            vectorTextBlock.Update();
        }
        private static void OnFormattedTextUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is VectorTextBlock vectorTextBlock)) return;

            vectorTextBlock.UpdateText();
            vectorTextBlock.Refresh();
        }
        private void UpdateText()
        {
            _textLength = 0;
            _formattedChars = null;
            if (Text == null || FontFamily == null || FontSize <= 0) return;

            _formattedChars = new FormattedText[Text.Length];

            for (int i = 0; i < Text.Length; i++)
            {
                var formattedText = getFormattedText(new string(Text[i], 1));

                _formattedChars[i] = formattedText;
                _textLength += formattedText.WidthIncludingTrailingWhitespace;
            }
        }
        private FormattedText getFormattedText(string text)
        {
            var formattedText = new FormattedText(
                text ?? "",
                CultureInfo.CurrentUICulture,
                FlowDirection,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                FontSize,
                Brushes.Transparent,
                VisualTreeHelper.GetDpi(this).PixelsPerDip)
            {
                MaxLineCount = TextWrapping == TextWrapping.NoWrap ? 1 : int.MaxValue,
                Trimming = TextTrimming
            };

            formattedText.SetTextDecorations(TextDecorations);

            return formattedText;
        }
        private void Refresh()
        {
            EnsureGeometry();
            InvalidateMeasure();
            InvalidateVisual();
        }
        private void VectorTextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _newSize = e.NewSize;
            Update();
        }
        private static double GetPathFigureLength(PathFigure pathFigure)
        {
            if (pathFigure == null)
                return 0;

            var isAlreadyFlattened = true;

            foreach (PathSegment pathSegment in pathFigure.Segments)
            {
                if (pathSegment is PolyLineSegment || pathSegment is LineSegment) continue;
                isAlreadyFlattened = false;
                break;
            }

            PathFigure pathFigureFlattened = isAlreadyFlattened ? pathFigure : pathFigure.GetFlattenedPathFigure();
            double length = 0;
            Point pt1 = pathFigureFlattened.StartPoint;

            foreach (PathSegment pathSegment in pathFigureFlattened.Segments)
            {
                if (pathSegment is LineSegment lineSegment)
                {
                    Point pt2 = lineSegment.Point;
                    length += (pt2 - pt1).Length;
                    pt1 = pt2;
                }
                else if (pathSegment is PolyLineSegment polyLineSegment)
                {
                    foreach (Point pt2 in polyLineSegment.Points)
                    {
                        length += (pt2 - pt1).Length;
                        pt1 = pt2;
                    }
                }
            }
            return length;
        }
        void Update()
        {
            UpdateText();
            UpdateSize();
            EnsureGeometry();
        }
        private Geometry GetDefaultGeometry()
        {
            var width = _newSize.Width > 0 ? _newSize.Width : _textLength;
            var lineGeometry = new LineGeometry(new Point(0, 0), new Point(width, 0));
            var height = FontSize * 1.1;
            var translate = new TranslateTransform(0, height);
            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(translate);
            lineGeometry.Transform = transformGroup;

            return lineGeometry;
        }
        private void EnsureGeometry()
        {
            if (_layoutPanel == null) return;

            _layoutPanel.Children.Clear();
            _layoutPanel.Margin = Padding;

            var textPath = TextPath ?? GetDefaultGeometry();

            var flatGeometry = textPath.GetFlattenedPathGeometry();
            var figure = flatGeometry.Figures[0];
            var pathLength = GetPathFigureLength(figure);

            if (_textLength <= 0 || pathLength <= 0) return;

            var scalingFactor = ContentAlignment == HorizontalAlignment.Stretch ? pathLength / _textLength : 1;

            double progress;
            switch (ContentAlignment)
            {
                case HorizontalAlignment.Left:
                case HorizontalAlignment.Stretch:
                    progress = 0;
                    break;
                case HorizontalAlignment.Center:
                    progress = Math.Abs(pathLength - _textLength) / 2 / pathLength;
                    break;
                case HorizontalAlignment.Right:
                    progress = Math.Abs(pathLength - _textLength) / pathLength;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (ShowPath)
            {
                var path = new Path { Data = textPath, Stroke = Foreground ?? Stroke };
                _layoutPanel.Children.Add(path);
            }

            if (_formattedChars == null) return;

            foreach (var ch in _formattedChars)
            {
                var width = scalingFactor * ch.WidthIncludingTrailingWhitespace;
                var baseline = ch.Baseline;

                progress += width / 2 / pathLength;

                flatGeometry.GetPointAtFractionLength(progress, out var point, out var tangent);

                var curGeometry = ch.BuildGeometry(new Point(0, 0));

                if (!curGeometry.IsFrozen)
                {
                    var angle = Math.Atan2(tangent.Y, tangent.X) * 180 / Math.PI;
                    var shift = Shift + baseline;
                    var rotateTransform = new RotateTransform(angle, width / 2, shift);
                    var translateTransform = new TranslateTransform(point.X - width / 2, point.Y - shift);

                    var transformGroup = new TransformGroup();
                    transformGroup.Children.Add(rotateTransform);
                    transformGroup.Children.Add(translateTransform);

                    curGeometry.Transform = transformGroup;
                    pushGeometry(curGeometry);
                }

                progress += width / 2 / pathLength;
            }

            _isDrawn = true;
        }
        private void pushGeometry(Geometry geometry)
        {
            if (_layoutPanel == null) return;
            var groupPath = new Path { Data = geometry, Stroke = Stroke, Fill = Fill, StrokeThickness = StrokeThickness };
            _layoutPanel.Children.Add(groupPath);
        }
        private void UpdateSize()
        {
            if (TextPath == null || double.IsNaN(TextPath.Bounds.Width) || double.IsNaN(TextPath.Bounds.Height))
            {
                return;
            }

            var xScale = TextPath.Bounds.Width > 0 ? _newSize.Width / Math.Abs(TextPath.Bounds.Width) : 1;
            var yScale = TextPath.Bounds.Height > 0 ? _newSize.Height / Math.Abs(TextPath.Bounds.Height) : 1;

            if (xScale <= 0 || yScale <= 0)
                return;

            if (TextPath.Transform is TransformGroup transformGroup)
            {
                if (!(transformGroup.Children[0] is ScaleTransform) || !(transformGroup.Children[1] is TranslateTransform)) return;

                if (AutoScalePath)
                {
                    var scale = (ScaleTransform)transformGroup.Children[0];
                    scale.ScaleX *= xScale;
                    scale.ScaleY *= yScale;
                }

                var translate = (TranslateTransform)transformGroup.Children[1];
                translate.X += -TextPath.Bounds.X;
                translate.Y += -TextPath.Bounds.Y;
            }
            else
            {
                ScaleTransform scale;
                TranslateTransform translate;

                if (AutoScalePath)
                {
                    scale = new ScaleTransform(xScale, yScale);
                    translate = new TranslateTransform(-TextPath.Bounds.X * xScale, -TextPath.Bounds.Y * yScale);
                }
                else
                {
                    scale = new ScaleTransform(1.0, 1.0);
                    translate = new TranslateTransform(-TextPath.Bounds.X, -TextPath.Bounds.Y);
                }

                TransformGroup transform = new TransformGroup();
                transform.Children.Add(scale);
                transform.Children.Add(translate);
                TextPath.Transform = transform;
            }

        }
    }

}
