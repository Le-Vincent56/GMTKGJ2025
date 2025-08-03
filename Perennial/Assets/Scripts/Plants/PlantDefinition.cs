using System.Collections.Generic;
using Perennial.Core.Extensions;
using Perennial.Plants.Abilities;
using Perennial.Plants.Behaviors;
using Perennial.Plants.Stats;
using Perennial.Seasons;
using UnityEngine;

namespace Perennial.Plants
{
    [CreateAssetMenu(fileName = "Plant Definition", menuName = "Plants/Definition")]
    public class PlantDefinition : ScriptableObject
    {
        [Header("Details")]
        [SerializeField] private SerializableGuid id;
        [SerializeField] private string plantName;
        [SerializeField] [TextArea(3,5)] private string description;
        [SerializeField] private PlantBaseStats baseStats;
        [SerializeField] private Sprite seedSprite;
        [SerializeField] private Sprite youngSprite;
        [SerializeField] private Sprite oldSprite;
        
        [Header("Lifetime")]
        [SerializeField] private int growthTime;
        [SerializeField] private int harvestTime;
        [SerializeField] private List<Season> incompatibleSeasons;
        [SerializeField] private List<Season> bonusSeasons;

        [Header("Abilities")] 
        [SerializeField] private PlantAbility[] abilities;

        [Header("Behaviors")] 
        [SerializeField] private List<PlantBehavior> behaviors;
        
        [Header("Resources")]
        [SerializeField] private int foodMultiplier;
        [SerializeField] private int foodConstant;

        public SerializableGuid ID => id;
        public string Name => plantName;
        public string Description => description;
        public int GrowTime => growthTime;
        public int HarvestTime => harvestTime;
        public List<Season> IncompatibleSeasons => incompatibleSeasons;
        public List<Season> BonusSeasons => bonusSeasons;
        public Sprite SeedSprite => seedSprite;
        public Sprite YoungSprite => youngSprite;
        public Sprite OldSprite => oldSprite;
        public PlantAbility[] Abilities => abilities;
        public List<PlantBehavior> Behaviors => behaviors;
        public PlantBaseStats BaseStats => baseStats;
        public int FoodMultiplier => foodMultiplier;
        public int FoodConstant => foodConstant;

        /// <summary>
        /// Create an instance of a Plant using this definition
        /// </summary>
        public IPlant CreateInstance() => PlantFactory.CreatePlant(this);

        /// <summary>
        /// Generate an unique ID on validation
        /// </summary>
        private void OnValidate()
        {
            // Exit if the ID already exists
            if (id != SerializableGuid.Empty) return;
            
            id = SerializableGuid.NewGuid();
        }
    }
}
