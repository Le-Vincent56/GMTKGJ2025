using Perennial.Core.Extensions;
using Perennial.Plants.Stats;

namespace Perennial.Plants.Data
{
    public readonly record struct Affector(SerializableGuid ID, StatType Type, float Value);
}
