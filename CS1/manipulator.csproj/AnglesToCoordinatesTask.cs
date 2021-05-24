using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        /// <summary>
        /// По значению углов суставов возвращает массив координат суставов
        /// в порядке new []{elbow, wrist, palmEnd}
        /// </summary>
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            var elbowNew = shoulder + elbow - Math.PI;
            var wristNew = elbowNew + wrist - Math.PI;
            var elbowX   = Manipulator.UpperArm * Math.Cos(shoulder);
            var elbowY   = Manipulator.UpperArm * Math.Sin(shoulder);
            var wristX   = Manipulator.Forearm  * Math.Cos(elbowNew) + elbowX;
            var wristY   = Manipulator.Forearm  * Math.Sin(elbowNew) + elbowY;
            var palmEndX = Manipulator.Palm     * Math.Cos(wristNew) + wristX;
            var palmEndY = Manipulator.Palm     * Math.Sin(wristNew) + wristY;

            var elbowPos   = new PointF((float)elbowX,   (float)elbowY);
            var wristPos   = new PointF((float)wristX,   (float)wristY);
            var palmEndPos = new PointF((float)palmEndX, (float)palmEndY);
            return new PointF[]
            {
                elbowPos,
                wristPos,
                palmEndPos
            };
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        // Доработайте эти тесты!
        // С помощью строчки TestCase можно добавлять новые тестовые данные.
        // Аргументы TestCase превратятся в аргументы метода.
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI / 2, Manipulator.Forearm, Manipulator.UpperArm - Manipulator.Palm)]
        [TestCase(Math.PI / 2, Math.PI / 2, 3 * Math.PI / 2, Manipulator.Forearm, Manipulator.UpperArm + Manipulator.Palm)]
        [TestCase(Math.PI / 2, Math.PI, Math.PI, 0, Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm)]
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
            //Assert.Fail("TODO: проверить, что расстояния между суставами равны длинам сегментов манипулятора!");
        }
    }
}