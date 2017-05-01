 - tagline: What is Stanford.NLP.Segmenter?

# Stanford.NLP.Segmenter

 <div class="snlp-actions">
  <div class="row">
    <div class="col-sm-6">
      <h2>Install NuGet package</h2>
      <i class="fa" aria-hidden="true"><img src="../images/nuget.png" style="width:40px;" /></i>
      <h3 class="actionlink">
        <a href="https://www.nuget.org/packages/Stanford.NLP.Segmenter/">Stanford.NLP.Segmenter</a>
      </h3>
    </div>
    <div class="col-sm-6">
      <h2>Download models</h2>
      <i class="fa fa-download" aria-hidden="true"></i>
      <h3 class="actionlink">
        <a href="http://nlp.stanford.edu/software/stanford-segmenter-2016-10-31.zip">Download Segmenter 3.7.0</a>
      </h3>
    </div>
  </div>
  <div class="row">
    <div class="col-sm-6">
      <h2>Check out .NET Samples</h2>
      <i class="fa fa-book" aria-hidden="true"></i>
      <h3 class="actionlink">
        <a href="/samples.html#Stanford-Word-Segmenter">Open .NET Samples</a>
      </h3>
    </div>
    <div class="col-sm-6">
      <h2>Read official Java docs</h2>
      <i class="fa" aria-hidden="true"><img src="../images/logo.jpg" style="width:40px;" /></i>
      <h3 class="actionlink">
        <a href="https://nlp.stanford.edu/software/segmenter.html">Go to Segmenter page</a>
      </h3>
    </div>
  </div>
 </div>


>Tokenization of raw text is a standard pre-processing step for many NLP tasks. For English, tokenization usually involves punctuation splitting and separation of some affixes like possessives. Other languages require more extensive token pre-processing, which is usually called segmentation.
>
>**The Stanford Word Segmenter** currently supports `Arabic` and `Chinese`. The provided segmentation schemes have been found to work well for a variety of applications.
>
>Stanford NLP group recommend at least `1G` of memory for documents that contain long sentences.
>
>The segmenter is available for download, licensed under the `GNU General Public License` (v2 or later). Source is included. The package includes components for command-line invocation and a Java API. The segmenter code is dual licensed (in a similar manner to MySQL, etc.). Open source licensing is under the full GPL, which allows many free uses. For distributors of proprietary software, [commercial licensing](http://otlportal.stanford.edu/techfinder/technology/ID=27276) is available.