using System;
using System.Drawing;
using System.Linq;

namespace AoC18
{
    public class State : IEquatable<State>
    {
        public Point Position;
        public readonly bool[] Keys;

        public State(Point position, bool[] keys)
        {
            Position = position;
            Keys = new bool[26];

            Array.Copy(keys, Keys, keys.Length);
        }

        public override bool Equals(object? obj)
        {
            return Equals((State)obj);
        }

        public bool Equals(State other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Position.Equals(other.Position) && Keys.SequenceEqual(other.Keys);
        }

        public override int GetHashCode()
        {
            var pointHash = HashCode.Combine(Position.X, Position.Y);
            var keyHash = Keys.Aggregate(17, (current, element) => current * 31 + element.GetHashCode());
            return HashCode.Combine(pointHash, keyHash);
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