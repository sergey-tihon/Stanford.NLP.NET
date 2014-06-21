[<AutoOpen>]
module Stanford.NLP.CoreNLP.Config

let [<Literal>] jarRoot = __SOURCE_DIRECTORY__ + @"..\..\..\src\temp\stanford-corenlp-full-2014-06-16\stanford-corenlp-3.4-models\"
let [<Literal>] modelsDirectry = jarRoot + @"edu\stanford\nlp\models\"
type Models = FSharp.Management.FileSystem<path=modelsDirectry>