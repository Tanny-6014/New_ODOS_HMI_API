using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    public class OrderAmendmentModels
    {

        public string CustomerCode { get; set; }

        [Key]
        public string ProjectCode { get; set; }

        public string ProjectTitle { get; set; }

        public string EmployeeName { get; set; }
        public string EmployeeId { get; set; }

    }
}