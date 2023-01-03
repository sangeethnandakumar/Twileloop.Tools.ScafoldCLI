using System.Xml;
using Twileloop.Tools.ScafoldCLI.Core;

namespace Twileloop.Tools.ScafoldCLI.Utilities.SonarScan
{
    public class SonarScanUtility : AbstractUtility
    {
        public override string UniqueID { get; set; } = "sonar-scan";
        public override string DisplayName { get; set; } = "Sonar Scanner";
        public override string Description { get; set; } = "Adds a SonarCloud scan workflow yml to project, Which triggers on any changes to 'master'";
        public override string[] Authors { get; set; } = new string[] { "Sangeeth Nandakumar", "Twileloop" };

        public override bool OnExecute(ProjectInfo projectInfo)
        {
            Log("Detecting all csproj files in current root directory");
            var allProjFiles = GetCsprojFiles(projectInfo.Directives.RootDirectory);

            Log($"Found these project files");
            foreach (string csprojFile in allProjFiles)
            {
                Log($"  - {csprojFile}");
            }

            var csprojPath = AskQuery($"Enter project path");


            Log("Scafolding CI/CD sonarscan.yml...");
            var template = File.ReadAllText("Utilities\\SonarScan\\sonarscan.yml");
            template = template.Replace("{PACKAGEID}", projectInfo.Package.PackageId);       
            template = template.Replace("{SONARQUBEPROJECTID}", projectInfo.Directives.GitOrgAndRepo.Replace("/", "_"));       
            template = template.Replace("{GITUSERNAME}", projectInfo.Directives.GitOrgAndRepo.Split("/")[0].ToLower());       


            CopyFileToWorkflowsFolder(projectInfo.Directives.RootDirectory, template);
            return true;
        }

        public void CopyFileToWorkflowsFolder(string rootFolder, string content)
        {
            string workflowsFolder = Path.Combine(rootFolder, ".github\\workflows");

            if (!Directory.Exists(workflowsFolder))
            {
                Directory.CreateDirectory(workflowsFolder);
            }

            string destinationPath = Path.Combine(workflowsFolder, "sonarscan.yml");
            Log("Writing file...");
            File.WriteAllText(destinationPath, content);
        }

        public List<string> GetCsprojFiles(string rootFolder)
        {
            List<string> csprojFiles = new List<string>();

            foreach (string file in Directory.EnumerateFiles(rootFolder, "*.csproj", SearchOption.AllDirectories))
            {
                string relativePath = file.Replace(rootFolder, "").TrimStart('\\');
                csprojFiles.Add(relativePath);
            }

            return csprojFiles;
        }
    }
}