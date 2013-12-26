// include Fake lib
#load @".\Core.fsx"
open Fake 
open Fake.AssemblyInfoFile
open Fake.IKVM.Helpers

// Assembly / NuGet package properties
let projectName = "Stanford Word Segmenter"
let projectDescription = "Tokenization of raw text is a standard pre-processing step for many NLP tasks. For English, tokenization usually involves punctuation splitting and separation of some affixes like possessives. Other languages require more extensive token pre-processing, which is usually called segmentation."

// Targets

// Run IKVM compiler
Target "RunIKVMCompiler" (fun _ ->
    restoreFolderFromUrl 
        @".\temp\stanford-segmenter-2013-11-12" 
        "http://nlp.stanford.edu/software/stanford-segmenter-2013-11-12.zip"
    [IKVMcTask(@"temp\stanford-segmenter-2013-11-12\seg.jar", Version=version)]
    |> IKVMCompile ikvmDir @".\Stanford.NLP.snk"
)

// Create NuGet package
Target "CreateNuGet" (fun _ ->     
    copyFilesToNugetFolder()
        
    "Stanford.NLP.Segmenter.nuspec"
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