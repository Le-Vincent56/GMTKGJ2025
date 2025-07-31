using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Perennial.Actions
{
    public class CommandManager : SerializedMonoBehaviour
    {
        public ICommand SingleCommand;
        public List<ICommand> Commands;

        private void Start()
        {
            //SingleCommand = 
        }
    }
}
