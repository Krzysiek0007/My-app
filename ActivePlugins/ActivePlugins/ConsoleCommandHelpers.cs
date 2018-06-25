using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivePlugins
{
    public abstract class ConsoleCommand
    {
        protected abstract void processCommand(string toProcess);

        protected abstract string getSyntaxDescription();

        protected abstract List<string> possibleStarts();

        protected ConsoleCommand(string description)
        {
            Description = description;
        }

        public void Do(string commandText)
        {
            processCommand(commandText);
        }

        public IEnumerable<string> Prefixes
        {
            get { return possibleStarts(); }
        }
        public string Description { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} \tSyntax: {1}", Description, getSyntaxDescription());
        }
    }

    public sealed class SimpleCommand : ConsoleCommand
    {
        private readonly List<string> commands = new List<string>();
        private readonly Action<string> toDo;

        public SimpleCommand(string description, IEnumerable<string> theCommands, Action<string> action) : base(description)
        {
            commands.AddRange(theCommands);
            toDo = action;
        }

        protected override void processCommand(string toProcess)
        {
            toDo(toProcess);
        }

        protected override List<string> possibleStarts()
        {
            return commands;
        }

        protected override string getSyntaxDescription()
        {
            string possibleForms = commands.Aggregate(string.Empty, (current, next) => current + string.Format("<{0}>", next));
            return possibleForms;
        }

    }

    public class ConsoleCommandHolder

    {
        private readonly List<ConsoleCommand> unique = new List<ConsoleCommand>();

        public void AddRange(IEnumerable<ConsoleCommand> toAdd)
        {
            foreach (var consoleCommand in toAdd)
            {
                Add(consoleCommand);
            }
        }

        public void Add(ConsoleCommand toAdd)
        {
            unique.Add(toAdd);
        }

        public ConsoleCommand Find(string command)
        {
            return unique.FirstOrDefault(x => x.Prefixes.Any(command.StartsWith));
        }

        public override string ToString()
        {
            return unique.Aggregate(string.Empty, (current, next) => current + ("\r\n" + next));
        }
    }

    //public sealed class AddCommand : ConsoleCommand
    //{
    //    private readonly Dictionary<string, string> domainObjectToAffect;


    //    public AddCommand(Dictionary<string, string> domainToAffect) : base("Add string  to dictionary")
    //    {
    //        domainObjectToAffect = domainToAffect;

    //    }

    //    protected override void processCommand(string toProcess)
    //    {
    //        string[] tokens = toProcess.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
    //        if (tokens.Length != 3) Console.WriteLine("Wrong command format. Syntax: " + getSyntaxDescription());
    //        domainObjectToAffect.Add(tokens[1], tokens[2]);
    //        Console.WriteLine("OK");
    //    }

    //    protected override string getSyntaxDescription()
    //    {
    //        return "<add 'v1' 'v2'>";
    //    }

    //    protected override List<string> possibleStarts()
    //    {
    //        return new List<string>() { "add " };
    //    }
    //}


}

