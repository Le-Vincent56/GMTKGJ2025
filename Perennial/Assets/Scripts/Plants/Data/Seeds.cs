using Perennial.Core.Extensions;

namespace Perennial.Plants.Data
{
    public readonly record struct Seeds(SerializableGuid ID, StorageAmount Value)
    {
        public SerializableGuid ID { get; init; } = ID;
        public StorageAmount Value { get; init; } = Value;
        private const int MINIMUM_REWARD_LENGTH = 0;
        
        public static explicit operator int(Seeds seeds) => (int)seeds.Value;
        public static explicit operator StorageAmount(Seeds seeds) => seeds.Value;
        public static explicit operator SerializableGuid(Seeds seeds) => seeds.ID;
        
        public static Seeds operator -(Seeds left, StorageAmount right) => new Seeds(left.ID, (StorageAmount)StayAboveMinimum((int)left.Value - (int)right));
        public static Seeds operator +(Seeds left, StorageAmount right) => new Seeds(left.ID, left.Value + (StorageAmount)right);
        public static Seeds operator -(Seeds left, int right) => new Seeds(left.ID, (StorageAmount)StayAboveMinimum((int)left.Value - right));
        public static Seeds operator +(Seeds left, int right) => new Seeds(left.ID, left.Value + (StorageAmount)right);
        
        private static int StayAboveMinimum(int value)
        {
            return value < MINIMUM_REWARD_LENGTH 
                ? MINIMUM_REWARD_LENGTH 
                : value;
        }
    }
}
