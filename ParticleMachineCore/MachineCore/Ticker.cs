using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleMachineCore.MachineCore
{
    public class Ticker
    {
        #region Поля

        public int MinInterval = 0;
        public int MaxInterval = int.MaxValue;

        private bool _en;
        private int _interval = 20;
        private readonly Stopwatch _timer;
        private Task _task;

        private double _fps;
        private int _targetFps;

        #endregion

        #region Конструкторы

        public Ticker()
        {
            _timer = new Stopwatch();
        }

        #endregion

        #region Свойства

        /// <summary>
        /// Интервал, с которым таймер будет вызывать событие.
        /// </summary>
        public int Interval
        {
            get => _interval;
            set
            {
                if (value >= MinInterval && value <= MaxInterval)
                {
                    _interval = value;
                }
            }
        }

        /// <summary>
        /// Значение FPS, полученное в результате работы таймера.
        /// </summary>
        public double Fps => _fps;

        /// <summary>
        /// Устанавливает целевой FPS для таймера.
        /// </summary>
        public int TargetFps
        {
            get => _targetFps;
            set
            {
                if (value > 0)
                {
                    _targetFps = value;
                    _interval = 1000 / value;
                }
            }
        }

        /// <summary>
        /// Включен ли таймер.
        /// </summary>
        public bool Enabled => _en;

        /// <summary>
        /// Основное событие таймера.
        /// </summary>
        public event EventHandler Tick;

        #endregion

        #region Методы

        /// <summary>
        /// Запускает таймер.
        /// </summary>
        public void Start()
        {
            if (_en)
                return;

            _en = true;
            _task = Task.Run(() =>
            {
                _timer.Start();

                while (_en)
                {
                    Task.WaitAll(NextTick());
                }
            });
        }

        /// <summary>
        /// Останавливает таймер.
        /// </summary>
        public void Stop()
        {
            if (!_en)
                return;

            _en = false;
            _task.Dispose();
        }

        /// <summary>
        /// Считает значение, на которое нужно произвести преобразование за кадр,
        /// исходя из значения, ожидаемого за секунду.
        /// </summary>
        /// <param name="perSecond"></param>
        /// <returns></returns>
        public double IntoPerFrame(double perSecond)
        {
            return perSecond / _fps;
        }

        private async Task NextTick()
        {
            if (_timer.ElapsedMilliseconds >= _interval)
            {
                if (_timer.ElapsedMilliseconds > 0)
                {
                    _fps = 1000.0 / _timer.ElapsedMilliseconds;

                    _timer.Reset();
                    _timer.Start();

                    // асинхронность будто бы снижает нагрузку на цп
                    await Task.Run(() => OnTick());
                }
            }
        }

        private void OnTick()
        {
            Tick?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
