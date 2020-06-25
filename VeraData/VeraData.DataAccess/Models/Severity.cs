using System;
using System.Collections.Generic;
using System.Text;

namespace VeraData.DataAccess.Models
{
    public class Severity : BaseEntity
    {
        public int VeracodeId { get; set; }
        public string Title { get; set; }
    }
}
