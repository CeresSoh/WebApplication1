using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Lookup_Department")]
    public class LookupDepartment
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}