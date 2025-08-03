using Perennial.Plants.Data;
using UnityEngine;

namespace Perennial.Plants
{
    public class PlantLifetime
    {
        private readonly Plant _owner;
        private readonly LifetimeSegment _growthTime;
        private readonly Lifetime _totalLifetime;
        private readonly Sprite _plantSprite;
        private bool _wasPreviouslyGrown;

        public bool FullyGrown => CurrentLifetime > _growthTime;
        public Lifetime CurrentLifetime { get; private set; }

        public PlantLifetime(Plant owner, int growTime, int harvestTime, Sprite plantSprite)
        {
            _owner = owner;
            _growthTime = (LifetimeSegment)growTime;
            CurrentLifetime = (Lifetime)0;
            _totalLifetime = (Lifetime)(growTime + harvestTime);
            _plantSprite = plantSprite;
            _wasPreviouslyGrown = false;
        }

        /// <summary>
        /// Grow the plant by increasing its lifetime
        /// </summary>
        public void Grow(int growthRate)
        {
            // Check if not previously grown
            if (!FullyGrown) _wasPreviouslyGrown = false;
            
            // Grow the plant
            CurrentLifetime += growthRate;

            // Check for sprite updates
            UpdateSprite();
        }

        /// <summary>
        /// Get the percentage of how far the plant has gone through it's lifetime
        /// </summary>
        public float GetPercentage() => CurrentLifetime / _totalLifetime;

        /// <summary>
        /// Reset the plant's lifetime to the start of harvest
        /// </summary>
        public void ResetToHarvestStart()
        {
            // Exit if not in the harvest state
            if (!FullyGrown) return;
            
            // Exit if not alive
            if (!IsAlive()) return;
            
            // Set the current lifetime to the start of the harvest time
            CurrentLifetime = (Lifetime)_growthTime;
        }

        /// <summary>
        /// Check if the plant is alive
        /// </summary>
        public bool IsAlive() => CurrentLifetime < _totalLifetime;

        /// <summary>
        /// Update the sprite of the Plant according to its lifetime
        /// </summary>
        private void UpdateSprite()
        {
            // Exit if the plant was previously grown;
            // prevents unnecessary sprite changes when already grown
            if (_wasPreviouslyGrown) return;
            
            // Exit if the plant is not grown;
            // The seed sprite is set at creation
            if (!FullyGrown) return;
            
            // Update to the plant sprite
            _owner.Tile.UpdatePlantSprite(_plantSprite);
        }
    }
}
