namespace Perennial.Plants.Stats
{
    public enum StatType
    {
        FoodModifier,
        MutationChance,
        MutationDropRate,
    }
    
    public class PlantStats
    {
        private readonly PlantBaseStats _baseStats;

        public PlantStatsMediator Mediator { get; }

        public float FoodModifier
        {
            get
            {
                Query query = new Query(StatType.FoodModifier, _baseStats.FoodModifier);
                Mediator.PerformQuery(this, query);
                return query.Value;
            }
        }

        public float MutationChance
        {
            get
            {
                Query query = new Query(StatType.MutationChance, _baseStats.MutationChance);
                Mediator.PerformQuery(this, query);
                return query.Value;
            }
        }

        public float MutationDropRate
        {
            get
            {
                Query query = new Query(StatType.MutationDropRate, _baseStats.MutationDropRate);
                Mediator.PerformQuery(this, query);
                return query.Value;
            }
        }

        public PlantStats(PlantBaseStats baseStats)
        {
            _baseStats = baseStats;
            Mediator = new PlantStatsMediator();
        }
    }
}