using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_API.Modelss
{
    [Table("OESCustomerMaster")]
    public class CustomerModels
    {
        [Key]
        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }
    }
}
