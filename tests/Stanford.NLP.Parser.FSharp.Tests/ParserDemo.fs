module Stanford.NLP.Parser.ParserDemo

open NUnit.Framework
open FsUnit
open java.io
open edu.stanford.nlp.objectbank
open edu.stanford.nlp.``process``
open edu.stanford.nlp.ling
open edu.stanford.nlp.trees
open edu.stanford.nlp.parser.lexparser

let models = 
    [|  Models.lexparser.``englishPCFG.ser.gz``
        Models.lexparser.``englishPCFG.caseless.ser.gz``
        Models.lexparser.``englishFactored.ser.gz``
        Models.lexparser.``englishRNN.ser.gz`` |]
    |> Seq.map (edu.stanford.nlp.parser.lexparser.LexicalizedParser.loadModel)

[<TestCaseSourceAttribute("models")>]
let [<Test>] ``Parse easy sentence`` (lp:LexicalizedParser) =
    // This option shows parsing a list of correctly tokenized words
    let sent = [|"This"; "is"; "an"; "easy"; "sentence"; "." |]
    let rawWords = Sentence.toCoreLabelList(sent)
    let parse = lp.apply(rawWords)
    parse |> should not' (be Null)
    parse.pennPrint()

    // This option shows loading and using an explicit tokenizer
    let sent2 = "This is another sentence."
    let tokenizerFactory = PTBTokenizer.factory(CoreLabelTokenFactory(), "")
    use sent2Reader = new StringReader(sent2)
    let rawWords2 = tokenizerFactory.getTokenizer(sent2Reader).tokenize()
    let parse = lp.apply(rawWords2)
    parse |> should not' (be Empty)

    let tlp = PennTreebankLanguagePack()
    let gsf = tlp.grammaticalStructureFactory()
    let gs = gsf.newGrammaticalStructure(parse)
    let tdl = gs.typedDependenciesCCprocessed()
    printfn "\n%O\n" tdl

    let tp = new TreePrint("penn,typedDependenciesCollapsed")
    tp |> should not' (be Null)
    tp.printTree(parse)

[<TestCaseSourceAttribute("models")>]
let [<Test>] ``Loading sentences from file and tokenizing`` (lp:LexicalizedParser) =
    // This option shows loading and sentence-segment and tokenizing
    // a file using DocumentPreprocessor
    let tlp = PennTreebankLanguagePack();
    let gsf = tlp.grammaticalStructureFactory();
    // You could also create a tokenizer here (as below) and pass it
    // to DocumentPreprocessor
    DocumentPreprocessor(DataFiles.``SampleText.txt``)
    |> toSeq
    |> Seq.cast<java.util.List>
    |> Seq.iter (fun sentence -> 
        let parse = lp.apply(sentence);
        parse |> should not' (be Empty)
        parse.pennPrint();

        let gs = gsf.newGrammaticalStructure(parse);
        let tdl = gs.typedDependenciesCCprocessed(true);
        printfn "\n%O\n" tdl
        )
