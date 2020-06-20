using System;
using System.Collections.Generic;
using System.Text;

namespace VeraData.DataAccess.Models
{
    public class SourceFile : BaseEntity
    {
        public List<Flaw> Flaws { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }}

