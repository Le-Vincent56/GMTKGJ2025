using System;
using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Perennial
{
    public class WinPopup : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Button replayButton;
        [SerializeField] private UnityEngine.UI.Button quitButton;
        [SerializeField] private Canvas winCanvas;


        private EventBinding<WinGameEvent> winGameEvent;

        private void Start()
        {
            winCanvas = GetComponent<Canvas>();
            winCanvas.enabled = false;
            replayButton.onClick.AddListener(Replay);
            quitButton.onClick.AddListener(LoadMainMenu);
        }

        private void OnEnable()
        {
            winGameEvent = new EventBinding<WinGameEvent>(WinGame);
            EventBus<WinGameEvent>.Register(winGameEvent);
        }

        private void OnDisable()
        {
            EventBus<WinGameEvent>.Deregister(winGameEvent);
        }

        private void WinGame()
        {
            winCanvas.enabled = true;
        }

        private void LoadMainMenu()
        {
            SceneManager.LoadScene(1);
        }

        private void Replay()
        {
            SceneManager.LoadScene(0);
        }
    }
}
