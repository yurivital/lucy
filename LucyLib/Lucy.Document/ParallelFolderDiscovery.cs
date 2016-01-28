﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil
//     Les modifications apportées à ce fichier seront perdues si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Lucy.Document
{
    using Lucy.Core;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    /// <summary>
    ///  Fetch all document present in a fileSystem location.
    ///  Implement a parallel algorithm
    /// </summary>
    public class ParallelFolderDiscovery : IDiscovery
    {
        ILogger Logger
        {
            get;
            set;
        }

        /// <summary>
        /// Explore a location and return individual documents
        /// </summary>
        /// <param name="location"></param>
        /// <returns>Document</returns>
        public virtual ICollection<DocumentIdentity> Discover(DocumentLocation location)
        {
            Contract.Assert(location != null);
            List<DocumentIdentity> documents = new List<DocumentIdentity>();
            location.State = DiscoveryStates.Exploring;
            DirectoryInfo dir = new DirectoryInfo(location.Location);
            IEnumerable<FileInfo> files = dir.EnumerateFiles("*.*", SearchOption.AllDirectories);
            Parallel.ForEach(files, (FileInfo file) =>
            {
                try
                {
                    if (!file.Exists)
                    {
                        return;
                    }

                    DocumentIdentity identity = new DocumentIdentity();
                    identity.DocumentID = ComputeId(file.FullName);
                    identity.Checksum = ComputeChecksum(file);
                    identity.State = IndexationStates.NotIndexed;
                    identity.FilePath = file.FullName;
                    identity.LastIndexed = null;
                    documents.Add(identity);
                }
                catch (IOException ex)
                {

                }
                catch (InvalidOperationException ex)
                {
                    //Logger.Error("Checksum failed");
                    //Logger.Error(ex);
                }
                catch (UnauthorizedAccessException ex)
                {
                    //    Logger.Error("Not authorized to accces to" + file.FullName);
                    //    Logger.Error(ex);
                }
            });

            location.LastDiscovered = DateTime.Now;
            location.State = DiscoveryStates.Explored;
            return documents;
        }

        /// <summary>
        /// Compute the ID of the document
        /// </summary>
        /// <param name="url">ressource location of the doc</param>
        /// <returns>Hashed string</returns>
        protected static string ComputeId(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            byte[] hash;
            using (SHA256Managed crypto = new SHA256Managed())
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(url);
                hash = crypto.ComputeHash(buffer, 0, buffer.Length);
            }
            return hash.ToHexaString();
        }

        /// <summary>
        /// Compute the checksum of an file
        /// </summary>
        /// <param name="file">file to checksum</param>
        /// <returns>Hexadecimal string hash</returns>
        protected static string ComputeChecksum(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            if (file.Length == 0)
            {
                return string.Empty;
            }

            byte[] hash;
            using (SHA256Managed crypto = new SHA256Managed())
            using (Stream stream = file.Open(FileMode.Open, FileAccess.Read))
            {
                hash = crypto.ComputeHash(stream);
            }
            return hash.ToHexaString();
        }
    }
}
