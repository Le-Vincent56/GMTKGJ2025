using System;
using UnityEngine;

namespace Perennial.Plants.Mutations
{
    [Serializable]
    public class MutationRule
    {
        [SerializeField] private PlantDefinition parent1;
        [SerializeField] private PlantDefinition parent2;
        [SerializeField] private PlantDefinition result;
        
        public PlantDefinition Parent1 => parent1;
        public PlantDefinition Parent2 => parent2;
        public PlantDefinition Result => result;

        public bool Matches(PlantDefinition plant1, PlantDefinition plant2)
        {
            return (parent1 == plant1 && parent2 == plant2) || (parent1 == plant2 && parent2 == plant1);
        }
    }
}
