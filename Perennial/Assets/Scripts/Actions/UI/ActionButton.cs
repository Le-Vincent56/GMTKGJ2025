using Perennial.Actions.UI.Strategies;
using Perennial.Core.Debugging;
using UnityEngine;
using UnityEngine.UI;
using LogType = Perennial.Core.Debugging.LogType;

namespace Perennial.Actions.UI
{
    public class ActionButton : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Sprite activeSprite;
        [SerializeField] private Sprite cancelSprite;
        private Button _button;
        private Image _image;
        private CanvasGroup _canvasGroup;
        private ActionsView _view;

        [Header("Fields")] 
        [SerializeField] private ButtonStrategy actionStrategy;
        [SerializeField] private ButtonStrategy cancelStrategy;
        [SerializeField] private float disabledAlpha;
        [SerializeField] private bool hasCancelState;
        private bool _usingButton;
        private bool _addedOnClick;
        
        private void OnEnable()
        {
            // Exit if there is no button assigned
            if (_button == null) return;
            
            // Exit if on click has already been added
            if (_addedOnClick) return;
            
            // Register onClick listeners
            _button.onClick.AddListener(HandleClick);
            _addedOnClick = true;
        }

        private void OnDisable()
        {
            // Exit if there is no button assigned
            if(_button ==  null) return;
            
            // Exit if on click has not been added
            if(!_addedOnClick) return;
            
            // Deregister onClick listeners
            _button.onClick.RemoveListener(HandleClick);
            _addedOnClick = false;
        }

        /// <summary>
        /// Initialize the Action Button
        /// </summary>
        /// <param name="view"></param>
        public void Initialize(ActionsView view)
        {
            _button = GetComponent<Button>();
            _image = GetComponentInChildren<Image>();
            _view = view;
            _canvasGroup = GetComponent<CanvasGroup>();
            _usingButton = false;
            _button.onClick.AddListener(HandleClick);
            _addedOnClick = true;
        }

        /// <summary>
        /// Enable the Action Button
        /// </summary>
        public void Enable()
        {
            _button.interactable = true;
            _canvasGroup.interactable = true;
            _canvasGroup.alpha = 1f;
        }
        
        /// <summary>
        /// Disable the Action Button
        /// </summary>
        public void Disable()
        {
            _button.interactable = false;
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = disabledAlpha;
        }

        /// <summary>
        /// Reset the button state to a usable button
        /// </summary>
        public void Reset()
        {
            _button.interactable = true;
            _canvasGroup.interactable = true;
            _canvasGroup.alpha = 1f;
            _image.sprite = activeSprite;
            _usingButton = false;
        }

        /// <summary>
        /// Handle the clicking of the button
        /// </summary>
        private void HandleClick()
        {
            // Exit if the button is not interactable
            if (!_button.interactable || !_canvasGroup.interactable) return;
            
            // Decide which function to use for clicking
            if (_usingButton && hasCancelState) CancelClick();
            else ActionClick();
        }

        /// <summary>
        /// Handle clicks when the button action is not active
        /// </summary>
        private void ActionClick()
        {
            // Remove any cancel states from other buttons
            _view.ResetButtons();

            // Activate the action strategy
            actionStrategy.Press();

            // Set to using the button
            _usingButton = true;
            _image.sprite = cancelSprite;
        }

        /// <summary>
        /// Handle button clicks when the button action is active
        /// </summary>
        private void CancelClick()
        {
            // Reset the button
            Reset();

            // Activate the cancel strategy
            cancelStrategy.Press();
        }
    }
}
