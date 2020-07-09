using System;
using System.IO;

using edu.stanford.nlp.coref;
using edu.stanford.nlp.coref.data;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;

using java.util;

using Console = System.Console;
using Stanford.NLP.Tools;

namespace Stanford.NLP.CoreNLP.CSharp
{
    class DemoCorefAnnotator
    {
        // Sample from https://stanfordnlp.github.io/CoreNLP/coref.html
        public static void Run()
        {
            Annotation document = new Annotation("Barack Obama was born in Hawaii.  He is the president. Obama was elected in 2008.");
            Properties props = new Properties();
            props.setProperty("annotators", "tokenize,ssplit,pos,lemma,ner,parse,mention,coref");
            props.setProperty("ner.useSUTime", "0");

            var curDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(Files.CoreNLP.jarRoot);
            var pipeline = new StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(curDir);

            pipeline.annotate(document);

            var corefChainAnnotation = new CorefCoreAnnotations.CorefChainAnnotation().getClass();
            var sentencesAnnotation = new CoreAnnotations.SentencesAnnotation().getClass();
            var corefMentionsAnnotation = new CorefCoreAnnotations.CorefMentionsAnnotation().getClass();

            Console.WriteLine("---");
            Console.WriteLine("coref chains");
            var corefChain = document.get(corefChainAnnotation) as Map;
            foreach (CorefChain cc in corefChain.values().toArray()) {
                Console.WriteLine($"\t{cc}");
            }
            var sentences = document.get(sentencesAnnotation) as ArrayList;
            foreach (CoreMap sentence in sentences.toArray()) {
                Console.WriteLine("---");
                Console.WriteLine("mentions");
                var corefMentions = sentence.get(corefMentionsAnnotation) as ArrayList;
                foreach (Mention m in corefMentions) {
                    Console.WriteLine("\t" + m);
                }
            }
        }
    }
}
