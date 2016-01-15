using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lucy.Document;

namespace Lucy.Test
{
    [TestClass]
    public class DiscoveryProviderTest
    {
        [TestMethod, TestCategory("Document")]
        public void TestProvider()
        {
            IDiscovery disco = DiscoveryProvider.GetDiscovery("C:\\");
            Assert.IsInstanceOfType(disco, typeof( ParallelFolderDiscovery),"File system folder must be handled by ParallerFolderDiscovry");
        }
    }
}
