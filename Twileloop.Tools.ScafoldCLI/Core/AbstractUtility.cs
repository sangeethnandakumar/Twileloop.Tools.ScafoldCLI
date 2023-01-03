using Spectre.Console;

namespace Twileloop.Tools.ScafoldCLI.Core
{
    public abstract class AbstractUtility
    {
        public abstract string UniqueID { get; set; }
        public abstract string DisplayName { get; set; }
        public abstract string Description { get; set; }
        public abstract string[] Authors { get; set; }

        public void Log(string logText)
        {
            AnsiConsole.Markup($"\n   → [grey]{logText}[/]");
        }

        public bool AskYesOrNo(string question)
        {
            Log($"{question} [lime][[y/n]][/]: ");
            var choice = Console.ReadLine();
            if (choice.ToLower() != "y")
            {
                return false;
            }
            return true;
        }

        public string AskQuery(string question)
        {
            AnsiConsole.Markup($"\n   → Q.[white] {question}[/]");
            return Console.ReadLine();
        }

        public virtual bool OnStart(ProjectInfo basicInfo)
        {
            return true;
        }

        public virtual bool OnExecute(ProjectInfo basicInfo)
        {
            return true;
        }

        public virtual bool OnFinish(ProjectInfo basicInfo)
        {
            return true;
        }
    }
}