using System;
using edu.stanford.nlp.ie.machinereading.structure;
using edu.stanford.nlp.simple;
using java.util;
using Stanford.NLP.CoreNLP.Tests.Fixtures;
using Stanford.NLP.CoreNLP.Tests.Helpers;
using Xunit;

namespace Stanford.NLP.CoreNLP.Tests.Samples;

[Collection(nameof(IkvmCollection))]
public class SimpleNlpTests : IDisposable
{
    private readonly string _currentDir;

    public SimpleNlpTests()
    {
        _currentDir = Environment.CurrentDirectory;
        Environment.CurrentDirectory = Files.CoreNlp.JarRoot;
    }

    public void Dispose()
    {
        Environment.CurrentDirectory = _currentDir;
    }

    [Fact]
    public void Pos()
    {
        var sent = new Sentence("Lucy is in the sky with diamonds.");
        var props = new Properties();
        props.setProperty("ner.useSUTime", "0");
        var nerTags = sent.nerTags(props);
        Assert.Equal("PERSON", nerTags.get(0));

        var firstPOSTag = sent.posTag(0);
        Assert.Equal("NNP", firstPOSTag);
    }

    [Fact]
    public void Sentences()
    {
        var doc = new Document("add your text here! It can contain multiple sentences.");
        var sentences = doc.sentences().toArray();
        Assert.Equal(2, sentences.Length);
    }

    [Fact]
    public void HeadOfSpan()
    {
        var sentence = new Sentence("your text should go here");
        var actual = sentence.algorithms().headOfSpan(new Span(0, 2));
        Assert.Equal(1, actual);
    }
}
