[<AutoOpen>]
module Stanford.NLP.POSTagger.Config

let [<Literal>] modelsDirectry = __SOURCE_DIRECTORY__ + @"..\..\..\data\paket-files\nlp.stanford.edu\stanford-postagger-full-2018-02-27\models\"
type Models = FSharp.Management.FileSystem<path=modelsDirectry>

let [<Literal>] dataFilesRoot  = __SOURCE_DIRECTORY__ + @"..\..\data"
type DataFiles = FSharp.Management.FileSystem<dataFilesRoot>

let tagger =
    let model = Models.``wsj-0-18-bidirectional-nodistsim.tagger``
    let tagger = edu.stanford.nlp.tagger.maxent.MaxentTagger(model)
    Expecto.Expect.isNotNull tagger "Tagger is null"
    tagger