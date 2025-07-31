using Perennial.Plants;
using System.Collections.Generic;
using UnityEngine;

namespace Perennial
{
	public class Garden : MonoBehaviour
	{
		[SerializeField, Range(1, 20)] private int _gardenWidth = 1;
		[SerializeField, Range(1, 20)] private int _gardenHeight = 1;

		private Tile[ , ] tiles;

		/// <summary>
		/// The width of the garden in tiles
		/// </summary>
		public int GardenWidth => _gardenWidth;

		/// <summary>
		/// The height of the garden in tiles
		/// </summary>
		public int GardenHeight => _gardenHeight;

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
				return tiles[x, y];
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
		/// Get all of the surrounding tiles around a position within a given radius. Surrounding tiles includes all cardinal directions as well as all four diagonal directions
		/// </summary>
		/// <param name="x">The x position to get the surrounding tiles of</param>
		/// <param name="y">The y position to get the surrounding tiles of</param>
		/// <param name="radius">The radius around the specified to get the surrounding tiles of</param>
		/// <returns>A list of surrounding tiles to the specified position, excluding the tile at the specified position. There will be no null values in this list</returns>
		public List<Tile> GetSurroundingTiles (int x, int y, int radius = 1)
		{
			List<Tile> surroundingTiles = new List<Tile>( );

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
		/// <param name="radius">The radius around the specified to get the surrounding plants of</param>
		/// <returns>A list of surrounding plants to the specified position, excluding the plant on the tile at the specified position. There will be no null values in this list</returns>
		public List<Plant> GetSurroundingPlants (int x, int y, int radius = 1)
		{
			List<Plant> surroundingPlants = new List<Plant>( );
			List<Tile> surroundingTiles = GetSurroundingTiles(x, y, radius: radius);

			Plant plant;
			for (int i = 0; i < surroundingTiles.Count; i++)
			{
				plant = surroundingTiles[i].Plant;

				if (plant != null)
				{
					surroundingPlants.Add(plant);
				}
			}

			return surroundingPlants;
		}
	
		
	}
}
