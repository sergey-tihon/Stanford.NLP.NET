# SUTime - recognizing and normalizing time expressions

>[SUTime](http://www-nlp.stanford.edu/software/sutime.shtml) is a library for recognizing and normalizing time expressions. SUTime is available as part of the [Stanford CoreNLP](CoreNLP.html) pipeline and can be used to annotate documents with temporal information. It is a deterministic rule-based system designed for extensibility.
>
>SUTime was developed using [TokensRegex](http://www-nlp.stanford.edu/software/tokensregex.shtml), a generic framework for defining patterns over text and mapping to semantic objects. An included set of [PowerPoint slides](https://nlp.stanford.edu/software/SUTime.pptx) and the [javadoc for SUTime](http://nlp.stanford.edu/nlp/javadoc/javanlp/edu/stanford/nlp/time/SUTime.html) provide an overview of this package.
>
>SUTime annotations are provided automatically with the [Stanford CoreNLP](CoreNLP.html) pipeline by including
>the `ner` annotator. When a time expression is identified, the `NamedEntityTagAnnotation` is set with one of four temporal types (`DATE`, `TIME`, `DURATION`, and `SET`) and the `NormalizedNamedEntityTagAnnotation` is set to the value of the normalized temporal expression.
>The temporal type and value correspond to the [TIMEX3 standard](http://www.timeml.org/site/publications/timeMLdocs/timeml_1.2.1.html#timex3) for type and value. (Note the slightly weird and non-specific entity name `SET`, which refers to a set of times, such as a recurring event.)

```csharp
using java.util;
using java.io;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.tagger.maxent;
using edu.stanford.nlp.time;
using edu.stanford.nlp.util;
using edu.stanford.nlp.ling;
using Console = System.Console;

class Program
{
    private static void Main()
    {

        // Path to the folder with models extracted from `stanford-corenlp-4.5.0-models.jar`
        var jarRoot = @"..\..\..\..\data\paket-files\nlp.stanford.edu\stanford-corenlp-4.5.0\models";
        var modelsDirectory = jarRoot + @"\edu\stanford\nlp\models";

        // Annotation pipeline configuration
        var pipeline = new AnnotationPipeline();
        pipeline.addAnnotator(new TokenizerAnnotator(false));
        pipeline.addAnnotator(new WordsToSentencesAnnotator(false));

        // Loading POS Tagger and including them into pipeline
        var tagger = new MaxentTagger(modelsDirectory +
                        @"\pos-tagger\english-left3words\english-left3words-distsim.tagger");
        pipeline.addAnnotator(new POSTaggerAnnotator(tagger));

        // SUTime configuration
        var sutimeRules = modelsDirectory + @"\sutime\defs.sutime.txt,"
                            + modelsDirectory + @"\sutime\english.holidays.sutime.txt,"
                            + modelsDirectory + @"\sutime\english.sutime.txt";
        var props = new Properties();
        props.setProperty("sutime.rules", sutimeRules);
        props.setProperty("sutime.binders", "0");
        pipeline.addAnnotator(new TimeAnnotator("sutime", props));

        // Sample text for time expression extraction
        var text = "Three interesting dates are 18 Feb 1997, the 20th of july and 4 days from today.";
        var annotation = new Annotation(text);
        annotation.set(new CoreAnnotations.DocDateAnnotation().getClass(), "2013-07-14");
        pipeline.annotate(annotation);

        Console.WriteLine("{0}\n", annotation.get(new CoreAnnotations.TextAnnotation().getClass()));

        var timexAnnsAll = annotation.get(new TimeAnnotations.TimexAnnotations().getClass()) as ArrayList;
        foreach (CoreMap cm in timexAnnsAll)
        {
            var tokens = cm.get(new CoreAnnotations.TokensAnnotation().getClass()) as List;
            var first = tokens.get(0);
            var last = tokens.get(tokens.size() - 1);
            var time = cm.get(new TimeExpression.Annotation().getClass()) as TimeExpression;
            Console.WriteLine("{0} [from char offset {1} to {2}] --> {3}",
                                cm, first, last, time.getTemporal());
        }
    }
}
```
