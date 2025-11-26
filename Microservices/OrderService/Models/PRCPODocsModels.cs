using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESPRCPODoc")]
    public class PRCPODocsModels
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
        public int PODocID { get; set; }

        public string FileName { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public byte[] PODoc { get; set; }

    }

    public class PRCPODocsFileModels
    {
        public int PODocID { get; set; }
        public string FileName { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string FileSize { get; set; }
        public int Exists { get; set; }
    }

}