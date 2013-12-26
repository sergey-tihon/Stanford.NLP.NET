// include Fake lib
#load @".\Core.fsx"
open Fake 
open Fake.AssemblyInfoFile
open Fake.IKVM.Helpers

// Assembly / NuGet package properties
let projectName = "Stanford Named Entity Recognizer (NER)"
let projectDescription = "Stanford NER (also known as CRFClassifier) is a Java implementation of a Named Entity Recognizer. Named Entity Recognition (NER) labels sequences of words in a text which are the names of things, such as person and company names, or gene and protein names. The software provides a general (arbitrary order) implementation of linear chain Conditional Random Field (CRF) sequence models, coupled with well-engineered feature extractors for Named Entity Recognition."

// Targets

// Run IKVM compiler
Target "RunIKVMCompiler" (fun _ ->
    restoreFolderFromUrl 
        @".\temp\stanford-ner-2013-11-12" 
        "http://nlp.stanford.edu/software/stanford-ner-2013-11-12.zip"
    [IKVMcTask(@"temp\stanford-ner-2013-11-12\stanford-ner.jar", Version=version)]
    |> IKVMCompile ikvmDir @".\Stanford.NLP.snk"
)

// Create NuGet package
Target "CreateNuGet" (fun _ ->     
    copyFilesToNugetFolder()
        
    "Stanford.NLP.NER.nuspec"
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