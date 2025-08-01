using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using Perennial.Core.Architecture.State_Machine;
using Perennial.TurnController.States.ActionStates;
using UnityEngine;
using StateMachine = Perennial.Core.Architecture.State_Machine.StateMachine;

namespace Perennial.TurnController.States
{
    public class ActionState : BaseState
    {
        private StateMachine _actionStateMachine;

        //for the sub action state machine controls
        private ActionStateType _actionStateType;
        
        /// <summary>
        /// I wasn't sure if I wanted to include this in the turn controller
        /// It made some sense to separate it as its a separate action system
        /// Can be easily moved a step above with a few small changes
        /// </summary>
        private readonly EventBinding<ChangeActionState> _changeActionStateEventBinding;
        private readonly EventBinding<PerformCommand> _performCommandEventBinding;
        private readonly EventBinding<TurnEnded> _turnEndedEventBinding;

        public ActionState (TurnController turnController) : base(turnController)
        {
            //setup event listeners
            _changeActionStateEventBinding = new EventBinding<ChangeActionState>((e) =>
            {
                /* If button swaps to 'cancel'
                if (_actionStateType == e.StateType)
                    _actionStateType = ActionStateType.Nothing;
                */
                
                _actionStateType = e.StateType;
            });
            
            _performCommandEventBinding = new EventBinding<PerformCommand>(() =>
            {
                _actionStateType = ActionStateType.Nothing; 
            });
            
               
            _turnEndedEventBinding = new EventBinding<TurnEnded>(() =>
            {
              //  _actionStateType = ActionStateType.Nothing; 
            });
            
            StartUpStateMachine();
        }

        public override void OnEnter()
        {
            Debug.Log($"{this} started");
            EventBus<ChangeActionState>.Register(_changeActionStateEventBinding);
            EventBus<PerformCommand>.Register(_performCommandEventBinding);
        }
        
        public override void Update()
        {
            _actionStateMachine.Update();
        }

        public override void OnExit()
        {
            _actionStateType = ActionStateType.Nothing; 
            EventBus<ChangeActionState>.Deregister(_changeActionStateEventBinding);
            EventBus<PerformCommand>.Deregister(_performCommandEventBinding);
        }

        private void StartUpStateMachine()
        {
            //setup sub state machine
            _actionStateMachine = new StateMachine();

            HarvestActionState harvestActionState = new HarvestActionState();
            PlantActionState plantActionState = new PlantActionState();
            TillActionState tillActionState = new TillActionState();
            NothingActionState nothingActionState = new NothingActionState();
            
            _actionStateMachine.Any(harvestActionState, new FuncPredicate(() => _actionStateType == ActionStateType.Harvest));
            _actionStateMachine.Any(plantActionState, new FuncPredicate(() => _actionStateType == ActionStateType.Plant));
            _actionStateMachine.Any(tillActionState, new FuncPredicate(() => _actionStateType == ActionStateType.Till));
            _actionStateMachine.Any(nothingActionState, new FuncPredicate(() => _actionStateType == ActionStateType.Nothing));
        
            //start at nothing
            _actionStateType = ActionStateType.Nothing;
            _actionStateMachine.SetState(nothingActionState);
        }
    }
}
