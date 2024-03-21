using System.Linq;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using java.lang;
using java.util;
using Stanford.NLP.CoreNLP.Tests.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace Stanford.NLP.CoreNLP.Tests.Samples;

[Collection(nameof(IkvmCollection))]
public class StanfordServer
{
    private readonly ITestOutputHelper _testOutputHelper;

    public StanfordServer(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    // Sample from https://stanfordnlp.github.io/CoreNLP/corenlp-server.html
    [Fact(Skip = "CoreNLP server is not available in CI")]
    public void CoreNlpClient()
    {
        // creates a StanfordCoreNLP object with POS tagging, lemmatization, NER, parsing, and coreference resolution
        var props = new Properties();
        props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
        var pipeline = new StanfordCoreNLPClient(props, "http://localhost", 9000, 2);

        // read some text in the text variable
        var text = "Kosgi Santosh sent an email to Stanford University.";
        // create an empty Annotation just with the given text
        Annotation document = new(text);
        // run all Annotators on this text
        pipeline.annotate(document);


        var tokensAnnotationClass = typeof(CoreAnnotations.TokensAnnotation);
        var textAnnotationClass = typeof(CoreAnnotations.TextAnnotation);
        var partOfSpeechAnnotationClass = typeof(CoreAnnotations.PartOfSpeechAnnotation);
        var namedEntityTagAnnotationClass = typeof(CoreAnnotations.NamedEntityTagAnnotation);

        var sentences = ((Class)typeof(CoreAnnotations.SentencesAnnotation)).getClasses().Select(document.get).ToList();

        foreach (CoreMap sentence in sentences)
        {
            var tokens = (AbstractList)sentence.get(tokensAnnotationClass);
            _testOutputHelper.WriteLine("----");
            foreach (CoreLabel token in tokens)
            {
                var word = token.get(textAnnotationClass);
                var pos = token.get(partOfSpeechAnnotationClass);
                var ner = token.get(namedEntityTagAnnotationClass);
                _testOutputHelper.WriteLine($"{word}\t[pos={pos};\tner={ner};]");
            }
        }
    }
}
