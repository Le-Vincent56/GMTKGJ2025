using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using Sirenix.OdinInspector;

namespace Perennial.Actions
{
    public class CommandManager : SerializedMonoBehaviour
    {
        private EventBinding<PerformCommand> _performCommandEventBinding;

        private void OnEnable()
        {
            _performCommandEventBinding = new EventBinding<PerformCommand>((data) =>
            {
                ExecuteCommand(data.Command);
            });
            
            EventBus<PerformCommand>.Register(_performCommandEventBinding);
        }

        private void OnDisable()
        {
            EventBus<PerformCommand>.Deregister(_performCommandEventBinding);
        }

        /// <summary>
        /// Executes a given command. 
        /// </summary>
        /// <param name="command">ICommand to call its Execute function</param>
        private void ExecuteCommand(ICommand command)
        {
            command.Execute();
        }
        
    }
}
