using UnityEngine;

namespace Perennial.Plants.Stats.Operations
{
    public interface IOperationStrategy
    {
        float Calculate(float value);
    }
}
