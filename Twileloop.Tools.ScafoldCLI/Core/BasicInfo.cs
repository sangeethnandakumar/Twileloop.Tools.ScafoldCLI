namespace Twileloop.Tools.ScafoldCLI.Core
{
    public class Package
    {
        public string PackageId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public string[] Authors { get; set; }
        public string PackageIconURL { get; set; }
    }

    public class Directives
    {
        public string RootDirectory { get; set; }
        public string GitOrgAndRepo { get; set; }
    }

    public class Support
    {
        public string ContactMail { get; set; }
        public string BuyMeACoffeeUsername { get; set; }
    }
    public class ProjectInfo
    {
        public Package Package { get; set; }
        public Directives Directives { get; set; }
        public Support Support { get; set; }
    }
}
