using System;
using System.Collections.Generic;
using System.Text;
using Perennial.Core.Debugging;
using Perennial.Core.Extensions;
using Perennial.Plants.Data;
using Unity.VisualScripting;

namespace Perennial.Plants.UI
{
    public class PlantStorageModel
    {
        private readonly PlantDatabase _database;
        private readonly Dictionary<SerializableGuid, StorageAmount> _availablePlants;

        public event Action<Dictionary<SerializableGuid, StorageAmount>> OnModified = delegate { };
        public event Action OnEmpty = delegate { };
        
        public Dictionary<SerializableGuid, StorageAmount> AvailablePlants => _availablePlants;

        public PlantStorageModel(PlantDatabase database)
        {
            _database  = database;
            _availablePlants = new Dictionary<SerializableGuid, StorageAmount>();

            List<SerializableGuid> databaseIDs = _database.GetIDs();

            // Track how many of each plant definition there are in the storage through its ID
            foreach (SerializableGuid id in databaseIDs)
            {
                StorageAmount startingAmount = (StorageAmount)UnityEngine.Random.Range(0, 3);
                _availablePlants.Add(id, startingAmount);
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
            
            // Add to the current number of stored plants
            _availablePlants[plantID] = newAmount;
            
            // Notify that the model has been modified
            OnModified?.Invoke(_availablePlants);
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
            
            // Notify that the model has been modified
            OnModified?.Invoke(_availablePlants);

            // Exit if the storage is not empty
            if (!IsStorageEmpty()) return;
            
            // Notify that the model is empty
            OnEmpty?.Invoke();
        }

        /// <summary>
        /// Check if the storage is empty
        /// </summary>
        private bool IsStorageEmpty()
        {
            bool emptyStorage = true;

            foreach (KeyValuePair<SerializableGuid, StorageAmount> kvp in _availablePlants)
            {
                // Skip if there are no available plants of a given type
                if (kvp.Value <= 0) continue;
                
                // If there's at least one available plant, the storage is not empty
                emptyStorage = false;
                break;
            }

            return emptyStorage;
        }

        /// <summary>
        /// Get the plant definitions from the Database
        /// </summary>
        /// <returns></returns>
        public List<PlantDefinition> GetPlantDefinitions() => _database.GetDefinitions();

        /// <summary>
        /// Get a plant definition from the model
        /// </summary>
        public PlantDefinition GetPlantDefinition(SerializableGuid plantID)
        {
            return !ValidateDatabaseID(plantID, out PlantDefinition definition) 
                ? null 
                : definition;
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

        private void DebugModel()
        {
            StringBuilder storageBuilder = new StringBuilder();
            storageBuilder.Append("Available Plants Storage");
            storageBuilder.Append(" (Count: ");
            storageBuilder.Append(_availablePlants.Count);
            storageBuilder.Append("): ");
            foreach (KeyValuePair<SerializableGuid, StorageAmount> kvp in _availablePlants)
            {
                storageBuilder.AppendLine("Key: ");
                storageBuilder.Append(kvp.Key);
                storageBuilder.AppendLine("\tValue: ");
                storageBuilder.Append(kvp.Value);
            }
            
            Debugger.Log(storageBuilder.ToString(), LogType.Info);
            Debugger.Log(_database.Debug(), LogType.Info);
        }
    }
}
