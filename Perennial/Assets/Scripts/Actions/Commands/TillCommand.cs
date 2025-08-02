using System.Threading.Tasks;
using Perennial.Garden;
using UnityEngine;

namespace Perennial.Actions.Commands
{
    public class TillCommand : BaseCommand
    {
        public TillCommand(TillArgs input) : base(input)
        {

        }
        
        /// <summary>
        /// Executes the Till action
        /// </summary>
        public override async Task Execute()
        {
            Tile.SoilState = SoilState.TILLED;
        }
    }
}
