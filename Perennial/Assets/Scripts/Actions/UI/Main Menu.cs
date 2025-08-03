using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Perennial
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Button playButton;

        private void Start()
        {
            playButton.onClick.AddListener(Play);
        }
        
        private void Play()
        {
            SceneManager.LoadScene(0);
        }
    }
}
