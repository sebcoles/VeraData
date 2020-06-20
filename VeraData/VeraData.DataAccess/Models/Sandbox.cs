using System;
using System.Collections.Generic;
using System.Text;

namespace VeraData.DataAccess.Models
{
    public class Sandbox : BaseEntity
    {
        public long VeracodeId { get; set; }
        public string Name { get; set; }
    }
}
