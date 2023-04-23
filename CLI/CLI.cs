using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI
{
    public class CLSoft
    {
        private CommandFactory _commandFactory;
        public CLSoft()
        {
            _commandFactory = new CommandFactory();
        }

        public void ReadAndProcess(int noCommands=int.MaxValue)
        {
            for(int k=0; k<noCommands; k++)
            {
                string line = Console.ReadLine();
                line = line.Trim();
                string[] args = line.Split(' ');

                ICommand command = _commandFactory.CreateCommand(args);
                command.Execute();
            }
        }
    }

    public class CommandFactory
    {
        private IEnumerable<ICommand> _commands;

        public ICommand CreateCommand(string[] args)
        {
            ICommand command = _commands.FirstOrDefault(c => c.CommandName.Equals(args[0]));

            if (command != null || !command.SetArgs(args.Skip(1).ToArray()))
                command = new NotFoundCommand(args[0]);
                
            return command;
        }
    }

    public interface ICommand
    {
        string CommandName { get; }
        bool SetArgs(string[] args = null);
        void Execute();
    }

    public class NotFoundCommand : ICommand
    {
        public string CommandName { get; set; }
        public NotFoundCommand(string commandName)
        {
            CommandName = commandName;
        }
        public bool SetArgs(string[] args = null) => false;
        public void Execute()
        {
            Console.WriteLine("Couldn't find command: " + CommandName);
        }
    }
}
