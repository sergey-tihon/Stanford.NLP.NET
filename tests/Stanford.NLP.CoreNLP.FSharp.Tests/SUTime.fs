module Stanford.NLP.CoreNLP.SUTimeTests

open Expecto
open Stanford.NLP.Config
open java.util
open edu.stanford.nlp.tagger.maxent
open edu.stanford.nlp.ling
open edu.stanford.nlp.pipeline
open edu.stanford.nlp.time
open edu.stanford.nlp.util

/// http://nlp.stanford.edu/software/sutime.shtml#Usage
let [<Tests>] suTimeTest =
  testCase "SUTime Defaut Test : Three interesting dates" <| fun _ ->
    let pipeline = AnnotationPipeline()
    pipeline.addAnnotator(TokenizerAnnotator(false))
    pipeline.addAnnotator(WordsToSentencesAnnotator(false))

    let tagger = MaxentTagger(CoreNLP.models "pos-tagger/english-left3words-distsim.tagger")
    pipeline.addAnnotator(POSTaggerAnnotator(tagger))

    let sutimeRules =
        [| CoreNLP.models "sutime/defs.sutime.txt"
           CoreNLP.models "sutime/english.holidays.sutime.txt"
           CoreNLP.models "sutime/english.sutime.txt" |]
        |> String.concat ","
    let props = Properties()
    props.setProperty("sutime.rules", sutimeRules ) |> ignore
    props.setProperty("sutime.binders", "0") |> ignore
    pipeline.addAnnotator(TimeAnnotator("sutime", props))

    let text = "Three interesting dates are 18 Feb 1997, the 20th of july and 4 days from today."
    let annotation = Annotation(text)
    annotation.set(CoreAnnotations.DocDateAnnotation().getClass(), "2013-07-14") |> ignore
    pipeline.annotate(annotation)

    printfn "%O\n" (annotation.get(CoreAnnotations.TextAnnotation().getClass()))
    let timexAnnsAll = annotation.get(TimeAnnotations.TimexAnnotations().getClass()) :?> java.util.ArrayList
    Expect.isGreaterThan (timexAnnsAll.size()) 0 "No timex annotations found"

    for cm in timexAnnsAll |> Seq.cast<CoreMap> do
        let tokens = cm.get(CoreAnnotations.TokensAnnotation().getClass()) :?> java.util.List
        Expect.isGreaterThan (tokens.size()) 0 "No tokens found"
        let first = tokens.get(0)
        let last = tokens.get(tokens.size() - 1)
        let time = cm.get(TimeExpression.Annotation().getClass()) :?> TimeExpression
        Expect.isNotNull time "Time expression is null"
        printfn "%A [from char offset '%A' to '%A'] --> %A"
            cm first last (time.getTemporal())
