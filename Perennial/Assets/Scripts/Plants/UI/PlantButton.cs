using UnityEngine;
using UnityEngine.UI;

namespace Perennial.Plants.UI
{
    public class PlantButton : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Image spriteImage;
        private Button _button;
        private PlantDefinition _plantDefinition;
        
        /// <summary>
        /// Initialize the Plant Button
        /// </summary>
        
        public void Initialize(PlantDefinition associatedPlant)
        {
            // Set references
            _button = GetComponent<Button>();
            _plantDefinition = associatedPlant;
            
            // Set the sprite
            spriteImage.sprite = associatedPlant.PlantSprite;
            
            // Set event listeners
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            if (_button == null) return;
            
            _button.onClick.RemoveListener(OnClick);
        }

        /// <summary>
        /// Handle click functionality for button
        /// </summary>
        private void OnClick()
        {
            
        }
    }
}
