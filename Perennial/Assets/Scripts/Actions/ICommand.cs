using System.Threading.Tasks;
using UnityEngine;

namespace Perennial.Actions
{
    public interface ICommand
    {
        /// <summary>
        /// Executes a given command
        /// </summary>
        Task Execute();
    }
}
