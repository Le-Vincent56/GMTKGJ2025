namespace Perennial.Plants.Stats.Operations
{
    public class MultiplyOperation : IOperationStrategy
    {
        private readonly float _value;

        public MultiplyOperation(float value)
        {
            _value = value;
        }
        
        public float Calculate(float value) => _value * value;
    }
}