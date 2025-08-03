using System;
using System.Text;
using Perennial.Plants.Data;
using TMPro;
using UnityEngine;

namespace Perennial.FoodMVC
{
    public class FoodView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI displayText;
        [SerializeField] private TextMeshProUGUI goalText;
        
        private FoodController _foodController;

        public void Initialize(FoodController foodController, float startingFood)
        {
            _foodController = foodController;
            SetGoal(startingFood);
        }

        /// <summary>
        /// Update the food goal
        /// </summary>
        public void SetGoal(float amount) => goalText.text = ((int)amount).ToString();

        /// <summary>
        /// Update the food text
        /// </summary>
        public void UpdateUI(Food amount)
        {
            displayText.text = $"{amount.Value.ToString()}/{_foodController.FoodToWin.ToString()}";
        }
    }
}
