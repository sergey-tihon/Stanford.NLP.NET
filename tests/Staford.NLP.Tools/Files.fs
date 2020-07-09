[<AutoOpen>]
module Stanford.NLP.Tools.Files

open System.IO

[<AutoOpen>]
module Common =
    let getFullPath path =
        if File.Exists path then FileInfo(path).FullName
        elif Directory.Exists path then DirectoryInfo(path).FullName
        else failwithf "File or directory %s does not exist" path
    let combine paths = Path.Combine(Array.ofList paths) |> getFullPath

    let nlpStanford = combine [__SOURCE_DIRECTORY__; "../../data/paket-files/nlp.stanford.edu/" ]
    let dataFile path = combine [__SOURCE_DIRECTORY__; "../data/"; path]

module CoreNLP =
    let jarRoot = combine [nlpStanford; "stanford-corenlp-4.0.0/models/"]
    let models path = combine [jarRoot; "edu/stanford/nlp/models/"; path]

module NER =
    let classifier path = combine [nlpStanford; "stanford-ner-4.0.0/classifiers/"; path]

module Parser =
    let models path = combine [nlpStanford; "stanford-parser-4.0.0/models/edu/stanford/nlp/models/"; path]

module Tagger =
    let model path = combine [nlpStanford; "stanford-tagger-4.0.0/models/"; path]

module Segmenter =
    let root = combine [nlpStanford; "stanford-segmenter-4.0.0/data/"]
    let data path = combine [root; path]
