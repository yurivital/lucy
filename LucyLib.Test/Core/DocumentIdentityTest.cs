using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lucy.Core;
using System.IO;

namespace Lucy.Test.Lucy.Document
{
    [TestClass]
    public class DocumentIdentityTest
    {
        public const string fileA = "TestMaterial\\Level 1\\document1A.txt";
        public const string fileB = "TestMaterial\\Level 1\\document1B.txt";

        [TestMethod, TestCategory("Core")]
        public void ComparisonPartialyLoaded()
        {
            // ref
            DocumentIdentity docA = new DocumentIdentity();
            docA.FilePath = Path.GetFullPath(fileA);
            // same file
            DocumentIdentity docAbis = new DocumentIdentity();
            docAbis.FilePath = Path.GetFullPath(fileA);
            Assert.IsTrue(docA.CompareTo(docAbis) == 0, "Same files");
            
            // File differ
            DocumentIdentity docB = new DocumentIdentity();
            docB.FilePath = Path.GetFullPath(fileB);
            Assert.IsFalse(docA.CompareTo(docB) == 0, "Not same files");

        }

        [TestMethod, TestCategory("Core")]
        public void ComparisonChecksum()
        {
            //Ref
            DocumentIdentity docA = new DocumentIdentity();
            docA.FilePath = Path.GetFullPath(fileA);
            docA.State = IndexationStates.Indexed;
            docA.Checksum = "AAAA";
            
            // Same checksum
            DocumentIdentity docB = new DocumentIdentity();
            docB.FilePath = Path.GetFullPath(fileA);
            docB.State = IndexationStates.Indexed;
            docB.Checksum = "AAAA";
            Assert.IsTrue(docA.CompareTo(docB) == 0, "Same files");
            
            // Same file, checksum differt
            DocumentIdentity docC = new DocumentIdentity();
            docC.FilePath = Path.GetFullPath(fileA);
            docC.State = IndexationStates.Indexed;
            docC.Checksum = "CCCC";
            Assert.IsFalse(docA.CompareTo(docC) == 0, "Not same files");

        }
    }
}
