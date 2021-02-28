# Stanford.NLP.POSTagger

<img align="right" width="150" src="images/logo.png">

Log-linear Part-Of-Speech Tagger for English, Arabic, Chinese, French, and German. The software that reads text in some language and assigns parts of speech to each word (and other token).

[![](https://buildstats.info/nuget/Stanford.NLP.POSTagger)](https://www.nuget.org/packages/Stanford.NLP.POSTagger/)


>A Part-Of-Speech Tagger (`POS Tagger`) is a piece of software that reads text in some language and assigns parts of speech to each word (and other token), such as noun, verb, adjective, etc., although generally computational applications use more fine-grained POS tags like 'noun-plural'.
>
>The full download contains three trained `English` tagger models, an `Arabic` tagger model, a `Chinese` tagger model, and a German tagger model. Both versions include the same source and other required files. The tagger can be retrained on any language, given POS-annotated training text for the language.
>
>Part-of-speech name abbreviations: The English taggers use the Penn Treebank tag set. Here are some links to documentation of the Penn Treebank English POS tag set: [1993 Computational Linguistics article in PDF](http://acl.ldc.upenn.edu/J/J93/J93-2004.pdf), [AMALGAM page](http://www.comp.leeds.ac.uk/amalgam/tagsets/upenn.html), [Aoife Cahill's list](http://www.computing.dcu.ie/~acahill/tagset.html). See the included README-Models.txt in the models directory for more information about the tagsets for the other languages.
>
>The tagger is licensed under the `GNU General Public License`. Source is included. The package includes components for
>command-line invocation, running as a server, and a Java API. The tagger code is dual licensed (in a similar manner to MySQL, etc.). Open source licensing is under the full GPL, which allows many free uses. For distributors of proprietary software, [commercial licensing](http://otlportal.stanford.edu/techfinder/technology/ID=26062) is available.

## Getting started

- [Install NuGet package](https://www.nuget.org/packages/Stanford.NLP.POSTagger/)
- [Download models](https://nlp.stanford.edu/software/stanford-tagger-4.0.0.zip)
- Try samples from this page
- [Read official Tagger docs](https://nlp.stanford.edu/software/tagger.html)

## Samples

```csharp
using java.io;
using java.util;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.tagger.maxent;
using Console = System.Console;
using System;

class Program
{
    static void Main()
    {
        var jarRoot = @"../../../data/paket-files/nlp.stanford.edu/stanford-tagger-4.0.0";
        var modelsDirectory = jarRoot + @"/models";
        var model = modelsDirectory + @"/wsj-0-18-bidirectional-nodistsim.tagger";

        if (!IO.File.Exists(model))
            throw new Exception($"Check path to the model file '{model}'");

        // Loading POS Tagger
        var tagger = new MaxentTagger(model);

        // Text for tagging
        var text = "A Part-Of-Speech Tagger (POS Tagger) is a piece of software that reads text"
                    +"in some language and assigns parts of speech to each word (and other token),"
                    +" such as noun, verb, adjective, etc., although generally computational "
                    +"applications use more fine-grained POS tags like 'noun-plural'.";

        var sentences = MaxentTagger.tokenizeText(new StringReader(text)).toArray();
        foreach (ArrayList sentence in sentences)
        {
            var taggedSentence = tagger.tagSentence(sentence);
            Console.WriteLine(SentenceUtils.listToString(taggedSentence, false));
        }
    }
}
```