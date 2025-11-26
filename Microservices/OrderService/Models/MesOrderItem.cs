namespace OrderService.Models
{
    public class MesOrderItem
    {
     
            public string QT_CLIENT { get; set; }
            public string QT_SO_NUM { get; set; }
            public string QT_SOR_NUM { get; set; }
            public string QT_ITEM_NO { get; set; }
            public int QT_CLUBBED_ITEM_NO { get; set; }
            public string QT_MATL_NO { get; set; }
            public string QT_PROD_GRP { get; set; }
            public int QT_ITEM_PCS { get; set; }
            public decimal QT_INV_WT_KG { get; set; }
            public decimal QT_SALES_UOM_WT { get; set; }
            public string QT_SALES_UOM { get; set; }
            public string QT_REQ_DATE { get; set; }
            public string QT_CONF_DATE { get; set; }
            public string QT_SAP_STATUS { get; set; }
            public string QT_MES_STATUS { get; set; }
            public string QT_CREATE_BY { get; set; }
            public string QT_CREATE_DATE { get; set; }
            public string QT_MODIFY_BY { get; set; }
            public string QT_MODIFY_DATE { get; set; }
            public string QT_PRODN_DATE { get; set; }
            public string QT_PRODN_SHIFT { get; set; }
            public string QT_WRK_CTR { get; set; }
            public string QT_RELEASE_CODE { get; set; }
            public string QT_RUN_NUM { get; set; }
            public string QT_BBSNO { get; set; }
            public string QT_PROD_MARK { get; set; }
            public string QT_PROD_TYPE { get; set; }
            public string QT_PROD_CD { get; set; }
            public string QT_IN_OUT_SOURCE_STATUS { get; set; }
            public string QT_QTY_IN_BASE_UOM { get; set; }
    }

        public class MesOrderTagPlan
        {
            public string MES_TAG_SO_NUM { get; set; }
            public string MES_TAG_ITEM_NUM { get; set; }
            public string MES_TAG_MATL_NO { get; set; }
            public string MES_TAG_PROD_GRP { get; set; }
            public string MES_TAG_INDPNT_ITEM_NO { get; set; }
            public string MES_TAG_BATCHID { get; set; }
            public string MES_TAG_WRK_CTR { get; set; }
            public string MES_TAG_PRODN_DATE { get; set; }
            public string MES_TAG_PRODN_SHIFT { get; set; }
            public int? MES_TAG_STOR_LOC { get; set; }
            public string MES_TAG_RUN_NO { get; set; }
            public string MES_TAG_RELEASE_CODE { get; set; }
            public decimal? MES_TAG_FG_QTY_KG { get; set; }
            public int? MES_TAG_QTY_PCS { get; set; }
            public decimal? MES_TAG_TOT_LENGTH { get; set; }
            public decimal? MES_TAG_BEND { get; set; }
            public decimal? MES_TAG_CUT { get; set; }
            public decimal? MES_TAG_END { get; set; }
            public decimal? MES_TAG_STK { get; set; }
            public string MES_TAG_GR_STATUS { get; set; }
            public string MES_TAG_STATUS { get; set; }
            public string MES_TAG_CREATE_BY { get; set; }
            public string MES_TAG_CREATE_DATE { get; set; }
            public string MES_TAG_MODIFY_BY { get; set; }
            public string MES_TAG_MODIFY_DATE { get; set; }
            public int MES_TAG_REC_ID { get; set; }
        }

        // Helper DTO for generated batches
        public class BatchDto
        {
            public string BatchId { get; set; }
            public double InvWt { get; set; }
            public int CountPieces { get; set; }
        }
}

