using System;
using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;
using Perennial.Core.Architecture.State_Machine;
using Perennial.TurnController.States;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using StateMachine = Perennial.Core.Architecture.State_Machine.StateMachine;


namespace Perennial.TurnController
{
    public enum State
    {
        Start,
        End
    }
    public class TurnController : MonoBehaviour
    {

        [Header("Fields")]
        [SerializeField] private string currentState;
        [SerializeField] private int allowedActions;

        private EventBinding<PerformCommand> _performCommandEventBinding;
        private EventBinding<EndTurn> _endTurnEventBinding;
        private StateMachine _stateMachine;
        private State _stateFinished;
        private int _actionsTaken;
        private bool _endTurn;

        #region Properties
        public State StateFinished { set => _stateFinished = value;}

        public int ActionsTaken { set{ if (value < 0) value = 0; _actionsTaken = value; } }

        #endregion

        private void Start()
        {
            _actionsTaken = 0;
            StartupStateMachine();
        }

        private void OnEnable()
        {
            _performCommandEventBinding = new EventBinding<PerformCommand>(TakeAction);
            EventBus<PerformCommand>.Register(_performCommandEventBinding);
            
            _endTurnEventBinding = new EventBinding<EndTurn>(() =>
            {
                _endTurn = true;
            });

            EventBus<EndTurn>.Register(_endTurnEventBinding);
        }

        private void OnDisable()
        {
            EventBus<PerformCommand>.Deregister(_performCommandEventBinding);
            EventBus<EndTurn>.Deregister(_endTurnEventBinding);
        }

        private void Update()
        {
            _stateMachine.Update();
            
            //debugging to see active state in inspector
            currentState = _stateMachine.GetState().ToString();
        }

        /// <summary>
        /// Setup states
        /// </summary>
        private void StartupStateMachine()
        {
            _stateMachine = new StateMachine();

            StartTurnState startTurnState = new StartTurnState(this);
            ActionState actionState = new ActionState(this);
            EndTurnState endTurnState = new EndTurnState(this);
            
           _stateMachine.At(startTurnState, actionState, new FuncPredicate(() => _stateFinished == State.Start));
           _stateMachine.At(actionState, endTurnState, new FuncPredicate(() => _actionsTaken >= allowedActions || _endTurn)); 
           _stateMachine.At(endTurnState, startTurnState, new FuncPredicate(() => _stateFinished == State.End));
        }

        /// <summary>
        /// Called when an action is taken
        /// </summary>
        private void TakeAction() => _actionsTaken++;


    }
}
