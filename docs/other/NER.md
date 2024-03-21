# Stanford.NLP.NER

<Note type="warning">
This package is deprecated and should not be used. <a href="../">Read more.</a>
</Note>

<img align="right" width="150" src="images/logo.png">

A Conditional Random Field sequence model, together with well-engineered features for Named Entity Recognition in English, Chinese, and German.

See also: [online NER demo](http://nlp.stanford.edu:8080/ner/)

[![](https://buildstats.info/nuget/Stanford.NLP.NER)](https://www.nuget.org/packages/Stanford.NLP.NER/)

> **Stanford NER** is an implementation of a `Named Entity Recognizer`. Named Entity Recognition (`NER`) labels sequences of words in a text which are the names of things, such as person and company names, or gene and protein names. It comes with well-engineered feature extractors for Named Entity Recognition, and many options for defining feature extractors. Included with the download are good named entity recognizers for English, particularly for the 3 classes (`PERSON`, `ORGANIZATION`, `LOCATION`), and Stanford NLP Group also makes available on [the original page](https://nlp.stanford.edu/software/CRF-NER.html) various other models for different languages and circumstances, including models trained on just the [CoNLL 2003](http://www.cnts.ua.ac.be/conll2003/ner/) English training data. The distributional similarity features in some models improve performance but the models require considerably more memory.
>
> Stanford NER is also known as `CRFClassifier`. The software provides a general implementation of (arbitrary order) linear chain `Conditional Random Field` (`CRF`) sequence models. That is, by training your own models, you can actually use this code to build sequence models for any task.
>
> You can look at a PowerPoint Introduction to NER and the Stanford NER package [ppt](http://www-nlp.stanford.edu/software/jenny-ner-2007.ppt) [pdf](http://www-nlp.stanford.edu/software/jenny-ner-2007.pdf) or the [FAQ](http://www-nlp.stanford.edu/software/crf-faq.shtml), which has some information on training models. Further documentation is provided in the included README and in the javadocs.
>
> Stanford NER is available for download, licensed under the `GNU General Public License` (v2 or later). Source is included. The package includes components for command-line invocation, running as a server, and a Java API. Stanford NER code is dual licensed (in a similar manner to MySQL, etc.). Open source licensing is under the full GPL, which allows many free uses. For distributors of proprietary software, [commercial licensing](http://otlportal.stanford.edu/techfinder/technology/ID=24628) is available.

## Getting started

- [Install NuGet package](https://www.nuget.org/packages/Stanford.NLP.NER/)
- [Download models](https://nlp.stanford.edu/software/stanford-ner-4.2.0.zip)
- Try samples from this page
- [Read official NER docs](https://nlp.stanford.edu/software/CRF-NER.html)

## Samples

```csharp
using edu.stanford.nlp.ie.crf;
using Console = System.Console;

class Program
{
    static void Main()
    {
        // Path to the folder with classifiers models
        var jarRoot = @"..\..\..\..\data\paket-files\nlp.stanford.edu\stanford-ner-4.2.0";
        var classifiersDirectory = jarRoot + @"\classifiers";

        // Loading 3 class classifier model
        var classifier = CRFClassifier.getClassifierNoExceptions(
            classifiersDirectory + @"\english.all.3class.distsim.crf.ser.gz");

        var s1 = "Good afternoon Rajat Raina, how are you today?";
        Console.WriteLine("{0}\n", classifier.classifyToString(s1));

        var s2 = "I go to school at Stanford University, which is located in California.";
        Console.WriteLine("{0}\n", classifier.classifyWithInlineXML(s2));

        Console.WriteLine("{0}\n", classifier.classifyToString(s2, "xml", true));
    }
}
```
