namespace OrderService.Dtos
{
    public class UpdateIncomingSE
    {
        public int OrderNumber { get; set; }

        public string CustomerCode { get; set; }

        public string ProjectCode { get; set; }

        public string OriginalSE { get; set; }

        public string UpdatedSE { get; set; }

        public int intOrginalPostHeaderId { get; set; }

        public string ProdType { get; set; }

        public string ScheduledProd { get; set; }

        public string UserName { get; set; }

        public int intUpdatedPostHeaderId { get; set; }

    }
}
