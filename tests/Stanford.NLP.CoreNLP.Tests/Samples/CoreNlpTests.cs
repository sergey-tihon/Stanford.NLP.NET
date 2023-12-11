using System;
using System.Collections.Generic;
using System.IO;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.neural.rnn;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.semgraph;
using edu.stanford.nlp.sentiment;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.util;
using java.io;
using java.util;
using Stanford.NLP.CoreNLP.Tests.Fixtures;
using Stanford.NLP.CoreNLP.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace Stanford.NLP.CoreNLP.Tests.Samples;

[Collection(nameof(IkvmCollection))]
public class CoreNlpTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public CoreNlpTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    // Annotation pipeline configuration
    public static Properties Props =>
        GetProperties("tokenize, ssplit, pos, lemma, ner, parse");

    public static Properties GetProperties(string annotators)
    {
        Dictionary<string, string> props = new()
        {
            { "annotators", annotators },
            { "tokenize.language", "en" },

            { "pos.model", Files.CoreNlp.Models("pos-tagger/english-left3words-distsim.tagger") },
            {
                "ner.model", string.Join(",",
                    Files.CoreNlp.Models("ner/english.all.3class.distsim.crf.ser.gz"),
                    Files.CoreNlp.Models("ner/english.muc.7class.distsim.crf.ser.gz"),
                    Files.CoreNlp.Models("ner/english.conll.4class.distsim.crf.ser.gz")
                )
            },
            { "ner.useSUTime", "false" }, // !!!
            {
                "sutime.rules", string.Join(",",
                    Files.CoreNlp.Models("sutime/defs.sutime.txt"),
                    Files.CoreNlp.Models("sutime/english.sutime.txt"),
                    Files.CoreNlp.Models("sutime/english.holidays.sutime.txt")
                )
            },

            {
                "ner.fine.regexner.mapping",
                $"ignorecase=true,validpospattern=^(NN|JJ).*,{Files.CoreNlp.Models("kbp/english/gazetteers/regexner_caseless.tab")};{Files.CoreNlp.Models("kbp/english/gazetteers/regexner_cased.tab")}"
            },
            { "ner.fine.regexner.noDefaultOverwriteLabels", "CITY" },

            { "parse.model", Files.CoreNlp.Models("lexparser/englishPCFG.ser.gz") },
            //{"depparse.model", Files.CoreNlp.Models("parser/nndep/english_UD.gz")}

            { "sentiment.model", Files.CoreNlp.Models("sentiment/sentiment.ser.gz") }
        };
        return Java.Props(props);
    }

    [Fact]
    public void ManualPropsConfiguration()
    {
        // Constants/Keys - https://github.com/stanfordnlp/CoreNLP/blob/1d5d8914500e1110f5c6577a70e49897ccb0d084/src/edu/stanford/nlp/dcoref/Constants.java
        // DefaultPaths/Values - https://github.com/stanfordnlp/CoreNLP/blob/master/src/edu/stanford/nlp/pipeline/DefaultPaths.java
        // Dictionaries/Matching - https://github.com/stanfordnlp/CoreNLP/blob/8f70e42dcd39e40685fc788c3f22384779398d63/src/edu/stanford/nlp/dcoref/Dictionaries.java
        var text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply.";

        var pipeline = new StanfordCoreNLP(Props);
        // Annotation
        var annotation = new Annotation(text);
        pipeline.annotate(annotation);

        // Result - Pretty Print
        using var stream = new ByteArrayOutputStream();
        pipeline.prettyPrint(annotation, new PrintWriter(stream));
        _testOutputHelper.WriteLine(stream.toString());

        CustomAnnotationPrint(annotation);
    }

    [Fact]
    public void DirectoryChangeLoad()
    {
        var text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply.";

        // Annotation pipeline configuration
        var props = Java.Props(new Dictionary<string, string>
        {
            { "annotators", "tokenize, ssplit, pos, lemma, ner, parse" },
            { "ner.useSUTime", "false" }
        });

        // we should change current directory so StanfordCoreNLP could find all the model files
        StanfordCoreNLP pipeline;
        var curDir = Environment.CurrentDirectory;
        try
        {
            Directory.SetCurrentDirectory(Files.CoreNlp.JarRoot);
            pipeline = new StanfordCoreNLP(props);
        }
        finally
        {
            Directory.SetCurrentDirectory(curDir);
        }

        // Annotation
        var annotation = new Annotation(text);
        pipeline.annotate(annotation);

        // Result - Pretty Print
        using var stream = new ByteArrayOutputStream();
        pipeline.prettyPrint(annotation, new PrintWriter(stream));
        _testOutputHelper.WriteLine(stream.toString());

        CustomAnnotationPrint(annotation);
    }

    private void CustomAnnotationPrint(Annotation annotation)
    {
        _testOutputHelper.WriteLine("-------------");
        _testOutputHelper.WriteLine("Custom print:");
        _testOutputHelper.WriteLine("-------------");

        var sentences = (ArrayList)annotation.get(typeof(CoreAnnotations.SentencesAnnotation));
        Assert.InRange(sentences.size(), 0, int.MaxValue);

        foreach (CoreMap sentence in sentences)
        {
            _testOutputHelper.WriteLine($"\n\nSentence : '{sentence}'");
            var tokens = (ArrayList)sentence.get(typeof(CoreAnnotations.TokensAnnotation));
            Assert.InRange(tokens.size(), 0, int.MaxValue);
            foreach (CoreLabel token in tokens)
            {
                var word = token.get(typeof(CoreAnnotations.TextAnnotation));
                Assert.NotNull(word);
                var pos = token.get(typeof(CoreAnnotations.PartOfSpeechAnnotation));
                Assert.NotNull(pos);
                var ner = token.get(typeof(CoreAnnotations.NamedEntityTagAnnotation));
                Assert.NotNull(ner);
                _testOutputHelper.WriteLine($"{word} \t[pos={pos}; ner={ner}]");
            }

            _testOutputHelper.WriteLine("\nTree:");
            var tree = (Tree)sentence.get(typeof(TreeCoreAnnotations.TreeAnnotation));
            Assert.NotNull(tree);
            using var stream = new ByteArrayOutputStream();
            tree.pennPrint(new PrintWriter(stream));
            _testOutputHelper.WriteLine($"The first sentence parsed is:\n {stream.toString()}");

            _testOutputHelper.WriteLine("\nDependencies:");
            var deps = (SemanticGraph)sentence.get(
                typeof(SemanticGraphCoreAnnotations.CollapsedDependenciesAnnotation));
            Assert.NotNull(deps);
            foreach (SemanticGraphEdge edge in deps.edgeListSorted().toArray())
            {
                var gov = edge.getGovernor();
                Assert.NotNull(gov);
                var dep = edge.getDependent();
                Assert.NotNull(dep);

                _testOutputHelper.WriteLine(
                    $"{edge.getRelation()}({gov.word()}-{gov.index()},{dep.word()}-{dep.index()})");
            }
        }
    }


    [Fact]
    public void SentimentAnalysisTest()
    {
        // Annotation pipeline configuration
        var props = GetProperties("tokenize, ssplit, pos, parse, sentiment");
        var pipeline = new StanfordCoreNLP(props);

        // Annotation
        var text =
            "The mission of the F# Software Foundation is to promote and advance the F# programming language, including a diverse and international community of F# programmers.";
        var annotation = new Annotation(text);
        pipeline.annotate(annotation);

        var sentences = (ArrayList)annotation.get(typeof(CoreAnnotations.SentencesAnnotation));
        foreach (CoreMap sentence in sentences)
        {
            _testOutputHelper.WriteLine($"Sentence : '{sentence}'");
            var sentenceTree = (Tree)sentence.get(typeof(SentimentCoreAnnotations.SentimentAnnotatedTree));
            Assert.NotNull(sentenceTree);

            var sentiment = RNNCoreAnnotations.getPredictedClass(sentenceTree);
            var preds = RNNCoreAnnotations.getPredictions(sentenceTree);

            for (var i = 0; i <= 4; i++)
            {
                var prob = preds.get(i);
                var descr = i switch
                {
                    0 => "Negative",
                    1 => "Somewhat negative",
                    2 => "Neutral",
                    3 => "Somewhat positive",
                    4 => "Positive",
                    _ => "Unknown"
                };
                _testOutputHelper.WriteLine($"\tP('{descr}') = {prob}");
            }
        }
    }
}
