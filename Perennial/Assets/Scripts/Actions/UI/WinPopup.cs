using System;
using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Perennial
{
    public class WinPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Image titleImage;
		[SerializeField] private Button replayButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Canvas winCanvas;
        [SerializeField] private Image loseImage;


        private EventBinding<WinGameEvent> winGameEvent;
        private EventBinding<LoseGameEvent> loseGameEvent;

        private void Start()
        {
            winCanvas = GetComponent<Canvas>();
            winCanvas.enabled = false;
            replayButton.onClick.AddListener(Replay);
            //quitButton.onClick.AddListener(LoadMainMenu);
        }

        private void OnEnable()
        {
            winGameEvent = new EventBinding<WinGameEvent>(WinGame);
            loseGameEvent = new EventBinding<LoseGameEvent>(LoseGame);

			EventBus<WinGameEvent>.Register(winGameEvent);
            EventBus<LoseGameEvent>.Register(loseGameEvent);
        }

        private void OnDisable()
        {
            EventBus<WinGameEvent>.Deregister(winGameEvent);
			EventBus<LoseGameEvent>.Deregister(loseGameEvent);
		}

        private void WinGame()
        {
            winCanvas.enabled = true;
            titleText.text = "you won";
            titleImage.color = new Color(158 / 255f, 198 / 255f, 153 / 255f);
		}

        private void LoseGame()
        {
            winCanvas.enabled = true;
            loseImage.enabled = true;
            titleText.text = "you lose";
			titleImage.color = new Color(202 / 255f, 73 / 255f, 72 / 255f);
		}

        private void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        private void Replay()
        {
            SceneManager.LoadScene(0);
        }
    }
}
