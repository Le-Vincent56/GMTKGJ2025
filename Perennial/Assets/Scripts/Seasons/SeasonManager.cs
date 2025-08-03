using System;
using System.Collections.Generic;
using System.Linq;
using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using Perennial.VFX;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Perennial.Seasons
{
    public enum Month
    {
        Mar,
        Apr,
        May,
        Jun,
        Jul, 
        Aug,
        Sept,
        Oct,
        Nov,
        Dec,
        Jan,
        Feb,
    }

    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter,
    }

    public class SeasonManager : SerializedMonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private SeasonStartingData seasonStartingData;

        private SeasonModel _model;
        private SeasonView _view;

        #region Properties

        public Month CurrentMonth => _model.CurrentMonth;
        public Season CurrentSeason => _model.CurrentSeason;

        #endregion


        private EventBinding<TurnEnded> _turnEndedEventBinding;


        private void Awake()
        {
            _model = new SeasonModel(seasonStartingData);
            _view = GetComponent<SeasonView>();

            ConnectModel();
            ConnectView();
        }

        private void OnEnable()
        {
            _turnEndedEventBinding = new EventBinding<TurnEnded>(AdvanceMonth);
            EventBus<TurnEnded>.Register(_turnEndedEventBinding);
        }

        private void OnDisable()
        {
            EventBus<TurnEnded>.Deregister(_turnEndedEventBinding);
        }
        
        private void ConnectModel()
        {
            _model.OnModified += _view.UpdateUI;
        }

        private void ConnectView()
        {
            _view.Initialize(CurrentMonth);
        }

        private void AdvanceMonth()
        {
            _model.AdvanceMonth();
        }
    }
}
