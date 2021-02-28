using edu.stanford.nlp.ie.crf;
using edu.stanford.nlp.ling;

using NUnit.Framework;

using Stanford.NLP.Tools;

namespace Stanford.NLP.NER.Tests
{
    [TestFixture]
    public class NERTests
    {
        // For either a file to annotate or for the hardcoded text example,
        // this demo file shows two ways to process the output, for teaching
        // purposes.  For the file, it shows both how to run NER on a String
        // and how to run it on a whole file.  For the hard-coded String,
        // it shows how to run it on a single sentence, and how to do this
        // and produce an inline XML output format.

        [Test]
        public void ExtractNeFromPredefinedPhrase()
        {
            var filePath = Files.NER.Classifier("english.all.3class.distsim.crf.ser.gz");
            var classifier = CRFClassifier.getClassifierNoExceptions(filePath);

            var s1 = "Good afternoon Rajat Raina, how are you today?";
            var s2 = "I go to school at Stanford University, which is located in California.";
            TestContext.Out.WriteLine(classifier.classifyToString(s1));
            TestContext.Out.WriteLine(classifier.classifyWithInlineXML(s2));
            TestContext.Out.WriteLine(classifier.classifyToString(s2, "xml", true));

            var labels = classifier.classify(s2).toArray();
            Assert.NotNull(labels);

            for (var i = 0; i < labels.Length; i++)
            {
                TestContext.Out.WriteLine($"{i}\n:{labels[i]}");
            }
        }

        [Test]
        public void ExtractNeFromFile()
        {
            var filePath = Files.NER.Classifier("english.all.3class.distsim.crf.ser.gz");
            var classifier = CRFClassifier.getClassifierNoExceptions(filePath);
            var fileContent = System.IO.File.ReadAllText(Files.DataFile("SampleText.txt"));

            var sentences = classifier.classify(fileContent).toArray();
            Assert.NotNull(sentences);

            var key = new CoreAnnotations.AnswerAnnotation().getClass();
            foreach (java.util.List rawSentence in sentences)
            {
                var sentence = rawSentence.toArray();
                Assert.NotNull(sentence);

                foreach (CoreLabel word in sentence)
                {
                    var annotation = word.get(key);
                    Assert.NotNull(annotation);

                    TestContext.Out.WriteLine($"{word.word()}/{annotation}");
                }
                TestContext.Out.WriteLine();
            }
        }
    }
}
