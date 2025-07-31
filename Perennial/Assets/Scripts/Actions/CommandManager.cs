using System.Collections.Generic;
using System.Threading.Tasks;
using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using Sirenix.OdinInspector;

namespace Perennial.Actions
{
    public class CommandManager : SerializedMonoBehaviour
    {

        private Queue<ICommand> _commandsQueue;
        private EventBinding<PerformCommand> _performCommandEventBinding;
        private bool _isRunning; 
        
        private void OnEnable()
        {
            _performCommandEventBinding = new EventBinding<PerformCommand>((data) =>
            {
                _commandsQueue.Enqueue(data.Command);
                if (!_isRunning) ExecuteCommands(); // no need to await
            });
            
            EventBus<PerformCommand>.Register(_performCommandEventBinding);
        }

        private void OnDisable()
        {
            EventBus<PerformCommand>.Deregister(_performCommandEventBinding);
        }
        

        /// <summary>
        /// Executes the commands in order of oldest to newest.
        /// </summary>
        private  async Task ExecuteCommands()
        {
            _isRunning = true;
            while (_commandsQueue.Count > 0)
            {
                ICommand command = _commandsQueue.Dequeue();
                await command.Execute();
            }
            _isRunning = false;
        }
        
    }
}
