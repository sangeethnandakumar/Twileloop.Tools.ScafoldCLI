using Twileloop.Tools.ScafoldCLI.Core;
namespace Twileloop.Tools.ScafoldCLI.Utilities.ReadTheDoc
{
    public class ReadTheDocUtility : AbstractUtility
    {
        public override string UniqueID { get; set; } = "readthedoc";
        public override string DisplayName { get; set; } = "Read The Doc";
        public override string Description { get; set; } = "Adds readthedoc template to solution and includes 'Doc' folder";
        public override string[] Authors { get; set; } = new string[] { "Sangeeth Nandakumar", "Twileloop" };

        public override bool OnExecute(ProjectInfo info)
        {
            Log("Migrating document template...");

            string sourceDirectory = @"Utilities\ReadTheDoc\Docs";
            string targetDirectory = $"{info.Directives.RootDirectory}\\Doc";

            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            Log("Copying sub directories...");
            foreach (string dirPath in Directory.GetDirectories(sourceDirectory, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourceDirectory, targetDirectory));
            }

            foreach (string newPath in Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourceDirectory, targetDirectory), true);
            }

            Log("Scafolding python entry file");
            var template = File.ReadAllText($"{targetDirectory}\\conf.py");
            template = template.Replace("{PACKAGEID}", info.Package.PackageId);
            template = template.Replace("{NAME}", info.Package.Name);
            template = template.Replace("{YEAR}", DateTime.Now.Year.ToString());
            template = template.Replace("{COMPANY}", info.Package.Company);
            template = template.Replace("{PRIMARYAUTHOR}", info.Package.Authors.FirstOrDefault());
            template = template.Replace("{AUTHORS}", string.Join(", ", info.Package.Authors));
            template = template.Replace("{DESCRIPTION}", info.Package.Description);
            template = template.Replace("{GITORGANDREPO}", info.Directives.GitOrgAndRepo);
            template = template.Replace("{PACKAGEICONURL}", info.Package.PackageIconURL);
            template = template.Replace("{READTHEDOCSSUBDOMAIN}", info.Directives.GitOrgAndRepo.Split("/")[1].ToLower());
            template = template.Replace("{CONTACTMAIL}", info.Support.ContactMail);
            template = template.Replace("{BUYMEACOFFEEUSERNAME}", info.Support.BuyMeACoffeeUsername);
            template = template.Replace("{SONARQUBEPROJECTID}", info.Directives.GitOrgAndRepo.Replace("/", "_"));
            Log("Copying file...");
            File.WriteAllText($"{targetDirectory}\\conf.py", template);


            Log("Scafolding intro file");
            template = File.ReadAllText($"{targetDirectory}\\Sections\\sectionA.rst");
            template = template.Replace("{PACKAGEID}", info.Package.PackageId);
            template = template.Replace("{NAME}", info.Package.Name);
            template = template.Replace("{YEAR}", DateTime.Now.Year.ToString());
            template = template.Replace("{COMPANY}", info.Package.Company);
            template = template.Replace("{PRIMARYAUTHOR}", info.Package.Authors.FirstOrDefault());
            template = template.Replace("{AUTHORS}", string.Join(", ", info.Package.Authors));
            template = template.Replace("{DESCRIPTION}", info.Package.Description);
            template = template.Replace("{GITORGANDREPO}", info.Directives.GitOrgAndRepo);
            template = template.Replace("{PACKAGEICONURL}", info.Package.PackageIconURL);
            template = template.Replace("{READTHEDOCSSUBDOMAIN}", info.Directives.GitOrgAndRepo.Split("/")[1].ToLower());
            template = template.Replace("{CONTACTMAIL}", info.Support.ContactMail);
            template = template.Replace("{BUYMEACOFFEEUSERNAME}", info.Support.BuyMeACoffeeUsername);
            template = template.Replace("{SONARQUBEPROJECTID}", info.Directives.GitOrgAndRepo.Replace("/", "_"));
            Log("Copying file...");
            File.WriteAllText($"{targetDirectory}\\Sections\\sectionA.rst", template);



            return true;
        }

    }
}