using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.tagger.maxent;
using edu.stanford.nlp.time;
using edu.stanford.nlp.util;
using java.util;
using Stanford.NLP.CoreNLP.Tests.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace Stanford.NLP.CoreNLP.Tests.Samples;

[Collection(nameof(IkvmCollection))]
public class SUTimeTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SUTimeTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    /// http://nlp.stanford.edu/software/sutime.shtml#Usage
    [Fact(Skip = "Requires models from 'stanford-corenlp-4.5.6-models.jar' and not available in CI")]
    public void ThreeInterestingDates()
    {
        var pipeline = new AnnotationPipeline();
        pipeline.addAnnotator(new TokenizerAnnotator(false));
        pipeline.addAnnotator(new WordsToSentencesAnnotator(false));

        var filePath = "stanford-corenlp-4.5.6/models/edu/stanford/nlp/models/pos-tagger/english-left3words-distsim.tagger";
        var tagger = new MaxentTagger(filePath);
        pipeline.addAnnotator(new POSTaggerAnnotator(tagger));


        var props = new Properties();
        props.setProperty("sutime.binders", "0");
        pipeline.addAnnotator(new TimeAnnotator("sutime", props));

        var text = "Three interesting dates are 18 Feb 1997, the 20th of july and 4 days from today.";
        var annotation = new Annotation(text);
        annotation.set(typeof(CoreAnnotations.DocDateAnnotation), "2013-07-14");
        pipeline.annotate(annotation);

        _testOutputHelper.WriteLine((string)annotation.get(typeof(CoreAnnotations.TextAnnotation)));
        var timexAnnsAll = (ArrayList)annotation.get(typeof(TimeAnnotations.TimexAnnotations));
        Assert.InRange(timexAnnsAll.size(), 0, int.MaxValue);

        foreach (CoreMap cm in timexAnnsAll)
        {
            var tokens = (List)cm.get(typeof(CoreAnnotations.TokensAnnotation));
            Assert.InRange(tokens.size(), 0, int.MaxValue);

            var first = tokens.get(0);
            var last = tokens.get(tokens.size() - 1);
            var time = (TimeExpression)cm.get(typeof(TimeExpression.Annotation));
            Assert.NotNull(time);

            _testOutputHelper.WriteLine($"{cm} [from char offset '{first}' to '{last}'] --> {time.getTemporal()}");
        }
    }
}
