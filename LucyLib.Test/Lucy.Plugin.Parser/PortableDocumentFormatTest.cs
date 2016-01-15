using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lucy.Extensibility;
using Lucy.Plugin.Parsers;
using Lucy.Core;
using System.IO;
using System.Linq;

namespace Lucy.Test.Lucy.Plugin.Parser
{
            
    [TestClass]
    public class PortableDocumentFormatTest
    {
        public const string textPdfFile = "TestMaterial\\Level 1\\OpenSL_ES_Specification_1.0.1.pdf";

        [TestMethod, TestCategory("Extension")]
        public void TestParsePdfText()
        {
            IParser parser = new PortableDocumentFormat();
            DocumentIdentity doc = new DocumentIdentity();
            doc.FilePath = new FileInfo(textPdfFile);
             var chuncks =  parser.Parse(doc);

             Assert.IsTrue(chuncks.Count() > 0, "The PDF doc containt text");
        }
    }
}
