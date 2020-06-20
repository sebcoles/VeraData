using System;
using System.Collections.Generic;
using System.Text;

namespace VeraData.DataAccess.Models
{
    public class PreScanError : BaseEntity
    {
        public string Filename { get; set; }
        public string Error { get; set; }
    }
}
