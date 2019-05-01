(**
 - tagline: Package: Stanford.NLP.CoreNLP

# Simple CoreNLP

This page is direct translation of the [original Simple CoreNLP page](https://stanfordnlp.github.io/CoreNLP/simple.html)

### Simple CoreNLP

In addition to the fully-featured annotator pipeline interface to CoreNLP, Stanford provides a simple API for users who do not need a lot of customization. The intended audience of this package is users of `CoreNLP` who want **"just use nlp"** to work as fast and easily as possible, and do not care about the details of the behaviors of the algorithms.

An example usage is given below:

*)
(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../../bin/Stanford.NLP.CoreNLP/lib/"
#I "../../../packages/test/IKVM/lib/"

(**
*)
#r "IKVM.OpenJDK.Core.dll"
#r "IKVM.OpenJDK.Util.dll"
#r "stanford-corenlp-3.9.1.dll"

open System
open java.util
open edu.stanford.nlp.simple

// Path to the folder with models extracted from `stanford-corenlp-3.9.1-models.jar`
let jarRoot = (__SOURCE_DIRECTORY__)+ @"/../../../data/paket-files/nlp.stanford.edu/"
                                    + @"stanford-corenlp-full-2018-10-05/models/"
System.IO.Directory.SetCurrentDirectory(jarRoot)

// Custom properties for annotators
let props = Properties()
props.setProperty("ner.useSUTime","0") |> ignore

let sent : Sentence = new Sentence("Lucy is in the sky with diamonds.")
let nerTags : List = sent.nerTags(props);
let firstPOSTag : string = sent.posTag(0);
// [fsi:> ]
// [fsi:val sent : Sentence = Lucy is in the sky with diamonds.]
// [fsi:val nerTags : List = seq ["PERSON"; "O"; "O"; "O"; ...]]
// [fsi:val firstPOSTag : string = "NNP"]

(**

The API is included in the `CoreNLP` release from `3.6.0` onwards. Visit the [download page](https://stanfordnlp.github.io/CoreNLP/download.html) to download CoreNLP; make sure to set current directory to folder with models!

<code>
    <b>Note:</b> If you use Simple CoreNLP API, your current directory should always be set to the root folder of an unzipped model, since Simple CoreNLP loads models lazily. <a href="../faq.html#Stanford-NLP-CoreNLP-not-loading-models">Read more about model loading</a>
</code>

### Advantages and Disadvantages

This interface offers a number of advantages (and a few disadvantages – see below) over the default annotator pipeline:

- **Intuitive Syntax** Conceptually, documents and sentences are stored as objects, and have functions corresponding to annotations you would like to retrieve from them.

- **Lazy Computation** Annotations are run as needed only when requested. This allows you to “change your mind” later in a program and request new annotations.

- **No `NullPointerException`s** Lazy computation allows us to ensure that no function will ever return null. Items which may not exist are wrapped inside of an `Optional` to clearly mark that they may be empty.

- **Fast, Robust Serialization** All objects are backed by [protocol buffers](https://developers.google.com/protocol-buffers/?hl=en), meaning that serialization and deserialization is both very easy and very fast. In addition to being easily readable from other languages, our experiments show this to be over an order of magnitude faster than the default Java serialization.

- **Maintains Thread Safety** Like the `CoreNLP` pipeline, this wrapper is thread-safe.


In exchange for these advantages, users should be aware of a few disadvantages:

- **Less Customizability** Although the ability to pass properties to annotators is supported, it is significantly more clunky than the annotation pipeline interface, and is generally discouraged.

- **Possible Nondeterminism** There is no guarantee that the same algorithm will be used to compute the requested function on each invocation. For example, if a dependency parse is requested, followed by a constituency parse, we will compute the dependency parse with the [Neural Dependency Parser](https://nlp.stanford.edu/software/nndep.shtml), and then use the [Stanford Parser](https://nlp.stanford.edu/software/lex-parser.shtml) for the constituency parse. If, however, you request the constituency parse before the dependency parse, we will use the Stanford Parser for both.



### Usage

There are two main classes in the interface: `Document` and `Sentence`. Tokens are represented as array elements in a sentence; e.g., to get the lemma of a token, get the lemmas array from the sentence and index it at the appropriate index. A constructor is provided for both the `Document` and `Sentence` class. For the former, the text is treated as an entire document containing potentially multiple sentences. For the latter, the text is forced to be interpreted as a single sentence.

An example program using the interface is given below:

*)

open edu.stanford.nlp.simple

// Create a document. No computation is done yet.
let doc : Document = new Document("add your text here! It can contain multiple sentences.");
let sentences = doc.sentences().toArray()
for sentObj in sentences do  // Will iterate over two sentences
    let sent : Sentence = sentObj :?> Sentence
    // We're only asking for words -- no need to load any models yet
    Console.WriteLine("The second word of the sentence '{0}' is {1}", sent, sent.word(1));
    // When we ask for the lemma, it will load and run the part of speech tagger
    Console.WriteLine("The third lemma of the sentence '{0}' is {1}", sent, sent.lemma(2));
    // When we ask for the parse, it will load and run the parser
    Console.WriteLine("The parse of the sentence '{0}' is {1}", sent, sent.parse());
// [fsi:> ]
// [fsi:The second word of the sentence 'add your text here!' is your]
// [fsi:The third lemma of the sentence 'add your text here!' is text]
// [fsi:The parse of the sentence 'add your text here!' is (ROOT (S (VP (VB add) (NP (PRP$ your) (NN text)) (ADVP (RB here))) (. !)))]
// [fsi:The second word of the sentence 'It can contain multiple sentences.' is can]
// [fsi:The third lemma of the sentence 'It can contain multiple sentences.' is contain]
// [fsi:The parse of the sentence 'It can contain multiple sentences.' is (ROOT (S (NP (PRP It)) (VP (MD can) (VP (VB contain) (NP (JJ multiple) (NNS sentences)))) (. .)))]

(**

### Supported Annotators

The interface is not guaranteed to support all of the annotators in the CoreNLP pipeline. However, most common annotators are supported. A list of these, and their invocation, is given below. Functionality is the plain-english description of the task to be performed. The second column lists the analogous CoreNLP annotator for that task. The implementing class and function describe the class and function used in this wrapper to perform the same tasks.

Functionality |	Anootator in CoreNLP | Implementation class | Function
--------------|----------------------|--------------------|---------
Tokenization | tokenize | `Sentence` |  `.words()` / `.word(int)`
Sentence Splitting | ssplit | `Document` | `.sentences()` / `.sentence(int)`
Part of Speech Tagging | pos | `Sentence` |	`.posTags()` / `.posTag(int)`
Lemmatization |	lemma |	`Sentence` | `.lemmas()` / `.lemma(int)`
Named Entity Recognition | ner | `Sentence` | `.nerTags()` / `.nerTag(int)`
Constituency Parsing | parse | `Sentence` | `.parse()`
Dependency Parsing | depparse | `Sentence` | `.governor(int)` / `.incomingDependencyLabel(int)`
Coreference Resolution | dcoref | `Document` | `.coref()`
Natural Logic Polarity | natlog | `Sentence` | `.natlogPolarities()` / `natlogPolarity(int)`
Open Information Extraction | openie | `Sentence` |	`.openie()` / `.openieTriples()`


### Miscellaneous Extras

Some potentially useful utility functions are implemented in the `SentenceAlgorithms` class. These can be called from a `Sentence` object with, e.g.:
*)

open edu.stanford.nlp.ie.machinereading.structure

let sent2 : Sentence = new Sentence("your text should go here");
sent2.algorithms().headOfSpan(new Span(0, 2));  // Should return 1
// [fsi:> ]
// [fsi:val sent2 : Sentence = your text should go here]
// [fsi:val it : int = 1]

(**

A selection of useful algorithms are:

- **`headOfSpan(Span)`** Finds the index of the head word of the given span. So, for example, _United States_ president _Barack Obama_ would return _Obama_.

- **`dependencyPathBetween(int, int)`** Returns the dependency path between the words at the given two indices. This is returned as a list of `string` objects, meant primarily as an input to a featurizer.

*)