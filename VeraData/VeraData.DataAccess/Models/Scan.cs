using System;
using System.Collections.Generic;
using System.Text;
using VeraData.DataAccess.Models;

namespace VeraData.DataAccess
{
    public class Scan : BaseEntity
    {
        public int VeracodeId { get; set; }
        public string Submitter { get; set; }
        public string ScanStatus { get; set; }
        public string ScanType { get; set; }
        public string Name { get; set; }
        public Sandbox Sandbox { get; set; }
        public DateTime ScanStart { get; set; }
        public DateTime ScanEnd { get; set; }
        public List<UploadFile> UploadFiles { get; set; }
        public List<Module> Modules { get; set; }
    }
}
