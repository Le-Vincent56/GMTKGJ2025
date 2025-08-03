using Perennial.Core.Architecture.Singletons;
using Perennial.Core.Debugging;
using UnityEngine;
using LogType = Perennial.Core.Debugging.LogType;

namespace Perennial.Plants.Mutations
{
    public class MutationManager : PersistentSingleton<MutationManager>
    {
        [Header("Configuration")] 
        [SerializeField] private MutationTable mutationTable;
        
        /// <summary>
        /// Get the mutation result for two plants
        /// </summary>
        public PlantDefinition GetMutationResult(Plant plant1, Plant plant2)
        {
            if (!mutationTable)
            {
                Debugger.Log("No mutation table assigned to MutationManager!", LogType.Error);
                return null;
            }
            
            // Get the definitions from the plants
            // Note: You'll need to add a way to get PlantDefinition from Plant
            // Either store it in Plant or have a lookup system
            PlantDefinition def1 = plant1.Definition;
            PlantDefinition def2 = plant2.Definition;
            
            if (!def1 || !def2) return null;
            
            return mutationTable.GetMutationResult(def1, def2);
        }
        
        /// <summary>
        /// Get the mutation result directly from definitions
        /// </summary>
        public PlantDefinition GetMutationResult(PlantDefinition def1, PlantDefinition def2)
        {
            // Return the mutation result if the mutation table exists
            if (mutationTable) return mutationTable.GetMutationResult(def1, def2);
            
            Debugger.Log("No mutation table assigned to MutationManager!", LogType.Error);
            return null;
        }
        
        /// <summary>
        /// Check if two plants can mutate
        /// </summary>
        public bool CanMutate(Plant plant1, Plant plant2)
        {
            PlantDefinition def1 = plant1.Definition;
            PlantDefinition def2 = plant2.Definition;
            
            if (!def1 || !def2) return false;
            
            return mutationTable.CanMutate(def1, def2);
        }
    }
}
