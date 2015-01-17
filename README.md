[![AppVeyor build status](https://ci.appveyor.com/api/projects/status/ftkg1n73xcibqwmx)](https://ci.appveyor.com/project/sergey-tihon/stanford-nlp-net)
[![Travis build status](https://travis-ci.org/sergey-tihon/Stanford.NLP.NET.svg)](https://travis-ci.org/sergey-tihon/Stanford.NLP.NET)
Stanford.NLP for .NET
=====================

`Stanford.NLP for .NET` is a port of Stanford NLP distributions to .NET.

This project contains build scripts that recompile Stanford NLP `.jar` packages to .NET assemblies using [IKVM.NET](http://www.ikvm.net/), tests that help to be sure that recompiled packages are workable and [Stanford.NLP for .NET documentation site](http://sergey-tihon.github.io/Stanford.NLP.NET/) that hosts samples for all packages. All recompiled packages are available on [NuGet](https://www.nuget.org/packages?q=Stanford.NLP).


Versioning
----------

Versioning model used for NuGet packages is aligned to versioning used by Stanford NLP Group. 
For example, if you get `Stanford CoreNLP` distribution from [Stanford NLP site](http://www-nlp.stanford.edu/software/index.shtml) with version `3.3.1`, then the NuGet version of this package has a version `3.3.1.x`, where `x` is the greatest that is available on NuGet. Last number is used for internal versioning of .NET assemblies.

Licensing
----------
All these software distributions are open source, **licensed under the [GNU General Public License](http://www.gnu.org/licenses/gpl-2.0.html)** (v2 or later). Note that this is the *full* GPL, which allows many free uses, but *does not allow* its incorporation into any type of distributed [proprietary software](http://www.gnu.org/licenses/gpl-faq.html#GPLInProprietarySystem), even in part or in translation. **Commercial licensing** is also available; please contact [The Stanford Natural Language Processing Group](http://www-nlp.stanford.edu/) if you are interested. 

---

Project structure was created based on the structure proposed by [FSharp.ProjectScaffold](https://github.com/fsprojects/FSharp.ProjectScaffold).
