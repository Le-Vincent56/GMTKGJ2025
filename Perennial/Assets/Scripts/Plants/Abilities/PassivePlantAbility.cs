namespace Perennial.Plants.Abilities
{
    public abstract class PassivePlantAbility : PlantAbility, IPassiveAbility
    {
        public abstract void OnTick(PlantAbilityContext context);
        public abstract void Cancel(PlantAbilityContext context);
    }
}