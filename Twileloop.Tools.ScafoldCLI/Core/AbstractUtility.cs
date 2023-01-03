using Spectre.Console;

namespace Twileloop.Tools.ScafoldCLI.Core
{
    public abstract class AbstractUtility
    {
        public abstract string UniqueID { get; set; }
        public abstract string DisplayName { get; set; }
        public abstract string Description { get; set; }
        public abstract string[] Authors { get; set; }

        public void ReportLog(string logText)
        {
            AnsiConsole.MarkupLine($"   → [grey]{logText}[/]");
        }

        public virtual bool OnStart(BasicInfo basicInfo)
        {
            return true;
        }

        public virtual bool OnExecute(BasicInfo basicInfo)
        {
            return true;
        }

        public virtual bool OnFinish(BasicInfo basicInfo)
        {
            return true;
        }
    }
}