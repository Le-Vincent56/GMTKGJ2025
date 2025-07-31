using Perennial.Plants;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace Perennial
{
	/// <summary>
	/// The different states that a tile's soil can be
	/// </summary>
	public enum SoilState
	{
		GRASS, TILLED
	}

	public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		[SerializeField] private SpriteRenderer plantSpriteRenderer;
		[SerializeField] private SpriteRenderer soilSpriteRenderer;
		[SerializeField] private Canvas tooltipCanvas;
		[SerializeField] private SerializedDictionary<SoilState, Sprite> soilStateSprites;
		[Space]
		[SerializeField] private SoilState _soilState;
		[SerializeField] private Vector2Int _gardenPosition;
		private Plant _plant;

		/// <summary>
		/// The current plant object on this tile
		/// </summary>
		public Plant Plant
		{
			get => _plant;
			set
			{
				if (_plant != null)
				{
					return;
				}

				_plant = value;
				UpdatePlantSprite( );
			}
		}

		/// <summary>
		/// Whether or not this tile currently has a plant on it
		/// </summary>
		public bool HasPlant => (Plant != null);

		/// <summary>
		/// The current state of this tile's soil
		/// </summary>
		public SoilState SoilState
		{
			get => _soilState;
			set
			{
				_soilState = value;
				UpdateSoilSprite( );
			}
		}

		/// <summary>
		/// The position of this tile within the garden
		/// </summary>
		public Vector2Int GardenPosition { get => _gardenPosition; set => _gardenPosition = value; }

		/// <summary>
		/// Whether or not the tooltip for this tile is currently active and visible
		/// </summary>
		public bool IsTooltipEnabled
		{
			get => tooltipCanvas.gameObject.activeSelf;
			set => tooltipCanvas.gameObject.SetActive(value);
		}

		private void OnValidate ( )
		{
			UpdatePlantSprite( );
			UpdateSoilSprite( );
		}

		private void Awake ( )
		{
			OnValidate( );

			tooltipCanvas.worldCamera = Camera.main;
			IsTooltipEnabled = false;
		}

		/// <summary>
		/// Update the plant sprite for this tile
		/// </summary>
		public void UpdatePlantSprite ( )
		{
			// Something like this eventually
			// plantSpriteRenderer.sprite = Plant.GetSprite( );
		}

		/// <summary>
		/// Update the soil sprite for this tile
		/// </summary>
		public void UpdateSoilSprite ( )
		{
			// Later this can be expanded to update surrounding tile soil sprites as well
			soilSpriteRenderer.sprite = soilStateSprites[SoilState];
		}

		public void OnPointerEnter (PointerEventData eventData)
		{
			IsTooltipEnabled = true;
		}

		public void OnPointerExit (PointerEventData eventData)
		{
			IsTooltipEnabled = false;
		}

		public void OnPointerClick (PointerEventData eventData)
		{
			Debug.Log("Tile Clicked");
		}
	}
}
