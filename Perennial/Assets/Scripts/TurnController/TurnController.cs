using System;
using UnityEngine;
using Perennial.Core.Architecture.State_Machine;
using Perennial.TurnController.States;


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
        [SerializeField] private bool endTurn;
        
        
        private StateMachine _stateMachine;
        private State _stateFinished;
        
        #region Properties
        public bool EndingTurn { get => endTurn; set => endTurn = value; }
        public State StateFinished { set => _stateFinished = value;}

        #endregion


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
           _stateMachine.At(actionState, endTurnState, new FuncPredicate(() => true)); //TODO Add action checks
           _stateMachine.At(endTurnState, startTurnState, new FuncPredicate(() => _stateFinished == State.End));
            

        }
        
        
    }
}
