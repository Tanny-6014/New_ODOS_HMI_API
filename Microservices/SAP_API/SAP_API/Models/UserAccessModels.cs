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
    [Table("OESUserAccess")]
    public class UserAccessModels
    {
        [Key, Column(Order = 1)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Key, Column(Order = 2)]
        public string CustomerCode { get; set; }
        [Key, Column(Order = 3)]
        public string ProjectCode { get; set; }

        public string UserType { get; set; }
        // AD -- Administrator
        // PM -- Project Manager
        // PA -- Project Admin
        // PL -- Planner
        // PU -- Power User
        // CU -- Customer
        public string OrderSubmission { get; set; }
        public string OrderCreation { get; set; }
        public string APISubmission { get; set; }
        public string APICreation { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }
}
