# Stanford.NLP for .NET

<a href="https://stanfordnlp.github.io/CoreNLP/">
  <img src="https://stanfordnlp.github.io/CoreNLP/assets/images/corenlp-title.png">
</a>

### Key facts

- `Stanford.NLP.NET` is built on top of [IKVM.NET](https://github.com/ikvm-revived/ikvm) (`Java` VM that runs on top of `.NET` VM).
- It supports full `.NET` framework and `.NET Core`.
- You should always start from the main [CoreNLP](https://www.nuget.org/packages/Stanford.NLP.CoreNLP/) package, which provides the full range of features. Other, related packages exist only for historical/compatibility reasons.
- Use the [official CoreNLP site](https://stanfordnlp.github.io/CoreNLP/demo.html) for the latest docs, samples and demos. This site is maintained by the library's authors.
- Use [StackOverflow](https://stackoverflow.com/questions/tagged/stanford-nlp) to ask all "how to" NLP-related questions.

### Licensing

All these software distributions are open source, **licensed under the [GNU General Public License](https://github.com/sergey-tihon/Stanford.NLP.NET/blob/master/LICENSE.txt) (v2 or later)**. Note that this is the *full* GPL, which allows many free uses, but *does not* allow its incorporation into any type of distributed proprietary software, even in part or in translation. **Commercial licensing** is also available; please contact [The Stanford Natural Language Processing Group](http://www-nlp.stanford.edu/) if you are interested.

### Versioning

The versioning model used for NuGet packages is aligned to versioning used by the Stanford NLP Group. For example, if you get a Stanford CoreNLP distribution from [Stanford NLP site](https://nlp.stanford.edu/software/index.shtml) with version `3.9.1`, then the NuGet version of this package has a version `3.9.1.x`, where `x` is the greatest that is available on NuGet. The last number is used for internal versioning of .NET assemblies.
