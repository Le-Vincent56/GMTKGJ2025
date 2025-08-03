using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;

namespace Perennial.Actions.UI
{
    public class ActionsController : MonoBehaviour
    {
        private ActionsModel _model;
        private ActionsView _view;

        private EventBinding<TurnStarted> _onTurnStarted;
        private EventBinding<TurnEnded> _onTurnEnded;
        private EventBinding<PerformCommand> _onPerformCommand;
        
        private void Awake()
        {
            _model = new ActionsModel(3);
            _view = GetComponent<ActionsView>();
            
            ConnectView();
        }

        private void OnEnable()
        {
            ConnectModel();
            
            _onTurnStarted = new EventBinding<TurnStarted>(RestoreActions);
            EventBus<TurnStarted>.Register(_onTurnStarted);
            
            _onTurnEnded = new EventBinding<TurnEnded>(DisableActions);
            EventBus<TurnEnded>.Register(_onTurnEnded);
            
            _onPerformCommand = new EventBinding<PerformCommand>(UseAction);
            EventBus<PerformCommand>.Register(_onPerformCommand);
        }

        private void OnDisable()
        {
            _model.UsedAllActions -= _view.DisableButtons;
            _model.ActionsRefreshed -= _view.ResetButtons;
            _model.ActionsChanged -= _view.UpdateActionsRemaining;
            
            EventBus<TurnStarted>.Deregister(_onTurnStarted);
            EventBus<TurnEnded>.Deregister(_onTurnEnded);
            EventBus<PerformCommand>.Deregister(_onPerformCommand);
        }

        /// <summary>
        /// Connect the Model to the Controller
        /// </summary>
        private void ConnectModel()
        {
            _model.UsedAllActions += _view.DisableButtons;
            _model.ActionsRefreshed += _view.ResetButtons;
            _model.ActionsChanged += _view.UpdateActionsRemaining;
        }

        /// <summary>
        /// Connect the View to the Controller
        /// </summary>
        private void ConnectView()
        {
            // Initialize the view
            _view.Initialize();
            
            // Enable all buttons
            _view.EnableButtons();
        }
        
        /// <summary>
        /// Restore the number of actions that can be used
        /// </summary>
        private void RestoreActions() => _model.RefreshActions();

        /// <summary>
        /// Disable the player from taking actions
        /// </summary>
        private void DisableActions() => _view.DisableButtons();
        
        /// <summary>
        /// Use an action
        /// </summary>
        private void UseAction() => _model.UseAction();
    }
}
