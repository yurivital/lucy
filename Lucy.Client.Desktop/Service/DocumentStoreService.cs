using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucy.Document;
using Lucy.Core;
using System.IO;
using System.Xml.Serialization;
using Lucy.Client.Desktop.Model;

namespace Lucy.Client.Desktop.Service
{
    public class DocumentStoreService : IDisposable
    {

        protected string documentLocationFile;
        protected string documentLocationDocFile;

        protected DocumentIndex _storeIndex;
        private XmlSerializer serializer = new XmlSerializer(typeof(DocumentStoreModel));
        private XmlSerializer serializerIndex = new XmlSerializer(typeof(List<DocumentIdentity>));

        public DocumentStoreService()
        {

        }


        /// <summary>
        ///  Load the document store from workspace volume
        /// </summary>
        /// <param name="workspaceFolder">Workspace location</param>
        public DocumentStoreModel LoadStore(string workspaceFolder)
        {
            if (_storeIndex == null)
            {
                this._storeIndex = new DocumentIndex(workspaceFolder);

            }

            DocumentStoreModel docStore = new DocumentStoreModel();

            documentLocationFile = Path.Combine(workspaceFolder, "DocumentStore.lucy");
            documentLocationDocFile = Path.Combine(workspaceFolder, "DocumentStoreDoc.lucy");


            if (File.Exists(documentLocationFile))
            {
                using (Stream stream = File.Open(documentLocationFile, FileMode.Open))
                {
                    docStore = (DocumentStoreModel)serializer.Deserialize(stream);
                }
                using (Stream stream = File.Open(documentLocationDocFile, FileMode.Open))
                {
                    _storeIndex.DocumentIdentity = (List<DocumentIdentity>)serializerIndex.Deserialize(stream);
                }

            }
            

            this._storeIndex.PluginManager = new PluginManager();
            _storeIndex.PluginManager.Load();

            return docStore;
        }


        /// <summary>
        /// Save de document store to the workspace volume
        /// </summary>
        public void SaveStore(DocumentStoreModel model)
        {

            using (Stream stream = File.Open(documentLocationFile, FileMode.Create))
            {
                serializer.Serialize(stream, model);
            }
            using (Stream stream = File.Open(documentLocationDocFile, FileMode.Create))
            {
                serializerIndex .Serialize(stream, _storeIndex.DocumentIdentity);
            }

        }

        public void Index(IList<DocumentLocation> locations)
        {

            foreach (var v in locations)
            {
                v.State = DiscoveryStates.Exploring;
                IDiscovery disco = Document.DiscoveryProvider.GetDiscovery(v.Location);

                var docs = disco.Discover(v);
                foreach (var doc in docs)
                {
                    this._storeIndex.Add(doc);
                }
                v.State = DiscoveryStates.Explored;
            }

            _storeIndex.Scan();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this._storeIndex.Dispose();
                }


                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

        }
        #endregion
    }
}
