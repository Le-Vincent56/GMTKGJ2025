using System;
using Perennial.Core.Extensions;
using Perennial.Garden;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Perennial.Plants.UI
{
	public class PlantButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[Header("References")]
		[SerializeField] private Image spriteImage;
		[SerializeField] private GameObject tooltip;
		[SerializeField] private TextMeshProUGUI plantNameText;
		[SerializeField] private TextMeshProUGUI plantLifetimeText;
		[SerializeField] private TextMeshProUGUI plantUngrowableSeasonsText;
		[SerializeField] private TextMeshProUGUI plantBonusSeasonsText;
		[SerializeField] private TextMeshProUGUI plantAbilityText;
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

		public void Initialize (PlantDefinition associatedPlant)
		{
			// Set references
			_button = GetComponentInChildren<Button>( );
			_plantDefinition = associatedPlant;

			// Set the sprite
			spriteImage.sprite = associatedPlant.SeedSprite;

			// Set tooltip text
			plantNameText.text = associatedPlant.Name;
			plantLifetimeText.text = $"<color=#DDDDDD>Lifetime:</color> {associatedPlant.HarvestTime + associatedPlant.GrowTime} Months";
			plantAbilityText.text = associatedPlant.Description;
			plantUngrowableSeasonsText.text = $"<color=#DDDDDD>Ungrowable during:</color>\n<b>{Tile.GetSeasonString(associatedPlant.IncompatibleSeasons)}</b>";
			plantBonusSeasonsText.text = $"<color=#DDDDDD>Bonus during:</color>\n<b>{Tile.GetSeasonString(associatedPlant.BonusSeasons)}</b>";

			//tooltip.GetComponent<RectTransform>( ).position = new Vector3(283, 327);
			tooltip.SetActive(false);
		}

		public void RegisterListener (UnityAction action) => _button.onClick.AddListener(action);
		public void UnregisterListener (UnityAction action) => _button.onClick.RemoveListener(action);

		public void OnPointerExit (PointerEventData eventData)
		{
			tooltip.SetActive(false);
		}

		public void OnPointerEnter (PointerEventData eventData)
		{
			tooltip.SetActive(true);
		}
	}
}
