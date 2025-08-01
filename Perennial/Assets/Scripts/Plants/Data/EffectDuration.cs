using UnityEngine;

namespace Perennial.Plants.Data
{
    public readonly record struct EffectDuration(int Value)
    {
        public int Value { private get; init; } = Value;

        public static explicit operator int(EffectDuration effectDuration) => effectDuration.Value;
        public static explicit operator EffectDuration(int value) => new(value);
        
        public static bool operator >(EffectDuration left, int right) => left.Value > right;
        public static bool operator <(EffectDuration left, int right) => left.Value < right;
        public static bool operator >=(EffectDuration left, int right) => left.Value >= right;
        public static bool operator <=(EffectDuration left, int right) => left.Value <= right;
        public static bool operator ==(EffectDuration left, int right) => left.Value == right;
        public static bool operator !=(EffectDuration left, int right) => left.Value != right;

        public static EffectDuration operator --(EffectDuration effectDuration)
        {
            int value = effectDuration.Value - 1;
            return new EffectDuration(value);
        }
    }
}
