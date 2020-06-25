using System;
using System.Collections.Generic;
using System.Text;

namespace VeraData.DataAccess.Models
{
    public class PreScanMessage : BaseEntity
    {
        public string Filename { get; set; }
        public string Message { get; set; }
        public int VeracodeModuleId { get; set; }
    }
}
