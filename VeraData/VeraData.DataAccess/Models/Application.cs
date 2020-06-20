using System;
using System.Collections.Generic;
using System.Text;

namespace VeraData.DataAccess.Models
{
    public class Application : BaseEntity
    {
        public long VeracodeId { get; set; }
        public string Name { get; set; }
        public List<Scan> Scans { get; set; }        
        public List<Sandbox> Sandboxes { get; set; }
    }
}
