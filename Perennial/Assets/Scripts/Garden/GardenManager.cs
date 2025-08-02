using System;
using Perennial.Core.Architecture.Singletons;
using Perennial.Plants;
using System.Collections.Generic;
using System.Linq;
using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Perennial.Garden
{
	public class GardenManager : MonoBehaviour
	{
		[SerializeField] private GameObject tilePrefab;
		[Space]
		[SerializeField, Range(1, 20)] private int gardenWidth = 1;
		[SerializeField, Range(1, 20)] private int gardenHeight = 1;
		[SerializeField, Range(0f, 1f)] private float startTilledPercentage = 0.5f;
		[SerializeField, Range(0f, 1f)] private float plantMutationPercentage = 0.1f;
		[SerializeField, Range(0f, 1f)] private float weedSpreadPercentage = 0.1f;

		// [0, 0] corresponds to the bottom-left corner of the garden
		private Tile[ , ] _garden;
		private List<Plant> _plants;
		private List<Tile> _tiles;

		private EventBinding<TurnStarted> _onTurnedStarted;
		private EventBinding<TurnEnded> _onTurnedEnded;

		/// <summary>
		/// The width of the garden in tiles
		/// </summary>
		public int GardenWidth => gardenWidth;

		/// <summary>
		/// The height of the garden in tiles
		/// </summary>
		public int GardenHeight => gardenHeight;

		/// <summary>
		/// The total area of the garden, or the total number of tiles within the garden
		/// </summary>
		public int GardenArea => GardenWidth * GardenHeight;

		/// <summary>
		/// A list of all the tiles that currently have plants on them
		/// </summary>
		public List<Plant> Plants { get => _plants; private set => _plants = value; }

		/// <summary>
		/// A list of all the tiles that make up the garden
		/// </summary>
		public List<Tile> Tiles { get => _tiles; private set => _tiles = value; }

		/// <summary>
		/// The chance for plants to mutate on any given check
		/// </summary>
		public float PlantMutationPercentage => plantMutationPercentage;

		/// <summary>
		/// The chance for weed to spread on any given check
		/// </summary>
		public float WeedSpreadPercentage => weedSpreadPercentage;

		private void Awake ( )
		{
			_garden = new Tile[GardenWidth, GardenHeight];
			Plants = new List<Plant>( );
			Tiles = new List<Tile>( );
		}

		private void Start ( )
		{
			// NOTE: This can be changed later to load in static tiles that were placed in the scene manually
			// Having them generate might be good for playtesting a get a good size for the garden
			GenerateTiles( );
		}

		private void OnEnable ( )
		{
			_onTurnedStarted = new EventBinding<TurnStarted>(StartTurn);
			_onTurnedEnded = new EventBinding<TurnEnded>(EndTurn);

			EventBus<TurnEnded>.Register(_onTurnedEnded);
			EventBus<TurnStarted>.Register(_onTurnedStarted);
		}

		private void OnDisable ( )
		{
			EventBus<TurnEnded>.Deregister(_onTurnedEnded);
			EventBus<TurnStarted>.Deregister(_onTurnedStarted);
		}

		private void StartTurn(TurnStarted eventData)
		{
			TickAllPlants();
		}

		private void EndTurn(TurnEnded eventData)
		{
			UpdatePlantMutations();
			UpdateWeedSpread();
		}

		// For testing grass spread
		//float timer = 0f;
		//private void Update ( )
		//{
		//	timer += Time.deltaTime;
		//	if (timer >= 1f)
		//	{
		//		Debug.Log("Updated Grass Spread");
		//		UpdateGrassSpread( );
		//		timer = 0f;
		//	}
		//}

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
		/// Check to see if a specific position is along the edges of the garden
		/// </summary>
		/// <param name="x">The x coordinate to check</param>
		/// <param name="y">The y coordinate to check</param>
		/// <returns>true if the position is on the edges of the garden, false otherwise</returns>
		public bool IsPositionAtGardenEdge (int x, int y)
		{
			return (x == 0 || x == GardenWidth - 1 || y == 0 || y == GardenHeight - 1);
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
				return _garden[x, y];
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
		/// Add a specific plant to a tile in the garden
		/// </summary>
		/// <param name="plant">The plant to add to the garden</param>
		/// <param name="tile">The tile to add the plant to</param>
		/// <returns>true if the plant was successfully placed in the garden, false otherwise</returns>
		public bool AddPlantToTile (Plant plant, Tile tile)
		{
			if (tile == null || tile.HasPlant)
			{
				return false;
			}

			Plants.Add(plant);
			tile.Plant = plant;
			
			// Activate plant placement logic
			plant.Place();
			
			return true;
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
			return AddPlantToTile(plant, GetTileAtPosition(x, y));
		}

		/// <summary>
		/// Remove a plant on a specific tile in the garden
		/// </summary>
		/// <param name="tile">The tile to remove the plant from</param>
		/// <returns>true if a plant was successfully removed from the garden at the specified position, false otherwise</returns>
		public bool RemovePlantFromTile (Tile tile)
		{
			if (tile == null || !tile.HasPlant)
			{
				return false;
			}
			
			Plants.Remove(tile.Plant);
			tile.RemovePlantFromTile();
			tile.UpdatePlantSprite(null);
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
			return RemovePlantFromTile(GetTileAtPosition(x, y));
		}

		/// <summary>
		/// Get all of the surrounding tiles around a tile within a given radius. Surrounding tiles includes all cardinal directions as well as all four diagonal directions
		/// </summary>
		/// <param name="tile">The tile to check the surrounding tiles of</param>
		/// <param name="radius">The radius around the specified tile to get the surrounding tiles of. The minimum value this can be is 1</param>
		/// <param name="onlyEmptyTiles">If true, all tiles returned in the final list will be tiles without a plant on them</param>
		/// <returns>A list of surrounding tiles to the specified tile, excluding the specified tile. There will be no null values in this list</returns>
		public List<Tile> GetSurroundingTiles (Tile tile, int radius = 1, bool onlyEmptyTiles = false)
		{
			return GetSurroundingTiles(tile.GardenPosition.x, tile.GardenPosition.y, radius: radius, onlyEmptyTiles: onlyEmptyTiles);
		}

		/// <summary>
		/// Get all of the surrounding tiles around a position within a given radius. Surrounding tiles includes all cardinal directions as well as all four diagonal directions
		/// </summary>
		/// <param name="x">The x position to get the surrounding tiles of</param>
		/// <param name="y">The y position to get the surrounding tiles of</param>
		/// <param name="radius">The radius around the specified position to get the surrounding tiles of. The minimum value this can be is 1</param>
		/// <param name="onlyEmptyTiles">If true, all tiles returned in the final list will be tiles without a plant on them</param>
		/// <returns>A list of surrounding tiles to the specified position, excluding the tile at the specified position. There will be no null values in this list</returns>
		public List<Tile> GetSurroundingTiles (int x, int y, int radius = 1, bool onlyEmptyTiles = false)
		{
			List<Tile> surroundingTiles = new List<Tile>( );
			radius = Mathf.Max(1, radius);

			Tile tile;
			for (int i = -radius; i <= radius; i++)
			{
				for (int j = -radius; j <= radius; j++)
				{
					if (i == 0 && j == 0)
					{
						continue;
					}

					tile = GetTileAtPosition(x + i, y + j);

					if (tile == null)
					{
						continue;
					}

					if (onlyEmptyTiles && tile.HasPlant)
					{
						continue;
					}

					surroundingTiles.Add(tile);
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
			for (int i = -radius; i <= radius; i++)
			{
				for (int j = -radius; j <= radius; j++)
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
		/// <param name="onlyEmptyTiles">If true, all tiles returned in the final list will be tiles without a plant on them</param>
		/// <returns>A list of all the tiles that fall within the rectangular section. There will be no null values in this list</returns>
		public List<Tile> GetTilesInSection (int x, int y, int width, int height, bool onlyEmptyTiles = false)
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

					if (tile == null)
					{
						continue;
					}

					if (onlyEmptyTiles && tile.HasPlant)
					{
						continue;
					}

					tilesInSection.Add(tile);
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

					if (plant == null)
					{
						continue;
					}

					plantsInSection.Add(plant);
				}
			}

			return plantsInSection;
		}

		/// <summary>
		/// Get all of the plants that are on an inputted list of tiles
		/// </summary>
		/// <param name="tiles">The list of tiles to check</param>
		/// <returns>A list of all the plants that are on the tiles in the inputted list. There will be no null values in this list</returns>
		public List<Plant> GetPlantsOnTiles (List<Tile> tiles)
		{
			return tiles.Select(tile => tile.Plant).NotNull( ).ToList( );
		}

		/// <summary>
		/// Get a list of all the tiles within a list of tiles that do not currently have a plant on them
		/// </summary>
		/// <param name="tiles">The list of tiles to check</param>
		/// <returns>A list of tiles that contains all of the empty tiles from the inputted tile list</returns>
		public List<Tile> GetEmptyTiles (List<Tile> tiles)
		{
			return tiles.Where(tile => !tile.HasPlant).ToList( );
		}

		/// <summary>
		/// Get a list of tiles that have a certain soil state
		/// </summary>
		/// <param name="tiles">The list of tiles to check</param>
		/// <param name="soilState">The soil state to check for</param>
		/// <returns>A list of all the tiles within the inputted list that have the specified soil state</returns>
		public List<Tile> GetTilesOfSoilState (List<Tile> tiles, SoilState soilState)
		{
			return tiles.Where(tile => tile.SoilState == soilState).ToList( );
		}

		/// <summary>
		/// Get a list of tiles that do not have a certain soil state
		/// </summary>
		/// <param name="tiles">The list of tiles to check</param>
		/// <param name="soilState">The soil state the returned tiles cannot be</param>
		/// <returns>A list of all the tiles within the inputted list that do not have the specified soil state</returns>
		public List<Tile> GetTilesNotOfSoilState (List<Tile> tiles, SoilState soilState)
		{
			return tiles.Where(tile => tile.SoilState != soilState).ToList( );
		}

		/// <summary>
		/// Get a list of tiles that do not have a certain soil state and are empty
		/// </summary>
		/// <param name="tiles">The list of tiles to check</param>
		/// <param name="soilState">THe soil state the returned tiles cannot have</param>
		/// <returns>A list of all the tiles within the inputted list that do not have the specified soil state and are empty</returns>
		public List<Tile> GetEmptyTilesNotOfSoilState (List<Tile> tiles, SoilState soilState)
		{
			return tiles.Where(tile => tile.SoilState != soilState && !tile.HasPlant).ToList( );
		}

		/// <summary>
		/// Update the spread of weeds throughout the garden
		/// </summary>
		public void UpdateWeedSpread ( )
		{
			List<Tile> spreadableTiles = GetWeedSpreadableTiles( );
			for (int i = 0; i < spreadableTiles.Count; i++)
			{
				// Roll for a random chance to spread weeds
				if (Random.Range(0f, 1f) > WeedSpreadPercentage)
				{
					continue;
				}

				// The weed spread was successful
				spreadableTiles[i].SoilState = SoilState.WEEDS;
			}
		}

		/// <summary>
		/// Get a list of all the tiles that weeds can currently spread to
		/// </summary>
		/// <returns>A list of all the tiles that weeds can currently spread to</returns>
		private List<Tile> GetWeedSpreadableTiles ( )
		{
			// Spreadable tiles are all tiles that are on the edge of the garden or are directly surrounding a current weed tile
			List<Tile> possibleTiles = new List<Tile>( );

			// Add all edge tiles
			possibleTiles.AddRange(Tiles.Where(tile => tile.IsAtGardenEdge));

			// Add all tiles that surround weed tiles
			GetTilesOfSoilState(Tiles, SoilState.WEEDS).ForEach(grassTile => possibleTiles.AddRange(GetSurroundingTiles(grassTile)));

			// Filter out all current weed tiles and non-empty tiles
			return GetEmptyTilesNotOfSoilState(possibleTiles, SoilState.WEEDS).Distinct( ).ToList( );
		}

		/// <summary>
		/// Check for and update all mutations for plants within the garden (Not Fully Implemented)
		/// </summary>
		public void UpdatePlantMutations ( )
		{
			for (int x = 0; x < GardenWidth - 1; x++)
			{
				for (int y = 0; y < GardenHeight - 1; y++)
				{
					UpdatePlantMutationCombinations(GetTilesInSection(x, y, 2, 2));
				}
			}
		}

		/// <summary>
		/// Update and perform mutations between the plants on a specific section of the garden
		/// </summary>
		/// <param name="tiles">The section of tiles to perform the mutation on</param>
		private void UpdatePlantMutationCombinations (List<Tile> tiles)
		{
			List<Tile> emptyTiles = GetEmptyTiles(tiles);
			List<Plant> plants = GetPlantsOnTiles(tiles);

			// Loop through every combination of plant in the section
			for (int i = 0; i < plants.Count; i++)
			{
				for (int j = 0; j < plants.Count; j++)
				{
					// If there are no more empty tiles within the checked section, then return as no more mutations can occur
					if (emptyTiles.Count == 0)
					{
						return;
					}

					// Do not try to mutate the same plant against itself
					if (i == j)
					{
						continue;
					}

					// Roll for a random chance to mutate
					if (Random.Range(0f, 1f) > PlantMutationPercentage)
					{
						continue;
					}

					// Mutation was successful, add plant to an empty tile
					// Plant mutationPlant = mutationDictionary[plants[i].Type, plants[j].Type];
					// int emptyTileIndex = Random.Range(0, emptyTiles.Count);
					// AddPlantToTile(mutationPlant, emptyTiles[emptyTileIndex]);
					// emptyTiles.RemoveAt(emptyTileIndex);
				}
			}
		}

		/// <summary>
		/// Tick all plants currently on the garden
		/// </summary>
		public void TickAllPlants ( )
		{
			foreach (Plant plant in Plants)
			{
				plant.Upkeep( );
			}
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

					_garden[i, j] = tile;
					Tiles.Add(tile);
				}
			}

			// Randomly till a certain percentage of tiles at the start of the game
			List<Tile> shuffledTileList = Tiles.OrderBy(x => Random.value).ToList( );
			int tilledSoilCount = Mathf.CeilToInt(GardenArea * startTilledPercentage);
			for (int i = 0; i < Tiles.Count; i++)
			{
				shuffledTileList[i].SoilState = (i < tilledSoilCount ? SoilState.TILLED : SoilState.WEEDS);
			}
		}
	}
}
