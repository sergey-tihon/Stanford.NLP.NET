using edu.stanford.nlp.ie.crf;
using java.util;

namespace Stanford.NLP.Segmenter.CSharp
{
    class Program
    {
        static void Main()
        {
            // Path to the folder with models
            var segmenterData = @"..\..\..\..\paket-files\nlp.stanford.edu\stanford-segmenter-2015-04-20\data";
            var sampleData = @"..\..\..\..\paket-files\nlp.stanford.edu\stanford-segmenter-2015-04-20\test.simp.utf8";

            // `test.simple.utf8` contains following text:
            // 面对新世纪，世界各国人民的共同愿望是：继续发展人类以往创造的一切文明成果，克服20世纪困扰着人类的战争和贫
            // 困问题，推进和平与发展的崇高事业，创造一个美好的世界。

            // This is a very simple demo of calling the Chinese Word Segmenter programmatically.  
            // It assumes an input file in UTF8. This will run correctly in the distribution home 
            // directory. To run in general, the properties for where to find dictionaries or
            // normalizations have to be set.
            // @author Christopher Manning

            // Setup Segmenter loading properties
            var props = new Properties();
            props.setProperty("sighanCorporaDict", segmenterData);
            // Lines below are needed because CTBSegDocumentIteratorFactory accesses it
            props.setProperty("serDictionary", segmenterData + @"\dict-chris6.ser.gz");
            props.setProperty("testFile", sampleData);
            props.setProperty("inputEncoding", "UTF-8");
            props.setProperty("sighanPostProcessing", "true");

            // Load Word Segmenter
            var segmenter = new CRFClassifier(props);
            segmenter.loadClassifierNoExceptions(segmenterData + @"\ctb.gz", props);
            segmenter.classifyAndWriteAnswers(sampleData);
        }
    }
}
