(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use 
// it to define helpers that you do not want to show in the documentation.
#I "../../packages/Stanford.NLP.Segmenter.3.4.0.0/lib"
#I "../../packages/IKVM.7.4.5196.0/lib/"

(**
Stanford Word Segmenter for .NET
================================

>Tokenization of raw text is a standard pre-processing step for many NLP tasks. For English, tokenization usually involves punctuation splitting 
>and separation of some affixes like possessives. Other languages require more extensive token pre-processing, which is usually called segmentation.
>
>[The Stanford Word Segmenter][goToOrigin] currently supports Arabic and Chinese. The provided segmentation schemes have been found to work well 
>for a variety of applications.
>
>Stanford NLP group recommend at least 1G of memory for documents that contain long sentences.
>
>The segmenter is available for download, licensed under the [GNU General Public License][license] (v2 or later). Source is included. The package 
>includes components for command-line invocation and a Java API. The segmenter code is dual licensed (in a similar manner to MySQL, etc.). 
>Open source licensing is under the full GPL, which allows many free uses. For distributors of proprietary software, 
>[commercial licensing](http://otlportal.stanford.edu/techfinder/technology/ID=27276) is available. If you don't need a commercial license, 
>but would like to support maintenance of these tools, Stanford NLP Group welcomes gift funding.
 
 <div class="row" style="margin-left: auto; margin-right: auto; display: block;">
  <div class="span1"></div>
  <div class="span6">
    <div class="well well-small" id="nuget">
      The Stanford Word Segmenter library can be <a href="https://www.nuget.org/packages/Stanford.NLP.Segmenter/">installed from NuGet</a>:
      <pre>PM> Install-Package Stanford.NLP.Segmenter</pre>
    </div>
    <form method="get" action="http://nlp.stanford.edu/software/stanford-segmenter-2014-06-16.zip">
    <button type="submit" class="btn btn-large btn-info" style="margin-left: auto; margin-right: auto; display: block;">
    Download Stanford Word Segmenter archive with models (248Mb)</button>
    </form>
  </div>
  <div class="span1"></div>
 </div>

F# Sample of Word Segmentation 
-----------------------------
*)
#r "IKVM.OpenJDK.Core.dll"
#r "IKVM.OpenJDK.Util.dll"
#r "seg.dll"

open java.util
open edu.stanford.nlp.ie.crf

// Path to the folder with models
let segmenterData = __SOURCE_DIRECTORY__ + @"..\..\..\src\temp\stanford-segmenter-2014-06-16\data\"
let sampleData = __SOURCE_DIRECTORY__ + @"..\..\..\tests\data\test.simple.utf8";

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
// [fsi:CRFClassifier tagged 80 words in 1 documents at 159.36 words per second.]
// [fsi:val it : unit = ()]
(**
Read more about Stanford Word Segmenter on [the official page][goToOrigin].

  [goToOrigin]: http://www-nlp.stanford.edu/software/segmenter.shtml
  [license]: https://github.com/sergey-tihon/Stanford.NLP.NET/blob/master/LICENSE.txt

Relevant posts
--------------
*   [Stanford Word Segmenter is available on NuGet](http://sergeytihon.wordpress.com/2013/09/09/stanford-word-segmenter-is-available-on-nuget/)

*)
