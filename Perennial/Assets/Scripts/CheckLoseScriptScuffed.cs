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

        private EventBinding<TurnEnded> _turnEnded;
        void Start()
        {
            _gardenManager = FindFirstObjectByType<GardenManager>();
            _plantStorageController = FindFirstObjectByType<PlantStorageController>();
        }

        private void OnEnable()
        {
            _turnEnded = new EventBinding<TurnEnded>(CheckLost);
            EventBus<TurnEnded>.Register(_turnEnded);
        }

        private void OnDisable()
        {
            EventBus<TurnEnded>.Deregister(_turnEnded);
        }

        private void CheckLost()
        {
            
        }
    }
}
