// include Fake lib
#load @".\Core.fsx"
open Fake 
open Fake.AssemblyInfoFile
open Fake.IKVM.Helpers

// Assembly / NuGet package properties
let projectName = "Stanford CoreNLP"
let projectDescription = "Stanford CoreNLP provides a set of natural language analysis tools which can take raw English language text input and give the base forms of words, their parts of speech, whether they are names of companies, people, etc., normalize dates, times, and numeric quantities, and mark up the structure of sentences in terms of phrases and word dependencies, and indicate which noun phrases refer to the same entities. Stanford CoreNLP is an integrated framework, which make it very easy to apply a bunch of language analysis tools to a piece of text. Starting from plain text, you can run all the tools on it with just two lines of code. Its analyses provide the foundational building blocks for higher-level and domain-specific text understanding applications."

// Targets

// Run IKVM compiler
Target "RunIKVMCompiler" (fun _ ->
    restoreFolderFromUrl 
        @".\temp\stanford-corenlp-full-2013-11-12" 
        "http://nlp.stanford.edu/software/stanford-corenlp-full-2013-11-12.zip"
    [IKVMcTask(@"temp\stanford-corenlp-full-2013-11-12\stanford-corenlp-3.3.0.jar", Version=version,
           Dependencies = [IKVMcTask(@"temp\stanford-corenlp-full-2013-11-12\joda-time.jar", Version="2.1");
                           IKVMcTask(@"temp\stanford-corenlp-full-2013-11-12\jollyday.jar", Version="0.4.7",
                                Dependencies =[IKVMcTask(@"temp\stanford-corenlp-full-2013-11-12\joda-time.jar", Version="2.1")]);
                           IKVMcTask(@"temp\stanford-corenlp-full-2013-11-12\ejml-0.23.jar", Version="0.23");
                           IKVMcTask(@"temp\stanford-corenlp-full-2013-11-12\xom.jar", Version="1.2.8");])]
    |> IKVMCompile ikvmDir @".\Stanford.NLP.snk"
)

// Create NuGet package
Target "CreateNuGet" (fun _ ->     
    copyFilesToNugetFolder()
        
    "Stanford.NLP.CoreNLP.nuspec"
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