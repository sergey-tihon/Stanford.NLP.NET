using System.IO;
using System.Reflection;

namespace Stanford.NLP.CoreNLP.Tests.Helpers;

public static class Files
{
    private static string? _rootFolder;

    private static string NlpStanford =>
        Path.Combine(GetRootFolder(), "data/paket-files/nlp.stanford.edu/");

    private static string GetRootFolder()
    {
        if (_rootFolder is not null)
            return _rootFolder;

        _rootFolder = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName!;
        while (true)
        {
            if (File.Exists(Path.Combine(_rootFolder, ".gitignore")))
                return _rootFolder;
            _rootFolder = new DirectoryInfo(_rootFolder).Parent!.FullName;
        }
    }

    public static class CoreNlp
    {
        public static string JarRoot =>
            Path.Combine(NlpStanford, "stanford-corenlp-4.5.5/models/");

        public static string Models(string path)
        {
            return Path.Combine(JarRoot, "edu/stanford/nlp/models/", path);
        }
    }
}
