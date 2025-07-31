using Perennial.Plants;
using UnityEngine;
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

	public class Tile : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer plantSpriteRenderer;
		[SerializeField] private SpriteRenderer soilSpriteRenderer;
		[SerializeField] private SerializedDictionary<SoilState, Sprite> soilStateSprites;
		[Space]
		[SerializeField] private SoilState _soilState;
		private Plant _plant;

		/// <summary>
		/// The current plant object on this tile
		/// </summary>
		public Plant Plant
		{
			get => _plant;
			set
			{
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

		private void OnValidate ( )
		{
			UpdatePlantSprite( );
			UpdateSoilSprite( );
		}

		private void Awake ( )
		{
			OnValidate( );
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
			soilSpriteRenderer.sprite = soilStateSprites[SoilState];
		}
	}
}
