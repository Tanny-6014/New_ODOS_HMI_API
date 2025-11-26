using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESAddLimitShape")]
    public class BBSAddnLimitShapeModels
    {
        [Key, Column(Order = 1)]
        public string shape_code { get; set; }
        [Key, Column(Order = 2)]
        public string shape_paras { get; set; }
        public bool hook_shape { get; set; }
        public string replace_shape { get; set; }
        public string changed_by { get; set; }
        public DateTime? changed_date { get; set; }
    }
}