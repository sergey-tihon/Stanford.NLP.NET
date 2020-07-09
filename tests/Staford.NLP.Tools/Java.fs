module Stanford.NLP.Tools.Java

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

let props (values) =
    let props = Properties()
    for (key, value) in values do
        props.setProperty(key, value) |> ignore
    props
