using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESBPCTemplates")]
    public class BPCTemplateModels
    {
        [Key, Column(Order = 1)]
        public string CustomerCode { get; set; }
        [Key, Column(Order = 2)]
        public string ProjectCode { get; set; }
        [Key, Column(Order = 3)]
        public bool Template { get; set; }
        [Key, Column(Order = 4)]
        public int JobID { get; set; }
        [Key, Column(Order = 5)]
        public int CageID { get; set; }
        [Key, Column(Order = 6)]
        public int TemplateID { get; set; }

        public string template_code { get; set; }
        public int pile_dia { get; set; }
        public int cage_dia { get; set; }
        public int no_of_holes { get; set; }
        public int cover { get; set; }
        public int bundled { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

}