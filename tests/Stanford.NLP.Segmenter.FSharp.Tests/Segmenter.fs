module Stanford.NLP.Segmenter.FSharp.Tests

open NUnit.Framework
open FsUnit
open java.util
open edu.stanford.nlp.ie.crf

let [<Literal>] segmenterData = __SOURCE_DIRECTORY__ + @"..\..\..\src\temp\stanford-segmenter-2013-11-12\data\"
type SegmenterData = FSharp.Management.FileSystem<segmenterData>

let [<Literal>] dataFilesRoot  = __SOURCE_DIRECTORY__ + @"..\..\data"
type DataFiles = FSharp.Management.FileSystem<dataFilesRoot>

//  This is a very simple demo of calling the Chinese Word Segmenter
//  programmatically.  It assumes an input file in UTF8.
//  This will run correctly in the distribution home directory.  To
//  run in general, the properties for where to find dictionaries or
//  normalizations have to be set.
//  @author Christopher Manning

let [<Test>] ``Chinese Word Segmenter``() = 
    let props = Properties();
    props.setProperty("sighanCorporaDict", SegmenterData.Path) |> ignore
    props.setProperty("NormalizationTable", SegmenterData.``norm.simp.utf8``) |> ignore
    props.setProperty("normTableEncoding", "UTF-8") |> ignore
    // below is needed because CTBSegDocumentIteratorFactory accesses it
    props.setProperty("serDictionary", SegmenterData.``dict-chris6.ser.gz``) |> ignore
    props.setProperty("testFile", DataFiles.``test.simple.utf8``) |> ignore
    props.setProperty("inputEncoding", "UTF-8") |> ignore
    props.setProperty("sighanPostProcessing", "true") |> ignore

    let segmenter = CRFClassifier(props)
    segmenter.loadClassifierNoExceptions(SegmenterData.``ctb.gz``, props)
    segmenter.classifyAndWriteAnswers(DataFiles.``test.simple.utf8``)

