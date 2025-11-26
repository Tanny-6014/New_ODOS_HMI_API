using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESCTSMESHDrawings")]
    public class CTSMESHDrawingsModels
    {
        [Key, Column(Order = 1)]
        [Display(Name = "Customer Code")]
        public string CustomerCode { get; set; }

        [Display(Name = "Project Code")]
        [Key, Column(Order = 2)]
        public string ProjectCode { get; set; }

        [Key, Column(Order = 3)]
        public string ProdType { get; set; }

        [Key, Column(Order = 4)]
        public string StrucElem { get; set; }

        [Key, Column(Order = 5)]
        public string WBS1 { get; set; }

        [Key, Column(Order = 6)]
        public string WBS2 { get; set; }

        [Key, Column(Order = 7)]
        public string WBS3 { get; set; }

        [Key, Column(Order = 8)]
        public int DrawingID { get; set; }

        public string DrawingNo { get; set; }

        public string FileName { get; set; }

        public int Revision { get; set; }

        public string Remarks { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public byte[] DrawingDoc { get; set; }

    }

    [Table("OESCTSMESHDrawingsRev")]
    public class CTSMESHDrawingsRevModels
    {
        [Key, Column(Order = 1)]
        [Display(Name = "Customer Code")]
        public string CustomerCode { get; set; }

        [Display(Name = "Project Code")]
        [Key, Column(Order = 2)]
        public string ProjectCode { get; set; }

        [Key, Column(Order = 3)]
        public string ProdType { get; set; }

        [Key, Column(Order = 4)]
        public string StrucElem { get; set; }

        [Key, Column(Order = 5)]
        public string WBS1 { get; set; }

        [Key, Column(Order = 6)]
        public string WBS2 { get; set; }

        [Key, Column(Order = 7)]
        public string WBS3 { get; set; }

        [Key, Column(Order = 8)]
        public int DrawingID { get; set; }

        public string DrawingNo { get; set; }

        public string FileName { get; set; }

        [Key, Column(Order = 9)]
        public int Revision { get; set; }

        public string Remarks { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public byte[] DrawingDoc { get; set; }

    }
}