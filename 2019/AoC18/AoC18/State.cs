using System;
using System.Collections;
using System.Drawing;

namespace AoC18
{
    public class State : IEquatable<State>
    {
        public readonly Point Position;
        public readonly BitArray Keys;

        public State(Point position, ICloneable keys)
        {
            Position = position;
            Keys = (BitArray)keys.Clone();
        }

        public override bool Equals(object? obj)
        {
            return Equals((State)obj);
        }

        public bool Equals(State other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return Position.Equals(other.Position) && Keys.BitEquals(other.Keys);
        }

        public override int GetHashCode()
        {
            var pointHash = HashCode.Combine(Position.X, Position.Y);
            
            var hash = new int[1];
            Keys.CopyTo(hash, 0);

            return pointHash + hash[0];
        }

        public static bool operator ==(State left, State right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(State left, State right)
        {
            return !Equals(left, right);
        }
    }
}