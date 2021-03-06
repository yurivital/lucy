﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil
//     Les modifications apportées à ce fichier seront perdues si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Lucy.Document
{
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.Search;
    using Lucene.Net.Store;
    using Lucy.Core;
    using Lucy.Extensibility;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Manage the document indexing  
    /// </summary>
    public class DocumentIndex : IDisposable
    {
        /// <summary>
        /// Store the state of the instance
        /// </summary>
        bool disposed = false;

        /// <summary>
        /// Root path of the lucene store index
        /// </summary>
  
        private string workspacePath;

        /// <summary>
        /// Storage of the index
        /// </summary>
        private Lucene.Net.Store.Directory indexStore;

        
        /// <summary>
        /// Documents for indexing
        /// </summary>
        
        public List<DocumentIdentity> DocumentIdentity
        {
            get;
            set;
        }


        /// <summary>
        /// Get or Set a référence of plugin Manager
        /// </summary>
        public virtual PluginManager PluginManager
        {
            get;
            set;
        }

        /// <summary>
        /// Create a new instance of <see cref="DocumentIndex"/>
        /// </summary>
        public DocumentIndex(string workspacePath)
        {
            this.DocumentIdentity = new List<DocumentIdentity>(10000);
            this.workspacePath = workspacePath;
        }

        public DocumentIndex()
        {

        }

        /// <summary>
        /// Add a document for indexing
        /// </summary>
        /// <param name="doc">document to add</param>
        public virtual void Add(DocumentIdentity doc)
        {
            Contract.Assert(doc != null);
            Contract.Assert(doc.FilePath != null);

            int nbOfPath = this.DocumentIdentity.Count(p => p.FilePath == doc.FilePath);

            if (nbOfPath == 0)
            {
                this.DocumentIdentity.Add(doc);
            }
        }

        /// <summary>
        /// Remove a document from the indexing list
        /// </summary>
        /// <param name="doc"></param>
        public virtual void Remove(DocumentIdentity doc)
        {
            this.Remove(doc);
        }

        /// <summary>
        /// Verify if the document is in the indexing list
        /// </summary>
        /// <param name="doc">document to remove</param>
        /// <returns>Return true if the document is in the indexing list</returns>
        public virtual bool Contain(DocumentIdentity doc)
        {
            return this.DocumentIdentity.Contains(doc);
        }


        private void OpenIndex()
        {
            if (indexStore != null)
            {
                return;
            }

            DirectoryInfo indexStoreLocation = new DirectoryInfo(
                Path.Combine(workspacePath, "Index"));
            indexStore = SimpleFSDirectory.Open(indexStoreLocation);
        }

        /// <summary>
        /// Perfom indexing
        /// </summary>
        public virtual void Scan()
        {
            OpenIndex();


            using (IndexWriter writer = new IndexWriter(
                   indexStore,
                    new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30),
                    IndexWriter.MaxFieldLength.UNLIMITED))
            {
                writer.DeleteAll();
                writer.Commit();
                foreach (var doc in this.DocumentIdentity)
                {
                    doc.State = IndexationStates.Indexing;
                    Index(writer, doc);
                    doc.State = IndexationStates.Indexed;
                    doc.LastIndexed = DateTime.Now;
                }
                writer.Commit();
            }
        }


        /// <summary>
        /// Perform document parsing and update lucen index
        /// </summary>
        /// <param name="write">Indew writer</param>
        /// <param name="doc">Document to index</param>
        private void Index(IndexWriter write, DocumentIdentity doc)
        {
            Document lucenDoc = new Lucene.Net.Documents.Document();

            // Metada
            Field docID = new Field("ID", doc.DocumentID, Field.Store.YES, Field.Index.NO, Field.TermVector.NO);
            lucenDoc.Add(docID);
            Field docName = new Field("Name", Path.GetFileNameWithoutExtension( doc.FilePath), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO);
            lucenDoc.Add(docName);
            Field docExtention = new Field("Extention",Path.GetExtension( doc.FilePath), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO);
            lucenDoc.Add(docExtention);
            Field docCRC = new Field("Checksum", doc.Checksum, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO);
            lucenDoc.Add(docCRC);
            Field locationField = new Field("Location", Path.GetDirectoryName( doc.FilePath), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO);
            lucenDoc.Add(locationField);
            Field dateField = new Field("Last modified",  File.GetLastWriteTime( doc.FilePath).ToLongDateString(), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO);
            lucenDoc.Add(dateField);
            Field sizeField = new Field("Size", doc.FilePath.Length.ToString(), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO);
            lucenDoc.Add(sizeField);

            // Can not parse, index limited to meta-data
            IParser parser = PluginManager.GetParser(doc);
            if (parser == null)
            {
                write.AddDocument(lucenDoc);
                return;
            }

            //  Index content datas
            IEnumerable<DocumentChunk> chunks = parser.Parse(doc);

            foreach (DocumentChunk chunk in chunks)
            {
                Field field = new Field(chunk.Metadata,
                    chunk.Text,
                    Field.Store.NO,
                    Field.Index.ANALYZED,
                    Field.TermVector.YES);
                lucenDoc.Add(field);
            }
            write.AddDocument(lucenDoc);
        }

        /// <summary>
        /// Perform a query
        /// </summary>
        /// <param name="query">text of the query</param>
        /// <returns>documents found</returns>
        public virtual IEnumerable<DocumentIdentity> Search(string query)
        {
            TopDocs results = null;
            List<DocumentIdentity> resultDocuments = new List<Core.DocumentIdentity>(100);

            OpenIndex();
            Lucene.Net.QueryParsers.QueryParser parse = new Lucene.Net.QueryParsers.QueryParser(
                Lucene.Net.Util.Version.LUCENE_30,
                string.Empty,
                new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30));

            using (IndexSearcher searcher = new Lucene.Net.Search.IndexSearcher(this.indexStore))
            {
                results = searcher.Search(parse.Parse(query), 100);

                foreach (var result in results.ScoreDocs)
                {

                    Document document = searcher.Doc(result.Doc);
                    string documentID = document.Get("ID");

                    DocumentIdentity foundDocument = this.DocumentIdentity.Single(p => p.DocumentID == documentID);
                    resultDocuments.Add(foundDocument);
                }
            }
            return resultDocuments;
        }

        /// <summary>
        /// Dispose ressources
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                Dispose(true);
            }
        }

        /// <summary>
        /// Dispose unmanaged ressource
        /// </summary>
        /// <param name="dispose">Should dispose non managed ressource</param>
        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                if (indexStore != null)
                {
                    indexStore.Dispose();
                    indexStore = null;
                    PluginManager = null;
                    this.DocumentIdentity = null;
                    GC.SuppressFinalize(this);
                    this.disposed = true;
                }
            }
        }
    }
}

