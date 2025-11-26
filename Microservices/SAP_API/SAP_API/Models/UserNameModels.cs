using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SAP_API.Modelss
{
    [Table("AspNetUsers")]
    public class UserNameModels
    {
        [Key]
        [Display(Name = "User ID")]
        public string Id { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
    }
}
