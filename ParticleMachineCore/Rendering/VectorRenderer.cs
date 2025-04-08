using ParticleMachineCore.Maths;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleMachineCore.Rendering
{
    public class VectorRenderer
    {
        private VectorD _vector;
        private readonly Pen _pen;
        private readonly Pen _selectingPen;
        private float _arrowAngle = 60;
        private int _arrowLinesLength = 30;
        private float _scaling = 1;

        public VectorRenderer()
        {
            _pen = new Pen(Color.DarkGray);
            _selectingPen = new Pen(Color.DarkGray) { Width = 1.7f };
            Color = Color.Black;
        }

        public VectorD Vector
        {
            get => _vector;
            set => _vector = value;
        }

        public float PenWidth
        {
            get => _pen.Width;
            set => _pen.Width = value > 0 ? value : 1;
        }

        public float Scaling
        {
            get => _scaling;
            set => _scaling = value > 0 ? value : _scaling;
        }

        public float ArrowAngle
        {
            get => _arrowAngle;
            set => _arrowAngle = value;
        }

        public int ArrowLinesLength
        {
            get => _arrowLinesLength;
            set => _arrowLinesLength = value > 0 ?
                value : _arrowLinesLength;
        }

        public double XPadding { get; set; } = 0;

        public double YPadding { get; set; } = 0;

        public Color Color
        {
            get => _selectingPen.Color;
            set
            {
                _selectingPen.Color = value;
                _pen.Color = Color.FromArgb(value.A / 2, value);
            }
        }

        public bool Selected { get; set; }

        public void Render(Graphics g)
        {
            PointF[] line = ConstructVectorLine(_vector, XPadding, YPadding);
            PointF[] arrow = ConstructArrowCap(line);

            Pen pen = _pen;

            if (Selected) pen = _selectingPen;

            g.DrawLine(pen, line[0], line[1]);
            g.DrawLine(pen, line[1], arrow[0]);
            g.DrawLine(pen, line[1], arrow[1]);
        }

        private PointF[] ConstructVectorLine(VectorD vector,
            double xpad, double ypad)
        {
            PointF[] line = new PointF[2];
            line[0] = new PointF((float)xpad, (float)ypad);
            line[1] = new PointF((float)(xpad + vector.X * _scaling),
                (float)(ypad + vector.Y * _scaling));

            return line;
        }

        private PointF[] ConstructArrowCap(PointF[] line)
        {
            VectorD arrVector1 = new VectorD(_arrowLinesLength, _arrowLinesLength);
            VectorD arrVector2 = new VectorD(_arrowLinesLength, _arrowLinesLength);

            double degrees = AngleUnitsConverter.RadiansToDegrees(_vector.XAngle);
            arrVector1.SetAngle(AngleUnitsConverter.DegreesToRadians(
                degrees - 180 + _arrowAngle / 2));
            arrVector2.SetAngle(AngleUnitsConverter.DegreesToRadians(
                degrees - 180 - _arrowAngle / 2));

            PointF[] arrLine1 = ConstructVectorLine(arrVector1, line[1].X, line[1].Y);
            PointF[] arrLine2 = ConstructVectorLine(arrVector2, line[1].X, line[1].Y);

            return new PointF[] { arrLine1[1], arrLine2[1] };
        }
    }
}
