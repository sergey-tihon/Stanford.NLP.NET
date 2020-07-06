[<AutoOpen>]
module Stanford.NLP.POSTagger.Config

let inline (</>) path1 path2 = System.IO.Path.Combine(path1, path2)
let model path = __SOURCE_DIRECTORY__ </> "/../../data/paket-files/nlp.stanford.edu/stanford-tagger-4.0.0/models/" </> path
let dataFile path = __SOURCE_DIRECTORY__ </> "/../data/" </> path

let tagger =
    let model = model "english-bidirectional-distsim.tagger"
    let tagger = edu.stanford.nlp.tagger.maxent.MaxentTagger(model)
    Expecto.Expect.isNotNull tagger "Tagger is null"
    tagger
