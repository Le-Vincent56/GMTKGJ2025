namespace Perennial.Core.Architecture.Event_Bus.Events
{
   /// <summary>
   /// Event for the start of a new turn.
   /// </summary>
   public struct TurnStarted : IEvent
   {

   }

   public struct ActionsStart : IEvent
   {
      
   }

   /// <summary>
   /// Event for the end of a turn.
   /// </summary>
   public struct TurnEnded : IEvent
   {
      
   }
   
   /// <summary>
   /// Event to end the turn
   /// </summary>
   public struct EndTurn : IEvent
   {
      
   }
}
