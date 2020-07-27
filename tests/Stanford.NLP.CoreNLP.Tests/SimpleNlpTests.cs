using System.Collections.Generic;

using edu.stanford.nlp.ie.machinereading.structure;
using edu.stanford.nlp.simple;

using java.util;

using NUnit.Framework;

namespace Stanford.NLP.CoreNLP.Tests
{
    [TestFixture]
    public class SimpleNlpTests
    {
        private readonly Properties _props = CoreNlpTests.Props;

        [Test]
        public void Pos()
        {
            var sent = new Sentence("Lucy is in the sky with diamonds.");
            var nerTags = sent.nerTags(_props);
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
            var sentence = new Sentence("your text should go here", _props);
            var actual = sentence.algorithms().headOfSpan(new Span(0, 2));
            Assert.AreEqual(1, actual);
        }
    }
}
