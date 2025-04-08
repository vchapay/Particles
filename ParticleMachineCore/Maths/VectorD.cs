using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleMachineCore.Maths
{
    public struct VectorD
    {
        public double X;
        public double Y;

        public VectorD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public VectorD(PointD start, PointD end)
        {
            X = end.X - start.X;
            Y = end.Y - start.Y;
        }

        public double Length => Math.Sqrt(X * X + Y * Y);

        public double XAngle
        {
            get
            {
                double value;
                value = (double)Math.Atan(Y / X);
                if (X < 0)
                {
                    value = Math.PI + value;
                }

                return value;
            }
        }

        public void SetLength(double length)
        {
            if (Length == 0) X++;
            UpdatePos(length, XAngle);
        }

        public void SetAngle(double angle)
        {
            UpdateAngle(angle);
        }

        public void Rotate(double angle)
        {
            UpdateAngle(angle + XAngle);
        }

        public override string ToString()
        {
            return $"length: {Length}, angle: {XAngle}";
        }

        private void UpdatePos(double length, double angle)
        {
            X = Math.Cos(angle) * length;
            Y = Math.Sin(angle) * length;
        }

        private void UpdateAngle(double angle)
        {
            UpdatePos(Length, angle);
        }

        public static VectorD operator +(VectorD a, VectorD b)
        {
            return new VectorD()
            {
                X = b.X + a.X,
                Y = b.Y + a.Y,
            };
        }

        public static VectorD operator +(VectorD v, double num)
        {
            v.SetLength(v.Length + num);
            return v;
        }

        public static VectorD operator -(VectorD v, double num)
        {
            v.SetLength(v.Length - num);
            return v;
        }

        public static VectorD operator *(VectorD v, double num)
        {
            v.SetLength(v.Length * num);
            return v;
        }
    }
}
