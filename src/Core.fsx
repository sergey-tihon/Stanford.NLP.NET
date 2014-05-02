module Fake.IKVM.Helpers

#r @"..\packages\FAKE\tools\FakeLib.dll"
#r "System.IO.Compression.FileSystem.dll"

open Microsoft.FSharp.Core.Printf
open System
open System.IO

open Fake 

// ================== Download manager =====================================

let downloadDir = @".\Download\"

let restoreFile url = 
    let downloadFile file url = 
        printfn "Downloading file '%s' to '%s'..." url file
        let BUFFER_SIZE = 16*1024
        use outputFileStream = File.Create(file, BUFFER_SIZE)
        let req = System.Net.WebRequest.Create(url)
        use response = req.GetResponse()
        use responseStream = response.GetResponseStream()
        let printStep = 100L*1024L
        let buffer = Array.create<byte> BUFFER_SIZE 0uy
        let rec download downloadedBytes =  
            let bytesRead = responseStream.Read(buffer, 0, BUFFER_SIZE)
            outputFileStream.Write(buffer, 0, bytesRead)
            if (downloadedBytes/printStep <> (downloadedBytes-int64(bytesRead))/printStep)
                then printfn "\tDownloaded '%d' bytes" downloadedBytes
            if (bytesRead > 0) then download (downloadedBytes + int64(bytesRead))
        download 0L
    let file = downloadDir @@ System.IO.Path.GetFileName(url)
    if (not <| File.Exists(file))
        then url |> downloadFile file
    file 

let unZipTo toDir file =
    printfn "Unzipping file '%s' to '%s'" file toDir
    Compression.ZipFile.ExtractToDirectory(file, toDir)

let restoreFolderFromUrl folder url =
    if not <| Directory.Exists folder then
        url |> restoreFile |> unZipTo (folder @@ @"..\")
        
let restoreFolderFromFile folder zipFile =
    if not <| Directory.Exists folder then
        zipFile |> unZipTo (folder @@ @"..\")
        
// ================== IKVM Compiler=== =====================================

let ikvmc = 
    restoreFolderFromUrl @".\temp\ikvm-7.4.5196.0" "http://www.frijters.net/ikvmbin-7.4.5196.0.zip"
    @".\temp\ikvm-7.4.5196.0\bin\ikvmc.exe"

let ildasm = @"c:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\x64\ildasm.exe"
let ilasm =  @"c:\Windows\Microsoft.NET\Framework64\v2.0.50727\ilasm.exe" 

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
        let file = workingDirectory @@ (Path.GetFileName(keyFile))
        File.Copy(keyFile, file, true)
        Path.GetFileName(file)
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
        if (File.Exists(keyFile)) then
            let dllFile = task.JarFile |> getNewFileName ".dll"
            let ilFile  = task.JarFile |> getNewFileName ".il"
            startProcess ildasm (sprintf " /all /out=%s %s" ilFile dllFile)
            File.Delete(dllFile)
            startProcess ilasm  (sprintf " /dll /key=%s %s" (newKeyFile) ilFile)
    tasks |> Seq.iter compile
    
// ================== Fake tasks ===========================================
let copyPackages fromDir toDir = 
    if (not <| Directory.Exists(toDir))
        then Directory.CreateDirectory(toDir) |> ignore
    Directory.GetFiles(fromDir)
    |> Seq.filter (fun x -> Path.GetExtension(x) = ".nupkg")
    |> Seq.iter   (fun x -> File.Copy(x, Path.Combine(toDir, Path.GetFileName(x)), true))

    
let version = "3.3.1.1"
let authors = ["The Stanford Natural Language Processing Group"]

// Folders
let ikvmDir = @".\temp\ikvm\"
let nugetDir = @".\temp\nuget\"
let targetDir = @".\packages\"

// Targets
// Clean IKVM working directory
Target "CleanIKVM" (fun _ ->
    CleanDir ikvmDir
)

// Clean NuGet directory
Target "CleanNuGet" (fun _ ->
    CleanDir nugetDir
)

let copyFilesToNugetFolder() =
    XCopy ikvmDir (nugetDir @@ "lib")
    !+ @"temp/nuget/lib/*.*"
        -- @"temp/nuget/lib/*.dll"
        |> ScanImmediately
        |> Seq.iter (System.IO.File.Delete)    
        
// Default target
Target "Default" (fun _ ->
    copyPackages nugetDir targetDir
    trace "Building The Stanford Parser"
)