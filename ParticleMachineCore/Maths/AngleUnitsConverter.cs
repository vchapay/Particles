using System;

namespace ParticleMachineCore.Maths
{
    public static class AngleUnitsConverter
    {
        public static double DegreesToRadians(double angle)
        {
            return angle * (Math.PI / 180);
        }

        public static double RadiansToDegrees(double angle)
        {
            return angle * (180 / Math.PI);
        }
    }
}
