All samples were moved to [FSharp.NLP.Stanford](https://github.com/sergey-tihon/FSharp.NLP.Stanford) project.

The Stanford Natural Language Processing Samples, in F#
===================================================

This project contains [Stanford NLP](http://www-nlp.stanford.edu/) assemblies (compiled from *.jar files using [IKVM.NET Bytecode Compiler](http://www.ikvm.net/userguide/ikvmc.html)) with samples translated to [F#](http://fsharp.org/).


### [Stanford Parser](http://www-nlp.stanford.edu/software/lex-parser.shtml) (v3.2.0 - 2013-06-20) [more](http://sergeytihon.wordpress.com/2013/02/05/nlp-stanford-parser-with-f-net/)

Implementations of probabilistic natural language parsers, both highly optimized PCFG and dependency parsers, and a lexicalized PCFG parser in Java. Includes: [Online parser demo](http://nlp.stanford.edu:8080/parser/), [Stanford Dependencies](http://nlp.stanford.edu/software/stanford-dependencies.shtml) page, and [Parser FAQ](http://www-nlp.stanford.edu/software/parser-faq.shtml).

Available on NuGet as [Stanford.NLP.Parser](https://www.nuget.org/packages/Stanford.NLP.Parser/)

### [Stanford POS Tagger](http://www-nlp.stanford.edu/software/tagger.shtml) (v3.2.0 - 2013-06-20) [more](http://sergeytihon.wordpress.com/2013/02/08/nlp-stanford-pos-tagger-with-f-net/)

A maximum-entropy (CMM) part-of-speech (POS) tagger for English, Arabic, Chinese, French, and German, in Java.

Available on NuGet as [Stanford.NLP.POSTagger](https://www.nuget.org/packages/Stanford.NLP.POSTagger/)

### [Stanford Named Entity Recognizer](http://www-nlp.stanford.edu/software/CRF-NER.shtml) (v3.2.0 - 2013-06-20) [more](http://sergeytihon.wordpress.com/2013/02/16/nlp-stanford-named-entity-recognizer-with-f-net/)

A Conditional Random Field sequence model, together with well-engineered features for Named Entity Recognition in English and German. [Online NER demo](http://nlp.stanford.edu:8080/ner/)

Available on NuGet as [Stanford.NLP.NER](https://www.nuget.org/packages/Stanford.NLP.NER/)

----------

All libraries are distributed only with models which were used in code samples. Full model set is available on the [The Stanford Natural Language Processing Group](http://www-nlp.stanford.edu/software/index.shtml) site.

----------

**All these software distributions are open source, licensed under the GNU General Public License (v2 or later). Note that this is the full GPL, which allows many free uses, but does not allow its incorporation into any type of distributed proprietary software, even in part or in translation. Commercial licensing is also available; please contact [The Stanford Natural Language Processing Group](http://www-nlp.stanford.edu/) if you are interested.**