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
    /// Описывает автомат частиц.
    /// </summary>
    public class Machine
    {
        #region Поля

        private readonly MachineSettings _settings;
        private readonly ParticlePhysics _physics;
        private readonly List<Particle> _particles;
        private readonly List<Particle> _start;

        private int _frame;
        private readonly Ticker _ticker;
        private readonly List<Request> _requests;

        private readonly Random _rnd;

        #endregion

        #region Конструкторы

        public Machine(MachineSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            _settings = settings;
            _physics = _settings.Physics;
            _particles = new List<Particle>();

            _ticker = new Ticker()
            {
                TargetFps = 60
            };

            _ticker.Tick += Update;
            _requests = new List<Request>();
            _rnd = new Random();

            _start = new List<Particle>();
        }

        #endregion

        #region Свойства

        /// <summary>
        /// Частицы, расположенные на поле автомата на текущий кадр.
        /// </summary>
        public List<Particle> Particles => _particles;

        /// <summary>
        /// Текущий кадр автомата.
        /// </summary>
        public int Frame => _frame;

        /// <summary>
        /// Определяет прямоугольные границы мира автомата.
        /// </summary>
        public RectangleD WorldShape { get; set; }

        /// <summary>
        /// Происходит при смене кадра.
        /// </summary>
        public event FrameHandler FrameChanged;

        #endregion

        #region Методы

        /// <summary>
        /// Запускает автомат.
        /// </summary>
        public void Run()
        {
            if (_ticker.Enabled)
                return;

            _frame = 0;
            _particles.Clear();
            _particles.AddRange(_start);
            _ticker.Start();
        }

        /// <summary>
        /// Случайно разбрасывает заданное число частиц по полю автомата.
        /// Используется только в выключенном состоянии автомата.
        /// Задает список-инициализатор автомата.
        /// </summary>
        /// <param name="count"></param>
        public void FillRandom(int count)
        {
            if (_ticker.Enabled)
                return;

            _start.Clear();
            for (int i = 0; i < count; i++)
            {
                Particle p = new Particle(_physics)
                {
                    X = _rnd.Next(0, (int)WorldShape.Width),
                    Y = _rnd.Next(0, (int)WorldShape.Height),
                    Group = _rnd.Next(0, 3),
                };

                double spin = _rnd.Next(0, 2) == 0 ? 1 : -1;
                p.Spin = spin * _physics.PrimarySpin;

                _start.Add(p);
            }
        }

        /// <summary>
        /// Формирует случайную матрицу правил для групп частиц.
        /// </summary>
        public void FillGroupsMatrixRandom()
        {
            for (int i = 0; i < ParticlePhysics.MaxGroup; i++)
            {
                for (int j = 0; j < ParticlePhysics.MaxGroup; j++)
                {
                    _physics.GroupsMatrix[i, j] =
                        _rnd.Next(-1, 1);
                }
            }
        }

        private void NextFrame()
        {
            _requests.Clear();
            ParallelCalculateFrame();
            ApplyFrame();
            _frame++;
        }

        private void Update(object sender, EventArgs e)
        {
            NextFrame();
            OnFrameChanged(new FrameArgs(_frame, _ticker.Fps));
        }

        private void OnFrameChanged(FrameArgs e)
        {
            FrameChanged?.Invoke(this, e);
        }

        private void ParallelCalculateFrame()
        {
            ParallelOptions ops = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 3
            };

            Parallel.For(0, _particles.Count, (i) =>
            {
                Particle p = _particles[i];
                VectorD frameVelocity = ToVectorPerFrame(p.Velocity);
                VectorD accelerate = new VectorD();

                Task.WaitAll(Task.Run(() => 
                    Parallel.For(0, _particles.Count, (j) =>
                    {
                        Particle infl = _particles[j];
                        if (p != infl)
                        {
                            VectorD next = _physics.GetInteraction(p, infl);

                            double velLimitCoef = _physics.
                                GetLimitVelocityCoef(p.Velocity.Length, next.Length);

                            next.SetLength(velLimitCoef * next.Length
                                / p.Energy);

                            accelerate += next;
                        }
                    }))
                );

                frameVelocity += ToVectorPerFrame(accelerate);
                if (!CollideWorldShape(p, frameVelocity))
                {
                    VelocityRequest vel = new VelocityRequest(p, accelerate);
                    _requests.Add(vel);

                    MovingRequest move = new MovingRequest(p, frameVelocity);
                    _requests.Add(move);
                }
            });
        }

        private void CalculateFrame()
        {
            for (int i = 0; i < _particles.Count; i++)
            {
                Particle p = _particles[i];
                VectorD frameVelocity = ToVectorPerFrame(p.Velocity);
                VectorD accelerate = new VectorD();

                for(int j = 0; j < _particles.Count; j++)
                {
                    Particle infl = _particles[j];
                    if (p == infl)
                        continue;

                    VectorD next = _physics.GetInteraction(p, infl);

                    double velLimitCoef = _physics.
                        GetLimitVelocityCoef(p.Velocity.Length, next.Length);

                    next.SetLength(velLimitCoef * next.Length
                        / p.Energy);

                    accelerate += next;
                }

                frameVelocity += ToVectorPerFrame(accelerate);
                if (!CollideWorldShape(p, frameVelocity))
                {
                    VelocityRequest vel = new VelocityRequest(p, accelerate);
                    _requests.Add(vel);

                    MovingRequest move = new MovingRequest(p, frameVelocity);
                    _requests.Add(move);
                }
            }
        }

        private void ApplyFrame()
        {
            foreach (Request request in _requests)
            {
                request.Apply();
            }
        }

        private bool CollideWorldShape(Particle p, VectorD frameVector)
        {
            bool inX = CollideInX(p, frameVector);
            bool inY = CollideInY(p, frameVector);

            return inX || inY;
        }

        private bool CollideInY(Particle p, VectorD frameVector)
        {
            double radius = _physics.GetRadius(p);
            if (p.Y + frameVector.Y + radius > WorldShape.Bottom)
            {
                double newY = WorldShape.Bottom - radius - 1;
                var bounceReq = new BounceYRequest(p, newY);
                _requests.Add(bounceReq);
                return true;
            }

            else if (p.Y + frameVector.Y < WorldShape.Top)
            {
                double newY = WorldShape.Top + 1;
                var bounceReq = new BounceYRequest(p, newY);
                _requests.Add(bounceReq);
                return true;
            }

            return false;
        }

        private bool CollideInX(Particle p, VectorD frameVector)
        {
            double radius = _physics.GetRadius(p);
            if (p.X + frameVector.X + radius > WorldShape.Right)
            {
                double newX = WorldShape.Right - radius - 1;
                var bounceReq = new BounceXRequest(p, newX);
                _requests.Add(bounceReq);
                return true;
            }

            else if (p.X + frameVector.X < WorldShape.Left)
            {
                double newX = WorldShape.Left + 1;
                var bounceReq = new BounceXRequest(p, newX);
                _requests.Add(bounceReq);
                return true;
            }

            return false;
        }

        private VectorD ToVectorPerFrame(VectorD v)
        {
            VectorD res = new VectorD()
            {
                X = v.X / _ticker.Fps,
                Y = v.Y / _ticker.Fps,
            };

            return res;
        }

        #endregion

        #region Вложенные типы

        private abstract class Request
        {
            protected readonly Particle Target;

            public Request(Particle p)
            {
                Target = p;
            }

            public abstract void Apply();
        }

        private class VelocityRequest : Request
        {
            private readonly VectorD _value;

            public VelocityRequest(Particle target, VectorD value) : base(target)
            {
                _value = value;
            }

            public override void Apply()
            {
                Target.Velocity *= Target.Physics.Conservation;
                Target.Velocity += _value;
            }
        }

        private class MovingRequest : Request
        {
            private readonly VectorD _value;

            public MovingRequest(Particle target, VectorD value) : base(target)
            {
                _value = value;
            }

            public override void Apply()
            {
                Target.ApplyMove(_value);
            }
        }

        private class BounceXRequest : Request
        {
            private readonly double _value;

            public BounceXRequest(Particle target, double value) : base(target)
            {
                _value = value;
            }

            public override void Apply()
            {
                Target.X = _value;
                Target.VelocityX *= -1;
                Target.Velocity *= Target.Physics.Conservation;
            }
        }

        private class BounceYRequest : Request
        {
            private readonly double _value;

            public BounceYRequest(Particle target, double value) : base(target)
            {
                _value = value;
            }

            public override void Apply()
            {
                Target.Y = _value;
                Target.VelocityY *= -1;
                Target.Velocity *= Target.Physics.Conservation;
            }
        }

        #endregion
    }
}
