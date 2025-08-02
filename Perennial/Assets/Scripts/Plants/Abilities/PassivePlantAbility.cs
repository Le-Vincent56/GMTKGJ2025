using UnityEngine;

namespace Perennial.Plants.Abilities
{
    public abstract class PassivePlantAbility : PlantAbility, IPassiveAbility
    {
        [Header("Tick Settings")]
        [SerializeField] protected bool onGrow;
        [SerializeField] protected bool onHarvest;
        
        public abstract void OnTick(PlantAbilityContext context);
        public abstract void Cancel(PlantAbilityContext context);
    }
}