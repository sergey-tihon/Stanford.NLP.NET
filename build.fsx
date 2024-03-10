#r "nuget: Fun.Build, 1.0.5"

open Fun.Build

pipeline "build" {
    workingDir __SOURCE_DIRECTORY__

    runBeforeEachStage (fun ctx ->
        if ctx.GetStageLevel() = 0 then
            printfn $"::group::{ctx.Name}")

    runAfterEachStage (fun ctx ->
        if ctx.GetStageLevel() = 0 then
            printfn "::endgroup::")

    stage "Restore Tools" { run "dotnet tool restore" }

    stage "Build" { run "dotnet build Stanford.NLP.NET.sln -c Release" }

    stage "Test" { run "dotnet test Stanford.NLP.NET.sln -c Release" }

    runIfOnlySpecified
}

pipeline "docs" {
    workingDir __SOURCE_DIRECTORY__

    stage "Build" {
        run "rm -rf ./docs/output"
        run "dotnet serve -o -d=./docs"
    }

    runIfOnlySpecified
}

tryPrintPipelineCommandHelp ()
