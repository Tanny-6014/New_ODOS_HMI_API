using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESAddLimit")]
    public class BBSAddnLimitModels
    {
        [Key]
        public int dia { get; set; }
        public int former { get; set; }
        public int former_max { get; set; }
        public int hook_height_max { get; set; }
        public int bend_len_min { get; set; }
        public string changed_by { get; set; }
        public DateTime? changed_date { get; set; }
    }
}