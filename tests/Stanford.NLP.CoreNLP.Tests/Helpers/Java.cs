using System.Collections.Generic;
using java.util;

namespace Stanford.NLP.CoreNLP.Tests.Helpers;

public static class Java
{
    public static Properties Props(Dictionary<string, string> values)
    {
        var props = new Properties();
        foreach (var kv in values) props.setProperty(kv.Key, kv.Value);
        return props;
    }
}
