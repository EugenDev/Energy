using System;
using NUnit.Framework;

namespace Energy.Tests
{
    [TestFixture]
    public class DebugTest
    {
        [Test, Explicit]
        public void Test()
        {
            PrintResult(1, 1);
            PrintResult(1, -1);
            PrintResult(-1, -1);
            PrintResult(-1, 1);
        }

        private double GetLineAngle(double ys, double xs)
        {
            var angle = Math.Atan2(ys, xs)*180/Math.PI;
            return angle >= 0 ? angle : angle + 360;
        }

        private double GetTargetAngle(double ys, double xs)
        {
            return GetLineAngle(ys, xs) - 90;
        }

        private void PrintResult(double ys, double xs)
        {
            var angle = GetLineAngle(ys, xs);
            var targetAngle = GetTargetAngle(ys, xs);
            var rad = targetAngle*Math.PI/180;
            var cos = Math.Cos(rad);
            var sin = Math.Sin(rad);
            Console.WriteLine($"{angle}\t{targetAngle}\t{cos}\t{sin}");
        }

    }
}
