using System;
using NUnit.Framework;

namespace Manipulation
{
    public class TriangleTask
    {
        /// <summary>
        /// Возвращает угол (в радианах) между сторонами a и b в треугольнике со сторонами a, b, c 
        /// </summary>
        public static double GetABAngle(double a, double b, double c)
        {
            if (a <= 0 || b <= 0 || c < 0)
                return Double.NaN;
            var angle = Math.Acos((a * a + b * b - c * c) / (2 * a * b));
            return angle;
        }
    }

    [TestFixture]
    public class TriangleTask_Tests
    {
        [TestCase(3, 4, 5, Math.PI / 2)]
        [TestCase(1, 1, 1, Math.PI / 3)]
        [TestCase(0, 1, 1, Double.NaN)]
        [TestCase(1, 0, 1, Double.NaN)]
        // добавьте ещё тестовых случаев!
        public void TestGetABAngle(double a, double b, double c, double expectedAngle)
        {
            var res = TriangleTask.GetABAngle(a, b, c);
            if ((a == 0 || b == 0) && !Double.IsNaN(res))
                Assert.Fail("Not implemented yet");
            Assert.AreEqual(expectedAngle, res, 1e-5, "Fail!");
        }
    }
}