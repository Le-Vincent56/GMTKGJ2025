namespace Perennial.Plants.Abilities
{
    public abstract class HarvestPlantAbility : PlantAbility, IHarvestAbility
    {
        public abstract void OnHarvest(PlantAbilityContext context);
        public override void Execute(PlantAbilityContext context) => OnHarvest(context);
    }
}