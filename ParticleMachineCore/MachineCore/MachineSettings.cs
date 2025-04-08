using ParticleMachineCore.Maths;
using ParticleMachineCore.ParticleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleMachineCore.MachineCore
{
    /// <summary>
    /// Описывает настройки автомата частиц.
    /// </summary>
    public class MachineSettings
    {
        #region Поля

        private readonly ParticlePhysics _physics;

        #endregion

        #region Конструкторы

        public MachineSettings(ParticlePhysics physics)
        {
            if (physics == null)
                throw new ArgumentNullException(nameof(physics));

            _physics = physics;
        }

        #endregion

        #region Свойства

        /// <summary>
        /// Задает физику автомата.
        /// </summary>
        public ParticlePhysics Physics => _physics;

        #endregion

        #region Методы

        #endregion
    }
}
