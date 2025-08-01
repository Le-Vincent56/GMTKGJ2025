using System.Threading.Tasks;

namespace Perennial.Actions.Commands
{
    public abstract class BaseCommand : ICommand
    {
        //TODO Put needed references here to pass to children. Like GardenManager
        protected BaseCommand()
        {
            //TODO Put needed references here to pass to children. Like GardenManager
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
