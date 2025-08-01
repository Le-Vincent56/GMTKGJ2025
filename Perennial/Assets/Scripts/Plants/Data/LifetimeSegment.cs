using UnityEngine;

namespace Perennial.Plants.Data
{
    public readonly record struct LifetimeSegment(int Value)
    {
        public int Value { get; init; } = Value;
        private const int MINIMUM_SEGMENT_LENGTH = 0;
        
        public static explicit operator int(LifetimeSegment segment) => StayAboveMinimum(segment.Value);
        public static explicit operator LifetimeSegment(int value) => new LifetimeSegment(StayAboveMinimum(value));

        
        private static int StayAboveMinimum(int value)
        {
            return value < MINIMUM_SEGMENT_LENGTH 
                ? MINIMUM_SEGMENT_LENGTH 
                : value;
        }
    }
}
