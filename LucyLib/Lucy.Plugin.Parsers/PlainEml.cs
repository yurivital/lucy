using Lucy.Core;
using Lucy.Extensibility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lucy.Plugin.Parsers
{

    [Export(typeof(IParser))]
    [ExportMetadata("Name", "Plain email")]
    public class PlainEml : IParser
    {
        public string Name
        {
            get
            {
                return "Plain-Email";
            }
        }

        public IEnumerable<string> SupportedFileExtensions
        {
            get
            {
                return new List<string>() { ".eml" };
            }
        }


        private static Regex fromRegex = new Regex("(?<from>From\\W+)", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        private static Regex toRegex = new Regex("(?<to>to\\W+)", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        private static Regex emailRegex = new Regex("(?<email>\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*)", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        private static Regex namedDestRegex = new Regex("(?<name>\"{1}\\w+\")", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);


        public IEnumerable<DocumentChunk> Parse(DocumentIdentity document)
        {
            List<DocumentChunk> chuncks = new List<DocumentChunk>(10);
            using (TextReader s = File.OpenText(document.FilePath))
            {
                string line = string.Empty;
                while ((line = s.ReadLine()) != null)
                {
                    if (fromRegex.IsMatch(line) && emailRegex.IsMatch(line))
                    {
                        foreach(DocumentChunk c in ExtractEmails(line))
                        {
                            c.Metadata = "from";
                            chuncks.Add(c);
                        }

                    }
                    if (toRegex.IsMatch(line) && emailRegex.IsMatch(line))
                    {
                        foreach (DocumentChunk c in ExtractEmails(line))
                        {
                            c.Metadata = "to";
                            chuncks.Add(c);
                        }
                    }
                }
            }
            return chuncks;
        }

        private IEnumerable ExtractEmails(string s)
        {
            foreach (Match m in emailRegex.Matches(s))
            {
                yield return new DocumentChunk()
                {
                    Text = m.Value
                };
            }
        }
    }
}
