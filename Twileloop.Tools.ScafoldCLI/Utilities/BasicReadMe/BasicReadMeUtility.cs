using System.Diagnostics;
using Twileloop.Tools.ScafoldCLI.Core;

namespace Twileloop.Tools.ScafoldCLI.Utilities.BasicReadMe
{
    public class BasicReadMeUtility : AbstractUtility
    {
        public override string UniqueID { get; set; } = "basicreadme";
        public override string DisplayName { get; set; } = "Basic ReadMe";
        public override string Description { get; set; } = "Adds a standard ReadMe file to current directory";
        public override string[] Authors { get; set; } = new string[] { "Sangeeth Nandakumar", "Twileloop" };

        public override bool OnExecute(ProjectInfo info)
        {
            Log("Scafolding README.md...");
            var template = File.ReadAllText("Utilities\\BasicReadMe\\README.md");
            template = template.Replace("{NAME}", info.Name);
            template = template.Replace("{PACKAGEID}", info.PackageId);
            template = template.Replace("{DESCRIPTION}", info.Description);
            template = template.Replace("{GITORGANDREPO}", info.GitOrgAndRepo);
            template = template.Replace("{PACKAGEICONURL}", info.PackageIconURL);
            template = template.Replace("{READTHEDOCSSUBDOMAIN}", info.GitOrgAndRepo.Split("/")[1].ToLower());
            template = template.Replace("{CONTACTMAIL}", info.ContactMail);
            template = template.Replace("{BUYMEACOFFEEUSERNAME}", info.BuyMeACoffeeUsername);
            template = template.Replace("{SONARQUBEPROJECTID}", info.GitOrgAndRepo.Replace("/", "_"));
            Log("Copying file...");
            File.WriteAllText($"{info.RootDirectory}\\README.md", template);
            return true;
        }

        public override bool OnFinish(ProjectInfo info)
        {
            Log("Done");
            return true;
        }

        public override bool OnStart(ProjectInfo info)
        {
            Log("Checking if an existing README.md file exists...");
            string fileName = $"{info.RootDirectory}\\README.md";
            if (File.Exists(fileName))
            {
                return AskYesOrNo("A file called 'README.md' already exists. Do you want to override");
            }
            return true;
        }
    }
}