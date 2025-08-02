using System;
using System.Collections.Generic;
using System.Linq;
using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
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
        [SerializeField] private Month startingMonth;
        [SerializeField] private Dictionary<Month, Season> monthSeasonMap;
        
        [Header("Inspector")]
        [SerializeField] private Month _currentMonth;
        [SerializeField] private Season _currentSeason;

        #region Properties
        
        public Month CurrentMonth => _currentMonth;
        public Season CurrentSeason => _currentSeason;
        
        #endregion
        

        private const int MONTHS_IN_SEASON = 3;
        private EventBinding<TurnEnded> _turnEndedEventBinding;


        private void Start()
        {
            //make sure dictionary is valid
            ValidateMonthSeasonMap();
            
            _currentMonth = startingMonth;
            _currentSeason = monthSeasonMap[_currentMonth];
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

        private void OnValidate()
        {
            ValidateMonthSeasonMap();
        }


        /// <summary>
        /// Validates the Dictionary based on needing to include each month.
        /// Remakes it based on the MONTHS_IN_SEASON and assigns a season to each month
        /// Dictionary can be edited after to 
        /// </summary>
        private void ValidateMonthSeasonMap()
        {
            //get months in a year
            List<Month> months = Enum.GetValues(typeof(Month)).Cast<Month>().ToList();
            
            //if count of dict is equal, no need to change anything
            if (monthSeasonMap.Count == months.Count ) return;
            
            monthSeasonMap = new Dictionary<Month, Season>();

            //get the total number of seasons
            int totalSeasons = Enum.GetValues(typeof(Season)).Length;

            //loop through months and add month - season map according to MONTHS_IN_SEASON
            for (int i = 0; i < months.Count; i++)
            {
                //get the season that should be associated with the month 
                int seasonIndex = Math.Min(i / MONTHS_IN_SEASON, totalSeasons - 1);
                
                monthSeasonMap.Add(months[i], (Season)seasonIndex);
            }
        }
        /// <summary>
        /// Moves onto the next month
        /// </summary>
        private void AdvanceMonth()
        {
            //increment month, get the length of the enum to allow dynamic number of months
            _currentMonth = (Month)(((int)_currentMonth + 1) % Enum.GetValues(typeof(Month)).Length);
            
            //check season 
            _currentSeason = monthSeasonMap[_currentMonth];
        }
        
    }
}
