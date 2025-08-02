using System.Collections.Generic;
using System.Linq;
using Perennial.Core.Debugging;
using Perennial.Core.Extensions;
using Perennial.Plants.Data;
using UnityEngine;
using LogType = Perennial.Core.Debugging.LogType;

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
            Debugger.Log($"Number of Buttons: {PlantButtons.Count}", LogType.Info);
        }

        /// <summary>
        /// Update the plant buttons
        /// </summary>
        public void UpdateButtons(Dictionary<SerializableGuid, StorageAmount> availablePlants)
        {
            foreach (KeyValuePair<SerializableGuid, StorageAmount> kvp in availablePlants)
            {
                foreach (PlantButton button in PlantButtons)
                {
                    // Skip if the IDs don't match
                    if (button.ID != kvp.Key) continue;

                    // Set the storage amount
                    button.Amount = (string)kvp.Value;

                    // Set the game object as active if there are any seeds available
                    // otherwise, set to none
                    button.gameObject.SetActive(kvp.Value > 0);
                }
            }
        }
    }
}
