using edu.stanford.nlp.ie.crf;
using Console = System.Console;

namespace Stanford.NLP.NER.CSharp
{
    class Program
    {
        static void Main()
        {
            // Path to the folder with classifies models
            var jarRoot = @"..\..\..\..\data\paket-files\nlp.stanford.edu\stanford-ner-2018-10-16";
            var classifiersDirecrory = jarRoot + @"\classifiers";

            // Loading 3 class classifier model
            var classifier =CRFClassifier.getClassifierNoExceptions(
                classifiersDirecrory + @"\english.all.3class.distsim.crf.ser.gz");

            var s1 = "Good afternoon Rajat Raina, how are you today?";
            Console.WriteLine("{0}\n", classifier.classifyToString(s1));

            var s2 = "I go to school at Stanford University, which is located in California.";
            Console.WriteLine("{0}\n", classifier.classifyWithInlineXML(s2));

            Console.WriteLine("{0}\n", classifier.classifyToString(s2, "xml", true));
        }
    }
}
