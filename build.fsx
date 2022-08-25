#r @"paket:
source https://api.nuget.org/v3/index.json
framework: net6.0
storage: none
nuget FSharp.Core
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
nuget Fake.Tools.Git
nuget Fake.Api.GitHub //"

#if !FAKE
#load "./.fake/build.fsx/intellisense.fsx"
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

Target.initEnvironment ()

// --------------------------------------------------------------------------------------
// FAKE build script
// --------------------------------------------------------------------------------------

[<Literal>]
let root = __SOURCE_DIRECTORY__
//Environment.CurrentDirectory <- RootFolder

// Read additional information from the release notes document
let release = ReleaseNotes.load "RELEASE_NOTES.md"

// --------------------------------------------------------------------------------------
// IKVM.NET compilation helpers

type TargetRuntime =
    | Net_461
    | NetCore_3_1
    override this.ToString() =
        match this with
        | Net_461 -> "net461"
        | NetCore_3_1 -> "netcoreapp3.1"

let frameworks = [ Net_461; NetCore_3_1 ]

// Location of IKVM Compiler
let ikvmRootFolder = root </> "data/paket-files/github.com"

let getIkmvcFolder =
    function
    | Net_461 -> ikvmRootFolder </> "tools-ikvmc-net461/any"
    | NetCore_3_1 ->
        ikvmRootFolder
        </> "tools-ikvmc-netcoreapp3.1/win7-x64"

let getIkvmRuntimeFolder =
    function
    | Net_461 -> ikvmRootFolder </> "bin-net461"
    | NetCore_3_1 -> ikvmRootFolder </> "bin-netcoreapp3.1"

let fixFileNames path =
    use file = File.Open(path, FileMode.Open, FileAccess.ReadWrite)
    use archive = new ZipArchive(file, ZipArchiveMode.Update)

    archive.Entries
    |> Seq.toList
    |> List.filter (fun x -> x.FullName.Contains(":"))
    |> List.iter (fun entry ->
        printfn "%s " entry.FullName
        let newName = entry.FullName.Replace(":", "_")
        let newEntry = archive.CreateEntry(newName)

        (use a = entry.Open()
         use b = newEntry.Open()
         a.CopyTo(b))

        entry.Delete())

let unZipTo toDir file =
    Trace.trace "Renaming files inside zip archive ..."
    fixFileNames file
    Trace.tracefn "Unzipping file '%s' to '%s'" file toDir
    Compression.ZipFile.ExtractToDirectory(file, toDir)

let restoreFolderFromFile folder zipFile =
    if not <| Directory.Exists folder then
        zipFile |> unZipTo folder

type IKVMcTask(jar: string, version: string) =
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

let IKVMCompile framework workingDirectory keyFile tasks =
    let ikvmcExe = (getIkmvcFolder framework) </> "ikvmc.exe"

    let ikvmc args =
        CreateProcess.fromRawCommandLine ikvmcExe args
        |> CreateProcess.withWorkingDirectory (DirectoryInfo(workingDirectory).FullName)
        |> CreateProcess.withTimeout timeOut
        |> CreateProcess.ensureExitCode
        |> Proc.run
        |> ignore
    let newKeyFile =
        if (File.Exists keyFile) then
            let file = workingDirectory @@ (Path.GetFileName(keyFile))
            File.Copy(keyFile, file, true)
            Path.GetFileName(file)
        else
            keyFile

    let bprintf = Microsoft.FSharp.Core.Printf.bprintf
    let cache = System.Collections.Generic.HashSet<_>()

    let rec compile (task: IKVMcTask) =
        let getIKVMCommandLineArgs () =
            let dependencies =
                task.Dependencies
                |> Seq.collect (fun x ->
                    compile x
                    x.GetDllReferences())
                |> Seq.distinct

            let sb = Text.StringBuilder()

            if framework = NetCore_3_1 then
                bprintf sb " -nostdlib"

                !!(sprintf "%s/refs/*.dll" (getIkmvcFolder framework))
                |> Seq.iter (fun lib -> bprintf sb " -r:%s" lib)

            let runtime = getIkvmRuntimeFolder framework
            bprintf sb " -runtime:%s/IKVM.Runtime.dll" runtime

            dependencies |> Seq.iter (bprintf sb " -r:%s")

            if not <| String.IsNullOrEmpty(task.Version) then
                task.Version |> bprintf sb " -version:%s"

            bprintf sb " %s -out:%s" task.JarFile task.DllFile
            sb.ToString()

        if cache.Contains task.JarFile then
            Trace.tracefn "Task '%s' already compiled" task.JarFile
        else
            ikvmc <| getIKVMCommandLineArgs ()

            if (File.Exists(newKeyFile)) then
                let key = FileInfo(newKeyFile).FullName |> File.ReadAllBytes

                ModuleDefinition
                    .ReadModule(task.DllFile, ReaderParameters(InMemory = true))
                    .Write(task.DllFile, WriterParameters(StrongNameKeyBlob = key))

            cache.Add(task.JarFile) |> ignore

    tasks |> Seq.iter compile

let copyPackages fromDir toDir =
    if (not <| Directory.Exists(toDir)) then
        Directory.CreateDirectory(toDir) |> ignore

    Directory.GetFiles(fromDir)
    |> Seq.filter (fun x -> Path.GetExtension(x) = ".nupkg")
    |> Seq.iter (fun x -> File.Copy(x, Path.Combine(toDir, Path.GetFileName(x)), true))

let createNuGetPackage version template =
    Fake.DotNet.Paket.pack (fun p ->
        { p with
            ToolType = Fake.DotNet.ToolType.CreateLocalTool()
            TemplateFile = template
            OutputPath = "bin"
            Version = version
            ReleaseNotes = String.toLines release.Notes })

let keyFile = @"nuget/Stanford.NLP.snk"

// --------------------------------------------------------------------------------------
// Clean build results

Target.create "Clean" (fun _ -> Shell.cleanDirs [ "bin"; "temp" ])

Target.create "CleanDocs" (fun _ -> Shell.cleanDirs [ "docs/output" ])

// --------------------------------------------------------------------------------------
// Compile Stanford.NLP.CoreNLP and build NuGet package

let coreNLPDir =
    root
    </> "data/paket-files/nlp.stanford.edu/stanford-corenlp-4.5.0"

Target.create "CompilerCoreNLP" (fun _ ->
    coreNLPDir </> "stanford-corenlp-4.5.0-models.jar"
    |> restoreFolderFromFile (Path.Combine(coreNLPDir, "models"))

    let jodaTime = IKVMcTask(coreNLPDir </> "joda-time.jar", version = "2.10.5")
    let ejmlCore = IKVMcTask(coreNLPDir </> "ejml-core-0.39.jar", version = "0.39")

    let ejmlDdense =
        IKVMcTask(coreNLPDir </> "ejml-ddense-0.39.jar", version = "0.39", Dependencies = [ ejmlCore ])

    let sl4japi = IKVMcTask(coreNLPDir </> "slf4j-api.jar", version = "1.7.2")

    let tasks =
        [ IKVMcTask(
              coreNLPDir </> "stanford-corenlp-4.5.0.jar",
              version = release.AssemblyVersion,
              Dependencies =
                  [ jodaTime
                    IKVMcTask(coreNLPDir </> "jollyday.jar", version = "0.4.9", Dependencies = [ jodaTime ])
                    ejmlCore
                    ejmlDdense
                    IKVMcTask(
                        coreNLPDir </> "ejml-simple-0.39.jar",
                        version = "0.39",
                        Dependencies = [ ejmlDdense; ejmlCore ]
                    )
                    IKVMcTask(coreNLPDir </> "xom.jar", version = "1.3.2")
                    IKVMcTask(coreNLPDir </> "javax.json.jar", version = "1.0.4")
                    sl4japi
                    IKVMcTask(coreNLPDir </> "slf4j-simple.jar", version = "1.7.2", Dependencies = [ sl4japi ])
                    IKVMcTask(coreNLPDir </> "protobuf-java-3.19.2.jar", version = "3.19.2") ]
          ) ]

    for framework in frameworks do
        let ikvmDir = $"bin/Stanford.NLP.CoreNLP/lib/{framework}"
        Shell.mkdir ikvmDir
        IKVMCompile framework ikvmDir keyFile tasks)

Target.create "NuGetCoreNLP" (fun _ ->
    root </> "nuget/Stanford.NLP.CoreNLP.template"
    |> createNuGetPackage release.NugetVersion)

// --------------------------------------------------------------------------------------
// Compile Stanford.NLP.NET and build NuGet package

let toolsVersion = "4.2.0.2"
let nerDir =
    root
    </> "data/paket-files/nlp.stanford.edu/stanford-ner-2020-11-17"

Target.create "CompilerNER" (fun _ ->
    let tasks =
        [ IKVMcTask(
              nerDir </> "stanford-ner.jar",
              version = toolsVersion,
              Dependencies =
                  [ IKVMcTask(
                        nerDir </> "lib/jollyday-0.4.9.jar",
                        version = "0.4.9",
                        Dependencies = [ IKVMcTask(nerDir </> "lib/joda-time.jar", version = "2.9.4") ]
                    ) ]
          ) ]

    for framework in frameworks do
        let ikvmDir = $"bin/Stanford.NLP.NER/lib/{framework}"
        Shell.mkdir ikvmDir
        IKVMCompile framework ikvmDir keyFile tasks)

Target.create "NuGetNER" (fun _ ->
    root </> "nuget/Stanford.NLP.NER.template"
    |> createNuGetPackage toolsVersion)

// --------------------------------------------------------------------------------------
// Compile Stanford.NLP.Parser and build NuGet package

let parserDir =
    root
    </> "data/paket-files/nlp.stanford.edu/stanford-parser-full-2020-11-17"

Target.create "CompilerParser" (fun _ ->
    let tasks =
        [ IKVMcTask(
              parserDir </> "stanford-parser.jar",
              version = toolsVersion,
              Dependencies =
                  [ IKVMcTask(parserDir </> "ejml-core-0.38.jar", version = "0.38.0.0")
                    IKVMcTask(coreNLPDir </> "slf4j-api.jar", version = "1.7.12") ]
          ) ]

    for framework in frameworks do
        let ikvmDir = $"bin/Stanford.NLP.Parser/lib/{framework}"
        Shell.mkdir ikvmDir
        IKVMCompile framework ikvmDir keyFile tasks)

Target.create "NuGetParser" (fun _ ->
    root </> "nuget/Stanford.NLP.Parser.template"
    |> createNuGetPackage toolsVersion)

// --------------------------------------------------------------------------------------
// Compile Stanford.NLP.POSTagger and build NuGet package

let posDir =
    root
    </> "data/paket-files/nlp.stanford.edu/stanford-postagger-full-2020-11-17"

Target.create "CompilerPOS" (fun _ ->
    let tasks =
        [ IKVMcTask(posDir </> "stanford-postagger-4.2.0.jar", version = toolsVersion, Dependencies = []) ]

    for framework in frameworks do
        let ikvmDir = $"bin/Stanford.NLP.POSTagger/lib/{framework}"
        Shell.mkdir ikvmDir
        IKVMCompile framework ikvmDir keyFile tasks)

Target.create "NuGetPOS" (fun _ ->
    root </> "nuget/Stanford.NLP.POSTagger.template"
    |> createNuGetPackage toolsVersion)

// --------------------------------------------------------------------------------------
// Compile Stanford.NLP.Segmenter and build NuGet package

let segmenterDir =
    root
    </> "data/paket-files/nlp.stanford.edu/stanford-segmenter-2020-11-17"

Target.create "CompilerSegmenter" (fun _ ->
    let tasks =
        [ IKVMcTask(
              segmenterDir </> "stanford-segmenter-4.2.0.jar",
              version = toolsVersion,
              Dependencies = []
          ) ]

    for framework in frameworks do
        let ikvmDir = $"bin/Stanford.NLP.Segmenter/lib/{framework}"
        Shell.mkdir ikvmDir
        IKVMCompile framework ikvmDir keyFile tasks)

Target.create "NuGetSegmenter" (fun _ ->
    root </> "nuget/Stanford.NLP.Segmenter.template"
    |> createNuGetPackage toolsVersion)


// --------------------------------------------------------------------------------------
// Build and run test projects

let dotnet cmd args =
    let result = DotNet.exec id cmd args
    if not result.OK then
        failwithf "Failed: %A" result.Errors

Target.create "BuildTests" (fun _ -> dotnet "build" "Stanford.NLP.NET.sln -c Release")

Target.create "RunTests" (fun _ ->
    for framework in frameworks do

        !! $"tests/**/bin/Release/{framework}/*.Tests.dll"
        |> Seq.iter (fun lib ->
            let logFileName =
                $"""${framework}-{Path.GetFileNameWithoutExtension(lib)}-TestResults.trx"""

            dotnet "test" $"{lib} --logger:\"console;verbosity=normal\" --logger:\"trx;LogFileName={logFileName}\""))

// --------------------------------------------------------------------------------------
// Generate the documentation

Target.create "BrowseDocs" (fun _ ->
    CreateProcess.fromRawCommandLine "dotnet" "serve -o -d ./docs"
    |> (Proc.run >> ignore))

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

"NuGet" ==> "BuildTests" ==> "RunTests" ==> "All"

Target.runOrDefault "All"
