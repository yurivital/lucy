using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lucy.Plugin.Parsers;
using Lucy.Extensibility;
using System.Collections.Generic;
using System.Linq;
using Lucy.Core;
using System.IO;

namespace Lucy.Test.Lucy.Plugin.Parser
{
    [TestClass]
    
    public class PlainTextTest
    {
        /// <summary>
        /// Test Plugin declaration
        /// </summary>
        [TestMethod, TestCategory("Extension")]
        public void TestPluginDefinition()
        {
            IParser plain = new PlainText();            
            Assert.IsTrue(plain.Name.Length > 3, "A parser plugin must have a name");
            Assert.IsTrue(plain.SupportedFileExtensions.Contains(".txt"), "plain parser support .txt extension");
        }
        /// <summary>
        /// Test the parsing
        /// </summary>
        [TestMethod, TestCategory("Extension")]
        public void TestParse3Chunks()
        {
            IParser plain = new PlainText();
            DocumentIdentity doc = new DocumentIdentity()
            {
                FilePath = new FileInfo(@"Lucy.Plugin.Parser\plainText_3Chunks.txt")
            };
            IEnumerable<DocumentChunk> chunks = plain.Parse(doc);

            Assert.IsNotNull(chunks, "The result can be empty but not null");
            Assert.IsTrue(chunks.Count() == 3, "We should have 3 chunks");
        }

        [TestMethod, TestCategory("Extension")]
        public void TestParseNoChunks()
        {
            IParser plain = new PlainText();
            DocumentIdentity doc = new DocumentIdentity()
            {
                FilePath = new FileInfo(@"Lucy.Plugin.Parser\plainText_0Chunks.txt")
            };
            IEnumerable<DocumentChunk> chunks = plain.Parse(doc);

            Assert.IsNotNull(chunks, "The result can be empty but not null");
            Assert.IsTrue(chunks.Count() == 0, "We should have not any chunk");
        }

    }
}
