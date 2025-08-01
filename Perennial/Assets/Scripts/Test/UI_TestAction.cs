using Perennial.Actions.Commands;
using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;

namespace Perennial.Test
{
    public class UI_TestAction : MonoBehaviour
    {
        private UnityEngine.UI.Button _button;

        void Start()
        {
            _button = GetComponent<UnityEngine.UI.Button>();
            _button.onClick.AddListener(() =>
            {
                EventBus<PerformCommand>.Raise(new PerformCommand()
                {
                    Command =  BaseCommand.Create<HarvestCommand>()
                });
            }); 
        }
    }
}
