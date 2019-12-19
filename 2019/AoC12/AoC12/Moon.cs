using System;

namespace AoC12
{
    public class Moon
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public int XVelocity { get; set; }
        public int YVelocity { get; set; }
        public int ZVelocity { get; set; }
        
        public int KineticEnergy
        {
            get { return Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z); }
        }

        public int PotentialEnergy
        {
            get { return Math.Abs(XVelocity) + Math.Abs(YVelocity) + Math.Abs(ZVelocity); }
        }

        public int TotalEnergy
        {
            get { return KineticEnergy * PotentialEnergy; }
        }

        public Moon(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        
        public override string ToString()
        {
            return $"pos=<x={X.ToString().PadLeft(2,' ')}, y={Y.ToString().PadLeft(2,' ')}, Z={Z.ToString().PadLeft(2,' ')}>, vel=<x={XVelocity.ToString().PadLeft(2,' ')}, y={YVelocity.ToString().PadLeft(2,' ')}, z={ZVelocity.ToString().PadLeft(2,' ')}>";
        }
    }
}