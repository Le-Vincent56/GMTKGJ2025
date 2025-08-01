namespace Perennial.Plants.Abilities
{
    public abstract class PlacePlantAbility : PlantAbility, IPlaceAbility
    {
        public abstract void OnPlace(PlantAbilityContext context);
    }
}
