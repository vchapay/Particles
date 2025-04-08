using ParticleMachineCore.MachineCore;
using ParticleMachineCore.ParticleModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleMachineCore.Rendering
{
    public class MachineRenderer
    {
        #region Поля

        private readonly ParticleRenderer _particleRenderer;

        #endregion

        #region Конструкторы

        public MachineRenderer()
        {
            _particleRenderer = new ParticleRenderer()
            {
            };
        }

        #endregion

        #region Свойства

        public Machine Machine { get; set; }

        public ParticleRenderer ParticleRenderer => _particleRenderer;

        #endregion

        #region Методы

        public void Render(Graphics g)
        {
            if (Machine == null)
                return;

            foreach (Particle p in Machine.Particles)
            {
                _particleRenderer.Target = p;
                _particleRenderer.Render(g);
            }
        }

        #endregion

        #region Вложенные типы

        #endregion
    }
}
