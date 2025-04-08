using ParticleMachineCore.MachineCore;
using ParticleMachineCore.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleMachineCore.ParticleModel
{
    public class Particle
    {
        #region Поля

        private readonly Guid _id;
        private int _group;
        private double _energy = 1;
        private double _spin = 1;
        private PointD _position;
        private VectorD _velocity;

        private readonly ParticlePhysics _physics;

        #endregion

        #region Конструкторы

        public Particle(ParticlePhysics physics)
        {
            if (physics == null)
                throw new ArgumentNullException(nameof(physics));

            _physics = physics;
            _id = Guid.NewGuid();
        }

        #endregion

        #region Свойства

        public Guid Identifier => _id;

        public ParticlePhysics Physics => _physics;

        public int Group
        {
            get => _group;
            set
            {
                if (value >= 0 && value <= ParticlePhysics.MaxGroup)
                    _group = value;
            }
        }

        public double Energy
        {
            get => _energy;
            set
            {
                if (value > 0)
                    _energy = value;
            }
        }

        public double Spin
        {
            get => _spin;
            set
            {
                if (value != 0)
                    _spin = value;
            }
        }

        public VectorD Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }

        public double VelocityX
        {
            get => _velocity.X;
            set => _velocity.X = value;
        }

        public double VelocityY
        {
            get => _velocity.Y;
            set => _velocity.Y = value;
        }

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

        #endregion

        #region Методы

        public void ApplyMove(VectorD vector)
        {
            X += vector.X;
            Y += vector.Y;
        }

        public double GetRadius()
        {
            return _physics.GetRadius(this);
        }

        #endregion
    }
}
