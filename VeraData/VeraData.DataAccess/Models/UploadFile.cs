using System;
using System.Collections.Generic;
using System.Text;

namespace VeraData.DataAccess.Models
{
    public class UploadFile : BaseEntity
    {
        public long VeracodeId { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public UploadFileStatus Status { get; set; }
    }
}
