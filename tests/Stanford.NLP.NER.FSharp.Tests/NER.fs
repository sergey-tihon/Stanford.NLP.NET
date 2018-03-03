module Stanford.NLP.NER.FSharp.Tests

open Expecto
open edu.stanford.nlp.ie
open edu.stanford.nlp.ie.crf
open edu.stanford.nlp.io
open edu.stanford.nlp.ling

// For either a file to annotate or for the hardcoded text example,
// this demo file shows two ways to process the output, for teaching
// purposes.  For the file, it shows both how to run NER on a String
// and how to run it on a whole file.  For the hard-coded String,
// it shows how to run it on a single sentence, and how to do this
// and produce an inline XML output format.

let [<Literal>] classifiersRoot = __SOURCE_DIRECTORY__ + @"..\..\..\data\paket-files\nlp.stanford.edu\stanford-ner-2018-02-27\classifiers\"
let [<Literal>] dataFilesRoot  = __SOURCE_DIRECTORY__ + @"..\..\data"
type Classifiers = FSharp.Management.FileSystem<classifiersRoot>
type DataFiles = FSharp.Management.FileSystem<dataFilesRoot>

let [<Tests>] nerTests =
  testList "NER" [
    testCase "Extract named entities from predefined phrase" <| fun _ ->
        let classifier =
            CRFClassifier.getClassifierNoExceptions(Classifiers.``english.all.3class.distsim.crf.ser.gz``)

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
            CRFClassifier.getClassifierNoExceptions(Classifiers.``english.all.3class.distsim.crf.ser.gz``)
        let fileContent = System.IO.File.ReadAllText(DataFiles.``SampleText.txt``)

        let sentances = classifier.classify(fileContent).toArray()
        Expect.isNonEmpty sentances "No sentences found"

        sentances
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