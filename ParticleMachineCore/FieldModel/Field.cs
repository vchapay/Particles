using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleMachineCore.FieldModel
{
    public class Field
    {
        #region Поля

        private double _tension = 1;
        private double _dispersal = 1;

        #endregion

        #region Конструкторы

        public Field() { }

        #endregion

        #region Свойства

        /// <summary>
        /// Название поля.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Определяет напряженность поля.
        /// </summary>
        public double Tension
        {
            get => _tension;
            set
            {
                if (value >= 0)
                {
                    _tension = value;
                }
            }
        }

        /// <summary>
        /// Определяет коэффицент рассеивания поля.
        /// </summary>
        public double Dispersal
        {
            get => _dispersal;
            set
            {
                if (value >= 0)
                {
                    _dispersal = value;
                }
            }
        }

        #endregion
    }
}
