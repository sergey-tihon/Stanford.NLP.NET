(**
 - tagline: Package: Stanford.NLP.POSTagger

# Getting started with Stanford POS Tagger
*)

(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../bin/Stanford.NLP.POSTagger/lib"
#I "../../packages/test/IKVM/lib/"

(**

F# Sample of POS Tagging
-----------------------------
*)
#r "IKVM.OpenJDK.Core.dll"
#r "IKVM.OpenJDK.Util.dll"
#r "stanford-postagger-3.7.0.dll"

open java.io
open java.util
open edu.stanford.nlp.ling
open edu.stanford.nlp.tagger.maxent

// Path to the folder with models
let modelsDirectry =
    __SOURCE_DIRECTORY__  + @"..\..\data\paket-files\nlp.stanford.edu\stanford-postagger-full-2016-10-31\models"

// Loading POS Tagger
let tagger = MaxentTagger(modelsDirectry + "wsj-0-18-bidirectional-nodistsim.tagger")

let tagTexrFromReader (reader:Reader) =
    let sentances = MaxentTagger.tokenizeText(reader).toArray()

    sentances |> Seq.iter (fun sentence ->
        let taggedSentence = tagger.tagSentence(sentence :?> ArrayList)
        printfn "%O" (SentenceUtils.listToString(taggedSentence, false))
    )


// Text for tagging
let text = """A Part-Of-Speech Tagger (POS Tagger) is a piece of software that reads text in some language
and assigns parts of speech to each word (and other token), such as noun, verb, adjective, etc., although
generally computational applications use more fine-grained POS tags like 'noun-plural'."""

tagTexrFromReader <| new StringReader(text)
// [fsi:>]
// [fsi:A/DT Part-Of-Speech/NNP Tagger/NNP -LRB-/-LRB- POS/NNP Tagger/NNP -RRB-/-RRB- is/VBZ a/DT piece/NN of/IN ]
// [fsi:software/NN that/WDT reads/VBZ text/NN in/IN some/DT language/NN and/CC assigns/VBZ parts/NNS of/IN ]
// [fsi:speech/NN to/TO each/DT word/NN -LRB-/-LRB- and/CC other/JJ token/JJ -RRB-/-RRB- ,/, such/JJ as/IN ]
// [fsi:noun/JJ ,/, verb/JJ ,/, adjective/JJ ,/, etc./FW ,/, although/IN generally/RB computational/JJ ]
// [fsi:applications/NNS use/VBP more/RBR fine-grained/JJ POS/NNP tags/NNS like/IN `/`` noun-plural/JJ '/'' ./.]
// [fsi:val it : unit = ()]

(**C# Sample of POS Tagging
-----------------------------
    [lang=csharp]
    using java.io;
    using java.util;
    using edu.stanford.nlp.ling;
    using edu.stanford.nlp.tagger.maxent;
    using Console = System.Console;

    namespace Stanford.NLP.POSTagger.CSharp
    {
        class Program
        {
            static void Main()
            {
                var jarRoot = @"..\..\..\..\data\paket-files\nlp.stanford.edu\stanford-postagger-full-2016-10-31";
                var modelsDirectory = jarRoot + @"\models";

                // Loading POS Tagger
                var tagger = new MaxentTagger(modelsDirectory + @"\wsj-0-18-bidirectional-nodistsim.tagger");

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
    }
*)
