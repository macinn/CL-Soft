using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI
{
    public class CLSoft
    {
        private readonly CommandFactory _commandFactory;
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
        private readonly IEnumerable<ICommand> _commands = new List<ICommand>() { new CmdList(), new CmdExit() };

        public ICommand CreateCommand(string[] args)
        {
            ICommand command = _commands.FirstOrDefault(c => c.CommandName.Equals(args[0]));

            if (command != null || (args.Length>1 && !command.SetArgs(args.Skip(1).ToArray())))
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

    public class CmdList : ICommand
    {
        public string CommandName { get; set; }

        private string className;
        public CmdList()
        { CommandName = "list"; }
        public bool SetArgs(string[] args = null)
        {
            if (args == null || args.Length != 1) return false;
            className = args[0];
            return true; 
        }

        public void Execute()
        {
            if (className == null) return;
            Console.WriteLine();
        }
    }

    public class CmdExit : ICommand
    {
        public string CommandName { get; set; }

        public CmdExit()
        { CommandName = "exit"; }
        public bool SetArgs(string[] args = null) => false;

        public void Execute()
        {
            Environment.Exit(0);
        }
    }
}
