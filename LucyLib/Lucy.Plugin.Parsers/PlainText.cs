﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil
//     Les modifications apportées à ce fichier seront perdues si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Lucy.Plugin.Parsers
{
    using Lucy.Core;
    using Lucy.Extensibility;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Provide parsing capability for plain text files
    /// </summary>
    [Export(typeof(IParser))]
    [ExportMetadata("Name","Plain text")]
    public class PlainText : IParser
    {
        /// <summary>
        /// Parse an document
        /// </summary>
        /// <param name="document">Document to parse</param>
        /// <returns>Chunk of text</returns>
        public virtual IEnumerable<DocumentChunk> Parse(DocumentIdentity document)
        {
            List<DocumentChunk> result = new List<DocumentChunk>();
            Contract.Assert(document != null);
            Contract.Result<IEnumerable<DocumentChunk>>();
            Contract.Ensures(result != null, "Empty collection can be returned but not null reference");

            using (StreamReader reader = File.OpenText(document.FilePath))
            {
                DocumentChunk chunk = new DocumentChunk();
                StringBuilder text = new StringBuilder();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    bool isEmptyParagraph = line.Length == 0;
                    if (isEmptyParagraph && text.Length > 0)
                    {
                        chunk.Metadata = "Content";
                        chunk.Text = text.ToString();
                        result.Add(chunk);
                        text.Clear();
                        chunk = new DocumentChunk();
                    }
                    else
                    {
                        text.Append(line);
                    }
                }
                // Post-Loop action : add remaining chunk
                if (text.Length > 0)
                {
                    chunk.Text = text.ToString();
                    chunk.Metadata = "Content";
                    result.Add(chunk);
                }
            }

            return result;
        }

        /// <summary>
        /// Get the friendly name of the plugin
        /// </summary>
        public string Name
        {
            get { return "Plain text"; }

        }

        /// <summary>
        /// Get the supported file extensions
        /// </summary>
        public IEnumerable<string> SupportedFileExtensions
        {
            get
            {
                return new List<string>() { ".txt", ".csv", ".md"  };
            }

        }
    }
}

