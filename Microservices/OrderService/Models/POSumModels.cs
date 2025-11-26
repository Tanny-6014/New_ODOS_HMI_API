using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderService.Models
{
    public class POSumModels
    {
        public int SSNNo { get; set; }
        public string PONo { get; set; }
        public string WBS1 { get; set; }
        public string WBS2 { get; set; }
        public string WBS3 { get; set; }
        public string PODate { get; set; }
        public string RequiredDate { get; set; }
        public string DeliveryDate { get; set; }
        public string POStatus { get; set; }
        public decimal CABOrderWT { get; set; }
        public decimal CABDelWT { get; set; }
        public decimal SBOrderWT { get; set; }
        public decimal SBDelWT { get; set; }
        public decimal MESHOrderWT { get; set; }
        public decimal MESHDelWT { get; set; }
        public decimal CAGEOrderWT { get; set; }
        public decimal CAGEDelWT { get; set; }
        public string PORemarks { get; set; }
    }
}