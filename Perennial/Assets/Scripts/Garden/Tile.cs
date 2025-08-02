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
					Command = BaseCommand.Create<HarvestCommand>(new HarvestArgs{ GardenManager = GardenManager, Tile = this })
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
					Command = BaseCommand.Create<PlantCommand>(new PlantArgs{ GardenManager = GardenManager, Tile = this, PlantDefinition = currentState.StoredPlantDefinition})
				});
			}
			else if (currentActionState is TillActionState)
			{
				if (SoilState != SoilState.TILLED || (SoilState == SoilState.TILLED && Plant == null))
				{
					return;
				}

				EventBus<PerformCommand>.Raise(new PerformCommand( )
				{
					Command = BaseCommand.Create<TillCommand>(new TillArgs{ GardenManager = GardenManager, Tile = this })
				});
			}
		}
	}
}
