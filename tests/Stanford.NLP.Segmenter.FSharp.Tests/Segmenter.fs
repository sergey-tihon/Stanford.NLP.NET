module Stanford.NLP.Segmenter.FSharp.Tests

open Expecto
open java.util
open edu.stanford.nlp.ie.crf

let inline (</>) path1 path2 = System.IO.Path.Combine(path1, path2)
let segmenterData path = __SOURCE_DIRECTORY__ </> "/../../data/paket-files/nlp.stanford.edu/stanford-segmenter-4.0.0/data/" </> path
let dataFile path = __SOURCE_DIRECTORY__ </> "../../data/" </> path

//  This is a very simple demo of calling the Chinese Word Segmenter
//  programmatically.  It assumes an input file in UTF8.
//  This will run correctly in the distribution home directory.  To
//  run in general, the properties for where to find dictionaries or
//  normalizations have to be set.
//  @author Christopher Manning

let [<Tests>] segmenterTest =
  testCase "Chinese Word Segmenter" <| fun _ ->
    let props = Properties();
    props.setProperty("sighanCorporaDict", segmenterData "Path") |> ignore
    props.setProperty("NormalizationTable", segmenterData "norm.simp.utf8") |> ignore
    props.setProperty("normTableEncoding", "UTF-8") |> ignore
    // below is needed because CTBSegDocumentIteratorFactory accesses it
    props.setProperty("serDictionary", segmenterData "dict-chris6.ser.gz") |> ignore
    props.setProperty("testFile", dataFile "test.simple.utf8") |> ignore
    props.setProperty("inputEncoding", "UTF-8") |> ignore
    props.setProperty("sighanPostProcessing", "true") |> ignore

    let segmenter = CRFClassifier(props)
    segmenter.loadClassifierNoExceptions(segmenterData "ctb.gz", props)
    segmenter.classifyAndWriteAnswers(dataFile "test.simple.utf8")

