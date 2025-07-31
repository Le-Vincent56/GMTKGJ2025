using UnityEngine;

namespace Perennial.Actions.Commands
{
    public class TillCommand : BaseCommand
    {
        /// <summary>
        /// Executes the Till action
        /// </summary>
        public override void Execute()
        {
            Debug.Log("Tilling ground");
        }
    }
}
