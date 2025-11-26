using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESPin")]
    public class PinModels
    {
        [Key, Column(Order = 1)]
        public string grade { get; set; }
        [Key, Column(Order = 2)]
        public string type { get; set; }
        [Key, Column(Order = 3)]
        public int dia { get; set; }

        public int pin { get; set; }
        public int bend_len_min { get; set; }
        public int hook_len_min { get; set; }
        public int hook_height_min { get; set; }

    }

    [Table("OESPinAdd")]
    public class PinAddModels
    {
        [Key, Column(Order = 1)]
        public string std_code { get; set; }
        [Key, Column(Order = 2)]
        public string grade { get; set; }
        [Key, Column(Order = 3)]
        public string type { get; set; }
        [Key, Column(Order = 4)]
        public int dia { get; set; }
        public int pin { get; set; }
        public int bend_len_min { get; set; }
        public int hook_len_min { get; set; }
        public int hook_height_min { get; set; }

    }
}
