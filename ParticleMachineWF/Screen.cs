using ParticleMachineCore.MachineCore;
using ParticleMachineCore.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParticleMachineWF
{
    public class Screen : Control
    {
        #region Константы

        public readonly Color DefaultBGColor = Color.FromArgb(21, 13, 56);

        #endregion

        #region Поля

        private MachineRenderer _renderer;

        #endregion

        #region Конструкторы

        public Screen()
        {
            SetStyle(ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Opaque,
                true);

            DoubleBuffered = true;
            BackColor = DefaultBGColor;
        }

        #endregion

        #region Свойства

        public MachineRenderer Renderer
        {
            get => _renderer;
            set => _renderer = value;
        }

        #endregion

        #region Методы

        protected override void OnResize(EventArgs e)
        {
            if (_renderer == null || _renderer.Machine == null)
                return;

            _renderer.Machine.WorldShape = ClientRectangle;
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(BackColor);
            _renderer?.Render(g);
        }

        #endregion

        #region Вложенные типы

        #endregion
    }
}
