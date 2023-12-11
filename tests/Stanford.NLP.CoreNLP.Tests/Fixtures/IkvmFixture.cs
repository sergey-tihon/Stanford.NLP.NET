namespace Stanford.NLP.CoreNLP.Tests.Fixtures;

public class IkvmFixture
{
    public IkvmFixture ()
    {
        // IKVM issue - https://github.com/ikvmnet/ikvm/issues/423
        _ = new java.lang.Object();
    }
}
