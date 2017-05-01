 - tagline: Questions and Answers

# Frequently asked questions (FAQ)

### How this works? Where is C# source code for Stanford.NLP.NET?

`Stanford.NLP.NET` built on top of [IKVM.NET](http://weblog.ikvm.net/) (`Java` VM that run on top of `.NET` VM). There is no `C#` source code and no efforts for `Java->C#` code translation. This project uses [IKVM.NET Bytecode Compiler](http://www.ikvm.net/userguide/ikvmc.html) that convert `Java` bytecode (`*.jar`) to `.NET` (`*.dll`).

### I heard that IKVM.NET is dead, what will be with this project?

Yes, there is an official announcement of [The End of IKVM.NET](http://weblog.ikvm.net/2017/04/21/TheEndOfIKVMNET.aspx), but it does not mean the end of `Stanford.NLP.NET`. Current stable version of `IKVM.NET` has perfect support of `Java 8` and we have confirmation from `Stanford NLP` group, that `CoreNLP` will remain `Java 8` compatible for foreseeable future.

<blockquote class="twitter-tweet" data-lang="en"><p lang="en" dir="ltr"><a href="https://twitter.com/foxyjackfox">@foxyjackfox</a> <a href="https://twitter.com/sergey_tihon">@sergey_tihon</a> We think CoreNLP can remain Java 8 compatible for the currently foreseeable future.</p>&mdash; Stanford NLP Group (@stanfordnlp) <a href="https://twitter.com/stanfordnlp/status/856538474089820161">April 24, 2017</a></blockquote> <script async src="//platform.twitter.com/widgets.js" charset="utf-8"></script>

### What NuGet package should I reference and use?

It really depends on your task, you always should try to choose one (referencing of more than one package is not supported scenario). If you task is rather complicated or you do not know which one to choose - use [Stanford.NLP.CoreNLP](samples/CoreNLP.html). This is an umbrella package that integrates all Stanford tools.

### Stanford.NLP.CoreNLP not loading models

This is probably [the most common problem](http://stackoverflow.com/questions/40814503/stanford-nlp-for-net-not-loading-models) with `CoreNLP` (especially for newcomers). `stanfrod-corenlp-ful-*.zip` archive contains files `stanford-corenlp-3.7.0-models.jar` with models inside ( this is zip archive). In Java world, you add this `jar` on the class path, and it automatically resolve location of models inside the archive.

CoreNLP has a file [DefaultPaths.java](https://github.com/stanfordnlp/CoreNLP/blob/ef653d4f64f82b0395f72d43cc7add8e61752fee/src/edu/stanford/nlp/pipeline/DefaultPaths.java) that specify path to model file. So when you instantiate `StanfordCoreNLP` with `Properties` object that does not specify models location, you should guarantee that models could be found by default path (related to `Environment.CurrentDirectory`).

The simplest way to guarantee existence of files like `Environment.CurrentDirectory + "edu/stanford/nlp/models/ner/english.all.3class.distsim.crf.ser.gz"` is to unzip jar archive to the folder, sand temporary change the current directory to unzipped folder.

```csharp
var jarRoot = "nlp.stanford.edu/stanford-corenlp-full-2016-10-31/jar-modules/";
...
var curDir = Environment.CurrentDirectory;
Directory.SetCurrentDirectory(jarRoot);
var pipeline = new StanfordCoreNLP(props);
Directory.SetCurrentDirectory(curDir);
```

The other way is to specify paths to all models that your pipeline need (it actually depends on the list of `annotators`).
This option is more complicated because you have to find correct property keys, and specify paths to all used model. But it may be useful if you want to minimize the size of you deployment package.

```csharp
var props = new Properties();
props.put("annotators", "tokenize, ssplit, pos, lemma, ner, depparse");
props.put("ner.model",
          "edu/stanford/nlp/models/ner/english.all.3class.distsim.crf.ser.gz");
props.put("ner.applyNumericClassifiers", "false");
var pipeline = new StanfordCoreNLP(props);
```

### StackOverflow Exception

You probably try to execute code under `IIS Express`. So you have two options here: [deploy to real IIS](https://github.com/sergey-tihon/Stanford.NLP.NET/issues/15#issuecomment-77526331) or [run under 64bit version of IIS Express](https://github.com/sergey-tihon/Stanford.NLP.NET/issues/43#issuecomment-190186401). In VS2015 it was under Tools> Options> Projects and Solutions > Web Projects > Use the 64 bit version of IIS Express for web sites and projects