(**
 - tagline: Package: Stanford.NLP.Segmenter

# Getting started with Stanford Word Segmenter
*)

(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../bin/Stanford.NLP.Segmenter/lib"
#I "../../packages/test/IKVM/lib/"

(**

F# Sample of Word Segmentation
-----------------------------
*)
#r "IKVM.OpenJDK.Core.dll"
#r "IKVM.OpenJDK.Util.dll"
#r "stanford-segmenter-4.0.0.dll"

open java.util
open edu.stanford.nlp.ie.crf

// Path to the folder with models
let segmenterData =
    __SOURCE_DIRECTORY__ + @"\..\..\data\paket-files\nlp.stanford.edu\stanford-segmenter-4.0.0\data\"
let sampleData = __SOURCE_DIRECTORY__ + @"\..\..\tests\data\test.simple.utf8";

// `test.simple.utf8` contains following text:
// 面对新世纪，世界各国人民的共同愿望是：继续发展人类以往创造的一切文明成果，克服20世纪困扰着人类的战争和贫
// 困问题，推进和平与发展的崇高事业，创造一个美好的世界。

// This is a very simple demo of calling the Chinese Word Segmenter programmatically.
// It assumes an input file in UTF8. This will run correctly in the distribution home
// directory. To run in general, the properties for where to find dictionaries or
// normalizations have to be set.
// @author Christopher Manning

// Setup Segmenter loading properties
let props = Properties();
props.setProperty("sighanCorporaDict", segmenterData) |> ignore
// Lines below are needed because CTBSegDocumentIteratorFactory accesses it
props.setProperty("serDictionary", segmenterData + "dict-chris6.ser.gz") |> ignore
props.setProperty("testFile", sampleData) |> ignore
props.setProperty("inputEncoding", "UTF-8") |> ignore
props.setProperty("sighanPostProcessing", "true") |> ignore

// Load Word Segmenter
let segmenter = CRFClassifier(props)
segmenter.loadClassifierNoExceptions(segmenterData + "ctb.gz", props)
segmenter.classifyAndWriteAnswers(sampleData)
// [fsi: >]
// [fsi:面对 新 世纪 ， 世界 各 国 人民 的 共同 愿望 是 ： 继续 发展 人类 以往 创造 的 一切 文明 成果 ， 克服 20 ]
// [fsi:世纪 困扰 着 人类 的 战争 和 贫困 问题 ， 推进 和平 与 发展 的 崇高 事业 ， 创造 一 个 美好 的 世界 。]
// [fsi:CRFClassifier tagged 80 words in 1 documents at 74.56 words per second.]
// [fsi:val it : unit = ()]
(**
C# Sample of Word Segmentation
-----------------------------
    [lang=csharp]
    using edu.stanford.nlp.ie.crf;
    using java.util;

    namespace Stanford.NLP.Segmenter.CSharp
    {
        class Program
        {
            static void Main()
            {
                // Path to the folder with models
                var segmenterData = @"nlp.stanford.edu\stanford-segmenter-4.0.0\data";
                var sampleData = @"nlp.stanford.edu\stanford-segmenter-2015-10-31\test.simp.utf8";

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
    }

*)