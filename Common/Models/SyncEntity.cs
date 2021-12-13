using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class SyncEntity
    {
        public Guid Id { get; set; }

        public DateTime LastChandeAt { get; set; }

        public string JsonData { get; set; }
        public string SyncType { get; set; }
        public string OjectType { get; set; }
        public string Origin { get; set; }

    }
}
