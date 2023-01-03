﻿using Twileloop.Tools.ScafoldCLI.Core;

namespace Twileloop.Tools.ScafoldCLI.Utilities
{
    public class PackageReadyUtility : AbstractUtility
    {
        public override string UniqueID { get; set; } = "pkgmeta";
        public override string DisplayName { get; set; } = "Package Ready";
        public override string Description { get; set; } = "Adds basic meta details to csproj for packaging";
        public override string[] Authors { get; set; } = new string[] { "Sangeeth Nandakumar", "Twileloop" };

        public override bool OnExecute(BasicInfo basicInfo)
        {
            ReportLog("Executing...");
            return true;
        }

        public override bool OnFinish(BasicInfo basicInfo)
        {
            ReportLog("Finishing off...");
            return true;
        }

        public override bool OnStart(BasicInfo basicInfo)
        {
            ReportLog("Starting...");
            return true;
        }
    }
}