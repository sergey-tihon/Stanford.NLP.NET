[<AutoOpen>]
module Stanford.NLP.CoreNLP.Config

let [<Literal>] jarRoot = __SOURCE_DIRECTORY__ + @"..\..\..\src\temp\stanford-corenlp-full-2014-10-31\stanford-corenlp-3.5.0-models\"
let [<Literal>] modelsDirectry = jarRoot + @"edu\stanford\nlp\models\"
type Models = FSharp.Management.FileSystem<path=modelsDirectry>