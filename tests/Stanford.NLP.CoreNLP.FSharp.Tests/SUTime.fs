module Stanford.NLP.CoreNLP.SUTimeTests

open NUnit.Framework
open FsUnit
open java.util
open edu.stanford.nlp.tagger.maxent
open edu.stanford.nlp.ling
open edu.stanford.nlp.pipeline
open edu.stanford.nlp.time
open edu.stanford.nlp.util

/// http://nlp.stanford.edu/software/sutime.shtml#Usage
let [<Test>] ``SUTime Defaut Test : Three interesting dates`` () =
    let pipeline = AnnotationPipeline()
    pipeline.addAnnotator(TokenizerAnnotator(false))
    pipeline.addAnnotator(WordsToSentencesAnnotator(false))

    let tagger = MaxentTagger(Models.``pos-tagger``.``english-bidirectional``.``english-bidirectional-distsim.tagger``)
    pipeline.addAnnotator(POSTaggerAnnotator(tagger))

    let sutimeRules =
        [| Models.sutime.``defs.sutime.txt``
           Models.sutime.``english.holidays.sutime.txt``
           Models.sutime.``english.sutime.txt`` |]
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
    timexAnnsAll |> should not' (be Empty)

    for cm in timexAnnsAll |> Seq.cast<CoreMap> do
        let tokens = cm.get(CoreAnnotations.TokensAnnotation().getClass()) :?> java.util.List
        tokens.size() |> should be (greaterThan 0)
        let first = tokens.get(0)
        let last = tokens.get(tokens.size() - 1)
        let time = cm.get(TimeExpression.Annotation().getClass()) :?> TimeExpression
        time |> should not' (be Null)
        printfn "%A [from char offset '%A' to '%A'] --> %A"
            cm first last (time.getTemporal())