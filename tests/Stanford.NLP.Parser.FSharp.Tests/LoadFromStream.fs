module Stanford.NLP.Parser.LoadFromStream

open System.IO
open Expecto

let [<Tests>] streamTests =
    testList "Load model from stream" [
        test "MaxentTagger" {
            // Plain model in the file
            let model = model "pos-tagger/english-left3words-distsim.tagger"
            use fs = new FileStream(model, FileMode.Open)
            use isw = new ikvm.io.InputStreamWrapper(fs)
            let tagger = edu.stanford.nlp.tagger.maxent.MaxentTagger(isw)
            Expect.isNotNull tagger ""
        }
        test "LexicalizedParser" {
            // GZIPed model in the file
            let model = model "lexparser/englishPCFG.ser.gz"
            use fs = new FileStream(model, FileMode.Open)
            use isw = new ikvm.io.InputStreamWrapper(fs)

            use ois =
                if model.EndsWith(".gz")
                then
                    let gzs = new java.util.zip.GZIPInputStream(isw)
                    new java.io.ObjectInputStream(gzs)
                else new java.io.ObjectInputStream(isw)

            let lp = edu.stanford.nlp.parser.lexparser.LexicalizedParser.loadModel(ois)
            Expect.isNotNull lp ""
        }
    ]
