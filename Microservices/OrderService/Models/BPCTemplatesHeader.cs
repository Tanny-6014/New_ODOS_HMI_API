using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESBPCTemplatesHeader")]
    public class BPCTemplateHeaderModels
    {
        [Key, Column(Order = 1)]
        [Display(Name = "Template Code")]
        public string template_code { get; set; }
        public int SNo { get; set; }
        public int Qty { get; set; }
        public string hall { get; set; }
        public string machine { get; set; }
        public string template_group { get; set; }
        public string Remarks { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }

}