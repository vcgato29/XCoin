using XCoin.IO;
using XCoin.IO.Caching;

namespace XCoin.UnitTests
{
    public class TestMetaDataCache<T> : MetaDataCache<T> where T : class, ISerializable, new()
    {
        public TestMetaDataCache()
            : base(null)
        {
        }

        protected override T TryGetInternal()
        {
            return null;
        }
    }
}
