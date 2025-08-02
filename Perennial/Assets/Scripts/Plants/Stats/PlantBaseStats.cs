using UnityEngine;

namespace Perennial.Plants.Stats
{
    [CreateAssetMenu(fileName = "Plant Base Stats", menuName = "Plants/Base Stats")]
    public class PlantBaseStats : ScriptableObject
    {
        public float FoodModifier;
        public float MutationChance;
        public float MutationDropRate;
    }
}
