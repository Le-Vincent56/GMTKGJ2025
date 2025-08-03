using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using Perennial.Plants.Data;
using UnityEngine;

namespace Perennial.FoodMVC
{
    public class FoodController : MonoBehaviour
    {
        [Header("Starting Data")]  //maybe use a so if there is ever more data
        [SerializeField] private float startingFood;
        
        private FoodModel _model;
        private FoodView _view;

        private EventBinding<AddFood> _onAddFoodEventBinding;
        
        private void Awake()
        {
            _model = new FoodModel();
            _view = GetComponent<FoodView>();

            ConnectModel();
            ConnectView();
        }

        private void OnEnable()
        {
            _onAddFoodEventBinding = new EventBinding<AddFood>(AddFood);
            EventBus<AddFood>.Register(_onAddFoodEventBinding);
        }

        private void OnDisable()
        {
            EventBus<AddFood>.Deregister(_onAddFoodEventBinding);
        }

        private void ConnectModel()
        {
            _model.OnModified += _view.UpdateUI;
        }

        private void ConnectView()
        {
            _view.Initialize(this);
            
            //update text with new amount
            _model.AddFood(new Food(startingFood));
        }

        /// <summary>
        /// Add a number of plants to storage
        /// </summary>
        private void AddFood(AddFood eventData) => _model.AddFood(eventData.Amount);
    }
}
