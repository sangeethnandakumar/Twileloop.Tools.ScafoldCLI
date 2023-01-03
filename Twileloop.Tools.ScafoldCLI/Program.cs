using Spectre.Console;
using System.Reflection;
using System.Text.RegularExpressions;
using Twileloop.Tools.ScafoldCLI.Core;
using Twileloop.Tools.ScafoldCLI.Helpers;

AnsiConsole.Write(
    new FigletText("Scafold CLI")
        .LeftAligned()
        .Color(Color.Red));

var projectInfo = Helpers.DeserializeFromXml(File.ReadAllText("projectinfo.xml"), typeof(ProjectInfo)) as ProjectInfo;


Console.WriteLine();

var assembly = Assembly.GetExecutingAssembly();
var utilityDisplayNames = new List<string>();
foreach (Type type in assembly.GetTypes())
{
    if (type.IsSubclassOf(typeof(AbstractUtility)))
    {
        var abstractInstance = (AbstractUtility)Activator.CreateInstance(type);
        utilityDisplayNames.Add($"[[{abstractInstance.UniqueID}]]: [white]{abstractInstance.Description}[/]");
    }
}

//Give list of scafold options
var selectedOption = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Select one [green]scafold utility[/] you would like to invoke:")
        .PageSize(10)
        .AddChoices(utilityDisplayNames));

var choice = Regex.Matches(selectedOption, @"\[\[(.*?)\]\]").FirstOrDefault().Groups[1].Value;

foreach (Type type in assembly.GetTypes())
{
    if (type.IsSubclassOf(typeof(AbstractUtility)))
    {
        var abstractInstance = (AbstractUtility)Activator.CreateInstance(type);
        if (abstractInstance.UniqueID == choice)
        {
            var panel = new Panel($"[underline bold blue]{abstractInstance.DisplayName}[/]");
            AnsiConsole.Write(panel);
            AnsiConsole.MarkupLine($"[white]{abstractInstance.Description}[/]");
            AnsiConsole.MarkupLine($"Authors: [blue]{string.Join(", ", abstractInstance.Authors)}[/]");

            var stageResult = true;
            stageResult = abstractInstance.OnStart(projectInfo);
            if (stageResult)
            {
                stageResult = abstractInstance.OnExecute(projectInfo);
                if (stageResult)
                {
                    stageResult = abstractInstance.OnFinish(projectInfo);
                    if (stageResult)
                    {
                        Console.WriteLine();
                        Console.WriteLine();
                        AnsiConsole.MarkupLine($"Completed Execution");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"   → [red]Stage Unsuccessfull[/]");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine($"   → [red]Stage Unsuccessfull[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine($"   → [red]Stage Unsuccessfull[/]");
            }

            break;
        }
    }
}

Console.Read();