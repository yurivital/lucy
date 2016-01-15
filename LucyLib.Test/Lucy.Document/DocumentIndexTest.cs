using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lucy.Core;
using Lucy.Document;
using System.Linq;
using System.IO;

namespace Lucy.Test.Lucy.Document
{
    [TestClass]
    public class DocumentIndexTest
    {
        private DocumentIdentity document1A = new DocumentIdentity();

        private TestContext testContextInstance;

        private String IndexDir
        {
            get
            {
                return Path.Combine(TestContext.TestRunDirectory, "Index");
            }
        }

        /// <summary>
        ///Obtient ou définit le contexte de test qui fournit
        ///des informations sur la série de tests active ainsi que ses fonctionnalités.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


        [TestInitialize]
        public void Init()
        {
            document1A.FilePath = new System.IO.FileInfo("TestMaterial\\Level 1\\document1A.txt");
            document1A.State = IndexationStates.NotIndexed;
            document1A.DocumentID = "AA";
            document1A.Checksum = "ZZZZ";
            Directory.CreateDirectory(IndexDir);
        }

        [TestCleanup]
        public void CleanDir()
        {
            Directory.Delete(IndexDir,true);
        }

       [TestMethod, TestCategory("Document")]
        public void TestAddDocumentToIndex()
        {
            DocumentIndex index = new DocumentIndex(IndexDir);
            index.Add(document1A);
            Assert.IsTrue(index.DocumentIdentity.Count == 1, "One document added");

        }

        [TestMethod, TestCategory("Document")]
        public void TestIndexUnicity()
        {
            DocumentIdentity document1ABis = new DocumentIdentity();
            document1ABis.Checksum = document1A.Checksum;
            document1ABis.FilePath = document1A.FilePath;
            document1ABis.DocumentID = document1A.DocumentID;
            document1ABis.State = IndexationStates.Undefined;

            DocumentIndex index = new DocumentIndex(IndexDir);
            index.Add(document1A);
            index.Add(document1ABis);
            Assert.IsTrue(index.DocumentIdentity.Count == 1, "One different document added");

            document1ABis.State = IndexationStates.NotIndexed;
            index.Add(document1ABis);
            Assert.IsTrue(index.DocumentIdentity.Count == 1, "One different document added");
        }

        [TestMethod, TestCategory("Document")]
        public void TestScan()
        {
            DocumentIndex index = new DocumentIndex(IndexDir);
            PluginManager plugin = new PluginManager();
            plugin.Load();
            index.PluginManager = plugin;
            index.Add(document1A);
            index.Scan();
            foreach (var doc in index.DocumentIdentity)
            {
                Assert.IsTrue(doc.State == IndexationStates.Indexed, "All documents must be index after a scan");
            }
        }
        [TestMethod, TestCategory("Document")]
        public void TestQuery()
        {
            DocumentIndex index = new DocumentIndex(IndexDir);
            PluginManager plugin = new PluginManager();
            plugin.Load();
            index.PluginManager = plugin;
            index.Add(document1A);
            index.Scan();
            var result = index.Search(
                String.Format("Name:\"{0}\"", document1A.FilePath.Name));
            Assert.IsTrue(result.Count() == 1, "Should found 1 doc, found = {0}", result.Count());
        }
    }
}
