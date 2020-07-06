module Stanford.NLP.POSTagger.FsharpTests

open Expecto
open java.io
open java.util
open edu.stanford.nlp.ling
open edu.stanford.nlp.tagger.maxent;

let tagReader (reader:Reader) =
    let sentences = MaxentTagger.tokenizeText(reader).toArray()
    Expect.isNonEmpty sentences "no sentences tokenized"

    sentences |> Seq.iter (fun sentence ->
        let tSentence = tagger.tagSentence(sentence :?> ArrayList)
        printfn "%O" (SentenceUtils.listToString(tSentence, false))
    )


let [<Tests>] taggerTests =
  testList "POS Tagger" [
    testCase "Tag file" <| fun _ ->
        let fileName = dataFile "SampleText.txt"
        tagReader (new BufferedReader(new FileReader(fileName)))

    testCase "Tag Text" <| fun _ ->
        let text = "A Part-Of-Speech Tagger (POS Tagger) is a piece of software that reads text in some language and assigns parts of speech to each word (and other token), such as noun, verb, adjective, etc., although generally computational applications use more fine-grained POS tags like 'noun-plural'."
        tagReader (new StringReader(text))
  ]
