using UnityEngine;

namespace Perennial.Plants.Behaviors
{
    public abstract class PlantBehavior : ScriptableObject
    {
        [SerializeField] protected string behaviorName;
        [SerializeField] [TextArea(2, 3)] protected string description;
        
        public string Name => behaviorName;
        public  string Description => description;
        
        public abstract PlantBehaviorInstance CreateInstance(Plant plant);
    }
}
