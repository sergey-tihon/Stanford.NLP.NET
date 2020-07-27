using edu.stanford.nlp.ling;
using edu.stanford.nlp.tagger.maxent;

using java.io;
using java.util;

using NUnit.Framework;

using Stanford.NLP.Tools;

namespace Stanford.NLP.POSTagger.Tests
{
    [TestFixture]
    public class POSTaggerTests
    {
        [SetUp]
        public void SetUp()
        {
            var model = Files.Tagger.Model("english-bidirectional-distsim.tagger");
            _tagger =new MaxentTagger(model);
        }

        private MaxentTagger _tagger = null!;

        private void TagReader(Reader reader)
        {
            var sentences = MaxentTagger.tokenizeText(reader).toArray();
            Assert.NotNull(sentences);

            foreach (ArrayList sentence in sentences)
            {
                var tSentence = _tagger.tagSentence(sentence);
                TestContext.Out.WriteLine(SentenceUtils.listToString(tSentence, false));
            }
        }

        [Test]
        public void TagTextFromFile()
        {
            var fileName = Files.DataFile("SampleText.txt");
            TagReader(new BufferedReader(new FileReader(fileName)));
        }

        [Test]
        public void TagText()
        {
            var text =
                "A Part-Of-Speech Tagger (POS Tagger) is a piece of software that reads text in some language and assigns parts of speech to each word (and other token), such as noun, verb, adjective, etc., although generally computational applications use more fine-grained POS tags like 'noun-plural'.";
            TagReader(new StringReader(text));
        }
    }
}
