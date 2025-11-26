using System;

using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models

{

    [Table("oesPreCastDetails")]

    public class PreCastDetailsModel

    {

        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Precast_ID { get; set; }

        [Required]

        [StringLength(20)]

        public string CustomerCode { get; set; }

        [Required]

        [StringLength(20)]

        public string ProjectCode { get; set; }

        [Required]

        public int JobID { get; set; }

        [Required]

        [StringLength(50)]

        public string ComponentMarking { get; set; }

     

        [StringLength(50)]

        public string Block { get; set; }

        [StringLength(50)]

        public string Level { get; set; }

        public int? Qty { get; set; }

        [StringLength(150)]

        public string Remark { get; set; }

        public int? PageNo { get; set; }

        [StringLength(10)]

        public string StructureElement { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(50)]

        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]

        public string ModifiedBy { get; set; }

    }

}

