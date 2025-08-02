using UnityEngine;

namespace Perennial.Plants.Data
{
    public readonly record struct EffectDuration(int Value)
    {
        public int Value { private get; init; } = Value;
        private const int MINIMUM_DURATION_LENGTH = 0;

        public static explicit operator int(EffectDuration effectDuration) => StayAboveMinimum(effectDuration.Value);
        public static explicit operator EffectDuration(int value) => new EffectDuration(StayAboveMinimum(value));
        
        public static bool operator >(EffectDuration left, int right) => left.Value > right;
        public static bool operator <(EffectDuration left, int right) => left.Value < right;
        public static bool operator >=(EffectDuration left, int right) => left.Value >= right;
        public static bool operator <=(EffectDuration left, int right) => left.Value <= right;
        public static bool operator ==(EffectDuration left, int right) => left.Value == right;
        public static bool operator !=(EffectDuration left, int right) => left.Value != right;

        public static EffectDuration operator --(EffectDuration effectDuration)
        {
            int value = StayAboveMinimum(effectDuration.Value - 1);
            return new EffectDuration(value);
        }
        
        private static int StayAboveMinimum(int value)
        {
            return value < MINIMUM_DURATION_LENGTH 
                ? MINIMUM_DURATION_LENGTH 
                : value;
        }
    }
}
