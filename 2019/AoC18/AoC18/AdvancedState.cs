using System;
using System.Drawing;
using System.Linq;

namespace AoC18
{
    public class AdvancedState : IEquatable<AdvancedState>
    {
        public readonly Point[] Position;
        public readonly bool[] Keys;
        public int Active;

        public AdvancedState()
        {
            Position = new Point[4];
            Keys = new bool[26];
        }
        
        public AdvancedState(Point[] position, bool[] keys, int active)
        {
            Position = position;
            Keys = keys;
            Active = active;
        }

        public override bool Equals(object? obj)
        {
            return Equals((AdvancedState)obj);
        }

        public bool Equals(AdvancedState other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Position.SequenceEqual(other.Position) && Keys.SequenceEqual(other.Keys) && Active == other.Active;
        }

        public override int GetHashCode()
        {
            var pointHash =  Position.Aggregate(27, (current, element) => current * 31 + element.GetHashCode());
            var keyHash = Keys.Aggregate(17, (current, element) => current * 31 + element.GetHashCode());
            return HashCode.Combine(pointHash, keyHash);
        }

        public static bool operator ==(AdvancedState left, AdvancedState right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AdvancedState left, AdvancedState right)
        {
            return !Equals(left, right);
        }
    }
}