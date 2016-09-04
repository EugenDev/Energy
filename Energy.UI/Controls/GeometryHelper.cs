using System;
using System.Windows;

namespace Energy.UI.Controls
{
    public static class GeometryHelper
    {
        public static Point GetCenterPoint(Point start, Point end)
        {
            var centerVector = new Vector(end.X - start.X, end.Y - start.Y) / 2;
            return start + centerVector;
        }

        public static Point GetBezierPoint(Point start, Point end, int pointShift)
        {
            var centerPoint = GetCenterPoint(start, end);

            var xl = end.X - start.X;
            var yl = end.Y - start.Y;

            var targetAngle = GetAngle(yl, xl) - 90;

            targetAngle *= Math.PI / 180;
            
            centerPoint.X += pointShift * Math.Cos(targetAngle);
            centerPoint.Y += pointShift * Math.Sin(targetAngle);

            return centerPoint;
        }

        public static double GetAngle(double ys, double xs)
        {
            var angle = Math.Atan2(ys, xs) * 180 / Math.PI;
            return angle >= 0 ? angle : angle + 360;
        }
    }
}
