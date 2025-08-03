using UnityEngine;

namespace Perennial.Plants.Stats
{
    [CreateAssetMenu(fileName = "Plant Base Stats", menuName = "Plants/Base Stats")]
    public class PlantBaseStats : ScriptableObject
    {
        public float FoodModifier = 1f;
        [Range(0f, 1f)]public float MutationChance;
        public float MutationDropRate = 1f;
        public int SeedsMinimum = 1;
        public int SeedsMaximum = 3;
    }
}
