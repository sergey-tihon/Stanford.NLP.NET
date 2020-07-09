using edu.stanford.nlp.ie.crf;
using Stanford.NLP.Tools;
using Console = System.Console;

namespace Stanford.NLP.NER.CSharp
{
    class Program
    {
        static void Main()
        {
            // Loading 3 class classifier model
            var classifier =CRFClassifier.getClassifierNoExceptions(
                Files.NER.classifier("english.all.3class.distsim.crf.ser.gz"));

            var s1 = "Good afternoon Rajat Raina, how are you today?";
            Console.WriteLine("{0}", classifier.classifyToString(s1));

            var s2 = "I go to school at Stanford University, which is located in California.";
            Console.WriteLine("{0}", classifier.classifyWithInlineXML(s2));

            Console.WriteLine("{0}", classifier.classifyToString(s2, "xml", true));
        }
    }
}
