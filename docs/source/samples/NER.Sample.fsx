(**
 - tagline: Package: Stanford.NLP.NER

# Getting started with Stanford NER
*)

(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../bin/Stanford.NLP.NER/lib"
#I "../../packages/test/IKVM/lib/"

(**


F# Sample of Named Entity Recognition
-----------------------------
*)
#r "IKVM.OpenJDK.Core.dll"
#r "IKVM.OpenJDK.Util.dll"
#r "stanford-ner.dll"

open edu.stanford.nlp.ie.crf

// Path to the folder with classifiers models
let classifiersDirectory =
    __SOURCE_DIRECTORY__ + @"..\..\data\paket-files\nlp.stanford.edu\stanford-ner-2016-10-31\classifiers\"

// Loading 3 class classifier model
let classifier =
    CRFClassifier.getClassifierNoExceptions(
        classifiersDirectory + "english.all.3class.distsim.crf.ser.gz")

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

    namespace Stanford.NLP.NER.CSharp
    {
        class Program
        {
            static void Main()
            {
                // Path to the folder with classifiers models
                var jarRoot = @"..\..\..\..\data\paket-files\nlp.stanford.edu\stanford-ner-2016-10-31";
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
    }
*)

