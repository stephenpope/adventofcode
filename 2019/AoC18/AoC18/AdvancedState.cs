using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AoC18
{
    public class AdvancedState : IEquatable<AdvancedState>
    {
        public readonly Point[] Position;
        public readonly BitArray Keys;
        public readonly int Active;

        public AdvancedState(Point[] position, BitArray keys, int active)
        {
            Position = position;
            Keys = keys;
            Active = active;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"Active: {Active} - ");
            
            sb.Append($"{Position[Active]} - Keys: {Keys.Cast<bool>().Count(x => x)}");
            
            return sb.ToString();
        }

        public override bool Equals(object? obj)
        {
            return Equals((AdvancedState) obj);
        }

        public bool Equals(AdvancedState other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            for (var i = 0; i < 4; i++)
            {
                if (Position[i].X != other.Position[i].X || Position[i].Y != other.Position[i].Y)
                {
                    return false;
                }
            }

            return Keys.BitEquals(other.Keys) && Active == other.Active;
        }

        public override int GetHashCode()
        {
            var pointHash = Position.Aggregate(27, (current, element) => current * 31 + element.GetHashCode());

            var hash = new int[1];
            Keys.CopyTo(hash, 0);

            return pointHash + hash[0] + Active;
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