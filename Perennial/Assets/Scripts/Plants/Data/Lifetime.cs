using System;

namespace Perennial.Plants.Data
{
    public readonly record struct Lifetime(float Value)
    {
        public float Value { get; init; } = Value;
        private const int MINIMUM_LIFETIME_LENGTH = 0;
        
        public static explicit operator float(Lifetime lifetime) => lifetime.Value;
        public static explicit operator Lifetime(int value) => new Lifetime(value);
        public static explicit operator Lifetime(float value) => new Lifetime(value);
        
        public static Lifetime operator -(Lifetime left, float right) => new Lifetime(StayAboveMinimum(left.Value - right));
        public static Lifetime operator +(Lifetime left, float right) => new Lifetime(left.Value + right);
        public static bool operator <(Lifetime left, Lifetime right) => left.Value < right.Value;
        public static bool operator <(Lifetime left, LifetimeSegment right) => left.Value < right.Value;
        public static bool operator <=(Lifetime left, Lifetime right) => left.Value <= right.Value;
        public static bool operator >(Lifetime left, Lifetime right) => left.Value > right.Value;
        public static bool operator >(Lifetime left, LifetimeSegment right) => left.Value > right.Value;
        public static bool operator >=(Lifetime left, Lifetime right) => left.Value >= right.Value;
        public static float operator /(Lifetime left, Lifetime right)
        {
            return right.Value == 0f 
                ? throw new DivideByZeroException("Error! Division by zero.") 
                : left.Value / right.Value;
        }
        
        public static Lifetime operator ++(Lifetime lifetime)
        {
            float value = lifetime.Value - 1f;
            return new Lifetime(value);
        }

        private static float StayAboveMinimum(float value)
        {
            return value < MINIMUM_LIFETIME_LENGTH 
                ? MINIMUM_LIFETIME_LENGTH 
                : value;
        }
    }
}
