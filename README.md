[![Build Status](https://github.com/sergey-tihon/Stanford.NLP.NET/workflows/Build%20and%20Test/badge.svg?branch=master)](https://github.com/fsprojects/SwaggerProvider/actions?query=branch%3Amaster)

Stanford.NLP for .NET
=====================

`Stanford.NLP for .NET` is a port of Stanford NLP distributions to .NET.

This project contains build scripts that recompile Stanford NLP `.jar` packages to .NET assemblies using [IKVM.NET](http://www.ikvm.net/), tests that help to be sure that recompiled packages are workable and [Stanford.NLP for .NET documentation site](http://sergey-tihon.github.io/Stanford.NLP.NET/) that hosts samples for all packages. All recompiled packages are available on [NuGet](https://www.nuget.org/packages?q=Stanford.NLP).

- [![NuGet Badge](https://buildstats.info/nuget/Stanford.NLP.CoreNLP)](https://www.nuget.org/packages/Stanford.NLP.CoreNLP/) - Stanford.NLP.CoreNLP
- [![NuGet Badge](https://buildstats.info/nuget/Stanford.NLP.NER)](https://www.nuget.org/packages/Stanford.NLP.NER/) - Stanford.NLP.NER
- [![NuGet Badge](https://buildstats.info/nuget/Stanford.NLP.Parser)](https://www.nuget.org/packages/Stanford.NLP.Parser/) - Stanford.NLP.Parser
- [![NuGet Badge](https://buildstats.info/nuget/Stanford.NLP.POSTagger)](https://www.nuget.org/packages/Stanford.NLP.POSTagger/) - Stanford.NLP.POSTagger
- [![NuGet Badge](https://buildstats.info/nuget/Stanford.NLP.Segmenter)](https://www.nuget.org/packages/Stanford.NLP.Segmenter/) - Stanford.NLP.Segmenter


Versioning
----------

Versioning model used for NuGet packages is aligned to versioning used by Stanford NLP Group. 
For example, if you get `Stanford CoreNLP` distribution from [Stanford NLP site](http://www-nlp.stanford.edu/software/index.shtml) with version `3.3.1`, then the NuGet version of this package has a version `3.3.1.x`, where `x` is the greatest that is available on NuGet. Last number is used for internal versioning of .NET assemblies.

Licensing of the code/content of this repo
---------------------------
The source code of this repo(build scripts, integration tests, docs and samples) under the [MIT](LICENSE) license.

Licensing of NuGet packages
---------------------------
All these software distributions are open source, **licensed under the [GNU General Public License](http://www.gnu.org/licenses/gpl-2.0.html)** (v2 or later). Note that this is the *full* GPL, which allows many free uses, but *does not allow* its incorporation into any type of distributed [proprietary software](http://www.gnu.org/licenses/gpl-faq.html#GPLInProprietarySystem), even in part or in translation. **Commercial licensing** is also available; please contact [The Stanford Natural Language Processing Group](http://www-nlp.stanford.edu/) if you are interested. 

---

Project structure was created based on the structure proposed by [FSharp.ProjectScaffold](https://github.com/fsprojects/FSharp.ProjectScaffold).
