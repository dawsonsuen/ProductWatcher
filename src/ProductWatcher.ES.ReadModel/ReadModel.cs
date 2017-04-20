using System;
using NEvilES.Pipeline;

namespace ProductWatcher.ES.ReadModel
{
    public class ReadModelSql
    {
        public Int64 Id { get; set; }
        public Guid StreamId { get; set; }
        public string Type { get; set; }
        public string Body { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}