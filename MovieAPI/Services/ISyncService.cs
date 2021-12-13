using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieAPI.Services
{
   public  interface ISyncService<T> where T: MongoDocument
    {
      HttpResponseMessage  Upsert(T record);
        HttpResponseMessage Delete(T record);

    }
}
