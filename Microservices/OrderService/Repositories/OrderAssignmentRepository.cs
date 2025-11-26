using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.ProjectServer.Client;
using MongoDB.Driver.Core.Configuration;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using OrderService.Constants;
using OrderService.Context;
using OrderService.Controllers;
using OrderService.Dtos;
using OrderService.Interfaces;
using OrderService.Models;
using Org.BouncyCastle.Ocsp;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using static iText.Kernel.Pdf.Colorspace.PdfPattern;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static OrderService.Models.SupplierDataResponse;

namespace OrderService.Repositories
{
    public class OrderAssignmentRepository : IOrderAssignment
    {
        //private OrderContext _dbContext;
        private readonly IConfiguration _configuration;
        private string connectionString;
        OracleTransaction oracleTransIDB;
        private readonly HttpClient client;
        private IDbContextFactory<OrderContext> _contextFactory;

        public OrderAssignmentRepository(IDbContextFactory<OrderContext> contextFactory, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _contextFactory = contextFactory;
            //_dbContext = dbContext;
            _configuration = configuration;
            client = httpClientFactory.CreateClient();
            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }

        public List<OrderDto> GetCabOrders(DateTime? FromReqDelDate = null, DateTime? ToReqDelDate = null, string? Customercode = null,
            string? Project = null, string? Producttype = null, string? Status = null, string? OrderNo = null)
        {
            IEnumerable<OrderDto> orderslist;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@FromReqDelDate", FromReqDelDate);
                parameters.Add("@ToReqDelDate", ToReqDelDate);
                parameters.Add("@Customercode", Customercode);
                parameters.Add("@Project", Project);
                parameters.Add("@producttype", Producttype);
                parameters.Add("@status", Status);
                parameters.Add("@OrderNo", OrderNo);
                sqlConnection.Open();

                orderslist = sqlConnection.Query<OrderDto>(SystemConstant.usp_GetOrderAssignment_OrdH, parameters, commandType: CommandType.StoredProcedure,

                        commandTimeout: 180);
                sqlConnection.Close();
                return orderslist.ToList();

            }
        }

        public List<OrderItemDet> GetOrderItems(string OrderRequestNo)
        {
            IEnumerable<OrderItemDet> orderItemslist;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@OrderReqNo", OrderRequestNo);
                    sqlConnection.Open();

                    orderItemslist = sqlConnection.Query<OrderItemDet>(SystemConstant.GetOrderAssignment_OrdItem, parameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return orderItemslist.ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<CustomerOrdAssgn> GetCustomersOrdAssgn()
        {
            IEnumerable<CustomerOrdAssgn> orderslist;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                orderslist = sqlConnection.Query<CustomerOrdAssgn>(SystemConstant.GetCustomer_OrdAssgn, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return orderslist.ToList();

            }
        }

        //public bool SaveOrderAssignment(List<Ordersdata> OrderNoList,string AssignedTo)
        public bool SaveOrderAssignment(List<string> OrderNoList,string AssignedTo)
        //public bool SaveOrderAssignment(List<string> OrderNoList,string AssignedTo, string AssignedBy)
        {
            int retunvalue = 0;
            try
            {
                if (OrderNoList != null && OrderNoList.Count > 0)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();

                        for (int i = 0; i < OrderNoList.Count; i++)
                        {
                            var dynamicParameters = new DynamicParameters();
                            //dynamicParameters.Add("@OrderRequestNo", OrderNoList[i].OrderRequestNo.ToString());
                            //dynamicParameters.Add("@Extralength", OrderNoList[i].ExtraLength.ToString());
                            dynamicParameters.Add("@OrderRequestNo", OrderNoList[i].ToString());
                            dynamicParameters.Add("@AssignedTo", AssignedTo);
                            //dynamicParameters.Add("@AssignedBy", AssignedBy);
                            dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                            sqlConnection.QueryFirstOrDefault<int>(SystemConstant.Save_OrderAssignment, dynamicParameters, commandType: CommandType.StoredProcedure);

                            retunvalue = dynamicParameters.Get<int>("Output");

                        }
                        sqlConnection.Close();
                    }
                }
                return true;
            }

            catch (Exception ex)
            {

                return false;

                throw ex;

            }

        }

        //public bool SaveOrderWithdraw(List<string> OrderRequestNoList, string WithdrawBy)
        public bool SaveOrderWithdraw(List<string> OrderRequestNoList)
        {
            int retunvalue = 0;
            try
            {
                if (OrderRequestNoList != null && OrderRequestNoList.Count > 0)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();

                        for (int i = 0; i < OrderRequestNoList.Count; i++)
                        {
                            var dynamicParameters = new DynamicParameters();
                            dynamicParameters.Add("@OrderRequestNo", OrderRequestNoList[i].ToString());
                            //dynamicParameters.Add("@WithdrawBy", WithdrawBy);
                            dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                            sqlConnection.QueryFirstOrDefault<int>(SystemConstant.Save_OrderWithdraw, dynamicParameters, commandType: CommandType.StoredProcedure);

                            retunvalue = dynamicParameters.Get<int>("Output");

                        }
                        sqlConnection.Close();
                    }
                }
                return true;
            }

            catch (Exception ex)
            {

                return false;

                throw ex;

            }
        }
        public List<OrderDto> GetOrdersOutsource(DateTime? FromReqDelDate = null, DateTime? ToReqDelDate = null, string? Customercode = null,
                    string? Project = null, string? Producttype = null, string? Status = null, string? OrderNo = null)
        {
            IEnumerable<OrderDto> orderslist;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@FromReqDelDate", FromReqDelDate);
                parameters.Add("@ToReqDelDate", ToReqDelDate);
                parameters.Add("@Customercode", Customercode);
                parameters.Add("@Project", Project);
                parameters.Add("@producttype", Producttype);
                parameters.Add("@status", Status);
                parameters.Add("@OrderNo", OrderNo);
                sqlConnection.Open();


                orderslist = sqlConnection.Query<OrderDto>(SystemConstant.GetOrderAssignment_Outsource_OrdH, parameters, commandType: CommandType.StoredProcedure,

                        commandTimeout: 180);
                sqlConnection.Close();
                return orderslist.ToList();

            }
        }

        public async Task<IEnumerable<FullOutsource>> GetFullOrdersDetails(List<string> orderRequestNos)
        {
            using var sqlConnection = new SqlConnection(connectionString);
            await sqlConnection.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@OrderRequestNos", string.Join(",", orderRequestNos));

            var orders = await sqlConnection.QueryAsync<FullOutsource>(
                SystemConstant.GetFullOutsourceOrderDetails,
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 180
            );

            return orders;
        }


        //public List<OrdAssgnSummary> GetOrderAssgnmntSummary(DateTime? FromReqDelDate = null, DateTime? ToReqDelDate = null)
        //{
        //    IEnumerable<OrdAssgnSummary> orderslist;

        //    using (var sqlConnection = new SqlConnection(connectionString))
        //    {
        //        var parameters = new DynamicParameters();
        //        parameters.Add("@FromReqDelDate", FromReqDelDate);
        //        parameters.Add("@ToReqDelDate", ToReqDelDate);
        //        sqlConnection.Open();

        //        orderslist = sqlConnection.Query<OrdAssgnSummary>(SystemConstant.GetOrderAssignment_Summary, parameters, commandType: CommandType.StoredProcedure);
        //        sqlConnection.Close();
        //        return orderslist.ToList();

        //    }
        //}

        public async Task<(bool IsSuccess, string Message)> GenerateBatchForOneAsync(GenerateBatchRequest request)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var lProcessObj = new ProcessController();
            OracleConnection cnIDB = new OracleConnection();
            lProcessObj.OpenIDBConnection(ref cnIDB);

            using var cmdIDB = cnIDB.CreateCommand();
            cmdIDB.CommandTimeout = 15000;

            foreach (var soNum in request.SoNums)
            {
                try
                {
                    var existingCount = await connection.ExecuteScalarAsync<int>(
                    @"SELECT COUNT(*) FROM MES_ORDER_TAG_PLAN WHERE MES_TAG_SO_NUM = @SO_NUM",
                    new { SO_NUM = soNum });

                    bool tagsAlreadyExist = existingCount > 0;

                    int successCount = 0;


                    //  TAG GENERATION RUNS ONLY IF tags do NOT already exist

                    if (!tagsAlreadyExist)
                    {
                        var sql = @"SELECT h.order_no         AS QT_SO_NUM,
                            i.item_no         AS QT_ITEM_NO,
                            i.material_no     AS QT_MATL_NO,
                            i.sap_product_group AS QT_PROD_GRP,
                            i.order_pieces    AS QT_ITEM_PCS,
                            i.fg_production_wt_kg AS QT_INV_WT_KG,
                            i.order_qty       AS QT_SALES_UOM_WT,
                            i.sales_uom       AS QT_SALES_UOM,
                            i.qty_in_base_uom  AS QT_QTY_IN_BASE_UOM,
                            h.cust_code   AS QT_CLIENT,
                            h.order_request_no AS QT_SOR_NUM,
                            i.delivery_status AS QT_SAP_STATUS,
                            'A'               AS QT_MES_STATUS,
                            h.create_by       AS QT_CREATE_BY,
                            h.create_date     AS QT_CREATE_DATE
                            FROM SCM_ORDER_HEADER h
                            JOIN SCM_ORDER_ITEM i
                                ON h.order_request_no = i.order_request_no
                            WHERE h.order_no = @SO_NUM ";

                        var orderItems = (await connection.QueryAsync<MesOrderItem>(
                        sql,
                        new { SO_NUM = soNum }
                        )).ToList();

                        if (orderItems.Count == 0)
                            continue;

                        var tagAssign = new TagID_Assign(connection);

                        foreach (var item in orderItems)
                        {
                            using (var tran = connection.BeginTransaction())
                            {
                                try
                                {
                                    var dtOrder = ConvertToDataTable(item);

                                    DataTable dtTags = await tagAssign.TagAssignmentAsync(dtOrder, item.QT_PROD_GRP, tran);

                                    foreach (DataRow dr in dtTags.Rows)
                                    {
                                        await connection.ExecuteAsync(@"
                                    INSERT INTO MES_ORDER_TAG_PLAN
                                    (MES_TAG_SO_NUM, MES_TAG_ITEM_NUM, MES_TAG_MATL_NO, MES_TAG_PROD_GRP,
                                     MES_TAG_INDPNT_ITEM_NO, MES_TAG_BATCHID, MES_TAG_WRK_CTR, MES_TAG_PRODN_DATE,
                                     MES_TAG_PRODN_SHIFT, MES_TAG_RUN_NO, MES_TAG_RELEASE_CODE, MES_TAG_FG_QTY_KG,
                                     MES_TAG_QTY_PCS, MES_TAG_CREATE_BY, MES_TAG_CREATE_DATE, MES_TAG_STATUS)
                                    VALUES
                                    (@SO_NUM, @ITEM_NO, @MATL_NO, @PROD_GRP, @INDPNT_ITEM_NO, @BATCH_ID, @WRK_CTR, @PRODN_DATE,
                                     @SHIFT, @RUN_NO, @RELEASE_CODE, @FG_QTY, @QTY_PCS, @CREATE_BY, @CREATE_DATE, @STATUS)",
                                            new
                                            {
                                                SO_NUM = item.QT_SO_NUM,
                                                ITEM_NO = item.QT_ITEM_NO,
                                                MATL_NO = item.QT_MATL_NO,
                                                PROD_GRP = item.QT_PROD_GRP,
                                                INDPNT_ITEM_NO = item.QT_ITEM_NO,
                                                BATCH_ID = dr["Batch_ID"],
                                                WRK_CTR = item.QT_WRK_CTR,
                                                PRODN_DATE = DateTime.Now,
                                                SHIFT = item.QT_PRODN_SHIFT ?? "A",
                                                RUN_NO = item.QT_RUN_NUM,
                                                RELEASE_CODE = item.QT_RELEASE_CODE,
                                                FG_QTY = Convert.ToDecimal(dr["INV_WT"]),
                                                QTY_PCS = Convert.ToInt32(dr["COUNT_PIECES"]),
                                                CREATE_BY = item.QT_CREATE_BY ?? "SYSTEM",
                                                CREATE_DATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                STATUS = "A"
                                            }, tran);

                                        string spName = item.QT_PROD_GRP switch
                                        {
                                            "CAB" => "Insert_into_tbl_scm_sts_cab_print_tag",
                                            "BPC" => "Insert_into_tbl_scm_sts_bpc_print_tag",
                                            "PRC" => "Insert_into_tbl_scm_sts_prc_print_tag",
                                            "MSD" => "Insert_into_tbl_scm_sts_msd_print_tag",
                                            "PRE" => "Insert_into_tbl_scm_sts_pre_print_tag",
                                            _ => null
                                        };

                                        if (!string.IsNullOrEmpty(spName))
                                        {
                                            try
                                            {
                                                await connection.ExecuteAsync(
                                                    $"EXEC {spName} @SO_Number, @Item_Number, @Batch_Id",
                                                    new
                                                    {
                                                        SO_Number = item.QT_SO_NUM,
                                                        Item_Number = item.QT_ITEM_NO,
                                                        Batch_Id = dr["Batch_ID"].ToString()
                                                    },
                                                    tran
                                                );
                                            }
                                            catch (Exception spEx)
                                            {
                                                Console.WriteLine("Error", spEx);
                                            }
                                        }
                                    }

                                    tran.Commit();
                                    successCount++;
                                }
                                catch
                                {
                                    tran.Rollback();
                                    continue;
                                }
                            }

                        }
                    }

                    //if (successCount > 0 || tagsAlreadyExist)
                    //{

                    string updateOutsourcingSql = @"
                        UPDATE SCM_ORDER_HEADER
                        SET 
                            Outsourcing_Flag = 'Y',
                            Delivery_By = @DeliveryBy,
                            TypeOfOutsourcing = 'Full Outsourcing',
                            ServiceOutsourced = '',
                            AssignedTo='Outsource',
                            update_date = GETDATE(),
                            update_by = 'SYSTEM',
                            VendorCode=@VendorCode,
                            VendorName=@VendorName,
                            ContractNo=@ContractNo,
                            ReqDelDate=@ReqDelDate
                        WHERE order_no = @OrderNo;";

                    await connection.ExecuteAsync(updateOutsourcingSql, new
                    {
                        OrderNo = soNum,
                        DeliveryBy = request.DeliveredBy,
                        VendorCode = request.VendorCode,
                        VendorName = request.VendorName,
                        ContractNo = request.ContractNo,
                        ReqDelDate = request.ReqDelDate
                    });

                    var lSQL = @"UPDATE SCM_ADMIN.order_header 
                        SET DELIVERY_BY = :DeliveredBy,
                            TYPE_OF_OUTSOURCING = 'Full Outsourcing',
                            ASSIGNED_TO = 'O',
                            SERVICE_OUTSOURCE = ''
                        WHERE ORDER_NO = :OrderNo";

                    var lOracleCmd = new OracleCommand(lSQL, cnIDB);
                    lOracleCmd.Parameters.Add(new OracleParameter(":DeliveredBy", request.DeliveredBy));
                    lOracleCmd.Parameters.Add(new OracleParameter(":OrderNo", soNum));
                    lOracleCmd.CommandTimeout = 1200;
                    lOracleCmd.Transaction = oracleTransIDB;
                    lOracleCmd.ExecuteNonQuery();

                //}
                   
                    //  ASSIGN ORDER 
                    
                    var assignOrder = await OrderHeaderItem(soNum);

                    if (assignOrder.IsSuccess)
                    {
                        string updateQuery = @"
                UPDATE SCM_ORDER_HEADER
                SET 
                    AssignOrderStatus='Y',
                    PR_Status='A'
                WHERE order_no = @OrderNo;";

                        await connection.ExecuteAsync(updateQuery, new { OrderNo = soNum });

                        var lSQLS = @"UPDATE SCM_ADMIN.order_header 
                            SET PR_STATUS='A'
                            WHERE ORDER_NO = :OrderNo";

                        var lOracleCmdS = new OracleCommand(lSQLS, cnIDB);
                        lOracleCmdS.Parameters.Add(new OracleParameter(":OrderNo", soNum));
                        lOracleCmdS.CommandTimeout = 1200;
                        lOracleCmdS.Transaction = oracleTransIDB;
                        lOracleCmdS.ExecuteNonQuery();
                    }
                    else
                    {
                        string updateQuery = @"
                UPDATE SCM_ORDER_HEADER
                SET 
                    AssignOrderStatus='E',
                    PR_Status='E'
                WHERE order_no = @OrderNo;";

                        await connection.ExecuteAsync(updateQuery, new { OrderNo = soNum });

                        var lSQLE = @"UPDATE SCM_ADMIN.order_header 
                            SET PR_STATUS='E'
                            WHERE ORDER_NO = :OrderNo";

                        var lOracleCmdE = new OracleCommand(lSQLE, cnIDB);
                        lOracleCmdE.Parameters.Add(new OracleParameter(":OrderNo", soNum));
                        lOracleCmdE.CommandTimeout = 1200;
                        lOracleCmdE.Transaction = oracleTransIDB;
                        lOracleCmdE.ExecuteNonQuery();
                    }

                    return (true, $"Batch generated successfully for {successCount} item(s) in {soNum}");
                }
                catch (Exception ex)
                {
                    return (false, $"Error in batch generation for {soNum}: {ex.Message}");
                }
            }

            return (true, "Batch generation attempted for all orders.");
        }


        private DataTable ConvertToDataTable(dynamic orderItem)
        {
            var dt = new DataTable();
            dt.Columns.Add("FLD_VATTR_INV_WT_ORDER", typeof(decimal));
            dt.Columns.Add("FLD_VATTR_COUNT", typeof(int));
            dt.Columns.Add("FLD_VATTR_INV_WT_PER_PC", typeof(decimal));
            dt.Columns.Add("FLD_QTY_IN_BASE_UOM", typeof(decimal));

            var row = dt.NewRow();
            row["FLD_VATTR_INV_WT_ORDER"] = Convert.ToDecimal(orderItem.QT_INV_WT_KG);
            row["FLD_VATTR_COUNT"] = Convert.ToInt32(orderItem.QT_ITEM_PCS);
            row["FLD_VATTR_INV_WT_PER_PC"] = Convert.ToDecimal(orderItem.QT_SALES_UOM_WT);
            row["FLD_QTY_IN_BASE_UOM"] = Convert.ToDecimal(orderItem.QT_QTY_IN_BASE_UOM);
            dt.Rows.Add(row);

            return dt;
        }
        private class TagID_Assign
        {
            private readonly SqlConnection _connection;
            private int[] TAG_PIECES_ARRY;
            private int TAG_PIECES, tag_no_cnt;

            public TagID_Assign(SqlConnection connection)
            {
                _connection = connection;
            }

            public async Task<DataTable> TagAssignmentAsync(DataTable dt, string prodGrp, SqlTransaction tran)
            {
                return prodGrp switch
                {
                    "BPC" => await TagAssignmentBPC(dt, tran),
                    "PRC" => await TagAssignmentPRC(dt, tran),
                    "CAB" => await TagAssignmentCAB(dt, tran),
                    "MSD" => await TagAssignmentMSD(dt, tran),
                    "PRE" => await TagAssignmentPRE(dt, tran),
                    _ => throw new Exception($"Unknown PROD_GRP: {prodGrp}")
                };
            }

            private async Task<DataTable> TagAssignmentBPC(DataTable dt, SqlTransaction tran) =>
                await GenerateTags(dt, "BPC", "O", tran);

            private async Task<DataTable> TagAssignmentPRC(DataTable dt, SqlTransaction tran) =>
                await GenerateTags(dt, "PRC", "F", tran);

            private async Task<DataTable> TagAssignmentCAB(DataTable dt, SqlTransaction tran) =>
                await GenerateTagsWithWeightLogic(dt, "CAB", 2000, 200, "A", tran);

            private async Task<DataTable> TagAssignmentMSD(DataTable dt, SqlTransaction tran) =>
                await GenerateTagsWithWeightLogic(dt, "MSD", 1500, 25, "M", tran);

            private async Task<DataTable> TagAssignmentPRE(DataTable dt, SqlTransaction tran) =>
                await GenerateTagsWithWeightLogic(dt, "PRE", 2000, 200, "P", tran);

            // Common tag generation
            private async Task<DataTable> GenerateTags(DataTable dt, string prodGrp, string prefix, SqlTransaction tran)
            {
                DataTable dt_tag_wt_pcs = CreateResultTable();

                foreach (DataRow dr in dt.Rows)
                {
                    double weight = Convert.ToDouble(dr["FLD_VATTR_INV_WT_ORDER"]);
                    int pieces = Convert.ToInt32(dr["FLD_VATTR_COUNT"]);
                    double qtyInBaseUOM = Convert.ToDouble(dr["FLD_QTY_IN_BASE_UOM"]);
                    double unitWeight = Math.Round(qtyInBaseUOM / pieces, 3);
                    double tag_FG_Qty_Kg = unitWeight;
                    int tag_Qty_Pcs = 1;

                    for (int counter = 0; counter < pieces; counter++)
                    {
                        int seq = await GetNextSequence(prodGrp, tran);
                        string tagId = BuildTagID(prefix, seq);
                        dt_tag_wt_pcs.Rows.Add(tagId, tag_FG_Qty_Kg, tag_Qty_Pcs);
                    }
                }

                return dt_tag_wt_pcs;
            }

            // Tag generation with weight & pieces logic (CAB/MSD/PRE)
            private async Task<DataTable> GenerateTagsWithWeightLogic(DataTable dt, string prodGrp, double weightLimit, int piecesLimit, string prefix, SqlTransaction tran)
            {
                DataTable dt_tag_wt_pcs = CreateResultTable();

                foreach (DataRow dr in dt.Rows)
                {
                    double weight = Convert.ToDouble(dr["FLD_VATTR_INV_WT_ORDER"]);
                    int pieces = Convert.ToInt32(dr["FLD_VATTR_COUNT"]);
                    decimal inv_wt_per_pc = dr.Table.Columns.Contains("FLD_VATTR_INV_WT_PER_PC")
                        ? Convert.ToDecimal(dr["FLD_VATTR_INV_WT_PER_PC"]) : 0;

                    if ((weight > weightLimit && pieces < piecesLimit) ||
                        (weight > weightLimit && pieces > piecesLimit && inv_wt_per_pc > 10))
                    {
                        tag_no_cnt = TagNumOfCountActWT(pieces, Convert.ToInt32(weight));
                    }
                    else if ((weight < weightLimit + 1) && (pieces > piecesLimit))
                    {
                        tag_no_cnt = TagNumOfCount(pieces);
                    }
                    else
                    {
                        tag_no_cnt = 1;
                        TAG_PIECES_ARRY = new int[1] { pieces };
                    }

                    for (int counter = 0; counter < tag_no_cnt; counter++)
                    {
                        TAG_PIECES = TAG_PIECES_ARRY[counter];
                        double qtyInBaseUOM = Convert.ToDouble(dr["FLD_QTY_IN_BASE_UOM"]);
                        double unitWeight = Math.Round(qtyInBaseUOM / pieces, 3);
                        double tag_FG_Qty_Kg = Math.Round(unitWeight * TAG_PIECES, 3);

                        int seq = await GetNextSequence(prodGrp, tran);
                        string tagId = BuildTagID(prefix, seq);

                        dt_tag_wt_pcs.Rows.Add(tagId, tag_FG_Qty_Kg, TAG_PIECES);
                    }
                }

                return dt_tag_wt_pcs;
            }

            private DataTable CreateResultTable()
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Batch_ID", typeof(string));
                dt.Columns.Add("INV_WT", typeof(double));
                dt.Columns.Add("COUNT_PIECES", typeof(int));
                return dt;
            }

            private string BuildTagID(string prefix, int seq)
            {
                string year = DateTime.Now.ToString("yy");
                string strSeq = seq.ToString().PadLeft(5, '0');
                return prefix + year + "90" + strSeq;
            }

            // Sequence logic using database
            //private async Task<int> GetMaxSequence(string prodGrp)
            //{
            //    using var conn = new SqlConnection(_connectionString);
            //    string sql = "SELECT SEQUENCE FROM BCP_MAX_TAG_SEQ WHERE PROD_GRP = @ProdGrp";
            //    var seq = await conn.ExecuteScalarAsync<int?>(sql, new { ProdGrp = prodGrp });
            //    return seq ?? 0;
            //}

            //private async Task SetMaxSequence(string prodGrp, int seq)
            //{
            //    using var conn = new SqlConnection(_connectionString);
            //    string sql = "UPDATE BCP_MAX_TAG_SEQ SET SEQUENCE = @Seq WHERE PROD_GRP = @ProdGrp";
            //    await conn.ExecuteAsync(sql, new { Seq = seq, ProdGrp = prodGrp });
            //}
            private async Task<int> GetNextSequence(string prodGrp, SqlTransaction tran)
            {
                var sql = @"
                UPDATE MAX_TAG_SEQ
                SET SEQUENCE = CASE WHEN SEQUENCE >= 99999 THEN 1 ELSE SEQUENCE + 1 END
                OUTPUT inserted.SEQUENCE
                WHERE PROD_GRP = @ProdGrp;";

                return await _connection.ExecuteScalarAsync<int>(sql, new { ProdGrp = prodGrp }, tran);
            }
            // Original helper methods
            private int TagNumOfCount(int VAR_ATTR_COUNT)
            {
                int divno = VAR_ATTR_COUNT / 200;
                int divextra = VAR_ATTR_COUNT % 200;
                if (divextra > 0) divno++;

                int new_tag_pcs = VAR_ATTR_COUNT / divno;
                int extrapart = VAR_ATTR_COUNT - (new_tag_pcs * divno);
                TAG_PIECES_ARRY = new int[divno];
                for (int a = 0; a < divno; a++)
                    TAG_PIECES_ARRY[a] = (a == divno - 1) ? new_tag_pcs + extrapart : new_tag_pcs;

                return divno;
            }
            private int TagNumOfCountActWT(int VAR_ATTR_COUNT, int VAR_ATTR_INV_WT_ORDER)
            {
                int divno = VAR_ATTR_INV_WT_ORDER / 2000;
                int divextra = VAR_ATTR_COUNT % 200;
                if (divextra > 0) divno++;

                int new_tag_pcs = VAR_ATTR_COUNT / divno;
                int extrapart = VAR_ATTR_COUNT - (new_tag_pcs * divno);
                TAG_PIECES_ARRY = new int[divno];
                for (int a = 0; a < divno; a++)
                    TAG_PIECES_ARRY[a] = (a == divno - 1) ? new_tag_pcs + extrapart : new_tag_pcs;

                if (divno == 0) { TAG_PIECES_ARRY = new int[1] { VAR_ATTR_COUNT }; divno = 1; }

                return divno;
            }
        }
        public async Task<(bool IsSuccess, string Message)> OrderHeaderItem(string input)
        {
            string orderNumber = input;
            string jsonResult = null;
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                await using var connection = context.Database.GetDbConnection();


                var headers = (await connection.QueryAsync<dynamic>(
                    @"SELECT order_no FROM SCM_Order_Header 
                  WHERE order_no = @orderNo 
                    AND AssignedTo = 'Outsource' 
                    AND (AssignOrderStatus='Y' OR PR_Status='Y')",
                    new { orderNo = orderNumber }
                )).ToList();

                if (headers.Any())
                    return (false, "Order is already assigned.");


                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_GetOrderDetails_JSON1", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OrderID", orderNumber);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess))
                        {
                            if (await reader.ReadAsync())
                            {
                                using (TextReader tr = reader.GetTextReader(reader.GetOrdinal("JSON_Result")))
                                {
                                    jsonResult = await tr.ReadToEndAsync();
                                }
                            }
                        }
                    }
                    if (jsonResult == null)
                    {
                        return (false, "No data present");
                    }
                    string jsonData = JsonConvert.SerializeObject(jsonResult, Formatting.Indented);

                    var content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                    var request = new HttpRequestMessage(HttpMethod.Post, "http://192.168.0.62:18080/esb/comm/api")
                    //var request = new HttpRequestMessage(HttpMethod.Post, "http://111.119.204.179:18080/esb/comm/api")
                    {
                        Content = content
                    };
                    request.Headers.Add("serviceCode", "174P175802");
                    request.Headers.Add("sourceCode", "174P");
                    request.Headers.Add("requestId", "5a1940e2-8b83-42d1-be8d-2c413782f639"); //5a1940e2-8b83-42d1-be8d-2c413782f639

                    var response = await client.SendAsync(request);

                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var APIResponse = JsonConvert.DeserializeObject<APIResponse>(jsonResponse);

                    if (APIResponse != null && APIResponse.CODE == "200" && APIResponse.success == "true")
                    {

                        string updateQuery = @"
                            UPDATE SCM_ORDER_HEADER
                            SET 
                               AssignOrderStatus='Y',
                               PR_Status='A'
                            WHERE order_no = @OrderNo;";

                        await conn.ExecuteAsync(updateQuery, new { OrderNo = orderNumber });

                        return (true, "sent successfully");
                    }

                    else
                    {
                        string updateQuery = @"
                            UPDATE SCM_ORDER_HEADER
                            SET 

                               AssignOrderStatus='E',
                               PR_Status='E'
                            WHERE order_no = @OrderNo;";

                        await conn.ExecuteAsync(updateQuery, new { OrderNo = orderNumber });
                        return (false, "In error while assgining order");
                    }
                }

            }
            catch (Exception ex)
            {
                try
                {
                    using (var conn = new SqlConnection(connectionString))
                    {
                        await conn.OpenAsync();

                        string updateQuery = @"
                UPDATE SCM_ORDER_HEADER
                SET AssignOrderStatus = 'E',
                    PR_Status = 'E'
                WHERE order_no = @OrderNo;";

                        await conn.ExecuteAsync(updateQuery, new { OrderNo = orderNumber });
                    }
                }
                catch (Exception updateEx)
                {
                    return (false,$"Order {orderNumber} failed. API Error: {ex.Message}. DB Update Error: {updateEx.Message}");
                }

               
                return (false, $"Order {orderNumber} failed. API Error: {ex.Message}");
            }
        }
        public async Task<(bool IsSuccess, string Message)> AssignOrderOutsource(GenerateBatchRequest req)
        {
            OracleConnection cnIDB = null;
            OracleTransaction oracleTransIDB = null;

            try
            {
                var lProcessObj = new ProcessController();
                cnIDB = new OracleConnection();
                lProcessObj.OpenIDBConnection(ref cnIDB);

                oracleTransIDB = cnIDB.BeginTransaction();

                await using var context = _contextFactory.CreateDbContext();
                await using var connection = context.Database.GetDbConnection();

                foreach (var soNum in req.SoNums)
                {

                    var headers = (await connection.QueryAsync<dynamic>(
                        @"SELECT order_no FROM SCM_Order_Header 
                  WHERE order_no = @orderNo 
                    AND AssignedTo = 'Outsource' 
                    AND (AssignOrderStatus='Y' OR PR_Status='Y')",
                        new { orderNo = soNum }
                    )).ToList();

                    if (headers.Any())
                        continue;

                    string jsonResult = null;


                    using (var sqlConn = new SqlConnection(connectionString))
                    {
                        await sqlConn.OpenAsync();
                        using (var cmd = new SqlCommand("sp_GetOrderDetails_JSON2", sqlConn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@OrderID", soNum);
                            cmd.Parameters.AddWithValue("@OutsourceType", req.Type);
                            cmd.Parameters.AddWithValue("@VendorCode", req.VendorCode);
                            cmd.Parameters.AddWithValue("@ReqDelDate", req.ReqDelDate);
                            cmd.Parameters.AddWithValue("@ContractNo", req.ContractNo);

                            using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess);
                            if (await reader.ReadAsync())
                            {
                                using var tr = reader.GetTextReader(reader.GetOrdinal("JSON_Result"));
                                jsonResult = await tr.ReadToEndAsync();
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(jsonResult))
                        return (false, $"No data found for Order {soNum}");

                    var content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                    //var request = new HttpRequestMessage(HttpMethod.Post, "http://111.119.204.179:18080/esb/comm/api")
                    var request = new HttpRequestMessage(HttpMethod.Post, "http://192.168.0.62:18080/esb/comm/api")
                    {
                        Content = content
                    };
                    request.Headers.Add("serviceCode", "174P175802");
                    request.Headers.Add("sourceCode", "174P");
                    request.Headers.Add("requestId", Guid.NewGuid().ToString());

                    var response = await client.SendAsync(request);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<APIResponse>(jsonResponse);

                    bool success = apiResponse?.CODE == "200" && apiResponse?.success == "true";

                    using var sqlConn2 = new SqlConnection(connectionString);
                    await sqlConn2.OpenAsync();

                    string status = success ? "A" : "E";

                    string updateQuery = @"
                UPDATE SCM_ORDER_HEADER
                SET 
                    AssignOrderStatus = @AssignOrderStatus,
                    PR_Status = @PRStatus,
                    Delivery_By = @DeliveryBy,
                    TypeOfOutsourcing = @Type,
                    ServiceOutsourced = '',
                    AssignedTo = 'Outsource',
                    update_date = GETDATE(),
                    update_by = 'SYSTEM',
                    VendorCode = @VendorCode,
                    VendorName = @VendorName,
                    ContractNo = @ContractNo,
                    ReqDelDate = @ReqDelDate
                WHERE order_no = @OrderNo;";

                    await sqlConn2.ExecuteAsync(updateQuery, new
                    {
                        AssignOrderStatus = success ? "Y" : "E",
                        PRStatus = status,
                        DeliveryBy = req.DeliveredBy,
                        Type = req.Type,
                        VendorCode = req.VendorCode,
                        VendorName = req.VendorName,
                        ContractNo = req.ContractNo,
                        ReqDelDate = req.ReqDelDate,
                        OrderNo = soNum
                    });


                    string lSQL = @"
                UPDATE SCM_ADMIN.order_header 
                SET DELIVERY_BY = :DeliveredBy,
                    TYPE_OF_OUTSOURCING = :OutsourceType,
                    ASSIGNED_TO = 'O',
                    PR_Status = :PRStatus,
                    SERVICE_OUTSOURCE = ''
                WHERE ORDER_NO = :OrderNo";

                    using var lOracleCmd = new OracleCommand(lSQL, cnIDB);
                    lOracleCmd.BindByName = true;
                    lOracleCmd.Parameters.Add(new OracleParameter("DeliveredBy", req.DeliveredBy));
                    lOracleCmd.Parameters.Add(new OracleParameter("OutsourceType", req.Type));
                    lOracleCmd.Parameters.Add(new OracleParameter("PRStatus", status));
                    lOracleCmd.Parameters.Add(new OracleParameter("OrderNo", soNum));
                    lOracleCmd.Transaction = oracleTransIDB;
                    lOracleCmd.CommandTimeout = 1200;
                    lOracleCmd.ExecuteNonQuery();
                }

                oracleTransIDB.Commit();
                return (true, "Assign order outsourcing completed successfully.");
            }
            catch (Exception ex)
            {

                return (false, $"Error during assignment: {ex.Message}");
            }

        }
        public async Task<(bool IsSuccess, string Message, List<SupplierRes> Data)> PostSupplierDataAsync(string? productType)
        {
            try
            {

                var requestPayload = new
                {
                    PRODUCT_TYPE = productType

                    //SERVICE_TYPE = SERVICE_TYPE
                };

                string jsonData = JsonConvert.SerializeObject(requestPayload);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");


                var request = new HttpRequestMessage(HttpMethod.Post, "http://192.168.0.62:18080/esb/comm/api")
                //var request = new HttpRequestMessage(HttpMethod.Post, "http://111.119.204.179:18080/esb/comm/api")

                {
                    Content = content
                };

                request.Headers.Add("serviceCode", "174P175801");
                request.Headers.Add("sourceCode", "174P");
                request.Headers.Add("requestId", Guid.NewGuid().ToString());

                var response = await client.SendAsync(request);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<SupplierDataResponse>(jsonResponse);

                if (responseData == null)
                {
                    return (false, "No response data received.", null);
                }
                var list = responseData.Suppliers
                 .GroupBy(s => new { s.SupplierCode, s.SupplierName })
                 .Select(g => new SupplierRes
                 {
                     SupplierCode = g.Key.SupplierCode,
                     SupplierName = g.Key.SupplierName,
                     ProductType = g.First().ProductType,
                     ServiceType = g.First().ServiceType,
                     Contracts = g.SelectMany(x => x.ContractList.Select(c => new ContractList
                     {
                         ContractNo = c.ContractNo,
                         ContractDesc = c.ContractDesc
                     })).ToList()
                 })
                 .ToList();

                return (true, "Data processed successfully.", list);



            }
            catch (Exception ex)
            {

                return (false, $"Error: {ex.Message}", null);
            }
        }

        public async Task<(bool IsSuccess, string Message)> Delete_OutsourceData(string orderNo)
        {
           
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                string jsonResult = null;
                try
                {

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        await conn.OpenAsync();

                        using (SqlCommand cmd = new SqlCommand("sp_DeleteOrderDetails_JSON", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@OrderID", orderNo);




                            using (SqlDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess))
                            {
                                if (await reader.ReadAsync())
                                {
                                    using (TextReader tr = reader.GetTextReader(reader.GetOrdinal("JSON_Result")))
                                    {
                                        jsonResult = await tr.ReadToEndAsync();
                                    }
                                }
                            }


                        }

                        if (jsonResult == null)
                        {
                            return (false, "No data present");
                        }
                        string jsonData = JsonConvert.SerializeObject(jsonResult, Formatting.Indented);

                        var content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                        var request = new HttpRequestMessage(HttpMethod.Post, "http://192.168.0.62:18080/esb/comm/api")
                        //var request = new HttpRequestMessage(HttpMethod.Post, "http://111.119.204.179:18080/esb/comm/api")
                        {
                            Content = content
                        };
                        request.Headers.Add("serviceCode", "174P175802");
                        request.Headers.Add("sourceCode", "174P");
                        request.Headers.Add("requestId", "5a1940e2-8b83-42d1-be8d-2c413782f639"); //5a1940e2-8b83-42d1-be8d-2c413782f639


                        var response = await client.SendAsync(request);


                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var APIResponse = JsonConvert.DeserializeObject<APIResponse>(jsonResponse);

                        if (APIResponse != null && APIResponse.CODE == "200" && APIResponse.success == "true")
                        {

                            string deleteOutsourcingSql = @"
                            UPDATE SCM_ORDER_HEADER
                            SET  
                               Outsourcing_Flag='C',
                               AssignOrderStatus='C'
                            WHERE order_no = @OrderNo;";

                            await connection.ExecuteAsync(deleteOutsourcingSql, new { OrderNo = orderNo });
                           
                        }
                    return (true, "Data Withdrawed Successfully.");
                }
                }
                catch (Exception ex)
                {

                    return (false, $"Error: {ex.Message}");
                }
    
        }

        public async Task<IEnumerable<dynamic>> GetSupplier_BatchPrinting()


        {


            IEnumerable<dynamic> supplierlist; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();

                supplierlist = sqlConnection.Query<dynamic>(SystemConstant.GetSupplier_BatchPrinting, commandType: CommandType.StoredProcedure);

                sqlConnection.Close();


                return supplierlist;


            }


        }





        public async Task<IEnumerable<dynamic>> GetSONO_BatchPrinting(int NoofDays, string Vendorcode)


        {


            IEnumerable<dynamic> SONolist; //new IEnumerable<List<ProjectMaster>>();





            using (var sqlConnection = new SqlConnection(connectionString))


            {


                sqlConnection.Open();


                var dynamicParameters = new DynamicParameters();


                dynamicParameters.Add("@NoOfDays", NoofDays);


                dynamicParameters.Add("@Vendorcode", Vendorcode);


                SONolist = sqlConnection.Query<dynamic>(SystemConstant.GetSONO_BatchPrinting, dynamicParameters, commandType: CommandType.StoredProcedure);


                sqlConnection.Close();


                return SONolist;


            }


        }





        public async Task<IEnumerable<dynamic>> GetBatch_BatchPrinting(string SONo)


        {


            IEnumerable<dynamic> Batchlist; //new IEnumerable<List<ProjectMaster>>();





            using (var sqlConnection = new SqlConnection(connectionString))


            {


                sqlConnection.Open();


                var dynamicParameters = new DynamicParameters();


                dynamicParameters.Add("@SONo", SONo);


                Batchlist = sqlConnection.Query<dynamic>(SystemConstant.GetBatch_BatchPrinting, dynamicParameters, commandType: CommandType.StoredProcedure);


                sqlConnection.Close();


                return Batchlist;


            }


        }
    }
}
