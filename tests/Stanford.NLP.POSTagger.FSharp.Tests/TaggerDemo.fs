module Stanford.NLP.POSTagger.FsharpTests

open NUnit.Framework
open FsUnit
open java.io
open java.util
open edu.stanford.nlp.ling
open edu.stanford.nlp.tagger.maxent;

let tagReader (reader:Reader) = 
    let sentances = MaxentTagger.tokenizeText(reader).toArray()
    sentances |> should not' (be Empty)

    sentances |> Seq.iter (fun sentence ->
        sentence |> should be ofExactType<ArrayList>
        let tSentence = tagger.tagSentence(sentence :?> ArrayList)
        printfn "%O" (Sentence.listToString(tSentence, false))
    )


let [<Test>] ``Tag file``() = 
    let fileName = DataFiles.``SampleText.txt``
    tagReader (new BufferedReader(new FileReader(fileName)))

let [<Test>] ``Tag Text``() =
    let text = "A Part-Of-Speech Tagger (POS Tagger) is a piece of software that reads text in some language and assigns parts of speech to each word (and other token), such as noun, verb, adjective, etc., although generally computational applications use more fine-grained POS tags like 'noun-plural'."
    tagReader (new StringReader(text))