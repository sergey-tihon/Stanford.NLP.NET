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

    [Fact]
    public void LoadFromAssemblyWithModels()
    {
        var text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply.";

        // Annotation pipeline configuration
        var props = new Properties();
        props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse");
        props.setProperty("ner.useSUTime", "false");
        
        var pipeline = new StanfordCoreNLP(props);

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
        var props = new Properties();
        props.setProperty("annotators", "tokenize, ssplit, pos, parse, sentiment");
        //props.setProperty("ner.useSUTime", "false");
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
