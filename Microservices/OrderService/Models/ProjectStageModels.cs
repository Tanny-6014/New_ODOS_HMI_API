using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("PROJ_LEVEL_MASTER")]
    public class ProjectStageModels
    {
        [Key]
        public string prj_level_cat { get; set; }
        public string prj_level_desc { get; set; }
    }
}