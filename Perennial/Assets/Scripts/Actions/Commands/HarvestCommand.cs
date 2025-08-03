using System.Threading.Tasks;
using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using Perennial.Plants;
using Perennial.Plants.Data;
using UnityEngine;


namespace Perennial.Actions.Commands
{
    public class HarvestCommand : BaseCommand
    {
        //reference to the plant this exists as the way I set up PlantStorage to listen it happens to sometimes trigger after plant is cleared from tile
        public Plant HarvestedPlant { get; }
        public HarvestCommand(HarvestArgs input) : base(input)
        {
            HarvestedPlant = input.Tile.Plant;
        }
        
        /// <summary>
        /// Executes the Harvest  action
        /// </summary>
        public override async Task Execute()
        {
            HarvestedPlant.Harvest(out (Food Food, Seeds BaseSeeds, Seeds? MutationSeeds) rewards);
            gardenManager.RemovePlantFromTile(Tile);
            
            // Increase the number of food
            EventBus<AddFood>.Raise(new AddFood()
            {
                Amount = rewards.Food
            });
            
            // Increase the number of base seeds
            EventBus<StorePlant>.Raise(new StorePlant()
            {
                ID = rewards.BaseSeeds.ID,
                Quantity = rewards.BaseSeeds.Value
            });

            // Exit if there are no mutation seeds
            if (!rewards.MutationSeeds.HasValue) return;
            
            // Increase the number of base seeds
            EventBus<StorePlant>.Raise(new StorePlant()
            {
                ID = rewards.MutationSeeds.Value.ID,
                Quantity = rewards.MutationSeeds.Value.Value
            });
        }
    }
}
