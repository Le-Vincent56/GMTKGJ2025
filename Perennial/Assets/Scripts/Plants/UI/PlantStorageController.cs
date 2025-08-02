using System.Collections.Generic;
using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using Perennial.Core.Extensions;
using Perennial.Garden;
using Perennial.Plants.Data;
using Unity.VisualScripting;
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
            Dictionary<SerializableGuid, StorageAmount> availablePlants = _model.AvailablePlants;

            // Iterate through the definitions
            for (int i = 0; i < _view.PlantButtons.Count; i++)
            {
                if (i >= definitions.Count)
                {
                    _view.PlantButtons[i].gameObject.SetActive(false);
                    continue;
                }
                
                SerializableGuid definitionID = definitions[i].ID;
                
                // Initialize the plant button with a definition
                _view.PlantButtons[i].Initialize(definitions[i]);
                _view.PlantButtons[i].RegisterListener(() => SelectPlant(definitionID));
            }

            // Update the buttons with their amounts
            _view.UpdateButtons(availablePlants);
        }

        private void SelectPlant(SerializableGuid id)
        {
            Debug.Log("Plant Selected");
            // TODO: Set the select plant
            // When a tile is selected with the plant, use the parameter 'id'
            // to lookup in the _model (GetPlantDefinition()) and then
            // use the static PlantFactory's CreatePlant() function while
            // passing in the PlantDefinition, Tile, and Garden.
            // Then, use the _model.RemovePlant() function, passing in the
            // parameter 'id'

            //grab the definition
            PlantDefinition plantDefinition = _model.GetPlantDefinition(id);

            //warnings return there is no definition, so bail out if true so plant action doesn't fail
            if (plantDefinition == null)
            {
                Debug.LogWarning("Plant state failed to update as definition was null");
                return;
            }
            
            //update the plant action state with the new definition
            EventBus<ChangeActionState>.Raise(new ChangeActionState()
            {
                    StateType = ActionStateType.Plant,
                    SelectedPlantDefinition = plantDefinition
            });

            //can't remove it yet as don't know if it was actually planted may have to use another event
            //_model.RemovePlant(id);
        }
    }
}
