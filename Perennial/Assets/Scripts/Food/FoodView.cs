using System;
using Perennial.Plants.Data;
using TMPro;
using UnityEngine;

namespace Perennial.FoodMVC
{
    public class FoodView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI displayText;

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
            displayText.text = $"{amount.Value.ToString()}/{_foodController.FoodToWin.ToString()}";
        }
    }
}
