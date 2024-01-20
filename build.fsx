#r "nuget: Fun.Build, 1.0.5"

open Fun.Build
open System.IO
open System.IO.Compression

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

let restoreFolderFromFile folder zipFile =
    if not <| Directory.Exists folder then
        fixFileNames zipFile
        Compression.ZipFile.ExtractToDirectory(zipFile, folder)


pipeline "build" {
    workingDir __SOURCE_DIRECTORY__

    runBeforeEachStage (fun ctx ->
        if ctx.GetStageLevel() = 0 then
            printfn $"::group::{ctx.Name}")

    runAfterEachStage (fun ctx ->
        if ctx.GetStageLevel() = 0 then
            printfn "::endgroup::")

    stage "Pre-Build" {
        run "dotnet tool restore"

        stage "Restore Dependencies" {
            workingDir "./data"
            run "dotnet paket restore"

            run (fun ctx ->
                let coreNLPDir =
                    Path.Combine(__SOURCE_DIRECTORY__, "data/paket-files/nlp.stanford.edu/stanford-corenlp-4.5.5")

                Path.Combine(coreNLPDir, "stanford-corenlp-4.5.5-models.jar")
                |> restoreFolderFromFile (Path.Combine(coreNLPDir, "models")))
        }
    }

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
