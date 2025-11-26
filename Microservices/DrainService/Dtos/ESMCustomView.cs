namespace DrainService.Dtos
{
    public class ESMCustomView
    {
        public int ID { get; set; }               // ID INT Identity(1,1) PRIMARY KEY
        public string TRACKINGNO { get; set; }    // TRACKINGNO VARCHAR(20) NOT NULL
        public string VIEWNAME { get; set; }      // VIEWNAME VARCHAR(20)
        public string VIEWDESCRIPTION { get; set; } // VIEWDESCRIPTION VARCHAR(20)
        public string CREATEDBY { get; set; }     // CREATEDBY VARCHAR(20)
        public DateTime? CREATEDON { get; set; }  // CREATEDON DATE
        public string COLUMNIDS { get; set; }     // COLUMNIDS VARCHAR(1000)
    }

}
