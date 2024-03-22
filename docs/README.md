# Stanford.NLP for .NET

<a href="https://stanfordnlp.github.io/CoreNLP/">
  <img src="https://stanfordnlp.github.io/CoreNLP/assets/images/corenlp-title.png">
</a>

<Note type="warning">

2024-03-21: All [Stanford.NLP NuGet packages](https://www.nuget.org/packages?q=Stanford.NLP) are marked as deprecated (legacy and no longer maintained). Please use the [official Stanford CoreNLP Maven package](https://mvnrepository.com/artifact/edu.stanford.nlp/stanford-corenlp) instead, along with the [IKVM.Maven.Sdk](https://github.com/ikvmnet/ikvm-maven).

</Note>

### Getting started with `IKVM.Maven.Sdk`

Copy the following lines into your project file:

```xml
<ItemGroup>
    <PackageReference Include="IKVM" Version="8.7.5" />
    <PackageReference Include="IKVM.Maven.Sdk" Version="1.6.8" PrivateAssets="all" />
</ItemGroup>
<ItemGroup>
    <MavenReference Include="edu.stanford.nlp:stanford-corenlp" Version="4.5.6"/>
    <MavenReference Include="edu.stanford.nlp:stanford-corenlp" Version="4.5.6" Classifier="models" />
</ItemGroup>
```

First two `PackageReference`es add [IKVM](https://github.com/ikvmnet/ikvm) and [IKVM.Maven.Sdk](https://github.com/ikvmnet/ikvm-maven) to your project. The `MavenReference`es are used to download and compile Stanford CoreNLP `.jar` files and models.

The next step is to manually load the assembly with models into you process:

```csharp
var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
var modelsAssemblyPath = Path.Combine(baseDirectory, "edu.stanford.nlp.corenlp_english_models.dll");
Assembly.LoadFile(modelsAssemblyPath);
```

[Previously](http://sergey-tihon.github.io/Stanford.NLP.NET/#/FAQ#stanfordnlpcorenlp-not-loading-models) you have to manually find `*.jar` file with models, unpack it and temporary change the current directory to the unpacked folder. This is no longer needed with `IKVM.Maven.Sdk`. The `*.jar` with models is automatically downloaded and compiled to `*.dll` with the same name. The only thing that you have to do it is to load this assembly into your process.

You are ready now to use Stanford CoreNLP in your .NET project.

```csharp
var text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply.";

// Annotation pipeline configuration
var props = new Properties();
props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse");
props.setProperty("ner.useSUTime", "false");

var pipeline = new StanfordCoreNLP(props);

// Annotation
var annotation = new Annotation(text);
pipeline.annotate(annotation);
```

P.S. More samples are available in the [Stanford.NLP.NET](http://sergey-tihon.github.io/Stanford.NLP.NET) repository.

