[<AutoOpen>]
module Stanford.NLP.POSTagger.Config

let [<Literal>] modelsDirectry = __SOURCE_DIRECTORY__ + @"..\..\..\src\temp\stanford-postagger-full-2014-10-26\models\"
type Models = FSharp.Management.FileSystem<path=modelsDirectry>

let [<Literal>] dataFilesRoot  = __SOURCE_DIRECTORY__ + @"..\..\data"
type DataFiles = FSharp.Management.FileSystem<dataFilesRoot>

let tagger = 
    edu.stanford.nlp.tagger.maxent.MaxentTagger(Models.``wsj-0-18-bidirectional-nodistsim.tagger``)

open FsUnit
tagger |> should not' (be Null)