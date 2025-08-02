using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Perennial.Plants.UI
{
    public class PlantStorageView : MonoBehaviour
    {
        private PlantController _controller;
        private List<PlantButton>  _plantButtons = new List<PlantButton>();

        /// <summary>
        /// Initialize the Plant Storage View
        /// </summary>
        public void Initialize(PlantController controller, PlantStorageModel model)
        {
            _controller = controller;
            _plantButtons = GetComponentsInChildren<PlantButton>().ToList();
            List<PlantDefinition> definitions = model.GetPlantDefinitions();

            // Iterate through the definitions
            for (int i = 0; i < definitions.Count; i++)
            {
                // Break out of the loop if we have reached more definitions
                // than there are buttons
                if (i >= _plantButtons.Count) break;
                
                // Initialize the plant button with a definition
                _plantButtons[i].Initialize(definitions[i]);
            }
        }
    }
}
