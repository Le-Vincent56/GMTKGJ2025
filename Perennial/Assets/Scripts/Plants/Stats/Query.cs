namespace Perennial.Plants.Stats
{
    public class Query
    {
        public StatType Type { get; private set; }
        public float Value { get; set; }

        public Query(StatType statType, float value)
        {
            Type = statType;
            Value = value;
        }
    }
}