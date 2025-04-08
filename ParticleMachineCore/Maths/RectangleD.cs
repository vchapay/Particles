using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleMachineCore.Maths
{
    public struct RectangleD
    {
        #region Поля

        private PointD _position;
        private double _width;
        private double _height;

        #endregion

        #region Конструкторы

        public RectangleD(double width, double height)
        {
            _width = width;
            _height = height;
            _position = new PointD();
        }

        #endregion

        #region Свойства

        public PointD Position
        {
            get => _position;
            set => _position = value;
        }

        public double X
        {
            get => _position.X;
            set => _position.X = value;
        }

        public double Y
        {
            get => _position.Y;
            set => _position.Y = value;
        }

        public double Width
        {
            get => _width;
            set => _width = value > 0 ? value : 0;
        }

        public double Height
        {
            get => _height;
            set => _height = value > 0 ? value : 0;
        }

        public double Top => _position.Y;

        public double Left => _position.X;

        public double Bottom => _position.Y + _height;

        public double Right => _position.X + _width;

        #endregion

        #region Методы

        public static implicit operator RectangleD(Rectangle rect)
        {
            return new RectangleD()
            {
                X = rect.X,
                Y = rect.Y,
                Width = rect.Width,
                Height = rect.Height
            };
        }

        #endregion
    }
}
