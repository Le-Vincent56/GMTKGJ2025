using System;
using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using Perennial.Garden;
using Perennial.Plants.UI;
using UnityEngine;

namespace Perennial
{
    public class CheckLoseScriptScuffed : MonoBehaviour
    {
        private GardenManager _gardenManager;
        private PlantStorageController _plantStorageController;

        private EventBinding<ActionsStart> _turnEnded;
        void Start()
        {
            _gardenManager = FindFirstObjectByType<GardenManager>();
            _plantStorageController = FindFirstObjectByType<PlantStorageController>();
        }

        private void OnEnable()
        {
            _turnEnded = new EventBinding<ActionsStart>(CheckLost);
            EventBus<ActionsStart>.Register(_turnEnded);
        }

        private void OnDisable()
        {
            EventBus<ActionsStart>.Deregister(_turnEnded);
        }

        private void CheckLost()
        {
            bool check = true;
            foreach (Tile tile in _gardenManager.Tiles)
            {
                if (tile.HasPlant) check = false;
            }
            if (_plantStorageController.Model.IsStorageEmpty() && check)
            {
                EventBus<LoseGameEvent>.Raise(new LoseGameEvent());
            }
        }
    }
}
