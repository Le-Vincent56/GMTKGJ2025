using Perennial.Core.Extensions;

namespace Perennial.Plants.Data
{
    public readonly record struct Seeds(SerializableGuid ID, int Value)
    {
        public SerializableGuid ID { get; init; } = ID;
        public int Value { get; init; } = Value;
        private const int MINIMUM_REWARD_LENGTH = 0;
        
        public static explicit operator int(Seeds seeds) => seeds.Value;
        public static explicit operator SerializableGuid(Seeds seeds) => seeds.ID;
        
        public static Seeds operator -(Seeds left, int right) => new Seeds(left.ID, StayAboveMinimum(left.Value - right));
        public static Seeds operator +(Seeds left, int right) => new Seeds(left.ID, left.Value + right);
        
        private static int StayAboveMinimum(int value)
        {
            return value < MINIMUM_REWARD_LENGTH 
                ? MINIMUM_REWARD_LENGTH 
                : value;
        }
    }
}
