#load ".cake/Configuration.cake"

/**********************************************************/
Setup<Configuration>(Configuration.Create);
/**********************************************************/

#load ".cake/CI.cake"

// -- DotNetCore
#load ".cake/Restore-DotNetCore.cake"
#load ".cake/Build-DotNetCore.cake"
#load ".cake/Test-DotNetCore.cake"
#load ".cake/Publish-Zip-DotNetCore.cake"
#load ".cake/Publish-Pack-DotNetCore.cake"
#load ".cake/Artifacts-DotNetCore-Ef.cake"
// -------------

Task("Restore:DotNetCore:Tools")
    .IsDependeeOf("Restore")
    .Does<Configuration>(config => {

    var settings = new ProcessSettings() 
    { 
        RedirectStandardOutput = false
    };

    settings.Arguments = string.Format($"tool restore");
    using(var process = StartAndReturnProcess("dotnet", settings))
    {
        process.WaitForExit();
    }
});

Task("Tools:Git-Export")
    .Does<Configuration>(config => {

    var settings = new ProcessSettings() 
    { 
        RedirectStandardOutput = false
    };

    var exportArtifactRootPath = $"{config.Artifacts.Root}/export";
    EnsureDirectoryExists(exportArtifactRootPath);
    var exportArchiveFile = MakeAbsolute(File($"{exportArtifactRootPath}/export.zip"));
    
    if(FileExists(exportArchiveFile))
    {
        DeleteFile(exportArchiveFile);
    }
    
    settings.Arguments = string.Format($"archive -o {exportArchiveFile} HEAD");
    
    using(var process = StartAndReturnProcess("git", settings))
    {
        process.WaitForExit();
        if(process.GetExitCode() == 0)
        {
            Information("Exported source to {0}", exportArchiveFile);
            config.Artifacts.Add(ArtifactTypeOption.Other, exportArchiveFile.GetFilename().ToString(), exportArchiveFile);            
        }
    }
});

RunTarget(Argument("target", Argument("Target", "Default")));