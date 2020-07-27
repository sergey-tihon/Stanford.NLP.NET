using System;

using edu.stanford.nlp.ie.crf;
using java.util;

using NUnit.Framework;

using Stanford.NLP.Tools;

namespace Stanford.NLP.Segmenter.Tests
{
    [TestFixture]
    public class SegmenterTests
    {
        [Test]
        public void ChineseWordSegmenter()
        {
            var sampleData = Files.Segmenter.Data("../test.simp.utf8");

            // This is a very simple demo of calling the Chinese Word Segmenter programmatically.
            // It assumes an input file in UTF8. This will run correctly in the distribution home
            // directory. To run in general, the properties for where to find dictionaries or
            // normalizations have to be set.
            // @author Christopher Manning

            // Setup Segmenter loading properties
            var props = new Properties();
            props.setProperty("sighanCorporaDict", Files.Segmenter.Root);
            props.setProperty("NormalizationTable", Files.Segmenter.Data("norm.simp.utf8"));
            props.setProperty("normTableEncoding", "UTF-8");
            // Lines below are needed because CTBSegDocumentIteratorFactory accesses it
            props.setProperty("serDictionary", Files.Segmenter.Data("dict-chris6.ser.gz"));
            props.setProperty("testFile", sampleData);
            props.setProperty("inputEncoding", "UTF-8");
            props.setProperty("sighanPostProcessing", "true");

            // Load Word Segmenter
            var segmenter = new CRFClassifier(props);
            segmenter.loadClassifierNoExceptions(Files.Segmenter.Data(@"ctb.gz"), props);
            segmenter.classifyAndWriteAnswers(sampleData);

            var sample = "2008年我住在美国。";
            var segmented = segmenter.segmentString(sample);
            Console.WriteLine(segmented);
        }
    }
}
