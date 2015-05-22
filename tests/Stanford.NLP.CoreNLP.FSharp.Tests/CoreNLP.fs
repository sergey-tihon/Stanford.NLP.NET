module Stanford.NLP.CoreNLP.CoreNLPTests

open NUnit.Framework
open FsUnit
open java.util
open java.io
open edu.stanford.nlp.ling
open edu.stanford.nlp.pipeline
open edu.stanford.nlp.util
open edu.stanford.nlp.io
open edu.stanford.nlp.trees
open edu.stanford.nlp.semgraph

let customAnnotationPrint (annotation:Annotation) =
    printfn "-------------"
    printfn "Custom print:"
    printfn "-------------"
    let sentences = annotation.get(CoreAnnotations.SentencesAnnotation().getClass()) :?> java.util.ArrayList
    sentences |> should not' (be Empty)
    for sentence in sentences |> Seq.cast<CoreMap> do
        printfn "\n\nSentence : '%O'" sentence

        let tokens = sentence.get(CoreAnnotations.TokensAnnotation().getClass()) :?> java.util.ArrayList
        tokens |> should not' (be Empty)
        for token in (tokens |> Seq.cast<CoreLabel>) do
            let word = token.get(CoreAnnotations.TextAnnotation().getClass())
            word |> should not' (be Null)
            let pos  = token.get(CoreAnnotations.PartOfSpeechAnnotation().getClass())
            pos |> should not' (be Null)
            let ner  = token.get(CoreAnnotations.NamedEntityTagAnnotation().getClass())
            ner |> should not' (be Null)
            printfn "%O \t[pos=%O; ner=%O]" word pos ner

        printfn "\nTree:"
        let tree = sentence.get(TreeCoreAnnotations.TreeAnnotation().getClass()) :?> Tree
        tree |> should not' (be Null)
        use stream = new ByteArrayOutputStream()
        tree.pennPrint(new PrintWriter(stream))
        printfn "The first sentence parsed is:\n %O" <| stream.toString()

        printfn "\nDependencies:"
        let deps = sentence.get(SemanticGraphCoreAnnotations.CollapsedDependenciesAnnotation().getClass()) :?> SemanticGraph
        deps |> should not' (be Null)
        for edge in deps.edgeListSorted().toArray() |> Seq.cast<SemanticGraphEdge> do
            let gov = edge.getGovernor()
            gov |> should not' (be Null)
            let dep = edge.getDependent()
            dep |> should not' (be Null)
            printfn "%O(%s-%d,%s-%d)"
                (edge.getRelation())
                (gov.word()) (gov.index())
                (dep.word()) (dep.index())

let [<Test>]``StanfordCoreNlpDemo.java that change current directory`` () =
    let text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply.";

    // Annotation pipeline configuration
    let props = Properties()
    props.setProperty("annotators","tokenize, ssplit, pos, lemma, ner, parse, dcoref") |> ignore
    props.setProperty("sutime.binders","0") |> ignore

    // we should change current directory so StanfordCoreNLP could find all the model files
    let curDir = System.Environment.CurrentDirectory
    System.IO.Directory.SetCurrentDirectory(jarRoot)
    let pipeline = StanfordCoreNLP(props)
    System.IO.Directory.SetCurrentDirectory(curDir)

    // Annotation
    let annotation = Annotation(text)
    pipeline.annotate(annotation)

    // Result - Pretty Print
    use stream = new ByteArrayOutputStream()
    pipeline.prettyPrint(annotation, new PrintWriter(stream))
    printfn "%O" <| stream.toString()

    customAnnotationPrint annotation

/// Constants/Keys - https://github.com/stanfordnlp/CoreNLP/blob/1d5d8914500e1110f5c6577a70e49897ccb0d084/src/edu/stanford/nlp/dcoref/Constants.java
/// DefaultPaths/Values - https://github.com/stanfordnlp/CoreNLP/blob/master/src/edu/stanford/nlp/pipeline/DefaultPaths.java
/// Dictionaries/Matching - https://github.com/stanfordnlp/CoreNLP/blob/8f70e42dcd39e40685fc788c3f22384779398d63/src/edu/stanford/nlp/dcoref/Dictionaries.java
let [<Test>]``StanfordCoreNlpDemo.java with manual configuration`` () =
    let text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply.";

    // Annotation pipeline configuration
    let props = Properties()
    let (<==) key value = props.setProperty(key, value) |> ignore
    "annotators"    <== "tokenize, ssplit, pos, lemma, ner, parse, dcoref"
    "pos.model"     <== Models.``pos-tagger``.``english-bidirectional``.``english-bidirectional-distsim.tagger``
    "ner.model"     <== Models.ner.``english.all.3class.distsim.crf.ser.gz``
    "ner.applyNumericClassifiers" <== "false"
    "parse.model"   <== Models.lexparser.``englishPCFG.ser.gz``

    "dcoref.demonym"            <== Models.dcoref.``demonyms.txt``
    "dcoref.states"             <== Models.dcoref.``state-abbreviations.txt``
    "dcoref.animate"            <== Models.dcoref.``animate.unigrams.txt``
    "dcoref.inanimate"          <== Models.dcoref.``inanimate.unigrams.txt``
    "dcoref.male"               <== Models.dcoref.``male.unigrams.txt``
    "dcoref.neutral"            <== Models.dcoref.``neutral.unigrams.txt``
    "dcoref.female"             <== Models.dcoref.``female.unigrams.txt``
    "dcoref.plural"             <== Models.dcoref.``plural.unigrams.txt``
    "dcoref.singular"           <== Models.dcoref.``singular.unigrams.txt``
    "dcoref.countries"          <== Models.dcoref.countries
    "dcoref.extra.gender"       <== Models.dcoref.``namegender.combine.txt``
    "dcoref.states.provinces"   <== Models.dcoref.statesandprovinces
    "dcoref.singleton.predictor"<== Models.dcoref.``singleton.predictor.ser``
    //"dcoref.big.gender.number"  <== Models.dcoref.``gender.data.gz``
    "dcoref.big.gender.number"  <== Models.dcoref.``gender.map.ser.gz``

    //"dcoref.signatures"         <== Models.dcoref.``ne.signatures.txt``
    //let dcorefDictionary =
    //    [|
    //        Models.dcoref.``coref.dict1.tsv``
    //        Models.dcoref.``coref.dict2.tsv``
    //        Models.dcoref.``coref.dict3.tsv``
    //        Models.dcoref.``coref.dict4.tsv``
    //    |]
    //"dcoref.dictlist" <== (dcorefDictionary |> String.concat ",")

    let sutimeRules =
        [| Models.sutime.``defs.sutime.txt``
           Models.sutime.``english.holidays.sutime.txt``
           Models.sutime.``english.sutime.txt`` |]
        |> String.concat ","
    "sutime.rules"      <== sutimeRules
    "sutime.binders"    <== "0"

    let pipeline = StanfordCoreNLP(props)

    // Annotation
    let annotation = Annotation(text)
    pipeline.annotate(annotation)

    // Result - Pretty Print
    use stream = new ByteArrayOutputStream()
    pipeline.prettyPrint(annotation, new PrintWriter(stream))
    printfn "%O" <| stream.toString()

    customAnnotationPrint annotation

