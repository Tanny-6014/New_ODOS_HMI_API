using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESPRCDrawings")]
    public class PRCDrawingsModels
    {
        [Key, Column(Order = 1)]
        [Display(Name = "Customer Code")]
        public string CustomerCode { get; set; }

        [Display(Name = "Project Code")]
        [Key, Column(Order = 2)]
        public string ProjectCode { get; set; }

        [Key, Column(Order = 3)]
        public int JobID { get; set; }

        [Key, Column(Order = 4)]
        public int BBSID { get; set; }

        [Key, Column(Order = 5)]
        public int DrawingID { get; set; }

        public string DrawingNo { get; set; }

        public string FileName { get; set; }

        public int Revision { get; set; }

        public string Remarks { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public byte[] DrawingDoc { get; set; }

    }

    [Table("OESPRCDrawingsRev")]
    public class PRCDrawingsRevModels
    {
        [Key, Column(Order = 1)]
        [Display(Name = "Customer Code")]
        public string CustomerCode { get; set; }

        [Display(Name = "Project Code")]
        [Key, Column(Order = 2)]
        public string ProjectCode { get; set; }

        [Key, Column(Order = 3)]
        public int JobID { get; set; }

        [Key, Column(Order = 4)]
        public int BBSID { get; set; }

        [Key, Column(Order = 5)]
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

    public class ByteArrayToBase64
    {

        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public int JobID { get; set; }
        public int BBSID { get; set; }
        public int DrawingID { get; set; }
        public string DrawingNo { get; set; }
        public string FileName { get; set; }
        public int Revision { get; set; }
        public string Remarks { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] DrawingDoc { get; set; }
        public string Base64 { get; set; }
        public int rowNum { get; set; }
        public int TotalNum { get; set; }
    }

}