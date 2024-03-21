using Xunit;

namespace Stanford.NLP.CoreNLP.Tests.Fixtures;

[CollectionDefinition(nameof(IkvmCollection))]
public class IkvmCollection : ICollectionFixture<IkvmFixture>
{
}
