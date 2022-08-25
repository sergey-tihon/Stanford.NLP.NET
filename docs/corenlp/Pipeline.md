# Building and using a Stanford CoreNLP pipeline

```csharp
using System;
using System.IO;
using java.util;
using java.io;
using edu.stanford.nlp.pipeline;
using Console = System.Console;

class Program
{
    static void Main()
    {
        // Path to the folder with models extracted from `stanford-corenlp-4.5.0-models.jar`
        var jarRoot = @"nlp.stanford.edu\stanford-corenlp-4.5.0\models";

        // Text for processing
        var text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply.";

        // Annotation pipeline configuration
        var props = new Properties();
        props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
        props.setProperty("ner.useSUTime", "0");

        // We should change current directory, so StanfordCoreNLP could find all the model files automatically
        var curDir = Environment.CurrentDirectory;
        Directory.SetCurrentDirectory(jarRoot);
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
```

Read more about Stanford CoreNLP on [the official page](http://www-nlp.stanford.edu/software/corenlp.shtml).
