namespace Perennial.Plants.Stats
{
    public enum StatType
    {
        GrowthRate,
    }
    
    public class PlantStats
    {
        private readonly PlantBaseStats _baseStats;

        public PlantStatsMediator Mediator { get; }

        public float GrowthRate
        {
            get
            {
                Query query = new Query(StatType.GrowthRate, _baseStats.GrowthRate);
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