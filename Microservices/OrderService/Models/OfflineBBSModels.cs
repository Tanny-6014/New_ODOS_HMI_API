using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESOfflBBS")]
    public class OfflineBBSModels
    {
        [Key, Column(Order = 1)]
        public int BackupID { get; set; }
        [Key, Column(Order = 2)]
        public int POID { get; set; }
        [Key, Column(Order = 3)]
        public int BBSID { get; set; }
        public string BBSNo { get; set; }

        public string BBSDesc { get; set; }

        public string BBSStrucElem { get; set; }

        public decimal BBSOrderCABWT { get; set; }

        public decimal BBSOrderSTDWT { get; set; }

        public decimal BBSTotalWT { get; set; }

        public string UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }
}