using Perennial.Plants.Data;

namespace Perennial.Plants
{
    public class PlantRewards
    {
        private readonly Plant _owner;
        private const float BONUS_MODIFIER = 1.5f;
        private readonly Food _foodMultiplier;
        private readonly Food _foodConstant;
        
        public bool BonusActive { get; set; }

        public PlantRewards(Plant owner, int foodMultiplier, int foodConstant)
        {
            _owner = owner;
            BonusActive = false;
            _foodMultiplier = (Food)foodMultiplier;
            _foodConstant = (Food)foodConstant;
        }

        /// <summary>
        /// Calculate the amount of food to reward the player
        /// </summary>
        public Food CalculateFood()
        {
            Food baseAmount = (_foodMultiplier * _owner.Lifetime.CurrentLifetime) + _foodConstant;

            if(BonusActive) baseAmount *= BONUS_MODIFIER;
            
            // Apply food modifier from stats
            float foodModifier = _owner.Stats.FoodModifier;
            baseAmount += (Food)foodModifier;

            return baseAmount;
        }
    }
}
