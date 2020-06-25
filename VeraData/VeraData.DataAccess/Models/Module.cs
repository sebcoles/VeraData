using System;
using System.Collections.Generic;
using System.Text;

namespace VeraData.DataAccess.Models
{
    public class Module : BaseEntity
    {
        public string Status { get; set; }
        public long VeracodeId { get; set; }
        public double Size { get; set; }
        public string Hash { get; set; }
        public string Name { get; set; }
        public string Platform { get; set; }
        public bool EntryPoint { get; set; }
        public bool HasFatalErrors { get; set; }
    }
}
