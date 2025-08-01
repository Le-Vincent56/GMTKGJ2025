using System;
using System.Collections.Generic;
using Perennial.Core.Debugging;
using Perennial.Core.Extensions;
using Perennial.Plants.Data;

namespace Perennial.Plants.UI
{
    public class PlantStorageModel
    {
        private readonly PlantDatabase _database;
        private Dictionary<SerializableGuid, StorageAmount> _availablePlants;

        public event Action<PlantDefinition, StorageAmount> OnPlantAdded = delegate { };
        public event Action<PlantDefinition, StorageAmount> OnPlantRemoved = delegate { };

        public PlantStorageModel(PlantDatabase database)
        {
            _database  = database;
            _availablePlants = new Dictionary<SerializableGuid, StorageAmount>();

            List<SerializableGuid> databaseIDs = _database.GetIDs();

            // Track how many of each plant definition there are in the storage through its ID
            foreach (SerializableGuid id in databaseIDs)
            {
                _availablePlants.Add(id, (StorageAmount)0);
            }
        }

        /// <summary>
        /// Add a number of plants of a given ID to the available plants in the storage
        /// </summary>
        public void AddPlants(SerializableGuid plantID, StorageAmount numberToAdd)
        {
            // Exit if the ID cannot be retrieved from the storage
            if(!ValidateStorageID(plantID, out StorageAmount currentAmount)) return;

            StorageAmount newAmount = currentAmount + numberToAdd;
            
            // Add to the current amount of stored plants
            _availablePlants[plantID] = newAmount;

            // Check if the ID cannot be retrieved from the database
            if (!ValidateDatabaseID(plantID, out PlantDefinition addedPlant)) return;
            
            // Invoke the added event
            OnPlantAdded?.Invoke(addedPlant, newAmount);
        }

        /// <summary>
        /// Remove a plant from the storage
        /// </summary>
        public void RemovePlant(SerializableGuid plantID)
        {
            // Exit if the ID cannot be retrieved from the storage
            if(!ValidateStorageID(plantID, out StorageAmount currentAmount)) return;

            // Exit if there are no plants to use
            if (currentAmount == 0) return;

            // Subtract from the number of plants
            _availablePlants[plantID]--;
            
            // Check if the ID cannot be retrieved from the database
            if (!ValidateDatabaseID(plantID, out PlantDefinition removedPlant)) return;
            
            // Invoke the removed event
            OnPlantRemoved?.Invoke(removedPlant, _availablePlants[plantID]);
        }

        /// <summary>
        /// Validate if a Plant ID exists within the storage
        /// </summary>
        private bool ValidateStorageID(SerializableGuid plantID, out StorageAmount result)
        {
            if (_availablePlants.TryGetValue(plantID, out StorageAmount storedAmount))
            {
                result = storedAmount;
                return true;
            }

            result = storedAmount;
            
            Debugger.Log($"Cannot find the value of Plant ID {plantID} in the Dictionary", 
                LogType.Warning);

            return false;
        }

        /// <summary>
        /// Validate if a Plant ID exists within the Database
        /// </summary>
        private bool ValidateDatabaseID(SerializableGuid plantID, out PlantDefinition result)
        {
            // Try to get the plant definition associated with the plant ID
            if (_database.TryGetValue(plantID, out PlantDefinition attachedPlant))
            {
                result = attachedPlant;
                return true;
            }

            result = attachedPlant;
            
            Debugger.Log($"Cannot find the value of Plant ID {plantID} in" +
                         $"the database", LogType.Warning);

            return false;
        }
    }
}
