# Stanford CoreNLP

<Note type="warning">
This package is deprecated and should not be used. <a href="../">Read more.</a>
</Note>
<img align="right" width="150" src="images/logo.png">

An integrated suite of natural language processing tools for English and (mainland) Chinese, including tokenization,part-of-speech tagging, named entity recognition, parsing, and coreference.

[![](https://buildstats.info/nuget/Stanford.NLP.CoreNLP)](https://www.nuget.org/packages/Stanford.NLP.CoreNLP/)

See also: [corenlp.run](http://corenlp.run) and [online CoreNLP demo](http://nlp.stanford.edu:8080/corenlp/).

> **Stanford CoreNLP** provides a set of natural language analysis tools which can take raw English language text input and give the base forms of words, their parts of speech, whether they are names of companies, people, etc., normalize dates, times, and numeric quantities, and mark up the structure of sentences in terms of phrases and word dependencies, and indicate which noun phrases refer to the same entities. Stanford CoreNLP is an integrated framework, which makes it very easy to apply a bunch of language analysis tools to a piece of text. Starting from plain text, you can run all the tools on it with just two lines of code. Its analyses provides the foundational building blocks for higher-level and domain-specific text understanding applications.
>
> `Stanford CoreNLP integrates all Stanford NLP tools`, including the part-of-speech (POS) tagger, the named entity recognizer (NER), the parser, the coreference resolution system, and the sentiment analysis tools, and provides model files for analysis of English. The goal of this project is to enable people to quickly and painlessly get complete linguistic annotations of natural language texts. It is designed to be highly flexible and extensible. With a single option, you can choose which tools should be enabled and which should be disabled.
>
> The Stanford CoreNLP code is licensed under the **GNU General Public License** (v2 or later). Note that this is the full GPL, which allows many free uses, but not its use in distributed proprietary software.

### Getting started

1. Choose the package that is the most suitable for your task. If your task is complex and you need a deep analysis - select [Stanford CoreNLP](samples/CoreNLP.html)
1. Install [NuGet package](https://www.nuget.org/packages/Stanford.NLP.CoreNLP/).
1. Download [CoreNLP ZIP archive](https://nlp.stanford.edu/software/stanford-corenlp-latest.zip).
1. Unzip `*.jar` file with models.
1. You are ready to start, please look at [samples](/samples)
1. Read more details on [official CoreNLP page](https://stanfordnlp.github.io/CoreNLP/).

<Note>Do not try to reference several NuGet packages from your project. They are incompatible with each other! If you need more than one - you should reference [Stanford CoreNLP](samples/CoreNLP.html) package. All features are packed inside.</Note>
