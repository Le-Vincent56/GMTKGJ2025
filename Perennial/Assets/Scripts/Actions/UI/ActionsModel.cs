using System;
using Perennial.Core.Debugging;
using Debugger = Perennial.Core.Debugging.Debugger;

namespace Perennial.Actions.UI
{
    public class ActionsModel
    {
        private readonly int _maxActions;
        public int ActionsRemaining { get; private set; }

        public event Action ActionsRefreshed = delegate { };
        public event Action<int> ActionsChanged = delegate { };
        public event Action UsedAllActions = delegate { };

        public ActionsModel(int maxActions)
        {
            _maxActions = maxActions;
            ActionsRemaining = maxActions;
        }

        /// <summary>
        /// Use an action
        /// </summary>
        public void UseAction()
        {
            // Decrement the number of actions remaining
            ActionsRemaining--;
            
            // Notify that an action has been used
            ActionsChanged?.Invoke(ActionsRemaining);
            
            Debugger.Log($"Used Action, Actions Remaining: {ActionsRemaining}", LogType.Info);

            // Exit if there are still actions to use
            if (HasActions()) return;

            // Notify that all actions have been used
            UsedAllActions?.Invoke();
        }

        /// <summary>
        /// Refresh the number of actions available
        /// </summary>
        public void RefreshActions()
        {
            // Set the number of actions remaining to the
            // max amount of actions available to the player
            ActionsRemaining = _maxActions;
            
            // Notify that the actions have been refreshed
            ActionsRefreshed?.Invoke();
            ActionsChanged?.Invoke(ActionsRemaining);
        }

        
        /// <summary>
        /// Check if there are any actions left for the player to use
        /// </summary>
        private bool HasActions() => ActionsRemaining > 0;
    }
}
