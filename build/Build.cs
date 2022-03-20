using System;
using System.IO;
using System.Linq;
using Anabasis.Deployment;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

[CheckBuildProjectConfigurations]
class Build : BaseAnabasisBuild
{
    public Build()
    {
        SourceDirectory = RootDirectory;
        TestsDirectory = RootDirectory;
    }

    public override bool IsDeployOnKubernetes => true;


    public override FileInfo[] GetApplicationToDeployProjects()
    {
        return PathConstruction.GlobFiles(SourceDirectory, "**/*.Actor.csproj", "**/*.Bus.csproj", "**/*.Api.csproj")
                               .OrderBy(path => $"{path}")
                               .Select(path => new FileInfo($"{path}"))
                               .ToArray();
    }

    public static int Main()
    {
        return Execute<Build>(build => build.DeployToKubernetes);
    }

}
