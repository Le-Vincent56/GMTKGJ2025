using UnityEngine;

namespace Perennial.Plants.Abilities
{
    public abstract class PlantAbility : ScriptableObject
    {
        [SerializeField] protected string abilityName;
        [SerializeField] [TextArea(2, 3)] protected string description;
        [SerializeField] protected int effectRadius;
        
        public string Name => abilityName;
        public string Description => description;
        public int Radius => effectRadius;
        
        /// <summary>
        /// Check if the ability can be executed within the current context
        /// </summary>
        public virtual bool CanExecute(PlantAbilityContext context) => true;
    }
}
