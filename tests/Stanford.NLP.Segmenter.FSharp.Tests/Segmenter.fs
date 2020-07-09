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
    let props = Java.props [
        "sighanCorporaDict", Segmenter.root
        "NormalizationTable", Segmenter.data "norm.simp.utf8"
        "normTableEncoding", "UTF-8"
        // below is needed because CTBSegDocumentIteratorFactory accesses it
        "serDictionary", Segmenter.data "dict-chris6.ser.gz"
        "testFile", dataFile "test.simple.utf8"
        "inputEncoding", "UTF-8"
        "sighanPostProcessing", "true"
    ]

    let segmenter = CRFClassifier(props)
    segmenter.loadClassifierNoExceptions(Segmenter.data "ctb.gz", props)
    segmenter.classifyAndWriteAnswers(dataFile "test.simple.utf8")

    let sample = "2008年我住在美国。"
    let segmented = segmenter.segmentString(sample)
    printfn "%A" segmented
