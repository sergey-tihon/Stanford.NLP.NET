module Stanford.NLP.Segmenter.FSharp.Tests

open Expecto
open Stanford.NLP.Config
open java.util
open edu.stanford.nlp.ie.crf

//  This is a very simple demo of calling the Chinese Word Segmenter
//  programmatically.  It assumes an input file in UTF8.
//  This will run correctly in the distribution home directory.  To
//  run in general, the properties for where to find dictionaries or
//  normalizations have to be set.
//  @author Christopher Manning

let [<Tests>] segmenterTest =
  testCase "Chinese Word Segmenter" <| fun _ ->
    let props = Properties();
    props.setProperty("sighanCorporaDict", Segmenter.data "Path") |> ignore
    props.setProperty("NormalizationTable", Segmenter.data "norm.simp.utf8") |> ignore
    props.setProperty("normTableEncoding", "UTF-8") |> ignore
    // below is needed because CTBSegDocumentIteratorFactory accesses it
    props.setProperty("serDictionary", Segmenter.data "dict-chris6.ser.gz") |> ignore
    props.setProperty("testFile", dataFile "test.simple.utf8") |> ignore
    props.setProperty("inputEncoding", "UTF-8") |> ignore
    props.setProperty("sighanPostProcessing", "true") |> ignore

    let segmenter = CRFClassifier(props)
    segmenter.loadClassifierNoExceptions(Segmenter.data "ctb.gz", props)
    segmenter.classifyAndWriteAnswers(dataFile "test.simple.utf8")

