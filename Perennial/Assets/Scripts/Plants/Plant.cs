using UnityEngine;

namespace Perennial.Plants
{
    public abstract class Plant : IPlant
    {
        private PlantData _data;

        public string Name => _data.Name;
        public int CurrentLifetime { get; private set; }
        public int TotalLifetime => _data.Lifetime;
        
        public abstract void Tick();
        public abstract void Harvest();
    }
}
