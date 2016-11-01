[<AutoOpen>]
module Stanford.NLP.CoreNLP.Config

let [<Literal>] jarRoot = __SOURCE_DIRECTORY__ + @"..\..\..\paket-files\nlp.stanford.edu\stanford-corenlp-full-2016-10-31\models\"
let [<Literal>] modelsDirectry = jarRoot + @"edu\stanford\nlp\models\"
type Models = FSharp.Management.FileSystem<path=modelsDirectry>