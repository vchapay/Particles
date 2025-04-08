using ParticleMachineCore.MachineCore;
using ParticleMachineCore.Maths;
using ParticleMachineCore.ParticleModel;
using ParticleMachineCore.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ParticleMachineWF
{
    public partial class Form1 : Form
    {
        private Machine _machine;
        private readonly List<double> _lastFPSValues;
        private readonly Ticker _averageFPSticker;
        private bool _fpsListLocked;

        public Form1()
        {
            InitializeComponent();
            _lastFPSValues = new List<double>();

            _averageFPSticker = new Ticker()
            {
                Interval = 1000
            };

            _averageFPSticker.Tick += UpdateAverageFPS;

            InitMachine();
            _averageFPSticker.Start();
        }

        private void InitMachine()
        {
            ParticlePhysics physics = new ParticlePhysics()
            {
                Conservation = 0.98
            };

            MachineSettings sets = new MachineSettings(physics);

            _machine = new Machine(sets);
            MachineRenderer renderer = new MachineRenderer()
            {
                Machine = _machine,
            };

            renderer.ParticleRenderer.ShowVelocityVector = false;
            renderer.ParticleRenderer.Layer = RenderLayer.Groups;

            _screen.Renderer = renderer;
            _machine.WorldShape = _screen.ClientRectangle;
            _machine.FrameChanged += Render;
            _machine.FillRandom(140);
            _machine.FillGroupsMatrixRandom();

            _machine.Run();
        }

        private void UpdateAverageFPS(object sender, EventArgs e)
        {
            _fpsListLocked = true;
            if (_lastFPSValues.Count == 0)
                return;

            SafetyCall(_fpsLbl,
                () => _fpsLbl.Text =
                $"FPS: ~{Math.Round(_lastFPSValues.Average(), 2)}");
            SafetyCall(_framesCounter,
                () => _framesCounter.Text = $"Frame: {_machine.Frame}");

            _lastFPSValues.Clear();
            _fpsListLocked = false;
        }

        private void Render(object sender, FrameArgs e)
        {
            SafetyCall(_screen,
                () => _screen.Invalidate());

            if (!_fpsListLocked) _lastFPSValues.Add(e.Fps);
        }

        private void SafetyCall(Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
        }
    }
}
