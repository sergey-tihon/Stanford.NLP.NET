 - tagline: What is Stanford.NLP.Parser?

# Stanford.NLP.Parser

 <div class="snlp-actions">
  <div class="row">
    <div class="col-sm-6">
      <h2>Install NuGet package</h2>
      <i class="fa" aria-hidden="true"><img src="../images/nuget.png" style="width:40px;" /></i>
      <h3 class="actionlink">
        <a href="https://www.nuget.org/packages/Stanford.NLP.Parser/">Stanford.NLP.Parser</a>
      </h3>
    </div>
    <div class="col-sm-6">
      <h2>Download models</h2>
      <i class="fa fa-download" aria-hidden="true"></i>
      <h3 class="actionlink">
        <a href="http://nlp.stanford.edu/software/stanford-parser-full-2016-10-31.zip">Download Parser 3.7.0</a>
      </h3>
    </div>
  </div>
  <div class="row">
    <div class="col-sm-6">
      <h2>Check out .NET Samples</h2>
      <i class="fa fa-book" aria-hidden="true"></i>
      <h3 class="actionlink">
        <a href="../samples.html#Stanford-Parser">Open .NET Samples</a>
      </h3>
    </div>
    <div class="col-sm-6">
      <h2>Read official Java docs</h2>
      <i class="fa" aria-hidden="true"><img src="../images/logo.jpg" style="width:40px;" /></i>
      <h3 class="actionlink">
        <a href="https://nlp.stanford.edu/software/lex-parser.shtml">Go to Parser page</a>
      </h3>
    </div>
  </div>
 </div>

>A natural language parser is a program that works out the grammatical **structure of sentences**, for instance, which groups of words go together (as "phrases") and which words are the **subject** or **object** of a verb. Probabilistic parsers use knowledge of language gained from hand-parsed sentences to try to produce the _most likely_ analysis of new sentences. These statistical parsers still make some mistakes, but commonly work rather well. Their development was one of the biggest breakthroughs in natural language processing in the 1990s. You can [try out our parser online](http://nlp.stanford.edu:8080/parser/).
>
>The lexicalized probabilistic parser implements a factored product model, with separate `PCFG` phrase structure and lexical dependency experts, whose preferences are combined by efficient exact inference, using an `A*` algorithm. Alternatively the software can be used simply as an accurate unlexicalized stochastic context-free grammar parser. Either of these yields a good performance statistical parsing system. A GUI (Java) is provided for viewing the phrase structure tree output of the parser.
>
>As well as providing an **English** parser, the parser can be and has been adapted to work with other languages. A **Chinese** parser based on the Chinese Treebank, a **German** parser based on the Negra corpus and **Arabic** parsers based on the Penn Arabic Treebank are also included. The parser has also been used for other languages, such as Italian, Bulgarian, and Portuguese.
>
>The parser provides [Stanford Dependencies](https://nlp.stanford.edu/software/stanford-dependencies.shtml) output as well as phrase structure trees. Typed dependencies are otherwise known **grammatical relations**. This style of output is available only for English and Chinese. For more details, please refer to the [Stanford Dependencies webpage](https://nlp.stanford.edu/software/stanford-dependencies.shtml).
>
>The parser is available for download, licensed under the `GNU General Public License` (v2 or later). Source is included. The package includes components for command-line invocation, a Java parsing GUI, and a Java API. The parser code is dual licensed (in a similar manner to MySQL, etc.). Open source licensing is under the full GPL, which allows many free uses. For distributors of proprietary software, commercial licensing is available.