using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAP_API.Models
{
    public class ShowDirDownloadDto
    {
        public string ddCustomerCode { get; set; }
        public string ddProjectCode { get; set; }
        public string ddFileName { get; set; }
        public int ddRevision { get; set; }
        public string UserName { get; set; }
    }
}