using System;
using System.Collections.Generic;
using System.Linq;

using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.tagger.maxent;
using edu.stanford.nlp.time;
using edu.stanford.nlp.util;

using NUnit.Framework;

using Stanford.NLP.Tools;

namespace Stanford.NLP.CoreNLP.Tests
{
    [TestFixture]
    public class SUTimeTests
    {
        /// http://nlp.stanford.edu/software/sutime.shtml#Usage
        [Test]
        public void ThreeInterestingDates()
        {
            var pipeline = new AnnotationPipeline();
            pipeline.addAnnotator(new TokenizerAnnotator(false));
            pipeline.addAnnotator(new WordsToSentencesAnnotator(false));

            var filePath = Files.CoreNlp.Models("pos-tagger/english-left3words-distsim.tagger");
            var tagger = new MaxentTagger(filePath);
            pipeline.addAnnotator(new POSTaggerAnnotator(tagger));

            var props = Java.Props(new Dictionary<string, string>
            {
                {"sutime.binders", "0"},
                {
                    "sutime.rules", string.Join(",", new[]
                    {
                        Files.CoreNlp.Models("sutime/defs.sutime.txt"),
                        Files.CoreNlp.Models("sutime/english.sutime.txt"),
                        Files.CoreNlp.Models("sutime/english.holidays.sutime.txt")
                    })
                }
            });
            pipeline.addAnnotator(new TimeAnnotator("sutime", props));

            var text = "Three interesting dates are 18 Feb 1997, the 20th of july and 4 days from today.";
            var annotation = new Annotation(text);
            annotation.set(Java.GetAnnotationClass<CoreAnnotations.DocDateAnnotation>(), "2013-07-14");
            pipeline.annotate(annotation);

            TestContext.Out.WriteLine(annotation.get(Java.GetAnnotationClass<CoreAnnotations.TextAnnotation>()));
            var timexAnnsAll = (java.util.ArrayList) annotation.get(Java.GetAnnotationClass<TimeAnnotations.TimexAnnotations>());
            Assert.Greater(timexAnnsAll.size(), 0);

            foreach (CoreMap cm in timexAnnsAll)
            {
                var tokens = (java.util.List) cm.get(Java.GetAnnotationClass<CoreAnnotations.TokensAnnotation>());
                Assert.Greater(tokens.size(), 0);

                var first = tokens.get(0);
                var last = tokens.get(tokens.size() - 1);
                var time = (TimeExpression) cm.get(Java.GetAnnotationClass<TimeExpression.Annotation>());
                Assert.IsNotNull(time, "Time expression is null");

                TestContext.Out.WriteLine($"{cm} [from char offset '{first}' to '{last}'] --> {time.getTemporal()}");
            }
        }
    }
}
