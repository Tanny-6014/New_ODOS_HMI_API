using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESBPCTemplatesMaster")]
    public class BPCTemplateMasterModels
    {
        [Key, Column(Order = 1)]
        [Display(Name = "Template Code")]
        public string template_code { get; set; }
        [Key, Column(Order = 2)]
        public int pile_dia { get; set; }
        [Key, Column(Order = 3)]
        public int cover { get; set; }
        public int no_of_holes { get; set; }
        public int bundled { get; set; }
        public int MainBarMax { get; set; }
        public int priority { get; set; }
        public string Remarks { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

}