(**
 - tagline: Package: Stanford.NLP.Parser

# Getting started with Stanford Parser
*)


(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../bin/Stanford.NLP.Parser/lib"
#I "../../packages/test/IKVM/lib/"

(**

F# Sample of sentence parsing
-----------------------------
*)
#r "IKVM.OpenJDK.Core.dll"
#r "IKVM.OpenJDK.Util.dll"
#r "stanford-parser.dll"

open java.io
open edu.stanford.nlp.``process``
open edu.stanford.nlp.ling
open edu.stanford.nlp.trees
open edu.stanford.nlp.parser.lexparser

// Path to models extracted from `stanford-parser-3.9.1-models.jar`
let modelsDirectry =
    __SOURCE_DIRECTORY__
    + @"..\..\data\paket-files\nlp.stanford.edu\stanford-parser-full-2018-02-27\models"
    + @"edu\stanford\nlp\models\"

// Loading english PCFG parser from file
let lp = LexicalizedParser.loadModel(modelsDirectry + @"lexparser\englishPCFG.ser.gz")

// This sample shows parsing a list of correctly tokenized words
let sent = [|"This"; "is"; "an"; "easy"; "sentence"; "." |]
let rawWords = SentenceUtils.toCoreLabelList(sent)
let tree = lp.apply(rawWords)
tree.pennPrint()
// [fsi:>]
// [fsi:(ROOT]
// [fsi:  (S]
// [fsi:    (NP (DT This))]
// [fsi:    (VP (VBZ is)]
// [fsi:      (NP (DT an) (JJ easy) (NN sentence)))]
// [fsi:    (. .)))]
// [fsi:val it : unit = ()]

// This option shows loading and using an explicit tokenizer
let sent2 = "This is another sentence."
let tokenizerFactory = PTBTokenizer.factory(CoreLabelTokenFactory(), "")
let sent2Reader = new StringReader(sent2)
let rawWords2 = tokenizerFactory.getTokenizer(sent2Reader).tokenize()
sent2Reader.close()
let tree2 = lp.apply(rawWords2)

// Extract dependencies from lexical tree
let tlp = PennTreebankLanguagePack()
let gsf = tlp.grammaticalStructureFactory()
let gs = gsf.newGrammaticalStructure(tree2)
let tdl = gs.typedDependenciesCCprocessed()
printfn "\n%O\n" tdl
// [fsi:>]
// [fsi:[nsubj(sentence-4, This-1), cop(sentence-4, is-2), det(sentence-4, another-3), root(ROOT-0, sentence-4)]]
// [fsi:]
// [fsi:val it : unit = ()]

// Extract collapsed dependencies from parsed tree
let tp = new TreePrint("penn,typedDependenciesCollapsed")
tp.printTree(tree2)
// [fsi:> ]
// [fsi:(ROOT]
// [fsi:  (S]
// [fsi:    (NP (DT This))]
// [fsi:    (VP (VBZ is)]
// [fsi:      (NP (DT another) (NN sentence)))]
// [fsi:    (. .)))]
// [fsi:]
// [fsi:nsubj(sentence-4, This-1)]
// [fsi:cop(sentence-4, is-2)]
// [fsi:det(sentence-4, another-3)]
// [fsi:root(ROOT-0, sentence-4)]
// [fsi:]
// [fsi:val it : unit = ()]

(**
C# Sample of sentence parsing
----------------------------
    [lang=csharp]
    using java.io;
    using edu.stanford.nlp.process;
    using edu.stanford.nlp.ling;
    using edu.stanford.nlp.trees;
    using edu.stanford.nlp.parser.lexparser;
    using Console = System.Console;

    namespace Stanford.NLP.Parser.CSharp
    {
        class Program
        {
            static void Main()
            {
                // Path to models extracted from `stanford-parser-3.9.1-models.jar`
                var jarRoot = "nlp.stanford.edu\\stanford-parser-full-2018-02-27\\models";
                var modelsDirectory = jarRoot+"\\edu\\stanford\\nlp\\models";

                // Loading english PCFG parser from file
                var lp = LexicalizedParser.loadModel(modelsDirectory + "\\lexparser\\englishPCFG.ser.gz");

                // This sample shows parsing a list of correctly tokenized words
                var sent = new[] { "This", "is", "an", "easy", "sentence", "." };
                var rawWords = SentenceUtils.toCoreLabelList(sent);
                var tree = lp.apply(rawWords);
                tree.pennPrint();

                // This option shows loading and using an explicit tokenizer
                var sent2 = "This is another sentence.";
                var tokenizerFactory = PTBTokenizer.factory(new CoreLabelTokenFactory(), "");
                var sent2Reader = new StringReader(sent2);
                var rawWords2 = tokenizerFactory.getTokenizer(sent2Reader).tokenize();
                sent2Reader.close();
                var tree2 = lp.apply(rawWords2);

                // Extract dependencies from lexical tree
                var tlp = new PennTreebankLanguagePack();
                var gsf = tlp.grammaticalStructureFactory();
                var gs = gsf.newGrammaticalStructure(tree2);
                var tdl = gs.typedDependenciesCCprocessed();
                Console.WriteLine("\n{0}\n", tdl);

                // Extract collapsed dependencies from parsed tree
                var tp = new TreePrint("penn,typedDependenciesCollapsed");
                tp.printTree(tree2);
            }
        }
    }
*)
