using System;

using System.Collections.Generic;

using System.Linq;

using System.Web;

using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace SAP_API.Models

{

    [Table("OESDrawings_posting")]

    public class Drawing_postingModels

    {

        [Key, Column(Order = 1)]

        public string CustomerCode { get; set; }

        [Key, Column(Order = 2)]

        public string ProjectCode { get; set; }

        [Key, Column(Order = 3)]

        public string FileName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int DrawingID { get; set; }

        public string DrawingNo { get; set; }

        public int Revision { get; set; }

        public string Remarks { get; set; }

        public string Status { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

    }

}
