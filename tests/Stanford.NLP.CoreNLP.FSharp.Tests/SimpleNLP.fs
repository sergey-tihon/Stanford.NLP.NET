module Stanford.NLP.CoreNLP.SimpleNLP

open Expecto
open System
open Stanford.NLP.Tools
open java.util
open edu.stanford.nlp.simple
open edu.stanford.nlp.ie.machinereading.structure

let runIn f =
    let cur = Environment.CurrentDirectory
    Environment.CurrentDirectory <- CoreNLP.jarRoot
    try
        try f()
        with
        | e -> printfn "Message: %s\nStackTrace:\n%s" e.Message e.StackTrace
               raise e
    finally
        Environment.CurrentDirectory <- cur

//let props = Java.props [
//    "annotators", "tokenize, ssplit, pos, lemma, ner, parse"
//    "ner.useSUTime", "false"
//]
let props = CoreNLPTests.props //TODO: fix

let [<Tests>] simpleNLP =
    testList "CoreNLP.SimpleNLP" [
        test "POS" {
            runIn (fun () ->
                let sent : Sentence = Sentence("Lucy is in the sky with diamonds.")
                let nerTags : List = sent.nerTags(props)
                Expect.equal ((string)(nerTags.get(0))) "PERSON" ""

                let firstPOSTag : string = sent.posTag(0);
                Expect.equal firstPOSTag "NNP" ""
            )
        }
        test "Sentences" {
            runIn (fun () ->
                let doc : Document = new Document("add your text here! It can contain multiple sentences.");
                let sentences = doc.sentences().toArray()
                Expect.equal (sentences.Length) 2 ""
            )
        }
        test "headOfSpan" {
            runIn (fun () ->
                let sentence : Sentence = Sentence("your text should go here", props);
                let actual = sentence.algorithms().headOfSpan(Span(0, 2));
                Expect.equal actual 1 ""
            )
        }
    ]
