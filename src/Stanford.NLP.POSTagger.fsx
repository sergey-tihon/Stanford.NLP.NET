// include Fake lib
#load @".\Core.fsx"
open Fake 
open Fake.AssemblyInfoFile
open Fake.IKVM.Helpers

// Assembly / NuGet package properties
let projectName = "Stanford Log-linear Part-Of-Speech Tagger"
let projectDescription = "A Part-Of-Speech Tagger (POS Tagger) is a piece of software that reads text in some language and assigns parts of speech to each word (and other token), such as noun, verb, adjective, etc., although generally computational applications use more fine-grained POS tags like 'noun-plural'."

// Targets

// Run IKVM compiler
Target "RunIKVMCompiler" (fun _ ->
    restoreFolderFromUrl 
        @".\temp\stanford-postagger-full-2013-11-12" 
        "http://nlp.stanford.edu/software/stanford-postagger-full-2013-11-12.zip"
    [IKVMcTask(@"temp\stanford-postagger-full-2013-11-12\stanford-postagger.jar", Version=version)]
    |> IKVMCompile ikvmDir @".\Stanford.NLP.snk"
)

// Create NuGet package
Target "CreateNuGet" (fun _ ->     
    copyFilesToNugetFolder()
        
    "Stanford.NLP.POSTagger.nuspec"
      |> NuGet (fun p -> 
            {p with
                Project = projectName
                Authors = authors
                Version = version
                Description = projectDescription
                NoPackageAnalysis = true
                ToolPath = ".\..\.nuget\NuGet.exe"
                WorkingDir = nugetDir
                OutputPath = nugetDir })
)

// Dependencies
"CleanIKVM"
  ==> "RunIKVMCompiler"
  ==> "CleanNuGet"
  ==> "CreateNuGet"
  ==> "Default"

// start build
Run "Default"