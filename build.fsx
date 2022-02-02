#r "paket:
source https://nuget.org/api/v2
nuget Fake.Core.Target prerelease
nuget Fake.DotNet.Cli //"
#load "./.fake/build.fsx/intellisense.fsx"

open System

open Fake.Core
open Fake.DotNet
open Fake.IO

open System.IO.Compression;

let serverPath = Path.getFullName "./"
let deployDir = Path.getFullName "./deploy"
let bundleDir = Path.getFullName "./bundle"

let runDotNet cmd workingDir =
    let result =
        DotNet.exec (DotNet.Options.withWorkingDirectory workingDir) cmd ""
    if result.ExitCode <> 0 then failwithf "'dotnet %s' failed in %s" cmd workingDir

Target.create "Build" (fun _ ->
    runDotNet "build" serverPath
)

Target.create "Run" (fun _ ->
    runDotNet "run" serverPath
)

Target.create "Clean" (fun _ ->
    [ deployDir
      bundleDir ]
    |> Shell.cleanDirs
)

Target.create "Bundle" (fun _ ->

    let publishArgs = sprintf "publish -c Release -o \"%s\"" deployDir
    runDotNet publishArgs serverPath

    let bundleInputDir = Path.combine bundleDir "input"
    let bundleFile = Path.combine bundleDir "dotnet-core-eb-api-server.zip"
    Shell.mkdir bundleDir
    Shell.mkdir bundleInputDir
    ZipFile.CreateFromDirectory (deployDir, (Path.combine bundleInputDir "dotnet-core-eb-api-server.zip"))
    Shell.copyFile bundleInputDir "aws-windows-deployment-manifest.json"
    Shell.copyDir (Path.combine bundleInputDir ".ebextensions") ".ebextensions" FileFilter.allFiles

    Shell.rm bundleFile
    ZipFile.CreateFromDirectory (bundleInputDir, bundleFile)
)

open Fake.Core.TargetOperators

"Clean"
    ==> "Build"
    ==> "Bundle"

"Clean"
    ==> "Build"
    ==> "Run"

Target.runOrDefault "Build"