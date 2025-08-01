using Perennial.Plants.Data;

namespace Perennial.Plants
{
    public class PlantLifetime
    {
        private readonly LifetimeSegment _growthTime;
        private readonly Lifetime _totalLifetime;

        public bool FullyGrown => RoundsAlive > _growthTime;
        public Lifetime RoundsAlive { get; private set; }

        public PlantLifetime(int growTime, int harvestTime)
        {
            _growthTime = (LifetimeSegment)growTime;
            RoundsAlive = (Lifetime)0;
            _totalLifetime = (Lifetime)(growTime + harvestTime);
        }

        /// <summary>
        /// Grow the plant by increasing its lifetime
        /// </summary>
        public void Grow(float growthRate)
        {
            RoundsAlive += growthRate;
        }

        /// <summary>
        /// Get the percentage of how far the plant has gone through it's lifetime
        /// </summary>
        public float GetPercentage() => RoundsAlive / _totalLifetime;

        /// <summary>
        /// Check if the plant is alive
        /// </summary>
        public bool IsAlive() => RoundsAlive < _totalLifetime;
    }
}
