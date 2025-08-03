using Perennial.Core.Extensions;
using Perennial.Plants.Data;

namespace Perennial.Core.Architecture.Event_Bus.Events
{
    public struct StorePlant : IEvent
    {
        public SerializableGuid ID;
        public StorageAmount Quantity;
    }

    public struct TakePlant : IEvent
    {
        public SerializableGuid ID;
    }

    public struct AddFood : IEvent
    {
        public Food Amount;
    }
    
    public struct StorageEmpty : IEvent { }
}
