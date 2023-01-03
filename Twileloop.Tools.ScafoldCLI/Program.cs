using Spectre.Console;
using System.Reflection;
using System.Text.RegularExpressions;
using Twileloop.Tools.ScafoldCLI.Core;

AnsiConsole.Write(
    new FigletText("Scafold CLI")
        .LeftAligned()
        .Color(Color.Red));

//Get basic user input
//var packageId = AnsiConsole.Ask<string>("Enter your [green]Unique Package ID[/]:");
//var name = AnsiConsole.Ask<string>("Enter your [green]Application Name[/]:");
//var description = AnsiConsole.Ask<string>("Enter your [green]Package Description[/]: ");
//var authors = AnsiConsole.Ask<string>("Enter [green]Author(s)[/] [grey](Seperated by comma)[/]: ");

var basicInfo = new BasicInfo
{
    PackageId = "Twileloop.JetTask",
    Name = "Twileloop JetTask",
    Description = "Twileloop.JetTask allows building automated pipeline system into your .NET application with yaml support.",
    Authors = new string[] { "Sangeeth Nandakumar", "Twileloop" },
    RootDirectory = "D:\\Twileloop\\PerfoBoost",
    GitOrgAndRepo = "sangeethnandakumar/JetTask",
    PackageIconURL = "https://cdn-icons-png.flaticon.com/512/7202/7202291.png",
    ContactMail = "twileloop@outlook.com",
    BuyMeACoffeeUsername = "sangeethnanda"
};

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
            AnsiConsole.MarkupLine($"Executing: [blue]'{abstractInstance.DisplayName.ToUpper()}'[/]");

            abstractInstance.OnStart(basicInfo);
            abstractInstance.OnExecute(basicInfo);
            abstractInstance.OnFinish(basicInfo);

            Console.WriteLine();
            AnsiConsole.MarkupLine($"Completed Execution");

            break;
        }
    }
}

Console.Read();