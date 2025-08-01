using System;
using Perennial.Core.Extensions;
using Perennial.Plants.Data;
using Perennial.Plants.Stats.Operations;

namespace Perennial.Plants.Stats
{
    public class StatModifier : IDisposable
    {
        public SerializableGuid ID { get; }
        public StatType Type { get; }
        public IOperationStrategy Operation { get; }
        public event Action<StatModifier> OnDispose = delegate { };
        
        public bool MarkedForRemoval { get; set; }

        public StatModifier(SerializableGuid id, StatType type, IOperationStrategy operation)
        {
            ID = id;
            Type = type;
            Operation = operation;
        }

        /// <summary>
        /// Modify the stat value
        /// </summary>
        public void Modify(object sender, Query query)
        {
            // Exit if the types mismatch
            if (query.Type != Type) return;
            
            // Calculate the query value
            query.Value = Operation.Calculate(query.Value);
        }

        /// <summary>
        /// Handle cleanup of the Stat Modifier on cleanup
        /// </summary>
        public void Dispose() => OnDispose?.Invoke(this);
    }
}