# Stanford.NLP.Segmenter

<Note type="warning">
This package is deprecated and should not be used. <a href="../">Read more.</a>
</Note>
<img align="right" width="150" src="images/logo.png">

A CRF-based word segmenter (tokenizer). Supports Arabic and Chinese (can be used for English, French, and Spanish.)

[![](https://buildstats.info/nuget/Stanford.NLP.Segmenter)](https://www.nuget.org/packages/Stanford.NLP.Segmenter/)

> Tokenization of raw text is a standard pre-processing step for many NLP tasks. For English, tokenization usually involves punctuation splitting and separation of some affixes like possessives. Other languages require more extensive token pre-processing, which is usually called segmentation.
>
> **The Stanford Word Segmenter** currently supports `Arabic` and `Chinese`. The provided segmentation schemes have been found to work well for a variety of applications.
>
> Stanford NLP group recommend at least `1Gb` of memory for documents that contain long sentences.
>
> The segmenter is available for download, licensed under the `GNU General Public License` (v2 or later). Source is included. The package includes components for command-line invocation and a Java API. The segmenter code is dual licensed (in a similar manner to MySQL, etc.). Open source licensing is under the full GPL, which allows many free uses. For distributors of proprietary software, [commercial licensing](http://otlportal.stanford.edu/techfinder/technology/ID=27276) is available.

## Getting started

- [Install NuGet package](https://www.nuget.org/packages/Stanford.NLP.Segmenter/)
- [Download models](https://nlp.stanford.edu/software/stanford-segmenter-4.2.0.zip)
- Try samples from this page
- [Read official Segmenter docs](https://nlp.stanford.edu/software/segmenter.html)

## Sample

```csharp
using edu.stanford.nlp.ie.crf;
using java.util;

class Program
{
    static void Main()
    {
        // Path to the folder with models
        var segmenterData = @"nlp.stanford.edu\stanford-segmenter-4.2.0\data";
        var sampleData = @"nlp.stanford.edu\stanford-segmenter-2020-11-17\test.simp.utf8";

        // `test.simple.utf8` contains following text:
        // 面对新世纪，世界各国人民的共同愿望是：继续发展人类以往创造的一切文明成果，克服20世纪困扰着人类的战争和贫
        // 困问题，推进和平与发展的崇高事业，创造一个美好的世界。

        // This is a very simple demo of calling the Chinese Word Segmenter programmatically.
        // It assumes an input file in UTF8. This will run correctly in the distribution home
        // directory. To run in general, the properties for where to find dictionaries or
        // normalizations have to be set.
        // @author Christopher Manning

        // Setup Segmenter loading properties
        var props = new Properties();
        props.setProperty("sighanCorporaDict", segmenterData);
        // Lines below are needed because CTBSegDocumentIteratorFactory accesses it
        props.setProperty("serDictionary", segmenterData + @"\dict-chris6.ser.gz");
        props.setProperty("testFile", sampleData);
        props.setProperty("inputEncoding", "UTF-8");
        props.setProperty("sighanPostProcessing", "true");

        // Load Word Segmenter
        var segmenter = new CRFClassifier(props);
        segmenter.loadClassifierNoExceptions(segmenterData + @"\ctb.gz", props);
        segmenter.classifyAndWriteAnswers(sampleData);
    }
}
```

