namespace Perennial.Plants.Data
{
    public readonly record struct Food(float Value)
    {
        public float Value { get; init; } = Value;
        
        public static explicit operator float(Food food) => food.Value;
        public static explicit operator Food(float value) => new Food(value);
        
        public static Food operator +(Food left, Food right) => new Food(left.Value + right.Value);
        public static Food operator -(Food left, float right) => new Food(left.Value - right);
        public static Food operator *(Food left, Food right) => new Food(left.Value * right.Value);
        public static Food operator *(Food left, float right) => new Food(left.Value * right);
        public static Food operator *(Food left, Lifetime right) => new Food(left.Value * right.Value);
    }
}
