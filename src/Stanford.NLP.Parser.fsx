// include Fake lib
#load @".\Core.fsx"
open Fake 
open Fake.AssemblyInfoFile
open Fake.IKVM.Helpers

// Assembly / NuGet package properties
let projectName = "Stanford Parser"
let projectDescription = "A natural language parser is a program that works out the grammatical structure of sentences, for instance, which groups of words go together (as \"phrases\") and which words are the subject or object of a verb. Probabilistic parsers use knowledge of language gained from hand-parsed sentences to try to produce the most likely analysis of new sentences."

// Targets

// Run IKVM compiler
Target "RunIKVMCompiler" (fun _ ->
    restoreFolderFromUrl 
        @".\temp\stanford-parser-full-2013-11-12" 
        "http://nlp.stanford.edu/software/stanford-parser-full-2013-11-12.zip"
    restoreFolderFromFile
        @".\temp\stanford-parser-full-2013-11-12\edu" 
        @".\temp\stanford-parser-full-2013-11-12\stanford-parser-3.3.0-models.jar"
    [IKVMcTask(@"temp\stanford-parser-full-2013-11-12\stanford-parser.jar", Version=version,
           Dependencies = [IKVMcTask(@"temp\stanford-parser-full-2013-11-12\ejml-0.23.jar", Version="0.23.0.0")])]
    |> IKVMCompile ikvmDir @".\Stanford.NLP.snk"
)

// Create NuGet package
Target "CreateNuGet" (fun _ ->     
    copyFilesToNugetFolder()
        
    "Stanford.NLP.Parser.nuspec"
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