using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lucy.Document;
using Lucy.Core;
using System.IO;
using System.Collections.Generic;

namespace Lucy.Test.Lucy.Document
{
    [TestClass]
    public class ParallelFolderDiscoveryTest
    {

        ParallelFolderDiscovery discovery;
        DocumentLocation location;

        [TestInitialize]
        public void Init()
        {
            discovery = new ParallelFolderDiscovery();
            location = new DocumentLocation();
            location.Location = Path.Combine(Environment.CurrentDirectory, "TestMaterial");
            location.State = DiscoveryStates.NotExplored;

        }

        [TestMethod, TestCategory("Document")]
        public void TestDiscovery()
        {
            ICollection<DocumentIdentity> docs = discovery.Discover(location);
            Assert.IsTrue(docs.Count == 5, "4 files present in test material, {0} found", docs.Count);

        }

        [TestMethod, TestCategory("Document")]
        public void TestChecksum()
        {
            ICollection<DocumentIdentity> docs = discovery.Discover(location);
            foreach (var doc in docs)
            {
                using (var stream = File.Open(doc.FilePath, FileMode.Open))
                {
                    if (stream.Length > 0)
                    {
                        Assert.IsFalse(string.IsNullOrEmpty(doc.Checksum), "Non zero length document must be checksumed");
                    }
                    else
                    {
                        Assert.IsTrue(string.IsNullOrEmpty(doc.Checksum), "Zero length Document must be checksumed");
                    }
                }
            }
        }

        [TestMethod, TestCategory("Document")]
        public void TestFilePath()
        {
            ICollection<DocumentIdentity> docs = discovery.Discover(location);
            foreach (var doc in docs)
            {
                Assert.IsNotNull(doc.FilePath, "Document must have a file attached");
            }
        }

        [TestMethod, TestCategory("Document")]
        public void TestDocumentId()
        {
            ICollection<DocumentIdentity> docs = discovery.Discover(location);
            foreach (var doc in docs)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(doc.DocumentID), "Document must have an Id");

            }
        }


        [TestMethod, TestCategory("Document")]
        public void TestState()
        {
            location.State = DiscoveryStates.NotExplored;
            ICollection<DocumentIdentity> docs = discovery.Discover(location);
            Assert.IsTrue(location.State == DiscoveryStates.Explored, "Location must be explored after discovery");
        }

        [TestMethod, TestCategory("Document")]
        public void TestDocState()
        {
            ICollection<DocumentIdentity> docs = discovery.Discover(location);
            foreach (var doc in docs)
            {
                Assert.IsTrue(doc.State == IndexationStates.NotIndexed, "Document must in not indexed state");
            }
        }
    }
}