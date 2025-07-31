using UnityEngine;

namespace Perennial.Core.Architecture.Event_Bus.Events
{
   /// <summary>
   /// Event for the start of a new turn.
   /// </summary>
   public struct StartTurn : IEvent
   {

   }

   /// <summary>
   /// Event for the end of a turn.
   /// </summary>
   public struct EndTurn : IEvent
   {
      
   }
}
