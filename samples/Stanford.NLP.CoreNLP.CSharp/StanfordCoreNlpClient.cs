using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using System;

namespace standfordnlp
{
    class StanfordCoreNlpClient
    {
        readonly static java.lang.Class sentencesAnnotationClass = new CoreAnnotations.SentencesAnnotation().getClass();
        readonly static java.lang.Class tokensAnnotationClass = new CoreAnnotations.TokensAnnotation().getClass();
        readonly static java.lang.Class textAnnotationClass = new CoreAnnotations.TextAnnotation().getClass();
        readonly static java.lang.Class partOfSpeechAnnotationClass = new CoreAnnotations.PartOfSpeechAnnotation().getClass();
        readonly static java.lang.Class namedEntityTagAnnotationClass = new CoreAnnotations.NamedEntityTagAnnotation().getClass();
        readonly static java.lang.Class normalizedNamedEntityTagAnnotation = new CoreAnnotations.NormalizedNamedEntityTagAnnotation().getClass();

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
                    Console.WriteLine("{0}\t[pos={1};\tner={2};", word, pos, ner);
                }
            }
        }
    }
}
