using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI
{
    public class CLSoft
    {
        private readonly IEnumerable<Object> _dataContainer;
        private readonly CommandFactory _commandFactory;
        public CLSoft(IEnumerable<Object> data = null)
        {
            _commandFactory = new CommandFactory();
            _dataContainer = data;
        }

        public void ReadAndProcess(int noCommands=int.MaxValue)
        {
            for(int k=0; k<noCommands; k++)
            {
                string line = Console.ReadLine();
                line = line.Trim();
                string[] args = line.Split(' ');

                ICommand command = _commandFactory.CreateCommand(args, _dataContainer);
                command.Execute();
            }
        }
    }

    public class CommandFactory
    {
        private readonly IEnumerable<ICommand> _commands = new List<ICommand>() { new CmdList(), new CmdExit() };

        public ICommand CreateCommand(string[] args, IEnumerable<Object> data)
        {
            ICommand command = _commands.FirstOrDefault(c => c.CommandName.Equals(args[0]));

            if (command == null || !command.SetArgs(args.Skip(1).ToArray(),data))
                command = new NotFoundCommand(args[0]);
                
            return command;
        }
    }

    public interface ICommand
    {
        string CommandName { get; }
        bool SetArgs(string[] args = null, IEnumerable<Object> data = null);
        void Execute();
    }

    public class NotFoundCommand : ICommand
    {
        public string CommandName { get; set; }
        public NotFoundCommand(string commandName)
        {
            CommandName = commandName;
        }
        public bool SetArgs(string[] args = null, IEnumerable<Object> data = null) => false;
        public void Execute()
        {
            Console.WriteLine("Couldn't find command: " + CommandName);
        }
    }

    public class CmdList : ICommand
    {
        public string CommandName { get; set; }

        private string className;
        private IEnumerable<Object> _dataContainer;
        public CmdList()
        { CommandName = "list"; }
        public bool SetArgs(string[] args = null, IEnumerable<Object> data = null)
        {
            if (args == null || args.Length != 1 || data == null) return false;
            className = args[0];
            _dataContainer = data;
            return true; 
        }

        public void Execute()
        {
            Console.WriteLine();
            foreach(Object obj in _dataContainer)
            {
                if(obj.GetType().Name.Equals(className) || className.Equals("-a"))
                {
                    Console.WriteLine( obj.ToString());
                }
            }
            Console.WriteLine();
        }
    }

    public class CmdExit : ICommand
    {
        public string CommandName { get; set; }
        
        public CmdExit()
        {
            CommandName = "exit";
        }
        public bool SetArgs(string[] args = null, IEnumerable<Object> data = null) => false;

        public void Execute()
        {
            Environment.Exit(0);
        }
    }
}
