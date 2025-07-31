using Perennial.Plants;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Perennial.Garden
{
	public class Garden : MonoBehaviour
	{
		[SerializeField] private GameObject tilePrefab;
		[Space]
		[SerializeField, Range(1, 20)] private int _gardenWidth = 1;
		[SerializeField, Range(1, 20)] private int _gardenHeight = 1;
		[SerializeField, Range(0f, 1f)] private float startTilledPercentage = 0.5f;
		[SerializeField, Range(0f, 1f)] private float _plantMutationPercentage = 0.15f;

		// [0, 0] corresponds to the bottom-left corner of the garden
		private Tile[ , ] garden;
		private List<Tile> _plantedTiles;
		private List<Tile> _tiles;

		/// <summary>
		/// The width of the garden in tiles
		/// </summary>
		public int GardenWidth => _gardenWidth;

		/// <summary>
		/// The height of the garden in tiles
		/// </summary>
		public int GardenHeight => _gardenHeight;

		/// <summary>
		/// The total area of the garden, or the total number of tiles within the garden
		/// </summary>
		public int GardenArea => GardenWidth * GardenHeight;

		/// <summary>
		/// A list of all the tiles that currently have plants on them
		/// </summary>
		public List<Tile> PlantedTiles { get => _plantedTiles; private set => _plantedTiles = value; }

		/// <summary>
		/// A list of all the tiles that make up the garden
		/// </summary>
		public List<Tile> Tiles { get => _tiles; private set => _tiles = value; }

		/// <summary>
		/// The chance for plants to mutate on any given check
		/// </summary>
		public float PlantMutationPercentage => _plantMutationPercentage;

		private void Awake ( )
		{
			garden = new Tile[GardenWidth, GardenHeight];
			PlantedTiles = new List<Tile>( );
			Tiles = new List<Tile>( );
		}

		private void Start ( )
		{
			// NOTE: This can be changed later to load in static tiles that were placed in the scene manually
			// Having them generate might be good for playtesting a get a good size for the garden
			GenerateTiles( );
		}

		/// <summary>
		/// Check to see if a given position is inside the bounds of the garden
		/// </summary>
		/// <param name="x">The x coordinate to check</param>
		/// <param name="y">The y coordinate to check</param>
		/// <returns>true if the position is within the bounds of the garden, false otherwise</returns>
		public bool IsPositionInBounds (int x, int y)
		{
			return (x >= 0 && x < GardenWidth && y >= 0 && y < GardenHeight);
		}

		/// <summary>
		/// Gets a reference to a tile in the garden at a specific position
		/// </summary>
		/// <param name="x">The x coordinate of the position to get</param>
		/// <param name="y">The y coordinate of the position to get</param>
		/// <returns>A reference to the tile if the specified position is within the bounds of the garden, null otherwise</returns>
		public Tile GetTileAtPosition (int x, int y)
		{
			if (IsPositionInBounds(x, y))
			{
				return garden[x, y];
			}

			return null;
		}

		/// <summary>
		/// Get a reference to a plant in the garden at a specific position
		/// </summary>
		/// <param name="x">The x coordinate of the position to get</param>
		/// <param name="y">The y coordinate of the position to get</param>
		/// <returns>A reference to the plant if the specified position is within the bounds of the garden AND there is a plant currently at that position, null otherwise</returns>
		public Plant GetPlantAtPosition (int x, int y)
		{
			Tile tile = GetTileAtPosition(x, y);

			if (tile != null)
			{
				return tile.Plant;
			}

			return null;
		}

		/// <summary>
		/// Add a specific plant at a position in the garden
		/// </summary>
		/// <param name="plant">The plant to add to the garden</param>
		/// <param name="x">The x coordinate to place the plant at</param>
		/// <param name="y">The y coordinate to place the plant at</param>
		/// <returns>true if the plant was successfully placed in the garden, false otherwise</returns>
		public bool AddPlantAtPosition (Plant plant, int x, int y)
		{
			Tile tile = GetTileAtPosition(x, y);

			if (tile == null || tile.HasPlant)
			{
				return false;
			}

			tile.Plant = plant;
			PlantedTiles.Add(tile);
			return true;
		}

		/// <summary>
		/// Remove a plant at a specific position in the garden
		/// </summary>
		/// <param name="x">The x coordinate to remove the plant at</param>
		/// <param name="y">The y coordinate to remove the plant at</param>
		/// <returns>true if a plant was successfully removed from the garden at the specified position, false otherwise</returns>
		public bool RemovePlantAtPosition (int x, int y)
		{
			Tile tile = GetTileAtPosition(x, y);

			if (tile == null || !tile.HasPlant)
			{
				return false;
			}

			tile.Plant = null;
			PlantedTiles.Remove(tile);
			return true;
		}

		/// <summary>
		/// Get all of the surrounding tiles around a position within a given radius. Surrounding tiles includes all cardinal directions as well as all four diagonal directions
		/// </summary>
		/// <param name="x">The x position to get the surrounding tiles of</param>
		/// <param name="y">The y position to get the surrounding tiles of</param>
		/// <param name="radius">The radius around the specified to get the surrounding tiles of. The minimum value this can be is 1</param>
		/// <returns>A list of surrounding tiles to the specified position, excluding the tile at the specified position. There will be no null values in this list</returns>
		public List<Tile> GetSurroundingTiles (int x, int y, int radius = 1)
		{
			List<Tile> surroundingTiles = new List<Tile>( );
			radius = Mathf.Max(1, radius);

			Tile tile;
			for (int i = -radius; i >= radius; i++)
			{
				for (int j = -radius; j >= radius; j++)
				{
					if (i == 0 && j == 0)
					{
						continue;
					}

					tile = GetTileAtPosition(x + i, y + j);

					if (tile != null)
					{
						surroundingTiles.Add(tile);
					}
				}
			}

			return surroundingTiles;
		}

		/// <summary>
		/// Get all of the surrounding plants around a position within a given radius. Surrounding plants includes all cardinal directions as well as all four diagonal directions
		/// </summary>
		/// <param name="x">The x position to get the surrounding plants of</param>
		/// <param name="y">The y position to get the surrounding plants of</param>
		/// <param name="radius">The radius around the specified to get the surrounding plants of. The minimum value this can be is 1</param>
		/// <returns>A list of surrounding plants to the specified position, excluding the plant on the tile at the specified position. There will be no null values in this list</returns>
		public List<Plant> GetSurroundingPlants (int x, int y, int radius = 1)
		{
			List<Plant> surroundingPlants = new List<Plant>( );
			radius = Mathf.Max(1, radius);

			Plant plant;
			for (int i = -radius; i >= radius; i++)
			{
				for (int j = -radius; j >= radius; j++)
				{
					if (i == 0 && j == 0)
					{
						continue;
					}

					plant = GetPlantAtPosition(x + i, y + j);

					if (plant != null)
					{
						surroundingPlants.Add(plant);
					}
				}
			}

			return surroundingPlants;
		}

		/// <summary>
		/// Get a list of all the tiles that fall within a specific rectangular section of the garden
		/// </summary>
		/// <param name="x">The x position of the rectangular section. This corresponds to the bottom-left corner</param>
		/// <param name="y">The y position of the rectangular section. This corresponds to the bottom-left corner</param>
		/// <param name="width">The width of the rectangular section. The minimum value this can be is 1</param>
		/// <param name="height">The height of the rectangular section. The minimum value this can be is 1</param>
		/// <returns>A list of all the tiles that fall within the rectangular section. There will be no null values in this list</returns>
		public List<Tile> GetTilesInSection (int x, int y, int width, int height)
		{
			List<Tile> tilesInSection = new List<Tile>( );
			width = Mathf.Max(1, width);
			height = Mathf.Max(1, height);

			Tile tile;
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					tile = GetTileAtPosition(x + i, y + j);

					if (tile != null)
					{
						tilesInSection.Add(tile);
					}
				}
			}

			return tilesInSection;
		}

		/// <summary>
		/// Get a list of all the plants that fall within a specific rectangular section of the garden
		/// </summary>
		/// <param name="x">The x position of the rectangular section. This corresponds to the bottom-left corner</param>
		/// <param name="y">The y position of the rectangular section. This corresponds to the bottom-left corner</param>
		/// <param name="width">The width of the rectangular section. The minimum value this can be is 1</param>
		/// <param name="height">The height of the rectangular section. The minimum value this can be is 1</param>
		/// <returns>A list of all the plants that fall within the rectangular section. There will be no null values in this list</returns>
		public List<Plant> GetPlantsInSection (int x, int y, int width, int height)
		{
			List<Plant> plantsInSection = new List<Plant>( );
			width = Mathf.Max(1, width);
			height = Mathf.Max(1, height);

			Plant plant;
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					plant = GetPlantAtPosition(x + i, y + j);

					if (plant != null)
					{
						plantsInSection.Add(plant);
					}
				}
			}

			return plantsInSection;
		}

		/// <summary>
		/// Generate a 2D grid of all the tiles in the garden
		/// </summary>
		private void GenerateTiles ( )
		{
			float offsetX = transform.position.x - (GardenWidth / 2f) + 0.5f;
			float offsetY = transform.position.y - (GardenHeight / 2f) + 0.5f;

			// Spawn in tile objects
			for (int i = 0; i < GardenWidth; i++)
			{
				for (int j = 0; j < GardenHeight; j++)
				{
					// Tiles will parented to the garden object
					// The position of the garden object will be the center of the tile grid
					Tile tile = Instantiate(tilePrefab, transform).GetComponent<Tile>( );
					tile.GardenPosition = new Vector2Int(i, j);
					tile.transform.localPosition = new Vector3(offsetX + i, offsetY + j);

					garden[i, j] = tile;
					Tiles.Add(tile);
				}
			}

			// Randomly till a certain percentage of tiles at the start of the game
			List<Tile> shuffledTileList = Tiles.OrderBy(x => Random.value).ToList( );
			int tilledSoilCount = Mathf.CeilToInt(GardenArea * startTilledPercentage);
			for (int i = 0; i < tilledSoilCount; i++)
			{
				shuffledTileList[i].SoilState = SoilState.TILLED;
			}
		}
	}
}
