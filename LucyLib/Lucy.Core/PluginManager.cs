﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil
//     Les modifications apportées à ce fichier seront perdues si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Lucy.Core
{
    using Lucy.Extensibility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.IO;
    /// <summary>
    /// Manage the extensibity
    /// </summary>
    public class PluginManager : IDisposable
    {

        /// <summary>
        /// Store the local IoC Contener
        /// </summary>
        private CompositionContainer contener = new CompositionContainer();

        /// <summary>
        /// Store the value of the state of the instance
        /// </summary>
        bool isDisposed = false;


        /// <summary>
        /// Get the value if the plugin manager is ready
        /// </summary>
        public Boolean IsInitialized
        {
            get;
            private set;
        }



        [ImportMany(typeof(IParser))]
        public IEnumerable<IParser> Parsers
        {
            get;
            set;
        }

        /// <summary>
        /// Return the good parser for a document based on the extension
        /// </summary>
        /// <param name="doc">Document </param>
        /// <returns></returns>
        public IParser GetParser(DocumentIdentity doc)
        {
            IParser parser = null;
            Func<IParser, bool> predicate = (IParser p) => p.SupportedFileExtensions.Contains(Path.GetExtension(doc.FilePath));
            if (Parsers.Count(predicate) == 1)
            {
                parser = Parsers.Single(predicate);
            }
            return parser;
        }

        /// <summary>
        /// Load the plugins
        /// </summary>
        public virtual void Load()
        {
            // we can't load plugin twice - Facility shortcut
            if (this.IsInitialized)
            {
                return;
            }

            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(this.GetType().Assembly));
            contener = new CompositionContainer(catalog);

            try
            {
                contener.ComposeParts(this);
                this.IsInitialized = true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Dispose ressources
        /// </summary>
        public void Dispose()
        {
            if (!isDisposed)
            {
                this.Dispose(true);
            }
        }

        /// <summary>
        /// Dispose ressources and remove finalizer
        /// </summary>
        /// <param name="dispose">Should dipose inner ressources</param>
        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                this.contener.Dispose();
                GC.SuppressFinalize(this);
            }
            this.isDisposed = true;
        }
    }
}

