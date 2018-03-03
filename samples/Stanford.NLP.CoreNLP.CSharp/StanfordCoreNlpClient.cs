using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using System;

namespace standfordnlp
{
    class StanfordCoreNlpClient
    {
        public static java.lang.Class GetAnnotationClass<T>()
            => ikvm.@internal.ClassLiteral<T>.Value; // = new T().getClass()

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


            var sentencesAnnotationClass = GetAnnotationClass<CoreAnnotations.SentencesAnnotation>();
            var tokensAnnotationClass = GetAnnotationClass<CoreAnnotations.TokensAnnotation>();
            var textAnnotationClass = GetAnnotationClass<CoreAnnotations.TextAnnotation>();
            var partOfSpeechAnnotationClass = GetAnnotationClass<CoreAnnotations.PartOfSpeechAnnotation>();
            var namedEntityTagAnnotationClass = GetAnnotationClass<CoreAnnotations.NamedEntityTagAnnotation>();
            var normalizedNamedEntityTagAnnotation = GetAnnotationClass<CoreAnnotations.NormalizedNamedEntityTagAnnotation>();

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
                    Console.WriteLine($"{word}\t[pos={pos};\tner={ner};]");
                }
            }
        }
    }
}
