namespace Perennial.Plants.Data
{
    public readonly record struct StorageAmount(int Value)
    {
        public int Value { get; init; } = Value;
        private const int MINIMUM_STORAGE_AMOUNT = 0;
        
        public static explicit operator int(StorageAmount amount) => StayAboveMinimum(amount.Value);
        public static explicit operator StorageAmount(int value) => new StorageAmount(StayAboveMinimum(value));
        public static explicit operator string(StorageAmount amount) => amount.Value.ToString();
        public static explicit operator StorageAmount(float value) => new StorageAmount(StayAboveMinimum((int)value));
        
        public static StorageAmount operator +(StorageAmount left, StorageAmount right) => new StorageAmount(left.Value + right.Value);
        public static bool operator <(StorageAmount left, int right) => left.Value < right;
        public static bool operator >(StorageAmount left, int right) => left.Value > right;
        public static bool operator ==(StorageAmount left, int right) => left.Value == right;
        public static bool operator !=(StorageAmount left, int right) => left.Value != right;
        public static StorageAmount operator --(StorageAmount storageAmount)
        {
            int value = StayAboveMinimum(storageAmount.Value - 1);
            return new StorageAmount(value);
        }
        
        private static int StayAboveMinimum(int value)
        {
            return value < MINIMUM_STORAGE_AMOUNT 
                ? MINIMUM_STORAGE_AMOUNT 
                : value;
        }
    }
}
