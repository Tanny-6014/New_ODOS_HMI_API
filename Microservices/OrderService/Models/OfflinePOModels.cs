using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESOfflPO")]
    public class OfflinePOModels
    {
        [Key, Column(Order = 1)]
        public int BackupID { get; set; }
        [Key, Column(Order = 2)]
        public int POID { get; set; }

        public string PONo { get; set; }

        public string PODesc { get; set; }

        public string StructureElement{ get; set; }

        public string BBSNo { get; set; }

        public string BBSDesc { get; set; }

        public decimal BBSTotalWT { get; set; }

        public int BBSTotalPCs { get; set; }

        public string CouplerType{ get; set; }

        public string Transport{ get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }
    }
}