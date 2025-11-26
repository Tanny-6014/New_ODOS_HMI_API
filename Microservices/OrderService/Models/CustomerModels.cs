using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESCustomerMaster")]
    public class CustomerModels
    {
        [Key]
        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }
    }
}