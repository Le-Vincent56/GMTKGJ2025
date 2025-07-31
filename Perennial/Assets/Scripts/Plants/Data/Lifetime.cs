namespace Perennial.Plants.Data
{
    public readonly record struct Lifetime(float Value)
    {
        public float Value { private get; init; } = Value;
        
        public static explicit operator float(Lifetime lifetime) => lifetime.Value;
        public static explicit operator Lifetime(int value) => new Lifetime(value);
        public static explicit operator Lifetime(float value) => new Lifetime(value);
        
        public static Lifetime operator -(Lifetime left, float right) => new Lifetime(left.Value - right);
        public static Lifetime operator +(Lifetime left, float right) => new Lifetime(left.Value + right);
        
        public static Lifetime operator ++(Lifetime lifetime)
        {
            float value = lifetime.Value - 1f;
            return new Lifetime(value);
        }
    }
}
