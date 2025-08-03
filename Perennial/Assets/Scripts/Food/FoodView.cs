using System;
using Perennial.Plants.Data;
using TMPro;
using UnityEngine;

namespace Perennial.FoodMVC
{
    public class FoodView : MonoBehaviour
    {
        private FoodController _foodController;
        [SerializeField] private TextMeshProUGUI _displayText;

        public void Initialize(FoodController foodController)
        {
            _foodController = foodController;
            _displayText = GetComponentInChildren<TextMeshProUGUI>();
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
