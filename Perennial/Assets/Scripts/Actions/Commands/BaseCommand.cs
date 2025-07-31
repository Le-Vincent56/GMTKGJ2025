namespace Perennial.Actions.Commands
{
    public abstract class BaseCommand : ICommand
    {
        protected BaseCommand()
        {
            
        }

        /// <summary>
        /// Execute the Command
        /// </summary>
        public abstract void Execute();

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
