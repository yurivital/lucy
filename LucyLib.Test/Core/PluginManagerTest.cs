using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lucy.Core;
using System.Linq;

namespace Lucy.Test.Core
{
    /// <summary>
    /// Test du PluginManagerS
    /// </summary>
    [TestClass]
    public class PluginManagerTest
    {
        

        private TestContext testContextInstance;

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


        [TestMethod, TestCategory("Extensibility")]
        public void TestLoadCorePlugin()
        {
            PluginManager pMan = new PluginManager();
            pMan.Load();
            Assert.IsTrue(pMan.IsInitialized, "The plugin manager should be initialized after a load");   
        }

        [TestMethod, TestCategory("Extensibility")]
        public void TestCorePluginAreLoaded()
        {
            PluginManager pMan = new PluginManager();
            pMan.Load();
            Assert.IsTrue(pMan.Parsers.Count() == 3, "There is 3 parser in the LucyLib");
        }
        
    }
}
