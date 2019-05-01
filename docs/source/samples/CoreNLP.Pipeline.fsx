(**
 - tagline: Package: Stanford.NLP.CoreNLP

# Building and using a Stanford CoreNLP pipeline

*)
(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../../bin/Stanford.NLP.CoreNLP/lib/"
#I "../../../packages/test/IKVM/lib/"

(**

F# Sample of text annotation
----------------------------
*)
#r "IKVM.OpenJDK.Core.dll"
#r "IKVM.OpenJDK.Util.dll"
#r "stanford-corenlp-3.9.1.dll"

open System
open System.IO
open java.util
open java.io
open edu.stanford.nlp.pipeline

// Path to the folder with models extracted from `stanford-corenlp-3.9.1-models.jar`
let jarRoot = __SOURCE_DIRECTORY__ + @"/../../../data/paket-files/nlp.stanford.edu/stanford-corenlp-full-2018-10-05/models/"

// Text for processing
let text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply.";

// Annotation pipeline configuration
let props = Properties()
props.setProperty("annotators","tokenize, ssplit, pos, lemma, ner, parse, dcoref") |> ignore
props.setProperty("ner.useSUTime","0") |> ignore

// We should change current directory, so StanfordCoreNLP could find all the model files automatically
let curDir = Environment.CurrentDirectory
Directory.SetCurrentDirectory(jarRoot)
let pipeline = StanfordCoreNLP(props)
Directory.SetCurrentDirectory(curDir)

// Annotation
let annotation = Annotation(text)
pipeline.annotate(annotation)

// Result - Pretty Print
let stream = new ByteArrayOutputStream()
pipeline.prettyPrint(annotation, new PrintWriter(stream))
printfn "%O" <| stream.toString()
stream.close()

// [fsi:Sentence #1 (9 tokens):]
// [fsi:Kosgi Santosh sent an email to Stanford University.]
// [fsi:[Text=Kosgi CharacterOffsetBegin=0 CharacterOffsetEnd=5 PartOfSpeech=NNP Lemma=Kosgi NamedEntityTag=PERSON] ... ]
// [fsi:(ROOT]
// [fsi:  (S]
// [fsi:    (NP (NNP Kosgi) (NNP Santosh))]
// [fsi:    (VP (VBD sent)]
// [fsi:      (NP (DT an) (NN email))]
// [fsi:      (PP (TO to)]
// [fsi:        (NP (NNP Stanford) (NNP University))))]
// [fsi:    (. .)))]
// [fsi:]
// [fsi:root(ROOT-0, sent-3)]
// [fsi:nn(Santosh-2, Kosgi-1)]
// [fsi:nsubj(sent-3, Santosh-2)]
// [fsi:det(email-5, an-4)]
// [fsi:dobj(sent-3, email-5)]
// [fsi:nn(University-8, Stanford-7)]
// [fsi:prep_to(sent-3, University-8)]
// [fsi:]
// [fsi:Sentence #2 (7 tokens):]
// [fsi:He didn't get a reply.]
// [fsi:[Text=He CharacterOffsetBegin=52 CharacterOffsetEnd=54 PartOfSpeech=PRP Lemma=he NamedEntityTag=O] ... ]
// [fsi:(ROOT]
// [fsi:  (S]
// [fsi:    (NP (PRP He))]
// [fsi:    (VP (VBD did) (RB n't)]
// [fsi:      (VP (VB get)]
// [fsi:        (NP (DT a) (NN reply))))]
// [fsi:    (. .)))]
// [fsi:]
// [fsi:root(ROOT-0, get-4)]
// [fsi:nsubj(get-4, He-1)]
// [fsi:aux(get-4, did-2)]
// [fsi:neg(get-4, n't-3)]
// [fsi:det(reply-6, a-5)]
// [fsi:dobj(get-4, reply-6)]
// [fsi:]
// [fsi:Coreference set:]
// [fsi:	(2,1,[1,2]) -> (1,2,[1,3]), that is: "He" -> "Kosgi Santosh"]

(**
C# Sample of text annotation
----------------------------
    [lang=csharp]
    using System;
    using System.IO;
    using java.util;
    using java.io;
    using edu.stanford.nlp.pipeline;
    using Console = System.Console;

    namespace Stanford.NLP.CoreNLP.CSharp
    {
        class Program
        {
            static void Main()
            {
                // Path to the folder with models extracted from `stanford-corenlp-3.9.1-models.jar`
                var jarRoot = @"nlp.stanford.edu\stanford-corenlp-full-2018-10-05\models";

                // Text for processing
                var text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply.";

                // Annotation pipeline configuration
                var props = new Properties();
                props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
                props.setProperty("ner.useSUTime", "0");

                // We should change current directory, so StanfordCoreNLP could find all the model files automatically
                var curDir = Environment.CurrentDirectory;
                Directory.SetCurrentDirectory(jarRoot);
                var pipeline = new StanfordCoreNLP(props);
                Directory.SetCurrentDirectory(curDir);

                // Annotation
                var annotation = new Annotation(text);
                pipeline.annotate(annotation);

                // Result - Pretty Print
                using (var stream = new ByteArrayOutputStream())
                {
                    pipeline.prettyPrint(annotation, new PrintWriter(stream));
                    Console.WriteLine(stream.toString());
                    stream.close();
                }
            }
        }
    }
*)

(**
Read more about Stanford CoreNLP on [the official page][goToOrigin].
  [goToOrigin]: http://www-nlp.stanford.edu/software/corenlp.shtml
  [license]: https://github.com/sergey-tihon/Stanford.NLP.NET/blob/master/LICENSE.txt
*)