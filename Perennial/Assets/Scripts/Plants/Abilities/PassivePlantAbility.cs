namespace Perennial.Plants.Abilities
{
    public abstract class PassivePlantAbility : PlantAbility, IPassiveAbility
    {
        public abstract void OnTick(PlantAbilityContext context);
        public override void Execute(PlantAbilityContext context) => OnTick(context);
    }
}