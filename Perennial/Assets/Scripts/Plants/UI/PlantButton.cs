using System;
using Perennial.Core.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Perennial.Plants.UI
{
    public class PlantButton : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Image spriteImage;
        [SerializeField] private TextMeshProUGUI text;
        private Button _button;
        private PlantDefinition _plantDefinition;
        
        public SerializableGuid ID => _plantDefinition.ID;

        public string Amount
        {
            get => text.text;
            set => text.text = value;
        }
        
        /// <summary>
        /// Initialize the Plant Button
        /// </summary>
        
        public void Initialize(PlantDefinition associatedPlant)
        {
            // Set references
            _button = GetComponentInChildren<Button>();
            _plantDefinition = associatedPlant;
            
            // Set the sprite
            spriteImage.sprite = associatedPlant.SeedSprite;
        }

        public void RegisterListener(UnityAction action) => _button.onClick.AddListener(action);
        public void UnregisterListener(UnityAction action) => _button.onClick.RemoveListener(action);
    }
}
