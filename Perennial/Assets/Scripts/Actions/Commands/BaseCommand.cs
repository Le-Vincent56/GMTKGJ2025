using System.Threading.Tasks;
using Perennial.Garden;

namespace Perennial.Actions.Commands
{
    public abstract class BaseCommand : ICommand
    {
        private readonly GardenManager _gardenManager;
        protected BaseCommand(GardenManager gardenManager)
        {
            _gardenManager = gardenManager;
        }

        /// <summary>
        /// Execute the Command
        /// </summary>
        public abstract Task Execute();

        /// <summary>
        /// Static factory method to create a command
        /// </summary>
        /// <typeparam name="T">Type of command</typeparam>
        /// <returns>Instance of ICommand of type T</returns>
        public static T Create<T>() where T : BaseCommand
        {
            return (T) System.Activator.CreateInstance(typeof(T));
        }
    }
}
