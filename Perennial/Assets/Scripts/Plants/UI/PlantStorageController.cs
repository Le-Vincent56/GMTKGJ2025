using System.Collections.Generic;
using Perennial.Core.Extensions;
using UnityEngine;

namespace Perennial.Plants.UI
{
    public class PlantStorageController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlantDatabase database;
        
        private PlantStorageModel _model;
        private PlantStorageView _view;
        
        private void Awake()
        {
            // Connect the MVC
            _model = new PlantStorageModel(database);
            _view = GetComponent<PlantStorageView>();

            ConnectModel();
            ConnectView();
        }

        private void ConnectModel()
        {
            _model.OnModified += _view.UpdateButtons;
        }

        private void ConnectView()
        {
            // Initialize the view with the controller and the model
            _view.Initialize(this);
            
            List<PlantDefinition> definitions = _model.GetPlantDefinitions();

            // Iterate through the definitions
            for (int i = 0; i < definitions.Count; i++)
            {
                // Break out of the loop if we have reached more definitions
                // than there are buttons
                if (i >= _view.PlantButtons.Count) break;
                
                // Initialize the plant button with a definition
                _view.PlantButtons[i].Initialize(definitions[i]);
                _view.PlantButtons[i].RegisterListener(() => SelectPlant(definitions[i].ID));
            }
        }

        private void SelectPlant(SerializableGuid id)
        {
            // TODO: Set the select plant
        }
    }
}
