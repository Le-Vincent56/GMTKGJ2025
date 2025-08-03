using System;
using Perennial.Plants.Data;
using TMPro;
using UnityEngine;

namespace Perennial.FoodMVC
{
    public class FoodView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _displayText;
        
        private FoodController _foodController;

        public void Initialize(FoodController foodController)
        {
            _foodController = foodController;
        }

        /// <summary>
        /// Update the food text
        /// </summary>
        public void UpdateUI(Food amount)
        {
            _displayText.text = amount.Value.ToString();
            //TODO actual UI
        }
    }
}
