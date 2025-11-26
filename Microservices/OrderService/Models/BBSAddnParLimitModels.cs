using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESAddParLimit")]
    public class BBSAddnParLimitModels
    {
        [Key, Column(Order = 1)]
        public string par_type { get; set; }
        [Key, Column(Order = 2)]
        public int dia { get; set; }
        public int bend_len_min { get; set; }
    }
}