using java.io;
using java.util;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.tagger.maxent;
using Console = System.Console;
using Stanford.NLP.Tools;

namespace Stanford.NLP.POSTagger.CSharp
{
    class Program
    {
        static void Main()
        {
            // Loading POS Tagger
            var tagger = new MaxentTagger(Files.Tagger.model("english-bidirectional-distsim.tagger"));

            // Text for tagging
            var text = "A Part-Of-Speech Tagger (POS Tagger) is a piece of software that reads text in some language "
                       +"and assigns parts of speech to each word (and other token), such as noun, verb, adjective, etc., although "
                       + "generally computational applications use more fine-grained POS tags like 'noun-plural'.";

            var sentences = MaxentTagger.tokenizeText(new StringReader(text)).toArray();
            foreach (ArrayList sentence in sentences)
            {
                var taggedSentence = tagger.tagSentence(sentence);
                Console.WriteLine(SentenceUtils.listToString(taggedSentence, false));
            }
        }
    }
}
