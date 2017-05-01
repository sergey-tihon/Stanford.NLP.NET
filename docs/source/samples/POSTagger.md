 - tagline: What is Stanford.NLP.POSTagger?

# Stanford.NLP.POSTagger

 <div class="snlp-actions">
  <div class="row">
    <div class="col-sm-6">
      <h2>Install NuGet package</h2>
      <i class="fa" aria-hidden="true"><img src="../images/nuget.png" style="width:40px;" /></i>
      <h3 class="actionlink">
        <a href="https://www.nuget.org/packages/Stanford.NLP.POSTagger/">Stanford.NLP.POSTagger</a>
      </h3>
    </div>
    <div class="col-sm-6">
      <h2>Download models</h2>
      <i class="fa fa-download" aria-hidden="true"></i>
      <h3 class="actionlink">
        <a href="http://nlp.stanford.edu/software/stanford-postagger-full-2016-10-31.zip">Download POS Tagger 3.7.0</a>
      </h3>
    </div>
  </div>
  <div class="row">
    <div class="col-sm-6">
      <h2>Check out .NET Samples</h2>
      <i class="fa fa-book" aria-hidden="true"></i>
      <h3 class="actionlink">
        <a href="/samples.html#Stanford-Log-linear-Part-Of-Speech-POS-Tagger">Open .NET Samples</a>
      </h3>
    </div>
    <div class="col-sm-6">
      <h2>Read official Java docs</h2>
      <i class="fa" aria-hidden="true"><img src="../images/logo.jpg" style="width:40px;" /></i>
      <h3 class="actionlink">
        <a href="https://nlp.stanford.edu/software/tagger.html">Go to Tagger page</a>
      </h3>
    </div>
  </div>
 </div>

>A Part-Of-Speech Tagger (`POS Tagger`) is a piece of software that reads text in some language and assigns parts of speech to each word (and other token), such as noun, verb, adjective, etc., although generally computational applications use more fine-grained POS tags like 'noun-plural'.
>
>The full download contains three trained `English` tagger models, an `Arabic` tagger model, a `Chinese` tagger model, and a German tagger model. Both versions include the same source and other required files. The tagger can be retrained on any language, given POS-annotated training text for the language.
>
>Part-of-speech name abbreviations: The English taggers use the Penn Treebank tag set. Here are some links to documentation of the Penn Treebank English POS tag set: [1993 Computational Linguistics article in PDF](http://acl.ldc.upenn.edu/J/J93/J93-2004.pdf), [AMALGAM page](http://www.comp.leeds.ac.uk/amalgam/tagsets/upenn.html), [Aoife Cahill's list](http://www.computing.dcu.ie/~acahill/tagset.html). See the included README-Models.txt in the models directory for more information about the tagsets for the other languages.
>
>The tagger is licensed under the `GNU General Public License`. Source is included. The package includes components for
>command-line invocation, running as a server, and a Java API. The tagger code is dual licensed (in a similar manner to MySQL, etc.). Open source licensing is under the full GPL, which allows many free uses. For distributors of proprietary software, [commercial licensing](http://otlportal.stanford.edu/techfinder/technology/ID=26062) is available.