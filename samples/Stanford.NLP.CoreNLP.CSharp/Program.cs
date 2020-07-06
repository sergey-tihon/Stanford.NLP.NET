using System;
using System.IO;
using java.util;
using java.io;
using edu.stanford.nlp.pipeline;
using Console = System.Console;

namespace Stanford.NLP.CoreNLP.CSharp
{
    class Program
    {
        static void Main()
        {
            Demo.Run();
            DemoCorefAnnotator.Run();
            DemoSUTime.Run();
            DemoStanfordCoreNlpClient.Run();
        }
    }
}
