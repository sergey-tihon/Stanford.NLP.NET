# Stanford.NLP for .NET

<a href="https://stanfordnlp.github.io/CoreNLP/">
  <img src="https://stanfordnlp.github.io/CoreNLP/assets/images/corenlp-title.png">
</a>

### Key facts

- `Stanford.NLP.NET` is built on top of [IKVM.NET](http://weblog.ikvm.net/) (`Java` VM that runs on top of `.NET` VM).
- It supports only full `.NET` framework and does not work on `.NET Core` and `.NET 5+`.
- You should always start from [CoreNLP](corenlp) master package that provide full range of features (other packages are exist for historical/compatibility reasons)
- Use [official CoreNLP site](https://stanfordnlp.github.io/CoreNLP/demo.html) for latest docs, samples and demos. Site is maintained by library authors.
- Use [StackOverflow](https://stackoverflow.com/questions/tagged/stanford-nlp) to ask all "how to" nlp-related questions.

### Licensing

All these software distributions are open source, **licensed under the [GNU General Public License](https://github.com/sergey-tihon/Stanford.NLP.NET/blob/master/LICENSE.txt) (v2 or later)**. Note that this is the *full* GPL, which allows many free uses, but *does not* allow its incorporation into any type of distributed proprietary software, even in part or in translation. **Commercial licensing** is also available; please contact [The Stanford Natural Language Processing Group](http://www-nlp.stanford.edu/) if you are interested.

### Versioning

Versioning model used for NuGet packages is aligned to versioning used by Stanford NLP Group. For example, if you get Stanford CoreNLP distribution from [Stanford NLP site](https://nlp.stanford.edu/software/index.shtml) with version `3.9.1`, then the NuGet version of this package has a version `3.9.1.x`, where `x` is the greatest that is available on NuGet. Last number is used for internal versioning of .NET assemblies.