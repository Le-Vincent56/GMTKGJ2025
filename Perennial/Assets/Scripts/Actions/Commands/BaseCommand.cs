using System.Threading.Tasks;
using Perennial.Garden;
using Perennial.Plants;

namespace Perennial.Actions.Commands
{
    
    /// <summary>
    /// Allows type to be inferred for parameters,
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommandArgs<T> where T : ICommand
    {
    }

    public class BaseArgs : ICommandArgs<BaseCommand>
    {
        public GardenManager GardenManager { get; set; }
        public Tile Tile { get; set; }
    }

    public class PlantArgs : BaseArgs, ICommandArgs<PlantCommand>
    { 
        public PlantDefinition PlantDefinition { get; set; }
    }
    
    public class HarvestArgs : BaseArgs, ICommandArgs<HarvestCommand>
    {
    }
    
    public class TillArgs : BaseArgs, ICommandArgs<TillCommand>
    {
    }
    
    
    public abstract class BaseCommand : ICommand
    {
        private readonly GardenManager _gardenManager;
        private readonly Tile _tile;
        
        // storing this is kinda scuffed, but its quick and dirty,
        // I would rather have a way to get the data down to the class that needs it without this approach 
        //with a more dynamic factory
        private readonly PlantDefinition _plantDefinition;

        protected BaseCommand(BaseArgs input)
        {
            _gardenManager = input.GardenManager;
            _tile = input.Tile;
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
        public static T Create<T>(ICommandArgs<T> input) where T : BaseCommand
        {
            return (T) System.Activator.CreateInstance(typeof(T), input);
        }
    }
}
