using System.IO;

using edu.stanford.nlp.parser.lexparser;
using edu.stanford.nlp.tagger.maxent;

using java.io;
using java.util.zip;

using NUnit.Framework;

using Stanford.NLP.Tools;

namespace Stanford.NLP.Parser.Tests
{
    [TestFixture]
    public class LoadFromStreamTests
    {
        [Test]
        public void MaxentTaggerTest()
        {
            // Plain model in the file
            var model = Files.Parser.Models("pos-tagger/english-left3words-distsim.tagger");
            using var fs = new FileStream(model, FileMode.Open);
            using var isw = new ikvm.io.InputStreamWrapper(fs);

            var tagger = new MaxentTagger(isw);
            Assert.NotNull(tagger);
        }

        [Test]
        public void LexicalizedParserTest()
        {
            // GZIPed model in the file
            var model = Files.Parser.Models("lexparser/englishPCFG.ser.gz");
            using var fs = new FileStream(model, FileMode.Open);
            using var isw = new ikvm.io.InputStreamWrapper(fs);

            using var ois =
                model.EndsWith(".gz")
                    ? new ObjectInputStream(new GZIPInputStream(isw))
                    : new ObjectInputStream(isw);

            var lp = LexicalizedParser.loadModel(ois);
            Assert.NotNull(lp);
        }
    }
}
