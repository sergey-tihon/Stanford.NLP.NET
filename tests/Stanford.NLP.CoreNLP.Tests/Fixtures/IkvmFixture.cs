using System;
using System.IO;
using System.Reflection;

namespace Stanford.NLP.CoreNLP.Tests.Fixtures;

public class IkvmFixture
{
    public IkvmFixture ()
    {
        PreloadAssemblyWithModels();
    }

    private static void PreloadAssemblyWithModels()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var modelsAssemblyPath = Path.Combine(baseDirectory, "edu.stanford.nlp.corenlp_english_models.dll");
        Assembly.LoadFile(modelsAssemblyPath);
    }
}
