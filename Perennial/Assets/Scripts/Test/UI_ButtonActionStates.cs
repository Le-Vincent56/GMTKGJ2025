using System;
using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Perennial.Test
{
    public class UI_ButtonActionStates : MonoBehaviour
    {
        [SerializeField] private ActionStateType actionStateType;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() =>
            {
                EventBus<ChangeActionState>.Raise(new ChangeActionState()
                {
                    StateType = actionStateType
                });
            });
        }
    }
}
