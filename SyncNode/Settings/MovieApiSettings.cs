using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyncNode.Settings
{
    public class MovieApiSettings : IMovieApiSettings

    {
        public string[] Hosts { get; set; }
    }
}
