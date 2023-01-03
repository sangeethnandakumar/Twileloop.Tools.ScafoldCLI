using System.Xml;
using Twileloop.Tools.ScafoldCLI.Core;

namespace Twileloop.Tools.ScafoldCLI.Utilities.NuGetDeploy
{
    public class NuGetDeployUtility : AbstractUtility
    {
        public override string UniqueID { get; set; } = "nupkg-deploy";
        public override string DisplayName { get; set; } = "NuGet Deploy";
        public override string Description { get; set; } = "Adds a NuGet deployment workflow yml to project, Which triggers on release-tag creations";
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


            Log("Scafolding CI/CD nugetcd.yml...");
            var template = File.ReadAllText("Utilities\\NuGetDeploy\\nugetcd.yml");
            template = template.Replace("{TARGETPROJECT}", csprojPath);       
            template = template.Replace("{PACKAGEID}", projectInfo.Package.PackageId);       


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

            string destinationPath = Path.Combine(workflowsFolder, "nugetcd.yml");
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