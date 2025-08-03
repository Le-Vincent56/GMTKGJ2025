using System;
using System.Collections.Generic;
using Perennial.VFX;
using UnityEngine;

namespace Perennial.Seasons
{
    public class SeasonModel
    {
        private Month _currentMonth;
        private Season _currentSeason;
        private Dictionary<Month, Season> _monthSeasonMap;

        public Season CurrentSeason => _currentSeason;
        public Month CurrentMonth => _currentMonth;
        
        public event Action<Month> OnModified = delegate { };

        public SeasonModel(SeasonStartingData seasonStartingData)
        {
            _currentMonth = seasonStartingData.StartingMonth;
            _monthSeasonMap = seasonStartingData.MonthSeasonMap;
            _currentSeason = _monthSeasonMap[_currentMonth];
            VFXManager.Instance.SetWeatherParticles(_currentSeason);
        }
        
        /// <summary>
        /// Moves onto the next month
        /// </summary>
        public void AdvanceMonth()
        {
            //increment month, get the length of the enum to allow dynamic number of months
            _currentMonth = (Month)(((int)_currentMonth + 1) % Enum.GetValues(typeof(Month)).Length);
            
            //check season 
            _currentSeason = _monthSeasonMap[_currentMonth];

            OnModified?.Invoke(_currentMonth);
            VFXManager.Instance.SetWeatherParticles(_currentSeason);
        }
    }
}
