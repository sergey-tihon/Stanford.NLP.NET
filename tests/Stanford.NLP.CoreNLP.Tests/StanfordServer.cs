using System;
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
            var props = new Properties();
            props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
            StanfordCoreNLPClient pipeline = new(props, "http://localhost", 9000, 2);

            // read some text in the text variable
            var text = "Kosgi Santosh sent an email to Stanford University.";
            // create an empty Annotation just with the given text
            Annotation document = new(text);
            // run all Annotators on this text
            pipeline.annotate(document);
            
            var sentences = ((Class)typeof(CoreAnnotations.SentencesAnnotation)).getClasses().Select(document.get).ToList();

            foreach (CoreMap sentence in sentences)
            {
                var tokens = (AbstractList)sentence.get(typeof(CoreAnnotations.TokensAnnotation));
                Console.WriteLine("----");
                foreach (CoreLabel token in tokens)
                {
                    var word = token.get(typeof(CoreAnnotations.TextAnnotation));
                    var pos = token.get(typeof(CoreAnnotations.PartOfSpeechAnnotation));
                    var ner = token.get(typeof(CoreAnnotations.NamedEntityTagAnnotation));
                    Console.WriteLine($"{word}\t[pos={pos};\tner={ner};]");
                }
            }
        }
    }
}
