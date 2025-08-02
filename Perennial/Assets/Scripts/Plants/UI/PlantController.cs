using UnityEngine;

namespace Perennial.Plants.UI
{
    public class PlantController : MonoBehaviour
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
            
        }

        private void ConnectView()
        {
            // Initialize the view with the controller and the model
            _view.Initialize(this, _model);
        }
    }
}
