using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Lucy.Core;
using Lucy.Extensibility;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy.Plugin.Parsers
{
    [Export(typeof(IParser))]
    [ExportMetadata("Name", "PDF")]
    public class PortableDocumentFormat : IParser
    {

        public string Name
        {
            get { return "PDF"; }
        }

        public IEnumerable<string> SupportedFileExtensions
        {
            get
            {
                return new List<string>() { ".pdf" };
            }
        }

        /// <summary>
        /// Parse an PDF document and extract the text content
        /// </summary>
        /// <param name="document">Document to analyze</param>
        /// <returns>One text chunk per page</returns>
        public IEnumerable<DocumentChunk> Parse(DocumentIdentity document)
        {
            IList<DocumentChunk> result = new List<DocumentChunk>();
            Contract.Assert(document != null);
            Contract.Result<IEnumerable<DocumentChunk>>();
            Contract.Ensures(result != null, "Empty collection can be returned but not null reference");

            using (PdfReader reader = new PdfReader(File.Open(document.FilePath, FileMode.Open)))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    DocumentChunk chunk = new DocumentChunk();
                    chunk.Text = PdfTextExtractor.GetTextFromPage(reader, i);
                    chunk.Metadata = "Content";

                    if (!String.IsNullOrEmpty(chunk.Text))
                    {
                        result.Add(chunk);
                    }
                }
            }

            return result;
        }
    }
}
