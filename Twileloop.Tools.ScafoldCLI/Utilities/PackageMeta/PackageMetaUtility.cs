using System.Xml;
using Twileloop.Tools.ScafoldCLI.Core;

namespace Twileloop.Tools.ScafoldCLI.Utilities.PackageMeta
{
    public class PackageMetaUtility : AbstractUtility
    {
        public override string UniqueID { get; set; } = "pkgmeta";
        public override string DisplayName { get; set; } = "Package Meta";
        public override string Description { get; set; } = "Adds meta details to your csproj and put a logo for packaging";
        public override string[] Authors { get; set; } = new string[] { "Sangeeth Nandakumar", "Twileloop" };

        public override bool OnExecute(ProjectInfo basicInfo)
        {
            var csprojPath = AskQuery($"Enter project path");
            Log($"Preparing XML tags to patch in *.csproj file");
            Dictionary<string, string> tags = new Dictionary<string, string>
            {
                { "TargetFramework", "netstandard2.1" },
                { "PackageId", basicInfo.Package.PackageId },
                { "Title", basicInfo.Package.Name },
                { "Version", "1.0.0-alpha" },
                { "Authors", string.Join(", ", basicInfo.Package.Authors) },
                { "Company", basicInfo.Package.Company },
                { "Description", basicInfo.Package.Description },
                { "PackageProjectUrl", $"https://{basicInfo.Directives.GitOrgAndRepo.Split("/")[1].ToLower()}.readthedocs.io" },
                { "RepositoryUrl", $"https://github.com/{basicInfo.Directives.GitOrgAndRepo}" },
                { "RepositoryType", "git" },
                { "PackageTags", "library" },
                { "PackageIcon", "logo.png" }
            };
            Log($"Updating project file...");
            UpdateCsprojFile(csprojPath, tags);

            Log($"Adding 'images' folder for '{csprojPath}'");
            AddImagesFolder(csprojPath);

            Log($"Adding physical 'images' folder");
            if (!Directory.Exists(Directory.GetParent(csprojPath) + "\\images"))
            {
                Directory.CreateDirectory(Directory.GetParent(csprojPath) + "\\images");
            }

            Log($"Copying sample NuGet icon 'logo.png' and updating csproj file");
            CopyFileAndAddElement("Utilities\\PackageMeta\\logo.png", csprojPath);

            return true;
        }

        public override bool OnStart(ProjectInfo basicInfo)
        {
            Log($"Locating *.csproj file in root directory ({basicInfo.Directives.RootDirectory})...");
            string searchPattern = "*.csproj";
            Log($"Found these project files");
            foreach (string csprojFile in Directory.EnumerateFiles(basicInfo.Directives.RootDirectory, searchPattern, SearchOption.AllDirectories))
            {
                Log($"  - {csprojFile}");
            }
            return true;
        }


        public void CopyFileAndAddElement(string filePath, string csprojFile)
        {
            // Copy the file to the images folder
            string destinationPath = Directory.GetParent(csprojFile) + "\\images\\logo.png";
            File.Copy(filePath, destinationPath, true);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(csprojFile);

            XmlElement root = xmlDoc.DocumentElement;

            XmlElement itemGroup = xmlDoc.CreateElement("ItemGroup");
            root.AppendChild(itemGroup);

            XmlElement logoFile = xmlDoc.CreateElement("None");
            logoFile.SetAttribute("Include", "images\\logo.png");
            logoFile.SetAttribute("Pack", "true");
            logoFile.SetAttribute("PackagePath", "");
            itemGroup.AppendChild(logoFile);

            xmlDoc.Save(csprojFile);
        }


        public void UpdateCsprojFile(string csprojFile, Dictionary<string, string> tags)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(csprojFile);

            XmlElement root = xmlDoc.DocumentElement;

            foreach (KeyValuePair<string, string> tag in tags)
            {
                string tagName = tag.Key;
                string tagValue = tag.Value;

                XmlElement element = (XmlElement)root.SelectSingleNode($"/Project/PropertyGroup/{tagName}");

                if (element == null)
                {
                    // Element does not exist, so create it
                    XmlElement propertyGroup = (XmlElement)root.SelectSingleNode("/Project/PropertyGroup");
                    element = xmlDoc.CreateElement(tagName);
                    propertyGroup.AppendChild(element);
                }

                // Update the element with the new value
                element.InnerText = tagValue;
            }

            xmlDoc.Save(csprojFile);
        }

        public void AddImagesFolder(string csprojFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(csprojFile);

            XmlElement root = xmlDoc.DocumentElement;

            XmlElement itemGroup = (XmlElement)root.SelectSingleNode("/Project/ItemGroup");

            if (itemGroup == null)
            {
                // ItemGroup element does not exist, so create it
                itemGroup = xmlDoc.CreateElement("ItemGroup");
                root.AppendChild(itemGroup);
            }

            XmlElement imagesFolder = (XmlElement)itemGroup.SelectSingleNode("Folder[@Include='images\\']");

            if (imagesFolder == null)
            {
                // Images folder does not exist, so create it
                imagesFolder = xmlDoc.CreateElement("Folder");
                imagesFolder.SetAttribute("Include", "images\\");
                itemGroup.AppendChild(imagesFolder);
            }

            xmlDoc.Save(csprojFile);
        }

    }
}