using System;

namespace AoC18
{
    public static class Extensions
    {
        public static AdvancedState Clone(this AdvancedState state)
        {
            var newState = new AdvancedState();
            Array.Copy(state.Position, newState.Position, state.Position.Length);
            Array.Copy(state.Keys, newState.Keys, state.Keys.Length);
            newState.Active = state.Active;
            
            return newState;
        }
    }
}