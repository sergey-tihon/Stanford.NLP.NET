using System.IO;
using System.Reflection;

using FileNotFoundException = System.IO.FileNotFoundException;

namespace Stanford.NLP.Tools
{
    public static class Files
    {
        private static string GetFullPath(string path)
        {
            if (File.Exists(path))
                return new FileInfo(path).FullName;
            if (Directory.Exists(path))
                return new DirectoryInfo(path).FullName;
            throw new FileNotFoundException($"File or directory {path} does not exist");
        }

        private static string? _rootFolder = null;
        private static string GetRootFolder()
        {
            if (_rootFolder is {})
                return _rootFolder;

            _rootFolder = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            while (true)
            {
                if (File.Exists(Path.Combine(_rootFolder, ".gitignore")))
                    return _rootFolder;
                _rootFolder = new DirectoryInfo(_rootFolder).Parent.FullName;
            }
        }

        private static string NlpStanford =>
            Path.Combine(GetRootFolder(), "data/paket-files/nlp.stanford.edu/");

        public static string DataFile(string path) =>
            Path.Combine(GetRootFolder(), "tests/data/", path);

        public static class CoreNlp
        {
            public static string JarRoot =>
                Path.Combine(NlpStanford, "stanford-corenlp-4.2.0/models/");

            public static string Models(string path) =>
                Path.Combine(JarRoot, "edu/stanford/nlp/models/", path);
        }

        public static class NER
        {
            public static string Classifier(string path) =>
                Path.Combine(NlpStanford, "stanford-ner-2020-11-17/classifiers/", path);
        }

        public static class Parser
        {
            public static string Models(string path) =>
                Path.Combine(NlpStanford, "stanford-parser-full-2020-11-17/models/edu/stanford/nlp/models/", path);
        }

        public static class Tagger
        {
            public static string Model(string path) =>
                Path.Combine(NlpStanford, "stanford-postagger-full-2020-11-17/models/", path);
        }

        public static class Segmenter
        {
            public static string Root =>
                Path.Combine(NlpStanford, "stanford-segmenter-2020-11-17/data/");

            public static string Data(string path) =>
                Path.Combine(Root, path);
        }
    }
}
