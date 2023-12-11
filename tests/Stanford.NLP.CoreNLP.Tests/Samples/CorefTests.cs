using System;
using System.IO;
using edu.stanford.nlp.coref;
using edu.stanford.nlp.coref.data;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using java.util;
using Stanford.NLP.CoreNLP.Tests.Fixtures;
using Stanford.NLP.CoreNLP.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace Stanford.NLP.CoreNLP.Tests.Samples;

[Collection(nameof(IkvmCollection))]
public class CorefTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public CorefTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    // Sample from https://stanfordnlp.github.io/CoreNLP/coref.html
    [Fact]
    public void CorefTest()
    { 
        var document =
            new Annotation("Barack Obama was born in Hawaii.  He is the president. Obama was elected in 2008.");
        var props = new Properties();
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

        _testOutputHelper.WriteLine("---");
        _testOutputHelper.WriteLine("coref chains");
        var corefChain = (Map)document.get(corefChainAnnotation);
        foreach (CorefChain cc in corefChain.values().toArray()) _testOutputHelper.WriteLine($"\t{cc}");
        var sentences = (ArrayList)document.get(sentencesAnnotation);
        foreach (CoreMap sentence in sentences.toArray())
        {
            _testOutputHelper.WriteLine("---");
            _testOutputHelper.WriteLine("mentions");
            var corefMentions = (ArrayList)sentence.get(corefMentionsAnnotation);
            foreach (Mention m in corefMentions) _testOutputHelper.WriteLine("\t" + m);
        }
    }
}
