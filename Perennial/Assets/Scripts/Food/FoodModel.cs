using System;
using Perennial.Plants.Data;

namespace Perennial.FoodMVC
{
    public class FoodModel 
    {
        public event Action<Food> OnModified = delegate { };
        
        private Food _foodAmount;
        
        /// <summary>
        /// Adds the foods, can be negative
        /// </summary>
        /// <param name="amountToAdd"></param>
        public void AddFood(Food amountToAdd)
        {
            _foodAmount += amountToAdd;
            
            OnModified?.Invoke(_foodAmount);
        }
    }
}
