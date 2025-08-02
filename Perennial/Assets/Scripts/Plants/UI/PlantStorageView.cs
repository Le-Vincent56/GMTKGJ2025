using System.Collections.Generic;
using System.Linq;
using Perennial.Core.Extensions;
using Perennial.Plants.Data;
using UnityEngine;

namespace Perennial.Plants.UI
{
    public class PlantStorageView : MonoBehaviour
    {
        private PlantStorageController _storageController;

        public List<PlantButton> PlantButtons { get; private set; } = new List<PlantButton>();

        /// <summary>
        /// Initialize the Plant Storage View
        /// </summary>
        public void Initialize(PlantStorageController storageController)
        {
            _storageController = storageController;
            PlantButtons = GetComponentsInChildren<PlantButton>().ToList();
            
        }

        public void UpdateButtons(Dictionary<SerializableGuid, StorageAmount> availablePlants)
        {
            foreach (PlantButton button in PlantButtons)
            {
                
            }
        }
    }
}
