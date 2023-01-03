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

        public override bool OnExecute(BasicInfo basicInfo)
        {
            ReportLog("Executing...");
            var template = File.ReadAllText("Utilities\\BasicReadMe\\README.md");
            template = template.Replace("{NAME}", basicInfo.Name);
            template = template.Replace("{PACKAGEID}", basicInfo.PackageId);
            template = template.Replace("{DESCRIPTION}", basicInfo.Description);
            template = template.Replace("{GITORGANDREPO}", basicInfo.GitOrgAndRepo);
            template = template.Replace("{PACKAGEICONURL}", basicInfo.PackageIconURL);
            template = template.Replace("{READTHEDOCSSUBDOMAIN}", basicInfo.GitOrgAndRepo.Split("/")[1].ToLower());
            template = template.Replace("{CONTACTMAIL}", basicInfo.ContactMail);
            template = template.Replace("{BUYMEACOFFEEUSERNAME}", basicInfo.BuyMeACoffeeUsername);
            template = template.Replace("{SONARQUBEPROJECTID}", basicInfo.GitOrgAndRepo.Replace("/", "_"));
            File.WriteAllText($"{basicInfo.RootDirectory}\\README.md", template);
            return true;
        }

        public override bool OnFinish(BasicInfo basicInfo)
        {
            return true;
        }

        public override bool OnStart(BasicInfo basicInfo)
        {
            ReportLog("Checking if an existing README.md file exists...");
            string fileName = $"{basicInfo.RootDirectory}\\README.md";

            if (File.Exists(fileName))
            {
                ReportLog("A file called 'README.md' already exists. Do you want to override (y/n)?");
                if (File.Exists(fileName))
                {
                    ReportLog("A file called 'README.md' already exists. Do you want to override?");
                }
            }
            return true;
        }
    }
}