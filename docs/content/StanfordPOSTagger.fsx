(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../bin/Stanford.NLP.POSTagger/lib"
#I "../../packages/IKVM/lib/"

(**
Stanford Log-linear Part-Of-Speech Tagger for .NET
==================================================

>[A Part-Of-Speech Tagger (POS Tagger)][goToOrigin] is a piece of software that reads text in some language and assigns parts of speech to each word
>(and other token), such as noun, verb, adjective, etc., although generally computational applications use more fine-grained POS tags like 'noun-plural'.
>
>Several downloads are available. The basic download contains two trained tagger models for English. The full download contains three trained English
>tagger models, an Arabic tagger model, a Chinese tagger model, and a German tagger model. Both versions include the same source and other required files.
>The tagger can be retrained on any language, given POS-annotated training text for the language.
>
>Part-of-speech name abbreviations: The English taggers use the Penn Treebank tag set. Here are some links to documentation of the Penn Treebank English
>POS tag set: [1993 Computational Linguistics article in PDF](http://acl.ldc.upenn.edu/J/J93/J93-2004.pdf), [AMALGAM page](http://www.comp.leeds.ac.uk/amalgam/tagsets/upenn.html),
>[Aoife Cahill's list](http://www.computing.dcu.ie/~acahill/tagset.html). See the included README-Models.txt in the models directory for more information
>about the tagsets for the other languages.
>
>The tagger is licensed under the [GNU General Public License][license] (v2 or later). Source is included. The package includes components for
>command-line invocation, running as a server, and a Java API. The tagger code is dual licensed (in a similar manner to MySQL, etc.).
>Open source licensing is under the full GPL, which allows many free uses. For distributors of proprietary software,
>[commercial licensing](http://otlportal.stanford.edu/techfinder/technology/ID=26062) is available. If you don't need a commercial license,
>but would like to support maintenance of these tools, Stanford NLP Group welcomes gift funding.

 <div class="row" style="margin-left: auto; margin-right: auto; display: block;">
  <div class="span1"></div>
  <div class="span6">
    <div class="well well-small" id="nuget">
      The Stanford POS Tagger library can be <a href="https://www.nuget.org/packages/Stanford.NLP.POSTagger/">installed from NuGet</a>:
      <pre>PM> Install-Package Stanford.NLP.POSTagger</pre>
    </div>
    <form method="get" action="http://nlp.stanford.edu/software/stanford-postagger-full-2015-12-09.zip">
    <button type="submit" class="btn btn-large btn-info" style="margin-left: auto; margin-right: auto; display: block;">
    Download Stanford POS Tagger full archive with models</button>
    </form>
  </div>
  <div class="span1"></div>
 </div>

F# Sample of POS Tagging
-----------------------------
*)
#r "IKVM.OpenJDK.Core.dll"
#r "IKVM.OpenJDK.Util.dll"
#r "stanford-postagger-3.5.2.dll"

open java.io
open java.util
open edu.stanford.nlp.ling
open edu.stanford.nlp.tagger.maxent

// Path to the folder with models
let modelsDirectry =
    __SOURCE_DIRECTORY__  + @"..\..\paket-files\nlp.stanford.edu\stanford-postagger-full-2015-12-09\models"

// Loading POS Tagger
let tagger = MaxentTagger(modelsDirectry + "wsj-0-18-bidirectional-nodistsim.tagger")

let tagTexrFromReader (reader:Reader) =
    let sentances = MaxentTagger.tokenizeText(reader).toArray()

    sentances |> Seq.iter (fun sentence ->
        let taggedSentence = tagger.tagSentence(sentence :?> ArrayList)
        printfn "%O" (Sentence.listToString(taggedSentence, false))
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
                var jarRoot = @"..\..\..\..\paket-files\nlp.stanford.edu\stanford-postagger-full-2015-12-09";
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
                    Console.WriteLine(Sentence.listToString(taggedSentence, false));
                }
            }
        }
    }
*)

(**
Read more about Stanford POS Tagger on [the official page][goToOrigin].

  [goToOrigin]: http://www-nlp.stanford.edu/software/tagger.shtml
  [license]: https://github.com/sergey-tihon/Stanford.NLP.NET/blob/master/LICENSE.txt

Relevant posts
--------------
*   [Stanford Log-linear Part-Of-Speech Tagger is available on NuGet.](http://sergeytihon.wordpress.com/2013/07/14/stanford-log-linear-part-of-speech-tagger-is-available-on-nuget/)
*   [NLP: Stanford POS Tagger with F# (.NET)](http://sergeytihon.wordpress.com/2013/02/08/nlp-stanford-pos-tagger-with-f-net/)

*)
