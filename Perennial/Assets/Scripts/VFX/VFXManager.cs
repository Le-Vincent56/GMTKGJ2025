using System;
using System.Collections.Generic;
using Perennial.Core.Extensions;
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
        Carrot = 5, // Technically not used
        //Scorched = 6
    }
    
    public class VFXManager : MonoBehaviour
    {
        private static VFXManager _instance;
        public static VFXManager Instance
        {
            get
            {
                if (_instance)
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
                var go = new GameObject("Empty VFX Manager");
                _instance = go.AddComponent<VFXManager>();
                return _instance;
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
        public void AddVFX(Tile tile, VFXType vfx, SerializableGuid id, bool isAttachedToPlant = false)
        {
            // Don't add if plant was marked for removal anyway.
            if (isAttachedToPlant && tile.Plant.MarkedForRemoval) return;
            
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
                vfxCount.ids.Add(id);
                if (vfxCount.vfx != null)
                {
                    if (!vfxCount.vfx.isPlaying)
                    {
                        vfxCount.vfx.Play();
                    }
                }
                else
                {
                    ParticleSystem newVFX;
                    if(!vfxPlantList.TryGetValue(vfx, out newVFX)) return;
                    
                    if (newVFX != null)
                    {
                        vfxCount.vfx = Instantiate(newVFX, tile.transform.position + Vector3.back, Quaternion.Euler(-90, 0, 0));
                    }
                }
            }
            // Otherwise, create a new entry
            else
            {
                TileVFX.VfxCount newVfxCount = new TileVFX.VfxCount();
                newVfxCount.ids.Add(id);
                
                ParticleSystem newVFX;
                if(!vfxPlantList.TryGetValue(vfx, out newVFX)) return;
                
                if (newVFX != null)
                {
                    newVfxCount.vfx = Instantiate(newVFX, tile.transform.position + Vector3.back, Quaternion.Euler(-90, 0, 0));
                }

                newVfxCount.isAttachedToPlant = isAttachedToPlant;
                tileVfx.vfxOnTile.Add(vfx, newVfxCount);
            }
        }
        
        /// <summary>
        /// Remove a VFX
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="vfx"></param>
        public void RemoveVFX(Tile tile, VFXType vfx, SerializableGuid id)
        {
            // Check if the current tile exists in the dictionary,
            if (!tileVFXList.TryGetValue(tile, out TileVFX tileVfx))
            {
                return;
            }
            
            // Check to see if dictionary already has an entry for the vfx
            if(tileVfx.vfxOnTile.TryGetValue(vfx, out TileVFX.VfxCount vfxCount))
            {
                if (vfxCount.ids.Remove(id) && vfxCount.ids.Count <= 0)
                {
                    vfxCount.vfx.Stop();
                    vfxCount.vfx = null;
                }
            }
        }

        public void RemovePlantVFX(Tile tile)
        {
            // Check if the current tile exists in the dictionary,
            if (!tileVFXList.TryGetValue(tile, out TileVFX tileVfx))
            {
                return;
            }

            LinkedList<VFXType> keys = new LinkedList<VFXType>();
            
            // Check to see if dictionary already has an entry for the vfx
            foreach (var currVFX in tileVfx.vfxOnTile)
            {
                if (currVFX.Value.isAttachedToPlant)
                {
                    keys.AddLast(currVFX.Key);
                }
            }

            foreach (var key in keys)
            {
                tileVfx.vfxOnTile.Remove(key);
            }
        }

        public void SetWeatherParticles(Season season)
        {
            switch (season)
            {
                case Season.Spring:
                    vfxWeatherList[Season.Spring].Play();
                    vfxWeatherList[Season.Fall].Stop();
                    vfxWeatherList[Season.Winter].Stop();
                    break;
                
                case Season.Summer:
                    vfxWeatherList[Season.Spring].Stop();
                    vfxWeatherList[Season.Fall].Stop();
                    vfxWeatherList[Season.Winter].Stop();
                    break;
                
                case Season.Fall:
                    vfxWeatherList[Season.Spring].Stop();
                    vfxWeatherList[Season.Fall].Play();
                    vfxWeatherList[Season.Winter].Stop();
                    break;
                
                case Season.Winter:
                    vfxWeatherList[Season.Spring].Stop();
                    vfxWeatherList[Season.Fall].Stop();
                    vfxWeatherList[Season.Winter].Play();
                    break;
            }
        }

        private class TileVFX
        {
            public Dictionary<VFXType, VfxCount> vfxOnTile;

            public struct VfxCount
            {
                public ParticleSystem vfx;
                public HashSet<SerializableGuid> ids;
                public bool isAttachedToPlant = false;

                public VfxCount()
                {
                    ids = new HashSet<SerializableGuid>();
                    vfx = null;
                }
            }

            public TileVFX()
            {
                vfxOnTile = new Dictionary<VFXType, VfxCount>();
            }
        }
    }

    
}
