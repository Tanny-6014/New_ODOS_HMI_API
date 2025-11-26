using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESBPCCagesLoad")]
    public class BPCCagesLoadModels
    {
        [Key, Column(Order = 1)]
        public int pile_dia { get; set; }
        public int? qty { get; set; }
    }

}