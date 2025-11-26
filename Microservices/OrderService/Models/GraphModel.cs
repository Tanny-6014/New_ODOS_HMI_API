using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace OrderService.Models
{
    public class GraphModel
    {
        public string PODateFrom { get; set; }
        public string PODateTo { get; set; }
        public string checkdate { get; set; }
        public int OrderCount { get; set; }
     
        public string Month { get; set; }
        public string  day { get; set; }
        public string Year { get; set; }
        public string Product { get; set; }
        public int BPC { get; set; }
        public int CAB { get; set; }
        public int StandardBar { get; set; }
        public int StandardMESH { get; set; }
        public int DBIC { get; set; }
        public int CRWIC { get; set; }
        public int WIROD { get; set; }
        public int MESH { get; set; }
        public int Cages { get; set; }
       

    }
}