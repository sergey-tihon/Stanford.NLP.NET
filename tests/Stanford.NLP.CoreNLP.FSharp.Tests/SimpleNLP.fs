module Stanford.NLP.CoreNLP.SimpleNLP

open Expecto
open System
open java.util
open edu.stanford.nlp.simple
open edu.stanford.nlp.ie.machinereading.structure

let runIn root f =
    let cur = Environment.CurrentDirectory
    Environment.CurrentDirectory <- root
    f()
    Environment.CurrentDirectory <- cur


let [<Tests>] simpleNLP =
    testList "CoreNLP - Simple NLP" [
        test "POS" {
            runIn jarRoot (fun () ->
                let props = Properties()
                props.setProperty("ner.useSUTime","0") |> ignore

                let sent : Sentence = new Sentence("Lucy is in the sky with diamonds.")
                let nerTags : List = sent.nerTags(props);
                Expect.equal ((string)(nerTags.get(0))) "PERSON" ""

                let firstPOSTag : string = sent.posTag(0);
                Expect.equal firstPOSTag "NNP" ""
            )
        }
        test "Sentences" {
            runIn jarRoot (fun () ->
                let doc : Document = new Document("add your text here! It can contain multiple sentences.");
                let sentences = doc.sentences().toArray()
                Expect.equal (sentences.Length) 2 ""
            )
        }
        test "headOfSpan" {
            runIn jarRoot (fun () ->
                let sent2 : Sentence = new Sentence("your text should go here");
                let actual = sent2.algorithms().headOfSpan(new Span(0, 2));
                Expect.equal actual 1 ""
            )
        }
    ]