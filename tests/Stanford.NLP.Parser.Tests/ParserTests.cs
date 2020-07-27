using System.Linq;

using edu.stanford.nlp.ling;
using edu.stanford.nlp.parser.lexparser;
using edu.stanford.nlp.process;
using edu.stanford.nlp.trees;

using java.io;

using NUnit.Framework;

using Stanford.NLP.Tools;

using List = java.util.List;

namespace Stanford.NLP.Parser.Tests
{
    [TestFixture]
    public class ParserTests
    {
        [SetUp]
        public void SetUp()
        {
            var modelPath = Files.Parser.Models("lexparser/englishPCFG.ser.gz");
            _lp = LexicalizedParser.loadModel(modelPath);
        }

        private LexicalizedParser _lp = null!;

        [Test]
        public void ParseEasySentence()
        {
            // This option shows parsing a list of correctly tokenized words
            var sent = new[] {"This", "is", "an", "easy", "sentence", "."};
            var rawWords = SentenceUtils.toCoreLabelList(sent);
            var parse = _lp.apply(rawWords);
            Assert.NotNull(parse);
            parse.pennPrint();

            // This option shows loading and using an explicit tokenizer
            var sent2 = "This is another sentence.";
            var tokenizerFactory = PTBTokenizer.factory(new CoreLabelTokenFactory(), "");
            using var sent2Reader = new StringReader(sent2);
            var rawWords2 = tokenizerFactory.getTokenizer(sent2Reader).tokenize();
            parse = _lp.apply(rawWords2);
            Assert.NotNull(parse);

            var tlp = new PennTreebankLanguagePack();
            var gsf = tlp.grammaticalStructureFactory();
            var gs = gsf.newGrammaticalStructure(parse);
            var tdl = gs.typedDependenciesCCprocessed();
            TestContext.Out.WriteLine($"\n{tdl}\n");

            var tp = new TreePrint("penn,typedDependenciesCollapsed");
            Assert.NotNull(tp);
            tp.printTree(parse);
        }

        [Test]
        public void LoadSentencesFromFile()
        {
            // This option shows loading and sentence-segment and tokenizing
            // a file using DocumentPreprocessor
            var tlp = new PennTreebankLanguagePack();
            var gsf = tlp.grammaticalStructureFactory();

            // You could also create a tokenizer here (as below) and pass it
            // to DocumentPreprocessor
            var preprocessor = new DocumentPreprocessor(Files.DataFile("SampleText.txt"));
            foreach (var sentence in preprocessor.ToSeq().Cast<List>())
            {
                var parse = _lp.apply(sentence);
                Assert.NotNull(parse);
                parse.pennPrint();

                var gs = gsf.newGrammaticalStructure(parse);
                var tdl = gs.typedDependenciesCCprocessed(true);
                TestContext.Out.WriteLine("\n{tdl}\n");
            }
        }
    }
}
