using System.Collections.Generic;
using Perennial.VFX;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Perennial.Seasons
{
    public class SeasonView : SerializedMonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private UnityEngine.UI.Image image;
        [SerializeField] private TextMeshProUGUI monthText;
        [SerializeField] private Dictionary<Month, Sprite> _imageDictionary;
        
        public void Initialize(Month currentMonth)
        {
            UpdateUI(currentMonth);
        }

        /// <summary>
        /// Update the food text
        /// </summary>
        public void UpdateUI(Month month)
        {
            image.sprite = _imageDictionary[month];
            monthText.text = month.ToString();
        }
    }
}
