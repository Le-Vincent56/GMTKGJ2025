using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;

namespace Perennial.Actions.UI.Strategies
{
    [CreateAssetMenu(fileName = "Pass Turn",  menuName = "UI/Actions/Pass Turn")]
    public class PassTurnStrategy : ButtonStrategy
    {
        public override void Press() => EventBus<EndTurn>.Raise(new EndTurn());
    }
}
