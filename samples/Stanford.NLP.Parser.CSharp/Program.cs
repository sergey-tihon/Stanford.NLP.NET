using java.io;
using edu.stanford.nlp.process;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.parser.lexparser;
using Console = System.Console;
using Stanford.NLP.Tools;

namespace Stanford.NLP.Parser.CSharp
{
    class Program
    {
        static void Main()
        {
            // Loading english PCFG parser from file
            var lp = LexicalizedParser.loadModel(Files.Parser.models("lexparser/englishPCFG.ser.gz"));

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
            Console.WriteLine("{0}", tdl);

            // Extract collapsed dependencies from parsed tree
            var tp = new TreePrint("penn,typedDependenciesCollapsed");
            tp.printTree(tree2);
        }
    }
}
