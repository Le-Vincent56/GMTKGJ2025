using System.Collections.Generic;

namespace Perennial.Actions
{
    public class CommandInvoker
    {
        /// <summary>
        /// Executes all commands given in a command list. 
        /// </summary>
        /// <param name="Commands"></param>
        public void ExecuteCommand(List<ICommand> Commands)
        {
            foreach (ICommand command in Commands)
            {
                command.Execute();
            }
        }
    }
}
