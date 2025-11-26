namespace OrderService.Dtos
{
    public class YMSDT_CUS_LOGDto
    {
        public string MANDT { get; set; }

        public string Docu_No { get; set; }

        public string Docu_tp { get; set; }

        public DateTime? Updt_Dt { get; set; }

        public DateTime? Sent_Dt { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }
    }
}
