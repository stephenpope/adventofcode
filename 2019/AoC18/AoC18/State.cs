using System;

namespace AoC18
{
    public struct State
    {
        public ulong Position;
        public long Keys;
        public int Active;

        public override bool Equals(object? obj)
        {
            var state = (State) obj;
            return Position == state.Position && Keys == state.Keys && Active == state.Active;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Keys, Active);
        }
    }
}