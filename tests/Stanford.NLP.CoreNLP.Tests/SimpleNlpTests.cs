using System;

using edu.stanford.nlp.ie.machinereading.structure;
using edu.stanford.nlp.simple;

using NUnit.Framework;

using Stanford.NLP.Tools;

namespace Stanford.NLP.CoreNLP.Tests
{
    [TestFixture]
    public class SimpleNlpTests
    {
        [SetUp]
        public void SetUp()
        {
            _currentDir = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Files.CoreNlp.JarRoot;
        }

        [TearDown]
        public void TearDown()
        {
            Environment.CurrentDirectory = _currentDir;
        }

        private string _currentDir = null!;

        [Test]
        public void Pos()
        {
            var sent = new Sentence("Lucy is in the sky with diamonds.");
            var nerTags = sent.nerTags();
            Assert.AreEqual("PERSON", nerTags.get(0));

            var firstPOSTag = sent.posTag(0);
            Assert.AreEqual("NNP", firstPOSTag);
        }

        [Test]
        public void Sentences() {
            var doc = new Document("add your text here! It can contain multiple sentences.");
            var sentences = doc.sentences().toArray();
            Assert.AreEqual(2, sentences.Length);
        }

        [Test]
        public void HeadOfSpan() {
            var sentence = new Sentence("your text should go here");
            var actual = sentence.algorithms().headOfSpan(new Span(0, 2));
            Assert.AreEqual(1, actual);
        }
    }
}
