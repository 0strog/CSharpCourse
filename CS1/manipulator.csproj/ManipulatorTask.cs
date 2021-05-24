using System;
using NUnit.Framework;

namespace Manipulation
{
    public static class ManipulatorTask
    {
        /// <summary>
        /// Возвращает массив углов (shoulder, elbow, wrist),
        /// необходимых для приведения эффектора манипулятора в точку x и y 
        /// с углом между последним суставом и горизонталью, равному alpha (в радианах)
        /// См. чертеж manipulator.png!
        /// </summary>
        public static double[] MoveManipulatorTo(double x, double y, double alpha)
        {
            // Используйте поля Forearm, UpperArm, Palm класса Manipulator
            var rmax = Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm;
            var rmin = Manipulator.UpperArm - Manipulator.Forearm - Manipulator.Palm;
            var countDist = Math.Sqrt(x * x + y * y);
            if (countDist >= rmax ||
                (rmin > 0 && countDist <= rmin)
               )
                return new[] { double.NaN, double.NaN, double.NaN };

            var wristX = x - Manipulator.Palm * Math.Cos(alpha);
            var wristY = y + Manipulator.Palm * Math.Sin(alpha);

            double elbow = TriangleTask.GetABAngle(Manipulator.UpperArm,
                                                 Manipulator.Forearm,
                                                 Math.Sqrt(wristX*wristX + wristY*wristY));
            double shoulder = TriangleTask.GetABAngle
                (
                Math.Sqrt(wristX * wristX + wristY * wristY),
                Manipulator.UpperArm,
                Manipulator.Forearm
                ) + Math.Atan2(wristY, wristX);

            double wrist = -alpha - shoulder - elbow;

            return new[] { shoulder, elbow, wrist };
        }
    }

    [TestFixture]
    public class ManipulatorTask_Tests
    {
        [Test]
        public void TestMoveManipulatorTo()
        {
            int casesCount = 1000;
            var minDist = -(Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm);
            var maxDist = Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm;
            var random = new Random();
            for (int i = 0; i < casesCount; i++)
            {
                double x = random.NextDouble();
                double y = random.NextDouble();
                double alpha = random.NextDouble();
                double[] res = ManipulatorTask.MoveManipulatorTo(x, y, alpha);
                double dist = Math.Sqrt(x * x + y * y);
                if (dist <= minDist ||
                    dist >= maxDist)
                    Assert.Fail("Manipulator can not go here!"); break;
            }
        }
    }
}