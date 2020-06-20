using System;
using System.Collections.Generic;
using System.Text;
using VeraData.DataAccess.Models;

namespace VeraData.DataAccess
{
    public class Flaw : BaseEntity
    {
        public int VeracodeId { get; set; }
        public Severity Severity { get; set; }
        public int SeverityId { get; set; }
        public Category Category { get; set; }
        public int VeracodeCategoryId { get; set; }
        public Cwe Cwe { get; set; }
        public int VeracodeCweId { get; set; }
        public string RemediationStatus { get; set; }
        public int LineNumber { get; set; }
        public string PrototypeFunction { get; set; }
        public string Function { get; set; }
        public int Count { get; set; }
    }
}
