using Perennial.Plants.Data;

namespace Perennial.Plants
{
    public interface IPlant
    {
        void Upkeep();
        void Harvest(out (Food Food, Seeds BaseSeeds, Seeds? MutationSeeds) rewards);
    }
}
