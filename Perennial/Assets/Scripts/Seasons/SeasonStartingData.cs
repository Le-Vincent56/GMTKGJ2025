using System;
using System.Collections.Generic;
using System.Linq;
using Perennial.Seasons;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Perennial.Seasons
{
    [CreateAssetMenu(fileName = "SeasonStartingData", menuName = "Seasons/SeasonStartingData")]
    public class SeasonStartingData : SerializedScriptableObject
    {
        [Header("Season Settings")] 
        [SerializeField] private Month startingMonth;
        [SerializeField] private Dictionary<Month, Season> monthSeasonMap;
        [SerializeField] private int monthsInSeason;
            
        public Month StartingMonth { get => startingMonth;  }
        public Dictionary<Month, Season> MonthSeasonMap { get => monthSeasonMap; private set => monthSeasonMap = value; }
        public int MonthsInSeason { get => monthsInSeason;  }
        
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
            if (MonthSeasonMap.Count == months.Count ) return;
            
            MonthSeasonMap = new Dictionary<Month, Season>();

            //get the total number of seasons
            int totalSeasons = Enum.GetValues(typeof(Season)).Length;

            //loop through months and add month - season map according to MONTHS_IN_SEASON
            for (int i = 0; i < months.Count; i++)
            {
                //get the season that should be associated with the month 
                int seasonIndex = Math.Min(i / MonthsInSeason, totalSeasons - 1);
                
                MonthSeasonMap.Add(months[i], (Season)seasonIndex);
            }
        }
    
    }
}
