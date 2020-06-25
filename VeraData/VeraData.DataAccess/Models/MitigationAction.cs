using System;
using System.Collections.Generic;
using System.Text;

namespace VeraData.DataAccess.Models
{
    public class MitigationAction : BaseEntity
    {
        public string Description { get; set; }
        public string Comment { get; set; }
        public string Reviewer { get; set; }
        public DateTime Date { get; set; }
    }
}
