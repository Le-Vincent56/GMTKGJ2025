using Perennial.Actions;

namespace Perennial.Core.Architecture.Event_Bus.Events
{
   public struct PerformCommand : IEvent
   {
      public ICommand Command;
   }
}
