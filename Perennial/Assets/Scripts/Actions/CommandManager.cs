using System.Collections.Generic;
using System.Threading.Tasks;
using Perennial.Actions.Commands;
using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;

namespace Perennial.Actions
{
    public class CommandManager : MonoBehaviour
    {

        private Queue<ICommand> _commandsQueue;
        private EventBinding<PerformCommand> _performCommandEventBinding;
        private bool _isRunning;

        private void Start()
        {
            _commandsQueue = new Queue<ICommand>();
        }

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
                EventBus<CommandFinished>.Raise(new CommandFinished());
            }
            _isRunning = false;
        }

        /// <summary>
        /// Enqueue a command and startup execution command if not running
        /// </summary>
        /// <param name="command">Command to add to the Queue</param>
        private async void EnqueueNewCommand(ICommand command)
        {
            _commandsQueue.Enqueue(command);
            if (!_isRunning) await ExecuteCommands(); // no need to await
        }
        
    }
}
