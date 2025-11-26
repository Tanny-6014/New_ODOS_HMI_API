using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_API.Modelss
{
    [Table("OESProdCode")]
    public class ProdCodeModels
    {
        [Key, Column(Order = 1)]
        public string BarType { get; set; }
        [Key, Column(Order = 2)]
        public Int16 BarSize { get; set; }
        [Key, Column(Order = 3)]
        public int BarLength { get; set; }
        public int BundlePcs_fr { get; set; }
        public int BundlePcs { get; set; }
        public int BundleWT { get; set; }
        public string ProdCodeSAP { get; set; }
    }
}
