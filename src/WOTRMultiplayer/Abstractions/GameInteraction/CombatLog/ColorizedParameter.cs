using UnityEngine;

namespace WOTRMultiplayer.Abstractions.GameInteraction.CombatLog
{
    public abstract class ColorizedParameter
    {
        public string Value { get; private set; }

        public Color Color { get; private set; }

        protected ColorizedParameter(string value, Color color)
        {
            Value = value;
            Color = color;
        }
    }
}
