namespace Perennial.Plants.Stats.Operations
{
    public class AddOperation : IOperationStrategy
    {
        private readonly float _value;

        public AddOperation(float value)
        {
            _value = value;
        }

        public float Calculate(float value) => _value + value;
    }   
}