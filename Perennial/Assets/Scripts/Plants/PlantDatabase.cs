using System.Collections.Generic;
using System.Linq;
using Perennial.Core.Debugging;
using Perennial.Core.Extensions;
using UnityEngine;
using UnityEditor;
using LogType = Perennial.Core.Debugging.LogType;


namespace Perennial.Plants
{
    [CreateAssetMenu(fileName = "Plant Database", menuName = "Plants/Database")]
    public class PlantDatabase : ScriptableObject
    {
        [SerializeField] private List<PlantDefinition> plantDefinitions;

        private Dictionary<SerializableGuid, PlantDefinition> _lookup;

        private void OnValidate()
        {
            // Initialize the lookup
            _lookup = new Dictionary<SerializableGuid, PlantDefinition>();
            
            // Loop through each plant
            foreach (PlantDefinition plant in plantDefinitions)
            {
                // Skip if there's no plant or plant ID
                if (plant == null || plant.ID == SerializableGuid.Empty) continue;
                
                // Skip if adding the plant definition was successful
                if (_lookup.TryAdd(plant.ID, plant)) continue;
                
                // Log a warning for duplicates
                Debugger.Log($"Duplicate Plant ID found: {plant.Name}", LogType.Warning);
            }
        }
        
        /// <summary>
        /// Get all the IDs of the lookup table
        /// </summary>
        public List<SerializableGuid> GetIDs() => _lookup.Keys.ToList();
        
        public List<PlantDefinition> GetKeys() => _lookup.Values.ToList();
        
        /// <summary>
        /// Try to get a value from the lookup table
        /// </summary>
        public bool TryGetValue(SerializableGuid id, out PlantDefinition plant) => _lookup.TryGetValue(id, out plant);
        
        #if UNITY_EDITOR
        /// <summary>
        /// Automatically refresh the Plant Database with all found Plant Definition objects in the project
        /// </summary>
        [ContextMenu("Refresh Plant Database")]
        public void RefreshDatabase()
        {
            // Clear the current list
            plantDefinitions.Clear();
            
            // Find all Plant Definition assets
            string[] guids = AssetDatabase.FindAssets("t:PlantDefinition");
            foreach (string guid in guids)
            {
                // Extract the plant definition at the path and add it to the plant definitions list
                // if not added already
                string path = AssetDatabase.GUIDToAssetPath(guid);
                PlantDefinition plant = AssetDatabase.LoadAssetAtPath<PlantDefinition>(path);
                
                // If a plant definition exists, add it
                if (plant != null && !plantDefinitions.Contains(plant))
                {
                    plantDefinitions.Add(plant);
                }
            }
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            
            Debugger.Log("Plant Database refreshed!", LogType.Info);
        }

        [ContextMenu("Validate Database")]
        public void ValidateDatabase()
        {
            HashSet<SerializableGuid> usedIDs = new HashSet<SerializableGuid>();
            List<string> duplicates = new List<string>();

            // Loop through each Plant Definition
            foreach (PlantDefinition item in plantDefinitions)
            {
                // Skip if there is no definition
                if (item == null) continue;

                // Check if each ID is unique
                if (!usedIDs.Add(item.ID))
                {
                    duplicates.Add($"{item.Name} has a duplicate ID: {item.ID}");
                }
            }
            
            // Log duplicate errors if they exist
            if (duplicates.Count > 0)
            {
                Debugger.Log($"Found {duplicates.Count} duplicate IDs:", LogType.Error);
                foreach (string duplicate in duplicates)
                {
                    Debugger.Log(duplicate, LogType.Error);
                }
            }
            else
            {
                Debug.Log($"Validation passed! All {plantDefinitions.Count} items have unique IDs.");
            }
        }
        #endif
    }
}
