using ParticleMachineCore.FieldModel;
using ParticleMachineCore.ParticleModel;
using System;

namespace ParticleMachineCore.Maths
{
    /// <summary>
    /// Описывает физику автомата.
    /// </summary>
    public class ParticlePhysics
    {
        #region Поля

        /// <summary>
        /// Максимальное число групп.
        /// </summary>
        public const int MaxGroup = 10;

        private double _conservation = 1;
        private double _massDistorsion = 2;
        private double _maxVelocity = 10000;
        private float _primarySpin = 1 / 2f;
        private double _primaryRadius = 4;

        private readonly Field _gravityField;
        private readonly Field _collisionField;
        private readonly Field _electicityField;
        private readonly Field _spinField;
        private readonly Field _groupField;

        private readonly double[,] _groupsMatrix;

        #endregion

        #region Конструкторы

        public ParticlePhysics()
        {
            _gravityField = new Field() 
            {
                Tension = 0.001,
                Dispersal = 70
            };

            _collisionField = new Field()
            {
                Dispersal = 0.5,
                Tension = 100,
            };

            _electicityField = new Field();

            _spinField = new Field()
            {
                Dispersal = 40,
                Tension = 0.5,
            };

            _groupField = new Field()
            {
                Tension = 0.3,
                Dispersal = 40
            };

            _groupsMatrix = new double[MaxGroup, MaxGroup];
        }

        #endregion

        #region Свойства

        /// <summary>
        /// Задает коэффициент сохранения скоростей (в диапазоне между 0 и 1).
        /// Каждый кадр скорость умножается на коэффициент.
        /// Чем меньше коэффициент сохранения, тем сильнее гаснет
        /// скорость за кадр.
        /// </summary>
        public double Conservation
        {
            get => _conservation;
            set
            {
                if (value > 0 && value <= 1)
                {
                    _conservation = value;
                }
            }
        }

        /// <summary>
        /// Определяет гравитационное искривление частицей массой 1.
        /// </summary>
        public double MassDistorsion
        {
            get => _massDistorsion;
            set
            {
                if (value > 0)
                {
                    _massDistorsion = value;
                }
            }
        }

        /// <summary>
        /// Определяет максимальную скорость, которую частицы
        /// могут достигнуть (пикс / сек)
        /// </summary>
        public double MaxVelocity
        {
            get => _maxVelocity;
            set
            {
                if (value > 0)
                {
                    _maxVelocity = value;
                }
            }
        }

        /// <summary>
        /// Определяет спин примитива.
        /// </summary>
        public float PrimarySpin
        {
            get => _primarySpin;
            set
            {
                if (value > 0)
                {
                    _primarySpin = value;
                }
            }
        }

        /// <summary>
        /// Определяет радиус примитива (пикс).
        /// </summary>
        public double PrimaryRadius
        {
            get => _primaryRadius;
            set => _primaryRadius = value;
        }

        /// <summary>
        /// Задает физику гравитационного поля.
        /// </summary>
        public Field GravityField => _gravityField;

        /// <summary>
        /// Задает физику коллизионного поля.
        /// </summary>
        public Field CollisionField => _collisionField;

        /// <summary>
        /// Задает физику электро-магнитного поля.
        /// </summary>
        public Field ElectricityField => _electicityField;

        /// <summary>
        /// Задает физику взаимодействия спинов.
        /// </summary>
        public Field SpinField => _spinField;

        /// <summary>
        /// Задает физику взаимодействия групп.
        /// </summary>
        public Field GroupField => _groupField;

        /// <summary>
        /// Задает матрицу, описывающую специальные коэффициенты
        /// напряженности полей разных групп.
        /// </summary>
        public double[,] GroupsMatrix => _groupsMatrix;

        #endregion

        #region Методы

        /// <summary>
        /// Возвращает вектор силы, приложенной
        /// к данной частице другой частицей в результате взаимодействия.
        /// </summary>
        /// <param name="p"> Частица, на которую воздействует сила. </param>
        /// <param name="infl"> Воздействующая частица. </param>
        /// <returns> Вектор силы воздействия. </returns>
        public VectorD GetInteraction(Particle p, Particle infl)
        {
            VectorD distVector = ConstructVector(p, infl);
            double distance = distVector.Length;
            VectorD result = new VectorD(0, 1);
            result.SetAngle(distVector.XAngle);
            result += GetGravityForce(infl.Energy, distance);
            result += GetCollisionForce(GetRadius(infl), distance);
            result += GetSpinForce(p.Spin, infl.Spin, distance);
            result += GetGroupForce(p.Group, infl.Group, distance);
            // другие поля

            // инициализация проводилась ненулевым вектором,
            // чтобы возможно было задать направление,
            // теперь нужно отнять лишнюю силу
            result += -1;
            return result;
        }

        /// <summary>
        /// Возвращает радиус частицы.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public double GetRadius(Particle p)
        {
            return Math.Sqrt(p.Energy) * _primaryRadius;
        }

        /// <summary>
        /// Возвращает коэффициент, ограничивающий рост скорости.
        /// </summary>
        /// <param name="vel"></param>
        /// <param name="acc"></param>
        /// <returns></returns>
        public double GetLimitVelocityCoef(double vel, double acc)
        {
            double coef = (_maxVelocity - vel) * (_maxVelocity - acc)
                / (_maxVelocity * _maxVelocity);

            return coef;
        }

        private double GetGravityForce(double mass, double distance)
        {
            double tension = mass * _massDistorsion * _gravityField.Tension;
            return GetFieldForce(tension,
                _gravityField.Dispersal, distance);
        }

        private double GetCollisionForce(double radius, double distance)
        {
            double dispersal = 4 * radius / Math.PI * _collisionField.Dispersal;
            return -GetFieldForce(_collisionField.Tension,
                dispersal, distance);
        }

        private double GetElectricityForce(double pCharge,
            double inflCharge, double distance)
        {
            if (pCharge == inflCharge)
                return 0;

            double sign = inflCharge * pCharge > 0 ? -1 : 1;
            double tension = pCharge * inflCharge * _electicityField.Tension;
            return sign * GetFieldForce(tension,
                _electicityField.Dispersal, distance);
        }

        private double GetSpinForce(double pSpin,
            double inflSpin, double distance)
        {
            double sign = inflSpin * pSpin > 0 ? -1 : 1;
            double tension = Math.Abs(inflSpin - pSpin) * _spinField.Tension;
            return sign * GetFieldForce(tension,
                _spinField.Dispersal, distance);
        }

        private double GetGroupForce(int pGroup,
            int inflGroup, double distance)
        {
            double coef = _groupsMatrix[pGroup, inflGroup];
            int sign = coef > 0 ? 1 : -1;
            double tension = coef * _groupField.Tension;
            return sign * GetFieldForce(tension,
                _groupField.Dispersal, distance);
        }

        private double GetFieldForce(double tension,
            double dispersal, double distance)
        {
            if (distance >= Math.PI * dispersal)
                return 0;

            return Math.Abs(-tension * dispersal
                * Math.Sin(1 / dispersal * distance));
        }

        /// <summary>
        /// Строит вектор от центра первой частицы до центра второй.
        /// Если частицы совпадают, возвращает вектор скорости первой частицы.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>
        /// Вектор, показывающий расположение
        /// одной частиц относительно другой.
        /// </returns>
        public static VectorD ConstructVector(Particle start, Particle end)
        {
            VectorD v = new VectorD()
            {
                X = end.X - start.X,
                Y = end.Y - start.Y
            };

            if (double.IsNaN(v.XAngle))
                return start.Velocity;

            return v;
        }

        #endregion
    }
}
