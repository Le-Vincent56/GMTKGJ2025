using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Perennial.Plants.UI
{
    public class PlantStorageView : MonoBehaviour
    {
        private List<PlantButton>  _plantButtons = new List<PlantButton>();

        public void Initialize(PlantStorageModel model)
        {
            _plantButtons = GetComponentsInChildren<PlantButton>().ToList();
        }
    }
}
