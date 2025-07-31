using System.Collections.Generic;
using Perennial.Core.Extensions;
using Perennial.Plants.Abilities;
using Perennial.Plants.Behaviors;
using UnityEngine;

namespace Perennial.Plants
{
    [CreateAssetMenu(fileName = "Plant Definition", menuName = "Plants/Definition")]
    public class PlantDefinition : ScriptableObject
    {
        [Header("Details")]
        [SerializeField] protected SerializableGuid id;
        [SerializeField] protected string plantName;
        [SerializeField] [TextArea(3,5)] protected string description;
        [SerializeField] protected int lifetime;
        [SerializeField] protected Sprite sprite;

        [Header("Abilities")] 
        [SerializeField] protected PlantAbility[] abilities;

        [Header("Behaviors")] 
        [SerializeField] private List<PlantBehavior> behaviors;

        public SerializableGuid ID => id;
        public string Name => plantName;
        public string Description => description;
        public int Lifetime => lifetime;
        public Sprite Sprite => sprite;
        public PlantAbility[] Abilities => abilities;
        public List<PlantBehavior> Behaviors => behaviors;

        /// <summary>
        /// Create an instance of a Plant using this definition
        /// </summary>
        public IPlant CreateInstance() => PlantFactory.CreatePlant(this);
        
        /// <summary>
        /// Generate an unique ID on validation
        /// </summary>
        private void OnValidate() => id = SerializableGuid.NewGuid();
    }
}
