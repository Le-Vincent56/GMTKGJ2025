using System;
using System.Collections.Generic;
using Perennial.Garden;
using Perennial.Seasons;
using UnityEngine;
using UnityEngine.Rendering;

namespace Perennial.VFX
{
    public enum VFXType
    {
        Fire = 0,
        Snow = 1,
        Ivy = 2,
        Pearl = 3,
        Garlic = 4, // Technically not used
        Carrot = 5  // Technically not used
    }
    public class VFXManager : MonoBehaviour
    {
        private static VFXManager _instance;
        public static VFXManager Instance
        {
            get
            {
                if (_instance?.isActiveAndEnabled != null)
                {
                    return _instance;
                }
                else
                {
                    _instance = GameObject.FindFirstObjectByType<VFXManager>();
                    if (_instance != null)
                    {
                        return _instance;
                    }
                }

                Debug.LogError("Tried to call VFXManager while not VFXManager was in the scene!");
                return null;
            }
        }

        [SerializeField] private SerializedDictionary<VFXType, ParticleSystem> vfxPlantList;
        [SerializeField] private SerializedDictionary<Season, ParticleSystem> vfxWeatherList;
        [SerializeField] private SerializedDictionary<Tile, TileVFX> tileVFXList;

        private void Awake()
        {
            tileVFXList = new SerializedDictionary<Tile, TileVFX>();
        }

        /// <summary>
        /// Add a new tracked VFX to the manager's list. Based on Tile position and enum.
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="vfx"></param>
        public void AddVFX(Tile tile, VFXType vfx)
        {
            // Check if the current tile exists in the dictionary,
            if (!tileVFXList.TryGetValue(tile, out TileVFX tileVfx))
            {
                // and make a new entry if it doesn't exist.
                tileVfx = new TileVFX();
                tileVFXList.Add(tile, tileVfx);
            }
            
            // Check to see if dictionary already has an entry for the vfx
            if(tileVfx.vfxOnTile.TryGetValue(vfx, out TileVFX.VfxCount vfxCount))
            {
                vfxCount.count++;
                if (vfxCount.vfx != null)
                {
                    if (!vfxCount.vfx.isPlaying)
                    {
                        vfxCount.vfx.Play();
                    }
                }
                else
                {
                    var newVFX = vfxPlantList[vfx];
                    if (newVFX != null)
                    {
                        vfxCount.vfx = Instantiate(newVFX, tile.transform.position + Vector3.back, Quaternion.identity);
                    }
                }
            }
            // Otherwise, create a new entry
            else
            {
                TileVFX.VfxCount newVfxCount = new TileVFX.VfxCount();
                newVfxCount.count = 1;
                var newVFX = vfxPlantList[vfx];
                if (newVFX != null)
                {
                    newVfxCount.vfx = Instantiate(newVFX, tile.transform.position + Vector3.back, Quaternion.identity);
                }
                tileVfx.vfxOnTile.Add(vfx, newVfxCount);
            }
        }
        
        /// <summary>
        /// Remove a VFX
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="vfx"></param>
        public void RemoveVFX(Tile tile, VFXType vfx)
        {
            // Check if the current tile exists in the dictionary,
            if (!tileVFXList.TryGetValue(tile, out TileVFX tileVfx))
            {
                return;
            }
            
            // Check to see if dictionary already has an entry for the vfx
            if(tileVfx.vfxOnTile.TryGetValue(vfx, out TileVFX.VfxCount vfxCount))
            {
                vfxCount.count--;
                if (vfxCount.count <= 0)
                {
                    vfxCount.count = 0;
                    vfxCount.vfx.Stop();
                    vfxCount.vfx = null;
                }
            }
        }

        private class TileVFX
        {
            public Dictionary<VFXType, VfxCount> vfxOnTile;

            public struct VfxCount
            {
                public ParticleSystem vfx;
                public int count;
            }

            public TileVFX()
            {
                vfxOnTile = new Dictionary<VFXType, VfxCount>();
            }
        }
    }

    
}
