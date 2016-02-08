using Lucy.Core;
using Lucy.Extensibility;
using Lucy.Plugin.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy.Test.Lucy.Plugin.Parser
{
    [TestClass]
   public class PlainEmlTest
    {

        [TestMethod, TestCategory("Extension")]
        public void TestPluginDefinition()
        {
            IParser plain = new PlainEml();
            Assert.IsTrue(plain.Name.Length > 3, "A parser plugin must have a name");
            Assert.IsTrue(plain.SupportedFileExtensions.Contains(".eml"), "plain email parser support .eml extension");
        }

        [TestMethod, TestCategory("Extension")]
        public void TestParseEmail()
        {
            DocumentIdentity doc = new DocumentIdentity()
            {
                FilePath = Path.GetFullPath(@"Lucy.Plugin.Parser\simpleMail.eml")
            };
            IParser plain = new PlainEml();
            var r = plain.Parse(doc);
            Assert.IsTrue(r.Count() == 3, "must be found 3 chuncks, 1 from and 2 to");
            Assert.IsTrue(r.Count(p => p.Metadata == "from") == 1, "from not found");
            Assert.IsTrue(r.Count(p => p.Metadata == "to") == 2, "to not found");


        }

    }
}
