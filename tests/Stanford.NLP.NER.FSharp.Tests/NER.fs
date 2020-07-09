module Stanford.NLP.NER.FSharp.Tests

open Expecto
open Stanford.NLP.Tools
open edu.stanford.nlp.ie.crf
open edu.stanford.nlp.ling

// For either a file to annotate or for the hardcoded text example,
// this demo file shows two ways to process the output, for teaching
// purposes.  For the file, it shows both how to run NER on a String
// and how to run it on a whole file.  For the hard-coded String,
// it shows how to run it on a single sentence, and how to do this
// and produce an inline XML output format.

let [<Tests>] nerTests =
  testList "NER" [
    testCase "Extract named entities from predefined phrase" <| fun _ ->
        let classifier =
            CRFClassifier.getClassifierNoExceptions(NER.classifier "english.all.3class.distsim.crf.ser.gz")

        let s1 = "Good afternoon Rajat Raina, how are you today?"
        let s2 = "I go to school at Stanford University, which is located in California."
        printfn "%s\n" (classifier.classifyToString(s1))
        printfn "%s\n" (classifier.classifyWithInlineXML(s2))
        printfn "%s\n" (classifier.classifyToString(s2, "xml", true));

        let labels =  classifier.classify(s2).toArray()
        Expect.isNonEmpty labels "Labels does not exist"

        labels |> Seq.iteri (fun i coreLabel ->
            printfn "%d\n:%O\n" i coreLabel
        )

    testCase "Extract named entities from file" <| fun _ ->
        let classifier =
            CRFClassifier.getClassifierNoExceptions(NER.classifier "english.all.3class.distsim.crf.ser.gz")
        let fileContent = System.IO.File.ReadAllText(dataFile "SampleText.txt")

        let sentences = classifier.classify(fileContent).toArray()
        Expect.isNonEmpty sentences "No sentences found"

        sentences
        |> Seq.cast<java.util.List>
        |> Seq.iter (fun rawSentence ->
            let sentence = rawSentence.toArray()
            Expect.isNonEmpty sentence "Sentence is empty"
            sentence
            |> Seq.cast<CoreLabel>
            |> Seq.iter (fun word ->
                let annotation = word.get(CoreAnnotations.AnswerAnnotation().getClass())
                Expect.isNotNull annotation "Annotation is null"
                printf "%s/%O " (word.word()) annotation
            )
            printfn ""
        )
  ]
