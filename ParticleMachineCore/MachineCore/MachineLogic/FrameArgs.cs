using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleMachineCore.MachineCore
{
    public class FrameArgs : EventArgs
    {
        public FrameArgs(int index, double fps)
        {
            Index = index;
            Fps = fps;
        }

        public int Index { get; }

        public double Fps { get; }
    }
}
