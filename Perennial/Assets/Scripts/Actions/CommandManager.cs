using System.Collections.Generic;
using System.Threading.Tasks;
using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Perennial.Actions
{
    public class CommandManager : SerializedMonoBehaviour
    {

        [SerializeField] private Queue<ICommand> _commandsQueue;
        private EventBinding<PerformCommand> _performCommandEventBinding;
        private bool _isRunning; 
        
        private void OnEnable()
        {
            _performCommandEventBinding = new EventBinding<PerformCommand>((data) =>
            {
               EnqueueNewCommand(data.Command);
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

        /// <summary>
        /// Enqueue a command and startup execution command if not running
        /// </summary>
        /// <param name="command">Command to add to the Queue</param>
        private void EnqueueNewCommand(ICommand command)
        {
            _commandsQueue.Enqueue(command);
            if (!_isRunning) ExecuteCommands(); // no need to await
        }
        
    }
}
