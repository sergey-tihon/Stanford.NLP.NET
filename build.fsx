// --------------------------------------------------------------------------------------
// FAKE build script
// --------------------------------------------------------------------------------------

#r "packages/FAKE/tools/FakeLib.dll"
#r "System.IO.Compression.FileSystem.dll"
#r "packages/FSharp.Management/lib/net40/FSharp.Management.dll"
#r "packages/Mono.Cecil/lib/net45/Mono.Cecil.dll"

open Microsoft.FSharp.Core.Printf
open Fake
open Fake.Git
open Fake.ReleaseNotesHelper
open System
open System.IO
open System.Reflection
open FSharp.Management
open Mono.Cecil

let [<Literal>]RootFolder = __SOURCE_DIRECTORY__
type root = FileSystem<RootFolder>

// --------------------------------------------------------------------------------------
// Provide project-specific details

let project = "Stanford.NLP.NET"
let solutionFile  = "Stanford.NLP.NET.sln"
// Pattern specifying assemblies to be tested using NUnit
let testAssemblies = "tests/**/bin/Release/*Tests*.dll"

// Git configuration (used for publishing documentation in gh-pages branch)
// The profile where the project is posted
let gitOwner = "sergey-tihon"
let gitHome = "https://github.com/" + gitOwner

// The name of the project on GitHub
let gitName = "Stanford.NLP.NET"
// The url for the raw files hosted
let gitRaw = environVarOrDefault "gitRaw" "https://raw.github.com/sergey-tihon"
// --------------------------------------------------------------------------------------

// Read additional information from the release notes document
let release = LoadReleaseNotes "RELEASE_NOTES.md"

// --------------------------------------------------------------------------------------
// IKVM.NET compilation helpers

let unZipTo toDir file =
    printfn "Unzipping file '%s' to '%s'" file toDir
    Compression.ZipFile.ExtractToDirectory(file, toDir)

let restoreFolderFromFile folder zipFile =
    if not <| Directory.Exists folder then
        zipFile |> unZipTo folder

// Location of IKVM Compiler & ildasm / ilasm
let ikvmc = root.``paket-files``.``www.frijters.net``.``ikvm-8.1.5717.0``.bin.``ikvmc.exe``

type IKVMcTask(jar:string) =
    member val JarFile = jar
    member val Version = "" with get, set
    member val Dependencies = List.empty<IKVMcTask> with get, set

let timeOut = TimeSpan.FromSeconds(120.0)

let IKVMCompile workingDirectory keyFile tasks =
    let getNewFileName newExtension (fileName:string) =
        Path.GetFileName(fileName).Replace(Path.GetExtension(fileName), newExtension)
    let startProcess fileName args =
        let result =
            ExecProcess
                (fun info ->
                    info.FileName <- fileName
                    info.WorkingDirectory <- FullName workingDirectory
                    info.Arguments <- args)
                timeOut
        if result<> 0 then
            failwithf "Process '%s' failed with exit code '%d'" fileName result

    let newKeyFile =
        if (File.Exists keyFile) then
            let file = workingDirectory @@ (Path.GetFileName(keyFile))
            File.Copy(keyFile, file, true)
            Path.GetFileName(file)
        else keyFile
    let rec compile (task:IKVMcTask) =
        let getIKVMCommandLineArgs() =
            let sb = Text.StringBuilder()
            task.Dependencies |> Seq.iter
                (fun x ->
                    compile x
                    x.JarFile |> getNewFileName ".dll" |> bprintf sb " -r:%s")
            if not <| String.IsNullOrEmpty(task.Version)
                then task.Version |> bprintf sb " -version:%s"
            bprintf sb " %s -out:%s"
                (task.JarFile |> getNewFileName ".jar")
                (task.JarFile |> getNewFileName ".dll")
            sb.ToString()

        File.Copy(task.JarFile, workingDirectory @@ (Path.GetFileName(task.JarFile)) ,true)
        startProcess ikvmc (getIKVMCommandLineArgs())
        if (File.Exists(newKeyFile)) then
            let key = FullName newKeyFile
                      |> File.ReadAllBytes
                      |> StrongNameKeyPair
            let dllFile = task.JarFile |> getNewFileName ".dll"
            ModuleDefinition
                .ReadModule(dllFile)
                .Write(dllFile, WriterParameters(StrongNameKeyPair=key))
    tasks |> Seq.iter compile

let copyPackages fromDir toDir =
    if (not <| Directory.Exists(toDir))
        then Directory.CreateDirectory(toDir) |> ignore
    Directory.GetFiles(fromDir)
    |> Seq.filter (fun x -> Path.GetExtension(x) = ".nupkg")
    |> Seq.iter   (fun x -> File.Copy(x, Path.Combine(toDir, Path.GetFileName(x)), true))

let removeNotAssembliesFrom dir =
    !! (dir + @"/*.*")
      -- (dir + @"/*.dll")
      |> Seq.iter (System.IO.File.Delete)

let createNuGetPackage workingDir nuspec =
    removeNotAssembliesFrom (workingDir+"\\lib")
    NuGet (fun p ->
        { p with
            Version = release.NugetVersion
            ReleaseNotes = String.Join(Environment.NewLine, release.Notes)
            OutputPath = "bin"
            AccessKey = getBuildParamOrDefault "nugetkey" ""
            Publish = false//hasBuildParam "nugetkey"
            WorkingDir = workingDir
            ToolPath = root.packages.``NuGet.CommandLine``.tools.``NuGet.exe`` })
        nuspec

let keyFile = @"nuget\Stanford.NLP.snk"

// --------------------------------------------------------------------------------------
// Clean build results

Target "Clean" (fun _ ->
    CleanDirs ["bin"; "temp"]
)

Target "CleanDocs" (fun _ ->
    CleanDirs ["docs/output"]
)

// --------------------------------------------------------------------------------------
// Compile Stanford.NLP.CoreNLP and build NuGet package

type coreNLPDir = root.``paket-files``.``nlp.stanford.edu``.``stanford-corenlp-full-2016-10-31``

Target "CompilerCoreNLP" (fun _ ->
    let ikvmDir  = @"bin\Stanford.NLP.CoreNLP\lib"
    CreateDir ikvmDir

    coreNLPDir.``stanford-corenlp-3.7.0-models.jar``
    |> restoreFolderFromFile (Path.Combine(coreNLPDir.Path, "models"))

    [IKVMcTask(coreNLPDir.``stanford-corenlp-3.7.0.jar``, Version=release.AssemblyVersion,
           Dependencies = [IKVMcTask(coreNLPDir.``joda-time.jar``, Version="2.9")
                           IKVMcTask(coreNLPDir.``jollyday.jar``, Version="0.4.7",
                                Dependencies =[IKVMcTask(coreNLPDir.``joda-time.jar``, Version="2.9")])
                           IKVMcTask(coreNLPDir.``ejml-0.23.jar``, Version="0.23")
                           IKVMcTask(coreNLPDir.``xom.jar``, Version="1.2.10")
                           IKVMcTask(coreNLPDir.``javax.json.jar``, Version="1.0.4")
                           IKVMcTask(coreNLPDir.``slf4j-api.jar``, Version="1.7.2")
                           IKVMcTask(coreNLPDir.``slf4j-simple.jar``, Version="1.7.2")
                           IKVMcTask(coreNLPDir.``protobuf.jar``, Version="2.6.1")])]
    |> IKVMCompile ikvmDir keyFile
)

Target "NuGetCoreNLP" (fun _ ->
    createNuGetPackage "bin/Stanford.NLP.CoreNLP" root.nuget.``Stanford.NLP.CoreNLP.nuspec``
)

// --------------------------------------------------------------------------------------
// Compile Stanford.NLP.NET and build NuGet package

type nerDir = root.``paket-files``.``nlp.stanford.edu``.``stanford-ner-2015-12-09``

Target "CompilerNER" (fun _ ->
    let ikvmDir  = @"bin\Stanford.NLP.NER\lib"
    CreateDir ikvmDir

    [IKVMcTask(nerDir.``stanford-ner.jar``, Version=release.AssemblyVersion,
        Dependencies = [IKVMcTask(nerDir.lib.``slf4j-api.jar``, Version="1.7.2")])]
    |> IKVMCompile ikvmDir keyFile
)

Target "NuGetNER" (fun _ ->
    createNuGetPackage "bin/Stanford.NLP.NER" root.nuget.``Stanford.NLP.NER.nuspec``
)

// --------------------------------------------------------------------------------------
// Compile Stanford.NLP.Parser and build NuGet package

type parserDir = root.``paket-files``.``nlp.stanford.edu``.``stanford-parser-full-2015-12-09``

Target "CompilerParser" (fun _ ->
    let ikvmDir  = @"bin\Stanford.NLP.Parser\lib"
    CreateDir ikvmDir

    restoreFolderFromFile (parserDir.Path + "models") parserDir.``stanford-parser-3.6.0-models.jar``
    [IKVMcTask(parserDir.``stanford-parser.jar``, Version=release.AssemblyVersion,
           Dependencies = [IKVMcTask(parserDir.``ejml-0.23.jar``, Version="0.23.0.0")
                           IKVMcTask(parserDir.``slf4j-api.jar``, Version="1.7.2")])]
    |> IKVMCompile ikvmDir keyFile
)

Target "NuGetParser" (fun _ ->
    createNuGetPackage "bin/Stanford.NLP.Parser" root.nuget.``Stanford.NLP.Parser.nuspec``
)

// --------------------------------------------------------------------------------------
// Compile Stanford.NLP.POSTagger and build NuGet package

type posDir = root.``paket-files``.``nlp.stanford.edu``.``stanford-postagger-full-2015-12-09``

Target "CompilerPOS" (fun _ ->
    let ikvmDir  = @"bin\Stanford.NLP.POSTagger\lib"
    CreateDir ikvmDir

    [IKVMcTask(posDir.``stanford-postagger-3.6.0.jar``, Version=release.AssemblyVersion,
        Dependencies = [IKVMcTask(posDir.lib.``slf4j-api.jar``, Version="1.7.2")])]
    |> IKVMCompile ikvmDir keyFile
)

Target "NuGetPOS" (fun _ ->
    createNuGetPackage "bin/Stanford.NLP.POSTagger" root.nuget.``Stanford.NLP.POSTagger.nuspec``
)

// --------------------------------------------------------------------------------------
// Compile Stanford.NLP.Segmenter and build NuGet package

type segmenterDir = root.``paket-files``.``nlp.stanford.edu``.``stanford-segmenter-2015-12-09``

Target "CompilerSegmenter" (fun _ ->
    let ikvmDir  = @"bin\Stanford.NLP.Segmenter\lib"
    CreateDir ikvmDir

    [IKVMcTask(segmenterDir.``stanford-segmenter-3.6.0.jar``, Version=release.AssemblyVersion,
        Dependencies=[IKVMcTask(segmenterDir.``slf4j-api.jar``, Version="1.7.2")])]
    |> IKVMCompile ikvmDir keyFile
)

Target "NuGetSegmenter" (fun _ ->
    createNuGetPackage "bin/Stanford.NLP.Segmenter" root.nuget.``Stanford.NLP.Segmenter.nuspec``
)


// --------------------------------------------------------------------------------------
// Build and run test projects

Target "BuildTests" (fun _ ->
    !! solutionFile
    |> MSBuildRelease "" "Rebuild"
    |> ignore
)

Target "RunTests" (fun _ ->
    !! testAssemblies
    |> NUnit (fun p ->
        { p with
            DisableShadowCopy = true
            TimeOut = TimeSpan.FromMinutes 60.
            OutputFile = "TestResults.xml" })
)

// --------------------------------------------------------------------------------------
// Generate the documentation

Target "GenerateReferenceDocs" (fun _ ->
    if not <| executeFSIWithArgs "docs/tools" "generate.fsx" ["--define:RELEASE"; "--define:REFERENCE"] [] then
      failwith "generating reference documentation failed"
)

let generateHelp' fail debug =
    let args =
        if debug then ["--define:HELP"]
        else ["--define:RELEASE"; "--define:HELP"]
    if executeFSIWithArgs "docs/tools" "generate.fsx" args [] then
        traceImportant "Help generated"
    else
        if fail then
            failwith "generating help documentation failed"
        else
            traceImportant "generating help documentation failed"

let generateHelp fail =
    generateHelp' fail false

Target "GenerateHelp" (fun _ ->
    DeleteFile "docs/content/release-notes.md"
    CopyFile "docs/content/" "RELEASE_NOTES.md"
    Rename "docs/content/release-notes.md" "docs/content/RELEASE_NOTES.md"

    DeleteFile "docs/content/license.md"
    CopyFile "docs/content/" "LICENSE.txt"
    Rename "docs/content/license.md" "docs/content/LICENSE.txt"

    generateHelp true
)

Target "GenerateHelpDebug" (fun _ ->
    DeleteFile "docs/content/release-notes.md"
    CopyFile "docs/content/" "RELEASE_NOTES.md"
    Rename "docs/content/release-notes.md" "docs/content/RELEASE_NOTES.md"

    DeleteFile "docs/content/license.md"
    CopyFile "docs/content/" "LICENSE.txt"
    Rename "docs/content/license.md" "docs/content/LICENSE.txt"

    generateHelp' true true
)

Target "KeepRunning" (fun _ ->
    use watcher = new FileSystemWatcher(DirectoryInfo("docs/content").FullName,"*.*")
    watcher.EnableRaisingEvents <- true
    watcher.Changed.Add(fun e -> generateHelp false)
    watcher.Created.Add(fun e -> generateHelp false)
    watcher.Renamed.Add(fun e -> generateHelp false)
    watcher.Deleted.Add(fun e -> generateHelp false)

    traceImportant "Waiting for help edits. Press any key to stop."

    System.Console.ReadKey() |> ignore

    watcher.EnableRaisingEvents <- false
    watcher.Dispose()
)

Target "GenerateDocs" DoNothing

let createIndexFsx lang =
    let content = """(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../../bin"

(**
F# Project Scaffold ({0})
=========================
*)
"""
    let targetDir = "docs/content" @@ lang
    let targetFile = targetDir @@ "index.fsx"
    ensureDirectory targetDir
    System.IO.File.WriteAllText(targetFile, System.String.Format(content, lang))

Target "AddLangDocs" (fun _ ->
    let args = System.Environment.GetCommandLineArgs()
    if args.Length < 4 then
        failwith "Language not specified."

    args.[3..]
    |> Seq.iter (fun lang ->
        if lang.Length <> 2 && lang.Length <> 3 then
            failwithf "Language must be 2 or 3 characters (ex. 'de', 'fr', 'ja', 'gsw', etc.): %s" lang

        let templateFileName = "template.cshtml"
        let templateDir = "docs/tools/templates"
        let langTemplateDir = templateDir @@ lang
        let langTemplateFileName = langTemplateDir @@ templateFileName

        if System.IO.File.Exists(langTemplateFileName) then
            failwithf "Documents for specified language '%s' have already been added." lang

        ensureDirectory langTemplateDir
        Copy langTemplateDir [ templateDir @@ templateFileName ]

        createIndexFsx lang)
)

// --------------------------------------------------------------------------------------
// Release Scripts

Target "ReleaseDocs" (fun _ ->
    let tempDocsDir = "temp/gh-pages"
    CleanDir tempDocsDir
    Repository.cloneSingleBranch "" (gitHome + "/" + gitName + ".git") "gh-pages" tempDocsDir

    CopyRecursive "docs/output" tempDocsDir true |> tracefn "%A"
    StageAll tempDocsDir
    Git.Commit.Commit tempDocsDir (sprintf "Update generated documentation for version %s" release.NugetVersion)
    Branches.push tempDocsDir
)

Target "Release" DoNothing

// --------------------------------------------------------------------------------------
// Run all targets by default. Invoke 'build <Target>' to override

Target "All" DoNothing
Target "NuGet" DoNothing

"Clean"
  ==> "CompilerCoreNLP"
  ==> "NuGetCoreNLP"
  ==> "NuGet"

"Clean"
  ==> "CompilerNER"
  ==> "NuGetNER"
  ==> "NuGet"

"Clean"
  ==> "CompilerParser"
  ==> "NuGetParser"
  ==> "NuGet"

"Clean"
  ==> "CompilerPOS"
  ==> "NuGetPOS"
  ==> "NuGet"

"Clean"
  ==> "CompilerSegmenter"
  ==> "NuGetSegmenter"
  ==> "NuGet"

"NuGet"
  ==> "BuildTests"
  ==> "RunTests"
  =?> ("GenerateReferenceDocs",isLocalBuild && not isMono)
  =?> ("GenerateDocs",isLocalBuild && not isMono)
  ==> "All"
  =?> ("ReleaseDocs",isLocalBuild && not isMono)

"CleanDocs"
  ==> "GenerateHelp"
  ==> "GenerateReferenceDocs"
  ==> "GenerateDocs"
  ==> "ReleaseDocs"

"CleanDocs"
  ==> "GenerateHelpDebug"

"GenerateHelp"
  ==> "KeepRunning"

"ReleaseDocs"
  ==> "Release"

RunTargetOrDefault "All"
