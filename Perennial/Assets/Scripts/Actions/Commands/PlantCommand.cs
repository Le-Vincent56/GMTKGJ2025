using System.Threading.Tasks;
using Perennial.Garden;
using Perennial.Plants;
using UnityEngine;

namespace Perennial.Actions.Commands
{
    public class PlantCommand : BaseCommand
    {
        private readonly PlantDefinition _plantDefinition;

        public PlantDefinition PlantDefinition => _plantDefinition;
        public PlantCommand(PlantArgs input) : base(input)
        {
            _plantDefinition = input.PlantDefinition;
        }
        
        /// <summary>
        /// Executes the Plant action
        /// </summary>
        public override async Task Execute()
        {
            Plant plant = PlantFactory.CreatePlant(_plantDefinition, Tile, gardenManager);
            gardenManager.Plants.Add(plant);
            Tile.Plant = plant;
        }
    }
}
