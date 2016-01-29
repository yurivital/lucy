using Lucy.Client.Desktop.Model;
using Lucy.Core;
using Lucy.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy.Client.Desktop.Service
{
    public class SearchService : DocumentStoreService
    {


         
        public IEnumerable<DocumentIdentity> Search(string query)
        {
           
            return  this._storeIndex.Search(query);
        }
    }
}
