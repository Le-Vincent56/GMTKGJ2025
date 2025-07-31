using System.Collections.Generic;
using System.Linq;
using Perennial.Plants.Abilities;
using Perennial.Plants.Behaviors;

namespace Perennial.Plants
{
    public class Plant : PlantBase
    {
        
        
        
        

        public Plant(PlantDefinition definition, Tile tile, Garden garden)
            : base(definition, tile, garden)
        {
            // Behaviors are initialized by the factory
        }

        public override void Harvest()
        {
            

            // Call the parent Harvest()
            base.Harvest();
        }

        
    }
}
