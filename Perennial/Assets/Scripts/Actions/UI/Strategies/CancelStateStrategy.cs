using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;

namespace Perennial.Actions.UI.Strategies
{
    [CreateAssetMenu(fileName = "Cancel",  menuName = "UI/Actions/Cancel")]
    public class CancelStateStrategy : ButtonStrategy
    {
        public override void Press()
        {
            // Change the action state
            EventBus<ChangeActionState>.Raise(new ChangeActionState()
            {
                StateType = ActionStateType.Nothing,
                SelectedPlantDefinition = null,
            });
        }
    }
}
