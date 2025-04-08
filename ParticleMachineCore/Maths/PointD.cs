using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleMachineCore.Maths
{
    public struct PointD
    {
        public double X;
        public double Y;

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static explicit operator PointF(PointD point)
        {
            return new PointF((float)point.X, (float)point.Y);
        }

        public static implicit operator PointD(PointF point)
        {
            return new PointD(point.X, point.Y);
        }

        public static implicit operator PointD(Point point)
        {
            return new PointD(point.X, point.Y);
        }
    }
}
