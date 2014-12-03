(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use 
// it to define helpers that you do not want to show in the documentation.
#I "../../packages/Stanford.NLP.NER.3.5.0.0/lib"
#I "../../packages/IKVM.8.0.5449.0/lib/"

(**
Stanford Named Entity Recognizer (NER) for .NET
===============================================

>[Stanford NER][goToOrigin] is an implementation of a Named Entity Recognizer. Named Entity Recognition (NER) labels sequences of words in a text 
>which are the names of things, such as person and company names, or gene and protein names. It comes with well-engineered feature extractors 
>for Named Entity Recognition, and many options for defining feature extractors. Included with the download are good named entity recognizers 
>for English, particularly for the 3 classes (`PERSON`, `ORGANIZATION`, `LOCATION`), and Stanford NLP Group also makes available on [the original page][goToOrigin]
>various other models for different languages and circumstances, including models trained on just the [CoNLL 2003](http://www.cnts.ua.ac.be/conll2003/ner/) 
>English training data. The distributional similarity features in some models improve performance but the models require considerably more memory.
>
>Stanford NER is also known as CRFClassifier. The software provides a general implementation of (arbitrary order) linear chain 
>Conditional Random Field (CRF) sequence models. That is, by training your own models, you can actually use this code to build sequence models 
>for any task.
>
>You can look at a PowerPoint Introduction to NER and the Stanford NER package [ppt](http://www-nlp.stanford.edu/software/jenny-ner-2007.ppt) 
>[pdf](http://www-nlp.stanford.edu/software/jenny-ner-2007.pdf) or the [FAQ](http://www-nlp.stanford.edu/software/crf-faq.shtml), which has 
>some information on training models. Further documentation is provided in the included README and in the javadocs.
>
>Stanford NER is available for download, licensed under the [GNU General Public License][license] (v2 or later). Source is included. 
>The package includes components for command-line invocation, running as a server, and a Java API. Stanford NER code is dual licensed 
>(in a similar manner to MySQL, etc.). Open source licensing is under the full GPL, which allows many free uses. 
>For distributors of proprietary software, [commercial licensing](http://otlportal.stanford.edu/techfinder/technology/ID=24628) is available. 
>If you don't need a commercial license, but would like to support maintenance of these tools, Stanford NLP Group welcomes gifts.
 
 <div class="row" style="margin-left: auto; margin-right: auto; display: block;">
  <div class="span1"></div>
  <div class="span6">
    <div class="well well-small" id="nuget">
      The Stanford NER library can be <a href="https://www.nuget.org/packages/Stanford.NLP.NER/">installed from NuGet</a>:
      <pre>PM> Install-Package Stanford.NLP.NER</pre>
    </div>
    <form method="get" action="http://nlp.stanford.edu/software/stanford-ner-2014-10-26.zip">
    <button type="submit" class="btn btn-large btn-info" style="margin-left: auto; margin-right: auto; display: block;">
    Download Stanford NER ZIP archive with models</button>
    </form>
  </div>
  <div class="span1"></div>
 </div>

F# Sample of Named Entity Recognition
-----------------------------
*)
#r "IKVM.OpenJDK.Core.dll"
#r "IKVM.OpenJDK.Util.dll"
#r "stanford-ner.dll"

open edu.stanford.nlp.ie.crf

// Path to the folder with classifies models
let classifiersDirecrory = 
    __SOURCE_DIRECTORY__ + @"..\..\..\src\temp\stanford-ner-2014-10-26\classifiers\"

// Loading 3 class classifier model
let classifier = 
    CRFClassifier.getClassifierNoExceptions(
        classifiersDirecrory + "english.all.3class.distsim.crf.ser.gz")

let s1 = "Good afternoon Rajat Raina, how are you today?"
printfn "%s\n" (classifier.classifyToString(s1))
// [fsi:> ]
// [fsi:Good/O afternoon/O Rajat/PERSON Raina/PERSON,/O how/O are/O you/O today/O?/O]
// [fsi:val it : unit = ()]

let s2 = "I go to school at Stanford University, which is located in California."
printfn "%s\n" (classifier.classifyWithInlineXML(s2))
// [fsi:> ]
// [fsi:I go to school at <ORGANIZATION>Stanford University</ORGANIZATION>, which is ]
// [fsi:located in <LOCATION>California</LOCATION>.]
// [fsi:val it : unit = ()]

printfn "%s\n" (classifier.classifyToString(s2, "xml", true));
// [fsi:> ]
// [fsi:<wi num="0" entity="O">I</wi> <wi num="1" entity="O">go</wi> <wi num="2" entity="O">to</wi> ]
// [fsi:<wi num="3" entity="O">school</wi> <wi num="4" entity="O">at</wi> ]
// [fsi:<wi num="5" entity="ORGANIZATION">Stanford</wi> <wi num="6" entity="ORGANIZATION">University</wi>]
// [fsi:<wi num="7" entity="O">,</wi> <wi num="8" entity="O">which</wi> <wi num="9" entity="O">is</wi>]
// [fsi:<wi num="10" entity="O">located</wi> <wi num="11" entity="O">in</wi> ]
// [fsi:<wi num="12" entity="LOCATION">California</wi><wi num="13" entity="O">.</wi>]
// [fsi:val it : unit = ()]

(**
C# Sample of Named Entity Recognition
-----------------------------
    [lang=csharp]
    using edu.stanford.nlp.ie.crf;
    using Console = System.Console;
    
    namespace ner
    {
        class Program
        {
            static void Main()
            {
                // Path to the folder with classifies models
                var jarRoot = @"c:models\stanford-ner-2014-10-26";
                var classifiersDirecrory = jarRoot + @"\classifiers";
    
                // Loading 3 class classifier model
                var classifier = CRFClassifier.getClassifierNoExceptions(
                    classifiersDirecrory + @"\english.all.3class.distsim.crf.ser.gz");
            
                var s1 = "Good afternoon Rajat Raina, how are you today?";
                Console.WriteLine("{0}\n", classifier.classifyToString(s1));
    
                var s2 = "I go to school at Stanford University, which is located in California.";
                Console.WriteLine("{0}\n", classifier.classifyWithInlineXML(s2));
    
                Console.WriteLine("{0}\n", classifier.classifyToString(s2, "xml", true));
            }
        }
    }
*)

(**
Read more about Stanford Stanford Named Entity Recognizer on [the official page][goToOrigin].

  [goToOrigin]: http://www-nlp.stanford.edu/software/CRF-NER.shtml
  [license]: https://github.com/sergey-tihon/Stanford.NLP.NET/blob/master/LICENSE.txt

Relevant posts
--------------
*   [Stanford Named Entity Recognizer (NER) is available on NuGet.](http://sergeytihon.wordpress.com/2013/07/12/stanford-named-entity-recognizer-ner-is-available-on-nuget/)
*   [NLP: Stanford Named Entity Recognizer with F# (.NET).](http://sergeytihon.wordpress.com/2013/02/16/nlp-stanford-named-entity-recognizer-with-f-net/)

*)
