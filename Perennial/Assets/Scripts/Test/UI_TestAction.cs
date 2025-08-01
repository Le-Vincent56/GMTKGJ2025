using UnityEngine;

namespace Perennial.Test
{
    public class UI_TestAction : MonoBehaviour
    {
        private UnityEngine.UI.Button _buttonPassTurn;

        void Start()
        {
            _buttonPassTurn = GetComponent<UnityEngine.UI.Button>();
            _buttonPassTurn.onClick.AddListener(() =>
            {
                EventBus<EndTurn>.Raise(new EndTurn());
            }); 
        }
    }
}
