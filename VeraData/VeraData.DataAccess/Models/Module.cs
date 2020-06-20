using System;
using System.Collections.Generic;
using System.Text;

namespace VeraData.DataAccess.Models
{
    public class Module : BaseEntity
    {
        public long VeracodeId { get; set; }
        public double Size { get; set; }
        public string Hash { get; set; }
        public List<PreScanError> PreScanErrors { get; set; }
        public string Name { get; set; }
        public bool EntryPoint { get; set; }
        public List<SourceFile> SourceFiles { get; set; }
        public ModuleFile Added { get; set; }
        public ModuleFile Modified { get; set; }
        public ModuleFile Removed { get; set; }
    }
}
