using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;

namespace Perennial.Actions.UI.Strategies
{
    public abstract class ButtonStrategy : ScriptableObject
    {
        public abstract void Press();
    }
}
