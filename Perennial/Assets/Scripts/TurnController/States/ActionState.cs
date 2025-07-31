using UnityEngine;

namespace Perennial.TurnController.States
{
    public class ActionState : BaseState
    {
        public ActionState (TurnController turnController) : base(turnController)
        {
        }

        public override void OnEnter()
        {
            Debug.Log($"{this} started");
        }
    }
}
