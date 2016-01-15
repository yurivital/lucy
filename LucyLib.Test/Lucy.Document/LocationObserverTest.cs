using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lucy.Document;
using System.IO;

namespace Lucy.Test.Lucy.Document
{
    [TestClass]
    public class LocationObserverTest
    {
        private const string testMaterialUri = "TestMaterial";

        [TestMethod, TestCategory("Document")]
        public void TestAddAndDelete()
        {
            string localMaterial = Path.Combine(Environment.CurrentDirectory, testMaterialUri); 
            LocationObserver location = new LocationObserver();
            location.AddLocation(localMaterial);
            Assert.IsTrue(location.Locations.Count == 1, "We must have one location");
            location.AddLocation(localMaterial);
            Assert.IsTrue(location.Locations.Count == 1, "We must still have one location");
            location.RemoveLocation("FoolLocation");
            Assert.IsTrue(location.Locations.Count == 1, "We must still have one location");
            location.RemoveLocation(localMaterial);
            Assert.IsTrue(location.Locations.Count == 0, "We must have no location");
            
            

        }
    }
}
