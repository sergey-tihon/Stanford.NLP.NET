# Stanford.NLP.Parser

<img align="right" width="150" src="images/logo.png">

Probabilistic natural language parsers: highly optimized PCFG and dependency parsers, a lexicalized PCFG parser, and a deep learning reranker.

See also: [online parser demo](http://nlp.stanford.edu:8080/parser/).

[![](https://buildstats.info/nuget/Stanford.NLP.Parser)](https://www.nuget.org/packages/Stanford.NLP.Parser/)


>A natural language parser is a program that works out the grammatical **structure of sentences**, for instance, which groups of words go together (as "phrases") and which words are the **subject** or **object** of a verb. Probabilistic parsers use knowledge of language gained from hand-parsed sentences to try to produce the _most likely_ analysis of new sentences. These statistical parsers still make some mistakes, but commonly work rather well. Their development was one of the biggest breakthroughs in natural language processing in the 1990s. You can [try out our parser online](http://nlp.stanford.edu:8080/parser/).
>
>The lexicalized probabilistic parser implements a factored product model, with separate `PCFG` phrase structure and lexical dependency experts, whose preferences are combined by efficient exact inference, using an `A*` algorithm. Alternatively the software can be used simply as an accurate unlexicalized stochastic context-free grammar parser. Either of these yields a good performance statistical parsing system. A GUI (Java) is provided for viewing the phrase structure tree output of the parser.
>
>As well as providing an **English** parser, the parser can be and has been adapted to work with other languages. A **Chinese** parser based on the Chinese Treebank, a **German** parser based on the Negra corpus and **Arabic** parsers based on the Penn Arabic Treebank are also included. The parser has also been used for other languages, such as Italian, Bulgarian, and Portuguese.
>
>The parser provides [Stanford Dependencies](https://nlp.stanford.edu/software/stanford-dependencies.shtml) output as well as phrase structure trees. Typed dependencies are otherwise known **grammatical relations**. This style of output is available only for English and Chinese. For more details, please refer to the [Stanford Dependencies webpage](https://nlp.stanford.edu/software/stanford-dependencies.shtml).
>
>The parser is available for download, licensed under the `GNU General Public License` (v2 or later). Source is included. The package includes components for command-line invocation, a Java parsing GUI, and a Java API. The parser code is dual licensed (in a similar manner to MySQL, etc.). Open source licensing is under the full GPL, which allows many free uses. For distributors of proprietary software, commercial licensing is available.


## Getting started

- [Install NuGet package](https://www.nuget.org/packages/Stanford.NLP.Parser/)
- [Download models](https://nlp.stanford.edu/software/stanford-parser-4.0.0.zip)
- Try samples from this page
- [Read official Parser docs](https://nlp.stanford.edu/software/lex-parser.shtml)

## Samples

```csharp
using java.io;
using edu.stanford.nlp.process;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.parser.lexparser;
using Console = System.Console;

class Program
{
    static void Main()
    {
        // Path to models extracted from `stanford-parser-3.9.1-models.jar`
        var jarRoot = "nlp.stanford.edu\\stanford-parser-4.0.0\\models";
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
```