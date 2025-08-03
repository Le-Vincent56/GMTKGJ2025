using Perennial.Actions.Commands;
using Perennial.Core.Architecture.Event_Bus.Events;
using Perennial.Core.Architecture.Event_Bus;
using Perennial.Plants;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using Perennial.TurnManagement;
using Perennial.TurnManagement.States.ActionStates;
using Perennial.Core.Architecture.State_Machine;
using Perennial.TurnManagement.States;
using TMPro;
using System.Collections.Generic;
using Perennial.Seasons;

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
		[Space]
		[SerializeField] private GameObject plantTooltipContainer;
		[SerializeField] private TextMeshProUGUI plantNameText;
		[SerializeField] private TextMeshProUGUI plantFoodText;
		[SerializeField] private TextMeshProUGUI plantLifetimeText;
		[SerializeField] private TextMeshProUGUI plantTurnsUntilText;
		[SerializeField] private TextMeshProUGUI plantAbilityText;
		[SerializeField] private TextMeshProUGUI plantUngrowableText;
		[SerializeField] private TextMeshProUGUI plantBonusText;
		[SerializeField] private GameObject emptyTooltipContainer;
		[SerializeField] private TextMeshProUGUI emptySoilText;
		[SerializeField] private TextMeshProUGUI emptyPlantableText;

		private Plant _plant;

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

				UpdateTooltip( );
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
				UpdateTooltip( );
			}
		}

		/// <summary>
		/// The position of this tile within the garden
		/// </summary>
		public Vector2Int GardenPosition
		{
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
		public GardenManager GardenManager { get; private set; }

		/// <summary>
		/// A reference to the turn controller object
		/// </summary>
		public TurnController TurnController { get; private set; }

		private void OnValidate ( )
		{
			UpdateSoilSprite( );
		}

		private void Awake ( )
		{
			OnValidate( );

			GardenManager = FindFirstObjectByType<GardenManager>( );
			TurnController = FindFirstObjectByType<TurnController>( );

			tooltipCanvas.worldCamera = Camera.main;
			IsTooltipEnabled = false;
		}

		/// <summary>
		/// Update the UI tooltip for this tile
		/// </summary>
		public void UpdateTooltip ( )
		{
			plantTooltipContainer.SetActive(Plant != null);
			emptyTooltipContainer.SetActive(Plant == null);

			if (Plant != null)
			{
				plantNameText.text = Plant.Name;
				plantFoodText.text = $"<color=#3E2309>{Plant.Rewards.CalculateFood(lifetimeOffset: 1).Value} Food</color> <color=#9EC699>+ {Plant.Rewards.CalculateFood(lifetimeOffset: 1).Value}</color>";
				plantLifetimeText.text = $"<color=#DDDDDD>Lifetime:</color> {Plant.Definition.HarvestTime + Plant.Definition.GrowTime} Months";
				plantTurnsUntilText.text = (Plant.Lifetime.FullyGrown ? $"<color=#DDDDDD>Dies in:</color> {Plant.Definition.HarvestTime + Plant.Definition.GrowTime - Plant.Lifetime.CurrentLifetime.Value + 1} Months" : $"<color=#DDDDDD>Grows in:</color> {Plant.Definition.GrowTime - Plant.Lifetime.CurrentLifetime.Value + 1} Months");
				plantAbilityText.text = Plant.Definition.Description;
				plantUngrowableText.text = $"<color=#DDDDDD>Ungrowable during:</color>\n<b>{GetSeasonString(Plant.Definition.IncompatibleSeasons)}</b>";
				plantBonusText.text = $"<color=#DDDDDD>Bonus during:</color>\n<b>{GetSeasonString(Plant.Definition.BonusSeasons)}</b>";
			}
			else
			{
				switch (SoilState)
				{
					case SoilState.TILLED:
						emptySoilText.text = $"Soil is <color=#EAD7A1>Tilled</color>";
						emptyPlantableText.text = "Tile is <color=#9EC699>Plantable</color>";
						break;
					case SoilState.WEEDS:
						emptySoilText.text = $"Soil is <color=#4C4128>Untilled</color>";
						emptyPlantableText.text = "Tile is <color=#CA4948>Unplantable</color>";
						break;
				}
			}
		}

		/// <summary>
		/// Convert a list of seasons to a string
		/// </summary>
		/// <param name="seasons">The list of seasons to convert</param>
		/// <returns>A formatted list of the seasons with rich text</returns>
		private string GetSeasonString (List<Season> seasons)
		{
			if (seasons.Count == 0)
			{
				return "None";
			}

			string seasonString = "";
			for (int i = 0; i < seasons.Count; i++)
			{
				switch (seasons[i])
				{
					case Season.Spring:
						seasonString += $"<color=#EEBCD5>Spring</color>";
						break;
					case Season.Summer:
						seasonString += $"<color=#9EC699>Summer</color>";
						break;
					case Season.Fall:
						seasonString += $"<color=#D9A059>Fall</color>";
						break;
					case Season.Winter:
						seasonString += $"<color=#B0EBF0>Winter</color>";
						break;
				}

				if (i + 1 < seasons.Count)
				{
					seasonString += ", ";
				}
			}

			return seasonString;
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

		/// <summary>
		/// Temporary? fix to the property not allowing plant to be null
		/// </summary>
		public void RemovePlantFromTile ( )
		{
			_plant = null;
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
			ActionState currentState = TurnController.StateMachine.GetState( ) as ActionState;

			if (currentState == null)
			{
				return;
			}

			IState currentActionState = currentState.ActionStateMachine.GetState( );

			if (currentActionState is HarvestActionState)
			{
				if (Plant == null || !Plant.Lifetime.FullyGrown)
				{
					return;
				}

				EventBus<PerformCommand>.Raise(new PerformCommand( )
				{
					Command = BaseCommand.Create<HarvestCommand>(new HarvestArgs { GardenManager = GardenManager, Tile = this })
				});
			}
			else if (currentActionState is PlantActionState)
			{
				if (Plant != null || SoilState != SoilState.TILLED)
				{
					return;
				}

				EventBus<PerformCommand>.Raise(new PerformCommand( )
				{
					Command = BaseCommand.Create<PlantCommand>(new PlantArgs { GardenManager = GardenManager, Tile = this, PlantDefinition = currentState.StoredPlantDefinition })
				});

				EventBus<TakePlant>.Raise(new TakePlant( )
				{
					ID = _plant.Definition.ID
				});
			}
			else if (currentActionState is TillActionState)
			{
				if ((SoilState == SoilState.TILLED && Plant == null))
				{
					return;
				}

				EventBus<PerformCommand>.Raise(new PerformCommand( )
				{
					Command = BaseCommand.Create<TillCommand>(new TillArgs { GardenManager = GardenManager, Tile = this })
				});
			}
		}
	}
}
