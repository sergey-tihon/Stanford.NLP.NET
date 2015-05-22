(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../bin/Stanford.NLP.Parser/lib"
#I "../../packages/IKVM/lib/"

(**
Stanford Parser for .NET (A statistical parser)
===============================================

>[A natural language parser][goToOrigin] is a program that works out the grammatical structure of sentences, for instance, which groups of words
>go together (as "phrases") and which words are the subject or object of a verb. Probabilistic parsers use knowledge of language gained from hand-parsed
>sentences to try to produce the most likely analysis of new sentences. These statistical parsers still make some mistakes, but commonly work
>rather well. Their development was one of the biggest breakthroughs in natural language processing in the 1990s. You can
>[try out our parser online](http://nlp.stanford.edu:8080/parser/).
>
>The lexicalized probabilistic parser implements a factored product model, with separate PCFG phrase structure and lexical dependency experts,
>whose preferences are combined by efficient exact inference, using an A* algorithm. Alternatively the software can be used simply as an accurate
>unlexicalized stochastic context-free grammar parser. Either of these yields a good performance statistical parsing system.
>A GUI is provided for viewing the phrase structure tree output of the parser.
>
>As well as providing an English parser, the parser can be and has been adapted to work with other languages. A Chinese parser based on
>the Chinese Treebank, a German parser based on the Negra corpus and Arabic parsers based on the Penn Arabic Treebank are also included.
>The parser has also been used for other languages, such as Italian, Bulgarian, and Portuguese.
>
>The parser provides [Stanford Dependencies](http://www-nlp.stanford.edu/software/stanford-dependencies.shtml) output as well as phrase
>structure trees. Typed dependencies are otherwise known grammatical relations. This style of output is available only for English and Chinese.
>For more details, please refer to the [Stanford Dependencies webpage](http://www-nlp.stanford.edu/software/stanford-dependencies.shtml).
>
>The parser is available for download, licensed under the [GNU General Public License][license] (v2 or later). Source is included.
>The package includes components for command-line invocation, a Java parsing GUI, and a Java API. The parser code is dual licensed
>(in a similar manner to MySQL, etc.). Open source licensing is under the full GPL, which allows many free uses. For distributors of
>proprietary software, [commercial licensing](http://otlportal.stanford.edu/techfinder/technology/ID=24472) is available.
>If you don't need a commercial license, but would like to support maintenance of these tools, Stanford NLP Group welcomes gift funding.

 <div class="row" style="margin-left: auto; margin-right: auto; display: block;">
  <div class="span1"></div>
  <div class="span6">
    <div class="well well-small" id="nuget">
      The Stanford Parser library can be <a href="https://www.nuget.org/packages/Stanford.NLP.Parser/">installed from NuGet</a>:
      <pre>PM> Install-Package Stanford.NLP.Parser</pre>
    </div>
    <form method="get" action="http://nlp.stanford.edu/software/stanford-parser-full-2015-04-20.zip">
    <button type="submit" class="btn btn-large btn-info" style="margin-left: auto; margin-right: auto; display: block;">
    Download Stanford Parser ZIP archive with models</button>
    </form>
  </div>
  <div class="span1"></div>
 </div>

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

// Path to models extracted from `stanford-parser-3.5.2-models.jar`
let modelsDirectry =
    __SOURCE_DIRECTORY__
    + @"..\..\paket-files\nlp.stanford.edu\stanford-parser-full-2015-04-20\models"
    + @"edu\stanford\nlp\models\"

// Loading english PCFG parser from file
let lp = LexicalizedParser.loadModel(modelsDirectry + @"lexparser\englishPCFG.ser.gz")

// This sample shows parsing a list of correctly tokenized words
let sent = [|"This"; "is"; "an"; "easy"; "sentence"; "." |]
let rawWords = Sentence.toCoreLabelList(sent)
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

    namespace parser
    {
        class Program
        {
            static void Main()
            {
                // Path to models extracted from `stanford-parser-3.5.2-models.jar`
                var jarRoot = @"c:\models\stanford-parser-full-2015-01-30\stanford-parser-3.5.2-models";
                var modelsDirectory = jarRoot+@"\edu\stanford\nlp\models";

                // Loading english PCFG parser from file
                var lp = LexicalizedParser.loadModel(modelsDirectory + @"\lexparser\englishPCFG.ser.gz");

                // This sample shows parsing a list of correctly tokenized words
                var sent = new[] { "This", "is", "an", "easy", "sentence", "." };
                var rawWords = Sentence.toCoreLabelList(sent);
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

(**
Read more about Stanford Parser on [the official page][goToOrigin].

  [goToOrigin]: http://www-nlp.stanford.edu/software/lex-parser.shtml
  [license]: https://github.com/sergey-tihon/Stanford.NLP.NET/blob/master/LICENSE.txt

Relevant posts
--------------
*   [Stanford Parser is available on NuGet for F# and C#.](http://sergeytihon.wordpress.com/2013/07/11/stanford-parser-is-available-on-nuget/)
*   [FSharp.NLP.Stanford.Parser available on NuGet.](http://sergeytihon.wordpress.com/2013/06/26/fsharp-nlp-stanford-parser-available-on-nuget/)
*   [FSharp.NLP.Stanford.Parser justification or StackOverflow questions understanding.](http://sergeytihon.wordpress.com/2013/07/21/fsharp-nlp-stanford-parser-justification-or-stackoverflow-questions-understanding/)
*   [NLP: Stanford Parser with F# (.NET).](http://sergeytihon.wordpress.com/2013/02/05/nlp-stanford-parser-with-f-net/)

*)
