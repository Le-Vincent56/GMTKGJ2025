namespace Perennial.Plants.Abilities
{
    public interface IPassiveAbility
    {
        void OnTick(PlantAbilityContext context);
        void Cancel(PlantAbilityContext context);
    }
}