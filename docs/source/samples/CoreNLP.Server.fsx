(**
 - tagline: How to host and use CoreNLP Server

# CoreNLP Server

CoreNLP includes a simple web API server for servicing your human language understanding needs (starting with version `3.6.0`). This page describes how to set it up. CoreNLP server provides both a convenient graphical way to interface with your installation of CoreNLP and an API with which to call CoreNLP using any programming language. If you’re writing a new wrapper of CoreNLP for using it in another language, you’re advised to do it using the CoreNLP Server.

## Getting Started
Stanford CoreNLP ships with a built-in server, which requires only the CoreNLP dependencies. To run this 

    java -mx4g -cp "*" edu.stanford.nlp.pipeline.StanfordCoreNLPServer -port 9000 -timeout 15000

Run the server using all jars in the current directory (e.g., the CoreNLP home directory)

[Read more about configuration and API](https://stanfordnlp.github.io/CoreNLP/corenlp-server.html)

## .NET Client

.NET client mirrors Java client API and behavior.

    [lang=csharp]
    using edu.stanford.nlp.ling;
    using edu.stanford.nlp.pipeline;
    using edu.stanford.nlp.util;
    using System;

    namespace standfordnlp
    {
        class StanfordCoreNlpClient
        {
            readonly static java.lang.Class sentencesAnnotationClass = 
                new CoreAnnotations.SentencesAnnotation().getClass();
            readonly static java.lang.Class tokensAnnotationClass = 
                new CoreAnnotations.TokensAnnotation().getClass();
            readonly static java.lang.Class textAnnotationClass = 
                new CoreAnnotations.TextAnnotation().getClass();
            readonly static java.lang.Class partOfSpeechAnnotationClass = 
                new CoreAnnotations.PartOfSpeechAnnotation().getClass();
            readonly static java.lang.Class namedEntityTagAnnotationClass = 
                new CoreAnnotations.NamedEntityTagAnnotation().getClass();
            readonly static java.lang.Class normalizedNamedEntityTagAnnotation = 
                new CoreAnnotations.NormalizedNamedEntityTagAnnotation().getClass();

            // Sample from https://stanfordnlp.github.io/CoreNLP/corenlp-server.html
            static void Main()
            {
                // creates a StanfordCoreNLP object with POS tagging, lemmatization, NER, parsing, and coreference resolution
                var props = new java.util.Properties();
                props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
                StanfordCoreNLPClient pipeline = new StanfordCoreNLPClient(props, "http://localhost", 9000, 2);
                // read some text in the text variable
                var text = "Kosgi Santosh sent an email to Stanford University.";
                // create an empty Annotation just with the given text
                Annotation document = new Annotation(text);
                // run all Annotators on this text
                pipeline.annotate(document);

                var sentences = document.get(sentencesAnnotationClass) as java.util.AbstractList;
                foreach (CoreMap sentence in sentences)
                {
                    var tokens = sentence.get(tokensAnnotationClass) as java.util.AbstractList;
                    Console.WriteLine("----");
                    foreach (CoreLabel token in tokens)
                    {
                        var word = token.get(textAnnotationClass);
                        var pos = token.get(partOfSpeechAnnotationClass);
                        var ner = token.get(namedEntityTagAnnotationClass);
                        Console.WriteLine("{0}\t[pos={1};\tner={2}]", word, pos, ner);
                    }
                }
            }
        }
    }

*)