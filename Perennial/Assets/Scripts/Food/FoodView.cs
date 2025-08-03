using Perennial.Plants.Data;
using TMPro;
using UnityEngine;

namespace Perennial.FoodMVC
{
    public class FoodView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI displayText;
        [SerializeField] private TextMeshProUGUI goalText;

        public void Initialize(float foodToWin)
        {
            SetGoal(foodToWin);
        }

        /// <summary>
        /// Update the food goal
        /// </summary>
        public void SetGoal(float amount) => goalText.text = ((int)amount).ToString();

        /// <summary>
        /// Update the food text
        /// </summary>
        public void UpdateCurrent(Food amount) => displayText.text = ((int)amount).ToString();
    }
}
