using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Manipulation
{
	public static class VisualizerTask
	{
		public static double X = 220;
		public static double Y = -100;
		public static double Alpha = 0.05;
		public static double Wrist = 2 * Math.PI / 3;
		public static double Elbow = 3 * Math.PI / 4;
		public static double Shoulder = Math.PI / 2;

		public static Brush UnreachableAreaBrush = new SolidBrush(Color.FromArgb(255, 255, 230, 230));
		public static Brush ReachableAreaBrush = new SolidBrush(Color.FromArgb(255, 230, 255, 230));
		public static Pen ManipulatorPen = new Pen(Color.Black, 3);
		public static Brush JointBrush = Brushes.Gray;

		public static void KeyDown(Form form, KeyEventArgs key)
		{
			var dAngle = 0.025;
			// TODO: Добавьте реакцию на QAWS и пересчитывать Wrist
			form.Invalidate(); //
			var value = key.KeyValue;
			switch(value)
			{
				case 'Q':
					Shoulder += dAngle;
					Wrist = -Alpha - Shoulder - Elbow;
					break;
				case 'A':
					Shoulder -= dAngle;
					Wrist = -Alpha - Shoulder - Elbow;
					break;
				case 'W':
					Elbow += dAngle;
					Wrist = -Alpha - Shoulder - Elbow;
					break;
				case 'S':
					Elbow -= dAngle;
					Wrist = -Alpha - Shoulder - Elbow;
					break;
			}
		}


		public static void MouseMove(Form form, MouseEventArgs e)
		{
			// TODO: Измените X и Y пересчитав координаты (e.X, e.Y) в логические.
			var windowPoint = new PointF(e.X, e.Y);
			var mousePoint = ConvertWindowToMath(windowPoint, GetShoulderPos(form));
			X = mousePoint.X;
			Y = mousePoint.Y;

			UpdateManipulator();
			form.Invalidate();
		}

		public static void MouseWheel(Form form, MouseEventArgs e)
		{
			// TODO: Измените Alpha, используя e.Delta — размер прокрутки колеса мыши
			Alpha += e.Delta;

			UpdateManipulator();
			form.Invalidate();
		}

		public static void UpdateManipulator()
		{
			// Вызовите ManipulatorTask.MoveManipulatorTo и обновите значения полей Shoulder, Elbow и Wrist, 
			// если они не NaN. Это понадобится для последней задачи.
			double[] angles = ManipulatorTask.MoveManipulatorTo(X, Y, Alpha);
			if (angles[0] == Double.NaN ||
			   angles[1] == Double.NaN ||
			   angles[2] == Double.NaN)
				return;
			Shoulder = angles[0];
			Elbow = angles[1];
			Wrist = angles[2];
		}

		public static void DrawManipulator(Graphics graphics, PointF shoulderPos)
		{
			var joints = AnglesToCoordinatesTask.GetJointPositions(Shoulder, Elbow, Wrist);

			graphics.DrawString(
				$"X={X:0}, Y={Y:0}, Alpha={Alpha:0.00}",
				new Font(SystemFonts.DefaultFont.FontFamily, 12),
				Brushes.DarkRed,
				10,
				10);
			DrawReachableZone(graphics, ReachableAreaBrush, UnreachableAreaBrush, shoulderPos, joints);

			// Нарисуйте сегменты манипулятора методом graphics.DrawLine используя ManipulatorPen.
			// Нарисуйте суставы манипулятора окружностями методом graphics.FillEllipse используя JointBrush.
			// Не забудьте сконвертировать координаты из логических в оконные
			PointF zeroCoord = new PointF( 0, 0 );
			PointF[] actualCoordinates = {
				ConvertMathToWindow(zeroCoord, shoulderPos),
				ConvertMathToWindow(joints[0], shoulderPos),
				ConvertMathToWindow(joints[1], shoulderPos),
				ConvertMathToWindow(joints[2], shoulderPos)
				};
			SizeF circleSize = new SizeF((float)5, (float)5);
			RectangleF rect0 = new RectangleF(actualCoordinates[0], circleSize);
			RectangleF rect1 = new RectangleF(actualCoordinates[1], circleSize);
			RectangleF rect2 = new RectangleF(actualCoordinates[2], circleSize);
			graphics.DrawEllipse(ManipulatorPen, rect0);
			graphics.DrawEllipse(ManipulatorPen, rect1);
			graphics.DrawEllipse(ManipulatorPen, rect2);
			graphics.DrawLine(ManipulatorPen, actualCoordinates[0], actualCoordinates[1]);
			graphics.DrawLine(ManipulatorPen, actualCoordinates[1], actualCoordinates[2]);
			graphics.DrawLine(ManipulatorPen, actualCoordinates[2], actualCoordinates[3]);
		}

		private static void DrawReachableZone(
            Graphics graphics, 
            Brush reachableBrush, 
            Brush unreachableBrush, 
            PointF shoulderPos, 
            PointF[] joints)
		{
			var rmin = Math.Abs(Manipulator.UpperArm - Manipulator.Forearm);
			var rmax = Manipulator.UpperArm + Manipulator.Forearm;
			var mathCenter = new PointF(joints[2].X - joints[1].X, joints[2].Y - joints[1].Y);
			var windowCenter = ConvertMathToWindow(mathCenter, shoulderPos);
			graphics.FillEllipse(reachableBrush, windowCenter.X - rmax, windowCenter.Y - rmax, 2 * rmax, 2 * rmax);
			graphics.FillEllipse(unreachableBrush, windowCenter.X - rmin, windowCenter.Y - rmin, 2 * rmin, 2 * rmin);
		}

		public static PointF GetShoulderPos(Form form)
		{
			return new PointF(form.ClientSize.Width / 2f, form.ClientSize.Height / 2f);
		}

		public static PointF ConvertMathToWindow(PointF mathPoint, PointF shoulderPos)
		{
			return new PointF(mathPoint.X + shoulderPos.X, shoulderPos.Y - mathPoint.Y);
		}

		public static PointF ConvertWindowToMath(PointF windowPoint, PointF shoulderPos)
		{
			return new PointF(windowPoint.X - shoulderPos.X, shoulderPos.Y - windowPoint.Y);
		}

	}
}