(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use 
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"

(**
Stanford NLP Software for .NET
===================

<img src="img/cover.png" style="float:right;width:363px;margin: 0px 0px 10px 10px;" />

The Stanford NLP Group makes parts of Natural Language Processing software available to everyone. These are statistical NLP 
toolkits for various major computational linguistics problems. They can be incorporated into applications with human language 
technology needs.

Getting started
---------------

*   Choose the package that is the most suitable for your task. If your task is complex and you need a deep analysis - select [Stanford CoreNLP](/Stanford.NLP.NET/StanfordCoreNLP.html).
*   Install selected [NuGet package](https://www.nuget.org/packages?q=Stanford.NLP).
*   Download original ZIP archive for selected package from The Stanford NLP Group site. (Direct links are mentioned on the packages pages)
*   Unzip *.jar file with models if such one exists.
*   You are ready to start, please look at examples.

Note: Do not try to reference several NuGet packages from your solution. They are incompatible. If you need more than one - you should
reference [Stanford CoreNLP](/Stanford.NLP.NET/StanfordCoreNLP.html) package. All features are packed inside.

Supported software distributions
--------------------------------

All these software distributions are open source, licensed under the [GNU General Public License][license] (v2 or later). Note that this is 
the full GPL, which allows many free uses, but does not allow its incorporation into any type of distributed proprietary software, 
even in part or in translation. Commercial licensing is also available; please contact 
[The Stanford Natural Language Processing Group](http://www-nlp.stanford.edu/) if you are interested.

<div class="span9" style="margin-left: 0px;">
    <div class="row-fluid">
        <div class="span4">
            <h2>Stanford CoreNLP</h2>
            <p style="min-height: 110px;">
               An integrated suite of natural language processing tools for English and (mainland) Chinese, including tokenization, 
               part-of-speech tagging, named entity recognition, parsing, and coreference.<br/>
            See also: <a href="http://nlp.stanford.edu:8080/corenlp/">Online CoreNLP demo</a>.</p>
            <p><a class="btn" href="/Stanford.NLP.NET/StanfordCoreNLP.html">View details »</a></p>
        </div><!--/span-->
        <div class="span4">
            <h2>Stanford Parser</h2>
            <p style="min-height: 110px;">
               Implementations of probabilistic natural language parsers: highly optimized PCFG and dependency parsers, a 
               lexicalized PCFG parser, and a deep learning reranker.<br/>
               See also: <a href="http://nlp.stanford.edu:8080/parser/">Online parser demo</a> and 
               the <a href="http://nlp.stanford.edu/software/stanford-dependencies.shtml">Stanford Dependencies page</a>.</p>
            <p><a class="btn" href="/Stanford.NLP.NET/StanfordParser.html">View details »</a></p>
        </div><!--/span-->
        <div class="span4">
            <h2>Stanford POS Tagger</h2>
            <p style="min-height: 110px;">
               A maximum-entropy (CMM) part-of-speech (POS) tagger for English, Arabic, Chinese, French, and German.</p>
            <p><a class="btn" href="/Stanford.NLP.NET/StanfordPOSTagger.html">View details »</a></p>
        </div><!--/span-->
    </div><!--/row-->
    <div class="row-fluid">
        <div class="span4">
            <h2>Stanford Named Entity Recognizer</h2>
            <p style="min-height: 110px;">
               A Conditional Random Field sequence model, together with well-engineered features for Named Entity Recognition in English, Chinese, and German.<br/>
               See also: <a href="http://nlp.stanford.edu:8080/ner/">Online NER demo</a></p>
            <p><a class="btn" href="/Stanford.NLP.NET/StanfordNER.html">View details »</a></p>
        </div><!--/span-->
        <div class="span4">
            <h2>Stanford Word Segmenter</h2>
            <p style="min-height: 110px;">
                A CRF-based word segmenter. Supports Arabic and Chinese.</p>
            <p><a class="btn" href="/Stanford.NLP.NET/StanfordWordSegmenter.html">View details »</a></p>
        </div><!--/span-->
        <div class="span4">
            <h2>Stanford Temporal Tagger (SUTime)</h2>
            <p style="min-height: 110px;">
               A rule-based temporal tagger for English text.<br/>
               See also: <a href="http://nlp.stanford.edu:8080/sutime">Online SUTime demo</a></p>
            <p><a class="btn" href="/Stanford.NLP.NET/StanfordSUTime.html">View details »</a></p>
        </div><!--/span-->
    </div><!--/row-->
</div>

Contributing
--------------------------

The project is hosted on [GitHub][gh] where you can [report issues][issues], fork 
the project and submit pull requests. You might also want to read [library notes][readme] 
to understand how it works.

  [gh]: https://github.com/sergey-tihon/Stanford.NLP.NET
  [issues]: https://github.com/sergey-tihon/Stanford.NLP.NET/issues
  [readme]: https://github.com/sergey-tihon/Stanford.NLP.NET/blob/master/README.md
  [license]: https://github.com/sergey-tihon/Stanford.NLP.NET/blob/master/LICENSE.txt
*)
