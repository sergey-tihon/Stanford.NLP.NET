module Stanford.NLP.Parser.ParserDemo

open System.IO
open Expecto
open java.io
open edu.stanford.nlp.objectbank
open edu.stanford.nlp.``process``
open edu.stanford.nlp.ling
open edu.stanford.nlp.trees
open edu.stanford.nlp.parser.lexparser

let models =
    [   model "lexparser/englishPCFG.ser.gz"
        model "lexparser/englishPCFG.caseless.ser.gz"]
    |> List.map (fun path ->
        let name = Path.GetFileName(path)
        let model = edu.stanford.nlp.parser.lexparser.LexicalizedParser.loadModel path
        name, model)


let [<Tests>] parserTests =
    models
    |> List.map (fun (name, lp) ->
        testCase name <| fun _ ->
            // This option shows parsing a list of correctly tokenized words
            let sent = [|"This"; "is"; "an"; "easy"; "sentence"; "." |]
            let rawWords = SentenceUtils.toCoreLabelList(sent)
            let parse = lp.apply(rawWords)
            Expect.isNotNull parse "Parse tree is null"
            parse.pennPrint()

            // This option shows loading and using an explicit tokenizer
            let sent2 = "This is another sentence."
            let tokenizerFactory = PTBTokenizer.factory(CoreLabelTokenFactory(), "")
            use sent2Reader = new StringReader(sent2)
            let rawWords2 = tokenizerFactory.getTokenizer(sent2Reader).tokenize()
            let parse = lp.apply(rawWords2)
            Expect.isNotNull parse "Parse tree is null"

            let tlp = PennTreebankLanguagePack()
            let gsf = tlp.grammaticalStructureFactory()
            let gs = gsf.newGrammaticalStructure(parse)
            let tdl = gs.typedDependenciesCCprocessed()
            printfn "\n%O\n" tdl

            let tp = new TreePrint("penn,typedDependenciesCollapsed")
            Expect.isNotNull tp "TreePrint is null"
            tp.printTree(parse)
    )
    |> testList "Parse easy sentence"


let [<Tests>] parserFileTests =
    models
    |> List.map (fun (name, lp) ->
        testCase name <| fun _ ->
            // This option shows loading and sentence-segment and tokenizing
            // a file using DocumentPreprocessor
            let tlp = PennTreebankLanguagePack();
            let gsf = tlp.grammaticalStructureFactory();
            // You could also create a tokenizer here (as below) and pass it
            // to DocumentPreprocessor
            DocumentPreprocessor(dataFile "SampleText.txt")
            |> toSeq
            |> Seq.cast<java.util.List>
            |> Seq.iter (fun sentence ->
                let parse = lp.apply(sentence);
                Expect.isNotNull parse "Parse tree is null"
                parse.pennPrint();

                let gs = gsf.newGrammaticalStructure(parse);
                let tdl = gs.typedDependenciesCCprocessed(true);
                printfn "\n%O\n" tdl
                )
    )
    |> testList "Loading sentences from file and tokenizing"
