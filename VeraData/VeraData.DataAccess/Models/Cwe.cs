using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VeraData.DataAccess.Models
{
    public class Cwe : BaseEntity
    {
        public int VeracodeId { get; set; }
        public string Description { get; set; }
        public int RemediationEffort { get; set; }
        public int Exploitability { get; set; }
        public bool Pci { get; set; }
        
    }
}
