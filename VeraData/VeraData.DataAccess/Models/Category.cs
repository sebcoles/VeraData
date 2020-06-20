using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VeraData.DataAccess.Models
{
    public class Category : BaseEntity
    {
        public int VeracodeId { get; set; }
        public string Description { get; set; }
    }
}
