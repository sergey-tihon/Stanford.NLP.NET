using System.Collections.Generic;
using IKVM.Runtime;
using java.lang;
using java.util;

namespace Stanford.NLP.Tools
{
    public static class Java
    {
        public static IEnumerable<object> ToSeq(this Iterable iterable)
        {
            var iterator = iterable.iterator();
            while (iterator.hasNext())
            {
                yield return iterator.next();
            }
        }

        public static Properties Props(Dictionary<string, string> values)
        {
            var props = new Properties();
            foreach (var kv in values)
            {
                props.setProperty(kv.Key, kv.Value);
            }
            return props;
        }
    }
}
