using ParticleMachineCore.Maths;
using ParticleMachineCore.ParticleModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleMachineCore.Rendering
{
    /// <summary>
    /// Описывает визуальный стиль частиц и предоставляет метод отрисовки.
    /// </summary>
    public class ParticleRenderer
    {
        #region Поля

        private Particle _target;
        private double _radius;
        private readonly SolidBrush _brush;
        private Color _primaryColor;
        private readonly Color[] _palette;
        private readonly VectorRenderer _vRenderer;
        private RenderLayer _layer;

        #endregion

        #region Конструкторы

        public ParticleRenderer()
        {
            _brush = new SolidBrush(Color.Black);
            _vRenderer = new VectorRenderer()
            {
                Color = Color.White,
                PenWidth = 2,
                Scaling = 0.2f
            };
            _palette = new[]
            {
                Color.FromArgb(237, 29, 2),
                Color.FromArgb(250, 159, 2),
                Color.FromArgb(222, 215, 2),
                Color.FromArgb(147, 232, 0),
                Color.FromArgb(2, 219, 114),
                Color.FromArgb(2, 219, 201),
                Color.FromArgb(2, 198, 242),
                Color.FromArgb(174, 2, 242),
                Color.FromArgb(228, 149, 240),
                Color.FromArgb(218, 223, 235)
            };
            _layer = RenderLayer.Energy;
        }

        #endregion

        #region Свойства

        /// <summary>
        /// Задает отрисовываемый объект.
        /// </summary>
        public Particle Target
        {
            get => _target;
            set
            {
                if (value == null)
                    return;

                _target = value;
            }
        }

        public bool ShowVelocityVector { get; set; }

        public RenderLayer Layer
        {
            get => _layer;
            set => _layer = value;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Отображает текущий объект Target на полотне Graphics.
        /// </summary>
        /// <param name="g"></param>
        public void Render(Graphics g)
        {
            if (_target == null)
                return;

            RenderBody(g);
            if (ShowVelocityVector) RenderVelocityVector(g);
        }

        private void RenderBody(Graphics g)
        {
            _brush.Color = GetColor();
            _radius = _target.GetRadius();
            RectangleF rect = new RectangleF()
            {
                X = (float)(Target.X - _radius),
                Y = (float)(Target.Y - _radius),
                Width = (float)(_radius * 2),
                Height = (float)(_radius * 2)
            };

            g.FillEllipse(_brush, rect);
        }

        private Color GetColor()
        {
            switch (_layer)
            {
                case RenderLayer.All:
                    break;
                case RenderLayer.Spins:
                    if (_target.Spin < 0)
                        return _palette[1];
                    break;
                case RenderLayer.Energy:
                    return _palette[(int)_target.Energy % 10];
                case RenderLayer.Groups:
                    return _palette[_target.Group];
                case RenderLayer.Charges:
                    break;
            }

            return _palette[0];
        }

        private void RenderVelocityVector(Graphics g)
        {
            _vRenderer.Vector = _target.Velocity;
            _vRenderer.XPadding = _target.X;
            _vRenderer.YPadding = _target.Y;
            _vRenderer.Render(g);
        }

        #endregion
    }
}
