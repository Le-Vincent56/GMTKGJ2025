using System;
using UnityEngine;
using Perennial.Core.Architecture.State_Machine;
using Perennial.TurnController.States;

namespace Perennial.TurnController
{
    public class TurnController : MonoBehaviour
    {
        private StateMachine _stateMachine;

        [Header("Fields")]
        [SerializeField] private string currentState;
        [SerializeField] private bool endTurn;
        
        #region Properties
        public bool EndingTurn { get => endTurn; set => endTurn = value; }


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

            StartTurnState startTurnState = new StartTurnState();
            ActionState actionState = new ActionState();
            EndTurnState endTurnState = new EndTurnState();

            //always go from start to actions automatically
           _stateMachine.At(startTurnState, actionState, new FuncPredicate(() => true));
            

        }
        
        
    }
}
