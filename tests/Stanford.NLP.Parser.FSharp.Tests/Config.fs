[<AutoOpen>]
module Stanford.NLP.Parser.Config

let inline (</>) path1 path2 = System.IO.Path.Combine(path1, path2)
let [<Literal>] jarRoot = __SOURCE_DIRECTORY__ + "/../../data/paket-files/nlp.stanford.edu/stanford-parser-4.0.0/models/"
let model path = jarRoot </> "edu/stanford/nlp/models/" </> path

let dataFile path = __SOURCE_DIRECTORY__ </> @"../../data/" </> path

open java.lang
open java.util

let toSeq (iter:Iterable) =
    let rec loop (x:Iterator) =
        seq {
            if x.hasNext() then
                yield x.next()
                yield! (loop x)
        }
    iter.iterator() |> loop |> Array.ofSeq |> Seq.readonly
