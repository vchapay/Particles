using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ParticleMachineCore.MachineCore
{
    public class LightTicker
    {
        #region Поля

        private Timer _timer;
        private readonly Stopwatch _stopWatch;
        private int _interval;
        private Task _curTask;

        private double _fps;
        private int _targetFps;

        #endregion

        #region Конструкторы

        public LightTicker()
        {
            _timer = new Timer();
            _stopWatch = new Stopwatch();
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
                if (value > 0)
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
                if (value > 0 && value <= 1000)
                {
                    _targetFps = value;
                    _interval = 1000 / value;
                }
            }
        }

        /// <summary>
        /// Включен ли таймер.
        /// </summary>
        public bool Enabled => _timer.Enabled;

        #endregion

        #region Методы

        public void Start(Action action)
        {
            _timer = new Timer(_interval);
            _timer.Elapsed += (_, __) =>
            {
                if (_curTask != null && !_curTask.IsCompleted)
                    return;

                _fps = 1000.0 / _stopWatch.ElapsedMilliseconds;
                _stopWatch.Reset();
                _stopWatch.Start();
                _curTask = Task.Run(action);
            };

            _timer.Start();
        }

        public void Stop()
        {
            if (_timer != null && !_timer.Enabled)
                _timer.Stop();
        }

        #endregion

        #region Вложенные типы

        #endregion
    }
}
