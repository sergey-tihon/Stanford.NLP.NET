using System.Collections.Generic;

using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.semgraph;

using java.io;
using java.util;

using NUnit.Framework;

using Stanford.NLP.Tools;
using edu.stanford.nlp.neural.rnn;

namespace Stanford.NLP.CoreNLP.Tests
{
    [TestFixture]
    public class CoreNlpTests
    {
        public static Properties GetProperties(string annotators)
        {
            Dictionary<string, string> props = new()
            {
                {"annotators", annotators},
                {"tokenize.language", "en"},

                {"pos.model", Files.CoreNlp.Models("pos-tagger/english-left3words-distsim.tagger")},
                {
                    "ner.model", string.Join(",",
                        Files.CoreNlp.Models("ner/english.all.3class.distsim.crf.ser.gz"),
                        Files.CoreNlp.Models("ner/english.muc.7class.distsim.crf.ser.gz"),
                        Files.CoreNlp.Models("ner/english.conll.4class.distsim.crf.ser.gz")
                    )
                },
                {"ner.useSUTime", "false"}, // !!!
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
                {"ner.fine.regexner.noDefaultOverwriteLabels", "CITY"},

                {"parse.model", Files.CoreNlp.Models("lexparser/englishPCFG.ser.gz")},
                //{"depparse.model", Files.CoreNlp.Models("parser/nndep/english_UD.gz")}

                { "sentiment.model", Files.CoreNlp.Models("sentiment/sentiment.ser.gz") },
            };
            return Java.Props(props);
        }

        // Annotation pipeline configuration
        public static Properties Props =>
            GetProperties("tokenize, ssplit, pos, lemma, ner, parse");

        [Test]
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
            TestContext.Out.WriteLine(stream.toString());

            CustomAnnotationPrint(annotation);
        }

        [Test]
        public void DirectoryChangeLoad()
        {
            var text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply.";

            // Annotation pipeline configuration
            var props = Java.Props(new Dictionary<string, string>
            {
                {"annotators", "tokenize, ssplit, pos, lemma, ner, parse"},
                {"ner.useSUTime", "false"}
            });

            // we should change current directory so StanfordCoreNLP could find all the model files
            StanfordCoreNLP pipeline;
            var curDir = System.Environment.CurrentDirectory;
            try
            {
                System.IO.Directory.SetCurrentDirectory(Files.CoreNlp.JarRoot);
                pipeline = new StanfordCoreNLP(props);
            }
            finally
            {
                System.IO.Directory.SetCurrentDirectory(curDir);
            }

            // Annotation
            var annotation = new Annotation(text);
            pipeline.annotate(annotation);

            // Result - Pretty Print
            using var stream = new ByteArrayOutputStream();
            pipeline.prettyPrint(annotation, new PrintWriter(stream));
            TestContext.Out.WriteLine(stream.toString());

            CustomAnnotationPrint(annotation);
        }
        private void CustomAnnotationPrint(Annotation annotation)
        {
            TestContext.Out.WriteLine("-------------");
            TestContext.Out.WriteLine("Custom print:");
            TestContext.Out.WriteLine("-------------");

            var sentences = (ArrayList)annotation.get(Java.GetAnnotationClass<CoreAnnotations.SentencesAnnotation>());
            Assert.Greater(sentences.size(), 0, "No sentences found");

            foreach (CoreMap sentence in sentences)
            {
                TestContext.Out.WriteLine($"\n\nSentence : '{sentence}'");
                var tokens = (ArrayList)sentence.get(Java.GetAnnotationClass<CoreAnnotations.TokensAnnotation>());
                Assert.Greater(tokens.size(), 0, "No tokens found");
                foreach (CoreLabel token in tokens)
                {
                    var word = token.get(Java.GetAnnotationClass<CoreAnnotations.TextAnnotation>());
                    Assert.NotNull(word, "Word not found");
                    var pos = token.get(Java.GetAnnotationClass<CoreAnnotations.PartOfSpeechAnnotation>());
                    Assert.NotNull(pos, "POS not found");
                    var ner = token.get(Java.GetAnnotationClass<CoreAnnotations.NamedEntityTagAnnotation>());
                    Assert.NotNull(ner, "NER not found");
                    TestContext.Out.WriteLine($"{word} \t[pos={pos}; ner={ner}]");
                }

                TestContext.Out.WriteLine("\nTree:");
                var tree = (Tree)sentence.get(Java.GetAnnotationClass<TreeCoreAnnotations.TreeAnnotation>());
                Assert.NotNull(tree, "Parse Tree is null");
                using var stream = new ByteArrayOutputStream();
                tree.pennPrint(new PrintWriter(stream));
                TestContext.Out.WriteLine($"The first sentence parsed is:\n {stream.toString()}");

                TestContext.Out.WriteLine("\nDependencies:");
                var deps = (SemanticGraph)sentence.get(Java.GetAnnotationClass<SemanticGraphCoreAnnotations.CollapsedDependenciesAnnotation>());
                Assert.NotNull(deps, "Semantic graph is null");
                foreach (SemanticGraphEdge edge in deps.edgeListSorted().toArray())
                {
                    var gov = edge.getGovernor();
                    Assert.NotNull(gov, "Governor is null");
                    var dep = edge.getDependent();
                    Assert.NotNull(dep, "Dependent is null");

                    TestContext.Out.WriteLine(
                        $"{edge.getRelation()}({gov.word()}-{gov.index()},{dep.word()}-{dep.index()})");
                }
            }
        }


        [Test]
        public void SentimentAnalysisTest()
        {
            // Annotation pipeline configuration
            var props = GetProperties("tokenize, ssplit, pos, parse, sentiment");
            var pipeline = new StanfordCoreNLP(props);

            // Annotation
            var text = "The mission of the F# Software Foundation is to promote and advance the F# programming language, including a diverse and international community of F# programmers.";
            var annotation = new Annotation(text);
            pipeline.annotate(annotation);

            var sentences = (ArrayList)annotation.get(Java.GetAnnotationClass<CoreAnnotations.SentencesAnnotation>());
            foreach (CoreMap sentence in sentences)
            {
                TestContext.Out.WriteLine($"Sentence : '{sentence}'");
                var sentenceTree = (Tree)sentence.get(Java.GetAnnotationClass<edu.stanford.nlp.sentiment.SentimentCoreAnnotations.SentimentAnnotatedTree>());
                Assert.NotNull(sentenceTree, "Cannot find SentimentAnnotatedTree");

                var sentiment = RNNCoreAnnotations.getPredictedClass(sentenceTree);
                var preds = RNNCoreAnnotations.getPredictions(sentenceTree);

                for(var i=0; i<=4; i++)
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
                    TestContext.Out.WriteLine($"\tP('{descr}') = {prob}");
                }
            }
        }
    }
}
