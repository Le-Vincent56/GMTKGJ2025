using Perennial.Plants;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace Perennial.Garden
{
	/// <summary>
	/// The different states that a tile's soil can be
	/// </summary>
	public enum SoilState
	{
		WEEDS, TILLED
	}

	public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		[SerializeField] private SpriteRenderer plantSpriteRenderer;
		[SerializeField] private SpriteRenderer soilSpriteRenderer;
		[SerializeField] private Canvas tooltipCanvas;
		[SerializeField] private SerializedDictionary<SoilState, Sprite> soilStateSprites;
		[Space]
		[SerializeField] private SoilState soilState;
		[SerializeField] private Vector2Int gardenPosition;
		[SerializeField] private bool isAtGardenEdge;

		private Plant _plant;
		private GardenManager _gardenManager;

		/// <summary>
		/// The current plant object on this tile. This should not be updated outside the garden's add/remove plant methods to ensure everything is updated properly
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
			get => soilState;
			set
			{
				soilState = value;
				UpdateSoilSprite( );
			}
		}

		/// <summary>
		/// The position of this tile within the garden
		/// </summary>
		public Vector2Int GardenPosition {
			get => gardenPosition;
			set
			{
				gardenPosition = value;
				IsAtGardenEdge = GardenManager.IsPositionAtGardenEdge(gardenPosition.x, gardenPosition.y);
			}
		}

		/// <summary>
		/// Whether or not this tile is at the edge of the garden
		/// </summary>
		public bool IsAtGardenEdge { get => isAtGardenEdge; private set => isAtGardenEdge = value; }

		/// <summary>
		/// Whether or not the tooltip for this tile is currently active and visible
		/// </summary>
		public bool IsTooltipEnabled
		{
			get => tooltipCanvas.gameObject.activeSelf;
			set => tooltipCanvas.gameObject.SetActive(value);
		}

		/// <summary>
		/// A reference to the garden manager object
		/// </summary>
		public GardenManager GardenManager { get; set; }

		private void OnValidate ( )
		{
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
		public void UpdatePlantSprite (Sprite plantSprite)
		{
			plantSpriteRenderer.sprite = plantSprite;
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
