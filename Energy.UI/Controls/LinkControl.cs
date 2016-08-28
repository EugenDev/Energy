using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace Energy.UI.Controls
{
    public class LinkControl : Shape
    {
        private const int BezierLineShift = 5;

        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set { SetValue(X1Property, value); }
        }
        public static readonly DependencyProperty X1Property =
            DependencyProperty.Register("X1", typeof(double), typeof(LinkControl), 
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set { SetValue(Y1Property, value); }
        }
        public static readonly DependencyProperty Y1Property =
            DependencyProperty.Register("Y1", typeof(double), typeof(LinkControl),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }
        public static readonly DependencyProperty X2Property =
            DependencyProperty.Register("X2", typeof(double), typeof(LinkControl),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }
        public static readonly DependencyProperty Y2Property =
            DependencyProperty.Register("Y2", typeof(double), typeof(LinkControl),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public double Distance
        {
            get { return (double)GetValue(DistanceProperty); }
            set { SetValue(DistanceProperty, value); }
        }
        public static readonly DependencyProperty DistanceProperty =
            DependencyProperty.Register("Distance", typeof(double), typeof(LinkControl),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public byte Conduction
        {
            get { return (byte)GetValue(ConductionProperty); }
            set { SetValue(ConductionProperty, value); }
        }
        public static readonly DependencyProperty ConductionProperty =
            DependencyProperty.Register("Conduction", typeof(byte), typeof(LinkControl),
                new FrameworkPropertyMetadata((byte)0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        
        protected override Geometry DefiningGeometry
        {
            get
            {
                var start = new Point(X1, Y1);
                var end = new Point(X2, Y2);
                var pathGeometry = new PathGeometry();
                pathGeometry.Figures.Add(new PathFigure(
                    start,new [] {new LineSegment(end, true) }, 
                    false));
                if (Conduction >= 2)
                    pathGeometry.Figures.Add(new PathFigure(
                        start,
                        new[] { new QuadraticBezierSegment(GetBezierPoint(start, end, false), end, true) },
                        false));
                if (Conduction >= 3)
                    pathGeometry.Figures.Add(new PathFigure(
                        start,
                        new[] { new QuadraticBezierSegment(GetBezierPoint(start, end, true), end, true) },
                        false));

                var text = new FormattedText(Distance.ToString(CultureInfo.CurrentCulture),
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(""), 10, Brushes.Black);
                var textGeometry = text.BuildGeometry(GetCenterPoint(start, end));

                var result = new GeometryGroup();
                result.Children.Add(pathGeometry);
                result.Children.Add(textGeometry);
                return result;
            }
        }

        private Point GetBezierPoint(Point start, Point end, bool reverse)
        {
            var centerPoint = GetCenterPoint(start, end);

            var xl = end.X - start.X;
            var yl = end.Y - start.Y;

            var targetAngle = GetAngle(yl, xl) - 90;

            targetAngle *= Math.PI/180;

            var shift = BezierLineShift * (reverse ? -1 : 1);
            
            centerPoint.X += shift * Math.Cos(targetAngle);
            centerPoint.Y += shift * Math.Sin(targetAngle);

            return centerPoint;
        }

        private Point GetCenterPoint(Point start, Point end)
        {
            var centerVector = new Vector(end.X - start.X, end.Y - start.Y) / 2;
            return start + centerVector;
        }

        private double GetAngle(double ys, double xs)
        {
            var angle = Math.Atan2(ys, xs) * 180 / Math.PI;
            return angle >= 0 ? angle : angle + 360;
        }
    }
}
