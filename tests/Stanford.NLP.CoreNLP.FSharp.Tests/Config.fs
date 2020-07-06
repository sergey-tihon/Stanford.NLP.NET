[<AutoOpen>]
module Stanford.NLP.CoreNLP.Config

let [<Literal>] jarRoot = __SOURCE_DIRECTORY__ + @"/../../data/paket-files/nlp.stanford.edu/stanford-corenlp-4.0.0/models/"
let [<Literal>] modelsDirectry = jarRoot + @"/edu/stanford/nlp/models/"
type Models = FSharp.Management.FileSystem<path=modelsDirectry>
