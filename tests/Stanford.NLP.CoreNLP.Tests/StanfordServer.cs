using System;
using System.Collections.Generic;
using System.Linq;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using java.lang;
using java.util;

using NUnit.Framework;

using Stanford.NLP.Tools;

namespace Stanford.NLP.CoreNLP.Tests
{
    [TestFixture]
    public class StanfordServer
    {
        // Sample from https://stanfordnlp.github.io/CoreNLP/corenlp-server.html
        [Test, Explicit]
        public static void CoreNlpClient()
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


            var sentencesAnnotationClass = Java.GetAnnotationClass<CoreAnnotations.SentencesAnnotation>();
            var tokensAnnotationClass = Java.GetAnnotationClass<CoreAnnotations.TokensAnnotation>();
            var textAnnotationClass = Java.GetAnnotationClass<CoreAnnotations.TextAnnotation>();
            var partOfSpeechAnnotationClass = Java.GetAnnotationClass<CoreAnnotations.PartOfSpeechAnnotation>();
            var namedEntityTagAnnotationClass = Java.GetAnnotationClass<CoreAnnotations.NamedEntityTagAnnotation>();

            var sentences = sentencesAnnotationClass.getClasses().Select(document.get).ToList();

            foreach (CoreMap sentence in sentences)
            {
                var tokens = (AbstractList) sentence.get(tokensAnnotationClass);
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
