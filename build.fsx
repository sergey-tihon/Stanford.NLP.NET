#r @"paket:
source https://nuget.org/api/v2
framework netstandard2.0
nuget Mono.Cecil
nuget System.IO.Compression.ZipFile
nuget Fake.Core.Target
nuget Fake.Core.Process
nuget Fake.Core.ReleaseNotes
nuget Fake.IO.FileSystem
nuget Fake.DotNet.Cli
nuget Fake.DotNet.MSBuild
nuget Fake.DotNet.AssemblyInfoFile
nuget Fake.DotNet.Paket
nuget Fake.DotNet.Testing.Expecto
nuget Fake.DotNet.FSFormatting
nuget Fake.Tools.Git
nuget Fake.Api.GitHub //"

#if !FAKE
#load "./.fake/build.fsx/intellisense.fsx"
#r "netstandard" // Temp fix for https://github.com/fsharp/FAKE/issues/1985
#endif

open System
open System.IO
open System.IO.Compression
open Microsoft.FSharp.Core
open Mono.Cecil
open Fake
open Fake.Core.TargetOperators
open Fake.Core
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.DotNet
open Fake.Tools
//open Fake.Tools.Git

Target.initEnvironment()

// --------------------------------------------------------------------------------------
// FAKE build script
// --------------------------------------------------------------------------------------

// #r "packages/FAKE/tools/FakeLib.dll"
// #r "System.IO.Compression.FileSystem.dll"
// #r "packages/FSharp.Management/lib/net40/FSharp.Management.dll"
// #r "packages/Mono.Cecil/lib/net45/Mono.Cecil.dll"
// #r "System.IO.Compression.dll"

// open Microsoft.FSharp.Core.Printf
// open Fake
// open Fake.Git
// open Fake.ReleaseNotesHelper
// open Fake.Testing.Expecto
// open System
// open System.IO
// open System.IO.Compression
// open System.Reflection
// open FSharp.Management
// open Mono.Cecil

let [<Literal>]root = __SOURCE_DIRECTORY__
//Environment.CurrentDirectory <- RootFolder

// Read additional information from the release notes document
let release = ReleaseNotes.load "RELEASE_NOTES.md"

// --------------------------------------------------------------------------------------
// IKVM.NET compilation helpers

let fixFileNames path =
    use file = File.Open(path, FileMode.Open, FileAccess.ReadWrite)
    use archive = new ZipArchive(file, ZipArchiveMode.Update)
    archive.Entries
    |> Seq.toList
    |> List.filter(fun x -> x.FullName.Contains(":"))
    |> List.iter (fun entry ->
        printfn "%s " entry.FullName
        let newName = entry.FullName.Replace(":","_")
        let newEntry = archive.CreateEntry(newName)
        begin
            use a = entry.Open()
            use b = newEntry.Open()
            a.CopyTo(b)
        end
        entry.Delete()
       )

let unZipTo toDir file =
    Trace.trace "Renaming files inside zip archive ..."
    fixFileNames file
    Trace.tracefn "Unzipping file '%s' to '%s'" file toDir
    Compression.ZipFile.ExtractToDirectory(file, toDir)

let restoreFolderFromFile folder zipFile =
    if not <| Directory.Exists folder then
        zipFile |> unZipTo folder

// Location of IKVM Compiler
let ikvmcExe = root </> "data/paket-files/www.frijters.net/ikvm-8.1.5717.0/bin/ikvmc.exe"

type IKVMcTask(jar:string, version:string) =
    member __.JarFile = jar
    member __.Version = version
    member __.DllFile = Path.ChangeExtension(Path.GetFileName(jar), ".dll")
    member val Dependencies = List.empty<IKVMcTask> with get, set
    member this.GetDllReferences() =
        seq {
            for t in this.Dependencies do
                yield! t.GetDllReferences()
            yield this.DllFile
        }

let timeOut = TimeSpan.FromSeconds(120.0)

let IKVMCompile workingDirectory keyFile tasks =
    let ikvmc args =
        (if Environment.isWindows
         then CreateProcess.fromRawCommandLine ikvmcExe args
         else CreateProcess.fromRawCommandLine "mono" (ikvmcExe + " " + args))
        |> CreateProcess.withWorkingDirectory (DirectoryInfo(workingDirectory).FullName)
        |> CreateProcess.withTimeout timeOut
        |> CreateProcess.ensureExitCode
        |> Proc.run
        |> ignore
        //let result =
        //    ExecProcess
        //        (fun info ->
        //            info.FileName <- ikvmcExe
        //            info.WorkingDirectory <- FullName workingDirectory
        //            info.Arguments <- args)
        //        timeOut
        //if result <> 0 then
        //    failwithf "Process 'ikvmc.exe' failed with exit code '%d'" result
    let newKeyFile =
        if (File.Exists keyFile) then
            let file = workingDirectory @@ (Path.GetFileName(keyFile))
            File.Copy(keyFile, file, true)
            Path.GetFileName(file)
        else keyFile

    let bprintf = Microsoft.FSharp.Core.Printf.bprintf
    let cache = System.Collections.Generic.HashSet<_>()
    let rec compile (task:IKVMcTask) =
        let getIKVMCommandLineArgs() =
            let sb = Text.StringBuilder()
            task.Dependencies
            |> Seq.collect (fun x ->
                compile x
                x.GetDllReferences()
            )
            |> Seq.distinct
            |> Seq.iter (bprintf sb " -r:%s")
            if not <| String.IsNullOrEmpty(task.Version)
                then task.Version |> bprintf sb " -version:%s"
            bprintf sb " %s -out:%s" task.JarFile task.DllFile
            sb.ToString()

        if cache.Contains task.JarFile
        then
            Trace.tracefn "Task '%s' already compiled" task.JarFile
        else
            //File.Copy(task.JarFile, workingDirectory @@ (Path.GetFileName(task.JarFile)) ,true)
            ikvmc <| getIKVMCommandLineArgs()
            if (File.Exists(newKeyFile)) then
                let key = FileInfo(newKeyFile).FullName
                          |> File.ReadAllBytes
                ModuleDefinition
                    .ReadModule(task.DllFile, ReaderParameters(InMemory=true))
                    .Write(task.DllFile, WriterParameters(StrongNameKeyBlob=key))
            cache.Add(task.JarFile) |> ignore
    tasks |> Seq.iter compile

let copyPackages fromDir toDir =
    if (not <| Directory.Exists(toDir))
        then Directory.CreateDirectory(toDir) |> ignore
    Directory.GetFiles(fromDir)
    |> Seq.filter (fun x -> Path.GetExtension(x) = ".nupkg")
    |> Seq.iter   (fun x -> File.Copy(x, Path.Combine(toDir, Path.GetFileName(x)), true))

let createNuGetPackage template =
    Fake.DotNet.Paket.pack(fun p ->
        { p with
            ToolType = Fake.DotNet.ToolType.CreateLocalTool()
            TemplateFile = template
            OutputPath = "bin"
            Version = release.NugetVersion
            ReleaseNotes = String.toLines release.Notes})

    // NuGet (fun p ->
    //     { p with
    //         Version = release.NugetVersion
    //         ReleaseNotes = String.Join(Environment.NewLine, release.Notes)
    //         OutputPath = "bin"
    //         AccessKey = getBuildParamOrDefault "nugetkey" ""
    //         Publish = false//hasBuildParam "nugetkey"
    //         WorkingDir = workingDir
    //         ToolPath = root.packages.``NuGet.CommandLine``.tools.``NuGet.exe`` })
    //     nuspec

let keyFile = @"nuget/Stanford.NLP.snk"

// --------------------------------------------------------------------------------------
// Clean build results

Target.create "Clean" (fun _ ->
    Shell.cleanDirs ["bin"; "temp"]
)

Target.create "CleanDocs" (fun _ ->
    Shell.cleanDirs ["docs/output"]
)

// --------------------------------------------------------------------------------------
// Compile Stanford.NLP.CoreNLP and build NuGet package

let coreNLPDir = root </> "data/paket-files/nlp.stanford.edu/stanford-corenlp-4.0.0"

Target.create "CompilerCoreNLP" (fun _ ->
    let ikvmDir  = @"bin/Stanford.NLP.CoreNLP/lib"
    Shell.mkdir ikvmDir

    coreNLPDir </> "stanford-corenlp-4.0.0-models.jar"
    |> restoreFolderFromFile (Path.Combine(coreNLPDir, "models"))

    let jodaTime = IKVMcTask(coreNLPDir </> "joda-time.jar", version="2.10.5")
    [IKVMcTask(coreNLPDir </> "stanford-corenlp-4.0.0.jar", version=release.AssemblyVersion,
           Dependencies = [jodaTime
                           IKVMcTask(coreNLPDir </> "jollyday.jar", version="0.4.9", Dependencies =[jodaTime])
                           IKVMcTask(coreNLPDir </> "ejml-core-0.38.jar", version="0.38")
                           IKVMcTask(coreNLPDir </> "xom.jar", version="1.3.2")
                           IKVMcTask(coreNLPDir </> "javax.json.jar", version="1.0.4")
                           IKVMcTask(coreNLPDir </> "slf4j-api.jar", version="1.7.2")
                           IKVMcTask(coreNLPDir </> "slf4j-simple.jar", version="1.7.2")
                           IKVMcTask(coreNLPDir </> "protobuf.jar", version="2.6.1")])]
    |> IKVMCompile ikvmDir keyFile
)

Target.create "NuGetCoreNLP" (fun _ ->
    root </> "nuget/Stanford.NLP.CoreNLP.template"
    |> createNuGetPackage
)

// --------------------------------------------------------------------------------------
// Compile Stanford.NLP.NET and build NuGet package

let nerDir = root </> "data/paket-files/nlp.stanford.edu/stanford-ner-4.0.0"

Target.create "CompilerNER" (fun _ ->
    let ikvmDir  = @"bin/Stanford.NLP.NER/lib"
    Shell.mkdir ikvmDir

    [IKVMcTask(nerDir </> "stanford-ner.jar", version=release.AssemblyVersion,
        Dependencies = [IKVMcTask(nerDir </> "lib/jollyday-0.4.9.jar", version="0.4.9",
                            Dependencies =[IKVMcTask(nerDir </> "lib/joda-time.jar", version="2.9.4")])]
     )
    ]|> IKVMCompile ikvmDir keyFile
)

Target.create "NuGetNER" (fun _ ->
    root </> "nuget/Stanford.NLP.NER.template"
    |> createNuGetPackage
)

// --------------------------------------------------------------------------------------
// Compile Stanford.NLP.Parser and build NuGet package

let parserDir = root </> "data/paket-files/nlp.stanford.edu/stanford-parser-4.0.0"

Target.create "CompilerParser" (fun _ ->
    let ikvmDir  = @"bin/Stanford.NLP.Parser/lib"
    Shell.mkdir ikvmDir

    restoreFolderFromFile (parserDir </> "models") (parserDir </> "stanford-parser-4.0.0-models.jar")
    [IKVMcTask(parserDir </> "stanford-parser.jar", version=release.AssemblyVersion,
           Dependencies = [IKVMcTask(parserDir </> "ejml-core-0.38.jar", version="0.38.0.0")
                           IKVMcTask(coreNLPDir </> "slf4j-api.jar", version="1.7.12")])]
    |> IKVMCompile ikvmDir keyFile
)

Target.create "NuGetParser" (fun _ ->
    root </> "nuget/Stanford.NLP.Parser.template"
    |> createNuGetPackage
)

// --------------------------------------------------------------------------------------
// Compile Stanford.NLP.POSTagger and build NuGet package

let posDir = root </> "data/paket-files/nlp.stanford.edu/stanford-tagger-4.0.0"

Target.create "CompilerPOS" (fun _ ->
    let ikvmDir  = @"bin/Stanford.NLP.POSTagger/lib"
    Shell.mkdir ikvmDir

    [IKVMcTask(posDir </> "stanford-postagger-4.0.0.jar", version=release.AssemblyVersion,
        Dependencies = [])]
    |> IKVMCompile ikvmDir keyFile
)

Target.create "NuGetPOS" (fun _ ->
    root </> "nuget/Stanford.NLP.POSTagger.template"
    |> createNuGetPackage
)

// --------------------------------------------------------------------------------------
// Compile Stanford.NLP.Segmenter and build NuGet package

let segmenterDir = root </> "data/paket-files/nlp.stanford.edu/stanford-segmenter-4.0.0"

Target.create "CompilerSegmenter" (fun _ ->
    let ikvmDir  = @"bin/Stanford.NLP.Segmenter/lib"
    Shell.mkdir ikvmDir

    [IKVMcTask(segmenterDir </> "stanford-segmenter-4.0.0.jar", version=release.AssemblyVersion,
        Dependencies=[])]
    |> IKVMCompile ikvmDir keyFile
)

Target.create "NuGetSegmenter" (fun _ ->
    root </> "nuget/Stanford.NLP.Segmenter.template"
    |> createNuGetPackage
)


// --------------------------------------------------------------------------------------
// Build and run test projects

open Fake.DotNet

Target.create "BuildTests" (fun _ ->
    DotNet.exec id "build" "Stanford.NLP.NET.sln -c Release" |> ignore
    // !! solutionFile
    // |> MSBuildRelease "" "Rebuild"
    // |> ignore
)

Target.create "RunTests" (fun _ ->
    !! "tests/**/bin/Release/net461/*Tests.exe"
    |> Seq.iter (fun path ->
        Trace.tracefn "Running tests '%s' ..." path
        
        let args = "--fail-on-focused-tests --summary --sequenced --version"
        (if Environment.isWindows
         then CreateProcess.fromRawCommandLine path args
         else CreateProcess.fromRawCommandLine "mono" (path + " " + args))
        |> CreateProcess.ensureExitCode
        |> Proc.run
        |> ignore
    )
    // |> Testing.Expecto.run (fun p ->
    //     { p with
    //         WorkingDirectory = __SOURCE_DIRECTORY__
    //         FailOnFocusedTests = true
    //         PrintVersion = true
    //         Parallel = false
    //         Summary =  true
    //         Debug = false
    //     })
    // |> Expecto (fun p ->
    //     { p with
    //         Parallel = false } )
    // |> ignore
)

// --------------------------------------------------------------------------------------
// Generate the documentation

module Fake =
    let fakePath = "packages" </> "FAKE" </> "tools" </> "FAKE.exe"
    let fakeStartInfo script workingDirectory args fsiargs environmentVars =
        (fun (info: System.Diagnostics.ProcessStartInfo) ->
            info.FileName <- System.IO.Path.GetFullPath fakePath
            info.Arguments <- sprintf "%s --fsiargs -d:FAKE %s \"%s\"" args fsiargs script
            info.WorkingDirectory <- workingDirectory
            let setVar k v = info.EnvironmentVariables.[k] <- v
            for (k, v) in environmentVars do setVar k v
            //setVar "MSBuild" msBuildExe
            setVar "GIT" Git.CommandHelper.gitPath
            //setVar "FSI" fsiPath
            )

    /// Run the given buildscript with FAKE.exe
    let executeFAKEWithOutput workingDirectory script fsiargs envArgs =
        let exitCode = 0
            //ExecProcessWithLambdas
            //    (fakeStartInfo script workingDirectory "" fsiargs envArgs)
            //    TimeSpan.MaxValue false ignore ignore
        System.Threading.Thread.Sleep 1000
        exitCode

Target.create "BrowseDocs" (fun _ ->
    let exit = Fake.executeFAKEWithOutput "docs" "docs.fsx" "" ["target", "BrowseDocs"]
    if exit <> 0 then failwith "Browsing documentation failed"
)

Target.create "GenerateDocs" (fun _ ->
    let exit = Fake.executeFAKEWithOutput "docs" "docs.fsx" "" ["target", "GenerateDocs"]
    if exit <> 0 then failwith "Generating documentation failed"
)

Target.create "PublishDocs" (fun _ ->
    let exit = Fake.executeFAKEWithOutput "docs" "docs.fsx" "" ["target", "PublishDocs"]
    if exit <> 0 then failwith "Publishing documentation failed"
)

Target.create "PublishStaticPages" (fun _ ->
    let exit = Fake.executeFAKEWithOutput "docs" "docs.fsx" "" ["target", "PublishStaticPages"]
    if exit <> 0 then failwith "Publishing documentation failed"
)

// --------------------------------------------------------------------------------------
// Run all targets by default. Invoke 'build <Target>' to override

Target.create "All" ignore
Target.create "NuGet" ignore

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
  ==> "All"

Target.runOrDefault "All"
