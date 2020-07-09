using System;
using System.IO;
using java.util;
using java.io;
using edu.stanford.nlp.pipeline;
using Console = System.Console;
using Stanford.NLP.Tools;

namespace Stanford.NLP.CoreNLP.CSharp
{
    class Demo
    {
        public static void Run()
        {
            // Text for processing
            var text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply.";

            // Annotation pipeline configuration
            var props = new Properties();
            props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse");
            props.setProperty("ner.useSUTime", "0");

            // We should change current directory, so StanfordCoreNLP could find all the model files automatically
            var curDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(Files.CoreNLP.jarRoot);
            var pipeline = new StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(curDir);

            // Annotation
            var annotation = new Annotation(text);
            pipeline.annotate(annotation);

            // Result - Pretty Print
            using (var stream = new ByteArrayOutputStream())
            {
                pipeline.prettyPrint(annotation, new PrintWriter(stream));
                Console.WriteLine(stream.toString());
                stream.close();
            }
        }
    }
}
