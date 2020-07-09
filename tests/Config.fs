namespace Stanford.NLP.Config

open System.IO

[<AutoOpen>]
module Common =
    let inline (</>) path1 path2 = Path.Combine(path1, path2)
    let getFullPath path =
        if File.Exists path
        then FileInfo(path).FullName
        else DirectoryInfo(path).FullName

    let nlpStanford = __SOURCE_DIRECTORY__ </> "../data/paket-files/nlp.stanford.edu"
    let dataFile path = getFullPath <| (__SOURCE_DIRECTORY__ </> "data/" </> path)

module CoreNLP =
    let jarRoot = getFullPath <| (nlpStanford </> "stanford-corenlp-4.0.0/models/")
    let models path = getFullPath <| ( jarRoot </> "edu/stanford/nlp/models/" </> path)

module NER =
    let classifier path = getFullPath <| (nlpStanford </> "stanford-ner-4.0.0/classifiers/" </> path)

module Parser =
    let models path = getFullPath <| (nlpStanford </> "stanford-parser-4.0.0/models/edu/stanford/nlp/models/" </> path)

module Tagger =
    let model path = getFullPath <| (nlpStanford </> "stanford-tagger-4.0.0/models/" </> path)

module Segmenter =
    let root = getFullPath <| (nlpStanford </> "stanford-segmenter-4.0.0/data/")
    let data path = getFullPath <| ( root </> path)


module Java =
    open java.lang
    open java.util

    let toSeq (iter:Iterable) =
        let rec loop (x:Iterator) =
            seq {
                if x.hasNext() then
                    yield x.next()
                    yield! (loop x)
            }
        iter.iterator() |> loop |> Array.ofSeq |> Seq.readonly

    let props (values) =
        let props = Properties()
        for (key, value) in values do
            props.setProperty(key, value) |> ignore
        props
