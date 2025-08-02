using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;
using Perennial.Core.Architecture.State_Machine;
using Perennial.TurnManagement.States;
using StateMachine = Perennial.Core.Architecture.State_Machine.StateMachine;


namespace Perennial.TurnManagement
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
        public State StateFinished { get => _stateFinished; set => _stateFinished = value;}
        public bool EndTurn { set => _endTurn = value; }
        public StateMachine StateMachine { get => _stateMachine; private set => _stateMachine = value; }
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
           
           //enum to end so it doesn't auto transition(start state sets it to start)
           _stateFinished = State.End;
           
           //start at start
           _stateMachine.SetState(startTurnState);
        }

        /// <summary>
        /// Called when an action is taken
        /// </summary>
        private void TakeAction() => _actionsTaken++;

        /// <summary>
        /// Resets actions taken to 0
        /// </summary>
        public void ResetActions()
        {
            _actionsTaken = 0;
        }


    }
}
