 - tagline: What is Stanford.NLP.NER?

# Stanford.NLP.NER

 <div class="snlp-actions">
  <div class="row">
    <div class="col-sm-6">
      <h2>Install NuGet package</h2>
      <i class="fa" aria-hidden="true"><img src="../images/nuget.png" style="width:40px;" /></i>
      <h3 class="actionlink">
        <a href="https://www.nuget.org/packages/Stanford.NLP.NER/">Stanford.NLP.NER</a>
      </h3>
    </div>
    <div class="col-sm-6">
      <h2>Download models</h2>
      <i class="fa fa-download" aria-hidden="true"></i>
      <h3 class="actionlink">
        <a href="https://nlp.stanford.edu/software/stanford-ner-2018-02-27.zip">Download NER 3.9.1</a>
      </h3>
    </div>
  </div>
  <div class="row">
    <div class="col-sm-6">
      <h2>Check out .NET Samples</h2>
      <i class="fa fa-book" aria-hidden="true"></i>
      <h3 class="actionlink">
        <a href="../samples.html#Stanford-Named-Entity-Recognizer-NER">Open .NET Samples</a>
      </h3>
    </div>
    <div class="col-sm-6">
      <h2>Read official Java docs</h2>
      <i class="fa" aria-hidden="true"><img src="../images/logo.jpg" style="width:40px;" /></i>
      <h3 class="actionlink">
        <a href="https://nlp.stanford.edu/software/CRF-NER.html">Go to NER page</a>
      </h3>
    </div>
  </div>
 </div>

>**Stanford NER** is an implementation of a `Named Entity Recognizer`. Named Entity Recognition (`NER`) labels sequences of words in a text which are the names of things, such as person and company names, or gene and protein names. It comes with well-engineered feature extractors for Named Entity Recognition, and many options for defining feature extractors. Included with the download are good named entity recognizers for English, particularly for the 3 classes (`PERSON`, `ORGANIZATION`, `LOCATION`), and Stanford NLP Group also makes available on [the original page](https://nlp.stanford.edu/software/CRF-NER.html) various other models for different languages and circumstances, including models trained on just the [CoNLL 2003](http://www.cnts.ua.ac.be/conll2003/ner/) English training data. The distributional similarity features in some models improve performance but the models require considerably more memory.
>
>Stanford NER is also known as `CRFClassifier`. The software provides a general implementation of (arbitrary order) linear chain `Conditional Random Field` (`CRF`) sequence models. That is, by training your own models, you can actually use this code to build sequence models for any task.
>
>You can look at a PowerPoint Introduction to NER and the Stanford NER package [ppt](http://www-nlp.stanford.edu/software/jenny-ner-2007.ppt) [pdf](http://www-nlp.stanford.edu/software/jenny-ner-2007.pdf) or the [FAQ](http://www-nlp.stanford.edu/software/crf-faq.shtml), which has some information on training models. Further documentation is provided in the included README and in the javadocs.
>
>Stanford NER is available for download, licensed under the `GNU General Public License` (v2 or later). Source is included. The package includes components for command-line invocation, running as a server, and a Java API. Stanford NER code is dual licensed (in a similar manner to MySQL, etc.). Open source licensing is under the full GPL, which allows many free uses. For distributors of proprietary software, [commercial licensing](http://otlportal.stanford.edu/techfinder/technology/ID=24628) is available.