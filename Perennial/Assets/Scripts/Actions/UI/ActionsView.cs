using System.Collections.Generic;
using System.Text;
using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using TMPro;
using UnityEngine;

namespace Perennial.Actions.UI
{
    public class ActionsView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI actionsText;
        
        public List<ActionButton> ActionButtons { get; private set; }

        /// <summary>
        /// Initialize the Actions View
        /// </summary>
        public void Initialize()
        { 
            // Get action buttons
            ActionButtons = new List<ActionButton>();
            GetComponentsInChildren(ActionButtons);

            // Iterate through each Action Button
            foreach (ActionButton button in ActionButtons)
            {
                // Initialize the Action Button
                button.Initialize(this);
            }
        }

        /// <summary>
        /// Update the text stating how many actions remain for the turn
        /// </summary>
        public void UpdateActionsRemaining(int actionsRemaining)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(actionsRemaining);
            stringBuilder.Append(" ACTIONS LEFT");
            actionsText.text = stringBuilder.ToString();
        }

        /// <summary>
        /// Disable all Action Buttons
        /// </summary>
        public void DisableButtons()
        {
            foreach (ActionButton button in ActionButtons)
            {
                button.Disable();
            }
        }

        /// <summary>
        /// Enable all Action Buttons
        /// </summary>
        public void EnableButtons()
        {
            foreach (ActionButton button in ActionButtons)
            {
                button.Enable();
            }
        }

        /// <summary>
        /// Reset all buttons to a usable state
        /// </summary>
        public void ResetButtons()
        {
            foreach (ActionButton button in ActionButtons)
            {
                button.Reset();
            }
        }

        /// <summary>
        /// Disable the Plant action button
        /// </summary>
        public void DisablePlantAction()
        {
            // Change the action state to none
            EventBus<ChangeActionState>.Raise(new ChangeActionState()
            {
                StateType = ActionStateType.Nothing,
                SelectedPlantDefinition = null,
            });
            
            // Disable the action button
            ActionButtons[0].Disable();
        }

        public void EnablePlantAction()
        {
            
        }
    }
}
