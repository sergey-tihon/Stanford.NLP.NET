[<AutoOpen>]
module Stanford.NLP.CoreNLP.Config

let inline (</>) path1 path2 = System.IO.Path.Combine(path1, path2)
let [<Literal>] jarRoot = __SOURCE_DIRECTORY__ + "/../../data/paket-files/nlp.stanford.edu/stanford-corenlp-4.0.0/models/"
let model path = jarRoot </> "/edu/stanford/nlp/models/" </> path
