using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP_API.Modelss
{
    [Table("OESBBS")]
    public class BBSModels
    {
        [Key, Column(Order = 1)]
        public string CustomerCode { get; set; }
        [Key, Column(Order = 2)]
        public string ProjectCode { get; set; }
        [Key, Column(Order = 3)]
        public int JobID { get; set; }
        [Key, Column(Order = 4)]
        public int BBSID { get; set; }

        public string BBSNo { get; set; }

        public string BBSDesc { get; set; }

        public string BBSCopiedFrom { get; set; }

        public string BBSStrucElem { get; set; }

        public decimal BBSOrderCABWT { get; set; }

        public decimal BBSOrderSTDWT { get; set; }

        public decimal? BBSCancelledWT { get; set; }

        public decimal BBSTotalWT { get; set; }

        public string BBSSOR { get; set; }

        public string BBSNoNDS { get; set; }

        public string BBSSAPSO { get; set; }

        public string BBSSORCoupler { get; set; }

        public string BBSNoNDSCoupler { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }
}