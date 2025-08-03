using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;

namespace Perennial.Actions.UI.Strategies
{
    [CreateAssetMenu(fileName = "Action",  menuName = "UI/Actions/Action State")]
    public class ActionStateStrategy : ButtonStrategy
    {
        [Header("Details")]
        [SerializeField] private ActionStateType actionType;
        
        public override void Press()
        {
            // Change the action state
            EventBus<ChangeActionState>.Raise(new ChangeActionState()
            {
                StateType = actionType,
                SelectedPlantDefinition = null,
            });
        }
    }
}
