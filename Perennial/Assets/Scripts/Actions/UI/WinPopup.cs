using System;
using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;

namespace Perennial
{
    public class WinPopup : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Button replayButton;
        [SerializeField] private UnityEngine.UI.Button quitButton;


        private EventBinding<WinGameEvent> winGameEvent;
        private void OnEnable()
        {
           
        }
    }
}
