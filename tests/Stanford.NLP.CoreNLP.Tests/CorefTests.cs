using System;
using System.IO;

using edu.stanford.nlp.coref;
using edu.stanford.nlp.coref.data;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;

using java.util;

using NUnit.Framework;

using Stanford.NLP.Tools;

namespace Stanford.NLP.CoreNLP.Tests
{
    [TestFixture]
    public class CorefTests
    {
        // Sample from https://stanfordnlp.github.io/CoreNLP/coref.html
        [Test]
        public void CorefTest()
        {
            Annotation document = new Annotation("Barack Obama was born in Hawaii.  He is the president. Obama was elected in 2008.");
            Properties props = new Properties();
            props.setProperty("annotators", "tokenize,ssplit,pos,lemma,ner,parse,coref");
            props.setProperty("ner.useSUTime", "false");

            var curDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(Files.CoreNlp.JarRoot);
            var pipeline = new StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(curDir);

            pipeline.annotate(document);

            var corefChainAnnotation = new CorefCoreAnnotations.CorefChainAnnotation().getClass();
            var sentencesAnnotation = new CoreAnnotations.SentencesAnnotation().getClass();
            var corefMentionsAnnotation = new CorefCoreAnnotations.CorefMentionsAnnotation().getClass();

            Console.WriteLine("---");
            Console.WriteLine("coref chains");
            var corefChain = (Map)document.get(corefChainAnnotation);
            foreach (CorefChain cc in corefChain.values().toArray())
            {
                Console.WriteLine($"\t{cc}");
            }
            var sentences = (ArrayList)document.get(sentencesAnnotation);
            foreach (CoreMap sentence in sentences.toArray())
            {
                Console.WriteLine("---");
                Console.WriteLine("mentions");
                var corefMentions = (ArrayList)sentence.get(corefMentionsAnnotation);
                foreach (Mention m in corefMentions)
                {
                    Console.WriteLine("\t" + m);
                }
            }
        }
    }
}
