using AutoMapper;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
using OrderService.Dtos;
using OrderService.Interfaces;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using System.Net;
using System.Globalization;
using OrderService.Models;
using System.Data;
using System.Diagnostics;
using OrderService.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Net.Http.Headers;
using MongoDB.Driver.Core.Clusters;
using System.Text;
using SixLabors.ImageSharp;
using NCalc;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewOrderController : Controller
    {
        private readonly IOrder _OrderRepository;
        private readonly IMapper _mapper;

        private DBContextModels db = new DBContextModels();

        public NewOrderController(IOrder orderService, IMapper mapper)
        {
            _OrderRepository = orderService;
            _mapper = mapper;

        }

        //[HttpGet]
        /////  [ValidateAntiForgeryHeader]
        //[Route("/getWBS2Multiple")]
        //public ActionResult getWBS2Multiple([FromBody] NewOrder newOrder)
        //{
        //    UserAccessController lUa = new UserAccessController();
        //    var lUserType = lUa.getUserType(User.Identity.GetUserName());
        //    lUa = null;

        //    var lWBS2 = new List<string>();
        //    var lWBS3 = new List<string>();

        //    var lDa = new SqlDataAdapter();
        //    var lCmd = new SqlCommand();
        //    var lDs = new DataSet();
        //    var cnNDS = new SqlConnection();
        //    SqlDataReader lRst;
        //    List<string> lWBS2F = new List<string>();
        //    var lSQL = "";

        //    var lProcess = new ProcessController();
        //    lProcess.OpenNDSConnection(ref cnNDS);
        //    if (cnNDS.State == ConnectionState.Open)
        //    {
        //        string lCond = "";
        //        if (newOrder.WBS1 != null && newOrder.WBS1.Count > 0)
        //        {
        //            for (int i = 0; i < newOrder.WBS1.Count; i++)
        //            {
        //                lCond = lCond + " OR vchWBS1 = '" + newOrder.WBS1[i] + "' ";
        //            }
        //            lCond = lCond.Substring(3);
        //        }
        //        else
        //        {
        //            lCond = lCond + " vchWBS1 = '' ";
        //        }

        //        if (lUserType == "TE" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
        //        {
        //            lSQL = "SELECT " +
        //            "rtrim(ltrim(vchWBS2)) as WBS2, " +
        //            "rtrim(ltrim(vchWBS3)) as WBS3  " +
        //            "FROM dbo.WBSElements E, dbo.WBS W, " +
        //            "dbo.ProjectMaster P, dbo.WBSElementsDetails D, " +
        //            "dbo.ProductTypeMaster T, dbo.StructureElementMaster S, " +
        //            "dbo.ContractMaster C " +
        //            "WHERE W.intWBSId = E.intWBSId " +
        //            "AND P.intProjectId = W.intProjectId " +
        //            "AND P.intContractID = C.intContractID " +
        //            "AND E.intWBSElementId = D.intWBSElementId " +
        //            "AND D.sitProductTypeID = T.sitProductTypeID " +
        //            "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
        //            "AND vchProjectCode = '" + newOrder.ProjectCode + "' " +
        //            "AND (vchProductType = 'CAB' " +
        //            "OR ((vchProductType = 'MSH' " +
        //            "OR vchProductType = 'CAR' " +
        //            "OR vchProductType = 'BPC' " +
        //            "OR vchProductType = 'PRC') " +
        //            "AND EXISTS(SELECT H.intPostHeaderid " +
        //            "FROM dbo.BBSPostHeader H " +
        //            "WHERE intWBSElementId = E.intWBSElementId " +
        //            "AND NOT EXISTS(SELECT R.intPostHeaderid " +
        //            "FROM dbo.BBSReleaseDetails R " +
        //            "WHERE R.intPostHeaderid = H.intPostHeaderid " +
        //            "AND R.tntStatusId = 12) ) ) ) " +
        //            "AND ( " + lCond + " ) " +
        //            "AND E.tntStatusId = 1 " +
        //            "AND D.intConfirm = 1 " +
        //            "GROUP BY " +
        //            "vchWBS2, " +
        //            "vchWBS3  " +
        //            "ORDER BY " +
        //            "(case when PATINDEX('B[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
        //            "PATINDEX('%[^0-9]%', substring(vchWBS2 + 'z', (PATINDEX('%[0-9]%', vchWBS2)), " +
        //            "len(vchWBS2)) + 'z') ) as int)    " +
        //            "else (case when PATINDEX('FDN[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
        //            "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
        //            "len(vchWBS2)) + 'z') ) as int) else  " +
        //            "case when(PATINDEX('B%', vchWBS2) > 0 OR PATINDEX('FDN%', vchWBS2) > 0) then 98  else 99 end " +
        //            "end) end), " +
        //            " " +
        //            "(case when PATINDEX('[^0-9]%',vchWBS2) > 0 then vchWBS2 " +
        //            "else '' end), " +
        //            " " +
        //            "(CASE WHEN PATINDEX('%[0-9]%',vchWBS2) > 0 THEN cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
        //            "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2) + 1), " +
        //            "len(vchWBS2)) + 'z') ) as int) " +
        //            "ELSE " +
        //            "cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), len(vchWBS2)), " +
        //            "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
        //            "len(vchWBS2)) + 'z') - 1) as int) " +
        //            "END),  " +
        //            "vchWBS2, " +
        //            "vchWBS3 ";
        //        }
        //        else
        //        {
        //            lSQL = "SELECT " +
        //            "rtrim(ltrim(vchWBS2)) as WBS2, " +
        //            "rtrim(ltrim(vchWBS3)) as WBS3  " +
        //            "FROM dbo.WBSElements E, dbo.WBS W, " +
        //            "dbo.ProjectMaster P, dbo.WBSElementsDetails D, " +
        //            "dbo.ProductTypeMaster T, dbo.StructureElementMaster S, " +
        //            "dbo.ContractMaster C " +
        //            "WHERE W.intWBSId = E.intWBSId " +
        //            "AND P.intProjectId = W.intProjectId " +
        //            "AND P.intContractID = C.intContractID " +
        //            "AND E.intWBSElementId = D.intWBSElementId " +
        //            "AND D.sitProductTypeID = T.sitProductTypeID " +
        //            "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
        //            "AND vchProjectCode = '" + newOrder.ProjectCode + "' " +
        //            "AND (vchProductType = 'CAB' " +
        //            "OR ((vchProductType = 'MSH' " +
        //            "OR vchProductType = 'CAR' " +
        //            "OR vchProductType = 'PRC') " +
        //            "AND EXISTS(SELECT H.intPostHeaderid " +
        //            "FROM dbo.BBSPostHeader H " +
        //            "WHERE intWBSElementId = E.intWBSElementId " +
        //            "AND NOT EXISTS(SELECT R.intPostHeaderid " +
        //            "FROM dbo.BBSReleaseDetails R " +
        //            "WHERE R.intPostHeaderid = H.intPostHeaderid " +
        //            "AND R.tntStatusId = 12) ) ) ) " +
        //            "AND ( " + lCond + " ) " +
        //            "AND E.tntStatusId = 1 " +
        //            "AND D.intConfirm = 1 " +
        //            "GROUP BY " +
        //            "vchWBS2, " +
        //            "vchWBS3  " +
        //            "ORDER BY " +
        //            "(case when PATINDEX('B[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
        //            "PATINDEX('%[^0-9]%', substring(vchWBS2 + 'z', (PATINDEX('%[0-9]%', vchWBS2)), " +
        //            "len(vchWBS2)) + 'z') ) as int)    " +
        //            "else (case when PATINDEX('FDN[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
        //            "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
        //            "len(vchWBS2)) + 'z') ) as int) else  " +
        //            "case when(PATINDEX('B%', vchWBS2) > 0 OR PATINDEX('FDN%', vchWBS2) > 0) then 98  else 99 end " +
        //            "end) end), " +
        //            " " +
        //            "(case when PATINDEX('[^0-9]%',vchWBS2) > 0 then vchWBS2 " +
        //            "else '' end), " +
        //            " " +
        //            "(CASE WHEN PATINDEX('%[0-9]%',vchWBS2) > 0 THEN cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
        //            "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2) + 1), " +
        //            "len(vchWBS2)) + 'z') ) as int) " +
        //            "ELSE " +
        //            "cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), len(vchWBS2)), " +
        //            "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
        //            "len(vchWBS2)) + 'z') - 1) as int) " +
        //            "END),  " +
        //            "vchWBS2, " +
        //            "vchWBS3 ";
        //        }

        //        lCmd.CommandType = CommandType.Text;
        //        lCmd.CommandText = lSQL;
        //        lCmd.Connection = cnNDS;
        //        lCmd.CommandTimeout = 300;
        //        lDa.SelectCommand = lCmd;
        //        lDs = new DataSet();
        //        lDa.Fill(lDs);
        //        if (lDs.Tables[0].Rows.Count > 0)
        //        {
        //            int lFound = 0;
        //            for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
        //            {
        //                lFound = 0;
        //                var lWBS2t = lDs.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
        //                var lWBS3t = lDs.Tables[0].Rows[i].ItemArray[1].ToString().Trim();

        //                lFound = 0;
        //                if (lWBS2.Count > 0)
        //                {
        //                    for (int j = 0; j < lWBS2.Count; j++)
        //                    {
        //                        if (lWBS2t == lWBS2[j])
        //                        {
        //                            lFound = 1;
        //                            break;
        //                        }
        //                    }
        //                }
        //                if (lFound == 0)
        //                {
        //                    lWBS2.Add(lWBS2t);

        //                    //get List of WBS2
        //                    if (newOrder.WBS2.Exists(x => x.Equals(lWBS2t)))
        //                    {
        //                        lWBS2F.Add(lWBS2t);
        //                    }

        //                }

        //            }

        //            //reverse the sorting

        //            if (lWBS2.Count() > 1)
        //            {
        //                var lWBS2a = lWBS2;
        //                lWBS2 = new List<string>();

        //                for (int i = lWBS2a.Count() - 1; i >= 0; i--)
        //                {
        //                    lWBS2.Add(lWBS2a[i]);
        //                }

        //            }

        //            // get WBS3
        //            for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
        //            {
        //                lFound = 0;
        //                var lWBS2t = lDs.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
        //                var lWBS3t = lDs.Tables[0].Rows[i].ItemArray[1].ToString().Trim();

        //                if (lWBS2F.Exists(x => x.Equals(lWBS2t)))
        //                {
        //                    lFound = 0;
        //                    if (lWBS3.Count > 0)
        //                    {
        //                        for (int m = 0; m < lWBS3.Count; m++)
        //                        {
        //                            if (lWBS3[m] == lWBS3t)
        //                            {
        //                                lFound = 1;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    if (lFound == 0)
        //                    {
        //                        lWBS3.Add(lWBS3t);
        //                    }
        //                }
        //            }


        //        }
        //        lProcess.CloseNDSConnection(ref cnNDS);
        //    }
        //    lDa = null;
        //    lCmd = null;
        //    lDs = null;
        //    cnNDS = null;



        //    var lReturn = new
        //    {
        //        WBS2 = lWBS2,
        //        WBS3 = lWBS3
        //    };

        //    return Ok(lReturn);
        //}

        //[HttpGet]
        /////   [ValidateAntiForgeryHeader]
        //[Route("/getWBS3Multiple")]
        //public ActionResult getWBS3Multiple([FromBody] NewOrder newOrder)
        //{
        //    UserAccessController lUa = new UserAccessController();
        //    var lUserType = lUa.getUserType(User.Identity.GetUserName());
        //    lUa = null;

        //    var lWBS3 = new List<string>();

        //    var lDa = new SqlDataAdapter();
        //    var lCmd = new SqlCommand();
        //    var lDs = new DataSet();
        //    var cnNDS = new SqlConnection();

        //    var lSQL = "";

        //    var lProcess = new ProcessController();
        //    lProcess.OpenNDSConnection(ref cnNDS);
        //    if (cnNDS.State == ConnectionState.Open)
        //    {
        //        string lCond = "";
        //        if (newOrder.WBS1 != null && newOrder.WBS1.Count > 0 && newOrder.WBS2 != null && newOrder.WBS2.Count > 0)
        //        {
        //            for (int i = 0; i < newOrder.WBS1.Count; i++)
        //            {
        //                string lCondWBS2 = "";
        //                for (int j = 0; j < newOrder.WBS2.Count; j++)
        //                {
        //                    lCondWBS2 = lCondWBS2 + " OR vchWBS2 = '" + newOrder.WBS2[j] + "' ";
        //                }
        //                lCondWBS2 = lCondWBS2.Substring(3);

        //                lCond = lCond + " OR (vchWBS1 = '" + newOrder.WBS1[i] + "' AND ( " + lCondWBS2 + " )) ";

        //            }
        //            lCond = lCond.Substring(3);
        //        }
        //        else
        //        {
        //            lCond = lCond + " vchWBS1 = '' AND vchWBS2 = '' ";
        //        }

        //        if (lUserType == "TE" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
        //        {
        //            lSQL = "SELECT " +
        //            "rtrim(ltrim(vchWBS3)) as WBS3  " +
        //            "FROM dbo.WBSElements E, dbo.WBS W, " +
        //            "dbo.ProjectMaster P, dbo.WBSElementsDetails D, " +
        //            "dbo.ProductTypeMaster T, dbo.StructureElementMaster S, " +
        //            "dbo.ContractMaster C " +
        //            "WHERE W.intWBSId = E.intWBSId " +
        //            "AND P.intProjectId = W.intProjectId " +
        //            "AND P.intContractID = C.intContractID " +
        //            "AND E.intWBSElementId = D.intWBSElementId " +
        //            "AND D.sitProductTypeID = T.sitProductTypeID " +
        //            "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
        //            "AND vchProjectCode = '" + newOrder.ProjectCode + "' " +
        //            "AND (vchProductType = 'CAB' " +
        //            "OR ((vchProductType = 'MSH' " +
        //            "OR vchProductType = 'CAR' " +
        //            "OR vchProductType = 'BPC' " +
        //            "OR vchProductType = 'PRC') " +
        //            "AND EXISTS(SELECT H.intPostHeaderid " +
        //            "FROM dbo.BBSPostHeader H " +
        //            "WHERE intWBSElementId = E.intWBSElementId " +
        //            "AND NOT EXISTS(SELECT R.intPostHeaderid " +
        //            "FROM dbo.BBSReleaseDetails R " +
        //            "WHERE R.intPostHeaderid = H.intPostHeaderid " +
        //            "AND R.tntStatusId = 12) ) ) ) " +
        //            "AND ( " + lCond + " ) " +
        //            "AND E.tntStatusId = 1 " +
        //            "AND D.intConfirm = 1 " +
        //            "GROUP BY " +
        //            "vchWBS3  " +
        //            "ORDER BY " +
        //            "vchWBS3 ";
        //        }
        //        else
        //        {
        //            lSQL = "SELECT " +
        //            "rtrim(ltrim(vchWBS3)) as WBS3  " +
        //            "FROM dbo.WBSElements E, dbo.WBS W, " +
        //            "dbo.ProjectMaster P, dbo.WBSElementsDetails D, " +
        //            "dbo.ProductTypeMaster T, dbo.StructureElementMaster S, " +
        //            "dbo.ContractMaster C " +
        //            "WHERE W.intWBSId = E.intWBSId " +
        //            "AND P.intProjectId = W.intProjectId " +
        //            "AND P.intContractID = C.intContractID " +
        //            "AND E.intWBSElementId = D.intWBSElementId " +
        //            "AND D.sitProductTypeID = T.sitProductTypeID " +
        //            "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
        //            "AND vchProjectCode = '" + newOrder.ProjectCode + "' " +
        //            "AND (vchProductType = 'CAB' " +
        //            "OR ((vchProductType = 'MSH' " +
        //            "OR vchProductType = 'CAR' " +
        //            "OR vchProductType = 'PRC') " +
        //            "AND EXISTS(SELECT H.intPostHeaderid " +
        //            "FROM dbo.BBSPostHeader H " +
        //            "WHERE intWBSElementId = E.intWBSElementId " +
        //            "AND NOT EXISTS(SELECT R.intPostHeaderid " +
        //            "FROM dbo.BBSReleaseDetails R " +
        //            "WHERE R.intPostHeaderid = H.intPostHeaderid " +
        //            "AND R.tntStatusId = 12) ) ) ) " +
        //            "AND ( " + lCond + " ) " +
        //            "AND E.tntStatusId = 1 " +
        //            "AND D.intConfirm = 1 " +
        //            "GROUP BY " +
        //            "vchWBS3  " +
        //            "ORDER BY " +
        //            "vchWBS3 ";

        //        }

        //        lCmd.CommandType = CommandType.Text;
        //        lCmd.CommandText = lSQL;
        //        lCmd.Connection = cnNDS;
        //        lCmd.CommandTimeout = 300;
        //        lDa.SelectCommand = lCmd;
        //        lDs = new DataSet();
        //        lDa.Fill(lDs);
        //        if (lDs.Tables[0].Rows.Count > 0)
        //        {
        //            for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
        //            {
        //                var lWBS3t = lDs.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
        //                lWBS3.Add(lWBS3t);
        //            }
        //        }
        //        lProcess.CloseNDSConnection(ref cnNDS);
        //    }
        //    lDa = null;
        //    lCmd = null;
        //    lDs = null;
        //    cnNDS = null;

        //    //var content = extractWBS3(ProjectCode, WBS1, WBS2);

        //    return Ok(lWBS3);
        //}

        [HttpPost]
        ///    [ValidateAntiForgeryHeader]
        [Route("/getScheduledData")]
        public async Task<ActionResult> getScheduledData([FromBody] NewOrder newOrder)
        {
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            string lSQL = "";

            var lReturn = (new[]{ new
            {
                SSNNo = 0,
                PostHeaderID = 0,
                WBS1 = "",
                WBS2 = "",
                WBS3 = "",
                BBSDesc = "",
                StructEle = "",
                ProductType = "",
                TotalPCs = "",
                TotalWeight = "",
                PostedBy = "",
                PostedDate = "",
                Status = ""
            }}).ToList();

            lReturn.RemoveAt(0);

            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                string lCond = "";
                if (newOrder.WBS1 != null && newOrder.WBS2 != null && newOrder.WBS3 != null && newOrder.WBS1.Count > 0 && newOrder.WBS2.Count > 0 && newOrder.WBS3.Count > 0)
                {
                    for (int i = 0; i < newOrder.WBS1.Count; i++)
                    {
                        string lCondWBS2 = "";
                        for (int j = 0; j < newOrder.WBS2.Count; j++)
                        {
                            string lCondWBS3 = "";
                            for (int k = 0; k < newOrder.WBS3.Count; k++)
                            {
                                lCondWBS3 = lCondWBS3 + " OR vchWBS3 = '" + newOrder.WBS3[k] + "' ";

                            }
                            lCondWBS3 = lCondWBS3.Substring(3);

                            lCondWBS2 = lCondWBS2 + " OR (vchWBS2 = '" + newOrder.WBS2[j] + "' AND ( " + lCondWBS3 + " )) ";
                        }
                        lCondWBS2 = lCondWBS2.Substring(3);

                        lCond = lCond + " OR (vchWBS1 = '" + newOrder.WBS1[i] + "' AND ( " + lCondWBS2 + " )) ";
                    }
                    lCond = lCond.Substring(3);
                }
                else
                {
                    lCond = " vchWBS1 = '' AND vchWBS2 = '' AND vchWBS3 = '' ";
                }
                lSQL = "SELECT H.intPostHeaderId, S.vchStructureElementType, T.vchProductType, " +
                "isNULl(H.intPostedQty, 0) + isNULL(intPostedCappingQty, 0) + isNULL(intPostedCLinkQty, 0), " +
                "isNULL(numPostedWeight, 0) + isNULL(numPostedCappingWeight, 0) + isNULL(numPostedClinkWeight, 0), " +
                "(SELECT vchUserName FROM dbo.NDSUsers WHERE intUserId = H.intPostedBy), " +
                "H.datPostedDate, " +
                "(SELECT SUM(tntGroupQty) FROM dbo.PostGroupMarkingDetails where intPostHeaderId = H.intPostHeaderId), " +
                "H.tntStatusId, " +
                "W.vchWBS1, W.vchWBS2, W.vchWBS3, H.BBS_DESC " +
                "FROM dbo.BBSPostHeader H, dbo.WBSElements W, dbo.SAPProjectMaster P, dbo.StructureElementMaster S, dbo.ProductTypeMaster T " +
                "WHERE P.intProjectId = H.intProjectId " +
                "AND H.intWBSElementId = W.intWBSElementId " +
                "AND S.intStructureElementTypeId = H.intStructureElementTypeId " +
                "AND T.sitProductTypeID = H.sitProductTypeId " +
                "AND vchProjectCode = '" + newOrder.ProjectCode + "' " +
                //"AND H.tntStatusId = 3 " +
                "AND NOT EXISTS (SELECT intReleaseId FROM dbo.BBSReleaseDetails WHERE intPostHeaderid = H.intPostHeaderid AND tntStatusId = 12) " +
                "AND ( " + lCond + " ) " +
                "AND W.tntStatusId = 1 " +
                "AND T.vchProductType <> 'CAB' " +
                "AND NOT EXISTS (SELECT SE.OrderNumber FROM dbo.OESProjOrdersSE SE, dbo.OESProjOrder SM " +
                "WHERE SE.OrderNumber = SM.OrderNumber AND SM.OrderStatus <> 'Deleted' " +
                "AND SE.OrderStatus <> '' AND SE.OrderStatus <> 'Cancelled' AND SE.OrderStatus <> 'Deleted' " +
                "AND SE.OrderStatus <> 'New' AND SE.OrderStatus <> 'Created' " +
                "AND SE.PostHeaderID = H.intPostHeaderid ) " +
                "GROUP BY H.intPostHeaderId, S.vchStructureElementType, T.vchProductType, " +
                "H.intPostedQty, intPostedCappingQty, intPostedCLinkQty, " +
                "numPostedWeight, numPostedCappingWeight, numPostedClinkWeight, " +
                "H.intPostedBy, " +
                "H.datPostedDate, " +
                "H.tntStatusId, " +
                "W.vchWBS1, " +
                "W.vchWBS2, " +
                "W.vchWBS3, H.BBS_DESC " +
                "ORDER BY W.vchWBS1, " +
                "(case when PATINDEX('B[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                "PATINDEX('%[^0-9]%', substring(vchWBS2 + 'z', (PATINDEX('%[0-9]%', vchWBS2)), " +
                "len(vchWBS2)) + 'z') ) as int)    " +
                "else (case when PATINDEX('FDN[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
                "len(vchWBS2)) + 'z') ) as int) else  " +
                "case when(PATINDEX('B%', vchWBS2) > 0 OR PATINDEX('FDN%', vchWBS2) > 0) then 98  else 99 end " +
                "end) end), " +
                " " +
                "(case when PATINDEX('[^0-9]%',vchWBS2) > 0 then vchWBS2 " +
                "else '' end), " +
                " " +
                "(CASE WHEN PATINDEX('%[0-9]%',vchWBS2) > 0 THEN cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2) + 1), " +
                "len(vchWBS2)) + 'z') ) as int) " +
                "ELSE " +
                "cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), len(vchWBS2)), " +
                "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
                "len(vchWBS2)) + 'z') - 1) as int) " +
                "END),  " +
                "W.vchWBS3, " +
                "S.vchStructureElementType, T.vchProductType ";

                lCmd.CommandText = lSQL;
                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = await lCmd.ExecuteReaderAsync();
                if (lRst.HasRows)
                {
                    var lSNo = 0;
                    while (lRst.Read())
                    {
                        lSNo = lSNo + 1;
                        var lStructEle = lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).ToUpper();
                        var lProdType = lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).ToUpper();
                        var lPostedPCs = lRst.GetValue(3) == DBNull.Value ? "0" : lRst.GetInt32(3).ToString();

                        if (lStructEle == "BEAM" && lProdType == "MSH")
                        {
                            lProdType = "STIRRUP-LINK-MESH";
                        }
                        else if (lStructEle == "COLUMN" && lProdType == "MSH")
                        {
                            lProdType = "COLUMN-LINK-MESH";
                        }
                        else if (lStructEle != "BEAM" && lStructEle != "COLUMN" && lProdType == "MSH")
                        {
                            lProdType = "CUT-TO-SIZE-MESH";
                        }

                        if (lProdType == "PRC")
                        {
                            lProdType = "PRE-CAGE";
                            lPostedPCs = lRst.GetValue(7) == DBNull.Value ? "0" : lRst.GetInt32(7).ToString();
                        }

                        //Core Cage Product Type added at line 587
                        //else if (lStructEle == "COLUMN" && lProdType == "CAR")
                        else if (lStructEle == "COLUMN" && (lProdType == "CAR") || (lProdType == "CORE"))
                        {
                            lProdType = "CORE-CAGE";
                            lPostedPCs = lRst.GetValue(7) == DBNull.Value ? "0" : lRst.GetInt32(7).ToString();
                        }
                        else if (lStructEle != "COLUMN" && lProdType == "CAR")
                        {
                            lProdType = "CARPET";
                        }

                        var lStatus = "Scheduled";
                        if ((lRst.GetValue(8) == DBNull.Value ? 0 : lRst.GetByte(8)) != 3)
                        {
                            lStatus = "In Progress";
                        }

                        var lWBS1 = lRst.GetValue(9) == DBNull.Value ? "" : lRst.GetString(9).ToUpper().Trim();
                        var lWBS2 = lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).ToUpper().Trim();
                        var lWBS3 = lRst.GetValue(11) == DBNull.Value ? "" : lRst.GetString(11).ToUpper().Trim();
                        var lBBSDesc = lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetString(12).ToUpper().Trim();

                        lReturn.Add(new
                        {
                            SSNNo = lSNo,
                            PostHeaderID = lRst.GetValue(0) == DBNull.Value ? 0 : lRst.GetInt32(0),
                            WBS1 = lWBS1,
                            WBS2 = lWBS2,
                            WBS3 = lWBS3,
                            BBSDesc = lBBSDesc,
                            StructEle = lStructEle,
                            ProductType = lProdType,
                            TotalPCs = lPostedPCs,
                            TotalWeight = lRst.GetValue(4) == DBNull.Value ? "0.000" : lRst.GetDecimal(4).ToString("###,###,##0.000;(###,##0.000); "),
                            PostedBy = lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5),
                            PostedDate = lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetDateTime(6).ToString("yyyy-MM-dd"),
                            Status = lStatus
                        });
                    }
                }

                lProcessObj.CloseNDSConnection(ref lNDSCon);
            }
            lProcessObj = null;

            return Ok(lReturn);
        }


        [HttpPost]
        [Route("/AddToCart")]
        public int AddToCart([FromBody] AddToCart addToCart)
        {
            int lReturnOrderNo = 0;
            int lReturnRefNo = 0;
            string lErrorMsg = "";
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;
            string lSQL = "";

            string lCustomerCode = addToCart.pCustomerCode;
            string lProjectCode = addToCart.pProjectCode;
            int lOrderNumber = 0;
            string lStruEle = addToCart.pStructureElement;
            string lProductType = addToCart.pProductType;
            string lScheduledProd = addToCart.pScheduledProd == null ? "N" : addToCart.pScheduledProd.Trim();

            string lCustomerVer = "";
            string lProjectVer = "";
            int lOrderVer = 0;
            string lStruEleVer = "";
            string lWBSVer = "";

            if (addToCart.pOrderNo > 0)
            {
                lOrderNumber = addToCart.pOrderNo;
            }

            lReturnOrderNo = lOrderNumber;

            if (lCustomerCode != null && lProjectCode != null && lCustomerCode != "" && lProjectCode != "")
            {
                lCustomerCode = lCustomerCode.Trim();
                lProjectCode = lProjectCode.Trim();

                if (lCustomerCode.Length > 0 && lCustomerCode.Length > 0)
                {
                    try
                    {
                        var lProcessObj = new ProcessController();
                        if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                        {
                            if (lOrderNumber > 0)
                            {
                                var lHeader = db.OrderProject.Find(lOrderNumber);

                                if (lHeader.OrderStatus == null || lHeader.OrderStatus == ""
                                    || lHeader.OrderStatus == "New" || lHeader.OrderStatus == "Created"
                                    || lHeader.OrderStatus == "Reserved" || lHeader.OrderStatus == "Created*"
                                    || lHeader.OrderStatus == "Sent")
                                {
                                    // Check order number
                                    lSQL =
                                    "SELECT CustomerCode, ProjectCode, OrderJobID, OrderReferenceNo " +
                                    "FROM dbo.OESProjOrder " +
                                    "WHERE OrderNumber = " + lOrderNumber.ToString() + " ";

                                    lCmd.CommandText = lSQL;
                                    lCmd.Connection = lNDSCon;
                                    lCmd.CommandTimeout = 300;
                                    lRst = lCmd.ExecuteReader();
                                    if (lRst.HasRows)
                                    {
                                        if (lRst.Read())
                                        {
                                            lCustomerVer = lRst.GetString(0).Trim();
                                            lProjectVer = lRst.GetString(1).Trim();
                                            lOrderVer = lRst.GetInt32(2);
                                            lReturnRefNo = lRst.GetInt32(3);
                                        }
                                    }
                                    lRst.Close();
                                    if (lCustomerVer != lCustomerCode || lProjectVer != lProjectCode)
                                    {
                                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                                        lReturnOrderNo = -1;
                                        lErrorMsg = "Invalid order number.";
                                        return lReturnOrderNo;
                                    }
                                    else
                                    {
                                        lReturnOrderNo = lOrderNumber;
                                    }

                                    //update WBS
                                    lSQL =
                                    "UPDATE dbo.OESProjOrder " +
                                    "SET OrderType = '" + addToCart.pOrderType + "' " +
                                    ", WBS1 = '" + addToCart.pWBS1 + "' " +
                                    ", WBS2 = '" + addToCart.pWBS2 + "' " +
                                    ", WBS3 = '" + addToCart.pWBS3 + "' " +
                                    ", UpdateDate = getDate() " +
                                    ", UpdateBy = '" + addToCart.UpdateBy + "' " +
                                    "WHERE OrderNumber = " + lOrderNumber.ToString() + " ";

                                    lCmd.CommandText = lSQL;
                                    lCmd.Connection = lNDSCon;
                                    lCmd.CommandTimeout = 300;
                                    lCmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // generate order Number
                                int lJobID = 0;
                                int lRefNo = addToCart.pRefNo;
                                if (lRefNo == 0)
                                {
                                    //lRefNo = OrderReferenceNumber();
                                    //lRefNo = lRefNo + 1;

                                    String tempRef = lProcessObj.GetDocNo("NSH", "ORN");
                                    lRefNo = int.Parse(tempRef);
                                }

                                lSQL =
                                "SELECT isNULL(MAX(OrderJobID),0) FROM dbo.OESProjOrder " +
                                "WHERE CustomerCode  = '" + lCustomerCode + "' " +
                                "AND  ProjectCode = '" + lProjectCode + "' ";
                                lCmd.CommandText = lSQL;
                                lCmd.Connection = lNDSCon;
                                lCmd.CommandTimeout = 300;
                                lRst = lCmd.ExecuteReader();
                                if (lRst.HasRows)
                                {
                                    if (lRst.Read())
                                    {
                                        lJobID = lRst.GetInt32(0);
                                    }
                                }
                                lRst.Close();

                                lJobID = lJobID + 1;
                                // lRefNo = lRefNo + 1;


                                //lReturnRefNo = lRefNo;

                                lSQL =
                                "INSERT INTO dbo.OESProjOrder " +
                                "(CustomerCode " +
                                ", ProjectCode " +
                                ", OrderJobID " +
                                ", OrderType " +
                                ", WBS1 " +
                                ", WBS2 " +
                                ", WBS3 " +
                                ", OrderStatus " +
                                ", OrderSource " +
                                ", OrderShared " +
                                ", UpdateDate " +
                                ", UpdateBy " +
                                ", OrderReferenceNo) " +
                                "VALUES " +
                                "('" + lCustomerCode + "' " +
                                ",'" + lProjectCode + "' " +
                                "," + lJobID.ToString() + " " +
                                ",'" + addToCart.pOrderType + "' " +
                                ",'" + addToCart.pWBS1 + "' " +
                                ",'" + addToCart.pWBS2 + "' " +
                                ",'" + addToCart.pWBS3 + "' " +
                                ",'Created' " +
                                ",'UX' " +
                                ",0 " +
                                ",getDate() " +
                                ",'" + addToCart.UpdateBy + "' " +
                                ",'" + lRefNo + "') ";
                                lCmd.CommandText = lSQL;
                                lCmd.Connection = lNDSCon;
                                lCmd.CommandTimeout = 300;
                                lCmd.ExecuteNonQuery();

                                lSQL =
                                "SELECT OrderNumber  " +
                                "FROM dbo.OESProjOrder " +
                                "WHERE CustomerCode = '" + lCustomerCode + "' " +
                                "AND ProjectCode = '" + lProjectCode + "' " +
                                "AND OrderJobID = " + lJobID.ToString() + " ";

                                lCmd.CommandText = lSQL;
                                lCmd.Connection = lNDSCon;
                                lCmd.CommandTimeout = 300;
                                lRst = lCmd.ExecuteReader();
                                if (lRst.HasRows)
                                {
                                    if (lRst.Read())
                                    {
                                        lReturnOrderNo = lRst.GetInt32(0);
                                        lOrderNumber = lReturnOrderNo;
                                    }
                                }
                                lRst.Close();
                            }

                            // structure element process
                            if (lReturnOrderNo >= 0)
                            {
                                // check Structure Element
                                int lFound = 0;
                                int lCABJobID = 0;
                                int lMESHJobID = 0;
                                int lBPCJobID = 0;
                                int lCageJobID = 0;
                                int lCarpetJobID = 0;
                                int lStdBarsJobID = 0;
                                int lStdMESHJobID = 0;
                                int lCoilProdJobID = 0;
                                int lPostHeaderID = 0;

                                lSQL =
                                "SELECT CABJobID, MESHJobID, BPCJobID, " +
                                "CageJobID, CarpetJobID, " +
                                "StdBarsJobID, StdMESHJobID, CoilProdJobID, " +
                                "isNULL(PostHeaderID, 0) " +
                                "FROM dbo.OESProjOrdersSE " +
                                "WHERE OrderNumber = " + lOrderNumber.ToString() + " " +
                                "AND StructureElement = '" + lStruEle + "' " +
                                "AND ProductType = '" + lProductType + "' " +
                                "AND ScheduledProd = '" + lScheduledProd + "' ";

                                lCmd.CommandText = lSQL;
                                lCmd.Connection = lNDSCon;
                                lCmd.CommandTimeout = 300;
                                lRst = lCmd.ExecuteReader();
                                if (lRst.HasRows)
                                {
                                    if (lRst.Read())
                                    {
                                        lFound = 1;
                                        lCABJobID = lRst.GetInt32(0);
                                        lMESHJobID = lRst.GetInt32(1);
                                        lBPCJobID = lRst.GetInt32(2);
                                        lCageJobID = lRst.GetInt32(3);
                                        lCarpetJobID = lRst.GetInt32(4);
                                        lStdBarsJobID = lRst.GetInt32(5);
                                        lStdMESHJobID = lRst.GetInt32(6);
                                        lCoilProdJobID = lRst.GetInt32(7);
                                        lPostHeaderID = lRst.GetInt32(8);
                                    }
                                }
                                lRst.Close();

                                if (lFound == 0)
                                {
                                    lSQL =
                                    "INSERT INTO dbo.OESProjOrdersSE " +
                                    "(OrderNumber " +
                                    ", StructureElement " +
                                    ", ProductType " +
                                    ", ScheduledProd" +
                                    ", CABJobID " +
                                    ", MESHJobID " +
                                    ", BPCJobID " +
                                    ", CageJobID " +
                                    ", CarpetJobID " +
                                    ", StdBarsJobID " +
                                    ", StdMESHJobID " +
                                    ", CoilProdJobID " +
                                    ", PostHeaderID " +
                                    ", OrderStatus " +
                                    ", TotalWeight " +
                                    ", UpdateDate " +
                                    ", UpdateBy) " +
                                    "VALUES " +
                                    "(" + lOrderNumber.ToString() + " " +
                                    ",'" + addToCart.pStructureElement.Trim() + "' " +
                                    ",'" + addToCart.pProductType.Trim() + "' " +
                                    ",'" + addToCart.pScheduledProd.Trim() + "' " +
                                    ",0 " +
                                    ",0 " +
                                    ",0 " +
                                    ",0 " +
                                    ",0 " +
                                    ",0 " +
                                    ",0 " +
                                    ",0 " +
                                    "," + addToCart.pPostID + " " +
                                    ",'Created' " +
                                    ",0 " +
                                    ",getDate() " +
                                    ",'" + addToCart.UpdateBy + "') ";

                                    lCmd.CommandText = lSQL;
                                    lCmd.Connection = lNDSCon;
                                    lCmd.CommandTimeout = 300;
                                    lCmd.ExecuteNonQuery();
                                }
                                if (addToCart.pScheduledProd != "Y")
                                {
                                    if (lProductType == "CAB")
                                    {
                                        if (lCABJobID == 0)
                                        {
                                            lCABJobID = createCABJobAdvice(lCustomerCode, lProjectCode, lStruEle, addToCart.pPONo, addToCart.UpdateBy);

                                            if (lCABJobID > 0)
                                            {
                                                lSQL =
                                                "UPDATE dbo.OESProjOrdersSE " +
                                                "SET CABJobID = " + lCABJobID + ", " +
                                                "UpdateDate = getDate(), " +
                                                "UpdateBy = '" + addToCart.UpdateBy + "' " +
                                                "WHERE OrderNumber = " + lOrderNumber.ToString() + " " +
                                                "AND StructureElement = '" + addToCart.pStructureElement + "' " +
                                                "AND ProductType = '" + addToCart.pProductType + "' " +
                                                "AND ScheduledProd = '" + addToCart.pScheduledProd.Trim() + "' ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lNDSCon;
                                                lCmd.CommandTimeout = 300;
                                                lCmd.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                lProcessObj.CloseNDSConnection(ref lNDSCon);
                                                lReturnOrderNo = -1;
                                                lErrorMsg = "Error on CAB job ID generation.";
                                            }
                                        }
                                    }
                                    else if (lProductType == "STANDARD-BAR")
                                    {
                                        if (lStdBarsJobID == 0)
                                        {
                                            lStdBarsJobID = createStdProductJobAdvice(lCustomerCode, lProjectCode, addToCart.UpdateBy);

                                            if (lStdBarsJobID > 0)
                                            {
                                                lSQL =
                                                "UPDATE dbo.OESProjOrdersSE " +
                                                "SET StdBarsJobID = " + lStdBarsJobID + ", " +
                                                "UpdateDate = getDate(), " +
                                                "UpdateBy = '" + addToCart.UpdateBy + "' " +
                                                "WHERE OrderNumber = " + lOrderNumber.ToString() + " " +
                                                "AND StructureElement = '" + addToCart.pStructureElement + "' " +
                                                "AND ProductType = '" + addToCart.pProductType + "' " +
                                                "AND ScheduledProd = '" + addToCart.pScheduledProd.Trim() + "' ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lNDSCon;
                                                lCmd.CommandTimeout = 300;
                                                lCmd.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                lProcessObj.CloseNDSConnection(ref lNDSCon);
                                                lReturnOrderNo = -1;
                                                lErrorMsg = "Error on CAB job ID generation.";
                                            }
                                        }
                                    }
                                    else if (lProductType == "CUT-TO-SIZE-MESH" || lProductType == "COLUMN-LINK-MESH" || lProductType == "STIRRUP-LINK-MESH")
                                    {
                                        if (lMESHJobID == 0)
                                        {
                                            lMESHJobID = createMeshJobAdvice(lCustomerCode, lProjectCode, addToCart.UpdateBy);

                                            if (lMESHJobID > 0)
                                            {
                                                lSQL =
                                                "UPDATE dbo.OESProjOrdersSE " +
                                                "SET MESHJobID = " + lMESHJobID + ", " +
                                                "UpdateDate = getDate(), " +
                                                "UpdateBy = '" + addToCart.UpdateBy + "' " +
                                                "WHERE OrderNumber = " + lOrderNumber.ToString() + " " +
                                                "AND StructureElement = '" + addToCart.pStructureElement + "' " +
                                                "AND ProductType = '" + addToCart.pProductType + "' " +
                                                "AND ScheduledProd = '" + addToCart.pScheduledProd.Trim() + "' ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lNDSCon;
                                                lCmd.CommandTimeout = 300;
                                                lCmd.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                lProcessObj.CloseNDSConnection(ref lNDSCon);
                                                lReturnOrderNo = -1;
                                                lErrorMsg = "Error on MESH job ID generation.";
                                            }
                                        }
                                    }
                                    else if (lProductType == "BPC")
                                    {
                                        if (lBPCJobID == 0)
                                        {
                                            lBPCJobID = createBPCJobAdvice(lCustomerCode, lProjectCode, addToCart.UpdateBy);

                                            if (lBPCJobID > 0)
                                            {
                                                lSQL =
                                                "UPDATE dbo.OESProjOrdersSE " +
                                                "SET BPCJobID = " + lBPCJobID + ", " +
                                                "UpdateDate = getDate(), " +
                                                "UpdateBy = '" + addToCart.UpdateBy + "' " +
                                                "WHERE OrderNumber = " + lOrderNumber.ToString() + " " +
                                                "AND StructureElement = '" + addToCart.pStructureElement + "' " +
                                                "AND ProductType = '" + addToCart.pProductType + "' " +
                                                "AND ScheduledProd = '" + addToCart.pScheduledProd.Trim() + "' ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lNDSCon;
                                                lCmd.CommandTimeout = 300;
                                                lCmd.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                lProcessObj.CloseNDSConnection(ref lNDSCon);
                                                lReturnOrderNo = -1;
                                                lErrorMsg = "Error on CAB job ID generation.";
                                            }
                                        }
                                    }
                                    else if (lProductType == "PRE-CAGE" || lProductType == "CORE-CAGE")
                                    {
                                        if (lCageJobID == 0)
                                        {
                                            lCageJobID = createCageJobAdvice(lCustomerCode, lProjectCode, addToCart.UpdateBy);

                                            if (lCageJobID > 0)
                                            {
                                                lSQL =
                                                "UPDATE dbo.OESProjOrdersSE " +
                                                "SET CageJobID = " + lCageJobID + ", " +
                                                "UpdateDate = getDate(), " +
                                                "UpdateBy = '" + addToCart.UpdateBy + "' " +
                                                "WHERE OrderNumber = " + lOrderNumber.ToString() + " " +
                                                "AND StructureElement = '" + addToCart.pStructureElement + "' " +
                                                "AND ProductType = '" + addToCart.pProductType + "' " +
                                                "AND ScheduledProd = '" + addToCart.pScheduledProd.Trim() + "' ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lNDSCon;
                                                lCmd.CommandTimeout = 300;
                                                lCmd.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                lProcessObj.CloseNDSConnection(ref lNDSCon);
                                                lReturnOrderNo = -1;
                                                lErrorMsg = "Error on MESH job ID generation.";
                                            }
                                        }
                                    }
                                    else if (lProductType == "STANDARD-MESH")
                                    {
                                        if (lStdMESHJobID == 0)
                                        {
                                            lStdMESHJobID = createStdProductJobAdvice(lCustomerCode, lProjectCode, addToCart.UpdateBy);

                                            if (lStdMESHJobID > 0)
                                            {
                                                lSQL =
                                                "UPDATE dbo.OESProjOrdersSE " +
                                                "SET StdMESHJobID = " + lStdMESHJobID + ", " +
                                                "UpdateDate = getDate(), " +
                                                "UpdateBy = '" + addToCart.UpdateBy + "' " +
                                                "WHERE OrderNumber = " + lOrderNumber.ToString() + " " +
                                                "AND StructureElement = '" + addToCart.pStructureElement + "' " +
                                                "AND ProductType = '" + addToCart.pProductType + "' " +
                                                "AND ScheduledProd = '" + addToCart.pScheduledProd.Trim() + "' ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lNDSCon;
                                                lCmd.CommandTimeout = 300;
                                                lCmd.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                lProcessObj.CloseNDSConnection(ref lNDSCon);
                                                lReturnOrderNo = -1;
                                                lErrorMsg = "Error on Standard Product job ID generation.";
                                            }
                                        }
                                    }
                                    else if (lProductType == "COIL")
                                    {
                                        if (lCoilProdJobID == 0)
                                        {
                                            lCoilProdJobID = createStdProductJobAdvice(lCustomerCode, lProjectCode, addToCart.UpdateBy);

                                            if (lCoilProdJobID > 0)
                                            {
                                                lSQL =
                                                "UPDATE dbo.OESProjOrdersSE " +
                                                "SET CoilProdJobID = " + lCoilProdJobID + ", " +
                                                "UpdateDate = getDate(), " +
                                                "UpdateBy = '" + addToCart.UpdateBy + "' " +
                                                "WHERE OrderNumber = " + lOrderNumber.ToString() + " " +
                                                "AND StructureElement = '" + addToCart.pStructureElement + "' " +
                                                "AND ProductType = '" + addToCart.pProductType + "' " +
                                                "AND ScheduledProd = '" + addToCart.pScheduledProd.Trim() + "' ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lNDSCon;
                                                lCmd.CommandTimeout = 300;
                                                lCmd.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                lProcessObj.CloseNDSConnection(ref lNDSCon);
                                                lReturnOrderNo = -1;
                                                lErrorMsg = "Error on Standard Product job ID generation.";
                                            }
                                        }

                                    }
                                    else if (lProductType == "COUPLER")
                                    {
                                        if (lCoilProdJobID == 0)
                                        {
                                            lCoilProdJobID = createStdProductJobAdvice(lCustomerCode, lProjectCode, addToCart.UpdateBy);

                                            if (lCoilProdJobID > 0)
                                            {
                                                lSQL =
                                                "UPDATE dbo.OESProjOrdersSE " +
                                                "SET CoilProdJobID = " + lCoilProdJobID + ", " +
                                                "UpdateDate = getDate(), " +
                                                "UpdateBy = '" + addToCart.UpdateBy + "' " +
                                                "WHERE OrderNumber = " + lOrderNumber.ToString() + " " +
                                                "AND StructureElement = '" + addToCart.pStructureElement + "' " +
                                                "AND ProductType = '" + addToCart.pProductType + "' " +
                                                "AND ScheduledProd = '" + addToCart.pScheduledProd.Trim() + "' ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lNDSCon;
                                                lCmd.CommandTimeout = 300;
                                                lCmd.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                lProcessObj.CloseNDSConnection(ref lNDSCon);
                                                lReturnOrderNo = -1;
                                                lErrorMsg = "Error on Standard Product job ID generation.";
                                            }
                                        }

                                    }
                                    else if (lProductType == "CARPET")
                                    {
                                    }
                                }
                            }
                            lProcessObj.CloseNDSConnection(ref lNDSCon);

                        }
                    }
                    catch (Exception ex)
                    {
                        lReturnOrderNo = -1;
                        lErrorMsg = ex.Message;
                    }
                }
                else
                {
                    lErrorMsg = "Invalid customer code or project code";
                }
            }
            else
            {
                lErrorMsg = "Invalid customer code or project code";
            }

            lCmd = null;
            lNDSCon = null;

            return lReturnOrderNo;
        }


        [HttpGet]
        [Route("/createCABJobAdvice/{pCustomerCode}/{pProjectCode}/{pStructureEle}/{pPONo}/{pUserID}")]
        public int createCABJobAdvice(string pCustomerCode, string pProjectCode, string pStructureEle, string pPONo, string pUserID)
        {
            var lProcessObj = new ProcessController();
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            var lErrorMsg = "";
            int lReturn = 0;
            int? lJobID = 0;
            try
            {
                if (pCustomerCode == null)
                {
                    pCustomerCode = "";
                }
                if (pProjectCode == null)
                {
                    pProjectCode = "";
                }
                pCustomerCode = pCustomerCode.Trim();
                pProjectCode = pProjectCode.Trim();
                if (pCustomerCode.Length > 0 && pProjectCode.Length > 0)
                {
                    lJobID = db.JobAdvice.Where(z => z.CustomerCode == pCustomerCode &&
                    z.ProjectCode == pProjectCode).Max(z => (int?)z.JobID);

                    var lJobAdv = new JobAdviceModels();
                    lJobAdv.CustomerCode = pCustomerCode;
                    lJobAdv.ProjectCode = pProjectCode;

                    if (lJobID == null)
                    {
                        lJobAdv.JobID = 1;
                    }
                    else
                    {
                        lJobAdv.JobID = (int)lJobID + 1;
                    }

                    lReturn = (int)lJobAdv.JobID;

                    lJobAdv.PODate = DateTime.Now;
                    lJobAdv.RequiredDate = DateTime.Now.AddDays(5);
                    lJobAdv.TotalCABWeight = 0;
                    lJobAdv.TotalSTDWeight = 0;
                    lJobAdv.TotalWeight = 0;

                    lJobAdv.OrderStatus = "Reserved";
                    lJobAdv.OrderSource = "UX";
                    lJobAdv.DeliveryAddress = "";
                    lJobAdv.Remarks = "";
                    lJobAdv.ProjectStage = "TYP";
                    lJobAdv.WBS1 = "";
                    lJobAdv.WBS2 = "";
                    lJobAdv.WBS3 = "";

                    lJobAdv.Scheduler_Name = "";
                    lJobAdv.Scheduler_HP = "";
                    lJobAdv.Scheduler_Tel = "";
                    lJobAdv.SiteEngr_Name = "";
                    lJobAdv.SiteEngr_HP = "";
                    lJobAdv.SiteEngr_Tel = "";

                    lJobAdv.TransportLimit = "Normal";
                    lJobAdv.TransportMode = "TR40/24";
                    lJobAdv.PONumber = "";
                    lJobAdv.UpdateDate = DateTime.Now;
                    lJobAdv.UpdateBy = pUserID;

                    lJobAdv.CouplerType = "No Coupler";
                    //var lCoupler = getCouplerTypeFromESB(pCustomerCode, pProjectCode);
                    //var lCoupler = getCouplerTypeFromSAP(pCustomerCode, pProjectCode);

                    var oldJobAdvice = db.JobAdvice.Find(lJobAdv.CustomerCode, lJobAdv.ProjectCode, lJobAdv.JobID);
                    if (oldJobAdvice == null)
                    {
                        db.JobAdvice.Add(lJobAdv);
                    }
                    else
                    {
                        db.Entry(oldJobAdvice).CurrentValues.SetValues(lJobAdv);
                    }

                    string lBBSNo = "BBS" + lJobAdv.JobID.ToString();
                    if (pPONo != null && pPONo != "")
                    {
                        lBBSNo = pPONo;
                        if (lBBSNo.Length > 14)
                        {
                            lBBSNo = lBBSNo.Replace("-", "");
                            lBBSNo = lBBSNo.Replace("/", "");
                            lBBSNo = lBBSNo.Replace("&", "");
                        }
                        if (lBBSNo.Length > 14)
                        {
                            lBBSNo = lBBSNo.Substring(lBBSNo.Length - 14);
                        }
                    }

                    var lBBS = new BBSModels();
                    lBBS.UpdateDate = DateTime.Now;
                    lBBS.UpdateBy = pUserID;
                    lBBS.CustomerCode = pCustomerCode;
                    lBBS.ProjectCode = pProjectCode;
                    lBBS.JobID = lJobAdv.JobID;
                    lBBS.BBSCancelledWT = 0;
                    lBBS.BBSCopiedFrom = "";
                    lBBS.BBSDesc = "";
                    lBBS.BBSID = 1;
                    lBBS.BBSNo = lBBSNo;
                    lBBS.BBSNoNDS = "";
                    lBBS.BBSNoNDSCoupler = "";
                    lBBS.BBSOrderCABWT = 0;
                    lBBS.BBSOrderSTDWT = 0;
                    lBBS.BBSSAPSO = "";
                    lBBS.BBSSOR = "";
                    lBBS.BBSSORCoupler = "";
                    lBBS.BBSStrucElem = pStructureEle;
                    lBBS.BBSTotalWT = 0;

                    var oldBBS = db.BBS.Find(pCustomerCode, pProjectCode, lJobAdv.JobID, 1);
                    if (oldBBS == null)
                    {
                        db.BBS.Add(lBBS);
                    }

                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                var lMsg = ex.Message;
                lProcessObj.SaveErrorMsg(ex.Message, ex.StackTrace);
                return -1;
            }
            return lReturn;
        }

        [HttpGet]
        [Route("/createStdProductJobAdvice/{pCustomerCode}/{pProjectCode}")]
        public int createStdProductJobAdvice(string pCustomerCode, string pProjectCode, string UpdateBy)
        {
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            var lErrorMsg = "";
            int lReturn = -1;
            int? lJobID = 0;
            try
            {
                if (pCustomerCode == null)
                {
                    pCustomerCode = "";
                }
                if (pProjectCode == null)
                {
                    pProjectCode = "";
                }
                pCustomerCode = pCustomerCode.Trim();
                pProjectCode = pProjectCode.Trim();
                if (pCustomerCode.Length > 0 && pProjectCode.Length > 0)
                {
                    lJobID = db.StdSheetJobAdvice.Where(z => z.CustomerCode == pCustomerCode &&
                    z.ProjectCode == pProjectCode).Max(z => (int?)z.JobID);

                    var lJobAdv = new StdSheetJobAdviceModels();
                    lJobAdv.CustomerCode = pCustomerCode;
                    lJobAdv.ProjectCode = pProjectCode;

                    if (lJobID == null)
                    {
                        lJobAdv.JobID = 1;
                    }
                    else
                    {
                        lJobAdv.JobID = (int)lJobID + 1;
                    }

                    lReturn = lJobAdv.JobID;

                    lJobAdv.PODate = DateTime.Now;
                    lJobAdv.RequiredDate = DateTime.Now.AddDays(5);
                    lJobAdv.TotalPcs = 0;
                    lJobAdv.TotalWeight = 0;

                    lJobAdv.Transport = "";
                    lJobAdv.OrderStatus = "Reserved";
                    lJobAdv.DeliveryAddress = "";
                    lJobAdv.Remarks = "";

                    lJobAdv.Scheduler_Name = "";
                    lJobAdv.Scheduler_HP = "";
                    lJobAdv.Scheduler_Tel = "";
                    lJobAdv.SiteEngr_Name = "";
                    lJobAdv.SiteEngr_HP = "";
                    lJobAdv.SiteEngr_Tel = "";
                    lJobAdv.DeliveryAddress = "";
                    lJobAdv.PONumber = "";
                    lJobAdv.UpdateDate = DateTime.Now;

                    lJobAdv.UpdateBy = UpdateBy;//User.Identity.GetUserName();

                    var oldJobAdvice = db.StdSheetJobAdvice.Find(lJobAdv.CustomerCode, lJobAdv.ProjectCode, lJobAdv.JobID);
                    if (oldJobAdvice == null)
                    {
                        db.StdSheetJobAdvice.Add(lJobAdv);
                    }
                    else
                    {
                        db.Entry(oldJobAdvice).CurrentValues.SetValues(lJobAdv);
                    }

                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                return -1;
            }
            return lReturn;
        }

        [HttpGet]
        [Route("/createMeshJobAdvice/{pCustomerCode}/{pProjectCode}/{pUserID}")]
        public int createMeshJobAdvice(string pCustomerCode, string pProjectCode, string pUserID)
        {
            int lReturn = -1;
            var lErrorMsg = "";
            int? lJobID = 0;
            try
            {
                if (pCustomerCode == null)
                {
                    pCustomerCode = "";
                }
                if (pProjectCode == null)
                {
                    pProjectCode = "";
                }
                pCustomerCode = pCustomerCode.Trim();
                pProjectCode = pProjectCode.Trim();
                if (pCustomerCode.Length > 0 && pProjectCode.Length > 0)
                {

                    lJobID = db.CTSMESHJobAdvice.Where(z => z.CustomerCode == pCustomerCode &&
                    z.ProjectCode == pProjectCode).Max(z => (int?)z.JobID);

                    var lJobAdv = new CTSMESHJobAdviceModels();
                    lJobAdv.CustomerCode = pCustomerCode;
                    lJobAdv.ProjectCode = pProjectCode;

                    if (lJobID == null)
                    {
                        lJobID = 1;
                    }
                    else
                    {
                        lJobID = lJobID + 1;
                    }

                    lJobAdv.JobID = (int)lJobID;

                    lReturn = lJobAdv.JobID;

                    lJobAdv.PODate = DateTime.Now;
                    lJobAdv.RequiredDate = DateTime.Now.AddDays(5);
                    lJobAdv.TotalPcs = 0;
                    lJobAdv.TotalWeight = 0;

                    lJobAdv.OrderStatus = "Reserved";
                    lJobAdv.OrderSource = "UX";
                    lJobAdv.DeliveryAddress = "";
                    lJobAdv.Remarks = "";

                    lJobAdv.Transport = "HC";
                    lJobAdv.AutoWBS = true;
                    lJobAdv.WBS1 = "";
                    lJobAdv.WBS2 = "";
                    lJobAdv.WBS3 = "";
                    lJobAdv.Model = false;

                    lJobAdv.Scheduler_Name = "";
                    lJobAdv.Scheduler_HP = "";
                    lJobAdv.Scheduler_Tel = "";
                    lJobAdv.SiteEngr_Name = "";
                    lJobAdv.SiteEngr_HP = "";
                    lJobAdv.SiteEngr_Tel = "";


                    lJobAdv.PONumber = "";
                    lJobAdv.OrderStatus = "New";
                    lJobAdv.UpdateDate = DateTime.Now;
                    lJobAdv.UpdateBy = pUserID;



                    var oldJobAdvice = db.CTSMESHJobAdvice.Find(lJobAdv.CustomerCode, lJobAdv.ProjectCode, lJobAdv.JobID);
                    if (oldJobAdvice == null)
                    {
                        db.CTSMESHJobAdvice.Add(lJobAdv);
                    }
                    else
                    {
                        db.Entry(oldJobAdvice).CurrentValues.SetValues(lJobAdv);
                    }

                    var lBBS1 = new CTSMESHBBSModels();
                    lBBS1.UpdateDate = DateTime.Now;
                    lBBS1.UpdateBy = pUserID;
                    lBBS1.CustomerCode = pCustomerCode;
                    lBBS1.ProjectCode = pProjectCode;
                    lBBS1.JobID = lJobAdv.JobID;

                    lBBS1.BBSID = 1;
                    lBBS1.BBSProdCategory = "";
                    lBBS1.BBSStrucElem = "";
                    lBBS1.BBSDesc = "";
                    lBBS1.BBSDrawing = "N";
                    lBBS1.BBSDrawingRev = "";
                    lBBS1.BBSTotalPcs = 0;
                    lBBS1.BBSTotalWT = 0;
                    lBBS1.BBSNDSGroupMark = "";
                    lBBS1.BBSSOR = "";
                    lBBS1.BBSOrder = false;

                    var oldBBS1 = db.CTSMESHBBS.Find(pCustomerCode, pProjectCode, lJobAdv.JobID, 1);
                    if (oldBBS1 == null)
                    {
                        db.CTSMESHBBS.Add(lBBS1);
                    }
                    db.SaveChanges();
                }


            }

            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                lReturn = -1;
            }
            return lReturn;
        }

        [HttpGet]
        [Route("/createBPCJobAdvice/{pCustomerCode}/{pProjectCode}")]
        public int createBPCJobAdvice(string pCustomerCode, string pProjectCode, string UserName)
        {
            int lReturn = -1;
            var lErrorMsg = "";
            int? lJobID = 0;
            try
            {
                if (pCustomerCode == null)
                {
                    pCustomerCode = "";
                }
                if (pProjectCode == null)
                {
                    pProjectCode = "";
                }
                pCustomerCode = pCustomerCode.Trim();
                pProjectCode = pProjectCode.Trim();
                if (pCustomerCode.Length > 0 && pProjectCode.Length > 0)
                {

                    lJobID = db.BPCJobAdvice.Where(z => z.CustomerCode == pCustomerCode &&
                    z.ProjectCode == pProjectCode).Max(z => (int?)z.JobID);

                    var lJobAdv = new BPCJobAdviceModels();
                    lJobAdv.CustomerCode = pCustomerCode;
                    lJobAdv.ProjectCode = pProjectCode;

                    if (lJobID == null)
                    {
                        lJobID = 1;
                    }
                    else
                    {
                        lJobID = lJobID + 1;
                    }

                    lJobAdv.JobID = (int)lJobID;

                    lReturn = lJobAdv.JobID;

                    lJobAdv.Template = false;
                    lJobAdv.PODate = DateTime.Now;
                    lJobAdv.RequiredDate = DateTime.Now.AddDays(10);
                    lJobAdv.TotalPcs = 0;
                    lJobAdv.TotalWeight = 0;

                    lJobAdv.OrderStatus = "Reserved";
                    lJobAdv.OrderSource = "UX";
                    lJobAdv.DeliveryAddress = "";
                    lJobAdv.Remarks = "";

                    lJobAdv.Transport = "TR40/24";

                    lJobAdv.Scheduler_Name = "";
                    lJobAdv.Scheduler_HP = "";
                    lJobAdv.Scheduler_Tel = "";
                    lJobAdv.SiteEngr_Name = "";
                    lJobAdv.SiteEngr_HP = "";
                    lJobAdv.SiteEngr_Tel = "";


                    lJobAdv.PONumber = "";
                    lJobAdv.OrderStatus = "New";
                    lJobAdv.UpdateDate = DateTime.Now;
                    lJobAdv.UpdateBy = UserName;//User.Identity.GetUserName();
                    lJobAdv.underload_ct = 0;
                    lJobAdv.pile_category = "BPC";
                    lJobAdv.cover_to_link = 75;

                    var oldJobAdvice = db.BPCJobAdvice.Find(lJobAdv.CustomerCode, lJobAdv.ProjectCode, false, lJobAdv.JobID);
                    if (oldJobAdvice == null)
                    {
                        db.BPCJobAdvice.Add(lJobAdv);
                    }
                    else
                    {
                        db.Entry(oldJobAdvice).CurrentValues.SetValues(lJobAdv);
                    }

                    db.SaveChanges();
                }


            }

            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                lReturn = -1;
            }
            return lReturn;
        }

        [HttpGet]
        [Route("/createCageJobAdvice/{pCustomerCode}/{pProjectCode}/{pUserID}")]
        public int createCageJobAdvice(string pCustomerCode, string pProjectCode, string pUserID)
        {
            int lReturn = -1;
            var lErrorMsg = "";
            int? lJobID = 0;
            try
            {
                if (pCustomerCode == null)
                {
                    pCustomerCode = "";
                }
                if (pProjectCode == null)
                {
                    pProjectCode = "";
                }
                pCustomerCode = pCustomerCode.Trim();
                pProjectCode = pProjectCode.Trim();
                if (pCustomerCode.Length > 0 && pProjectCode.Length > 0)
                {

                    lJobID = db.PRCJobAdvice.Where(z => z.CustomerCode == pCustomerCode &&
                    z.ProjectCode == pProjectCode).Max(z => (int?)z.JobID);

                    var lJobAdv = new PRCJobAdviceModels();
                    lJobAdv.CustomerCode = pCustomerCode;
                    lJobAdv.ProjectCode = pProjectCode;

                    if (lJobID == null)
                    {
                        lJobID = 1;
                    }
                    else
                    {
                        lJobID = lJobID + 1;
                    }

                    lJobAdv.JobID = (int)lJobID;

                    lReturn = lJobAdv.JobID;

                    lJobAdv.PODate = DateTime.Now;
                    lJobAdv.RequiredDate = DateTime.Now.AddDays(10);
                    lJobAdv.TotalPcs = 0;
                    lJobAdv.TotalWeight = 0;

                    lJobAdv.OrderStatus = "Reserved";
                    lJobAdv.OrderSource = "UX";
                    lJobAdv.DeliveryAddress = "";
                    lJobAdv.Remarks = "";

                    lJobAdv.Transport = "HC";
                    lJobAdv.WBS1 = "";
                    lJobAdv.WBS2 = "";
                    lJobAdv.WBS3 = "";
                    lJobAdv.Model = false;

                    lJobAdv.Scheduler_Name = "";
                    lJobAdv.Scheduler_HP = "";
                    lJobAdv.Scheduler_Tel = "";
                    lJobAdv.SiteEngr_Name = "";
                    lJobAdv.SiteEngr_HP = "";
                    lJobAdv.SiteEngr_Tel = "";


                    lJobAdv.PONumber = "";
                    lJobAdv.OrderStatus = "New";
                    lJobAdv.UpdateDate = DateTime.Now;
                    lJobAdv.UpdateBy = pUserID;

                    var oldJobAdvice = db.PRCJobAdvice.Find(lJobAdv.CustomerCode, lJobAdv.ProjectCode, lJobAdv.JobID);
                    if (oldJobAdvice == null)
                    {
                        db.PRCJobAdvice.Add(lJobAdv);
                    }
                    else
                    {
                        db.Entry(oldJobAdvice).CurrentValues.SetValues(lJobAdv);
                    }

                    db.SaveChanges();
                }


            }

            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                lReturn = -1;
            }
            return lReturn;
        }


        [HttpPost]
        [Route("/createWBS/{pCustomerCode}/{pProjectCode}/{WBS1}/{WBS2}/{WBS3}/{SEBeam}/{SEColumn}/{SEWall}/{SESlab}/{SESlabB}/{SESlabT}" +
            "/{SEDrain}/{SEDwall}/{SEFdn}/{SEPile}/{SEMisc}/{ProdCTSMesh}/{ProdStirrupMesh}/{ProdColumnMesh}/{ProdCAB}/{ProdPreCage}/{ProdBPC}/{ProdCoreCage}/{ProdCarpet}")]
        public ActionResult createWBS(string CustomerCode, string ProjectCode, string WBS1, string WBS2, string WBS3,
        int SEBeam, int SEColumn, int SEWall, int SESlab, int SESlabB, int SESlabT, int SEDrain, int SEDwall, int SEFdn, int SEPile, int SEMisc,
        int ProdCTSMesh, int ProdStirrupMesh, int ProdColumnMesh, int ProdCAB, int ProdPreCage, int ProdBPC, int ProdCoreCage, int ProdCarpet)
        {

            var lErrorMsg = "";
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            string lSQL = "";
            var lReturn = new[] {new {
                StructureElement = "",
                ProductType = "",
                Message = ""
                } }.ToList();

            lReturn.RemoveAt(0);

            try
            {
                var lProces = new ProcessController();
                if (lProces.OpenNDSConnection(ref lNDSCon) != true)
                {
                    //return Json(new { success = false, responseText = "Failed to create WBS: error on open database." }, JsonRequestBehavior.AllowGet);
                    return Ok(lProces);
                }
                if (SEBeam == 1)
                {
                    if (ProdStirrupMesh == 1)
                    {
                        var lStructureEle = "Beam";
                        var lProdType = "MSH";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Beam",
                                ProductType = "Stirrup Link Mesh",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Beam",
                                    ProductType = "Stirrup Link Mesh",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Beam",
                                    ProductType = "Stirrup Link Mesh",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdCAB == 1)
                    {
                        var lStructureEle = "Beam";
                        var lProdType = "CAB";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Beam",
                                ProductType = "CAB",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Beam",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Beam",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdPreCage == 1)
                    {
                        var lStructureEle = "Beam";
                        var lProdType = "PRC";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Beam",
                                ProductType = "Pre-Cage",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Beam",
                                    ProductType = "Pre-Cage",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Beam",
                                    ProductType = "Pre-Cage",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                }
                if (SEColumn == 1)
                {
                    if (ProdColumnMesh == 1)
                    {
                        var lStructureEle = "Column";
                        var lProdType = "MSH";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Column",
                                ProductType = "Column Link Mesh",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Column",
                                    ProductType = "Column Link Mesh",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Column",
                                    ProductType = "Column Link Mesh",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdCAB == 1)
                    {
                        var lStructureEle = "Column";
                        var lProdType = "CAB";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Column",
                                ProductType = "CAB",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Column",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Column",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdPreCage == 1)
                    {
                        var lStructureEle = "Column";
                        var lProdType = "PRC";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Column",
                                ProductType = "Pre-Cage",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Column",
                                    ProductType = "Pre-Cage",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Column",
                                    ProductType = "Pre-Cage",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdCoreCage == 1)
                    {
                        var lStructureEle = "Column";
                        //Core Cage Product Type added at line 1999
                        // var lProdType = "CAR";
                        var lProdType = "CORE";

                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Column",
                                ProductType = "Core-Cage",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Column",
                                    ProductType = "Core-Cage",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Column",
                                    ProductType = "Core-Cage",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                }
                if (SEWall == 1)
                {
                    if (ProdCTSMesh == 1)
                    {
                        var lStructureEle = "Wall";
                        var lProdType = "MSH";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Wall",
                                ProductType = "Cut-To-Size Mesh",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Wall",
                                    ProductType = "Cut-To-Size Mesh",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Wall",
                                    ProductType = "Cut-To-Size Mesh",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdCAB == 1)
                    {
                        var lStructureEle = "Wall";
                        var lProdType = "CAB";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Wall",
                                ProductType = "CAB",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Wall",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Wall",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdPreCage == 1)
                    {
                        var lStructureEle = "Wall";
                        var lProdType = "PRC";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Wall",
                                ProductType = "Pre-Cage",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Wall",
                                    ProductType = "Pre-Cage",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Wall",
                                    ProductType = "Pre-Cage",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdCarpet == 1)
                    {
                        var lStructureEle = "Wall";
                        var lProdType = "CAR";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Wall",
                                ProductType = "Carpet",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Wall",
                                    ProductType = "Carpet",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Wall",
                                    ProductType = "Carpet",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                }
                if (SESlab == 1)
                {
                    if (ProdCTSMesh == 1)
                    {
                        var lStructureEle = "Slab";
                        var lProdType = "MSH";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Slab",
                                ProductType = "Cut-To-Size Mesh",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Slab",
                                    ProductType = "Cut-To-Size Mesh",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Slab",
                                    ProductType = "Cut-To-Size Mesh",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdCAB == 1)
                    {
                        var lStructureEle = "Slab";
                        var lProdType = "CAB";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Slab",
                                ProductType = "CAB",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Slab",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Slab",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdPreCage == 1)
                    {
                        var lStructureEle = "Slab";
                        var lProdType = "PRC";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Slab",
                                ProductType = "Pre-Cage",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Slab",
                                    ProductType = "Pre-Cage",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Slab",
                                    ProductType = "Pre-Cage",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdCarpet == 1)
                    {
                        var lStructureEle = "Slab";
                        var lProdType = "CAR";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Slab",
                                ProductType = "Carpet",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Slab",
                                    ProductType = "Carpet",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Slab",
                                    ProductType = "Carpet",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                }
                if (SESlabB == 1)
                {
                    var lStructureEle = "SLAB-B";
                    if (ProdCTSMesh == 1)
                    {
                        var lProdType = "MSH";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "SLAB-B",
                                ProductType = "Cut-To-Size Mesh",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "SLAB-B",
                                    ProductType = "Cut-To-Size Mesh",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "SLAB-B",
                                    ProductType = "Cut-To-Size Mesh",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdCAB == 1)
                    {
                        var lProdType = "CAB";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "SLAB-B",
                                ProductType = "CAB",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "SLAB-B",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "SLAB-B",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdCarpet == 1)
                    {
                        var lProdType = "CAR";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "SLAB-B",
                                ProductType = "Carpet",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "SLAB-B",
                                    ProductType = "Carpet",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "SLAB-B",
                                    ProductType = "Carpet",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                }
                if (SESlabT == 1)
                {
                    var lStructureEle = "SLAB-T";
                    if (ProdCTSMesh == 1)
                    {
                        var lProdType = "MSH";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "SLAB-T",
                                ProductType = "Cut-To-Size Mesh",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "SLAB-T",
                                    ProductType = "Cut-To-Size Mesh",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "SLAB-T",
                                    ProductType = "Cut-To-Size Mesh",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdCAB == 1)
                    {
                        var lProdType = "CAB";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "SLAB-T",
                                ProductType = "CAB",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "SLAB-T",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "SLAB-T",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdCarpet == 1)
                    {
                        var lProdType = "CAR";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "SLAB-T",
                                ProductType = "Carpet",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "SLAB-T",
                                    ProductType = "Carpet",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "SLAB-T",
                                    ProductType = "Carpet",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                }
                if (SEDrain == 1)
                {
                    var lStructureEle = "DRAIN";
                    if (ProdCTSMesh == 1)
                    {
                        var lProdType = "MSH";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Drain",
                                ProductType = "Cut-To-Size Mesh",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Drain",
                                    ProductType = "Cut-To-Size Mesh",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Drain",
                                    ProductType = "Cut-To-Size Mesh",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdCAB == 1)
                    {
                        var lProdType = "CAB";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Drain",
                                ProductType = "CAB",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Drain",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Drain",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdCarpet == 1)
                    {
                        var lProdType = "CAR";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Drain",
                                ProductType = "Carpet",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Drain",
                                    ProductType = "Carpet",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Drain",
                                    ProductType = "Carpet",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                }
                if (SEDwall == 1)
                {
                    if (ProdCAB == 1)
                    {
                        var lStructureEle = "DWALL";
                        var lProdType = "CAB";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "DWall",
                                ProductType = "CAB",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "DWall",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "DWall",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdPreCage == 1)
                    {
                        var lStructureEle = "DWALL";
                        var lProdType = "PRC";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "DWall",
                                ProductType = "Pre-Cage",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "DWall",
                                    ProductType = "Pre-Cage",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "DWall",
                                    ProductType = "Pre-Cage",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                }
                if (SEFdn == 1)
                {
                    var lStructureEle = "FDN1";
                    if (ProdCTSMesh == 1)
                    {
                        var lProdType = "MSH";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Foundation",
                                ProductType = "Cut-To-Size Mesh",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Foundation",
                                    ProductType = "Cut-To-Size Mesh",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Foundation",
                                    ProductType = "Cut-To-Size Mesh",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdCAB == 1)
                    {
                        var lProdType = "CAB";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Foundation",
                                ProductType = "CAB",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Foundation",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Foundation",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                }
                if (SEPile == 1)
                {
                    var lStructureEle = "Pile";
                    if (ProdCTSMesh == 1)
                    {
                        var lProdType = "MSH";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Pile",
                                ProductType = "Cut-To-Size Mesh",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Pile",
                                    ProductType = "Cut-To-Size Mesh",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Pile",
                                    ProductType = "Cut-To-Size Mesh",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdCAB == 1)
                    {
                        var lProdType = "CAB";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Pile",
                                ProductType = "CAB",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Pile",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Pile",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdBPC == 1)
                    {
                        var lProdType = "BPC";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Pile",
                                ProductType = "Bored Pile Cage",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Pile",
                                    ProductType = "Bored Pile Cage",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Pile",
                                    ProductType = "Bored Pile Cage",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                }
                if (SEMisc == 1)
                {
                    if (ProdCTSMesh == 1)
                    {
                        var lStructureEle = "MISC";
                        var lProdType = "MSH";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Misc",
                                ProductType = "Cut-To-Size Mesh",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Misc",
                                    ProductType = "Cut-To-Size Mesh",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Misc",
                                    ProductType = "Cut-To-Size Mesh",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdCAB == 1)
                    {
                        var lStructureEle = "MISC";
                        var lProdType = "CAB";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Misc",
                                ProductType = "CAB",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Misc",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Misc",
                                    ProductType = "CAB",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdPreCage == 1)
                    {
                        var lStructureEle = "MISC";
                        var lProdType = "PRC";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Misc",
                                ProductType = "Pre-Cage",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Misc",
                                    ProductType = "Pre-Cage",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Misc",
                                    ProductType = "Pre-Cage",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                    if (ProdCarpet == 1)
                    {
                        var lStructureEle = "MISC";
                        var lProdType = "CAR";
                        var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

                        if (lContractNo == "")
                        {
                            lReturn.Add(new
                            {
                                StructureElement = "Misc",
                                ProductType = "Carpet",
                                Message = "Failed to create WBS for the product as contract is not available for the product. Please check whether you select a correct project or contact the project coordinator."
                            });

                        }
                        else
                        {
                            if (CheckWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon) == true)
                            {
                                lReturn.Add(new
                                {
                                    StructureElement = "Misc",
                                    ProductType = "Carpet",
                                    Message = "The WBS for the structure element and product type exists already. "
                                });
                            }
                            else
                            {
                                lErrorMsg = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, WBS2, WBS3, lNDSCon);
                                lReturn.Add(new
                                {
                                    StructureElement = "Misc",
                                    ProductType = "Carpet",
                                    Message = "The WBS for the structure element and product type is created successfully. "
                                });
                            }
                        }
                    }
                }

                lProces.CloseNDSConnection(ref lNDSCon);

                lProces = null;

                lCmd = null;
                lNDSCon = null;
                lRst = null;
                return Ok(lReturn);
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                //return Ok(lProces);
            }

            return Ok(lReturn);
        }

        [HttpGet]
        [Route("/getContractNo/{pCustomerCode}/{pProjectCode}/{pProdType}/{pNDScon}")]
        string getContractNo(string pCustomerCode, string pProjectCode, string pProdType, SqlConnection pNDScon)
        {
            string lContractNo = "";
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            string lSQL = "";

            lSQL = "SELECT isNull(MAX(C.vchContractNumber), '') " +
            "FROM dbo.ContractMaster C, dbo.CustomerMaster U,  " +
            "dbo.ContractProductMapping M, dbo.SAPProjectMaster P " +
            "WHERE C.intCustomerCode = U.intCustomerCode " +
            "AND C.vchContractNumber = M.VBELN " +
            // "AND C.intContractID = P.intContractId " +
            "AND U.vchCustomerNo = '" + pCustomerCode + "' " +
            "AND P.vchProjectCode = '" + pProjectCode + "' ";

            if (pProdType == "CAB")
            {
                lSQL = lSQL + "AND M.ytot_cab > 0 ";

            }
            else if (pProdType == "MSH")
            {
                lSQL = lSQL + "AND M.ytot_mesh > 0 ";

            }
            else if (pProdType == "PRC")
            {
                lSQL = lSQL + "AND M.ytot_precage > 0 ";

            }
            else if (pProdType == "BPC")
            {
                lSQL = lSQL + "AND M.ytot_bpc > 0 ";

            }
            else if (pProdType == "CAR")
            {
                lSQL = lSQL + "AND M.ytot_car > 0 ";

            }

            lCmd = new SqlCommand(lSQL, pNDScon);
            lCmd.CommandTimeout = 1200;
            lRst = lCmd.ExecuteReader();
            if (lRst.HasRows)
            {
                lRst.Read();
                lContractNo = lRst.GetString(0);
            }
            lRst.Close();

            return lContractNo;
        }

        [HttpGet]
        [Route("/CheckWBS/{pCustCode}/{pProject}/{pContractNo}/{pStrucEle}/{pProdType}/{pWBS1}/{pWBS2}/{pWBS3}/{cnNDS}")]
        bool CheckWBS(string pCustCode, string pProject, string pContractNo, string pStrucEle, string pProdType,
                   string pWBS1, string pWBS2, string pWBS3, SqlConnection cnNDS)
        {
            string lSQL;
            SqlCommand lCmd;
            SqlDataReader adoRst;
            int lWBSID = 0;
            int lProjectID = 0;
            int lProdTypeID = 0;
            int lStrEleID = 0;
            int lWBSElementId = 0;
            var lReturn = false;

            lSQL = "SELECT P.intProjectId FROM dbo.SAPProjectMaster P, dbo.ContractMaster C " +
            "WHERE P.vchProjectCode = '" + pProject + "' " +
            "AND C.vchContractNumber = '" + pContractNo + "'";


            lCmd = new SqlCommand(lSQL, cnNDS);
            //lCmd.Transaction = osqlTransNDS;
            lCmd.CommandTimeout = 1200;
            adoRst = lCmd.ExecuteReader();
            if (adoRst.HasRows)
            {
                adoRst.Read();
                lProjectID = (int)adoRst.GetValue(0);
            }
            adoRst.Close();

            if (lProjectID == 0)
            {
                return false;
            }

            lSQL = "SELECT sitProductTypeID " +
                        "FROM dbo.ProductTypeMaster " +
                        "WHERE vchProductType = '" + pProdType + "' ";
            lCmd = new SqlCommand(lSQL, cnNDS);
            //lCmd.Transaction = osqlTransNDS;
            lCmd.CommandTimeout = 1200;
            adoRst = lCmd.ExecuteReader();
            if (adoRst.HasRows)
            {
                adoRst.Read();
                lProdTypeID = (Int16)adoRst.GetValue(0);
            }
            adoRst.Close();

            lSQL = "SELECT intStructureElementTypeId " +
                        "FROM dbo.StructureElementMaster " +
                        "WHERE vchStructureElementType = '" + pStrucEle + "' ";
            lCmd = new SqlCommand(lSQL, cnNDS);
            //lCmd.Transaction = osqlTransNDS;
            lCmd.CommandTimeout = 1200;
            adoRst = lCmd.ExecuteReader();
            if (adoRst.HasRows)
            {
                adoRst.Read();
                lStrEleID = (int)adoRst.GetValue(0);
            }
            adoRst.Close();

            lSQL = "SELECT intWBSId FROM dbo.WBS " +
                        "WHERE intProjectid = " + lProjectID.ToString() + " " +
                        "AND intWBSTypeID = 1 ";

            lCmd = new SqlCommand(lSQL, cnNDS);
            //lCmd.Transaction = osqlTransNDS;
            lCmd.CommandTimeout = 1200;
            adoRst = lCmd.ExecuteReader();
            if (adoRst.HasRows)
            {
                adoRst.Read();
                lWBSID = (int)adoRst.GetValue(0);
            }
            adoRst.Close();

            if (lWBSID > 0)
            {

                lSQL = "SELECT isNull(MAX(E.intWBSElementId), 0) " +
                    "FROM dbo.WBSElements E, dbo.WBSElementsDetails D " +
                            "where e.intWBSId = " + lWBSID.ToString() + " " +
                            "and E.intWBSElementId = D.intWBSElementId " +
                            "and sitProductTypeId = " + lProdTypeID.ToString() + " " +
                            "and intStructureElementTypeId = " + lStrEleID.ToString() + " " +
                            "and E.vchWBS1 = '" + pWBS1 + "' " +
                            "and E.vchWBS2 = '" + pWBS2 + "' " +
                            "and E.vchWBS3 = '" + pWBS3 + "' ";

                lCmd = new SqlCommand(lSQL, cnNDS);
                //lCmd.Transaction = osqlTransNDS;
                lCmd.CommandTimeout = 1200;
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    adoRst.Read();
                    lWBSElementId = (int)adoRst.GetValue(0);
                }
                adoRst.Close();

                if (lWBSElementId > 0)
                {
                    lReturn = true;
                }
            }

            return lReturn;
        }



        [HttpGet]
        [Route("/GenerateWBS/{pCustCode}/{pProject}/{pContractNo}/{pStrucEle}/{pProdType}/{pWBS1}/{pWBS2}/{pWBS3}/{cnNDS}")]
        string GenerateWBS(string pCustCode, string pProject, string pContractNo, string pStrucEle, string pProdType,
               string pWBS1, string pWBS2, string pWBS3, SqlConnection cnNDS)
        {
            string lReturn = "";
            string lSQL;
            SqlCommand lCmd;
            SqlDataReader adoRst;
            int lWBSID = 0;
            int lProjectID = 0;
            int lProdTypeID = 0;
            int lStrEleID = 0;
            int lCLevelID = 0;

            try
            {
                lSQL = "SELECT P.intProjectId FROM dbo.SAPProjectMaster P, dbo.ContractMaster C " +
                "WHERE  P.vchProjectCode = '" + pProject + "' " +
                "AND C.vchContractNumber = '" + pContractNo + "'";

                lCmd = new SqlCommand(lSQL, cnNDS);
                //lCmd.Transaction = osqlTransNDS;
                lCmd.CommandTimeout = 1200;
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    adoRst.Read();
                    lProjectID = (int)adoRst.GetValue(0);
                }
                adoRst.Close();

                if (lProjectID == 0)
                {
                    return "Error to get project ID.";
                }

                lSQL = "SELECT sitProductTypeID " +
                            "FROM dbo.ProductTypeMaster " +
                            "WHERE vchProductType = '" + pProdType + "' ";
                lCmd = new SqlCommand(lSQL, cnNDS);
                //lCmd.Transaction = osqlTransNDS;
                lCmd.CommandTimeout = 1200;
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    adoRst.Read();
                    lProdTypeID = (Int16)adoRst.GetValue(0);
                }
                adoRst.Close();

                lSQL = "SELECT intStructureElementTypeId " +
                            "FROM dbo.StructureElementMaster " +
                            "WHERE vchStructureElementType = '" + pStrucEle + "' ";
                lCmd = new SqlCommand(lSQL, cnNDS);
                //lCmd.Transaction = osqlTransNDS;
                lCmd.CommandTimeout = 1200;
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    adoRst.Read();
                    lStrEleID = (int)adoRst.GetValue(0);
                }
                adoRst.Close();

                lSQL = "SELECT intWBSId FROM dbo.WBS " +
                            "WHERE intProjectid = " + lProjectID.ToString() + " " +
                            "AND intWBSTypeID = 1 ";

                lCmd = new SqlCommand(lSQL, cnNDS);
                //lCmd.Transaction = osqlTransNDS;
                lCmd.CommandTimeout = 1200;
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    adoRst.Read();
                    lWBSID = (int)adoRst.GetValue(0);
                }
                adoRst.Close();

                if (lWBSID == 0)
                {
                    lSQL = "INSERT INTO dbo.WBS " +
                    "(intWBSTypeId " +
                    ", intProjectId " +
                    ", intCreatedUID " +
                    ", datCreatedDate) " +
                    " VALUES " +
                    "(1 " +
                    "," + lProjectID.ToString() + " " +
                    ",101 " +
                    "getDate() )";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    //lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    lCmd.ExecuteNonQuery();

                    lSQL = "SELECT intWBSId from dbo.WBS " +
                                "WHERE intProjectid = " + lProjectID.ToString() + " " +
                                "and intWBSType = 1 ";
                    lCmd = new SqlCommand(lSQL, cnNDS);
                    //lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    adoRst = lCmd.ExecuteReader();
                    if (adoRst.HasRows)
                    {
                        adoRst.Read();
                        lWBSID = (int)adoRst.GetValue(0);
                    }
                    adoRst.Close();
                }

                //insert Collape Level

                lSQL = "INSERT INTO dbo.WBSAtCollapseLevel " +
                "(intWBSId " +
                ", vchWBS1From " +
                ", vchWBS2From " +
                ", vchWBS2To " +
                ", vchWBS3From " +
                ", tntStatusId " +
                ", intStructureElementTypeId " +
                ", sitProductTypeId) " +
                "VALUES " +
                "(" + lWBSID.ToString() + " " +
                ",'" + pWBS1 + "' " +
                ",'" + pWBS2 + "' " +
                ",'" + pWBS2 + "' " +
                ",'" + pWBS3 + "' " +
                ",1 " +
                "," + lStrEleID.ToString() + " " +
                "," + lProdTypeID.ToString() + ") ";

                lCmd = new SqlCommand(lSQL, cnNDS);
                //lCmd.Transaction = osqlTransNDS;
                lCmd.CommandTimeout = 1200;
                lCmd.ExecuteNonQuery();

                lSQL = "SELECT MAX(intStoreyLevelWBSId) FROM dbo.WBSAtCollapseLevel ";

                lCmd = new SqlCommand(lSQL, cnNDS);
                //lCmd.Transaction = osqlTransNDS;
                lCmd.CommandTimeout = 1200;
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    adoRst.Read();
                    lCLevelID = (int)adoRst.GetValue(0);
                }
                adoRst.Close();

                lSQL = "usp_WBSMaintenance_Insert_PV";
                lCmd = new SqlCommand(lSQL, cnNDS);
                //lCmd.Transaction = osqlTransNDS;
                lCmd.CommandType = CommandType.StoredProcedure;
                lCmd.CommandText = "usp_WBSMaintenance_Insert_PV";

                lCmd.Parameters.Add("@intWBSId", SqlDbType.Int).Value = lWBSID;
                lCmd.Parameters.Add("@intCollapseLevelWBSId", SqlDbType.Int).Value = lCLevelID;

                lCmd.Parameters.Add("@intProjectId", SqlDbType.Int).Value = lProjectID;
                lCmd.Parameters.Add("@intWBSTypeId", SqlDbType.Int).Value = 1;
                lCmd.Parameters.Add("@vchStatus", SqlDbType.VarChar).Value = "1";
                lCmd.Parameters.Add("@vchWBS1From", SqlDbType.VarChar).Value = pWBS1;
                lCmd.Parameters.Add("@vchWBS1To", SqlDbType.VarChar).Value = "";
                lCmd.Parameters.Add("@vchWBS2From", SqlDbType.VarChar).Value = pWBS2;
                lCmd.Parameters.Add("@vchWBS2To", SqlDbType.VarChar).Value = pWBS2;
                lCmd.Parameters.Add("@vchWBS3From", SqlDbType.VarChar).Value = pWBS3;
                lCmd.Parameters.Add("@vchWBS3To", SqlDbType.VarChar).Value = "";
                lCmd.Parameters.Add("@vchWBS4From", SqlDbType.VarChar).Value = "";
                lCmd.Parameters.Add("@vchWBS4To", SqlDbType.VarChar).Value = "";
                lCmd.Parameters.Add("@vchWBS5From", SqlDbType.VarChar).Value = "";
                lCmd.Parameters.Add("@vchWBS5To", SqlDbType.VarChar).Value = "";
                lCmd.Parameters.Add("@vchStructureElement", SqlDbType.VarChar).Value = lStrEleID.ToString() + "\xec";//+ Encoding.ASCII.GetString(new byte[] { 236 });
                lCmd.Parameters.Add("@intProductTypeId", SqlDbType.Int).Value = lProdTypeID;
                lCmd.Parameters.Add("@tntTechSubSecotrId", SqlDbType.Int).Value = 107;
                lCmd.Parameters.Add("@intUserId", SqlDbType.Int).Value = 11;
                lCmd.Parameters.Add("@intConfirm", SqlDbType.Int).Value = 1;

                var lMsg = lCmd.ExecuteReader();
                //sqlreader = myCommand.ExecuteNonQuery()

                if (lMsg.HasRows == true)
                {
                    lReturn = "The WBS on the selected structure element and product is created successfully.";
                }
                else
                {
                    lReturn = "Failed to create WBS.";
                }
            }
            catch (Exception ex)
            {
                lReturn = "Error on WBS creation. Error Message; " + ex.Message;
            }
            return lReturn;
        }


        [HttpPost]
        [Route("/getWBS1Multiple")]
        public ActionResult getWBS1Multiple(WBS1Multiple wBS1Multiple)
        {
            UserAccessController lUa = new UserAccessController();
            var lUserType = lUa.getUserType(wBS1Multiple.UserName);///"AD";
            lUa = null;

            var lWBS1 = new List<string>();
            var lWBS2 = new List<string>();
            var lWBS3 = new List<string>();

            var lDa = new SqlDataAdapter();
            var lCmd = new SqlCommand();
            var lDs = new DataSet();
            var cnNDS = new SqlConnection();
            SqlDataReader lRst;

            List<string> lWBS1F = new List<String>();
            List<string> lWBS2F = new List<String>();
            var lWBS2b = "";
            var lSQL = "";

            var lProcess = new ProcessController();
            lProcess.OpenNDSConnection(ref cnNDS);
            if (cnNDS.State == ConnectionState.Open)
            {
                if (lUserType == "TE" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
                {
                    lSQL = "SELECT rtrim(ltrim(vchWBS1)) as WBS1, " +
                   "rtrim(ltrim(vchWBS2)) as WBS2, " +
                   "rtrim(ltrim(vchWBS3)) as WBS3  " +
                   "FROM dbo.WBSElements E, dbo.WBS W, " +
                   "dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                   "dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                   //"dbo.ContractMaster C " +
                   "WHERE W.intWBSId = E.intWBSId " +
                   "AND P.intProjectId = W.intProjectId " +
                   //"AND P.intContractID = C.intContractID " +
                   "AND E.intWBSElementId = D.intWBSElementId " +
                   "AND D.sitProductTypeID = T.sitProductTypeID " +
                   "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                   "AND vchProjectCode = '" + wBS1Multiple.ProjectCode + "' " +
                   "AND (vchProductType = 'CAB' " +
                   "OR ((vchProductType = 'MSH' " +
                   "OR vchProductType = 'CAR' " +
                   "OR vchProductType = 'BPC' " +
                   "OR vchProductType = 'PRC' " +
                   "OR vchProductType = 'CORE') " +
                   "AND EXISTS(SELECT H.intPostHeaderid " +
                   "FROM dbo.BBSPostHeader H " +
                   "WHERE intWBSElementId = E.intWBSElementId " +
                   "AND NOT EXISTS(SELECT R.intPostHeaderid " +
                   "FROM dbo.BBSReleaseDetails R " +
                   "WHERE R.intPostHeaderid = H.intPostHeaderid " +
                   "AND R.tntStatusId = 12) ) ) ) " +
                   "AND E.tntStatusId = 1 " +
                   "AND D.intConfirm = 1 " +
                   "GROUP BY vchWBS1, " +
                   "vchWBS2, " +
                   "vchWBS3  " +
                   "ORDER BY " +
                   "vchWBS1, " +
                   "(case when PATINDEX('B[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                   "PATINDEX('%[^0-9]%', substring(vchWBS2 + 'z', (PATINDEX('%[0-9]%', vchWBS2)), " +
                   "len(vchWBS2)) + 'z') ) as int)    " +
                   "else (case when PATINDEX('FDN[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                   "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
                   "len(vchWBS2)) + 'z') ) as int) else  " +
                   "case when(PATINDEX('B%', vchWBS2) > 0 OR PATINDEX('FDN%', vchWBS2) > 0) then 98  else 99 end " +
                   "end) end), " +
                   " " +
                   "(case when PATINDEX('[^0-9]%',vchWBS2) > 0 then vchWBS2 " +
                   "else '' end), " +
                   " " +
                   "(CASE WHEN PATINDEX('%[0-9]%',vchWBS2) > 0 THEN cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                   "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2) + 1), " +
                   "len(vchWBS2)) + 'z') ) as int) " +
                   "ELSE " +
                   "cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), len(vchWBS2)), " +
                   "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
                   "len(vchWBS2)) + 'z') - 1) as int) " +
                   "END),  " +
                   "vchWBS2, " +
                   "vchWBS3 ";
                }
                else
                {
                    lSQL = "SELECT rtrim(ltrim(vchWBS1)) as WBS1, " +
                   "rtrim(ltrim(vchWBS2)) as WBS2, " +
                   "rtrim(ltrim(vchWBS3)) as WBS3  " +
                   "FROM dbo.WBSElements E, dbo.WBS W, " +
                   "dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                   "dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                   //"dbo.ContractMaster C " +
                   "WHERE W.intWBSId = E.intWBSId " +
                   "AND P.intProjectId = W.intProjectId " +
                   // "AND P.intContractID = C.intContractID " +
                   "AND E.intWBSElementId = D.intWBSElementId " +
                   "AND D.sitProductTypeID = T.sitProductTypeID " +
                   "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                   "AND vchProjectCode = '" + wBS1Multiple.ProjectCode + "' " +
                   "AND (vchProductType = 'CAB' " +
                   "OR ((vchProductType = 'MSH' " +
                   "OR vchProductType = 'CAR' " +
                   "OR vchProductType = 'BPC' " +
                   "OR vchProductType = 'PRC' " +
                   "OR vchProductType = 'CORE') " +
                   "AND EXISTS(SELECT H.intPostHeaderid " +
                   "FROM dbo.BBSPostHeader H " +
                   "WHERE intWBSElementId = E.intWBSElementId " +
                   "AND NOT EXISTS(SELECT R.intPostHeaderid " +
                   "FROM dbo.BBSReleaseDetails R " +
                   "WHERE R.intPostHeaderid = H.intPostHeaderid " +
                   "AND R.tntStatusId = 12) ) ) ) " +
                   "AND E.tntStatusId = 1 " +
                   "AND D.intConfirm = 1 " +
                   "GROUP BY vchWBS1, " +
                   "vchWBS2, " +
                   "vchWBS3  " +
                   "ORDER BY " +
                   "vchWBS1, " +
                   "(case when PATINDEX('B[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                   "PATINDEX('%[^0-9]%', substring(vchWBS2 + 'z', (PATINDEX('%[0-9]%', vchWBS2)), " +
                   "len(vchWBS2)) + 'z') ) as int)    " +
                   "else (case when PATINDEX('FDN[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                   "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
                   "len(vchWBS2)) + 'z') ) as int) else  " +
                   "case when(PATINDEX('B%', vchWBS2) > 0 OR PATINDEX('FDN%', vchWBS2) > 0) then 98  else 99 end " +
                   "end) end), " +
                   " " +
                   "(case when PATINDEX('[^0-9]%',vchWBS2) > 0 then vchWBS2 " +
                   "else '' end), " +
                   " " +
                   "(CASE WHEN PATINDEX('%[0-9]%',vchWBS2) > 0 THEN cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                   "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2) + 1), " +
                   "len(vchWBS2)) + 'z') ) as int) " +
                   "ELSE " +
                   "cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), len(vchWBS2)), " +
                   "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
                   "len(vchWBS2)) + 'z') - 1) as int) " +
                   "END),  " +
                   "vchWBS2, " +
                   "vchWBS3 ";
                }

                lCmd.CommandType = CommandType.Text;
                lCmd.CommandText = lSQL;
                lCmd.Connection = cnNDS;
                lCmd.CommandTimeout = 300;
                lDa.SelectCommand = lCmd;
                lDs = new DataSet();
                lDa.Fill(lDs);
                if (lDs.Tables[0].Rows.Count > 0)
                {
                    int lFound = 0;
                    for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                    {
                        lFound = 0;

                        var lWBS1t = lDs.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                        var lWBS2t = lDs.Tables[0].Rows[i].ItemArray[1].ToString().Trim();
                        var lWBS3t = lDs.Tables[0].Rows[i].ItemArray[2].ToString().Trim();

                        //get List of WBS1
                        if (lWBS1.Count > 0)
                        {
                            for (int j = 0; j < lWBS1.Count; j++)
                            {
                                if (lWBS1t == lWBS1[j])
                                {
                                    lFound = 1;
                                    break;
                                }
                            }
                        }
                        if (lFound == 0)
                        {
                            lWBS1.Add(lWBS1t);

                            //get existing WBS1
                            if (wBS1Multiple.WBS1 != null && wBS1Multiple.WBS1.Count > 0)
                            {
                                for (int j = 0; j < wBS1Multiple.WBS1.Count; j++)
                                {
                                    if (wBS1Multiple.WBS1[j] == lWBS1t)
                                    {
                                        lWBS1F.Add(wBS1Multiple.WBS1[j]);
                                    }
                                }
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    lWBS1F.Add(lWBS1t);
                                }
                            }
                        }

                        //get List of WBS2
                        if (lWBS1F.Exists(x => x.Equals(lWBS1t)))
                        {
                            lFound = 0;
                            if (lWBS2.Count > 0)
                            {
                                for (int j = 0; j < lWBS2.Count; j++)
                                {
                                    if (lWBS2t == lWBS2[j])
                                    {
                                        lFound = 1;
                                        break;
                                    }
                                }
                            }
                            if (lFound == 0)
                            {
                                lWBS2.Add(lWBS2t);

                                // get existing WBS2
                                if (wBS1Multiple.WBS2 != null && wBS1Multiple.WBS2.Count > 0)
                                {
                                    for (int j = 0; j < wBS1Multiple.WBS2.Count; j++)
                                    {
                                        if (wBS1Multiple.WBS2[j] == lWBS2t)
                                        {
                                            lWBS2F.Add(wBS1Multiple.WBS2[j]);
                                        }
                                    }
                                }
                                else
                                {
                                    if (lWBS2.Count == 1)
                                    {
                                        lWBS2F.Add(lWBS2[0]);
                                    }
                                }
                            }
                        }
                    }

                    //reverse the sorting

                    if (lWBS2.Count() > 1)
                    {
                        var lWBS2a = lWBS2;
                        lWBS2 = new List<string>();

                        for (int i = lWBS2a.Count() - 1; i >= 0; i--)
                        {
                            lWBS2.Add(lWBS2a[i]);
                        }
                    }

                    // get WBS3
                    for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                    {
                        var lWBS1t = lDs.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                        var lWBS2t = lDs.Tables[0].Rows[i].ItemArray[1].ToString().Trim();
                        var lWBS3t = lDs.Tables[0].Rows[i].ItemArray[2].ToString().Trim();

                        if (lWBS1F != null && lWBS1F.Count > 0 && lWBS2F != null && lWBS2F.Count > 0)
                        {
                            for (int j = 0; j < lWBS1F.Count; j++)
                            {
                                for (int k = 0; k < lWBS2F.Count; k++)
                                {
                                    if (lWBS1t == lWBS1F[j] && lWBS2t == lWBS2F[k])
                                    {
                                        lFound = 0;
                                        if (lWBS3.Count > 0)
                                        {
                                            for (int m = 0; m < lWBS3.Count; m++)
                                            {
                                                if (lWBS3[m] == lWBS3t)
                                                {
                                                    lFound = 1;
                                                    break;
                                                }
                                            }
                                        }
                                        if (lFound == 0)
                                        {
                                            lWBS3.Add(lWBS3t);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                lProcess.CloseNDSConnection(ref cnNDS);
            }
            lDa = null;
            lCmd = null;
            lDs = null;
            cnNDS = null;


            var lReturn = new
            {
                WBS1 = lWBS1,
                WBS2 = lWBS2,
                WBS3 = lWBS3
            };
            return Ok(lReturn);
        }


        [HttpPost]
        [Route("/getWBS2Multiple")]
        public ActionResult getWBS2Multiple(WBS1Multiple wBS1Multiple)
        {
            UserAccessController lUa = new UserAccessController();
            var lUserType = lUa.getUserType(wBS1Multiple.UserName);//"AD";
            lUa = null;

            var lWBS2 = new List<string>();
            var lWBS3 = new List<string>();

            var lDa = new SqlDataAdapter();
            var lCmd = new SqlCommand();
            var lDs = new DataSet();
            var cnNDS = new SqlConnection();
            SqlDataReader lRst;
            List<string> lWBS2F = new List<string>();
            var lSQL = "";

            var lProcess = new ProcessController();
            lProcess.OpenNDSConnection(ref cnNDS);
            if (cnNDS.State == ConnectionState.Open)
            {
                string lCond = "";
                if (wBS1Multiple.WBS1 != null && wBS1Multiple.WBS1.Count > 0)
                {
                    for (int i = 0; i < wBS1Multiple.WBS1.Count; i++)
                    {
                        lCond = lCond + " OR vchWBS1 = '" + wBS1Multiple.WBS1[i] + "' ";
                    }
                    lCond = lCond.Substring(3);
                }
                else
                {
                    lCond = lCond + " vchWBS1 = '' ";
                }

                if (lUserType == "TE" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
                {
                    lSQL = "SELECT " +
                    "rtrim(ltrim(vchWBS2)) as WBS2, " +
                    "rtrim(ltrim(vchWBS3)) as WBS3  " +
                    "FROM dbo.WBSElements E, dbo.WBS W, " +
                    "dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                    "dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                    //"dbo.ContractMaster C " +
                    "WHERE W.intWBSId = E.intWBSId " +
                    "AND P.intProjectId = W.intProjectId " +
                    // "AND P.intContractID = C.intContractID " +
                    "AND E.intWBSElementId = D.intWBSElementId " +
                    "AND D.sitProductTypeID = T.sitProductTypeID " +
                    "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                    "AND vchProjectCode = '" + wBS1Multiple.ProjectCode + "' " +
                    "AND (vchProductType = 'CAB' " +
                    "OR ((vchProductType = 'MSH' " +
                    "OR vchProductType = 'CAR' " +
                    "OR vchProductType = 'BPC' " +
                    "OR vchProductType = 'PRC' " +
                    "OR vchProductType = 'CORE') " +
                    "AND EXISTS(SELECT H.intPostHeaderid " +
                    "FROM dbo.BBSPostHeader H " +
                    "WHERE intWBSElementId = E.intWBSElementId " +
                    "AND NOT EXISTS(SELECT R.intPostHeaderid " +
                    "FROM dbo.BBSReleaseDetails R " +
                    "WHERE R.intPostHeaderid = H.intPostHeaderid " +
                    "AND R.tntStatusId = 12) ) ) ) " +
                    "AND ( " + lCond + " ) " +
                    "AND E.tntStatusId = 1 " +
                    "AND D.intConfirm = 1 " +
                    "GROUP BY " +
                    "vchWBS2, " +
                    "vchWBS3  " +
                    "ORDER BY " +
                    "(case when PATINDEX('B[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                    "PATINDEX('%[^0-9]%', substring(vchWBS2 + 'z', (PATINDEX('%[0-9]%', vchWBS2)), " +
                    "len(vchWBS2)) + 'z') ) as int)    " +
                    "else (case when PATINDEX('FDN[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                    "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
                    "len(vchWBS2)) + 'z') ) as int) else  " +
                    "case when(PATINDEX('B%', vchWBS2) > 0 OR PATINDEX('FDN%', vchWBS2) > 0) then 98  else 99 end " +
                    "end) end), " +
                    " " +
                    "(case when PATINDEX('[^0-9]%',vchWBS2) > 0 then vchWBS2 " +
                    "else '' end), " +
                    " " +
                    "(CASE WHEN PATINDEX('%[0-9]%',vchWBS2) > 0 THEN cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                    "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2) + 1), " +
                    "len(vchWBS2)) + 'z') ) as int) " +
                    "ELSE " +
                    "cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), len(vchWBS2)), " +
                    "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
                    "len(vchWBS2)) + 'z') - 1) as int) " +
                    "END),  " +
                    "vchWBS2, " +
                    "vchWBS3 ";
                }
                else
                {
                    lSQL = "SELECT " +
                    "rtrim(ltrim(vchWBS2)) as WBS2, " +
                    "rtrim(ltrim(vchWBS3)) as WBS3  " +
                    "FROM dbo.WBSElements E, dbo.WBS W, " +
                    "dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                    "dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                    //"dbo.ContractMaster C " +
                    "WHERE W.intWBSId = E.intWBSId " +
                    "AND P.intProjectId = W.intProjectId " +
                    // "AND P.intContractID = C.intContractID " +
                    "AND E.intWBSElementId = D.intWBSElementId " +
                    "AND D.sitProductTypeID = T.sitProductTypeID " +
                    "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                    "AND vchProjectCode = '" + wBS1Multiple.ProjectCode + "' " +
                    "AND (vchProductType = 'CAB' " +
                    "OR ((vchProductType = 'MSH' " +
                    "OR vchProductType = 'CAR' " +
                    "OR vchProductType = 'BPC' " +
                    "OR vchProductType = 'PRC' " +
                    "OR vchProductType = 'CORE') " +
                    "AND EXISTS(SELECT H.intPostHeaderid " +
                    "FROM dbo.BBSPostHeader H " +
                    "WHERE intWBSElementId = E.intWBSElementId " +
                    "AND NOT EXISTS(SELECT R.intPostHeaderid " +
                    "FROM dbo.BBSReleaseDetails R " +
                    "WHERE R.intPostHeaderid = H.intPostHeaderid " +
                    "AND R.tntStatusId = 12) ) ) ) " +
                    "AND ( " + lCond + " ) " +
                    "AND E.tntStatusId = 1 " +
                    "AND D.intConfirm = 1 " +
                    "GROUP BY " +
                    "vchWBS2, " +
                    "vchWBS3  " +
                    "ORDER BY " +
                    "(case when PATINDEX('B[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                    "PATINDEX('%[^0-9]%', substring(vchWBS2 + 'z', (PATINDEX('%[0-9]%', vchWBS2)), " +
                    "len(vchWBS2)) + 'z') ) as int)    " +
                    "else (case when PATINDEX('FDN[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                    "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
                    "len(vchWBS2)) + 'z') ) as int) else  " +
                    "case when(PATINDEX('B%', vchWBS2) > 0 OR PATINDEX('FDN%', vchWBS2) > 0) then 98  else 99 end " +
                    "end) end), " +
                    " " +
                    "(case when PATINDEX('[^0-9]%',vchWBS2) > 0 then vchWBS2 " +
                    "else '' end), " +
                    " " +
                    "(CASE WHEN PATINDEX('%[0-9]%',vchWBS2) > 0 THEN cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                    "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2) + 1), " +
                    "len(vchWBS2)) + 'z') ) as int) " +
                    "ELSE " +
                    "cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), len(vchWBS2)), " +
                    "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
                    "len(vchWBS2)) + 'z') - 1) as int) " +
                    "END),  " +
                    "vchWBS2, " +
                    "vchWBS3 ";
                }

                lCmd.CommandType = CommandType.Text;
                lCmd.CommandText = lSQL;
                lCmd.Connection = cnNDS;
                lCmd.CommandTimeout = 300;
                lDa.SelectCommand = lCmd;
                lDs = new DataSet();
                lDa.Fill(lDs);
                if (lDs.Tables[0].Rows.Count > 0)
                {
                    int lFound = 0;
                    for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                    {
                        lFound = 0;
                        var lWBS2t = lDs.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                        var lWBS3t = lDs.Tables[0].Rows[i].ItemArray[1].ToString().Trim();

                        lFound = 0;
                        if (lWBS2.Count > 0)
                        {
                            for (int j = 0; j < lWBS2.Count; j++)
                            {
                                if (lWBS2t == lWBS2[j])
                                {
                                    lFound = 1;
                                    break;
                                }
                            }
                        }
                        if (lFound == 0)
                        {
                            lWBS2.Add(lWBS2t);

                            //get List of WBS2
                            if (wBS1Multiple.WBS2.Exists(x => x.Equals(lWBS2t)))
                            {
                                lWBS2F.Add(lWBS2t);
                            }

                        }

                    }

                    //reverse the sorting

                    if (lWBS2.Count() > 1)
                    {
                        var lWBS2a = lWBS2;
                        lWBS2 = new List<string>();

                        for (int i = lWBS2a.Count() - 1; i >= 0; i--)
                        {
                            lWBS2.Add(lWBS2a[i]);
                        }

                    }

                    // get WBS3
                    for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                    {
                        lFound = 0;
                        var lWBS2t = lDs.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                        var lWBS3t = lDs.Tables[0].Rows[i].ItemArray[1].ToString().Trim();

                        if (lWBS2F.Exists(x => x.Equals(lWBS2t)))
                        {
                            lFound = 0;
                            if (lWBS3.Count > 0)
                            {
                                for (int m = 0; m < lWBS3.Count; m++)
                                {
                                    if (lWBS3[m] == lWBS3t)
                                    {
                                        lFound = 1;
                                        break;
                                    }
                                }
                            }
                            if (lFound == 0)
                            {
                                lWBS3.Add(lWBS3t);
                            }
                        }
                    }


                }
                lProcess.CloseNDSConnection(ref cnNDS);
            }
            lDa = null;
            lCmd = null;
            lDs = null;
            cnNDS = null;



            var lReturn = new
            {
                WBS2 = lWBS2,
                WBS3 = lWBS3
            };
            return Ok(lReturn);
        }


        [HttpPost]
        [Route("/getWBS3Multiple")]
        public ActionResult getWBS3Multiple(WBS1Multiple wBS1Multiple)
        {
            UserAccessController lUa = new UserAccessController();
            var lUserType = "AD";//lUa.getUserType(User.Identity.GetUserName());
            lUa = null;

            var lWBS3 = new List<string>();

            var lDa = new SqlDataAdapter();
            var lCmd = new SqlCommand();
            var lDs = new DataSet();
            var cnNDS = new SqlConnection();

            var lSQL = "";

            var lProcess = new ProcessController();
            lProcess.OpenNDSConnection(ref cnNDS);
            if (cnNDS.State == ConnectionState.Open)
            {
                string lCond = "";
                if (wBS1Multiple.WBS1 != null && wBS1Multiple.WBS1.Count > 0 && wBS1Multiple.WBS2 != null && wBS1Multiple.WBS2.Count > 0)
                {
                    for (int i = 0; i < wBS1Multiple.WBS1.Count; i++)
                    {
                        string lCondWBS2 = "";
                        for (int j = 0; j < wBS1Multiple.WBS2.Count; j++)
                        {
                            lCondWBS2 = lCondWBS2 + " OR vchWBS2 = '" + wBS1Multiple.WBS2[j] + "' ";
                        }
                        lCondWBS2 = lCondWBS2.Substring(3);

                        lCond = lCond + " OR (vchWBS1 = '" + wBS1Multiple.WBS1[i] + "' AND ( " + lCondWBS2 + " )) ";

                    }
                    lCond = lCond.Substring(3);
                }
                else
                {
                    lCond = lCond + " vchWBS1 = '' AND vchWBS2 = '' ";
                }

                if (lUserType == "TE" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
                {
                    lSQL = "SELECT " +
                    "rtrim(ltrim(vchWBS3)) as WBS3  " +
                    "FROM dbo.WBSElements E, dbo.WBS W, " +
                    "dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                    "dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                    //"dbo.ContractMaster C " +
                    "WHERE W.intWBSId = E.intWBSId " +
                    "AND P.intProjectId = W.intProjectId " +
                    // "AND P.intContractID = C.intContractID " +
                    "AND E.intWBSElementId = D.intWBSElementId " +
                    "AND D.sitProductTypeID = T.sitProductTypeID " +
                    "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                    "AND vchProjectCode = '" + wBS1Multiple.ProjectCode + "' " +
                    "AND (vchProductType = 'CAB' " +
                    "OR ((vchProductType = 'MSH' " +
                    "OR vchProductType = 'CAR' " +
                    "OR vchProductType = 'BPC' " +
                    "OR vchProductType = 'PRC' " +
                     "OR vchProductType = 'CORE') " +
                    "AND EXISTS(SELECT H.intPostHeaderid " +
                    "FROM dbo.BBSPostHeader H " +
                    "WHERE intWBSElementId = E.intWBSElementId " +
                    "AND NOT EXISTS(SELECT R.intPostHeaderid " +
                    "FROM dbo.BBSReleaseDetails R " +
                    "WHERE R.intPostHeaderid = H.intPostHeaderid " +
                    "AND R.tntStatusId = 12) ) ) ) " +
                    "AND ( " + lCond + " ) " +
                    "AND E.tntStatusId = 1 " +
                    "AND D.intConfirm = 1 " +
                    "GROUP BY " +
                    "vchWBS3  " +
                    "ORDER BY " +
                    "vchWBS3 ";
                }
                else
                {
                    lSQL = "SELECT " +
                    "rtrim(ltrim(vchWBS3)) as WBS3  " +
                    "FROM dbo.WBSElements E, dbo.WBS W, " +
                    "dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                    "dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                    //"dbo.ContractMaster C " +
                    "WHERE W.intWBSId = E.intWBSId " +
                    "AND P.intProjectId = W.intProjectId " +
                    //"AND P.intContractID = C.intContractID " +
                    "AND E.intWBSElementId = D.intWBSElementId " +
                    "AND D.sitProductTypeID = T.sitProductTypeID " +
                    "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                    "AND vchProjectCode = '" + wBS1Multiple.ProjectCode + "' " +
                    "AND (vchProductType = 'CAB' " +
                    "OR ((vchProductType = 'MSH' " +
                    "OR vchProductType = 'CAR' " +
                    "OR vchProductType = 'BPC' " +
                    "OR vchProductType = 'PRC' " +
                    "OR vchProductType = 'CORE') " +
                    "AND EXISTS(SELECT H.intPostHeaderid " +
                    "FROM dbo.BBSPostHeader H " +
                    "WHERE intWBSElementId = E.intWBSElementId " +
                    "AND NOT EXISTS(SELECT R.intPostHeaderid " +
                    "FROM dbo.BBSReleaseDetails R " +
                    "WHERE R.intPostHeaderid = H.intPostHeaderid " +
                    "AND R.tntStatusId = 12) ) ) ) " +
                    "AND ( " + lCond + " ) " +
                    "AND E.tntStatusId = 1 " +
                    "AND D.intConfirm = 1 " +
                    "GROUP BY " +
                    "vchWBS3  " +
                    "ORDER BY " +
                    "vchWBS3 ";

                }

                lCmd.CommandType = CommandType.Text;
                lCmd.CommandText = lSQL;
                lCmd.Connection = cnNDS;
                lCmd.CommandTimeout = 300;
                lDa.SelectCommand = lCmd;
                lDs = new DataSet();
                lDa.Fill(lDs);
                if (lDs.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                    {
                        var lWBS3t = lDs.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                        lWBS3.Add(lWBS3t);
                    }
                }
                lProcess.CloseNDSConnection(ref cnNDS);
            }
            lDa = null;
            lCmd = null;
            lDs = null;
            cnNDS = null;

            //var content = extractWBS3(ProjectCode, WBS1, WBS2);

            return Ok(lWBS3);
        }

        [HttpPost]
        [Route("/WithdrawOrderPE")]
        public ActionResult WithdrawOrderPE(WithdrawOrderPE withdrawOrderPE)
        {
            bool lReturn = true;
            string lResponseText = "";
            try
            {
                string lUserID = withdrawOrderPE.UserName;//"vishalw_ttl@natsteel.com.sg"; // User.Identity.GetUserName();
                UserAccessController lUa = new UserAccessController();
                var lUserType = lUa.getUserType(lUserID);
                var lGroupName = lUa.getGroupName(lUserID);

                var lSubmission = "No";
                var lEditable = "No";

                string lUserName = lUserID;
                if (lUserName.IndexOf("@") > 0)
                {
                    lUserName = lUserName.Substring(0, lUserName.IndexOf("@"));
                }

                lUa = null;

                //get Access right;
                if (lUserType == "CU" || lUserType == "CA" || lUserType == "CM")
                {
                    var lAccess = db.UserAccess.Find(lUserID, withdrawOrderPE.pCustomerCode, withdrawOrderPE.pProjectCode);
                    if (lAccess != null)
                    {
                        lSubmission = lAccess.OrderSubmission.Trim();
                        lEditable = lAccess.OrderCreation.Trim();
                    }
                }
                else
                {
                    if (lUserType == "TE" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
                    {
                        lSubmission = "Yes";
                        lEditable = "Yes";
                    }
                }

                if (lSubmission != "Yes")
                {
                    lResponseText = "You have insufficient permission to withdraw the order. Please contact NatSteel coordinator.";
                    lReturn = false;
                }

                if (lReturn == true)
                {
                    var lSE = db.OrderProjectSE.Find(withdrawOrderPE.pOrderNo, withdrawOrderPE.pStructureElement, withdrawOrderPE.pProductType, withdrawOrderPE.pScheduledProd);

                    if (lSubmission == "Yes" && lSE.OrderStatus != "Submitted")
                    {
                        lResponseText = "Order had been processed already. Please contact NatSteel coordinator to withdraw the order.";
                        lReturn = false;
                    }

                    var lNewSE = lSE;
                    lNewSE.OrderStatus = "Created";
                    db.Entry(lSE).CurrentValues.SetValues(lNewSE);

                    if (lSE.CABJobID > 0)
                    {
                        int lJobID = lSE.CABJobID;
                        var lCAB = db.JobAdvice.Find(withdrawOrderPE.pCustomerCode, withdrawOrderPE.pProjectCode, lJobID);
                        var lNewCAB = lCAB;
                        lNewCAB.OrderStatus = "Created";
                        lNewCAB.UpdateBy = lUserID;
                        lNewCAB.UpdateDate = DateTime.Now;

                        db.Entry(lCAB).CurrentValues.SetValues(lNewCAB);
                    }
                    if (lSE.MESHJobID > 0)
                    {
                        int lJobID = lSE.MESHJobID;
                        var lMESH = db.CTSMESHJobAdvice.Find(withdrawOrderPE.pCustomerCode, withdrawOrderPE.pProjectCode, lJobID);
                        var lNewMESH = lMESH;
                        lNewMESH.OrderStatus = "Created";
                        lNewMESH.UpdateBy = lUserID;
                        lNewMESH.UpdateDate = DateTime.Now;

                        db.Entry(lMESH).CurrentValues.SetValues(lNewMESH);
                    }
                    if (lSE.BPCJobID > 0)
                    {
                        int lJobID = lSE.BPCJobID;
                        var lBPC = db.BPCJobAdvice.Find(withdrawOrderPE.pCustomerCode, withdrawOrderPE.pProjectCode, false, lJobID);
                        var lNewBPC = lBPC;
                        lNewBPC.OrderStatus = "Created";
                        lNewBPC.UpdateBy = lUserID;
                        lNewBPC.UpdateDate = DateTime.Now;

                        db.Entry(lBPC).CurrentValues.SetValues(lNewBPC);
                    }
                    if (lSE.CageJobID > 0)
                    {
                        int lJobID = lSE.CageJobID;
                        var lPRC = db.PRCJobAdvice.Find(withdrawOrderPE.pCustomerCode, withdrawOrderPE.pProjectCode, lJobID);

                        var lNewPRC = lPRC;
                        lNewPRC.OrderStatus = "Created";
                        lNewPRC.UpdateBy = lUserID;
                        lNewPRC.UpdateDate = DateTime.Now;

                        db.Entry(lPRC).CurrentValues.SetValues(lNewPRC);
                    }

                    if (lSE.StdBarsJobID > 0 || lSE.StdMESHJobID > 0 || lSE.CoilProdJobID > 0)
                    {
                        int lJobID = 0;
                        if (lSE.StdBarsJobID > 0)
                        {
                            lJobID = lSE.StdBarsJobID;
                        }
                        if (lSE.StdMESHJobID > 0)
                        {
                            lJobID = lSE.StdMESHJobID;
                        }
                        if (lSE.CoilProdJobID > 0)
                        {
                            lJobID = lSE.CoilProdJobID;
                        }

                        var lStd = db.StdSheetJobAdvice.Find(withdrawOrderPE.pCustomerCode, withdrawOrderPE.pProjectCode, lJobID);
                        var lNewStd = lStd;
                        //lNewStd.OrderSource = "UX";
                        lNewStd.OrderStatus = "Created";
                        lNewStd.UpdateBy = lUserID;
                        lNewStd.UpdateDate = DateTime.Now;

                        db.Entry(lStd).CurrentValues.SetValues(lNewStd);
                    }
                    db.SaveChanges();

                    //No email sending if natsteel staff created and processed Standard products.
                    var lNeedEmail = 1;
                    var lStandardProd = 0;
                    if (lUserType != "CU" && lUserType != "CA" && lUserType != "CM")
                    {
                        var lOrderSE = (from p in db.OrderProjectSE
                                        where p.OrderNumber == withdrawOrderPE.pOrderNo
                                        select p).ToList();
                        if (lOrderSE != null && lOrderSE.Count > 0)
                        {
                            for (int i = 0; i < lOrderSE.Count; i++)
                            {
                                if (lOrderSE[i].ProductType == "STANDARD-MESH" ||
                                    lOrderSE[i].ProductType == "STANDARD-BAR" ||
                                    lOrderSE[i].ProductType == "COIL")
                                {
                                    lStandardProd = 1;
                                    break;
                                }
                            }
                        }
                        if (lUserID.Split('@')[1].ToLower() == "natsteel.com.sg"
                            && lStandardProd == 1)
                        {
                            lNeedEmail = 0;
                        }
                    }

                    //send prompting email
                    if (lNeedEmail == 1)
                    {
                        var lEmailObj = new SendGridEmail();
                        lEmailObj.sendOrderActionEmail(withdrawOrderPE.pCustomerCode, withdrawOrderPE.pProjectCode, withdrawOrderPE.pOrderNo, "Created", "Withdraw", lUserID, 0, withdrawOrderPE.pStructureElement, withdrawOrderPE.pProductType, withdrawOrderPE.pScheduledProd);
                        lEmailObj = null;
                    }
                }
            }
            catch (Exception ex)
            {
                //SaveErrorMsg(ex.Message, ex.StackTrace);
                lReturn = false;
                lResponseText = "Error:" + ex.Message;
            }

            return Json(new { success = lReturn, message = lResponseText });
        }

        [HttpPost]
        [Route("/SubmitOrderPE")]
        public ActionResult SubmitOrderPE(SubmitOrderPE submitOrderPE)
        {
            bool lReturn = true;
            string lResponseText = "";

            DateTime lReqDate = DateTime.Now;
            if (submitOrderPE.pRequiredDate != null && submitOrderPE.pRequiredDate != "")
            {
                lReqDate = DateTime.ParseExact(submitOrderPE.pRequiredDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            try
            {
                string lUserID = "jeevanantham.s@hpconstn.com.sg";
                UserAccessController lUa = new UserAccessController();
                var lUserType = lUa.getUserType(lUserID);
                var lGroupName = lUa.getGroupName(lUserType);

                var lSubmission = "No";
                var lEditable = "No";

                string lUserName = lUserID;
                if (lUserName.IndexOf("@") > 0)
                {
                    lUserName = lUserName.Substring(0, lUserName.IndexOf("@"));
                }

                lUa = null;

                //get Access right;
                if (lUserType == "CU" || lUserType == "CA" || lUserType == "CM")
                {
                    var lAccess = db.UserAccess.Find(lUserID, submitOrderPE.pCustomerCode, submitOrderPE.pProjectCode);
                    if (lAccess != null)
                    {
                        lSubmission = lAccess.OrderSubmission.Trim();
                        lEditable = lAccess.OrderCreation.Trim();
                    }
                }
                else
                {
                    if (lUserType == "TE" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
                    {
                        lSubmission = "Yes";
                        lEditable = "Yes";
                    }
                }

                if (lSubmission != "Yes")
                {
                    lResponseText = "You have insufficient permission to submit the order. Please contact NatSteel coordinator.";
                    lReturn = false;
                }

                if (lReturn == true)
                {
                    var lSE = db.OrderProjectSE.Find(submitOrderPE.pOrderNo, submitOrderPE.pStructureElement, submitOrderPE.pProductType, submitOrderPE.pScheduledProd);

                    if (lSE.OrderStatus != "Created" && lSE.OrderStatus != "Created*" && lSE.OrderStatus != "Sent")
                    {
                        lResponseText = "Order had been processed already. Please contact NatSteel coordinator for the order submission.";
                        lReturn = false;
                    }

                    var lNewSE = lSE;
                    lNewSE.OrderStatus = "Submitted";
                    lNewSE.PONumber = submitOrderPE.pPONo;
                    lNewSE.RequiredDate = lReqDate;
                    lNewSE.TransportMode = submitOrderPE.pTransport;

                    db.Entry(lSE).CurrentValues.SetValues(lNewSE);

                    if (lSE.CABJobID > 0)
                    {
                        int lJobID = lSE.CABJobID;
                        var lCAB = db.JobAdvice.Find(submitOrderPE.pCustomerCode, submitOrderPE.pProjectCode, lJobID);
                        var lNewCAB = lCAB;
                        lNewCAB.OrderStatus = "Submitted";
                        lNewCAB.UpdateBy = lUserID;
                        lNewCAB.UpdateDate = DateTime.Now;

                        db.Entry(lCAB).CurrentValues.SetValues(lNewCAB);
                    }
                    if (lSE.MESHJobID > 0)
                    {
                        int lJobID = lSE.MESHJobID;
                        var lMESH = db.CTSMESHJobAdvice.Find(submitOrderPE.pCustomerCode, submitOrderPE.pProjectCode, lJobID);
                        var lNewMESH = lMESH;
                        lNewMESH.OrderStatus = "Submitted";
                        lNewMESH.UpdateBy = lUserID;
                        lNewMESH.UpdateDate = DateTime.Now;

                        db.Entry(lMESH).CurrentValues.SetValues(lNewMESH);
                    }
                    if (lSE.BPCJobID > 0)
                    {
                        int lJobID = lSE.BPCJobID;
                        var lBPC = db.BPCJobAdvice.Find(submitOrderPE.pCustomerCode, submitOrderPE.pProjectCode, false, lJobID);
                        var lNewBPC = lBPC;
                        lNewBPC.OrderStatus = "Submitted";
                        lNewBPC.UpdateBy = lUserID;
                        lNewBPC.UpdateDate = DateTime.Now;

                        db.Entry(lBPC).CurrentValues.SetValues(lNewBPC);
                    }
                    if (lSE.CageJobID > 0)
                    {
                        int lJobID = lSE.CageJobID;
                        var lPRC = db.PRCJobAdvice.Find(submitOrderPE.pCustomerCode, submitOrderPE.pProjectCode, lJobID);

                        var lNewPRC = lPRC;
                        lNewPRC.OrderStatus = "Submitted";
                        lNewPRC.UpdateBy = lUserID;
                        lNewPRC.UpdateDate = DateTime.Now;

                        db.Entry(lPRC).CurrentValues.SetValues(lNewPRC);
                    }

                    if (lSE.StdBarsJobID > 0 || lSE.StdMESHJobID > 0 || lSE.CoilProdJobID > 0)
                    {
                        int lJobID = 0;
                        if (lSE.StdBarsJobID > 0)
                        {
                            lJobID = lSE.StdBarsJobID;
                        }
                        if (lSE.StdMESHJobID > 0)
                        {
                            lJobID = lSE.StdMESHJobID;
                        }
                        if (lSE.CoilProdJobID > 0)
                        {
                            lJobID = lSE.CoilProdJobID;
                        }

                        var lStd = db.StdSheetJobAdvice.Find(submitOrderPE.pCustomerCode, submitOrderPE.pProjectCode, lJobID);
                        var lNewStd = lStd;
                        //lNewStd.OrderSource = "UX";
                        lNewStd.OrderStatus = "Submitted";
                        lNewStd.UpdateBy = lUserID;
                        lNewStd.UpdateDate = DateTime.Now;

                        db.Entry(lStd).CurrentValues.SetValues(lNewStd);
                    }
                    db.SaveChanges();

                    //No email sending if natsteel staff created and processed Standard products.
                    var lNeedEmail = 1;
                    var lStandardProd = 0;
                    if (lUserType != "CU" && lUserType != "CA" && lUserType != "CM")
                    {
                        var lOrderSE = (from p in db.OrderProjectSE
                                        where p.OrderNumber == submitOrderPE.pOrderNo
                                        select p).ToList();
                        if (lOrderSE != null && lOrderSE.Count > 0)
                        {
                            for (int i = 0; i < lOrderSE.Count; i++)
                            {
                                if (lOrderSE[i].ProductType == "STANDARD-MESH" ||
                                    lOrderSE[i].ProductType == "STANDARD-BAR" ||
                                    lOrderSE[i].ProductType == "COIL")
                                {
                                    lStandardProd = 1;
                                    break;
                                }
                            }
                        }
                        if (lUserID.Split('@')[1].ToLower() == "natsteel.com.sg"
                            && lStandardProd == 1)
                        {
                            lNeedEmail = 0;
                        }
                    }

                    //send prompting email
                    if (lNeedEmail == 1)
                    {
                        var lEmailObj = new SendGridEmail();
                        lEmailObj.sendOrderActionEmail(submitOrderPE.pCustomerCode, submitOrderPE.pProjectCode, submitOrderPE.pOrderNo, "Submitted", "Created", lUserID, 0, submitOrderPE.pStructureElement, submitOrderPE.pProductType, submitOrderPE.pScheduledProd);
                        lEmailObj = null;
                    }
                }
            }
            catch (Exception ex)
            {
                //SaveErrorMsg(ex.Message, ex.StackTrace);
                lReturn = false;
                lResponseText = "Error:" + ex.Message;
            }
            return Ok(lReturn);
        }


        [HttpGet]
        [Route("/ProductSelect/{pCustomerCode}/{pProjectCode}/{UserName}")]
        public async Task<IActionResult> ProductSelect(string pCustomerCode, string pProjectCode, string UserName)
        {
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;
            string lSQL = "";

            //var lOCmd = new OracleCommand();
            //var lOcisCon = new OracleConnection();
            //OracleDataReader lORst;

            UserAccessController lUa = new UserAccessController();
            var lUserType = lUa.getUserType(UserName);//"Vishal.Wani@natsteel.com.sg";//
            var lGroupName = lUa.getGroupName(lUserType);

            lUa = null;

            ViewBag.UserType = lUserType;

            string lUserName = UserName;//User.Identity.GetUserName();
            //if (lUserName.IndexOf("@") > 0)
            //{
            //    lUserName = lUserName.Substring(0, lUserName.IndexOf("@"));
            //}

            ViewBag.UserName = lUserName;

            var lSubmission = "No";
            var lEditable = "No";

            //get Access right;
            if (lUserType == "CU" || lUserType == "CA" || lUserType == "CM")
            {
                var lAccess = db.UserAccess.Find(User.Identity.Name, pCustomerCode, pProjectCode);
                if (lAccess != null)
                {
                    lSubmission = lAccess.OrderSubmission.Trim();
                    lEditable = lAccess.OrderCreation.Trim();
                }
            }
            else
            {
                if (lUserType == "TE" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
                {
                    lSubmission = "Yes";
                    lEditable = "Yes";
                }
            }

            ViewBag.Submission = lSubmission;
            ViewBag.Editable = lEditable;

            ViewBag.AlertMessage = new List<string>();
            //var lSharedPrg = new SharedAPIController();
            //ViewBag.AlertMessage = lSharedPrg.getAlertMessage(pCustomerCode, pProjectCode, lUserName, lSubmission, lEditable);
            //lSharedPrg = null;

            var lProjectTitle = "";
            var lProcessObj = new ProcessController();
            lProcessObj.OpenNDSConnection(ref lNDSCon);

            //2020-07-27
            // Check if order created
            int lOrderCreated = 0;
            //var lOrderNoA =pOrderNo.Split(',');
            //if (lOrderNoA.Length > 0)
            //{
            //    for (int i = 0; i < lOrderNoA.Length; i++)
            //    {
            //        if (lOrderNoA[i] != null && lOrderNoA[i].Trim() != "" && lOrderNoA[i].Trim() != "0")
            //        {
            //            lOrderCreated = 1;
            //            break;
            //        }
            //    }
            //}

            if (lOrderCreated == 0)
            {
                SharedAPIController lBackEnd = new SharedAPIController();

                var lCustSelectList = lBackEnd.getCustomerSelectList(pCustomerCode, lUserType, lGroupName);

                ViewBag.CustomerSelection = lCustSelectList;

                if (pCustomerCode.Length == 0)
                {
                    pCustomerCode = lCustSelectList.First().Value;
                    if (pCustomerCode == null)
                    {
                        pCustomerCode = "";
                    }
                }

                var lProjSelectList = lBackEnd.getProjectSelectList(pCustomerCode, pProjectCode, lUserType, lGroupName);
                ViewBag.ProjectSelection = lProjSelectList;

                lBackEnd = null;
            }
            else
            {
                CustomerModels lCustomerModel = db.Customer.Find(pCustomerCode);

                //ViewBag.CustomerSelection = new SelectList(new List<SelectListItem>
                //    { new SelectListItem
                //    {
                //        Value = pCustomerCode,
                //        Text = lCustomerModel == null? "":lCustomerModel.CustomerName
                //    }
                //    }, "Value", "Text");

                //lSQL = "SELECT(NAME1 || NAME2) AS SHIP_TO_NAME, KUNNR AS SHIP_TO_PARTY FROM SAPSR3.KNA1 " +
                //    "WHERE KTOKD = 'Y001' AND MANDT ='" + lProcessObj.strClient + "' " +
                //    "AND KUNNR = '" + pProjectCode + "' ";
                lSQL = "SELECT ISNULL(project_name, '') + ISNULL(project_name2, '') AS SHIP_TO_NAME, project_code AS " +
                    "SHIP_TO_PARTY FROM HMIProjectMaster WHERE project_code = '" + pProjectCode + "';";


                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    lCmd.CommandText = lSQL;
                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lProjectTitle = lRst.GetString(0);
                        }
                    }
                    lRst.Close();
                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }

                //ViewBag.ProjectSelection = new SelectList(new List<SelectListItem>
                //{ new SelectListItem
                //{
                //    Value = pProjectCode,
                //    Text = lProjectTitle==null?"":lProjectTitle.Trim()
                //}
                //}, "Value", "Text");
            }
            List<ProjectSelect> lSEProdB = new List<ProjectSelect>();
            //ViewBag.SelectedCount = pSelectedCount;
            //ViewBag.SelectedSE = pSelectedSE.Split(',');
            //ViewBag.SelectedProd = pSelectedProd.Split(',');
            //ViewBag.SelectedPostID = pSelectedPostID.Split(',');
            //ViewBag.SelectedScheduled = pSelectedScheduled.Split(',');

            //ViewBag.SelectedWT = pSelectedWT == null ? new string[] { } : pSelectedWT.Split(',');
            //ViewBag.SelectedQty = pSelectedQty == null ? new string[] { } : pSelectedQty.Split(',');

            //ViewBag.SelectedWBS1 = pSelectedWBS1.Split(',');
            //ViewBag.SelectedWBS2 = pSelectedWBS2.Split(',');
            //ViewBag.SelectedWBS3 = pSelectedWBS3.Split(',');

            //ViewBag.WBS1 = pWBS1.Split(',');
            //ViewBag.WBS2 = pWBS2.Split(',');
            //ViewBag.WBS3 = pWlSEProdBBS3.Split(',');
            //ViewBag.OrderNo = pOrderNo.Split(',');
            ProjectSelect ProjectSelectitem = new ProjectSelect();

            //ProjectSelectitem.pSelectedCount = projectSelectModel.pSelectedCount;
            //ProjectSelectitem.pSelectedSE = projectSelectModel.pSelectedSE;
            //ProjectSelectitem.pSelectedProd = projectSelectModel.pSelectedProd;
            //ProjectSelectitem.pSelectedPostID = projectSelectModel.pSelectedPostID;
            //ProjectSelectitem.pSelectedSE = projectSelectModel.pSelectedSE;
            //ProjectSelectitem.pSelectedSE = projectSelectModel.pSelectedSE;
            //ProjectSelectitem.pSelectedSE = projectSelectModel.pSelectedSE;
            //ProjectSelectitem.pSelectedSE = projectSelectModel.pSelectedSE;
            //ProjectSelectitem.pSelectedSE = projectSelectModel.pSelectedSE;
            //ProjectSelectitem.pSelectedSE = projectSelectModel.pSelectedSE;
            //ProjectSelectitem.pSelectedSE = projectSelectModel.pSelectedSE;
            //ProjectSelectitem.pSelectedSE = projectSelectModel.pSelectedSE;

            //lSEProdB.Add(ProjectSelectitem);





            //Structure Element from WBS
            var lWBSStructEle = "";

            lSQL = "SELECT DISTINCT vchStructureElementType, " +
            "case when vchStructureElementType = 'Beam' then 1 " +
            "when vchStructureElementType = 'Column'  then 2 " +
            "when vchStructureElementType = 'Slab' then 3 " +
            "when vchStructureElementType = 'Slab-B' then 4 " +
            "when vchStructureElementType = 'Slab-T' then 5 " +
            "when vchStructureElementType = 'DWALL' then 6 " +
            "when vchStructureElementType = 'Pile' then 7 " +
            "when vchStructureElementType = 'Wall' then 8 " +
            "else S.intStructureElementTypeId end as TypeID " +
            "FROM dbo.StructureElementMaster S, " +
            "dbo.WBSElementsDetails D, " +
            "dbo.WBSElements E, " +
            "dbo.WBS W, " +
            "dbo.SAPProjectMaster P " +
            "WHERE " +
            "P.intProjectId = W.intProjectId " +
            "AND W.intWBSId = E.intWBSId " +
            "AND E.intWBSElementId = D.intWBSElementId " +
            //Commented by chetan
            //"AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
            "AND P.vchProjectCode = '" + pProjectCode + "' " +
            "AND S.tntStatusId = 1 " +
            "AND S.vchStructureElementType in ('Beam', 'Column', 'Slab', 'Slab-B', 'Slab-T', 'DWALL', 'Pile', 'Wall') " +
            "ORDER BY TypeID ";

            lCmd.CommandText = lSQL;
            lCmd.Connection = lNDSCon;
            lCmd.CommandTimeout = 300;
            lRst = lCmd.ExecuteReader();
            if (lRst.HasRows)
            {
                while (lRst.Read())
                {
                    if (lWBSStructEle == "")
                    {
                        lWBSStructEle = lRst.GetString(0).Trim();
                    }
                    else
                    {
                        lWBSStructEle = lWBSStructEle + "," + lRst.GetString(0).Trim();
                    }
                }
            }
            lRst.Close();

            if (lWBSStructEle == "")
            {
                lWBSStructEle = "NONWBS";
            }
            else
            {
                lWBSStructEle = lWBSStructEle + ",NONWBS";
            }

            //ViewBag.WBSSE = lWBSStructEle;

            var lWBSSEa = lWBSStructEle.Split(',');
            var lWBSSEChinesea = new List<string>();
            if (lWBSSEa.Length > 0)
            {
                for (int i = 0; i < lWBSSEa.Length; i++)
                {
                    lWBSSEChinesea.Add(ChineseCovertSE(lWBSSEa[i]));
                }
            }
            ViewBag.WBSSEChinese = lWBSSEChinesea.ToArray();

            var lProdStr = getProjectType(pCustomerCode, pProjectCode);

            //Three dimension array
            // [][0]   - SE Code
            // [][1][] - product code
            // [][2][] - product name

            var lSEProdA = lWBSStructEle.Split(',').ToList();



            if (lSEProdA.Count > 0)
            {
                for (int i = 0; i < lSEProdA.Count; i++)
                {
                    if (lSEProdA[i].ToUpper() == "BEAM")
                    {
                        var lProdCode = new List<string>();
                        var lProdName = new List<string>();
                        if (lProdStr.IndexOf("CAB,0") < 0)
                        {
                            lProdCode.Add("cab");
                            lProdName.Add("Cut & Bend");
                        }
                        //if (lProdStr.IndexOf("MESH,0") < 0)
                        //{
                        //    lProdCode.Add("stirrup-link-mesh");
                        //    lProdName.Add("Stirrup Link Mesh");
                        //}
                        //if (lProdStr.IndexOf("CAGE,0") < 0)
                        //{
                        //    lProdCode.Add("pre-cage");
                        //    lProdName.Add("Pre-Cages");
                        //}

                        if (lProdStr.IndexOf("REBAR,0") < 0)
                        {
                            lProdCode.Add("standard-bar");
                            lProdName.Add("Standard Bar");
                        }

                        lSEProdB.Add(new ProjectSelect
                        {
                            SECode = lSEProdA[i].ToUpper(),
                            ProdCode = lProdCode,
                            ProdName = lProdName
                        }
                        );
                    }
                    else if (lSEProdA[i].ToUpper() == "COLUMN")
                    {
                        var lProdCode = new List<string>();
                        var lProdName = new List<string>();
                        if (lProdStr.IndexOf("CAB,0") < 0)
                        {
                            lProdCode.Add("cab");
                            lProdName.Add("Cut & Bend");
                        }
                        //if (lProdStr.IndexOf("MESH,0") < 0)
                        //{
                        //    lProdCode.Add("column-link-mesh");
                        //    lProdName.Add("Column Link Mesh");
                        //}
                        //if (lProdStr.IndexOf("CAGE,0") < 0)
                        //{
                        //    lProdCode.Add("pre-cage");
                        //    lProdName.Add("Pre-Cages");
                        //}
                        if (lProdStr.IndexOf("REBAR,0") < 0)
                        {
                            lProdCode.Add("standard-bar");
                            lProdName.Add("Standard Bar");
                        }

                        lSEProdB.Add(new ProjectSelect
                        {
                            SECode = lSEProdA[i].ToUpper(),
                            ProdCode = lProdCode,
                            ProdName = lProdName
                        }
                        );
                    }
                    else if (lSEProdA[i].ToUpper() == "SLAB")
                    {
                        var lProdCode = new List<string>();
                        var lProdName = new List<string>();
                        if (lProdStr.IndexOf("CAB,0") < 0)
                        {
                            lProdCode.Add("cab");
                            lProdName.Add("Cut & Bend");
                        }
                        //if (lProdStr.IndexOf("MESH,0") < 0)
                        //{
                        //    lProdCode.Add("cut-to-size-mesh");
                        //    lProdName.Add("Cut-To-Size Mesh");
                        //}
                        //if (lProdStr.IndexOf("CAGE,0") < 0)
                        //{
                        //    lProdCode.Add("pre-cage");
                        //    lProdName.Add("Pre-Cages");
                        //}
                        if (lProdStr.IndexOf("MESH,0") < 0)
                        {
                            lProdCode.Add("standard-mesh");
                            lProdName.Add("Standard Mesh");
                        }
                        if (lProdStr.IndexOf("REBAR,0") < 0)
                        {
                            lProdCode.Add("standard-bar");
                            lProdName.Add("Standard Bar");
                        }

                        lSEProdB.Add(new ProjectSelect
                        {
                            SECode = lSEProdA[i].ToUpper(),
                            ProdCode = lProdCode,
                            ProdName = lProdName
                        }
                        );
                    }
                    else if (lSEProdA[i].ToUpper() == "SLAB-B")
                    {
                        var lProdCode = new List<string>();
                        var lProdName = new List<string>();
                        if (lProdStr.IndexOf("CAB,0") < 0)
                        {
                            lProdCode.Add("cab");
                            lProdName.Add("Cut & Bend");
                        }
                        //if (lProdStr.IndexOf("MESH,0") < 0)
                        //{
                        //    lProdCode.Add("cut-to-size-mesh");
                        //    lProdName.Add("Cut-To-Size Mesh");
                        //}
                        //if (lProdStr.IndexOf("CAGE,0") < 0)
                        //{
                        //    lProdCode.Add("pre-cage");
                        //    lProdName.Add("Pre-Cages");
                        //}
                        if (lProdStr.IndexOf("MESH,0") < 0)
                        {
                            lProdCode.Add("standard-mesh");
                            lProdName.Add("Standard Mesh");
                        }
                        if (lProdStr.IndexOf("REBAR,0") < 0)
                        {
                            lProdCode.Add("standard-bar");
                            lProdName.Add("Standard Bar");
                        }

                        lSEProdB.Add(new ProjectSelect
                        {
                            SECode = lSEProdA[i].ToUpper(),
                            ProdCode = lProdCode,
                            ProdName = lProdName
                        }
                        );
                    }
                    else if (lSEProdA[i].ToUpper() == "SLAB-T")
                    {
                        var lProdCode = new List<string>();
                        var lProdName = new List<string>();
                        if (lProdStr.IndexOf("CAB,0") < 0)
                        {
                            lProdCode.Add("cab");
                            lProdName.Add("Cut & Bend");
                        }
                        //if (lProdStr.IndexOf("MESH,0") < 0)
                        //{
                        //    lProdCode.Add("cut-to-size-mesh");
                        //    lProdName.Add("Cut-To-Size Mesh");
                        //}
                        //if (lProdStr.IndexOf("CAGE,0") < 0)
                        //{
                        //    lProdCode.Add("pre-cage");
                        //    lProdName.Add("Pre-Cages");
                        //}
                        if (lProdStr.IndexOf("MESH,0") < 0)
                        {
                            lProdCode.Add("standard-mesh");
                            lProdName.Add("Standard Mesh");
                        }
                        if (lProdStr.IndexOf("REBAR,0") < 0)
                        {
                            lProdCode.Add("standard-bar");
                            lProdName.Add("Standard Bar");
                        }

                        lSEProdB.Add(new ProjectSelect
                        {
                            SECode = lSEProdA[i].ToUpper(),
                            ProdCode = lProdCode,
                            ProdName = lProdName
                        }
                        );
                    }
                    else if (lSEProdA[i].ToUpper() == "DWALL")
                    {
                        var lProdCode = new List<string>();
                        var lProdName = new List<string>();
                        if (lProdStr.IndexOf("CAB,0") < 0)
                        {
                            lProdCode.Add("cab");
                            lProdName.Add("Cut & Bend");
                        }
                        //if (lProdStr.IndexOf("CAGE,0") < 0)
                        //{
                        //    lProdCode.Add("pre-cage");
                        //    lProdName.Add("Pre-Cages");
                        //}

                        lSEProdB.Add(new ProjectSelect
                        {
                            SECode = lSEProdA[i].ToUpper(),
                            ProdCode = lProdCode,
                            ProdName = lProdName
                        }
                        );
                    }
                    else if (lSEProdA[i].ToUpper() == "PILE")
                    {
                        var lProdCode = new List<string>();
                        var lProdName = new List<string>();
                        //if (lProdStr.IndexOf("BPC,0") < 0)
                        //{
                        //    lProdCode.Add("bpc");
                        //    lProdName.Add("BPC");
                        //}
                        if (lProdStr.IndexOf("CAB,0") < 0)
                        {
                            lProdCode.Add("cab");
                            lProdName.Add("Cut & Bend");
                        }
                        //if (lProdStr.IndexOf("MESH,0") < 0)
                        //{
                        //    lProdCode.Add("cut-to-size-mesh");
                        //    lProdName.Add("Cut-To-Size Mesh");
                        //}
                        if (lProdStr.IndexOf("MESH,0") < 0)
                        {
                            lProdCode.Add("standard-mesh");
                            lProdName.Add("Standard Mesh");
                        }
                        if (lProdStr.IndexOf("REBAR,0") < 0)
                        {
                            lProdCode.Add("standard-bar");
                            lProdName.Add("Standard Bar");
                        }

                        lSEProdB.Add(new ProjectSelect
                        {
                            SECode = lSEProdA[i].ToUpper(),
                            ProdCode = lProdCode,
                            ProdName = lProdName
                        }
                        );
                    }
                    else if (lSEProdA[i].ToUpper() == "WALL")
                    {
                        var lProdCode = new List<string>();
                        var lProdName = new List<string>();
                        if (lProdStr.IndexOf("CAB,0") < 0)
                        {
                            lProdCode.Add("cab");
                            lProdName.Add("Cut & Bend");
                        }
                        //if (lProdStr.IndexOf("MESH,0") < 0)
                        //{
                        //    lProdCode.Add("cut-to-size-mesh");
                        //    lProdName.Add("Cut-To-Size Mesh");
                        //}
                        //if (lProdStr.IndexOf("CAGE,0") < 0)
                        //{
                        //    lProdCode.Add("pre-cage");
                        //    lProdName.Add("Pre-Cages");
                        //}
                        if (lProdStr.IndexOf("MESH,0") < 0)
                        {
                            lProdCode.Add("standard-mesh");
                            lProdName.Add("Standard Mesh");
                        }
                        if (lProdStr.IndexOf("REBAR,0") < 0)
                        {
                            lProdCode.Add("standard-bar");
                            lProdName.Add("Standard Bar");
                        }

                        lSEProdB.Add(new ProjectSelect
                        {
                            SECode = lSEProdA[i].ToUpper(),
                            ProdCode = lProdCode,
                            ProdName = lProdName
                        }
                        );
                    }
                    else if (lSEProdA[i].ToUpper() == "NONWBS")
                    {
                        var lProdCode = new List<string>();
                        var lProdName = new List<string>();
                        if (lProdStr.IndexOf("CAB,0") < 0 || lProdStr.IndexOf("REBAR,0") < 0)
                        {
                            lProdCode.Add("standard-bar");
                            lProdName.Add("Standard Bar");
                        }
                        if (lProdStr.IndexOf("MESH,0") < 0)
                        {
                            lProdCode.Add("standard-mesh");
                            lProdName.Add("Standard / Sample Mesh");
                        }
                        //if (lProdStr.IndexOf("WR,0") < 0 || lProdStr.IndexOf("REBAR,0") < 0)
                        if (lProdStr.IndexOf("COIL,0") < 0)
                        {
                            lProdCode.Add("coil");
                            lProdName.Add("Coil Products");
                        }
                        if (lProdStr.IndexOf("BPC,0") < 0)
                        {
                            lProdCode.Add("bpc");
                            lProdName.Add("BPC");
                        }
                        if (lProdStr.IndexOf("MESH,0") < 0)
                        {
                            lProdCode.Add("cut-to-size-mesh");
                            lProdName.Add("Cut-To-Size Mesh");
                        }

                        //if (lProdStr.IndexOf("COUPLER,0") < 0 && lUserName.ToLower().IndexOf("natsteel.com.sg") > 0)
                        if (lProdStr.IndexOf("COUPLER,0") < 0)
                        {
                            lProdCode.Add("coupler");
                            lProdName.Add("Coupler");
                        }

                        if (lProdStr.IndexOf("MESH,0") < 0)
                        {
                            lProdCode.Add("component");
                            lProdName.Add("Component");
                        }

                        lSEProdB.Add(new ProjectSelect
                        {
                            SECode = lSEProdA[i].ToUpper(),
                            ProdCode = lProdCode,
                            ProdName = lProdName
                        }
                        );
                    }
                }
            }

            ViewData["SEProd"] = lSEProdB;

            lProcessObj.CloseNDSConnection(ref lNDSCon);
            lProcessObj = null;

            var result = lSEProdB;
            return Ok(result);
        }


        //HMI updated Function
        [HttpGet]
        [Route("/getProjectType/{pCustomerCode}/{pProjectCode}")]
        public string getProjectType(string pCustomerCode, string pProjectCode)
        {
            var lProcessObj = new ProcessController();
            try
            {
                //OracleDataReader lRst;
                //var lCmd = new OracleCommand();
                //var lcisCon = new OracleConnection();
                SqlDataReader lNDSRst;
                var lNDSCmd = new SqlCommand();
                var lNDSCon = new SqlConnection();
                int lSpotFound = 0;
                string lReturn = "CAB,1,MESH,1,REBAR,1,BPC,1,CAGE,1,WR,1,COUPLER,1";
                lProcessObj.OpenNDSConnection(ref lNDSCon);
                lNDSCmd.CommandText = "SELECT ProjectTitle, ProjectCode " +
                "FROM dbo.OESProject " +
                "WHERE CustomerCode = '0000000000' AND ProjectCode <> '0000000000' " +
                "AND ProjectCode = '" + pProjectCode + "' ";
                lNDSCmd.Connection = lNDSCon;
                lNDSCmd.CommandTimeout = 1200;
                lNDSRst = lNDSCmd.ExecuteReader();
                if (lNDSRst.HasRows)
                {
                    if (lNDSRst.Read())
                    {
                        lReturn = "CAB,0,"
                        + "MESH,1"
                        + "REBAR,0,"
                        + "BPC,0,"
                        + "CAGE,0,"
                        + "WR,0,"
                        + "COUPLER,0";
                        lSpotFound = 1;
                    }
                }
                lNDSRst.Close();

               // HMI Code added 
                if (lSpotFound == 0)
                {
                   List<string> productcodes = new List<string>();
                   List<double> productQty = new List<double>();
                    lNDSCmd.CommandText = $@"
                     SELECT 
                        PROD_CLASS_CODE, 
                        SUM(CAST(PROD_QTY AS FLOAT)) AS TOTAL_QTY 
                     FROM 
                        HMIContractItem 
                     WHERE 
                        CONTRACT_NO IN (
                            SELECT 
                                CM.CONTRACT_NO 
                            FROM 
                                HMIContractMaster CM 
                             INNER JOIN 
                                 HMIContractProject CP 
                                 ON CM.CONTRACT_NO = CP.CONTRACT_NO 
                             WHERE 
                                ((CM.con_status='Confirmed' and CM.CONTRACT_STATUS='Confirmed') or (CM.con_status='Confirmed' and CM.CONTRACT_STATUS is NULL)) 
                                 AND CM.CUST_ID = '{pCustomerCode}' 
                                 AND CP.PROJECT_CODE = '{pProjectCode}'
                         ) 
                     GROUP BY 
                         PROD_CLASS_CODE";


                    //lNDSCmd.Parameters.AddWithValue("@CustId", pCustomerCode);
                    //lNDSCmd.Parameters.AddWithValue("@ProjectCode", pProjectCode);

                    lNDSCmd.Connection = lNDSCon;
                    lNDSCmd.CommandTimeout = 1200;
                    lNDSRst = lNDSCmd.ExecuteReader();
                    if (lNDSRst.HasRows)
                    {
                        while (lNDSRst.Read())
                        {
                            productcodes.Add(lNDSRst.GetValue(0) == DBNull.Value ? "" : lNDSRst.GetString(0).Trim());
                            productQty.Add(lNDSRst.GetValue(1) == DBNull.Value ? 0 : Convert.ToDouble(lNDSRst.GetValue(1)));
                        }

                        StringBuilder tempString = new StringBuilder();
                        int totalItems = productcodes.Count;

                        var lCAB = "CAB,0,";
                        var lMESH = "MESH,0,";
                        var lREBAR = "REBAR,0,";
                        var lBPC = "BPC,0,";
                        var lCAGE = "CAGE,0,";
                        var lWR = "COIL,0,";
                        var lCOUPLER = "COUPLER,0,";

                        for (int i = 0; i < totalItems; i++)
                        {
                            if (productcodes[i] == "CAB")
                            {
                                lCAB = $"CAB,{productQty[i]},";
                            }
                            if (productcodes[i] == "MSH")
                            {
                                lMESH = $"MESH,{productQty[i]},";
                            }
                            if (productcodes[i] == "BAR" || productcodes[i] == "PBAR")
                            {
                                lREBAR = $"REBAR,{productQty[i]},";
                            }
                            if (productcodes[i] == "BPC")
                            {
                                lBPC = $"BPC,{productQty[i]},";
                            }
                            if (productcodes[i] == "CAR")
                            {
                                lCAGE = $"CAGE,{productQty[i]},";
                            }
                            if (productcodes[i] == "WRD" || 
                                productcodes[i] == "CRW" || 
                                productcodes[i] == "TR" || 
                                productcodes[i] == "DBIC" || 
                                productcodes[i] == "SDBIC" || 
                                productcodes[i] == "PBIC")
                            {
                                lWR = $"COIL,{productQty[i]},";
                            }
                            if (productcodes[i] == "COU")
                            {
                                lCOUPLER = $"COUPLER,{productQty[i]},";
                            }
                            //tempString.AppendFormat(",{0},{1}", productcodes[i], productQty[i]);
                        }
                        //lReturn = tempString.Length > 0 ? tempString.ToString().Substring(1) : lReturn;
                        lReturn = lCAB + lMESH + lREBAR + lBPC + lCAGE + lWR + lCOUPLER;
                    }
                    else
                    {
                        lReturn = "CAB,0,MESH,0,REBAR,0,BPC,0,CAGE,0,COIL,0,COUPLER,0";
                    }
                    lNDSRst.Close();

                }

                lProcessObj.CloseNDSConnection(ref lNDSCon);

                //if (lSpotFound == 0)
                //{
                //    if (lProcessObj.OpenCISConnection(ref lcisCon) == true)
                //    {
                //        lCmd.CommandText = "SELECT NVL(SUM(K.ytot_cab),0), " +
                //            "NVL(SUM(K.ytot_mesh),0), " +
                //            "NVL(SUM(K.ytot_rebar),0), " +
                //            "NVL(SUM(K.ytot_bpc),0), " +
                //            "NVL(SUM(K.ytot_precage),0), " +
                //            "NVL(SUM(K.YTOTAL_WR),0) + NVL(SUM(K.YTOT_COLD_ROLL),0) + NVL(SUM(K.YTOTAL_PCSTRAND),0), " +
                //            "NVL(SUM(TO_NUMBER(NVL(K.ytot_coupler,'0'))),0) " +
                //                "FROM SAPSR3.VBAK K, SAPSR3.VBPA P " +
                //                "WHERE K.MANDT = '" + lProcessObj.strClient + "' " +
                //                "AND (K.VBELN like 'NSH%' OR K.VBELN like '102%' OR K.VBELN like '_102%' OR K.VBELN like '112%' OR K.VBELN like '_112%') " +
                //                "AND (K.VKORG = '" + lProcessObj.strSalesOrg + "' " +
                //                "OR K.VKORG = '" + lProcessObj.strSalesExport + "') " +
                //                "AND K.KUNNR = '" + pCustomerCode + "' " +
                //                "AND K.TRVOG = '4' " +
                //                "AND K.MANDT = P.MANDT " +
                //                "AND K.VBELN = P.VBELN " +
                //                "AND P.KUNNR = '" + pProjectCode + "' " +//Commented by chetan
                //                "AND to_date(K.GUEEN,'YYYYMMDD') >= (SYSDATE - 120) ";//30 to 120 converted by chetan
                //        lCmd.Connection = lcisCon;
                //        lCmd.CommandTimeout = 1200;
                //        lRst = lCmd.ExecuteReader();
                //        if (lRst.HasRows)
                //        {
                //            if (lRst.Read())
                //            {
                //                lReturn = "CAB," + lRst.GetDecimal(0).ToString() + ","
                //                + "MESH," + lRst.GetDecimal(1).ToString() + ","
                //                + "REBAR," + lRst.GetDecimal(2).ToString() + ","
                //                + "BPC," + lRst.GetDecimal(3).ToString() + ","
                //                + "CAGE," + lRst.GetDecimal(4).ToString() + ","
                //                + "WR," + lRst.GetDecimal(5).ToString() + ","
                //                + "COUPLER," + lRst.GetDecimal(6).ToString();
                //            }
                //        }
                //        lRst.Close();
                //    }
                //    lProcessObj.CloseCISConnection(ref lcisCon);
                //}

                //lCmd = null;
                //lRst = null;
                //lcisCon = null;
                lNDSCmd = null;
                lNDSRst = null;
                lNDSCon = null;
                lProcessObj = null;
                return lReturn;
            }
            catch (Exception ex)
            {
                var lMsg = ex.Message;
                lProcessObj.SaveErrorMsg(ex.Message, ex.StackTrace);
                return "error: " + ex.Message;
            }
        }


        [HttpPost]
        [Route("/OrderSummary/{UserName}/{UserType}")]
        public async Task<ActionResult> OrderSummary([FromBody] reqOrderSumData reqorderSumData, string UserName, string UserType)
        {
            var lProcessObj = new ProcessController();
            try
            {
                var lCmd = new SqlCommand();
                SqlDataReader lRst;
                var lNDSCon = new SqlConnection();
                string lSQL = "";

                //var lOCmd = new OracleCommand();
                //var lOcisCon = new OracleConnection();
                //OracleDataReader lORst;

                decimal lTotalWT = 0;
                int lTransportLimit = 0;
                string lPEStatus = "";
                int lStatusDiff = 0;

                UserAccessController lUa = new UserAccessController();
                var lUserType = UserType;//"Vishal.Wani@natsteel.com.sg";
                var lGroupName = lUa.getGroupName(lUserType);

                var lSubmission = "No";
                var lEditable = "No";
                var lPlanEntCompl = "Yes";

                var lOrderList = new OrderSummaryData();

                if (reqorderSumData.pOrderNo == null)
                {
                    reqorderSumData.pOrderNo = "";
                }
                if (reqorderSumData.pSelectedCount == null)
                {
                    reqorderSumData.pSelectedCount = 0;
                }
                if (reqorderSumData.pSelectedSE == null)
                {
                    reqorderSumData.pSelectedSE = "";
                }
                if (reqorderSumData.pSelectedProd == null)
                {
                    reqorderSumData.pSelectedProd = "";
                }

                reqorderSumData.pOrderNo = reqorderSumData.pOrderNo.Trim();


                //ViewBag.UserType = lUserType;

                string lUserName = UserName;//"Vishal.Wani@natsteel.com.sg";

                //ViewBag.UserName = lUserName;

                lUa = null;

                //get Access right;
                if (lUserType == "CU" || lUserType == "CA" || lUserType == "CM")
                {
                    var lAccess = db.UserAccess.Find(User.Identity.Name, reqorderSumData.pCustomerCode, reqorderSumData.pProjectCode);
                    if (lAccess != null)
                    {
                        lSubmission = lAccess.OrderSubmission.Trim();
                        lEditable = lAccess.OrderCreation.Trim();
                    }
                }
                else
                {
                    if (lUserType == "TE" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
                    {
                        lSubmission = "Yes";
                        lEditable = "Yes";
                        if (lUserType != "PL")
                        {
                            var lSharedAPI = new SharedAPIController();
                            int lOrderNo1 = 0;
                            int.TryParse(reqorderSumData.pOrderNo, out lOrderNo1);
                            lPlanEntCompl = lSharedAPI.checkPlanComplete(lOrderNo1, UserName);

                            lSharedAPI = null;

                        }
                    }
                }

                //ViewBag.Submission = lSubmission;
                //ViewBag.Editable = lEditable;
                //ViewBag.EntryCompl = lPlanEntCompl;
                lOrderList.lSubmission = lSubmission;
                lOrderList.lEditable = lEditable;
                lOrderList.lPlanEntCompl = lPlanEntCompl;

                ViewBag.AlertMessage = new List<string>();
                //var lSharedPrg = new SharedAPIController();
                //ViewBag.AlertMessage = lSharedPrg.getAlertMessage(pCustomerCode, pProjectCode, lUserName, lSubmission, lEditable);
                //lSharedPrg = null;

                var lUnitOrderA = reqorderSumData.pOrderNo.Split(',').Distinct().ToArray();

                //taken out 2020-07-30 as NONWBS may 
                //if (pSelectedSE.IndexOf("NONWBS") >= 0 && (lUnitOrderA.Length == 0 || 
                //(lUnitOrderA.Length == 1 && lUnitOrderA[0] == "0")))
                //{
                //    //allow select customer and project
                //    var lProjectTitle = "";

                //    SharedAPIController lBackEnd = new SharedAPIController();

                //    var lCustSelectList = lBackEnd.getCustomerSelectList(pCustomerCode, lUserType, lGroupName);

                //    ViewBag.CustomerSelection = lCustSelectList;

                //    if (pCustomerCode.Length == 0)
                //    {
                //        pCustomerCode = lCustSelectList.First().Value;
                //        if (pCustomerCode == null)
                //        {
                //            pCustomerCode = "";
                //        }
                //    }

                //    var lProjSelectList = lBackEnd.getProjectSelectList(pCustomerCode, pProjectCode, lUserType, lGroupName);
                //    ViewBag.ProjectSelection = lProjSelectList;

                //    lBackEnd = null;

                //}
                //else
                //{
                //Fixed the customer code and project code
                CustomerModels lCustomerModel = db.Customer.Find(reqorderSumData.pCustomerCode);

                //ViewBag.CustomerSelection = new SelectList(new List<SelectListItem>
                //    { new SelectListItem
                //    {
                //        Value = pCustomerCode,
                //        Text = lCustomerModel == null? "":lCustomerModel.CustomerName
                //    }
                //    }, "Value", "Text");

                var lProcess = new ProcessController();
                var lProjectTitle = "";
                //lSQL = "SELECT(NAME1 || NAME2) AS SHIP_TO_NAME, KUNNR AS SHIP_TO_PARTY FROM SAPSR3.KNA1 " +
                //    "WHERE KTOKD = 'Y001' AND MANDT ='" + lProcess.strClient + "' " +
                //    "AND KUNNR = '" + reqorderSumData.pProjectCode + "' ";

                lSQL = "SELECT ISNULL(project_name, '') + ISNULL(project_name2, '') AS SHIP_TO_NAME, project_code AS " +
                  "SHIP_TO_PARTY FROM HMIProjectMaster WHERE project_code = '" + reqorderSumData.pProjectCode + "';";


                if (lProcess.OpenNDSConnection(ref lNDSCon) == true)
                {
                    lCmd.CommandText = lSQL;
                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lProjectTitle = lRst.GetString(0);
                        }
                    }
                    lRst.Close();
                    lProcess.CloseNDSConnection(ref lNDSCon);
                }
                lProcess = null;


                //ViewBag.ProjectSelection = new SelectList(new List<SelectListItem>
                //    { new SelectListItem
                //    {
                //        Value = pProjectCode,
                //        Text = lProjectTitle == null? "":lProjectTitle
                //    }
                //    }, "Value", "Text");

                ////}

                //get Order Status
                var lOrderStatus = "New";
                var lUpdateBy = "";

                //get unique Order Number list
                List<string> lOrderNoList = new List<string>();
                if (reqorderSumData.pOrderNo != "")
                {
                    var lOrderA1 = reqorderSumData.pOrderNo.Split(',').ToList();
                    if (lOrderA1.Count > 0)
                    {
                        for (int i = 0; i < lOrderA1.Count; i++)
                        {
                            if (lOrderA1[i] != null && lOrderA1[i].Trim() != "" && lOrderA1[i].Trim() != " " && lOrderA1[i].Trim() != "0")
                            {
                                if (lOrderNoList.Count == 0)
                                {
                                    lOrderNoList.Add(lOrderA1[i].Trim());
                                }
                                else
                                {
                                    if (lOrderNoList.IndexOf(lOrderA1[i].Trim()) < 0)
                                    {
                                        lOrderNoList.Add(lOrderA1[i].Trim());
                                    }
                                }
                            }
                        }
                    }
                }

                //cater for 3 ways come from: 1. Product Select; 2. Product Details entry; 3. from Active Order, delivered order and process 
                if (lOrderNoList.Count > 0 && reqorderSumData.pSelectedCount == 0)
                {
                    //Come from other apps and get data from db
                    reqorderSumData.pSelectedCount = 0;
                    reqorderSumData.pSelectedWBS1 = "";
                    reqorderSumData.pSelectedWBS2 = "";
                    reqorderSumData.pSelectedWBS3 = "";
                    reqorderSumData.pWBS1 = "";
                    reqorderSumData.pWBS2 = "";
                    reqorderSumData.pWBS3 = "";
                    reqorderSumData.pSelectedSE = "";
                    reqorderSumData.pSelectedProd = "";
                    reqorderSumData.pOrderNo = "";
                    reqorderSumData.pSelectedScheduled = "";
                    reqorderSumData.pSelectedPostID = "";
                    for (int k = 0; k < lOrderNoList.Count; k++)
                    {
                        int lOrderNumber = 0;
                        int.TryParse(lOrderNoList[k], out lOrderNumber);

                        if (lOrderNumber > 0)
                        {
                            var lOrderHeader = (from p in db.OrderProject
                                                where p.OrderNumber == lOrderNumber
                                                select p).ToList();


                            var lOrderSE = (from p in db.OrderProjectSE
                                            where p.OrderNumber == lOrderNumber
                                            select p).ToList();

                            if (lOrderSE.Count > 0 && lOrderHeader.Count > 0)
                            {
                                lOrderStatus = lOrderHeader[0].OrderStatus;
                                lUpdateBy = lOrderHeader[0].UpdateBy;

                                if (lOrderStatus == null) lOrderStatus = "New";
                                lOrderStatus = lOrderStatus.Trim();
                                if (lOrderStatus.Length == 0) lOrderStatus = "New";

                                // ViewBag.UpdateBy = lUpdateBy;
                                lOrderList.lUpdateBy = lUpdateBy;
                                //take from database if lost selection count
                                reqorderSumData.pSelectedCount = reqorderSumData.pSelectedCount + lOrderSE.Count;
                                for (int i = 0; i < lOrderSE.Count; i++)
                                {
                                    reqorderSumData.pSelectedWBS1 = reqorderSumData.pSelectedWBS1 + "," + (lOrderHeader[0].WBS1 == null ? "" : lOrderHeader[0].WBS1.Trim());
                                    reqorderSumData.pSelectedWBS2 = reqorderSumData.pSelectedWBS2 + "," + (lOrderHeader[0].WBS2 == null ? "" : lOrderHeader[0].WBS2.Trim());
                                    reqorderSumData.pSelectedWBS3 = reqorderSumData.pSelectedWBS3 + "," + (lOrderHeader[0].WBS3 == null ? "" : lOrderHeader[0].WBS3.Trim());

                                    reqorderSumData.pWBS1 = reqorderSumData.pWBS1 + "," + (lOrderHeader[0].WBS1 == null ? "" : lOrderHeader[0].WBS1.Trim());
                                    reqorderSumData.pWBS2 = reqorderSumData.pWBS2 + "," + (lOrderHeader[0].WBS2 == null ? "" : lOrderHeader[0].WBS2.Trim());
                                    reqorderSumData.pWBS3 = reqorderSumData.pWBS3 + "," + (lOrderHeader[0].WBS3 == null ? "" : lOrderHeader[0].WBS3.Trim());

                                    reqorderSumData.pSelectedSE = reqorderSumData.pSelectedSE + "," + (lOrderSE[i].StructureElement == null ? "NONWBS" : (lOrderSE[i].StructureElement.Trim().Length == 0 ? "NONWBS" : lOrderSE[i].StructureElement.Trim()));
                                    reqorderSumData.pSelectedProd = reqorderSumData.pSelectedProd + "," + lOrderSE[i].ProductType;
                                    reqorderSumData.pSelectedPostID = reqorderSumData.pSelectedPostID + "," + lOrderSE[i].PostHeaderID.ToString();
                                    reqorderSumData.pSelectedScheduled = reqorderSumData.pSelectedScheduled + "," + (lOrderSE[i].ScheduledProd == null ? "N" : lOrderSE[i].ScheduledProd);
                                    reqorderSumData.pOrderNo = reqorderSumData.pOrderNo + "," + lOrderSE[i].OrderNumber.ToString();
                                }
                            }
                        }
                    }
                    if (reqorderSumData.pSelectedWBS1.Length > 0)
                    {
                        reqorderSumData.pSelectedWBS1 = reqorderSumData.pSelectedWBS1.Substring(1);
                    }
                    if (reqorderSumData.pSelectedWBS2.Length > 0)
                    {
                        reqorderSumData.pSelectedWBS2 = reqorderSumData.pSelectedWBS2.Substring(1);
                    }
                    if (reqorderSumData.pSelectedWBS3.Length > 0)
                    {
                        reqorderSumData.pSelectedWBS3 = reqorderSumData.pSelectedWBS3.Substring(1);
                    }

                    if (reqorderSumData.pWBS1.Length > 0)
                    {
                        reqorderSumData.pWBS1 = reqorderSumData.pWBS1.Substring(1);
                    }
                    if (reqorderSumData.pWBS2.Length > 0)
                    {
                        reqorderSumData.pWBS2 = reqorderSumData.pWBS2.Substring(1);
                    }
                    if (reqorderSumData.pWBS3.Length > 0)
                    {
                        reqorderSumData.pWBS3 = reqorderSumData.pWBS3.Substring(1);
                    }

                    if (reqorderSumData.pSelectedSE.Length > 0)
                    {
                        reqorderSumData.pSelectedSE = reqorderSumData.pSelectedSE.Substring(1);
                    }
                    if (reqorderSumData.pSelectedProd.Length > 0)
                    {
                        reqorderSumData.pSelectedProd = reqorderSumData.pSelectedProd.Substring(1);
                    }
                    if (reqorderSumData.pSelectedPostID.Length > 0)
                    {
                        reqorderSumData.pSelectedPostID = reqorderSumData.pSelectedPostID.Substring(1);
                    }
                    if (reqorderSumData.pSelectedScheduled.Length > 0)
                    {
                        reqorderSumData.pSelectedScheduled = reqorderSumData.pSelectedScheduled.Substring(1);
                    }
                    if (reqorderSumData.pOrderNo.Length > 0)
                    {
                        reqorderSumData.pOrderNo = reqorderSumData.pOrderNo.Substring(1);
                    }
                }
                else if (reqorderSumData.pSelectedCount > 0)
                {
                    //Come from product select. check and remove delete items
                    int lCount = 0;
                    var lWBS1Ar = reqorderSumData.pSelectedWBS1.Split(',');
                    var lWBS2Ar = reqorderSumData.pSelectedWBS2.Split(',');
                    var lWBS3Ar = reqorderSumData.pSelectedWBS3.Split(',');
                    var lSEAr = reqorderSumData.pSelectedSE.Split(',');
                    var lProdTypeAr = reqorderSumData.pSelectedProd.Split(',');
                    var lScheduledProdAr = reqorderSumData.pSelectedScheduled.Split(',');

                    for (int k = 0; k < lOrderNoList.Count; k++)
                    {
                        int lOrderNumber = 0;
                        int.TryParse(lOrderNoList[k], out lOrderNumber);

                        if (lOrderNumber > 0)
                        {
                            var lOrderHeader = (from p in db.OrderProject
                                                where p.OrderNumber == lOrderNumber
                                                select p).ToList();

                            if (lOrderStatus == null) lOrderStatus = "New";
                            lOrderStatus = lOrderStatus.Trim();
                            if (lOrderStatus.Length == 0) lOrderStatus = "New";

                            ViewBag.UpdateBy = lUpdateBy;


                            var lOrderSE = (from p in db.OrderProjectSE
                                            where p.OrderNumber == lOrderNumber
                                            select p).ToList();

                            if (lOrderSE.Count > 0 && lOrderHeader.Count > 0)
                            {
                                lOrderStatus = lOrderHeader[0].OrderStatus;
                                lUpdateBy = lOrderHeader[0].UpdateBy;

                                if (lOrderHeader[0].WBS1 == null)
                                {
                                    lOrderHeader[0].WBS1 = "";
                                }
                                if (lOrderHeader[0].WBS2 == null)
                                {
                                    lOrderHeader[0].WBS2 = "";
                                }
                                if (lOrderHeader[0].WBS3 == null)
                                {
                                    lOrderHeader[0].WBS3 = "";
                                }
                                if (lOrderStatus == null) lOrderStatus = "New";
                                lOrderStatus = lOrderStatus.Trim();
                                if (lOrderStatus.Length == 0) lOrderStatus = "New";

                                ViewBag.UpdateBy = lUpdateBy;

                                //Remove SE if deselected 
                                for (int i = 0; i < lOrderSE.Count; i++)
                                {
                                    int lFound = 0;
                                    for (int j = 0; j < reqorderSumData.pSelectedCount; j++)
                                    {
                                        if (lOrderSE[i].StructureElement == "NONWBS")
                                        {
                                            if (lOrderSE[i].StructureElement.Equals(lSEAr[j]) &&
                                                lOrderSE[i].ProductType.Equals(lProdTypeAr[j]) &&
                                                lOrderSE[i].ScheduledProd.Equals(lScheduledProdAr[j]))
                                            {
                                                lFound = 1;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            var lOrderNoAr = reqorderSumData.pOrderNo.Split(',');
                                            if (j >= lWBS1Ar.Count()) { }
                                            else
                                            {
                                                if (lOrderHeader[0].WBS1 == lWBS1Ar[j] &&
                                                lOrderHeader[0].WBS2 == lWBS2Ar[j] &&
                                                lOrderHeader[0].WBS3 == lWBS3Ar[j] &&
                                                lOrderSE[i].StructureElement.Equals(lSEAr[j]) &&
                                                lOrderSE[i].ProductType.Equals(lProdTypeAr[j]) &&
                                                (lOrderSE[i].OrderNumber).ToString().Equals(lOrderNoAr[j]) &&
                                                lOrderSE[i].ScheduledProd.Equals(lScheduledProdAr[j]))
                                                {
                                                    lFound = 1;
                                                    break;
                                                }
                                            }
                                        }

                                        //Commented by ajit
                                        //if (lOrderSE[i].StructureElement.Equals(lSEAr[j]) &&

                                        //Commented by ajit
                                        //if (lOrderSE[i].StructureElement.Equals(lSEAr[j]) &&
                                        //    lOrderSE[i].ProductType.Equals(lProdTypeAr[j]) &&
                                        //    lOrderSE[i].ScheduledProd.Equals(lScheduledProdAr[j]))
                                        //{
                                        //    lFound = 1;
                                        //    break;
                                        //}
                                        //commneted by ajit
                                    }

                                    if (lFound == 0 && (lOrderSE[i].TotalWeight == null || lOrderSE[i].TotalWeight == 0) && 
                                        (lOrderSE[i].OrderStatus == "Created" || 
                                        lOrderSE[i].OrderStatus == "Created*" || 
                                        lOrderSE[i].OrderStatus == "New"))
                                    {
                                        db.OrderProjectSE.Remove(lOrderSE[i]);
                                        lCount = lCount + 1;
                                    }
                                }
                            }
                        }
                    }
                    if (lCount > 0)
                    {
                        db.SaveChanges();
                    }
                }

                List<decimal> lWT = new List<decimal>();
                List<int> lQty = new List<int>();

                List<string> lPONos = new List<string>();
                List<string> lRDs = new List<string>();
                List<string> lTransports = new List<string>();
                List<string> lNoDocs = new List<string>();
                List<string> lDeliveryAddress = new List<string>();
                List<string> lRemarks = new List<string>();
                List<string> lSiteContacts = new List<string>();
                List<string> lHandphones = new List<string>();
                List<string> lGoodsReceivers = new List<string>();
                List<string> lGoodsReceiverHandphones = new List<string>();
                List<string> AdditionalRemarks = new List<string>();

                var lProd = reqorderSumData.pSelectedProd.Split(',');
                var lSE = reqorderSumData.pSelectedSE.Split(',');
                var lScheduled = reqorderSumData.pSelectedScheduled.Split(',');
                var lPostID = reqorderSumData.pSelectedPostID.Split(',');
                var lOrderA = reqorderSumData.pOrderNo.Split(',').ToList();

                string lPONumber = "";
                string lRemark = "";
                string lSiteContact = "";
                string lHandphone = "";
                string lGoodsReceiver = "";
                string lGoodsReceiverHandphone = "";
                string AdditionalRemark = "";

                DateTime lPODate = DateTime.Now;
                string lRequiredDate = "";
                string lTransport = "";

                int lNoDoc = 0;
                string LastOrderNumber = "";

                string lOrderStatusT = "";

                string lLeadTimeProdType = "";
                string lLeadTime = "";
                string lHolidays = "";

                if (lProd.Length > 0)
                {
                    lProcessObj.OpenNDSConnection(ref lNDSCon);

                    lSQL = "SELECT ProductType, LeadTime FROM dbo.OESLeadTime ORDER BY ProductType ";
                    lCmd.CommandText = lSQL;
                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        while (lRst.Read())
                        {
                            lLeadTimeProdType = lLeadTimeProdType + "," + lRst.GetString(0);
                            lLeadTime = lLeadTime + "," + lRst.GetInt32(1).ToString();
                        }
                        lLeadTimeProdType = lLeadTimeProdType.Substring(1);
                        lLeadTime = lLeadTime.Substring(1);
                    }
                    lRst.Close();

                    lSQL = "SELECT Holiday FROM dbo.OESPublicHolidays ORDER BY Holiday ";
                    lCmd.CommandText = lSQL;
                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        while (lRst.Read())
                        {
                            lHolidays = lHolidays + "," + lRst.GetString(0);
                        }
                        lHolidays = lHolidays.Substring(1);
                    }
                    lRst.Close();

                    for (int i = 0; i < lProd.Length; i++)
                    {
                        int lOrderNo1 = 0;
                        int.TryParse(lOrderA[i], out lOrderNo1);
                        //lRemarks.Add(lRemark);//added by ajit
                        var lOrderHeader = db.OrderProject.Find(lOrderNo1);
                        var lOrderSE = db.OrderProjectSE.Find(lOrderNo1, lSE[i].Trim(), lProd[i].Trim(), lScheduled[i]);

                        if (i == 0)
                        {
                            LastOrderNumber = lOrderA[i];
                            lTotalWT = 0;
                        }

                        if (LastOrderNumber != lOrderA[i])
                        {
                            if (lOrderHeader != null && lOrderHeader.OrderNumber > 0 &&
                                (lOrderStatus == "" || lOrderStatus == "New" ||
                                lOrderStatus == "Created" || lOrderStatus == "Created*" || lOrderStatus == null))
                            {
                                var lNewHeader = lOrderHeader;

                                lNewHeader.TotalWeight = lTotalWT;
                                //lNewHeader.UpdateBy = User.Identity.GetUserName();
                                lNewHeader.UpdateDate = DateTime.Now;

                                db.Entry(lOrderHeader).CurrentValues.SetValues(lNewHeader);
                            }
                            LastOrderNumber = lOrderA[i];
                        }
                        lPONumber = "";
                        lPODate = DateTime.Now;
                        lRequiredDate = "";
                        lTransport = "";
                        lSiteContact = "";
                        lHandphone = "";
                        lGoodsReceiver = "";
                        lRemark = "";

                        if (lScheduled[i].Trim() == "Y")
                        {
                            lSQL = "SELECT H.intPostHeaderId, S.vchStructureElementType, T.vchProductType, " +
                             "isNULl(H.intPostedQty, 0) + isNULL(intPostedCappingQty, 0) + isNULL(intPostedCLinkQty, 0), " +
                             "isNULL(numPostedWeight, 0) + isNULL(numPostedCappingWeight, 0) + isNULL(numPostedClinkWeight, 0), " +
                             "(SELECT SUM(tntGroupQty) FROM dbo.PostGroupMarkingDetails where intPostHeaderId = H.intPostHeaderId), " +
                             "(SELECT SUM(tntGroupQty) FROM dbo.PostGroupMarkingDetails where intPostHeaderId = H.intPostHeaderId) " +
                             "FROM dbo.BBSPostHeader H, dbo.SAPProjectMaster P, dbo.StructureElementMaster S, dbo.ProductTypeMaster T " +
                             "WHERE P.intProjectId = H.intProjectId " +
                             "AND S.intStructureElementTypeId = H.intStructureElementTypeId " +
                             "AND T.sitProductTypeID = H.sitProductTypeId " +
                             "AND vchProjectCode = '" + reqorderSumData.pProjectCode + "' " +
                             //"AND H.tntStatusId = 3 " +
                             "AND H.intPostHeaderId = " + lPostID[i] + " ";

                            lCmd.CommandText = lSQL;
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lRst = lCmd.ExecuteReader();
                            if (lRst.HasRows)
                            {
                                var lSNo = 0;
                                while (lRst.Read())
                                {
                                    lSNo = lSNo + 1;
                                    var lStructEle = lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).ToUpper();
                                    var lProdType = lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).ToUpper();
                                    var lPostedPCs = lRst.GetValue(3) == DBNull.Value ? "0" : lRst.GetInt32(3).ToString();

                                    if (lStructEle == "BEAM" && lProdType == "MSH")
                                    {
                                        lProdType = "STIRRUP-LINK-MESH";
                                    }
                                    else if (lStructEle == "COLUMN" && lProdType == "MSH")
                                    {
                                        lProdType = "COLUMN-LINK-MESH";
                                    }
                                    else if (lStructEle != "BEAM" && lStructEle != "COLUMN" && lProdType == "MSH")
                                    {
                                        lProdType = "CUT-TO-SIZE-MESH";
                                    }
                                    else if (lProdType == "PRC")
                                    {
                                        lProdType = "PRE-CAGE";
                                    }

                                    //Core Cage Product Type added at line 5925 
                                    //else if (lStructEle == "COLUMN" && lProdType == "CAR")
                                    else if (lStructEle == "COLUMN" && ((lProdType == "CAR") || (lProdType == "CORE")))
                                    {
                                        lProdType = "CORE-CAGE";
                                    }
                                    else if (lStructEle != "COLUMN" && lProdType == "CAR")
                                    {
                                        lProdType = "CARPET";
                                    }

                                    if (lProdType == "PRE-CAGE" || lProdType == "CORE-CAGE")
                                    {
                                        lQty.Add(lRst.GetValue(5) == DBNull.Value ? 0 : lRst.GetInt32(5));
                                    }
                                    else
                                    {
                                        lQty.Add(lRst.GetValue(3) == DBNull.Value ? 0 : lRst.GetInt32(3));
                                    }


                                    lWT.Add(lRst.GetValue(0) == DBNull.Value ? 0 : lRst.GetDecimal(4));

                                    lTotalWT = lTotalWT + (lRst.GetValue(0) == DBNull.Value ? 0 : lRst.GetDecimal(4));

                                    lPONumber = "";
                                    lPODate = DateTime.Now;
                                    lRequiredDate = "";
                                    lTransport = "";
                                    lSiteContact = "";
                                    lHandphone = "";
                                    lGoodsReceiver = "";
                                    lGoodsReceiverHandphone = "";
                                    AdditionalRemark = "";

                                    if (lOrderSE != null && lOrderSE.OrderNumber > 0)
                                    {
                                        lPONumber = lOrderSE.PONumber;
                                        lPODate = lOrderSE.PODate == null ? DateTime.Now : (DateTime)lOrderSE.PODate;
                                        lRequiredDate = lOrderSE.RequiredDate == null ? "" : ((DateTime)lOrderSE.RequiredDate).ToString("yyyy-MM-dd");
                                        lTransport = lOrderSE.TransportMode;
                                        lSiteContact = lOrderSE.SiteContact;
                                        lHandphone = lOrderSE.Handphone;
                                        lGoodsReceiver = lOrderSE.GoodsReceiver;
                                        lGoodsReceiverHandphone = lOrderSE.GoodsReceiverHandphone;
                                        AdditionalRemark = lOrderSE.AdditionalRemark;
                                        lRemarks.Add(lRemark);//added by ajit for seperate devlivery
                                        lSiteContacts.Add(lSiteContact);//added by ajit seperate devlivery
                                        lHandphones.Add(lHandphone);//added by ajit seperate devlivery
                                        lGoodsReceivers.Add(lGoodsReceiver);//added by ajit seperate devlivery
                                        lGoodsReceiverHandphones.Add(lGoodsReceiverHandphone);//added by ajit seperate devlivery
                                        AdditionalRemarks.Add(AdditionalRemark);
                                    }
                                }
                            }
                            lRst.Close();
                        }
                        else
                        {
                            if (lProd[i] == "CAB")
                            {
                                decimal? lProdWT = 0;
                                int? lProdQty = 0;

                                if (lOrderSE != null && lOrderSE.OrderNumber > 0 && lOrderHeader != null && lOrderHeader.OrderNumber > 0)
                                {
                                    string lLocalSE = lSE[i].Trim();
                                    string lLocalProd = lProd[i].Trim();
                                    lPONumber = lOrderSE.PONumber;
                                    lPODate = lOrderSE.PODate == null ? DateTime.Now : (DateTime)lOrderSE.PODate;
                                    lRequiredDate = lOrderSE.RequiredDate == null ? "" : ((DateTime)lOrderSE.RequiredDate).ToString("yyyy-MM-dd");
                                    lTransport = lOrderSE.TransportMode;
                                    lSiteContact = lOrderSE.SiteContact;
                                    lHandphone = lOrderSE.Handphone;
                                    lGoodsReceiver = lOrderSE.GoodsReceiver;
                                    lGoodsReceiverHandphone = lOrderSE.GoodsReceiverHandphone;
                                    AdditionalRemark = lOrderSE.AdditionalRemark;

                                    lRemark = lOrderSE.SpecialRemark;
                                    lRemarks.Add(lRemark);
                                    lSiteContacts.Add(lSiteContact);
                                    lHandphones.Add(lHandphone);
                                    lGoodsReceivers.Add(lGoodsReceiver);
                                    lGoodsReceiverHandphones.Add(lGoodsReceiverHandphone);
                                    AdditionalRemarks.Add(AdditionalRemark);

                                    int lJobID = lOrderSE.CABJobID;

                                    var lCABJobAdv = db.JobAdvice.Find(reqorderSumData.pCustomerCode, reqorderSumData.pProjectCode, lJobID);
                                    if (lCABJobAdv != null && lCABJobAdv.TransportLimit != null)
                                    {
                                        if (lCABJobAdv.TransportLimit == "Low Bed")
                                        {
                                            lTransport = "LB30";
                                            lOrderHeader.TransportMode = lTransport;
                                        }
                                        else if (lCABJobAdv.TransportLimit == "Police Escort")
                                        {
                                            lTransport = "LBE";
                                            lOrderHeader.TransportMode = lTransport;
                                        }
                                        else
                                        {
                                            if (lOrderHeader.TransportMode == "LB30" || lOrderHeader.TransportMode == "LBE")
                                                lTransport = "TR40/24";
                                            lOrderHeader.TransportMode = lTransport;
                                        }
                                    }

                                    lProdWT = (from p in db.OrderDetails
                                               where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                               p.ProjectCode == reqorderSumData.pProjectCode &&
                                               p.JobID == lJobID &&
                                               p.Cancelled == null
                                               select p.BarWeight)
                                               .DefaultIfEmpty(0)
                                               .Sum();

                                    if (lProdWT == null) lProdWT = 0;

                                    //save WT back to SE
                                    var lNewCart = lOrderSE;
                                    if (lNewCart.TotalWeight != lProdWT)
                                    {
                                        lNewCart.TotalWeight = lProdWT;
                                        db.Entry(lOrderSE).CurrentValues.SetValues(lNewCart);
                                    }
                                    lTotalWT = lTotalWT + (decimal)lProdWT;

                                    lProdWT = Math.Round((decimal)lProdWT / 1000, 3);

                                    lProdQty = (from p in db.OrderDetails
                                                where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                                p.ProjectCode == reqorderSumData.pProjectCode &&
                                                p.JobID == lJobID &&
                                                p.Cancelled == null
                                                select p.BarTotalQty)
                                                .DefaultIfEmpty(0)
                                                .Sum();

                                    if (lProdQty == null) lProdQty = 0;

                                }

                                lWT.Add((decimal)(lProdWT == null ? 0 : lProdWT));
                                lQty.Add((int)(lProdQty == null ? 0 : lProdQty));
                            }
                            else if (lProd[i] == "CUT-TO-SIZE-MESH")
                            {
                                decimal? lProdWT = 0;
                                int? lProdQty = 0;

                                if (lOrderSE != null && lOrderSE.OrderNumber > 0 && lOrderHeader != null && lOrderHeader.OrderNumber > 0)
                                {
                                    lPONumber = lOrderSE.PONumber;
                                    lPODate = lOrderSE.PODate == null ? DateTime.Now : (DateTime)lOrderSE.PODate;
                                    lRequiredDate = lOrderSE.RequiredDate == null ? "" : ((DateTime)lOrderSE.RequiredDate).ToString("yyyy-MM-dd");
                                    lTransport = lOrderSE.TransportMode;
                                    lRemark = lOrderSE.SpecialRemark;
                                    lSiteContact = lOrderSE.SiteContact;
                                    lHandphone = lOrderSE.Handphone;
                                    lGoodsReceiver = lOrderSE.GoodsReceiver;
                                    lGoodsReceiverHandphone = lOrderSE.GoodsReceiverHandphone;
                                    AdditionalRemark = lOrderSE.AdditionalRemark;
                                    lRemarks.Add(lRemark);
                                    lSiteContacts.Add(lSiteContact);
                                    lHandphones.Add(lHandphone);
                                    lGoodsReceivers.Add(lGoodsReceiver);
                                    lGoodsReceiverHandphones.Add(lGoodsReceiverHandphone);
                                    AdditionalRemarks.Add(AdditionalRemark);
                                    int lJobID = lOrderSE.MESHJobID;

                                    lProdWT = (from p in db.CTSMESHOthersDetails
                                               where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                               p.ProjectCode == reqorderSumData.pProjectCode &&
                                               p.JobID == lJobID
                                               select p.MeshTotalWT)
                                               .DefaultIfEmpty(0)
                                               .Sum();

                                    if (lProdWT == null) lProdWT = 0;

                                    //save WT back to SE
                                    var lNewCart = lOrderSE;
                                    lNewCart.TotalWeight = lProdWT;
                                    db.Entry(lOrderSE).CurrentValues.SetValues(lNewCart);
                                    lTotalWT = lTotalWT + (decimal)lProdWT;

                                    lProdWT = Math.Round((decimal)lProdWT / 1000, 3);

                                    lProdQty = (from p in db.CTSMESHOthersDetails
                                                where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                                p.ProjectCode == reqorderSumData.pProjectCode &&
                                                p.JobID == lJobID
                                                select p.MeshMemberQty)
                                                .DefaultIfEmpty(0)
                                                .Sum();

                                    if (lProdQty == null) lProdQty = 0;

                                }

                                lWT.Add((decimal)(lProdWT == null ? 0 : lProdWT));
                                lQty.Add((int)(lProdQty == null ? 0 : lProdQty));
                            }
                            else if (lProd[i] == "STIRRUP-LINK-MESH")
                            {
                                decimal? lProdWT = 0;
                                int? lProdQty = 0;

                                if (lOrderSE != null && lOrderSE.OrderNumber > 0 && lOrderHeader != null && lOrderHeader.OrderNumber > 0)
                                {
                                    lPONumber = lOrderSE.PONumber;
                                    lPODate = lOrderSE.PODate == null ? DateTime.Now : (DateTime)lOrderSE.PODate;
                                    lRequiredDate = lOrderSE.RequiredDate == null ? "" : ((DateTime)lOrderSE.RequiredDate).ToString("yyyy-MM-dd");
                                    lTransport = lOrderSE.TransportMode;
                                    lRemark = lOrderSE.SpecialRemark;
                                    lSiteContact = lOrderSE.SiteContact;
                                    lHandphone = lOrderSE.Handphone;
                                    lGoodsReceiver = lOrderSE.GoodsReceiver;
                                    lGoodsReceiverHandphone = lOrderSE.GoodsReceiverHandphone;
                                    AdditionalRemark = lOrderSE.AdditionalRemark;
                                    lRemarks.Add(lRemark);
                                    lSiteContacts.Add(lSiteContact);
                                    lHandphones.Add(lHandphone);
                                    lGoodsReceivers.Add(lGoodsReceiver);
                                    lGoodsReceiverHandphones.Add(lGoodsReceiverHandphone);
                                    AdditionalRemarks.Add(AdditionalRemark);
                                    int lJobID = lOrderSE.MESHJobID;

                                    lProdWT = (from p in db.CTSMESHBeamDetails
                                               where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                               p.ProjectCode == reqorderSumData.pProjectCode &&
                                               p.JobID == lJobID
                                               select p.MeshTotalWT)
                                               .DefaultIfEmpty(0)
                                               .Sum();

                                    if (lProdWT == null) lProdWT = 0;

                                    //save WT back to SE
                                    var lNewCart = lOrderSE;
                                    lNewCart.TotalWeight = lProdWT;
                                    db.Entry(lOrderSE).CurrentValues.SetValues(lNewCart);
                                    lTotalWT = lTotalWT + (decimal)lProdWT;

                                    lProdWT = Math.Round((decimal)lProdWT / 1000, 3);

                                    lProdQty = (from p in db.CTSMESHBeamDetails
                                                where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                                p.ProjectCode == reqorderSumData.pProjectCode &&
                                                p.JobID == lJobID
                                                select p.MeshMemberQty)
                                                .DefaultIfEmpty(0)
                                                .Sum();

                                    if (lProdQty == null) lProdQty = 0;

                                }

                                lWT.Add((decimal)(lProdWT == null ? 0 : lProdWT));
                                lQty.Add((int)(lProdQty == null ? 0 : lProdQty));
                            }
                            else if (lProd[i] == "COLUMN-LINK-MESH")
                            {
                                decimal? lProdWT = 0;
                                int? lProdQty = 0;

                                if (lOrderSE != null && lOrderSE.OrderNumber > 0 && lOrderHeader != null && lOrderHeader.OrderNumber > 0)
                                {
                                    lPONumber = lOrderSE.PONumber;
                                    lPODate = lOrderSE.PODate == null ? DateTime.Now : (DateTime)lOrderSE.PODate;
                                    lRequiredDate = lOrderSE.RequiredDate == null ? "" : ((DateTime)lOrderSE.RequiredDate).ToString("yyyy-MM-dd");
                                    lTransport = lOrderSE.TransportMode;

                                    int lJobID = lOrderSE.MESHJobID;
                                    lRemark = lOrderSE.SpecialRemark;
                                    lSiteContact = lOrderSE.SiteContact;
                                    lHandphone = lOrderSE.Handphone;
                                    lGoodsReceiver = lOrderSE.GoodsReceiver;
                                    lGoodsReceiverHandphone = lOrderSE.GoodsReceiverHandphone;
                                    AdditionalRemark = lOrderSE.AdditionalRemark;
                                    lRemarks.Add(lRemark);
                                    lSiteContacts.Add(lSiteContact);
                                    lHandphones.Add(lHandphone);
                                    lGoodsReceivers.Add(lGoodsReceiver);
                                    lGoodsReceiverHandphones.Add(lGoodsReceiverHandphone);
                                    AdditionalRemarks.Add(AdditionalRemark);
                                    lProdWT = (from p in db.CTSMESHColumnDetails
                                               where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                               p.ProjectCode == reqorderSumData.pProjectCode &&
                                               p.JobID == lJobID
                                               select p.MeshTotalWT)
                                               .DefaultIfEmpty(0)
                                               .Sum();

                                    if (lProdWT == null) lProdWT = 0;

                                    //save WT back to SE
                                    var lNewCart = lOrderSE;
                                    lNewCart.TotalWeight = lProdWT;
                                    db.Entry(lOrderSE).CurrentValues.SetValues(lNewCart);
                                    lTotalWT = lTotalWT + (decimal)lProdWT;

                                    lProdWT = Math.Round((decimal)lProdWT / 1000, 3);

                                    lProdQty = (from p in db.CTSMESHColumnDetails
                                                where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                                p.ProjectCode == reqorderSumData.pProjectCode &&
                                                p.JobID == lJobID
                                                select p.MeshMemberQty)
                                                .DefaultIfEmpty(0)
                                                .Sum();

                                    if (lProdQty == null) lProdQty = 0;

                                }

                                lWT.Add((decimal)(lProdWT == null ? 0 : lProdWT));
                                lQty.Add((int)(lProdQty == null ? 0 : lProdQty));
                            }
                            else if (lProd[i] == "PRE-CAGE")
                            {
                                decimal? lProdWT = 0;
                                int? lProdQty = 0;

                                if (lOrderSE != null && lOrderSE.OrderNumber > 0 && lOrderHeader != null && lOrderHeader.OrderNumber > 0)
                                {
                                    lPONumber = lOrderSE.PONumber;
                                    lPODate = lOrderSE.PODate == null ? DateTime.Now : (DateTime)lOrderSE.PODate;
                                    lRequiredDate = lOrderSE.RequiredDate == null ? "" : ((DateTime)lOrderSE.RequiredDate).ToString("yyyy-MM-dd");
                                    lTransport = lOrderSE.TransportMode;
                                    lRemark = lOrderSE.SpecialRemark;
                                    lSiteContact = lOrderSE.SiteContact;
                                    lHandphone = lOrderSE.Handphone;
                                    lGoodsReceiver = lOrderSE.GoodsReceiver;
                                    lGoodsReceiverHandphone = lOrderSE.GoodsReceiverHandphone;
                                    AdditionalRemark = lOrderSE.AdditionalRemark;
                                    lRemarks.Add(lRemark);
                                    lSiteContacts.Add(lSiteContact);
                                    lHandphones.Add(lHandphone);
                                    lGoodsReceivers.Add(lGoodsReceiver);
                                    lGoodsReceiverHandphones.Add(lGoodsReceiverHandphone);
                                    AdditionalRemarks.Add(AdditionalRemark);
                                    int lJobID = lOrderSE.CageJobID;

                                    lProdWT = (from p in db.PRCJobAdvice
                                               where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                               p.ProjectCode == reqorderSumData.pProjectCode &&
                                               p.JobID == lJobID
                                               select p.TotalWeight)
                                               .DefaultIfEmpty(0)
                                               .Sum();

                                    if (lProdWT == null) lProdWT = 0;

                                    //save WT back to SE
                                    var lNewCart = lOrderSE;
                                    lNewCart.TotalWeight = lProdWT;
                                    db.Entry(lOrderSE).CurrentValues.SetValues(lNewCart);
                                    lTotalWT = lTotalWT + (decimal)lProdWT;

                                    lProdWT = Math.Round((decimal)lProdWT / 1000, 3);

                                    lProdQty = (from p in db.PRCJobAdvice
                                                where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                                p.ProjectCode == reqorderSumData.pProjectCode &&
                                                p.JobID == lJobID
                                                select p.TotalPcs)
                                                .DefaultIfEmpty(0)
                                                .Sum();

                                    if (lProdQty == null) lProdQty = 0;

                                }

                                lWT.Add((decimal)(lProdWT == null ? 0 : lProdWT));
                                lQty.Add((int)(lProdQty == null ? 0 : lProdQty));
                            }
                            else if (lProd[i] == "BPC")
                            {
                                decimal? lProdWT = 0;
                                int? lProdQty = 0;

                                if (lOrderSE != null && lOrderSE.OrderNumber > 0 && lOrderHeader != null && lOrderHeader.OrderNumber > 0)
                                {
                                    lPONumber = lOrderSE.PONumber;
                                    lPODate = lOrderSE.PODate == null ? DateTime.Now : (DateTime)lOrderSE.PODate;
                                    lRequiredDate = lOrderSE.RequiredDate == null ? "" : ((DateTime)lOrderSE.RequiredDate).ToString("yyyy-MM-dd");
                                    lTransport = lOrderSE.TransportMode;
                                    lSiteContact = lOrderSE.SiteContact;
                                    lHandphone = lOrderSE.Handphone;
                                    lGoodsReceiver = lOrderSE.GoodsReceiver;
                                    lGoodsReceiverHandphone = lOrderSE.GoodsReceiverHandphone;
                                    AdditionalRemark = lOrderSE.AdditionalRemark;
                                    lRemarks.Add(lRemark);
                                    lSiteContacts.Add(lSiteContact);
                                    lHandphones.Add(lHandphone);
                                    lGoodsReceivers.Add(lGoodsReceiver);
                                    lGoodsReceiverHandphones.Add(lGoodsReceiverHandphone);
                                    AdditionalRemarks.Add(AdditionalRemark);
                                    int lJobID = lOrderSE.BPCJobID;

                                    lProdWT = (from p in db.BPCJobAdvice
                                               where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                               p.ProjectCode == reqorderSumData.pProjectCode &&
                                               p.JobID == lJobID
                                               select p.TotalWeight)
                                               .DefaultIfEmpty(0)
                                               .Sum();

                                    if (lProdWT == null) lProdWT = 0;

                                    //save WT back to SE
                                    //BPC Weight MT in JobAdvice
                                    var lNewCart = lOrderSE;
                                    lNewCart.TotalWeight = lProdWT * 1000;
                                    db.Entry(lOrderSE).CurrentValues.SetValues(lNewCart);
                                    lTotalWT = lTotalWT + (decimal)lProdWT * 1000;

                                    //lProdWT = Math.Round((decimal)lProdWT / 1000, 3);

                                    lProdQty = (from p in db.BPCJobAdvice
                                                where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                                p.ProjectCode == reqorderSumData.pProjectCode &&
                                                p.JobID == lJobID
                                                select p.TotalPcs)
                                                .DefaultIfEmpty(0)
                                                .Sum();

                                    if (lProdQty == null) lProdQty = 0;

                                }

                                lWT.Add((decimal)(lProdWT == null ? 0 : lProdWT));
                                lQty.Add((int)(lProdQty == null ? 0 : lProdQty));
                            }
                            else if (lProd[i] == "STANDARD-BAR")
                            {
                                decimal? lProdWT = 0;
                                int? lProdQty = 0;

                                if (lOrderSE != null && lOrderSE.OrderNumber > 0 && lOrderHeader != null && lOrderHeader.OrderNumber > 0)
                                {
                                    lPONumber = lOrderSE.PONumber;
                                    lPODate = lOrderSE.PODate == null ? DateTime.Now : (DateTime)lOrderSE.PODate;
                                    lRequiredDate = lOrderSE.RequiredDate == null ? "" : ((DateTime)lOrderSE.RequiredDate).ToString("yyyy-MM-dd");
                                    lTransport = lOrderSE.TransportMode;
                                    lRemark = lOrderSE.SpecialRemark;
                                    lSiteContact = lOrderSE.SiteContact;
                                    lHandphone = lOrderSE.Handphone;
                                    lGoodsReceiver = lOrderSE.GoodsReceiver;
                                    lGoodsReceiverHandphone = lOrderSE.GoodsReceiverHandphone;
                                    AdditionalRemark = lOrderSE.AdditionalRemark;
                                    lRemarks.Add(lRemark);
                                    lSiteContacts.Add(lSiteContact);
                                    lHandphones.Add(lHandphone);
                                    lGoodsReceivers.Add(lGoodsReceiver);
                                    lGoodsReceiverHandphones.Add(lGoodsReceiverHandphone);
                                    AdditionalRemarks.Add(AdditionalRemark);
                                    int lJobID = lOrderSE.StdBarsJobID;

                                    lProdWT = (from p in db.StdProdDetails
                                               where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                               p.ProjectCode == reqorderSumData.pProjectCode &&
                                               p.JobID == lJobID &&
                                               (p.ProdType == "BAR12" ||
                                               p.ProdType == "BAR15" ||
                                               p.ProdType == "BAR14" ||
                                               p.ProdType == "500M" ||
                                               p.ProdType == "BAR6")
                                               select p.order_wt)
                                               .DefaultIfEmpty(0)
                                               .Sum();

                                    if (lProdWT == null) lProdWT = 0;

                                    //save WT back to SE
                                    var lNewCart = lOrderSE;
                                    if (lNewCart.TotalWeight != lProdWT)
                                    {
                                        lNewCart.TotalWeight = lProdWT;
                                        db.Entry(lOrderSE).CurrentValues.SetValues(lNewCart);
                                    }
                                    lTotalWT = lTotalWT + (decimal)lProdWT;

                                    lProdWT = Math.Round((decimal)lProdWT / 1000, 3);

                                    lProdQty = (from p in db.StdProdDetails
                                                where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                                p.ProjectCode == reqorderSumData.pProjectCode &&
                                                p.JobID == lJobID &&
                                                (p.ProdType == "BAR12" ||
                                                p.ProdType == "BAR15" ||
                                                p.ProdType == "BAR14" ||
                                                p.ProdType == "BAR6" ||
                                                p.ProdType == "500M")
                                                select p.order_pcs)
                                               .DefaultIfEmpty(0)
                                               .Sum();

                                    if (lProdQty == null) lProdQty = 0;

                                }

                                lWT.Add((decimal)(lProdWT == null ? 0 : lProdWT));
                                lQty.Add((int)(lProdQty == null ? 0 : lProdQty));
                            }
                            else if (lProd[i] == "STANDARD-MESH")
                            {
                                decimal? lProdWT = 0;
                                int? lProdQty = 0;

                                if (lOrderSE != null && lOrderSE.OrderNumber > 0 && lOrderHeader != null && lOrderHeader.OrderNumber > 0)
                                {
                                    lPONumber = lOrderSE.PONumber;
                                    lPODate = lOrderSE.PODate == null ? DateTime.Now : (DateTime)lOrderSE.PODate;
                                    lRequiredDate = lOrderSE.RequiredDate == null ? "" : ((DateTime)lOrderSE.RequiredDate).ToString("yyyy-MM-dd");
                                    lTransport = lOrderSE.TransportMode;
                                    lRemark = lOrderSE.SpecialRemark;
                                    lSiteContact = lOrderSE.SiteContact;
                                    lHandphone = lOrderSE.Handphone;
                                    lGoodsReceiver = lOrderSE.GoodsReceiver;
                                    lGoodsReceiverHandphone = lOrderSE.GoodsReceiverHandphone;
                                    AdditionalRemark = lOrderSE.AdditionalRemark;
                                    lRemarks.Add(lRemark);
                                    lSiteContacts.Add(lSiteContact);
                                    lHandphones.Add(lHandphone);
                                    lGoodsReceivers.Add(lGoodsReceiver);
                                    lGoodsReceiverHandphones.Add(lGoodsReceiverHandphone);
                                    AdditionalRemarks.Add(AdditionalRemark);
                                    int lJobID = lOrderSE.StdMESHJobID;

                                    lProdWT = (from p in db.StdSheetDetails
                                               where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                               p.ProjectCode == reqorderSumData.pProjectCode &&
                                               p.JobID == lJobID
                                               select p.order_wt)
                                               .DefaultIfEmpty(0)
                                               .Sum();

                                    if (lProdWT == null) lProdWT = 0;

                                    //save WT back to SE
                                    var lNewCart = lOrderSE;
                                    if (lNewCart.TotalWeight != lProdWT)
                                    {
                                        lNewCart.TotalWeight = lProdWT;
                                        db.Entry(lOrderSE).CurrentValues.SetValues(lNewCart);
                                    }
                                    lTotalWT = lTotalWT + (decimal)lProdWT;

                                    lProdWT = Math.Round((decimal)lProdWT / 1000, 3);

                                    lProdQty = (from p in db.StdSheetDetails
                                                where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                                p.ProjectCode == reqorderSumData.pProjectCode &&
                                                p.JobID == lJobID
                                                select p.order_pcs)
                                                .DefaultIfEmpty(0)
                                                .Sum();

                                    if (lProdQty == null) lProdQty = 0;
                                }

                                lWT.Add((decimal)(lProdWT == null ? 0 : lProdWT));
                                lQty.Add((int)(lProdQty == null ? 0 : lProdQty));
                            }
                            else if (lProd[i] == "COIL")
                            {
                                decimal? lProdWT = 0;
                                int? lProdQty = 0;

                                if (lOrderSE != null && lOrderSE.OrderNumber > 0 && lOrderHeader != null && lOrderHeader.OrderNumber > 0)
                                {
                                    lPONumber = lOrderSE.PONumber;
                                    lPODate = lOrderSE.PODate == null ? DateTime.Now : (DateTime)lOrderSE.PODate;
                                    lRequiredDate = lOrderSE.RequiredDate == null ? "" : ((DateTime)lOrderSE.RequiredDate).ToString("yyyy-MM-dd");
                                    lTransport = lOrderSE.TransportMode;
                                    lSiteContact = lOrderSE.SiteContact;
                                    lHandphone = lOrderSE.Handphone;
                                    lGoodsReceiver = lOrderSE.GoodsReceiver;
                                    lGoodsReceiverHandphone = lOrderSE.GoodsReceiverHandphone;
                                    AdditionalRemark = lOrderSE.AdditionalRemark;
                                    lRemarks.Add(lRemark);
                                    lSiteContacts.Add(lSiteContact);
                                    lHandphones.Add(lHandphone);
                                    lGoodsReceivers.Add(lGoodsReceiver);
                                    lGoodsReceiverHandphones.Add(lGoodsReceiverHandphone);
                                    AdditionalRemarks.Add(AdditionalRemark);
                                    int lJobID = lOrderSE.CoilProdJobID;

                                    lProdWT = (from p in db.StdProdDetails
                                               where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                               p.ProjectCode == reqorderSumData.pProjectCode &&
                                               p.JobID == lJobID &&
                                               p.ProdType != "BAR12" &&
                                               p.ProdType != "BAR15" &&
                                               p.ProdType != "BAR14" &&
                                               p.ProdType != "BAR6" &&
                                               p.ProdType != "500M" &&
                                               p.ProdType != "N-SPLICE-HEAD" &&
                                               p.ProdType != "E-SPLICE-HEAD" &&
                                               p.ProdType != "N-SPLICE-TAP" &&
                                               p.ProdType != "E-SPLICE-TAP"
                                               select p.order_wt)
                                               .DefaultIfEmpty(0)
                                               .Sum();

                                    if (lProdWT == null) lProdWT = 0;

                                    //save WT back to SE
                                    var lNewCart = lOrderSE;
                                    if (lNewCart.TotalWeight != lProdWT)
                                    {
                                        lNewCart.TotalWeight = lProdWT;
                                        db.Entry(lOrderSE).CurrentValues.SetValues(lNewCart);
                                    }
                                    lTotalWT = lTotalWT + (decimal)lProdWT;

                                    lProdWT = Math.Round((decimal)lProdWT / 1000, 3);

                                    lProdQty = (from p in db.StdProdDetails
                                                where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                                p.ProjectCode == reqorderSumData.pProjectCode &&
                                                p.JobID == lJobID &&
                                                p.ProdType != "BAR12" &&
                                                p.ProdType != "BAR15" &&
                                                p.ProdType != "BAR14" &&
                                                p.ProdType != "BAR6" &&
                                                p.ProdType != "500M" &&
                                                p.ProdType != "N-SPLICE-HEAD" &&
                                                p.ProdType != "E-SPLICE-HEAD" &&
                                                p.ProdType != "N-SPLICE-TAP" &&
                                                p.ProdType != "E-SPLICE-TAP"
                                                select p.order_pcs)
                                                .DefaultIfEmpty(0)
                                                .Sum();

                                    if (lProdQty == null) lProdQty = 0;
                                }

                                lWT.Add((decimal)(lProdWT == null ? 0 : lProdWT));
                                lQty.Add((int)(lProdQty == null ? 0 : lProdQty));
                            }
                            else if (lProd[i] == "COUPLER")
                            {
                                decimal? lProdWT = 0;
                                int? lProdQty = 0;

                                if (lOrderSE != null && lOrderSE.OrderNumber > 0 && lOrderHeader != null && lOrderHeader.OrderNumber > 0)
                                {
                                    lPONumber = lOrderSE.PONumber;
                                    lPODate = lOrderSE.PODate == null ? DateTime.Now : (DateTime)lOrderSE.PODate;
                                    lRequiredDate = lOrderSE.RequiredDate == null ? "" : ((DateTime)lOrderSE.RequiredDate).ToString("yyyy-MM-dd");
                                    lTransport = lOrderSE.TransportMode;
                                    lSiteContact = lOrderSE.SiteContact;
                                    lHandphone = lOrderSE.Handphone;
                                    lGoodsReceiver = lOrderSE.GoodsReceiver;
                                    lGoodsReceiverHandphone = lOrderSE.GoodsReceiverHandphone;
                                    AdditionalRemark = lOrderSE.AdditionalRemark;
                                    lRemarks.Add(lRemark);
                                    lSiteContacts.Add(lSiteContact);
                                    lHandphones.Add(lHandphone);
                                    lGoodsReceivers.Add(lGoodsReceiver);
                                    lGoodsReceiverHandphones.Add(lGoodsReceiverHandphone);
                                    AdditionalRemarks.Add(AdditionalRemark);
                                    int lJobID = lOrderSE.CoilProdJobID;

                                    lProdWT = (from p in db.StdProdDetails
                                               where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                               p.ProjectCode == reqorderSumData.pProjectCode &&
                                               p.JobID == lJobID &&
                                               (p.ProdType == "N-SPLICE-HEAD" ||
                                               p.ProdType == "E-SPLICE-HEAD" ||
                                               p.ProdType == "N-SPLICE-TAP" ||
                                               p.ProdType == "E-SPLICE-TAP")
                                               select p.order_wt)
                                               .DefaultIfEmpty(0)
                                               .Sum();

                                    if (lProdWT == null) lProdWT = 0;

                                    //save WT back to SE
                                    var lNewCart = lOrderSE;
                                    if (lNewCart.TotalWeight != lProdWT)
                                    {
                                        lNewCart.TotalWeight = lProdWT;
                                        db.Entry(lOrderSE).CurrentValues.SetValues(lNewCart);
                                    }
                                    //lTotalWT = lTotalWT + (decimal)lProdWT;

                                    //lProdWT = Math.Round((decimal)lProdWT / 1000, 3);

                                    lProdWT = 0;

                                    lProdQty = (from p in db.StdProdDetails
                                                where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                                p.ProjectCode == reqorderSumData.pProjectCode &&
                                                p.JobID == lJobID &&
                                                (p.ProdType == "N-SPLICE-HEAD" ||
                                                p.ProdType == "E-SPLICE-HEAD" ||
                                                p.ProdType == "N-SPLICE-TAP" ||
                                                p.ProdType == "E-SPLICE-TAP")
                                                select p.order_pcs)
                                                .DefaultIfEmpty(0)
                                                .Sum();

                                    if (lProdQty == null) lProdQty = 0;
                                }

                                lWT.Add((decimal)(lProdWT == null ? 0 : lProdWT));
                                lQty.Add((int)(lProdQty == null ? 0 : lProdQty));
                            }
                            else if (lProd[i] == "COMPONENT")
                            {
                                decimal? lProdWT = 0;
                                int? lProdQty = 0;

                                if (lOrderSE != null && lOrderSE.OrderNumber > 0 && lOrderHeader != null && lOrderHeader.OrderNumber > 0)
                                {
                                    lPONumber = lOrderSE.PONumber;
                                    lPODate = lOrderSE.PODate == null ? DateTime.Now : (DateTime)lOrderSE.PODate;
                                    lRequiredDate = lOrderSE.RequiredDate == null ? "" : ((DateTime)lOrderSE.RequiredDate).ToString("yyyy-MM-dd");
                                    lTransport = lOrderSE.TransportMode;
                                    lSiteContact = lOrderSE.SiteContact;
                                    lHandphone = lOrderSE.Handphone;
                                    lGoodsReceiver = lOrderSE.GoodsReceiver;
                                    lGoodsReceiverHandphone = lOrderSE.GoodsReceiverHandphone;
                                    int lJobID = lOrderSE.OrderNumber;

                                    lProdWT = (from p in db.ComponentSetOrder
                                               where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                               p.ProjectCode == reqorderSumData.pProjectCode &&
                                               p.OrderID == lJobID &&
                                               (p.OrderStatus == null ||
                                               p.OrderStatus != "Cancelled")
                                               select p.TotalWeight)
                                               .DefaultIfEmpty(0)
                                               .Sum();

                                    if (lProdWT == null) lProdWT = 0;

                                    //save WT back to SE
                                    var lNewCart = lOrderSE;
                                    if (lNewCart.TotalWeight != lProdWT)
                                    {
                                        lNewCart.TotalWeight = lProdWT;
                                        db.Entry(lOrderSE).CurrentValues.SetValues(lNewCart);
                                    }

                                    lTotalWT = lTotalWT + (decimal)lProdWT;

                                    lProdWT = Math.Round((decimal)lProdWT / 1000, 3);

                                    lProdQty = 0;

                                    lProdQty = (from p in db.ComponentSetOrder
                                                where p.CustomerCode == reqorderSumData.pCustomerCode &&
                                                p.ProjectCode == reqorderSumData.pProjectCode &&
                                                p.OrderID == lJobID &&
                                                (p.OrderStatus == null ||
                                                p.OrderStatus != "Cancelled")
                                                select p.TotalPCs)
                                                .DefaultIfEmpty(0)
                                                .Sum();

                                    if (lProdQty == null) lProdQty = 0;
                                }

                                lWT.Add((decimal)(lProdWT == null ? 0 : lProdWT));
                                lQty.Add((int)(lProdQty == null ? 0 : lProdQty));
                            }
                            else
                            {
                                lWT.Add(0);
                                lQty.Add(0);
                            }
                        }

                        //get number of documents attached
                        lSQL = "SELECT COUNT(DocID) " +
                        "FROM dbo.OESOrderDocs " +
                        "WHERE OrderNumber = " + lOrderA[i] + " " +
                        "AND StructureElement = '" + lSE[i] + "' " +
                        "AND ProductType = '" + lProd[i] + "' " +
                        "AND ScheduledProd = '" + lScheduled[i] + "' " +
                        "AND ItemID = 0 ";

                        lCmd.CommandText = lSQL;
                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                lNoDoc = lRst.GetValue(0) == DBNull.Value ? 0 : lRst.GetInt32(0);
                                if (lNoDoc > 15)
                                {
                                    lNoDoc = 15;
                                }
                            }
                        }
                        lRst.Close();

                        //get number of documents attached from share point

                        lSQL = "SELECT COUNT(W.DrawingID) " +
                        "FROM dbo.OESDrawings W, dbo.OESDrawingsOrder O " +
                        "WHERE W.DrawingID = O.DrawingID " +
                        "AND O.OrderNumber = " + lOrderA[i] + " " +
                        "AND O.StructureElement = '" + lSE[i] + "' " +
                        "AND O.ProductType = '" + lProd[i] + "' " +
                        "AND O.ScheduledProd = '" + lScheduled[i] + "' ";

                        lCmd.CommandText = lSQL;
                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                lNoDoc = lNoDoc + (lRst.GetValue(0) == DBNull.Value ? 0 : lRst.GetInt32(0));
                                if (lNoDoc > 15)
                                {
                                    lNoDoc = 15;
                                }
                            }
                        }
                        lRst.Close();

                        lPONos.Add(lPONumber);
                        lRDs.Add(lRequiredDate);
                        lTransports.Add(lTransport);
                        lSiteContacts.Add(lSiteContact);
                        lHandphones.Add(lHandphone);
                        lGoodsReceivers.Add(lGoodsReceiver);
                        //lRemarks.Add(lRemark);//added by ajit
                        lNoDocs.Add(lNoDoc.ToString());


                        if (lOrderStatusT == "")
                        {
                            lOrderStatusT = (lOrderHeader == null || lOrderHeader.OrderStatus == null || lOrderHeader.OrderStatus == "") ? "Created" : lOrderHeader.OrderStatus;
                        }
                        else
                        {
                            if (lOrderHeader != null && lOrderHeader.OrderStatus != "Created" && lOrderHeader.OrderStatus != "New" && lOrderHeader.OrderStatus != "")
                            {
                                lOrderStatusT = (lOrderHeader == null || lOrderHeader.OrderStatus == null || lOrderHeader.OrderStatus == "") ? "Created" : lOrderHeader.OrderStatus;
                            }
                        }

                        if (lOrderSE != null && lOrderSE.OrderNumber > 0)
                        {
                            if (lPEStatus == "")
                            {
                                lPEStatus = ((lOrderSE == null || lOrderSE.OrderStatus == null || lOrderSE.OrderStatus == "") ? "Created" : lOrderSE.OrderStatus);
                            }
                            else
                            {
                                lPEStatus = lPEStatus + "," + ((lOrderSE == null || lOrderSE.OrderStatus == null || lOrderSE.OrderStatus == "") ? "Created" : lOrderSE.OrderStatus);
                            }
                            if (lOrderStatus != "New" && lOrderStatus != lOrderSE.OrderStatus && lProd.Length > 1)
                            {
                                lStatusDiff = 1;
                            }
                        }
                        else
                        {
                            if (lPEStatus == "")
                            {
                                lPEStatus = "Created";
                            }
                            else
                            {
                                lPEStatus = lPEStatus + "," + "Created";
                            }
                            if (lOrderStatus != "New" && lOrderStatus != "Created" && lProd.Length > 1)
                            {
                                lStatusDiff = 1;
                            }
                        }

                    }

                    if (lTotalWT > 0)
                    {
                        int lOrderNo2 = 0;
                        int.TryParse(LastOrderNumber, out lOrderNo2);

                        var lOrderHeader = db.OrderProject.Find(lOrderNo2);
                        if (lOrderHeader != null && lOrderHeader.OrderNumber > 0 &&
                            (lOrderStatus == "" || lOrderStatus == "New" ||
                            lOrderStatus == "Created" || lOrderStatus == "Created*" || lOrderStatus == null))
                        {
                            var lNewHeader = lOrderHeader;

                            lNewHeader.TotalWeight = lTotalWT;
                            //lNewHeader.UpdateBy = User.Identity.GetUserName();
                            lNewHeader.UpdateDate = DateTime.Now;

                            db.Entry(lOrderHeader).CurrentValues.SetValues(lNewHeader);
                        }
                    }

                    db.SaveChanges();

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                    lProcessObj = null;
                }

                if (lSubmission != "Yes")
                {
                    lStatusDiff = 0;
                }

                string lSelectedWBS = "";
                if (reqorderSumData.pSelectedWBS1 != null && reqorderSumData.pSelectedWBS1.Length > 0)
                {
                    var lWBS1a = reqorderSumData.pSelectedWBS1.Split(',');
                    var lWBS2a = reqorderSumData.pSelectedWBS2.Split(',');
                    var lWBS3a = reqorderSumData.pSelectedWBS3.Split(',');
                    for (int i = 0; i < lWBS1a.Length; i++)
                    {
                        if (lWBS1a[i].Trim().Length > 0 || lWBS2a[i].Trim().Length > 0 || lWBS3a[i].Trim().Length > 0)
                        {
                            lSelectedWBS = lSelectedWBS + "," + lWBS1a[i].Trim() + " / " + lWBS2a[i].Trim() + " / " + lWBS3a[i].Trim();
                        }
                        else
                        {
                            lSelectedWBS = lSelectedWBS + "," + "";

                        }
                    }
                    lSelectedWBS = lSelectedWBS.Substring(1);
                }
                //lOrderList.Add(new OrderSummaryData
                //{


                //    pSelectedSE = pSelectedSE

                //}
                //)
                //ViewBag.SelectedCount = pSelectedCount;


                lOrderList.pSelectedSE = reqorderSumData.pSelectedSE;
                lOrderList.pSelectedProd = reqorderSumData.pSelectedProd;
                lOrderList.pSelectedPostID = reqorderSumData.pSelectedPostID;
                lOrderList.pSelectedScheduled = reqorderSumData.pSelectedScheduled;
                lOrderList.lSelectedWBS = lSelectedWBS;
                lOrderList.pSelectedWBS1 = reqorderSumData.pSelectedWBS1;
                lOrderList.pSelectedWBS2 = reqorderSumData.pSelectedWBS2;
                lOrderList.pSelectedWBS3 = reqorderSumData.pSelectedWBS3;
                lOrderList.lPEStatus = lPEStatus;
                lOrderList.lStatusDiff = lStatusDiff;
                lOrderList.lOrderStatusT = lOrderStatusT;
                lOrderList.pWBS1 = reqorderSumData.pWBS1;
                lOrderList.pWBS2 = reqorderSumData.pWBS2;
                lOrderList.pWBS3 = reqorderSumData.pWBS3;
                lOrderList.lWT = lWT;
                lOrderList.lQty = lQty;
                lOrderList.lNoDocs = lNoDocs;
                //ViewBag.SelectedProd = pSelectedProd.Split(',');
                //ViewBag.SelectedPostID = pSelectedPostID.Split(',');
                //ViewBag.SelectedScheduled = pSelectedScheduled.Split(',');
                //ViewBag.SelectedWBS = lSelectedWBS.Split(',');
                //ViewBag.SelectedWBS1 = pSelectedWBS1.Split(',');
                //ViewBag.SelectedWBS2 = pSelectedWBS2.Split(',');
                //ViewBag.SelectedWBS3 = pSelectedWBS3.Split(',');
                //ViewBag.PEStatus = lPEStatus.Split(',');
                //ViewBag.StatusDiff = lStatusDiff;
                //ViewBag.OrderStatus = lOrderStatusT;

                //ViewBag.WBS1 = pWBS1.Split(',');
                //ViewBag.WBS2 = pWBS2.Split(',');
                //ViewBag.WBS3 = pWBS3.Split(',');

                //ViewBag.SelectedWT = lWT;
                //ViewBag.SelectedQty = lQty;
                //ViewBag.SelectedNoDoc = lNoDocs;

                //ViewBag.MainPODate = lPODate.ToString("yyyy-MM-dd");

                //ViewBag.LeadTimeProdType = lLeadTimeProdType;
                //ViewBag.LeadTime = lLeadTime;
                //ViewBag.Holidays = lHolidays;

                //get main PO
                lPONumber = "";
                lRequiredDate = "";
                lTransport = "";

                if (lPONos.Count > 0)
                {
                    lPONumber = lPONos[0];
                    lRequiredDate = lRDs[0];
                    lTransport = lTransports[0];
                    lSiteContact = lSiteContacts[0];
                    lHandphone = lHandphones[0];
                    lGoodsReceiver = lGoodsReceivers[0];
                }

                if (lPONos.Count > 1)
                {
                    int lSameCT = 0;
                    int lMaxSameCT = 0;
                    for (int i = 0; i < lPONos.Count - 1; i++)
                    {
                        for (int j = i + 1; j < lPONos.Count; j++)
                        {
                            if (lPONos[j] == lPONos[i] && lRDs[j] == lRDs[i] && lTransports[j] == lTransports[i])
                            {
                                lSameCT = lSameCT + 1;
                            }
                        }
                        if (lSameCT > lMaxSameCT)
                        {
                            lMaxSameCT = lSameCT;
                            lPONumber = lPONos[i];
                            lRequiredDate = lRDs[i];
                            lTransport = lTransports[i];
                            lSiteContact = lSiteContacts[i];
                            lHandphone = lHandphones[i];
                            lGoodsReceiver = lGoodsReceivers[i];
                        }
                    }
                    lPONumber = lPONos[0];
                    lRequiredDate = lRDs[0];
                    lTransport = lTransports[0];
                    //lSiteContact = lSiteContacts[0];
                    //lHandphone = lHandphones[0];
                    //lGoodsReceiver = lGoodsReceivers[0];
                    //lRemark = lRemarks[0];
                }

                if (lTransport == null || lTransport == "")
                {
                    var lProjectM = db.Project.Find(reqorderSumData.pCustomerCode, reqorderSumData.pProjectCode);
                    if (lProjectM != null && lProjectM.TransportMode != null && lProjectM.TransportMode != "")
                    {
                        lTransport = lProjectM.TransportMode;
                        for (int i = 0; i < lPONos.Count; i++)
                        {
                            if (i < lTransports.Count)
                            {
                                if (lTransports[i] == null || lTransports[i] == "")
                                {
                                    lTransports[i] = lProjectM.TransportMode;
                                }
                            }
                        }
                    }
                }

                //ViewBag.MainTranspoprt = lTransport;
                lOrderList.lTransport = lTransport;
                lOrderList.lRemark = lRemarks;
                lOrderList.lSiteContact = lSiteContact;
                lOrderList.lHandphone = lHandphone;
                lOrderList.lGoodsReceiver = lGoodsReceiver;
                lOrderList.lSiteContacts = lSiteContacts;
                lOrderList.lHandphones = lHandphones;
                lOrderList.lGoodsReceivers = lGoodsReceivers;
                lOrderList.lGoodsReceiverHandphones = lGoodsReceiverHandphones;
                lOrderList.AdditionalRemark = AdditionalRemarks;
                if (lOrderNoList != null && lOrderNoList.Count > 0)
                {
                    int lMyOrder = 0;
                    int.TryParse(lOrderNoList[0], out lMyOrder);
                    var lOrderHeader = db.OrderProject.Find(lMyOrder);

                    if (lPODate == null)
                    {
                        lPODate = lOrderHeader.PODate == null ? DateTime.Now : (DateTime)lOrderHeader.PODate;
                    }

                    if (lPODate == null)
                    {
                        lPODate = DateTime.Now;
                    }

                    var lUsers = lUserName.Split('@');
                    if (lUsers != null && lUsers.Length == 2 && lUsers[1].ToLower().Trim() != "natsteel.com.sg" && (lOrderStatus == "New" || lOrderStatus == "Created"))
                    {
                        lPODate = DateTime.Now;
                    }

                    lOrderList.MainPONo = ((lOrderHeader != null && lOrderHeader.PONumber != null) ? lOrderHeader.PONumber : lPONumber);
                    lOrderList.MainPODate = lPODate.ToString("yyyy-MM-dd");
                    lOrderList.MainRequiredDate = (lOrderHeader != null && lOrderHeader.RequiredDate != null) ? ((DateTime)lOrderHeader.RequiredDate).ToString("yyyy-MM-dd") : lRequiredDate;
                    lOrderList.MainTranspoprt = (lOrderHeader != null && lOrderHeader.TransportMode != null) ? lOrderHeader.TransportMode : lTransport;
                    lOrderList.lPONumber = ((lOrderHeader != null && lOrderHeader.PONumber != null && lOrderHeader.PONumber != "") ? lOrderHeader.PONumber : lPONumber); ;
                    lOrderList.lPODate = lPODate.ToString("yyyy-MM-dd"); ;
                    lOrderList.lRequiredDate = (lOrderHeader != null && lOrderHeader.RequiredDate != null) ? ((DateTime)lOrderHeader.RequiredDate).ToString("yyyy-MM-dd") : lRequiredDate;
                    lOrderList.lTransport = (lOrderHeader != null && lOrderHeader.TransportMode != null) ? lOrderHeader.TransportMode : lTransport;
                    //fill individual if it is empty
                    if (lOrderHeader != null && lOrderHeader.OrderNumber > 0)
                    {
                        if (lPONos.Count > 0)
                        {
                            for (int i = 0; i < lPONos.Count; i++)
                            {
                                if (lOrderHeader.PONumber != null && lOrderHeader.PONumber != ""
                                    && (lPONos[i] == null || lPONos[i] == ""))
                                {
                                    lPONos[i] = lOrderHeader.PONumber;
                                }
                                if (lOrderHeader.RequiredDate != null && lOrderHeader.RequiredDate > DateTime.Now.AddDays(-1)
                                    && (lRDs[i] == null || lRDs[i] == ""))
                                {
                                    lRDs[i] = ((DateTime)lOrderHeader.RequiredDate).ToString("yyyy-MM-dd");
                                }
                                if (lOrderHeader.TransportMode != null && lOrderHeader.TransportMode != ""
                                    && (lTransports[i] == null || lTransports[i] == ""))
                                {
                                    lTransports[i] = lOrderHeader.TransportMode;
                                }

                            }
                        }
                    }

                    if (lOrderHeader != null && lOrderHeader.OrderNumber > 0)
                    {
                        //ViewData["SiteEngr_Name"] = lOrderHeader.SiteEngr_Name;
                        //ViewData["SiteEngr_HP"] = lOrderHeader.SiteEngr_HP;
                        //ViewData["SiteEngr_Email"] = lOrderHeader.SiteEngr_Email;
                        //ViewData["Scheduler_Name"] = lOrderHeader.Scheduler_Name;
                        //ViewData["Scheduler_HP"] = lOrderHeader.Scheduler_HP;
                        //ViewData["Scheduler_Email"] = lOrderHeader.Scheduler_Email;
                        //ViewData["Remarks"] = lOrderHeader.Remarks;
                        //ViewData["DeliveryAddress"] = lOrderHeader.DeliveryAddress;
                        lOrderList.SiteEngr_Name = lOrderHeader.SiteEngr_Name;
                        lOrderList.SiteEngr_HP = lOrderHeader.SiteEngr_HP;
                        lOrderList.SiteEngr_Email = lOrderHeader.SiteEngr_Email;
                        lOrderList.Scheduler_Name = lOrderHeader.Scheduler_Name;
                        lOrderList.Scheduler_HP = lOrderHeader.Scheduler_HP;
                        lOrderList.Scheduler_Email = lOrderHeader.Scheduler_Email;
                        lOrderList.Remarks = lOrderHeader.Remarks;
                        lOrderList.DeliveryAddress = lOrderHeader.DeliveryAddress;
                        lOrderList.Address = lOrderHeader.Address;
                        lOrderList.Gate = lOrderHeader.Gate;

                    }
                    else
                    {
                        //ViewData["SiteEngr_Name"] = "";
                        //ViewData["SiteEngr_HP"] = "";
                        //ViewData["SiteEngr_Email"] = "";
                        //ViewData["Scheduler_Name"] = "";
                        //ViewData["Scheduler_HP"] = "";
                        //ViewData["Scheduler_Email"] = "";
                        //ViewData["Remarks"] = "";
                        //ViewData["DeliveryAddress"] = "";
                        lOrderList.SiteEngr_Name = "";
                        lOrderList.SiteEngr_HP = "";
                        lOrderList.SiteEngr_Email = "";
                        lOrderList.Scheduler_Name = "";
                        lOrderList.Scheduler_HP = "";
                        lOrderList.Scheduler_Email = "";
                        lOrderList.Remarks = "";
                        lOrderList.DeliveryAddress = "";
                        lOrderList.Address = "";
                        lOrderList.Gate = "";

                    }


                }

                //ViewBag.PONos = lPONos;
                //ViewBag.RequiredDates = lRDs;
                lOrderList.lTransports = lTransports;
                lOrderList.lPONos = lPONos;
                lOrderList.lRDs = lRDs;
                //lOrderList.lRemark = lRemarks;


                //ViewBag.OrderNo = pOrderNo.Split(',');
                lOrderList.pOrderNo = reqorderSumData.pOrderNo;

                //if (Array.TrueForAll((string[])ViewBag.OrderNo, value => { int s = 0; Int32.TryParse(value, out s); return (s > 0); }))
                //{

                //}//Commected by ajit

                return Ok(lOrderList);
            }
            catch (Exception ex)
            {
                var lMsg = ex.Message;
                lProcessObj.SaveErrorMsg(ex.Message, ex.StackTrace);
                return BadRequest("error: " + ex.Message);
            }
        }

        string ChineseCovertSE(string pSE)
        {
            string lReturn = "";
            if (pSE == null) pSE = "";
            pSE = pSE.Trim();
            pSE = pSE.ToUpper();
            if (pSE == "BEAM")
            {
                lReturn = "梁";
            }
            else if (pSE == "COLUMN")
            {
                lReturn = "柱";
            }
            else if (pSE == "SLAB")
            {
                lReturn = "板";
            }
            else if (pSE == "SLAB-B")
            {
                lReturn = "下板";
            }
            else if (pSE == "SLAB-T")
            {
                lReturn = "上板";
            }
            else if (pSE == "WALL")
            {
                lReturn = "墙";
            }
            else if (pSE == "PILE")
            {
                lReturn = "桩";
            }
            else if (pSE == "DWALL")
            {
                lReturn = "连续墙";
            }
            return lReturn;

        }



        [HttpGet]
        [Route("/checkOrderDocs/{OrderNumber}/{StructureElement}/{ProductType}/{ScheduledProd}")]
        public ActionResult checkOrderDocs(int OrderNumber, string StructureElement, string ProductType, string ScheduledProd)
        {
            StructureElement = StructureElement.Trim();
            ProductType = ProductType.Trim();
            ScheduledProd = ScheduledProd.Trim();
            var lReturn = (new[]{ new
            {
                OrderNumber = 0,
                StructureElement = "",
                ProductType = "",
                ScheduledProd = "",
                DrawingID = 0,
                DrawingNo = "",
                FileName = "",
                Revision = 0,
                Remarks = "",
                UpdatedDate = DateTime.Now,
                UpdatedBy = "",
                Source = ""
            }}).ToList();

            if (lReturn.Count > 0)
            {
                lReturn.RemoveAt(0);
            }


            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();

            lCmd.CommandText =
            "SELECT DocID, " +
            "DocumentNo, " +
            "FileName, " +
            "RevID, " +
            "Remarks, " +
            "UpdatedDate, " +
            "UpdatedBy " +
            "FROM dbo.OESOrderDocs " +
            "WHERE OrderNumber = " + OrderNumber.ToString() + " " +
            "AND StructureElement = '" + StructureElement + "' " +
            "AND ProductType = '" + ProductType + "' " +
            "AND ScheduledProd = '" + ScheduledProd + "' " +
            "AND ItemID = 0 " +
            "ORDER BY DocID ";

            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        var lRecord = new
                        {
                            OrderNumber = OrderNumber,
                            StructureElement = StructureElement,
                            ProductType = ProductType,
                            ScheduledProd = ScheduledProd,
                            DrawingID = lRst.GetInt32(0),
                            DrawingNo = lRst.GetValue(1) == System.DBNull.Value ? "" : lRst.GetString(1).Trim(),
                            FileName = lRst.GetString(2),
                            Revision = lRst.GetInt32(3),
                            Remarks = lRst.GetValue(4) == System.DBNull.Value ? "" : lRst.GetString(4).Trim(),
                            UpdatedDate = lRst.GetDateTime(5),
                            UpdatedBy = lRst.GetString(6),
                            Source = "DB"
                        };
                        lReturn.Add(lRecord);
                    }
                }
                lRst.Close();

                lCmd.CommandText =
                "SELECT W.DrawingID, " +
                "W.DrawingNo, " +
                "W.FileName, " +
                "O.Revision, " +
                "W.Remarks, " +
                // "W.UpdatedDate, " +
                //"W.UpdatedBy " +
                "O.UpdatedDate ," +
                "O.UpdatedBy " +
                "FROM dbo.OESDrawings W, dbo.OESDrawingsOrder O " +
                "WHERE W.DrawingID = O.DrawingID " +
                "AND O.OrderNumber = " + OrderNumber.ToString() + " " +
                "AND O.StructureElement = '" + StructureElement + "' " +
                "AND O.ProductType = '" + ProductType + "' " +
                "AND O.ScheduledProd = '" + ScheduledProd + "' " +
                "ORDER BY W.DrawingID ";

                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        var lRecord = new
                        {
                            OrderNumber = OrderNumber,
                            StructureElement = StructureElement,
                            ProductType = ProductType,
                            ScheduledProd = ScheduledProd,
                            DrawingID = lRst.GetInt32(0),
                            DrawingNo = lRst.GetValue(1) == System.DBNull.Value ? "" : lRst.GetString(1).Trim(),
                            FileName = lRst.GetString(2),
                            Revision = lRst.GetInt32(3),
                            Remarks = lRst.GetValue(4) == System.DBNull.Value ? "" : lRst.GetString(4).Trim(),
                            UpdatedDate = lRst.GetDateTime(5),
                            UpdatedBy = lRst.GetString(6),
                            Source = "SP"
                        };
                        lReturn.Add(lRecord);
                    }
                }
                lRst.Close();

                lProcessObj.CloseNDSConnection(ref lNDSCon);
            }
            lProcessObj = null;


            lCmd = null;
            lNDSCon = null;
            lRst = null;
            return Ok(lReturn);
        }


        [HttpPost]
        [Route("/SaveOrderDetails")]
        public async Task<ActionResult> SaveOrderDetails([FromBody] SaveOrderDetailsDto saveOrderDetailsDto)
        {
            var lProcessObj = new ProcessController();

            int lOrderNo = 0;
            decimal lTotalWt = 0;
            string lSubmission = "No";
            string lEditable = "No";
            try
            {
                UserAccessController lUa = new UserAccessController();
                var Updateby = saveOrderDetailsDto.pOrderHeader.UpdateBy;
                var lUserType = lUa.getUserType(saveOrderDetailsDto.pOrderHeader.UpdateBy); // "Vishal.Wani@natsteel.com.sg";//lUa.getUserType(Updateby);
                var lGroupName = lUa.getGroupName(lUserType);

                lUa = null;


                if (saveOrderDetailsDto.pOrderHeader == null)
                {
                    return BadRequest("Invlid Order Header.");
                }

                //get Access right;
                if (lUserType == "CU" || lUserType == "CA" || lUserType == "CM")
                {
                    var lAccess = db.UserAccess.Find(User.Identity.Name, saveOrderDetailsDto.pOrderHeader.CustomerCode, saveOrderDetailsDto.pOrderHeader.ProjectCode);
                    if (lAccess != null)
                    {
                        lSubmission = lAccess.OrderSubmission.Trim();
                        lEditable = lAccess.OrderCreation.Trim();
                    }
                }
                else
                {
                    if (lUserType == "TE" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
                    {
                        lSubmission = "Yes";
                        lEditable = "Yes";
                    }
                }

                // now allow no Order Cart

                //if (pOrderCart == null || pOrderCart.Count == 0)
                //{
                //    return Json(new
                //    {
                //        success = false,
                //        responseText = "Invlid Order Cart."
                //    }, JsonRequestBehavior.AllowGet);
                //}

                lOrderNo = saveOrderDetailsDto.pOrderHeader.OrderNumber;
                if (lOrderNo == 0)
                {
                    if (saveOrderDetailsDto.pOrderCart.Count > 0)
                    {
                        AddToCart obj = new AddToCart();

                        obj.pCustomerCode = saveOrderDetailsDto.pOrderHeader.CustomerCode;
                        obj.pProjectCode = saveOrderDetailsDto.pOrderHeader.ProjectCode;
                        obj.pOrderType = saveOrderDetailsDto.pOrderHeader.OrderType;
                        obj.pOrderNo = saveOrderDetailsDto.pOrderHeader.OrderNumber;
                        obj.pStructureElement = saveOrderDetailsDto.pOrderCart[0].StructureElement;
                        obj.pProductType = saveOrderDetailsDto.pOrderCart[0].ProductType;
                        obj.pWBS1 = saveOrderDetailsDto.pOrderHeader.WBS1;
                        obj.pWBS2 = saveOrderDetailsDto.pOrderHeader.WBS2;
                        obj.pWBS3 = saveOrderDetailsDto.pOrderHeader.WBS3;
                        obj.pPONo = saveOrderDetailsDto.pOrderCart[0].PONumber;
                        obj.pScheduledProd = (string)(saveOrderDetailsDto.pOrderCart[0].ScheduledProd == null ? "N" : saveOrderDetailsDto.pOrderCart[0].ScheduledProd.Trim());
                        obj.pPostID = (int)(saveOrderDetailsDto.pOrderCart[0].PostHeaderID == null ? 0 : saveOrderDetailsDto.pOrderCart[0].PostHeaderID);

                        lOrderNo = AddToCart(
                            obj
                            );

                        saveOrderDetailsDto.pOrderHeader.OrderNumber = lOrderNo;
                    }
                    else
                    {
                        return BadRequest("Invlid Order Number.");
                    }
                }

                if (saveOrderDetailsDto.pOrderCart != null && saveOrderDetailsDto.pOrderCart.Count > 0)
                {
                    // change from MT to KG as show to user is MT
                    for (int i = 0; i < saveOrderDetailsDto.pOrderCart.Count; i++)
                    {
                        saveOrderDetailsDto.pOrderCart[i].TotalWeight = (saveOrderDetailsDto.pOrderCart[i].TotalWeight == null ? 0 : saveOrderDetailsDto.pOrderCart[i].TotalWeight) * 1000;
                    }

                    for (int i = 0; i < saveOrderDetailsDto.pOrderCart.Count; i++)
                    {
                        lTotalWt = (decimal)(lTotalWt + (saveOrderDetailsDto.pOrderCart[i].TotalWeight == null ? 0 : saveOrderDetailsDto.pOrderCart[i].TotalWeight));
                    }
                }

                var lHeader = db.OrderProject.Find(lOrderNo);
                if (lHeader == null || lHeader.OrderNumber == 0)
                {
                    return BadRequest("Invlid Order Number.");
                }

                if (lHeader.OrderStatus == null || lHeader.OrderStatus == ""
                    || lHeader.OrderStatus == "New" || lHeader.OrderStatus == "Created"
                    || lHeader.OrderStatus == "Reserved" || lHeader.OrderStatus == "Created*"
                    || lHeader.OrderStatus == "Sent" && lSubmission == "Yes")
                {
                    var lNewHeader = lHeader;

                    string lOrderType = "WBS";
                    if (saveOrderDetailsDto.pOrderHeader.WBS1 == null || saveOrderDetailsDto.pOrderHeader.WBS1 == "")
                    {
                        lOrderType = "NONWBS";
                    }

                    lNewHeader.OrderType = lOrderType;
                    lNewHeader.DeliveryAddress = saveOrderDetailsDto.pOrderHeader.DeliveryAddress;
                    if (lNewHeader.OrderSource == null || lNewHeader.OrderSource == "")
                    {
                        lNewHeader.OrderSource = "UX";
                    }
                    if (lHeader.OrderStatus == "New")
                    {
                        lNewHeader.OrderStatus = "Created";
                    }
                    lNewHeader.Scheduler_Name = saveOrderDetailsDto.pOrderHeader.Scheduler_Name;
                    lNewHeader.Scheduler_HP = saveOrderDetailsDto.pOrderHeader.Scheduler_HP;
                    lNewHeader.Scheduler_Email = saveOrderDetailsDto.pOrderHeader.Scheduler_Email;
                    lNewHeader.SiteEngr_Name = saveOrderDetailsDto.pOrderHeader.SiteEngr_Name;
                    lNewHeader.SiteEngr_HP = saveOrderDetailsDto.pOrderHeader.SiteEngr_HP;
                    lNewHeader.SiteEngr_Email = saveOrderDetailsDto.pOrderHeader.SiteEngr_Email;
                    lNewHeader.Remarks = saveOrderDetailsDto.pOrderHeader.Remarks;
                    lNewHeader.WBS1 = saveOrderDetailsDto.pOrderHeader.WBS1;
                    lNewHeader.WBS2 = saveOrderDetailsDto.pOrderHeader.WBS2;
                    lNewHeader.WBS3 = saveOrderDetailsDto.pOrderHeader.WBS3;
                    lNewHeader.TotalWeight = lTotalWt;
                    lNewHeader.PODate = saveOrderDetailsDto.pOrderHeader.PODate;
                    if (lNewHeader.PODate == null || lNewHeader.PODate > DateTime.Now)
                    {
                        lNewHeader.PODate = DateTime.Now;
                    }
                    lNewHeader.PONumber = saveOrderDetailsDto.pOrderHeader.PONumber;
                    var lReqDate = saveOrderDetailsDto.pOrderHeader.RequiredDate;
                    lNewHeader.RequiredDate = lReqDate;
                    lNewHeader.OrigReqDate = lReqDate;
                    lNewHeader.TransportMode = saveOrderDetailsDto.pOrderHeader.TransportMode;
                    lNewHeader.UpdateBy = Updateby;
                    lNewHeader.UpdateDate = DateTime.Now;
                    lNewHeader.Address= saveOrderDetailsDto.pOrderHeader.Address;
                    lNewHeader.Gate = saveOrderDetailsDto.pOrderHeader.Gate;

                    db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);

                    if (saveOrderDetailsDto.pOrderCart != null && saveOrderDetailsDto.pOrderCart.Count > 0)
                    {
                        for (int i = 0; i < saveOrderDetailsDto.pOrderCart.Count; i++)
                        {
                            var lOldCart = db.OrderProjectSE.Find(lOrderNo, saveOrderDetailsDto.pOrderCart[i].StructureElement, saveOrderDetailsDto.pOrderCart[i].ProductType, saveOrderDetailsDto.pOrderCart[i].ScheduledProd == null ? "N" : saveOrderDetailsDto.pOrderCart[i].ScheduledProd.Trim());
                            if (lOldCart == null || lOldCart.OrderNumber == 0)
                            {
                                AddToCart obj = new AddToCart();

                                obj.pCustomerCode = lHeader.CustomerCode;
                                obj.pProjectCode = lHeader.ProjectCode;
                                obj.pOrderType = lHeader.OrderType;
                                obj.pOrderNo = lHeader.OrderNumber;
                                obj.pStructureElement = saveOrderDetailsDto.pOrderCart[i].StructureElement;
                                obj.pProductType = saveOrderDetailsDto.pOrderCart[i].ProductType;
                                obj.pWBS1 = lHeader.WBS1;
                                obj.pWBS2 = lHeader.WBS2;
                                obj.pWBS3 = lHeader.WBS3;
                                obj.pPONo = saveOrderDetailsDto.pOrderCart[i].PONumber;
                                obj.pScheduledProd = (string)(saveOrderDetailsDto.pOrderCart[i].ScheduledProd == null ? "N" : saveOrderDetailsDto.pOrderCart[i].ScheduledProd.Trim());
                                obj.pPostID = (int)(saveOrderDetailsDto.pOrderCart[i].PostHeaderID == null ? 0 : saveOrderDetailsDto.pOrderCart[i].PostHeaderID);

                                int lCreatedOrder = AddToCart(
                                 obj
                                    );

                                lOldCart = db.OrderProjectSE.Find(lOrderNo, saveOrderDetailsDto.pOrderCart[i].StructureElement, saveOrderDetailsDto.pOrderCart[i].ProductType, saveOrderDetailsDto.pOrderCart[i].ScheduledProd.Trim());
                            }
                            var lNewCart = lOldCart;
                            if (lNewCart.OrderStatus == null || lNewCart.OrderStatus == "" || lNewCart.OrderStatus == "New")
                            {
                                lNewCart.OrderStatus = "Created";
                            }
                            if (lNewCart.PODate == null || lNewCart.PODate > DateTime.Now)
                            {
                                lNewCart.PODate = DateTime.Now;
                            }
                            lNewCart.PONumber = saveOrderDetailsDto.pOrderCart[i].PONumber;
                            var lReqDateItem = saveOrderDetailsDto.pOrderCart[i].RequiredDate;
                            lNewCart.RequiredDate = lReqDateItem;
                            lNewCart.OrigReqDate = lReqDateItem;
                            lNewCart.TransportMode = saveOrderDetailsDto.pOrderCart[i].TransportMode;
                            lNewCart.TotalWeight = saveOrderDetailsDto.pOrderCart[i].TotalWeight;
                            lNewCart.TotalPCs = saveOrderDetailsDto.pOrderCart[i].TotalPCs;
                            lNewCart.PostHeaderID = saveOrderDetailsDto.pOrderCart[i].PostHeaderID;
                            lNewCart.SpecialRemark = saveOrderDetailsDto.pOrderCart[i].SpecialRemark;
                            lNewCart.SiteContact = saveOrderDetailsDto.pOrderCart[i].SiteContact;
                            lNewCart.Handphone = saveOrderDetailsDto.pOrderCart[i].Handphone;
                            lNewCart.GoodsReceiver = saveOrderDetailsDto.pOrderCart[i].GoodsReceiver;
                            lNewCart.GoodsReceiverHandphone = saveOrderDetailsDto.pOrderCart[i].GoodsReceiverHandphone;
                            lNewCart.AdditionalRemark = saveOrderDetailsDto.pOrderCart[i].AdditionalRemark;

                            lNewCart.PODate = saveOrderDetailsDto.pOrderCart[i].PODate;
                            if (lNewCart.PODate == null || lNewCart.PODate > DateTime.Now)
                            {
                                lNewCart.PODate = DateTime.Now;
                            }


                            db.Entry(lOldCart).CurrentValues.SetValues(lNewCart);

                            if (lNewCart.CABJobID > 0)
                            {
                                int lJobID = lNewCart.CABJobID;
                                var lCAB = db.JobAdvice.Find(lNewHeader.CustomerCode, lNewHeader.ProjectCode, lJobID);

                                if (lCAB != null && lCAB.JobID > 0)
                                {
                                    var lNewCAB = lCAB;
                                    if (lNewCAB.OrderSource == null || lNewCAB.OrderSource == "")
                                    {
                                        lNewCAB.OrderSource = "UX";
                                    }
                                    lNewCAB.DeliveryAddress = lNewHeader.DeliveryAddress;
                                    //lNewCAB.OrderStatus = "Created";
                                    lNewCAB.PODate = (DateTime)lNewCart.PODate;
                                    lNewCAB.PONumber = lNewCart.PONumber;
                                    lNewCAB.Remarks = lNewHeader.Remarks;
                                    lNewCAB.RequiredDate = lNewCart.RequiredDate == null ? DateTime.Now.AddDays(5) : (DateTime)lNewCart.RequiredDate;
                                    lNewCAB.Scheduler_HP = lNewHeader.Scheduler_HP;
                                    lNewCAB.Scheduler_Name = lNewHeader.Scheduler_Name;
                                    lNewCAB.Scheduler_Tel = lNewHeader.SiteEngr_Email;
                                    lNewCAB.SiteEngr_HP = lNewHeader.SiteEngr_HP;
                                    lNewCAB.SiteEngr_Name = lNewHeader.SiteEngr_Name;
                                    lNewCAB.SiteEngr_Tel = lNewHeader.SiteEngr_Name;
                                    lNewCAB.TransportMode = lNewCart.TransportMode;
                                    lNewCAB.UpdateBy = Updateby;
                                    lNewCAB.UpdateDate = DateTime.Now;
                                    lNewCAB.WBS1 = lNewHeader.WBS1;
                                    lNewCAB.WBS2 = lNewHeader.WBS2;
                                    lNewCAB.WBS3 = lNewHeader.WBS3;


                                    db.Entry(lCAB).CurrentValues.SetValues(lNewCAB);

                                    var lCABBBS = db.BBS.Find(lNewHeader.CustomerCode, lNewHeader.ProjectCode, lJobID, 1);
                                    if (lCABBBS != null && lCABBBS.JobID > 0)
                                    {
                                        if (lCABBBS.BBSNo == "BBS" + lJobID.ToString())
                                        {
                                            if (lNewCart.PONumber != null && lNewCart.PONumber != "")
                                            {
                                                var lNewCABBBS = lCABBBS;
                                                lNewCABBBS.BBSNo = lNewCart.PONumber;
                                                lNewCABBBS.BBSNo = lNewCABBBS.BBSNo.Replace("/", "");
                                                lNewCABBBS.BBSNo = lNewCABBBS.BBSNo.Replace("&", "");

                                                if (lNewCABBBS.BBSNo.Length > 14)
                                                {
                                                    lNewCABBBS.BBSNo = lNewCABBBS.BBSNo.Replace("-", "");

                                                    if (lNewCABBBS.BBSNo.Length > 14)
                                                    {
                                                        lNewCABBBS.BBSNo = lNewCABBBS.BBSNo.Substring(lNewCABBBS.BBSNo.Length - 14);
                                                    }
                                                }
                                                db.Entry(lCABBBS).CurrentValues.SetValues(lNewCABBBS);
                                            }
                                        }
                                    }

                                }
                            }
                            if (lNewCart.MESHJobID > 0)
                            {
                                int lJobID = lNewCart.MESHJobID;
                                var lMESH = db.CTSMESHJobAdvice.Find(lNewHeader.CustomerCode, lNewHeader.ProjectCode, lJobID);

                                if (lMESH != null && lMESH.JobID > 0)
                                {
                                    var lNewMESH = lMESH;
                                    lNewMESH.OrderSource = "UX";
                                    lNewMESH.DeliveryAddress = lNewHeader.DeliveryAddress;
                                    //lNewMESH.OrderStatus = "Created";
                                    lNewMESH.PODate = (DateTime)lNewCart.PODate;
                                    lNewMESH.PONumber = lNewCart.PONumber;
                                    lNewMESH.Remarks = lNewHeader.Remarks;
                                    lNewMESH.RequiredDate = (DateTime)(lNewCart.RequiredDate == null ? DateTime.Now.AddDays(3) : lNewCart.RequiredDate);
                                    lNewMESH.Scheduler_HP = lNewHeader.Scheduler_HP;
                                    lNewMESH.Scheduler_Name = lNewHeader.Scheduler_Name;
                                    lNewMESH.Scheduler_Tel = lNewHeader.SiteEngr_Email;
                                    lNewMESH.SiteEngr_HP = lNewHeader.SiteEngr_HP;
                                    lNewMESH.SiteEngr_Name = lNewHeader.SiteEngr_Name;
                                    lNewMESH.SiteEngr_Tel = lNewHeader.SiteEngr_Name;
                                    lNewMESH.Transport = lNewCart.TransportMode;
                                    lNewMESH.TotalPcs = (int)(lNewCart.TotalPCs == null ? 0 : lNewCart.TotalPCs);
                                    lNewMESH.TotalWeight = (decimal)(lNewCart.TotalWeight == null ? 0 : lNewCart.TotalWeight);
                                    lNewMESH.UpdateBy = Updateby;
                                    lNewMESH.UpdateDate = DateTime.Now;
                                    lNewMESH.WBS1 = lNewHeader.WBS1;
                                    lNewMESH.WBS2 = lNewHeader.WBS2;
                                    lNewMESH.WBS3 = lNewHeader.WBS3;

                                    db.Entry(lMESH).CurrentValues.SetValues(lNewMESH);
                                }
                                var lMESHBBS = db.CTSMESHBBS.Find(lNewHeader.CustomerCode, lNewHeader.ProjectCode, lJobID, 1);

                                if (lMESHBBS != null && lMESHBBS.JobID > 0)
                                {
                                    var lNewMESHBBS = lMESHBBS;
                                    lNewMESHBBS.BBSTotalPcs = (int)(lNewCart.TotalPCs == null ? 0 : lNewCart.TotalPCs);
                                    lNewMESHBBS.BBSTotalWT = (decimal)(lNewCart.TotalWeight == null ? 0 : lNewCart.TotalWeight);
                                    lNewMESHBBS.UpdateBy = Updateby;
                                    lNewMESHBBS.UpdateDate = DateTime.Now;

                                    db.Entry(lMESHBBS).CurrentValues.SetValues(lNewMESHBBS);
                                }
                            }
                            if (lNewCart.BPCJobID > 0)
                            {
                                int lJobID = lNewCart.BPCJobID;
                                var lBPC = db.BPCJobAdvice.Find(lNewHeader.CustomerCode, lNewHeader.ProjectCode, false, lJobID);

                                if (lBPC != null && lBPC.JobID > 0)
                                {
                                    var lNewBPC = lBPC;
                                    lNewBPC.OrderSource = "UX";
                                    lNewBPC.DeliveryAddress = lNewHeader.DeliveryAddress;
                                    //lNewBPC.OrderStatus = "Created";
                                    lNewBPC.PODate = (DateTime)lNewCart.PODate;
                                    lNewBPC.PONumber = lNewCart.PONumber;
                                    lNewBPC.Remarks = lNewHeader.Remarks;
                                    lNewBPC.RequiredDate = (DateTime)(lNewCart.RequiredDate == null ? DateTime.Now.AddDays(5) : lNewCart.RequiredDate);
                                    lNewBPC.Scheduler_HP = lNewHeader.Scheduler_HP;
                                    lNewBPC.Scheduler_Name = lNewHeader.Scheduler_Name;
                                    lNewBPC.Scheduler_Tel = lNewHeader.SiteEngr_Email;
                                    lNewBPC.SiteEngr_HP = lNewHeader.SiteEngr_HP;
                                    lNewBPC.SiteEngr_Name = lNewHeader.SiteEngr_Name;
                                    lNewBPC.SiteEngr_Tel = lNewHeader.SiteEngr_Name;
                                    lNewBPC.Transport = lNewCart.TransportMode;
                                    lNewBPC.UpdateBy = Updateby;
                                    lNewBPC.UpdateDate = DateTime.Now;

                                    db.Entry(lBPC).CurrentValues.SetValues(lNewBPC);
                                }
                            }
                            if (lNewCart.CageJobID > 0)
                            {
                                int lJobID = lNewCart.CageJobID;
                                var lPRC = db.PRCJobAdvice.Find(lNewHeader.CustomerCode, lNewHeader.ProjectCode, lJobID);

                                if (lPRC != null && lPRC.JobID > 0)
                                {
                                    var lNewPRC = lPRC;
                                    lNewPRC.OrderSource = "UX";
                                    lNewPRC.DeliveryAddress = lNewHeader.DeliveryAddress;
                                    //lNewPRC.OrderStatus = "Created";
                                    lNewPRC.PODate = (DateTime)lNewCart.PODate;
                                    lNewPRC.PONumber = lNewCart.PONumber;
                                    lNewPRC.Remarks = lNewHeader.Remarks;
                                    lNewPRC.RequiredDate = lNewCart.RequiredDate == null ? DateTime.Now.AddDays(10) : lNewCart.RequiredDate; ;
                                    lNewPRC.Scheduler_HP = lNewHeader.Scheduler_HP;
                                    lNewPRC.Scheduler_Name = lNewHeader.Scheduler_Name;
                                    lNewPRC.Scheduler_Tel = lNewHeader.SiteEngr_Email;
                                    lNewPRC.SiteEngr_HP = lNewHeader.SiteEngr_HP;
                                    lNewPRC.SiteEngr_Name = lNewHeader.SiteEngr_Name;
                                    lNewPRC.SiteEngr_Tel = lNewHeader.SiteEngr_Name;
                                    lNewPRC.Transport = lNewCart.TransportMode;
                                    lNewPRC.UpdateBy = Updateby;
                                    lNewPRC.UpdateDate = DateTime.Now;

                                    db.Entry(lPRC).CurrentValues.SetValues(lNewPRC);
                                }
                            }

                            if (lNewCart.StdBarsJobID > 0 || lNewCart.StdMESHJobID > 0 || lNewCart.CoilProdJobID > 0)
                            {
                                int lJobID = 0;
                                if (lNewCart.StdBarsJobID > 0)
                                {
                                    lJobID = lNewCart.StdBarsJobID;
                                }
                                if (lNewCart.StdMESHJobID > 0)
                                {
                                    lJobID = lNewCart.StdMESHJobID;
                                }
                                if (lNewCart.CoilProdJobID > 0)
                                {
                                    lJobID = lNewCart.CoilProdJobID;
                                }

                                var lStd = db.StdSheetJobAdvice.Find(lNewHeader.CustomerCode, lNewHeader.ProjectCode, lJobID);
                                if (lStd != null && lStd.JobID > 0)
                                {
                                    var lNewStd = lStd;
                                    //lNewStd.OrderSource = "UX";
                                    lNewStd.DeliveryAddress = lNewHeader.DeliveryAddress;
                                    //lNewStd.OrderStatus = "Created";
                                    lNewStd.PODate = (DateTime)(lNewCart.PODate == null ? DateTime.Now : lNewCart.PODate);
                                    lNewStd.PONumber = lNewCart.PONumber;
                                    lNewStd.Remarks = lNewHeader.Remarks;
                                    lNewStd.RequiredDate = lNewCart.RequiredDate == null ? DateTime.Now.AddDays(3) : lNewCart.RequiredDate;
                                    lNewStd.Scheduler_HP = lNewHeader.Scheduler_HP;
                                    lNewStd.Scheduler_Name = lNewHeader.Scheduler_Name;
                                    lNewStd.Scheduler_Tel = lNewHeader.SiteEngr_Email;
                                    lNewStd.SiteEngr_HP = lNewHeader.SiteEngr_HP;
                                    lNewStd.SiteEngr_Name = lNewHeader.SiteEngr_Name;
                                    lNewStd.SiteEngr_Tel = lNewHeader.SiteEngr_Name;
                                    lNewStd.Transport = lNewCart.TransportMode;
                                    lNewStd.UpdateBy = Updateby;
                                    lNewStd.UpdateDate = DateTime.Now;

                                    db.Entry(lStd).CurrentValues.SetValues(lNewStd);
                                }
                            }
                        }
                    }

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var lMsg = ex.Message;
                lProcessObj.SaveErrorMsg(ex.Message, ex.StackTrace);
                return BadRequest("error: " + ex.Message);
            }

            return Ok(lOrderNo);
        }

        [HttpGet]
        [Route("/SaveErrorMsg/{pErrorMsg}/{pTrack}")]
        public int SaveErrorMsg(string pErrorMsg, string pTrack)
        {
            int lReturn = 0;
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            string lSQL = "";

            string lVar = pErrorMsg.Trim() + " / " + pTrack.Trim();

            if (lVar == null)
            {
                lVar = "";
            }
            lVar = lVar.Trim();
            if (lVar.Length > 8000)
            {
                lVar = lVar.Substring(0, 8000);
            }

            lVar = lVar.Replace("'", "''");

            var lSourceIP = "";
            //var lRequest =  Request; commented temporary



            //if (lRequest.GetOwinContext() != null)
            //{
            //    lSourceIP = lRequest.GetOwinContext().Request.RemoteIpAddress; //commented temp
            //}

            var lProcessObj = new ProcessController();
            lProcessObj.OpenNDSConnection(ref lNDSCon);

            lSQL = "INSERT INTO dbo.OESProjOrdersError " +
               "(ErrorMassage " +
               ", UpdateDate " +
               ", SourceIP " +
               ", UpdateBy) " +
               "VALUES " +
               "('" + lVar + "' " +
               ",getDate() " +
               ",'" + lSourceIP + "' " +
               ",'" + User.Identity.GetUserName() + "') ";

            lCmd.CommandText = lSQL;
            lCmd.Connection = lNDSCon;
            lCmd.CommandTimeout = 300;
            lCmd.ExecuteNonQuery();

            lProcessObj.CloseNDSConnection(ref lNDSCon);
            lProcessObj = null;

            return lReturn;
        }


        [HttpGet]
        [Route("/getPOHistory/{CustomerCode}/{ProjectCode}/{ProdTypes}/{UserName}")]
        public ActionResult getPOHistory(string CustomerCode, string ProjectCode, string ProdTypes, string UserName)
        {
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            string lSQL = "";
            string lUserID = UserName;//User.Identity.GetUserName();

            var lReturn = new List<string>();

            var lProdTypeList = ProdTypes.Split(',').ToList();
            if (lProdTypeList != null && lProdTypeList.Count > 0)
            {
                if (lProdTypeList.Count > 1)
                {
                    for (int i = lProdTypeList.Count - 1; i > 0; i--)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            if (lProdTypeList[i] == lProdTypeList[j])
                            {
                                lProdTypeList.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }


                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    for (int i = 0; i < lProdTypeList.Count; i++)
                    {
                        if (lProdTypeList[i] != null && lProdTypeList[i] != "")
                        {
                            lSQL =
                            "SELECT TOP 11 H.PONumber " +
                            "FROM dbo.OESProjOrder H, dbo.OESProjOrdersSE S " +
                            "WHERE H.OrderNumber = S.OrderNumber " +
                            "AND H.CustomerCode = '" + CustomerCode + "' " +
                            "AND H.ProjectCode = '" + ProjectCode + "' " +
                            "AND S.ProductType = '" + lProdTypeList[i] + "' " +
                            "AND H.OrderStatus <> 'Deleted' " +
                            "AND H.OrderStatus <> 'Cancelled' " +
                            "AND H.OrderStatus <> 'New' " +
                            "AND H.OrderStatus <> '' " +
                            "AND H.OrderStatus IS NOT NULL " +
                            "AND H.OrderStatus <> 'Reserved' " +
                            "AND H.OrderStatus <> 'Created' " +
                            "AND H.PONumber <> '' " +
                            "AND H.PONumber IS NOT NULL " +
                            "AND H.UpdateBy = '" + lUserID + "' " +
                            "GROUP BY H.PONumber " +
                            "ORDER BY H.PONumber DESC ";

                            lCmd.CommandText = lSQL;
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lRst = lCmd.ExecuteReader();
                            if (lRst.HasRows)
                            {
                                while (lRst.Read())
                                {
                                    if (lRst.GetValue(0) != DBNull.Value && lRst.GetString(0).Trim() != "")
                                    {
                                        lReturn.Add(lRst.GetString(0).Trim());
                                    }
                                }
                            }
                            lRst.Close();

                        }
                    }

                    if (lReturn.Count < 5)
                    {
                        for (int i = 0; i < lProdTypeList.Count; i++)
                        {
                            if (lProdTypeList[i] != null && lProdTypeList[i] != "")
                            {
                                lSQL =
                                "SELECT TOP 11 H.PONumber " +
                                "FROM dbo.OESProjOrder H, dbo.OESProjOrdersSE S " +
                                "WHERE H.OrderNumber = S.OrderNumber " +
                                "AND H.CustomerCode = '" + CustomerCode + "' " +
                                "AND H.ProjectCode = '" + ProjectCode + "' " +
                                "AND S.ProductType = '" + lProdTypeList[i] + "' " +
                                "AND H.OrderStatus <> 'Deleted' " +
                                "AND H.OrderStatus <> 'Cancelled' " +
                                "AND H.OrderStatus <> 'New' " +
                                "AND H.OrderStatus <> '' " +
                                "AND H.OrderStatus IS NOT NULL " +
                                "AND H.OrderStatus <> 'Reserved' " +
                                "AND H.OrderStatus <> 'Created' " +
                                "AND H.PONumber <> '' " +
                                "AND H.PONumber IS NOT NULL " +
                                "GROUP BY H.PONumber " +
                                "ORDER BY H.PONumber DESC ";

                                lCmd.CommandText = lSQL;
                                lCmd.Connection = lNDSCon;
                                lCmd.CommandTimeout = 300;
                                lRst = lCmd.ExecuteReader();
                                if (lRst.HasRows)
                                {
                                    while (lRst.Read())
                                    {
                                        if (lRst.GetValue(0) != DBNull.Value && lRst.GetString(0).Trim() != "")
                                        {
                                            lReturn.Add(lRst.GetString(0).Trim());
                                        }
                                    }
                                }
                                lRst.Close();
                            }
                        }
                    }
                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }

                lProcessObj = null;
            }
            return Json(lReturn);
        }


        [HttpGet]
        [Route("/getDeliveryAddress/{CustomerCode}/{ProjectCode}/{WBS1}/{UserName}")]
        public ActionResult getDeliveryAddress(string CustomerCode, string ProjectCode, string WBS1, string UserName)
        {
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lErrorMsg = "";
            int lFound = 0;
            int? lJobID = 0;

            if (WBS1 == null)
            {
                WBS1 = "";
            }

            var lReturn = new
            {
                SiteEngr_Name = "",
                SiteEngr_HP = "",
                SiteEngr_Email = "",
                Scheduler_Name = "",
                Scheduler_HP = "",
                Scheduler_Email = "",
                DeliveryAddress = "",
                Remarks = ""
            };


            string lUserType = "";
            UserAccessController lUa = new UserAccessController();
            lUserType = lUa.getUserType(UserName);
            lUa = null;

            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                int lLastJobID = 0;

                lCmd.CommandText =
                "SELECT isNull(MAX(OrderJobID), 0) FROM dbo.OESProjOrder " +
                "WHERE CustomerCode = '" + CustomerCode + "' " +
                "AND ProjectCode = '" + ProjectCode + "' " +
                "AND OrderStatus <> 'New' " +
                "AND OrderStatus <> 'Reserved' " +
                "AND OrderStatus <> 'Created' " +
                "AND OrderStatus <> 'Deleted' " +
                "AND OrderStatus <> 'Cancelled' " +
                "AND WBS1 = '" + WBS1.Trim() + "' " +
                "AND (SiteEngr_Name > '' " +
                "OR Scheduler_Name > '' " +
                "OR DeliveryAddress > '') ";


                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    if (lRst.Read())
                    {
                        lLastJobID = lRst.GetInt32(0);
                    }
                }
                lRst.Close();


                if (lLastJobID == 0)
                {
                    lCmd.CommandText =
                        "SELECT isNull(MAX(OrderJobID), 0) FROM dbo.OESProjOrder " +
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
                        "AND OrderStatus <> 'New' " +
                        "AND OrderStatus <> 'Reserved' " +
                        "AND OrderStatus <> 'Created' " +
                        "AND OrderStatus <> 'Deleted' " +
                        "AND OrderStatus <> 'Cancelled' " +
                        "AND SubmitBy = '" + UserName + "' " +
                        "AND(SiteEngr_Name > '' " +
                        "OR Scheduler_Name > '' " +
                        "OR DeliveryAddress > '') ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lLastJobID = lRst.GetInt32(0);
                        }
                    }
                    lRst.Close();
                }

                if (lLastJobID == 0 && WBS1 != null && WBS1.Trim() != "")
                {
                    lCmd.CommandText =
                        "SELECT isNull(MAX(OrderJobID), 0) FROM dbo.OESProjOrder " +
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
                        "AND OrderStatus <> 'New' " +
                        "AND OrderStatus <> 'Reserved' " +
                        "AND OrderStatus <> 'Created' " +
                        "AND OrderStatus <> 'Deleted' " +
                        "AND OrderStatus <> 'Cancelled' " +
                        "AND UpdateBy = '" + UserName + "' " +
                        "AND(SiteEngr_Name > '' " +
                        "OR Scheduler_Name > '' " +
                        "OR DeliveryAddress > '') ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lLastJobID = lRst.GetInt32(0);
                        }
                    }
                    lRst.Close();
                }

                if (lLastJobID == 0)
                {
                    lCmd.CommandText =
                        "SELECT isNull(MAX(OrderJobID), 0) FROM dbo.OESProjOrder " +
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
                        "AND OrderStatus <> 'New' " +
                        "AND OrderStatus <> 'Reserved' " +
                        "AND OrderStatus <> 'Created' " +
                        "AND OrderStatus <> 'Deleted' " +
                        "AND OrderStatus <> 'Cancelled' " +
                        "AND(SiteEngr_Name > '' " +
                        "OR Scheduler_Name > '' " +
                        "OR DeliveryAddress > '') ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lLastJobID = lRst.GetInt32(0);
                        }
                    }
                    lRst.Close();
                }

                if (lLastJobID > 0)
                {

                    lCmd.CommandText =
                    "SELECT isNull(SiteEngr_Name, ''), " +
                    "isNull(SiteEngr_HP, ''), " +
                    "isNull(SiteEngr_Email, ''), " +
                    "isNull(Scheduler_Name, ''), " +
                    "isNull(Scheduler_HP, ''), " +
                    "isNull(Scheduler_Email, ''), " +
                    "isNull(DeliveryAddress, ''), " +
                    "isNull(Remarks, '') " +
                    "FROM dbo.OESProjOrder " +
                    "WHERE CustomerCode = '" + CustomerCode + "' " +
                    "AND ProjectCode = '" + ProjectCode + "' " +
                    "AND OrderJobID = " + lLastJobID.ToString() + " ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lReturn = new
                            {
                                SiteEngr_Name = lRst.GetString(0),
                                SiteEngr_HP = lRst.GetString(1),
                                SiteEngr_Email = lRst.GetString(2),
                                Scheduler_Name = lRst.GetString(3),
                                Scheduler_HP = lRst.GetString(4),
                                Scheduler_Email = lRst.GetString(5),
                                DeliveryAddress = lRst.GetString(6),
                                Remarks = lRst.GetString(7)
                            };
                        }
                    }
                    lRst.Close();

                }
                else
                {
                    lCmd.CommandText =
                    "SELECT isNull(SiteEngr_Name, ''), " +
                    "isNull(SiteEngr_HP, ''), " +
                    "isNull(SiteEngr_Tel, ''), " +
                    "isNull(Scheduler_Name, ''), " +
                    "isNull(Scheduler_HP, ''), " +
                    "isNull(Scheduler_Tel, '') " +
                    "FROM dbo.OESProject " +
                    "WHERE CustomerCode = '" + CustomerCode + "' " +
                    "AND ProjectCode = '" + ProjectCode + "' ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lReturn = new
                            {
                                SiteEngr_Name = lRst.GetString(0),
                                SiteEngr_HP = lRst.GetString(1),
                                SiteEngr_Email = lRst.GetString(2),
                                Scheduler_Name = lRst.GetString(3),
                                Scheduler_HP = lRst.GetString(4),
                                Scheduler_Email = lRst.GetString(5),
                                DeliveryAddress = "",
                                Remarks = ""
                            };
                        }
                    }
                    lRst.Close();

                }

                lProcessObj.CloseNDSConnection(ref lNDSCon);
            }

            lProcessObj = null;

            return Ok(lReturn);
        }





        [HttpGet]
        [Route("/getJobId/{orderNumber}/{ProdType}/{StructurEelement}/{ScheduleProd}")]
        public ActionResult getJobId(string orderNumber, string ProdType, string StructurEelement, string ScheduleProd)
        {
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            string lSQL = "";
            string lUserID = User.Identity.GetUserName();

            var lReturn = new
            {
                CABJOBID = 0,
                MESHJobID = 0,
                BPCJobID = 0,
                CageJobID = 0,
                CarpetJobID = 0,
                StdBarsJobID = 0,
                StdMESHJobID = 0,
                CoilProdJobID = 0,
                PostHeaderID = 0
            };
            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                lSQL =
            "SELECT CABJOBID, MESHJobID, BPCJobID, CageJobID, CarpetJobID, StdBarsJobID, StdMESHJobID, CoilProdJobID, PostHeaderID " +
            "FROM dbo.OESProjOrdersSE " +
            "WHERE ORDERNUMBER =  " + orderNumber + " and ProductType='" + ProdType + " ' " + "and StructureElement='" + StructurEelement + " '" + " and ScheduledProd= '" + ScheduleProd + "'";

                lCmd.CommandText = lSQL;
                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    if (lRst.Read())
                    {
                        lReturn = new
                        {
                            CABJOBID = lRst.GetInt32(0),
                            MESHJobID = lRst.GetInt32(1),
                            BPCJobID = lRst.GetInt32(2),
                            CageJobID = lRst.GetInt32(3),
                            CarpetJobID = lRst.GetInt32(4),
                            StdBarsJobID = lRst.GetInt32(5),
                            StdMESHJobID = lRst.GetInt32(6),
                            CoilProdJobID = lRst.GetInt32(7),
                            PostHeaderID = lRst.GetInt32(8)
                        };
                    }
                }
                lRst.Close();
            }

            return Ok(lReturn);
        }


        [HttpGet]
        [Route("/setCloneOrderNonWBS/{CustomerCode}/{sProjectCode}/{dProjectCode}/{OrderNo}/{CloneNo}/{UserName}")]
        public ActionResult setCloneOrderNonWBS(string CustomerCode, string sProjectCode, string dProjectCode, int OrderNo, int CloneNo, string UserName)
        {
            var lErrorMsg = "";
            int lNewOrderNo = 0;
            try
            {
                if (CloneNo > 0)
                {
                    var lHeader = db.OrderProject.Find(OrderNo);
                    if (lHeader == null || lHeader.CustomerCode == null ||
                        lHeader.CustomerCode != CustomerCode || lHeader.ProjectCode != sProjectCode)
                    {
                        lErrorMsg = "Invalid Order Number.";
                        return BadRequest("Invalid Order Number.");
                    }

                    var lHeaderSE = (from p in db.OrderProjectSE
                                     where p.OrderNumber == OrderNo
                                     select p).ToList();
                    if (lHeaderSE == null || lHeaderSE.Count == 0)
                    {
                        lErrorMsg = "Invalid Order Number.";
                        return Ok("Invalid Order Number.");
                    }

                    for (int i = 0; i < CloneNo; i++)
                    {
                        lNewOrderNo = 0;
                        List<OrderProjectSEModels> lNewSE = new List<OrderProjectSEModels>();
                        for (int j = 0; j < lHeaderSE.Count; j++)
                        {
                            if (lHeaderSE[j].TotalWeight >= 0 || lHeaderSE[j].TotalPCs >= 0)
                            {
                                string lStrutEle = lHeaderSE[j].StructureElement;
                                if (lStrutEle == null)
                                {
                                    lStrutEle = "";
                                }
                                lStrutEle = lStrutEle.Trim();
                                if (lStrutEle.Length == 0)
                                {
                                    lStrutEle = "NONWBS";
                                }

                                string lProdType = lHeaderSE[j].ProductType;
                                if (lProdType == null)
                                {
                                    lProdType = "";
                                }
                                lProdType = lProdType.Trim();

                                string lScheduledProd = lHeaderSE[j].ScheduledProd;
                                if (lScheduledProd == null)
                                {
                                    lScheduledProd = "N";
                                }
                                lScheduledProd = lScheduledProd.Trim();

                                AddToCart obj = new AddToCart();

                                obj.pCustomerCode = CustomerCode;
                                obj.pProjectCode = dProjectCode;
                                obj.pOrderType = lHeader.OrderType;
                                obj.pOrderNo = lNewOrderNo;
                                obj.pStructureElement = lStrutEle;
                                obj.pProductType = lProdType;
                                obj.pWBS1 = lHeader.WBS1;
                                obj.pWBS2 = lHeader.WBS2;
                                obj.pWBS3 = lHeader.WBS3;
                                obj.pPONo = lHeaderSE[j].PONumber;
                                obj.pScheduledProd = lHeaderSE[j].ScheduledProd == null ? "N" : lHeaderSE[j].ScheduledProd.Trim();
                                obj.pPostID = (int)(lHeaderSE[j].PostHeaderID == null ? 0 : lHeaderSE[j].PostHeaderID);

                                lNewOrderNo = AddToCart(obj);


                                var lHeaderSENew = db.OrderProjectSE.Find(lNewOrderNo, lStrutEle, lProdType, lScheduledProd.Trim());

                                if (lHeaderSENew != null && lHeaderSENew.OrderNumber == lNewOrderNo)
                                {
                                    var lSENew = lHeaderSENew;
                                    //Clone CAB
                                    if (lHeaderSE[j].CABJobID > 0 && lHeaderSENew.CABJobID > 0)
                                    {
                                        cloneCAB(CustomerCode, sProjectCode, lHeaderSE[j].CABJobID, CustomerCode, dProjectCode, lHeaderSENew.CABJobID, UserName);
                                    }
                                    //Clone MESH
                                    if (lHeaderSE[j].MESHJobID > 0)
                                    {
                                        cloneMeshProducts(CustomerCode, sProjectCode, lHeaderSE[j].MESHJobID, CustomerCode, dProjectCode, lHeaderSENew.MESHJobID, UserName);
                                    }
                                    //Clone BPC
                                    if (lHeaderSE[j].BPCJobID > 0)
                                    {
                                        cloneBPC(CustomerCode, sProjectCode, lHeaderSE[j].BPCJobID, CustomerCode, dProjectCode, lHeaderSENew.BPCJobID, UserName);
                                    }
                                    //Clone CAGE
                                    if (lHeaderSE[j].CageJobID > 0)
                                    {

                                    }
                                    //Clone Carpet
                                    if (lHeaderSE[j].CarpetJobID > 0)
                                    {

                                    }
                                    //Clone Standrad Bar
                                    if (lHeaderSE[j].StdBarsJobID > 0)
                                    {
                                        cloneStdProducts(CustomerCode, sProjectCode, lHeaderSE[j].StdBarsJobID, CustomerCode, dProjectCode, lHeaderSENew.StdBarsJobID, UserName);
                                    }
                                    //Clone Standrad Mesh
                                    if (lHeaderSE[j].StdMESHJobID > 0)
                                    {
                                        cloneStdProducts(CustomerCode, sProjectCode, lHeaderSE[j].StdMESHJobID, CustomerCode, dProjectCode, lHeaderSENew.StdMESHJobID, UserName);
                                    }
                                    //Clone Coil Products
                                    if (lHeaderSE[j].CoilProdJobID > 0)
                                    {
                                        cloneStdProducts(CustomerCode, sProjectCode, lHeaderSE[j].CoilProdJobID, CustomerCode, dProjectCode, lHeaderSENew.CoilProdJobID, UserName);
                                    }

                                    lSENew.OrderNumber = lNewOrderNo;
                                    lSENew.OrderStatus = "Created";
                                    lSENew.StructureElement = "NONWBS";
                                    lSENew.PostHeaderID = 0;
                                    //lSENew.PODate = lHeaderSE[j].PODate;
                                    lSENew.PODate = DateTime.Now;
                                    lSENew.PONumber = lHeaderSE[j].PONumber;
                                    lSENew.RequiredDate = lHeaderSE[j].RequiredDate;
                                    lSENew.OrigReqDate = lHeaderSE[j].RequiredDate;
                                    lSENew.TotalPCs = lHeaderSE[j].TotalPCs;
                                    lSENew.TotalWeight = lHeaderSE[j].TotalWeight;
                                    lSENew.TransportMode = lHeaderSE[j].TransportMode;
                                    lSENew.UpdateBy = UserName;//User.Identity.GetUserName();
                                    lSENew.UpdateDate = DateTime.Now;

                                    db.Entry(lHeaderSENew).CurrentValues.SetValues(lSENew);
                                }
                            }
                        }
                        var lNewHeader = db.OrderProject.Find(lNewOrderNo);

                        var lOrderNew = lNewHeader;

                        lOrderNew.DeliveryAddress = lHeader.DeliveryAddress;
                        lOrderNew.OrderSource = lHeader.OrderSource;
                        lOrderNew.OrderStatus = "Created";
                        lOrderNew.OrderType = "NONWBS";
                        //lOrderNew.PODate = lHeader.PODate;
                        lOrderNew.PODate = DateTime.Now;
                        lOrderNew.PONumber = lHeader.PONumber;
                        lOrderNew.Remarks = lHeader.Remarks;
                        lOrderNew.RequiredDate = lHeader.RequiredDate;
                        lOrderNew.OrigReqDate = lHeader.RequiredDate;
                        lOrderNew.Scheduler_Email = lHeader.Scheduler_Email;
                        lOrderNew.Scheduler_HP = lHeader.Scheduler_HP;
                        lOrderNew.Scheduler_Name = lHeader.Scheduler_Name;
                        lOrderNew.SiteEngr_Email = lHeader.SiteEngr_Email;
                        lOrderNew.SiteEngr_HP = lHeader.SiteEngr_HP;
                        lOrderNew.SiteEngr_Name = lHeader.SiteEngr_Name;
                        lOrderNew.SubmitBy = lHeader.SubmitBy;
                        lOrderNew.SubmitDate = lHeader.SubmitDate;
                        lOrderNew.TotalWeight = lHeader.TotalWeight;
                        lOrderNew.TransportMode = lHeader.TransportMode;
                        lOrderNew.UpdateBy = UserName;//User.Identity.GetUserName();
                        lOrderNew.UpdateDate = DateTime.Now;
                        lOrderNew.WBS1 = "";
                        lOrderNew.WBS2 = "";
                        lOrderNew.WBS3 = "";
                        lOrderNew.GreenSteel = lHeader.GreenSteel;
                        lOrderNew.Address = lHeader.Address;
                        lOrderNew.Gate = lHeader.Gate;
                        lOrderNew.AddressCode = lHeader.AddressCode;
                        //lOrderNew.OrderReferenceNo = lHeader.OrderReferenceNo;

                        db.Entry(lNewHeader).CurrentValues.SetValues(lOrderNew);
                        db.SaveChanges();
                    }
                }
                return Json("Successfully saved.");
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            return Ok();
        }


        [HttpGet]
        [Route("/cloneStdProducts/{pOldCustomerCode}/{pOldProjectCode}/{pOldJobID}/{pNewCustomerCode}/{pNewProjectCode}/{pNewJobID}")]
        public int cloneStdProducts(string pOldCustomerCode, string pOldProjectCode, int pOldJobID, string pNewCustomerCode, string pNewProjectCode, int pNewJobID, string UserName)
        {
            int lReturn = 0;

            var lJobNewOr = db.StdSheetJobAdvice.Find(pNewCustomerCode, pNewProjectCode, pNewJobID);
            var lJobNew = lJobNewOr;
            var lJobOld = db.StdSheetJobAdvice.Find(pOldCustomerCode, pOldProjectCode, pOldJobID);
            if (lJobOld != null && lJobNew != null)
            {
                lJobNew.OrderStatus = "New";
                //lJobNew.PODate = lJobOld.PODate;
                lJobNew.PODate = DateTime.Now;
                lJobNew.PONumber = lJobOld.PONumber;
                lJobNew.DeliveryAddress = lJobOld.DeliveryAddress;
                lJobNew.Remarks = lJobOld.Remarks;
                lJobNew.RequiredDate = lJobOld.RequiredDate;
                lJobNew.Scheduler_HP = lJobOld.Scheduler_HP;
                lJobNew.Scheduler_Name = lJobOld.Scheduler_Name;
                lJobNew.Scheduler_Tel = lJobOld.Scheduler_Tel;
                lJobNew.SiteEngr_HP = lJobOld.SiteEngr_HP;
                lJobNew.SiteEngr_Name = lJobOld.SiteEngr_Name;
                lJobNew.SiteEngr_Tel = lJobOld.SiteEngr_Tel;
                lJobNew.TotalPcs = lJobOld.TotalPcs;
                lJobNew.TotalWeight = lJobOld.TotalWeight;
                lJobNew.Transport = lJobOld.Transport;
                lJobNew.UpdateBy = UserName;//User.Identity.GetUserName();
                lJobNew.UpdateDate = DateTime.Now;
                db.Entry(lJobNewOr).CurrentValues.SetValues(lJobNew);
            }

            var lOld = (from p in db.StdSheetDetails
                        where p.CustomerCode == pNewCustomerCode &&
                        p.ProjectCode == pNewProjectCode &&
                        p.JobID == pNewJobID
                        select p).ToList();
            if (lOld != null && lOld.Count > 0)
            {
                db.StdSheetDetails.RemoveRange(lOld);
            }

            var lOldDet = (from p in db.StdSheetDetails
                           where p.CustomerCode == pOldCustomerCode &&
                           p.ProjectCode == pOldProjectCode &&
                           p.JobID == pOldJobID
                           select p).ToList();
            if (lOldDet != null && lOldDet.Count > 0)
            {
                var lNewDet = new List<StdSheetDetailsModels>();
                for (int j = 0; j < lOldDet.Count; j++)
                {

                    lNewDet.Add(new StdSheetDetailsModels
                    {
                        CustomerCode = pNewCustomerCode,
                        ProjectCode = pNewProjectCode,
                        JobID = (int)pNewJobID,
                        SheetID = lOldDet[j].SheetID,
                        SheetSort = lOldDet[j].SheetSort,
                        std_type = lOldDet[j].std_type,
                        mesh_series = lOldDet[j].mesh_series,
                        sheet_name = lOldDet[j].sheet_name,
                        mw_length = lOldDet[j].mw_length,
                        mw_size = lOldDet[j].mw_size,
                        mw_spacing = lOldDet[j].mw_spacing,
                        mo1 = lOldDet[j].mo1,
                        mo2 = lOldDet[j].mo2,
                        cw_length = lOldDet[j].cw_length,
                        cw_size = lOldDet[j].cw_size,
                        cw_spacing = lOldDet[j].cw_spacing,
                        co1 = lOldDet[j].co1,
                        co2 = lOldDet[j].co2,
                        unit_weight = lOldDet[j].unit_weight,
                        order_pcs = lOldDet[j].order_pcs,
                        order_wt = lOldDet[j].order_wt,
                        sap_mcode = lOldDet[j].sap_mcode,
                        UpdateBy = UserName,//User.Identity.GetUserName(),
                        UpdateDate = DateTime.Now
                    });
                }
                db.StdSheetDetails.AddRange(lNewDet);
            }

            //Clone for other standard products
            var lOldOther = (from p in db.StdProdDetails
                             where p.CustomerCode == pNewCustomerCode &&
                             p.ProjectCode == pNewProjectCode &&
                             p.JobID == pNewJobID
                             select p).ToList();
            if (lOldOther != null && lOldOther.Count > 0)
            {
                db.StdProdDetails.RemoveRange(lOldOther);
            }

            var lOldDetOther = (from p in db.StdProdDetails
                                where p.CustomerCode == pOldCustomerCode &&
                           p.ProjectCode == pOldProjectCode &&
                           p.JobID == pOldJobID
                                select p).ToList();
            if (lOldDetOther != null && lOldDetOther.Count > 0)
            {
                var lNewDetOther = new List<StdProdDetailsModels>();
                for (int j = 0; j < lOldDetOther.Count; j++)
                {

                    lNewDetOther.Add(new StdProdDetailsModels
                    {
                        CustomerCode = pNewCustomerCode,
                        ProjectCode = pNewProjectCode,
                        JobID = (int)pNewJobID,
                        ProdCode = lOldDetOther[j].ProdCode,
                        SSID = lOldDetOther[j].SSID,
                        ProdType = lOldDetOther[j].ProdType,
                        ProdDesc = lOldDetOther[j].ProdDesc,
                        Diameter = lOldDetOther[j].Diameter,
                        Grade = lOldDetOther[j].Grade,
                        UnitWT = lOldDetOther[j].UnitWT,
                        order_pcs = lOldDetOther[j].order_pcs,
                        order_wt = lOldDetOther[j].order_wt,
                        UpdateBy = UserName,//User.Identity.GetUserName(),
                        UpdateDate = DateTime.Now
                    });
                }
                db.StdProdDetails.AddRange(lNewDetOther);
            }
            db.SaveChanges();
            return lReturn;
        }


        [HttpGet]
        [Route("/cloneMeshProducts/{pOldCustomerCode}/{pOldProjectCode}/{pOldJobID}/{pNewCustomerCode}/{pNewProjectCode}/{pNewJobID}/{UserName}")]
        public int cloneMeshProducts(string pOldCustomerCode, string pOldProjectCode, int pOldJobID, string pNewCustomerCode, string pNewProjectCode, int pNewJobID, string UserName)
        {
            int lReturn = 0;

            decimal lWTJob = 0;
            int lPCsJob = 0;

            //Clear previous details
            var lDelBeam = (from p in db.CTSMESHBeamDetails
                            where p.CustomerCode == pNewCustomerCode &&
                            p.ProjectCode == pNewProjectCode &&
                            p.JobID == pNewJobID
                            select p).ToList();
            if (lDelBeam != null && lDelBeam.Count > 0)
            {
                db.CTSMESHBeamDetails.RemoveRange(lDelBeam);
            }

            var lDelColumn = (from p in db.CTSMESHColumnDetails
                              where p.CustomerCode == pNewCustomerCode &&
                              p.ProjectCode == pNewProjectCode &&
                              p.JobID == pNewJobID
                              select p).ToList();
            if (lDelColumn != null && lDelColumn.Count > 0)
            {
                db.CTSMESHColumnDetails.RemoveRange(lDelColumn);
            }

            var lDelSlab = (from p in db.CTSMESHOthersDetails
                            where p.CustomerCode == pNewCustomerCode &&
                            p.ProjectCode == pNewProjectCode &&
                            p.JobID == pNewJobID
                            select p).ToList();
            if (lDelSlab != null && lDelSlab.Count > 0)
            {
                db.CTSMESHOthersDetails.RemoveRange(lDelSlab);
            }

            //Clone details
            var lOldBeam = (from p in db.CTSMESHBeamDetails
                            where p.CustomerCode == pOldCustomerCode &&
                            p.ProjectCode == pOldProjectCode &&
                            p.JobID == pOldJobID
                            select p).ToList();
            if (lOldBeam != null && lOldBeam.Count > 0)
            {
                var lNewBeam = new List<CTSMESHBeamDetailsModels>();
                for (int j = 0; j < lOldBeam.Count; j++)
                {
                    lPCsJob = lPCsJob + (lOldBeam[j].MeshMemberQty == null ? 0 : (int)lOldBeam[j].MeshMemberQty);
                    lWTJob = lWTJob + (lOldBeam[j].MeshTotalWT == null ? 0 : (decimal)lOldBeam[j].MeshTotalWT);

                    lNewBeam.Add(new CTSMESHBeamDetailsModels
                    {
                        CustomerCode = pNewCustomerCode,
                        ProjectCode = pNewProjectCode,
                        JobID = (int)pNewJobID,

                        BBSID = lOldBeam[j].BBSID,
                        MeshID = lOldBeam[j].MeshID,
                        MeshSort = lOldBeam[j].MeshSort,
                        MeshMark = lOldBeam[j].MeshMark,
                        MeshWidth = lOldBeam[j].MeshWidth,
                        MeshDepth = lOldBeam[j].MeshDepth,
                        MeshSlope = lOldBeam[j].MeshSlope,
                        MeshProduct = lOldBeam[j].MeshProduct,
                        MeshShapeCode = lOldBeam[j].MeshShapeCode,
                        MeshTotalLinks = lOldBeam[j].MeshTotalLinks,
                        MeshSpan = lOldBeam[j].MeshSpan,
                        MeshMemberQty = lOldBeam[j].MeshMemberQty,
                        MeshCapping = lOldBeam[j].MeshCapping,
                        MeshCPProduct = lOldBeam[j].MeshCPProduct,
                        A = lOldBeam[j].A,
                        B = lOldBeam[j].B,
                        C = lOldBeam[j].C,
                        D = lOldBeam[j].D,
                        E = lOldBeam[j].E,
                        P = lOldBeam[j].P,
                        Q = lOldBeam[j].Q,
                        HOOK = lOldBeam[j].HOOK,
                        LEG = lOldBeam[j].LEG,
                        MeshTotalWT = lOldBeam[j].MeshTotalWT,
                        Remarks = lOldBeam[j].Remarks,
                        MWLength = lOldBeam[j].MWLength,
                        MWBOM = lOldBeam[j].MWBOM,
                        CWBOM = lOldBeam[j].CWBOM,

                        UpdateBy = UserName,//User.Identity.GetUserName(),
                        UpdateDate = DateTime.Now
                    });
                }
                db.CTSMESHBeamDetails.AddRange(lNewBeam);
            }

            var lOlColumn = (from p in db.CTSMESHColumnDetails
                             where p.CustomerCode == pOldCustomerCode &&
                             p.ProjectCode == pOldProjectCode &&
                             p.JobID == pOldJobID
                             select p).ToList();
            if (lOlColumn != null && lOlColumn.Count > 0)
            {
                var lNewColumn = new List<CTSMESHColumnDetailsModels>();
                for (int j = 0; j < lOlColumn.Count; j++)
                {
                    lPCsJob = lPCsJob + (lOlColumn[j].MeshMemberQty == null ? 0 : (int)lOlColumn[j].MeshMemberQty);
                    lWTJob = lWTJob + (lOlColumn[j].MeshTotalWT == null ? 0 : (decimal)lOlColumn[j].MeshTotalWT);

                    lNewColumn.Add(new CTSMESHColumnDetailsModels
                    {
                        CustomerCode = pNewCustomerCode,
                        ProjectCode = pNewProjectCode,
                        JobID = (int)pNewJobID,

                        BBSID = lOlColumn[j].BBSID,
                        MeshID = lOlColumn[j].MeshID,
                        MeshSort = lOlColumn[j].MeshSort,
                        MeshMark = lOlColumn[j].MeshMark,
                        MeshWidth = lOlColumn[j].MeshWidth,
                        MeshLength = lOlColumn[j].MeshLength,
                        MeshProduct = lOlColumn[j].MeshProduct,
                        MeshShapeCode = lOlColumn[j].MeshShapeCode,
                        MeshTotalLinks = lOlColumn[j].MeshTotalLinks,
                        MeshHeight = lOlColumn[j].MeshHeight,
                        MeshMemberQty = lOlColumn[j].MeshMemberQty,
                        MeshCLinkRowsAtLen = lOlColumn[j].MeshCLinkRowsAtLen,
                        MeshCLinkProductAtLen = lOlColumn[j].MeshCLinkProductAtLen,
                        MeshCLinkRowsAtWidth = lOlColumn[j].MeshCLinkRowsAtWidth,
                        MeshCLinkProductAtWidth = lOlColumn[j].MeshCLinkProductAtWidth,
                        A = lOlColumn[j].A,
                        B = lOlColumn[j].B,
                        C = lOlColumn[j].C,
                        D = lOlColumn[j].D,
                        E = lOlColumn[j].E,
                        F = lOlColumn[j].F,
                        P = lOlColumn[j].P,
                        Q = lOlColumn[j].Q,
                        LEG = lOlColumn[j].LEG,
                        MeshTotalWT = lOlColumn[j].MeshTotalWT,
                        Remarks = lOlColumn[j].Remarks,
                        MWLength = lOlColumn[j].MWLength,
                        MWBOM = lOlColumn[j].MWBOM,
                        CWBOM = lOlColumn[j].CWBOM,

                        UpdateBy = UserName,//User.Identity.GetUserName(),
                        UpdateDate = DateTime.Now
                    });
                }
                db.CTSMESHColumnDetails.AddRange(lNewColumn);
            }

            var lOldSlab = (from p in db.CTSMESHOthersDetails
                            where p.CustomerCode == pOldCustomerCode &&
                            p.ProjectCode == pOldProjectCode &&
                            p.JobID == pOldJobID
                            select p).ToList();
            if (lOldSlab != null && lOldSlab.Count > 0)
            {
                var lNewSlab = new List<CTSMESHOthersDetailsModels>();
                for (int j = 0; j < lOldSlab.Count; j++)
                {
                    lPCsJob = lPCsJob + (lOldSlab[j].MeshMemberQty == null ? 0 : (int)lOldSlab[j].MeshMemberQty);
                    lWTJob = lWTJob + (lOldSlab[j].MeshTotalWT == null ? 0 : (decimal)lOldSlab[j].MeshTotalWT);

                    lNewSlab.Add(new CTSMESHOthersDetailsModels
                    {
                        CustomerCode = pNewCustomerCode,
                        ProjectCode = pNewProjectCode,
                        JobID = (int)pNewJobID,

                        BBSID = lOldSlab[j].BBSID,
                        MeshID = lOldSlab[j].MeshID,
                        MeshSort = lOldSlab[j].MeshSort,
                        MeshMark = lOldSlab[j].MeshMark,
                        MeshProduct = lOldSlab[j].MeshProduct,
                        MeshMainLen = lOldSlab[j].MeshMainLen,
                        MeshCrossLen = lOldSlab[j].MeshCrossLen,
                        MeshMO1 = lOldSlab[j].MeshMO1,
                        MeshMO2 = lOldSlab[j].MeshMO2,
                        MeshCO1 = lOldSlab[j].MeshCO1,
                        MeshCO2 = lOldSlab[j].MeshCO2,
                        MeshMemberQty = lOldSlab[j].MeshMemberQty,
                        MeshShapeCode = lOldSlab[j].MeshShapeCode,

                        A = lOldSlab[j].A,
                        B = lOldSlab[j].B,
                        C = lOldSlab[j].C,
                        D = lOldSlab[j].D,
                        E = lOldSlab[j].E,
                        F = lOldSlab[j].F,
                        G = lOldSlab[j].G,
                        H = lOldSlab[j].H,
                        I = lOldSlab[j].I,
                        J = lOldSlab[j].J,
                        K = lOldSlab[j].K,
                        L = lOldSlab[j].L,
                        M = lOldSlab[j].M,
                        N = lOldSlab[j].N,
                        O = lOldSlab[j].O,
                        P = lOldSlab[j].P,
                        Q = lOldSlab[j].Q,
                        R = lOldSlab[j].R,
                        S = lOldSlab[j].S,
                        T = lOldSlab[j].T,
                        U = lOldSlab[j].U,
                        V = lOldSlab[j].V,
                        W = lOldSlab[j].W,
                        X = lOldSlab[j].X,
                        Y = lOldSlab[j].Y,
                        Z = lOldSlab[j].Z,

                        HOOK = lOldSlab[j].HOOK,
                        MeshTotalWT = lOldSlab[j].MeshTotalWT,
                        Remarks = lOldSlab[j].Remarks,
                        MWBOM = lOldSlab[j].MWBOM,
                        CWBOM = lOldSlab[j].CWBOM,

                        UpdateBy = UserName,//User.Identity.GetUserName(),
                        UpdateDate = DateTime.Now
                    });
                }
                db.CTSMESHOthersDetails.AddRange(lNewSlab);
            }

            var lJobNewOr = db.CTSMESHJobAdvice.Find(pNewCustomerCode, pNewProjectCode, pNewJobID);
            var lJobNew = lJobNewOr;
            var lJobOld = db.CTSMESHJobAdvice.Find(pOldCustomerCode, pOldProjectCode, pOldJobID);
            if (lJobOld != null && lJobNew != null)
            {
                lJobNew.OrderStatus = "New";
                //lJobNew.PODate = lJobOld.PODate;
                lJobNew.PODate = DateTime.Now;
                lJobNew.PONumber = lJobOld.PONumber;
                lJobNew.DeliveryAddress = lJobOld.DeliveryAddress;
                lJobNew.Remarks = lJobOld.Remarks;
                lJobNew.RequiredDate = lJobOld.RequiredDate;
                lJobNew.Scheduler_HP = lJobOld.Scheduler_HP;
                lJobNew.Scheduler_Name = lJobOld.Scheduler_Name;
                lJobNew.Scheduler_Tel = lJobOld.Scheduler_Tel;
                lJobNew.SiteEngr_HP = lJobOld.SiteEngr_HP;
                lJobNew.SiteEngr_Name = lJobOld.SiteEngr_Name;
                lJobNew.SiteEngr_Tel = lJobOld.SiteEngr_Tel;
                lJobNew.TotalPcs = lPCsJob;
                lJobNew.TotalWeight = lWTJob;
                lJobNew.Transport = lJobOld.Transport;
                lJobNew.AutoWBS = lJobOld.AutoWBS;
                lJobNew.Model = lJobOld.Model;
                lJobNew.OrderSource = lJobOld.OrderSource;

                lJobNew.WBS1 = lJobOld.WBS1;
                lJobNew.WBS2 = lJobOld.WBS2;
                lJobNew.WBS3 = lJobOld.WBS3;

                lJobNew.UpdateBy = UserName;//User.Identity.GetUserName();
                lJobNew.UpdateDate = DateTime.Now;
                db.Entry(lJobNewOr).CurrentValues.SetValues(lJobNew);
            }

            // clone BBS
            var lBBSDel = (from p in db.CTSMESHBBS
                           where p.CustomerCode == pNewCustomerCode &&
                           p.ProjectCode == pNewProjectCode &&
                           p.JobID == pNewJobID
                           select p).ToList();
            if (lBBSDel != null && lBBSDel.Count > 0)
            {
                db.CTSMESHBBS.RemoveRange(lBBSDel);
            }

            var lOldBBS = (from p in db.CTSMESHBBS
                           where p.CustomerCode == pOldCustomerCode &&
                           p.ProjectCode == pOldProjectCode &&
                           p.JobID == pOldJobID
                           select p).ToList();
            if (lOldBBS != null && lOldBBS.Count > 0)
            {
                var lNewBBS = new List<CTSMESHBBSModels>();
                for (int j = 0; j < lOldBBS.Count; j++)
                {

                    lNewBBS.Add(new CTSMESHBBSModels
                    {
                        CustomerCode = pNewCustomerCode,
                        ProjectCode = pNewProjectCode,
                        JobID = (int)pNewJobID,
                        BBSID = lOldBBS[j].BBSID,
                        BBSOrder = lOldBBS[j].BBSOrder,
                        BBSProdCategory = lOldBBS[j].BBSProdCategory,
                        BBSStrucElem = lOldBBS[j].BBSStrucElem,
                        BBSDesc = lOldBBS[j].BBSDesc,
                        BBSTotalPcs = lPCsJob,
                        BBSTotalWT = lWTJob,
                        BBSDrawing = lOldBBS[j].BBSDrawing,
                        BBSDrawingRev = lOldBBS[j].BBSDrawingRev,
                        BBSNDSPostID = lOldBBS[j].BBSNDSPostID,
                        BBSNDSGroupMark = lOldBBS[j].BBSNDSGroupMark,
                        BBSSOR = "",
                        UpdateBy = UserName,//User.Identity.GetUserName(),
                        UpdateDate = DateTime.Now
                    });
                }
                db.CTSMESHBBS.AddRange(lNewBBS);
            }

            db.SaveChanges();
            return lReturn;
        }


        [HttpGet]
        [Route("/cloneCAB/{pOldCustomerCode}/{pOldProjectCode}/{pOldJobID}/{pNewCustomerCode}/{pNewProjectCode}/{pNewJobID}")]
        public int cloneCAB(string pOldCustomerCode, string pOldProjectCode, int pOldJobID, string pNewCustomerCode, string pNewProjectCode, int pNewJobID, string UserName)
        {
            int lReturn = 0;

            //Clon JobAdvice
            var lJobNewOr = db.JobAdvice.Find(pNewCustomerCode, pNewProjectCode, pNewJobID);
            var lJobNew = lJobNewOr;
            var lJobOld = db.JobAdvice.Find(pOldCustomerCode, pOldProjectCode, pOldJobID);
            if (lJobOld != null && lJobNew != null)
            {
                lJobNew.OrderStatus = "New";
                //lJobNew.PODate = lJobOld.PODate;
                lJobNew.PODate = DateTime.Now;
                lJobNew.PONumber = lJobOld.PONumber;
                lJobNew.DeliveryAddress = lJobOld.DeliveryAddress;
                lJobNew.Remarks = lJobOld.Remarks;
                lJobNew.RequiredDate = lJobOld.RequiredDate;
                lJobNew.Scheduler_HP = lJobOld.Scheduler_HP;
                lJobNew.Scheduler_Name = lJobOld.Scheduler_Name;
                lJobNew.Scheduler_Tel = lJobOld.Scheduler_Tel;
                lJobNew.SiteEngr_HP = lJobOld.SiteEngr_HP;
                lJobNew.SiteEngr_Name = lJobOld.SiteEngr_Name;
                lJobNew.SiteEngr_Tel = lJobOld.SiteEngr_Tel;
                lJobNew.TotalWeight = lJobOld.TotalWeight;
                lJobNew.TransportLimit = lJobOld.TransportLimit;
                lJobNew.TransportMode = lJobOld.TransportMode;
                lJobNew.OrderSource = lJobOld.OrderSource;
                lJobNew.ProjectStage = lJobOld.ProjectStage;
                lJobNew.TotalCABWeight = lJobOld.TotalCABWeight;
                lJobNew.TotalSTDWeight = lJobOld.TotalSTDWeight;
                lJobNew.TotalWeight = lJobOld.TotalWeight;
                lJobNew.WBS1 = lJobOld.WBS1;
                lJobNew.WBS2 = lJobOld.WBS2;
                lJobNew.WBS3 = lJobOld.WBS3;
                lJobNew.UpdateBy = UserName;//User.Identity.GetUserName();
                lJobNew.UpdateDate = DateTime.Now;
                db.Entry(lJobNewOr).CurrentValues.SetValues(lJobNew);
            }

            //Clone BBS

            var lBBSDel = (from p in db.BBS
                           where p.CustomerCode == pNewCustomerCode &&
                           p.ProjectCode == pNewProjectCode &&
                           p.JobID == pNewJobID
                           select p).ToList();
            if (lBBSDel != null && lBBSDel.Count > 0)
            {
                db.BBS.RemoveRange(lBBSDel);
            }

            var lBBSOld = (from p in db.BBS
                           where p.CustomerCode == pOldCustomerCode &&
                           p.ProjectCode == pOldProjectCode &&
                           p.JobID == pOldJobID
                           select p).ToList();
            if (lBBSOld != null && lBBSOld.Count > 0)
            {
                var lBBSNew = new List<BBSModels>();
                for (int j = 0; j < lBBSOld.Count; j++)
                {
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace(";", "");
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace("~", "");
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace("!", "");
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace("@", "");
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace("?", "");
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace("'", "");
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace("^", "");
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace("\\", "");
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace("\"", "");
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace("|", "");
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace(":", "");
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace(">", "");
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace("<", "");
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace("=", "");
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace("#", "");
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace("+", "");
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace("$", "");
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace("`", "");
                    lBBSOld[j].BBSNo = lBBSOld[j].BBSNo.Replace("%", "");

                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace(";", "");
                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace("~", "");
                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace("!", "");
                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace("@", "");
                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace("?", "");
                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace("'", "");
                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace("^", "");
                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace("\\", "");
                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace("\"", "");
                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace("|", "");
                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace(":", "");
                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace(">", "");
                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace("<", "");
                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace("=", "");
                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace("#", "");
                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace("+", "");
                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace("$", "");
                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace("`", "");
                    lBBSOld[j].BBSDesc = lBBSOld[j].BBSDesc.Replace("%", "");

                    lBBSNew.Add(new BBSModels
                    {
                        CustomerCode = pNewCustomerCode,
                        ProjectCode = pNewProjectCode,
                        JobID = (int)pNewJobID,
                        BBSID = lBBSOld[j].BBSID,
                        BBSNo = lBBSOld[j].BBSNo,
                        BBSDesc = lBBSOld[j].BBSDesc,
                        BBSCopiedFrom = lBBSOld[j].BBSNo,
                        BBSStrucElem = lBBSOld[j].BBSStrucElem,
                        BBSOrderCABWT = lBBSOld[j].BBSOrderCABWT,
                        BBSOrderSTDWT = lBBSOld[j].BBSOrderSTDWT,
                        BBSCancelledWT = lBBSOld[j].BBSCancelledWT,
                        BBSTotalWT = lBBSOld[j].BBSTotalWT,
                        //BBSSOR = lBBSOld[j].BBSSOR,
                        //BBSNoNDS = lBBSOld[j].BBSNoNDS,
                        //BBSSAPSO = lBBSOld[j].BBSSAPSO,
                        //BBSSORCoupler = lBBSOld[j].BBSSORCoupler,
                        //BBSNoNDSCoupler = lBBSOld[j].BBSNoNDSCoupler,
                        BBSSOR = "",
                        BBSNoNDS = "",
                        BBSSAPSO = "",
                        BBSSORCoupler = "",
                        BBSNoNDSCoupler = "",
                        UpdateDate = DateTime.Now,
                        UpdateBy = UserName,//User.Identity.GetUserName()
                    });
                }
                db.BBS.AddRange(lBBSNew);
            }

            //Create Bar Details
            var lDetDel = (from p in db.OrderDetails
                           where p.CustomerCode == pNewCustomerCode &&
                           p.ProjectCode == pNewProjectCode &&
                           p.JobID == pNewJobID
                           select p).ToList();
            if (lDetDel != null && lDetDel.Count > 0)
            {
                db.OrderDetails.RemoveRange(lDetDel);
            }

            var lOldDet = (from p in db.OrderDetails
                           where p.CustomerCode == pOldCustomerCode &&
                           p.ProjectCode == pOldProjectCode &&
                           p.JobID == pOldJobID
                           select p).ToList();
            if (lOldDet != null && lOldDet.Count > 0)
            {
                var lNewDet = new List<OrderDetailsModels>();
                for (int j = 0; j < lOldDet.Count; j++)
                {
                    lNewDet.Add(new OrderDetailsModels
                    {
                        CustomerCode = pNewCustomerCode,
                        ProjectCode = pNewProjectCode,
                        JobID = (int)pNewJobID,
                        BBSID = lOldDet[j].BBSID,
                        BarID = lOldDet[j].BarID,
                        BarSort = lOldDet[j].BarSort,
                        Cancelled = lOldDet[j].Cancelled,
                        BarCAB = lOldDet[j].BarCAB,
                        BarSTD = lOldDet[j].BarSTD,
                        ElementMark = lOldDet[j].ElementMark,
                        BarMark = lOldDet[j].BarMark,
                        BarType = lOldDet[j].BarType,
                        BarSize = lOldDet[j].BarSize,
                        BarMemberQty = lOldDet[j].BarMemberQty,
                        BarEachQty = lOldDet[j].BarEachQty,
                        BarTotalQty = lOldDet[j].BarTotalQty,
                        BarShapeCode = lOldDet[j].BarShapeCode,
                        A = lOldDet[j].A,
                        B = lOldDet[j].B,
                        C = lOldDet[j].C,
                        D = lOldDet[j].D,
                        E = lOldDet[j].E,
                        F = lOldDet[j].F,
                        G = lOldDet[j].G,
                        H = lOldDet[j].H,
                        I = lOldDet[j].I,
                        J = lOldDet[j].J,
                        K = lOldDet[j].K,
                        L = lOldDet[j].L,
                        M = lOldDet[j].M,
                        N = lOldDet[j].N,
                        O = lOldDet[j].O,
                        P = lOldDet[j].P,
                        Q = lOldDet[j].Q,
                        R = lOldDet[j].R,
                        S = lOldDet[j].S,
                        T = lOldDet[j].T,
                        U = lOldDet[j].U,
                        V = lOldDet[j].V,
                        W = lOldDet[j].W,
                        X = lOldDet[j].X,
                        Y = lOldDet[j].Y,
                        Z = lOldDet[j].Z,
                        A2 = lOldDet[j].A2,
                        B2 = lOldDet[j].B2,
                        C2 = lOldDet[j].C2,
                        D2 = lOldDet[j].D2,
                        E2 = lOldDet[j].E2,
                        F2 = lOldDet[j].F2,
                        G2 = lOldDet[j].G2,
                        H2 = lOldDet[j].H2,
                        I2 = lOldDet[j].I2,
                        J2 = lOldDet[j].J2,
                        K2 = lOldDet[j].K2,
                        L2 = lOldDet[j].L2,
                        M2 = lOldDet[j].M2,
                        N2 = lOldDet[j].N2,
                        O2 = lOldDet[j].O2,
                        P2 = lOldDet[j].P2,
                        Q2 = lOldDet[j].Q2,
                        R2 = lOldDet[j].R2,
                        S2 = lOldDet[j].S2,
                        T2 = lOldDet[j].T2,
                        U2 = lOldDet[j].U2,
                        V2 = lOldDet[j].Y2,
                        W2 = lOldDet[j].W2,
                        X2 = lOldDet[j].X2,
                        Y2 = lOldDet[j].Y2,
                        Z2 = lOldDet[j].Z2,
                        BarLength = lOldDet[j].BarLength,
                        BarLength2 = lOldDet[j].BarLength2,
                        BarWeight = lOldDet[j].BarWeight,
                        Remarks = lOldDet[j].Remarks,
                        shapeTransport = lOldDet[j].shapeTransport,
                        PinSize = lOldDet[j].PinSize,
                        UpdateDate = DateTime.Now,
                        UpdateBy = UserName,//User.Identity.GetUserName(),
                        CreateDate = DateTime.Now,
                        CreateBy = UserName//User.Identity.GetUserName()
                    });
                }
                db.OrderDetails.AddRange(lNewDet);
            }

            //Create Bar Details double capture
            var lDetDelDB = (from p in db.OrderDetailsDouble
                             where p.CustomerCode == pNewCustomerCode &&
                             p.ProjectCode == pNewProjectCode &&
                             p.JobID == pNewJobID
                             select p).ToList();
            if (lDetDelDB != null && lDetDelDB.Count > 0)
            {
                db.OrderDetailsDouble.RemoveRange(lDetDelDB);
            }

            var lOldDetDB = (from p in db.OrderDetailsDouble
                             where p.CustomerCode == pOldCustomerCode &&
                             p.ProjectCode == pOldProjectCode &&
                             p.JobID == pOldJobID
                             select p).ToList();
            if (lOldDetDB != null && lOldDetDB.Count > 0)
            {
                var lNewDet = new List<OrderDetailsDoubleModels>();
                for (int j = 0; j < lOldDetDB.Count; j++)
                {
                    lNewDet.Add(new OrderDetailsDoubleModels
                    {
                        CustomerCode = pNewCustomerCode,
                        ProjectCode = pNewProjectCode,
                        JobID = (int)pNewJobID,
                        BBSID = lOldDetDB[j].BBSID,
                        BarID = lOldDetDB[j].BarID,
                        BarSort = lOldDetDB[j].BarSort,
                        Cancelled = lOldDetDB[j].Cancelled,
                        BarCAB = lOldDetDB[j].BarCAB,
                        BarSTD = lOldDetDB[j].BarSTD,
                        ElementMark = lOldDetDB[j].ElementMark,
                        BarMark = lOldDetDB[j].BarMark,
                        BarType = lOldDetDB[j].BarType,
                        BarSize = lOldDetDB[j].BarSize,
                        BarMemberQty = lOldDetDB[j].BarMemberQty,
                        BarEachQty = lOldDetDB[j].BarEachQty,
                        BarTotalQty = lOldDetDB[j].BarTotalQty,
                        BarShapeCode = lOldDetDB[j].BarShapeCode,
                        A = lOldDetDB[j].A,
                        B = lOldDetDB[j].B,
                        C = lOldDetDB[j].C,
                        D = lOldDetDB[j].D,
                        E = lOldDetDB[j].E,
                        F = lOldDetDB[j].F,
                        G = lOldDetDB[j].G,
                        H = lOldDetDB[j].H,
                        I = lOldDetDB[j].I,
                        J = lOldDetDB[j].J,
                        K = lOldDetDB[j].K,
                        L = lOldDetDB[j].L,
                        M = lOldDetDB[j].M,
                        N = lOldDetDB[j].N,
                        O = lOldDetDB[j].O,
                        P = lOldDetDB[j].P,
                        Q = lOldDetDB[j].Q,
                        R = lOldDetDB[j].R,
                        S = lOldDetDB[j].S,
                        T = lOldDetDB[j].T,
                        U = lOldDetDB[j].U,
                        V = lOldDetDB[j].V,
                        W = lOldDetDB[j].W,
                        X = lOldDetDB[j].X,
                        Y = lOldDetDB[j].Y,
                        Z = lOldDetDB[j].Z,
                        A2 = lOldDetDB[j].A2,
                        B2 = lOldDetDB[j].B2,
                        C2 = lOldDetDB[j].C2,
                        D2 = lOldDetDB[j].D2,
                        E2 = lOldDetDB[j].E2,
                        F2 = lOldDetDB[j].F2,
                        G2 = lOldDetDB[j].G2,
                        H2 = lOldDetDB[j].H2,
                        I2 = lOldDetDB[j].I2,
                        J2 = lOldDetDB[j].J2,
                        K2 = lOldDetDB[j].K2,
                        L2 = lOldDetDB[j].L2,
                        M2 = lOldDetDB[j].M2,
                        N2 = lOldDetDB[j].N2,
                        O2 = lOldDetDB[j].O2,
                        P2 = lOldDetDB[j].P2,
                        Q2 = lOldDetDB[j].Q2,
                        R2 = lOldDetDB[j].R2,
                        S2 = lOldDetDB[j].S2,
                        T2 = lOldDetDB[j].T2,
                        U2 = lOldDetDB[j].U2,
                        V2 = lOldDetDB[j].Y2,
                        W2 = lOldDetDB[j].W2,
                        X2 = lOldDetDB[j].X2,
                        Y2 = lOldDetDB[j].Y2,
                        Z2 = lOldDetDB[j].Z2,
                        BarLength = lOldDetDB[j].BarLength,
                        BarLength2 = lOldDetDB[j].BarLength2,
                        BarWeight = lOldDetDB[j].BarWeight,
                        Remarks = lOldDetDB[j].Remarks,
                        shapeTransport = lOldDetDB[j].shapeTransport,
                        PinSize = lOldDetDB[j].PinSize,
                        UpdateDate = DateTime.Now,
                        UpdateBy = UserName,//User.Identity.GetUserName(),
                        CreateDate = DateTime.Now,
                        CreateBy = UserName//User.Identity.GetUserName()
                    });
                }
                db.OrderDetailsDouble.AddRange(lNewDet);
            }
            db.SaveChanges();

            return lReturn;
        }


        [HttpGet]
        [Route("/cloneBPC/{pOldCustomerCode}/{pOldProjectCode}/{pOldJobID}/{pNewCustomerCode}/{pNewProjectCode}/{pNewJobID}/{UserName}")]
        public int cloneBPC(string pOldCustomerCode, string pOldProjectCode, int pOldJobID, string pNewCustomerCode, string pNewProjectCode, int pNewJobID, string UserName)
        {
            int lReturn = 0;
            Boolean lTemplate = false;

            //create JobAdvice
            var lJobNewOr = db.BPCJobAdvice.Find(pNewCustomerCode, pNewProjectCode, lTemplate, pNewJobID);
            var lJobNew = lJobNewOr;
            var lJobOld = db.BPCJobAdvice.Find(pOldCustomerCode, pOldProjectCode, lTemplate, pOldJobID);
            if (lJobOld != null && lJobNew != null)
            {
                lJobNew.cover_to_link = lJobOld.cover_to_link;
                lJobNew.OrderSource = lJobOld.OrderSource;
                lJobNew.pile_category = lJobOld.pile_category;
                lJobNew.redesign_req = lJobOld.redesign_req;
                lJobNew.TotalPcs = lJobOld.TotalPcs;
                lJobNew.Transport = lJobOld.Transport;
                lJobNew.underload_ct = lJobOld.underload_ct;

                lJobNew.OrderStatus = "New";
                //lJobNew.PODate = lJobOld.PODate;
                lJobNew.PODate = DateTime.Now;
                lJobNew.PONumber = lJobOld.PONumber;
                lJobNew.DeliveryAddress = lJobOld.DeliveryAddress;
                lJobNew.Remarks = lJobOld.Remarks;
                lJobNew.RequiredDate = lJobOld.RequiredDate;
                lJobNew.Scheduler_HP = lJobOld.Scheduler_HP;
                lJobNew.Scheduler_Name = lJobOld.Scheduler_Name;
                lJobNew.Scheduler_Tel = lJobOld.Scheduler_Tel;
                lJobNew.SiteEngr_HP = lJobOld.SiteEngr_HP;
                lJobNew.SiteEngr_Name = lJobOld.SiteEngr_Name;
                lJobNew.SiteEngr_Tel = lJobOld.SiteEngr_Tel;
                lJobNew.OrderSource = lJobOld.OrderSource;
                lJobNew.TotalWeight = lJobOld.TotalWeight;
                lJobNew.UpdateBy = UserName;//User.Identity.GetUserName();
                lJobNew.UpdateDate = DateTime.Now;
                db.Entry(lJobNewOr).CurrentValues.SetValues(lJobNew);
            }

            //Clone BPC Details

            var lBBSDel = (from p in db.BPCDetails
                           where p.CustomerCode == pNewCustomerCode &&
                           p.ProjectCode == pNewProjectCode &&
                           p.Template == lTemplate &&
                           p.JobID == pNewJobID
                           select p).ToList();
            if (lBBSDel != null && lBBSDel.Count > 0)
            {
                db.BPCDetails.RemoveRange(lBBSDel);
            }

            var lBBSOld = (from p in db.BPCDetails
                           where p.CustomerCode == pOldCustomerCode &&
                           p.ProjectCode == pOldProjectCode &&
                           p.Template == lTemplate &&
                           p.JobID == pOldJobID
                           select p).ToList();
            if (lBBSOld != null && lBBSOld.Count > 0)
            {
                var lBBSNew = new List<BPCDetailsModels>();
                for (int j = 0; j < lBBSOld.Count; j++)
                {

                    lBBSNew.Add(new BPCDetailsModels
                    {
                        CustomerCode = pNewCustomerCode,
                        ProjectCode = pNewProjectCode,
                        Template = lBBSOld[j].Template,
                        JobID = (int)pNewJobID,
                        cage_id = lBBSOld[j].cage_id,
                        pile_type = lBBSOld[j].pile_type,
                        pile_dia = lBBSOld[j].pile_dia,
                        cage_dia = lBBSOld[j].cage_dia,
                        main_bar_arrange = lBBSOld[j].main_bar_arrange,
                        main_bar_type = lBBSOld[j].main_bar_type,
                        main_bar_ct = lBBSOld[j].main_bar_ct,
                        main_bar_shape = lBBSOld[j].main_bar_shape,
                        main_bar_grade = lBBSOld[j].main_bar_grade,
                        main_bar_dia = lBBSOld[j].main_bar_dia,
                        main_bar_topjoin = lBBSOld[j].main_bar_topjoin,
                        main_bar_endjoin = lBBSOld[j].main_bar_endjoin,
                        cage_length = lBBSOld[j].cage_length,
                        spiral_link_type = lBBSOld[j].spiral_link_type,
                        spiral_link_grade = lBBSOld[j].spiral_link_grade,
                        spiral_link_dia = lBBSOld[j].spiral_link_dia,
                        spiral_link_spacing = lBBSOld[j].spiral_link_spacing,
                        lap_length = lBBSOld[j].lap_length,
                        end_length = lBBSOld[j].end_length,
                        cage_location = lBBSOld[j].cage_location,
                        rings_start = lBBSOld[j].rings_start,
                        rings_end = lBBSOld[j].rings_end,
                        rings_addn_no = lBBSOld[j].rings_addn_no,
                        rings_addn_member = lBBSOld[j].rings_addn_member,
                        coupler_top = lBBSOld[j].coupler_top,
                        coupler_end = lBBSOld[j].coupler_end,
                        no_of_sr = lBBSOld[j].no_of_sr,
                        sr_grade = lBBSOld[j].sr_grade,
                        sr_dia = lBBSOld[j].sr_dia,
                        sr1_location = lBBSOld[j].sr1_location,
                        sr2_location = lBBSOld[j].sr2_location,
                        sr3_location = lBBSOld[j].sr3_location,
                        sr4_location = lBBSOld[j].sr4_location,
                        crank_height_top = lBBSOld[j].crank_height_top,
                        crank_height_end = lBBSOld[j].crank_height_end,
                        sl1_length = lBBSOld[j].sl1_length,
                        sl2_length = lBBSOld[j].sl2_length,
                        sl3_length = lBBSOld[j].sl3_length,
                        sl1_dia = lBBSOld[j].sl1_dia,
                        sl2_dia = lBBSOld[j].sl2_dia,
                        sl3_dia = lBBSOld[j].sl3_dia,
                        total_sl_length = lBBSOld[j].total_sl_length,
                        no_of_cr_top = lBBSOld[j].no_of_cr_top,
                        cr_spacing_top = lBBSOld[j].cr_spacing_top,
                        no_of_cr_end = lBBSOld[j].no_of_cr_end,
                        cr_spacing_end = lBBSOld[j].cr_spacing_end,
                        cr_end_remarks = lBBSOld[j].cr_end_remarks,
                        extra_support_bar_ind = lBBSOld[j].extra_support_bar_ind,
                        extra_support_bar_dia = lBBSOld[j].extra_support_bar_dia,
                        extra_cr_no = lBBSOld[j].extra_cr_no,
                        extra_cr_dia = lBBSOld[j].extra_cr_dia,

                        mainbar_length_2layer = lBBSOld[j].mainbar_length_2layer,
                        mainbar_location_2layer = lBBSOld[j].mainbar_location_2layer,

                        per_set = lBBSOld[j].per_set,
                        bbs_no = lBBSOld[j].bbs_no,
                        cage_qty = lBBSOld[j].cage_qty,
                        cage_weight = lBBSOld[j].cage_weight,
                        cage_remarks = lBBSOld[j].cage_remarks,
                        set_code = lBBSOld[j].set_code,
                        nds_groupmarking = lBBSOld[j].nds_groupmarking,
                        nds_wbs1 = lBBSOld[j].nds_wbs1,
                        nds_wbs2 = lBBSOld[j].nds_wbs2,
                        nds_wbs3 = lBBSOld[j].nds_wbs3,
                        sor_no = lBBSOld[j].sor_no,
                        sap_mcode = lBBSOld[j].sap_mcode,
                        copyfrom_project = lBBSOld[j].copyfrom_project,
                        copyfrom_template = lBBSOld[j].copyfrom_template,
                        copyfrom_jobid = lBBSOld[j].copyfrom_jobid,
                        copyfrom_ponumber = lBBSOld[j].copyfrom_ponumber,

                        UpdateDate = DateTime.Now,
                        UpdateBy = UserName//User.Identity.GetUserName()
                    });
                }
                db.BPCDetails.AddRange(lBBSNew);
            }

            //Clone BPC Template

            var lTempDel = (from p in db.BPCTemplate
                            where p.CustomerCode == pNewCustomerCode &&
                            p.ProjectCode == pNewProjectCode &&
                            p.Template == lTemplate &&
                            p.JobID == pNewJobID
                            select p).ToList();
            if (lTempDel != null && lTempDel.Count > 0)
            {
                db.BPCTemplate.RemoveRange(lTempDel);
            }

            var lTempOld = (from p in db.BPCTemplate
                            where p.CustomerCode == pOldCustomerCode &&
                           p.ProjectCode == pOldProjectCode &&
                           p.Template == lTemplate &&
                           p.JobID == pOldJobID
                            select p).ToList();
            if (lTempOld != null && lTempOld.Count > 0)
            {
                var lTempNew = new List<BPCTemplateModels>();
                for (int j = 0; j < lTempOld.Count; j++)
                {

                    lTempNew.Add(new BPCTemplateModels
                    {
                        CustomerCode = pNewCustomerCode,
                        ProjectCode = pNewProjectCode,
                        Template = lTempOld[j].Template,
                        JobID = (int)pNewJobID,
                        CageID = lTempOld[j].CageID,
                        TemplateID = lTempOld[j].TemplateID,
                        template_code = lTempOld[j].template_code,
                        pile_dia = lTempOld[j].pile_dia,
                        cage_dia = lTempOld[j].cage_dia,
                        no_of_holes = lTempOld[j].no_of_holes,
                        cover = lTempOld[j].cover,
                        bundled = lTempOld[j].bundled,
                        UpdateDate = DateTime.Now,
                        UpdatedBy = UserName//User.Identity.GetUserName()
                    });
                }
                db.BPCTemplate.AddRange(lTempNew);
            }


            //Clone BPC Bars

            var lBarsDel = (from p in db.BPCCageBars
                            where p.CustomerCode == pNewCustomerCode &&
                            p.ProjectCode == pNewProjectCode &&
                            p.Template == lTemplate &&
                            p.JobID == pNewJobID
                            select p).ToList();
            if (lBarsDel != null && lBarsDel.Count > 0)
            {
                db.BPCCageBars.RemoveRange(lBarsDel);
            }

            var lBarsOld = (from p in db.BPCCageBars
                            where p.CustomerCode == pOldCustomerCode &&
                            p.ProjectCode == pOldProjectCode &&
                            p.Template == lTemplate &&
                            p.JobID == pOldJobID
                            select p).ToList();
            if (lBarsOld != null && lBarsOld.Count > 0)
            {
                var lBarsNew = new List<BPCCageBarsModels>();
                for (int j = 0; j < lBarsOld.Count; j++)
                {
                    lBarsNew.Add(new BPCCageBarsModels
                    {
                        CustomerCode = pNewCustomerCode,
                        ProjectCode = pNewProjectCode,
                        Template = lBarsOld[j].Template,
                        JobID = (int)pNewJobID,
                        CageID = lBarsOld[j].CageID,
                        BarID = lBarsOld[j].BarID,
                        BarSort = lBarsOld[j].BarSort,
                        ElementMark = lBarsOld[j].ElementMark,
                        BarMark = lBarsOld[j].BarMark,
                        BarType = lBarsOld[j].BarType,
                        BarSize = lBarsOld[j].BarSize,
                        BarMemberQty = lBarsOld[j].BarMemberQty,
                        BarEachQty = lBarsOld[j].BarEachQty,
                        BarTotalQty = lBarsOld[j].BarTotalQty,
                        BarShapeCode = lBarsOld[j].BarShapeCode,
                        A = lBarsOld[j].A,
                        B = lBarsOld[j].B,
                        C = lBarsOld[j].C,
                        D = lBarsOld[j].D,
                        E = lBarsOld[j].E,
                        F = lBarsOld[j].F,
                        G = lBarsOld[j].G,
                        H = lBarsOld[j].H,
                        I = lBarsOld[j].I,
                        J = lBarsOld[j].J,
                        K = lBarsOld[j].K,
                        L = lBarsOld[j].L,
                        M = lBarsOld[j].M,
                        N = lBarsOld[j].N,
                        O = lBarsOld[j].O,
                        P = lBarsOld[j].P,
                        Q = lBarsOld[j].Q,
                        R = lBarsOld[j].R,
                        S = lBarsOld[j].S,
                        T = lBarsOld[j].T,
                        U = lBarsOld[j].U,
                        V = lBarsOld[j].V,
                        W = lBarsOld[j].W,
                        X = lBarsOld[j].X,
                        Y = lBarsOld[j].Y,
                        Z = lBarsOld[j].Z,
                        BarLength = lBarsOld[j].BarLength,
                        BarWeight = lBarsOld[j].BarWeight,
                        Remarks = lBarsOld[j].Remarks,
                        PinSize = lBarsOld[j].PinSize,
                        UpdateDate = DateTime.Now,
                        isCABEdit = lBarsOld[j].isCABEdit,//Added by chetan for bpc enhamcment point
                    });
                }
                db.BPCCageBars.AddRange(lBarsNew);
            }
            db.SaveChanges();

            return lReturn;
        }


        [HttpGet]
        [Route("/ChangeStatus/{pCustomerCode}/{pProjectCode}/{pOrderNo}/{pOrderStatus}/{userName}")]
        public ActionResult ChangeStatus(string pCustomerCode, string pProjectCode, int pOrderNo, string pOrderStatus, string userName)
        {
            //OrderStatus 1. Submitted, 2. Sent 3. Withdraw; 4. Delete 
            try
            {
                //if (User == null || User.Identity == null || User.Identity.GetUserName() == null)
                //{
                //    return Ok("System session expired. Please re-login. \n\n登陆已过期. 请重新登陆.");
                //}

                if (pOrderStatus == "Submitted" || pOrderStatus == "Sent")
                {
                    int lCAB = 0;
                    var lSE = (from p in db.OrderProjectSE
                               where p.OrderNumber == pOrderNo
                               select p).ToList();
                    if (lSE != null && lSE.Count > 0)
                    {
                        for (int i = 0; i < lSE.Count; i++)
                        {
                            if ((lSE[i].ProductType != "BPC" && lSE[i].ProductType != "COUPLER" && lSE[i].ScheduledProd == "N" && lSE[i].TotalWeight == 0) ||
                            (lSE[i].ProductType == "COUPLER" && lSE[i].ScheduledProd == "N" && lSE[i].TotalPCs == 0))
                            {
                                return Ok("Incompleted order has been found (Ref No:" + pOrderNo + "). Please enter the order detail before submit it. \n\n发现未完成的订单. 请输入订单的数据再提交.");
                            }
                        }

                        for (int i = 0; i < lSE.Count; i++)
                        {
                            if (lSE[i].ProductType == "CAB")
                            {
                                lCAB = 1;
                                break;
                            }
                        }
                    }
                    if (lCAB == 1)
                    {
                        var lSharedAPI = new SharedAPIController();
                        string lCheckResult = lSharedAPI.checkCABDetails(pOrderNo, userName);
                        if (lCheckResult != "")
                        {
                            var lmessage = "Please settle Cut & Bend Order (Ref No:" + pOrderNo + ") issue before submit the order: \n\n" + lCheckResult;
                            return Json(new { success = false, message = lmessage });
                        }
                    }
                }

                OrderProcessAPIController lOrderProc = new OrderProcessAPIController();

                var lReturnMsg = lOrderProc.StatusProcess(pCustomerCode, pProjectCode, pOrderNo, pOrderStatus, userName);

                if (lReturnMsg != "")
                {
                    //return Ok();
                    return Json(new { success = false, message = lReturnMsg });

                }

            }
            catch (Exception ex)
            {
                SaveErrorMsg(ex.Message, ex.StackTrace);
                return Json(new { success = false, message = ("Error:" + ex.Message) });
                //return Ok("Error:");
            }

            //return Ok();
            return Json(new { success = true, message = "" });

        }

        [HttpGet]
        [Route("/printOrderDetail_neworder/{pOrderNumber}/{pUserName}")]
        public ActionResult printOrderDetail(int pOrderNumber, string pUserName)
        {
            Reports service = new Reports();
            int lPage = 0;
            byte[] lbPDF = new byte[] { };
            var lbPDF1 = service.rptOrderDetailsUX(pOrderNumber, "", "", "", "N", ref lPage, pUserName);
            lbPDF1 = service.rptOrderDetailsUX(pOrderNumber, "", "", "", "N", ref lPage, pUserName);

            string fileName = "abc.pdf";

            // Create a MemoryStream from the byte array
            using (MemoryStream memoryStream = new MemoryStream(lbPDF1))
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(memoryStream);
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                return File(response.Content.ReadAsByteArrayAsync().Result, "application/pdf");

            }
            // var lReturn = Ok(bPDF);
            //lReturn.MaxJsonLength = 50000000;
            //return Ok(lbPDF1);
        }

        [HttpGet]
        //[ValidateAntiForgeryHeader]
        [Route("/printOrderDetailSummary_neworder/{pOrderNumber}")]
        public ActionResult printOrderDetailSummary(int pOrderNumber)
        {
            Reports service = new Reports();
            int lPage = 1;
            byte[] lbPDF = new byte[] { };
            var bPDF = service.rptOrderDetailSummary(pOrderNumber, "N", ref lPage);
            //bPDF = service.rptOrderDetailSummary(pOrderNumber, "N", ref lPage);
            string fileName = "abc.pdf";
            using (MemoryStream memoryStream = new MemoryStream(bPDF))
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(memoryStream);
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                return File(response.Content.ReadAsByteArrayAsync().Result, "application/pdf");

            }
            //var lReturn = Ok(bPDF);
            //lReturn.MaxJsonLength = 50000000;
            //return lReturn;
        }

        [HttpGet]
        [Route("/getWBS1Clone/{ProjectCode}/{WBS1}/{WBS2}")]
        public ActionResult getWBS1(string ProjectCode, string WBS1, string WBS2)
        {
            //string lUserName = User.Identity.GetUserName();
            //var lUsers = lUserName.Split('@');
            //if (lUsers != null && lUsers.Length == 2 && (lUsers[1].ToLower() == "natsteel.com.sg" || lUsers[1].ToLower() == "gmaila.com"))
            //{
            var lWBS1 = new List<string>();
            var lWBS2 = new List<string>();
            var lWBS3 = new List<string>();

            var lDa = new SqlDataAdapter();
            var lCmd = new SqlCommand();
            var lDs = new DataSet();
            var cnNDS = new SqlConnection();
            SqlDataReader lRst;

            string lWBS1F = "";
            string lWBS2F = "";
            var lWBS2b = "";

            var lProcess = new ProcessController();
            lProcess.OpenNDSConnection(ref cnNDS);
            if (cnNDS.State == ConnectionState.Open)
            {
                var lSQL = "SELECT rtrim(ltrim(vchWBS1)) as WBS1, " +
                "rtrim(ltrim(vchWBS2)) as WBS2, " +
                "rtrim(ltrim(vchWBS3)) as WBS3  " +
                "FROM dbo.WBSElements E, dbo.WBS W, " +
                "dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                "dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                //"dbo.ContractMaster C " +
                "WHERE W.intWBSId = E.intWBSId " +
                "AND P.intProjectId = W.intProjectId " +
                //"AND P.intContractID = C.intContractID " +
                "AND E.intWBSElementId = D.intWBSElementId " +
                "AND D.sitProductTypeID = T.sitProductTypeID " +
                "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                "AND vchProjectCode = '" + ProjectCode + "' " +
                "AND (vchProductType = 'CAB' " +
                "OR vchProductType = 'MSH' " +
                "OR vchProductType = 'CAR' " +
                "OR vchProductType = 'BPC' " +
                "OR vchProductType = 'PRC' OR vchProductType = 'CORE') " +
                "AND E.tntStatusId = 1 " +
                "AND D.intConfirm = 1 " +
                "GROUP BY vchWBS1, " +
                "vchWBS2, " +
                "vchWBS3  " +
                "ORDER BY " +
                "vchWBS1, " +
                "(case when PATINDEX('B[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                "PATINDEX('%[^0-9]%', substring(vchWBS2 + 'z', (PATINDEX('%[0-9]%', vchWBS2)), " +
                "len(vchWBS2)) + 'z') ) as int)    " +
                "else (case when PATINDEX('FDN[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
                "len(vchWBS2)) + 'z') ) as int) else  " +
                "case when(PATINDEX('B%', vchWBS2) > 0 OR PATINDEX('FDN%', vchWBS2) > 0) then 98  else 99 end " +
                "end) end), " +
                " " +
                "(case when PATINDEX('[^0-9]%',vchWBS2) > 0 then vchWBS2 " +
                "else '' end), " +
                " " +
                "(CASE WHEN PATINDEX('%[0-9]%',vchWBS2) > 0 THEN cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2) + 1), " +
                "len(vchWBS2)) + 'z') ) as int) " +
                "ELSE " +
                "cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), len(vchWBS2)), " +
                "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
                "len(vchWBS2)) + 'z') - 1) as int) " +
                "END),  " +
                "vchWBS2, " +
                "vchWBS3 ";

                lCmd.CommandType = CommandType.Text;
                lCmd.CommandText = lSQL;
                lCmd.Connection = cnNDS;
                lCmd.CommandTimeout = 300;
                lDa.SelectCommand = lCmd;
                lDs = new DataSet();
                lDa.Fill(lDs);
                if (lDs.Tables[0].Rows.Count > 0)
                {
                    int lFound = 0;
                    for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                    {
                        lFound = 0;
                        var lWBS1t = lDs.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                        var lWBS2t = lDs.Tables[0].Rows[i].ItemArray[1].ToString().Trim();
                        var lWBS3t = lDs.Tables[0].Rows[i].ItemArray[2].ToString().Trim();

                        if (i == 0)
                        {
                            if (WBS1 != null && WBS1.Trim() != "")
                            {
                                lWBS1F = WBS1;
                            }
                            else
                            {
                                lWBS1F = lWBS1t;
                            }
                        }

                        if (lWBS1.Count > 0)
                        {
                            for (int j = 0; j < lWBS1.Count; j++)
                            {
                                if (lWBS1t == lWBS1[j])
                                {
                                    lFound = 1;
                                    break;
                                }
                            }
                        }
                        if (lFound == 0)
                        {
                            lWBS1.Add(lWBS1t);
                        }

                        if (lWBS1t == lWBS1F)
                        {
                            lFound = 0;
                            if (lWBS2.Count > 0)
                            {
                                for (int j = 0; j < lWBS2.Count; j++)
                                {
                                    if (lWBS2t == lWBS2[j])
                                    {
                                        lFound = 1;
                                        break;
                                    }
                                }
                            }
                            if (lFound == 0)
                            {
                                lWBS2.Add(lWBS2t);
                            }
                        }
                    }

                    //reverse the sorting

                    if (lWBS2.Count() > 1)
                    {
                        var lWBS2a = lWBS2;
                        lWBS2 = new List<string>();

                        for (int i = lWBS2a.Count() - 1; i >= 0; i--)
                        {
                            lWBS2.Add(lWBS2a[i]);
                        }
                    }

                    //lSQL = "SELECT Max(W.vchWBS2) " +
                    //"FROM dbo.BBSPostHeader H, dbo.WBSElements W, dbo.ProjectMaster P, dbo.ProductTypeMaster T " +
                    //"WHERE P.intProjectId = H.intProjectId " +
                    //"AND H.intWBSElementId = W.intWBSElementId " +
                    //"AND T.sitProductTypeID = H.sitProductTypeId " +
                    //"AND vchProjectCode = '" + ProjectCode + "' " +
                    //"AND H.tntStatusId = 3 " +
                    //"AND W.vchWBS1 = '" + lWBS1F + "' " +
                    //"AND W.vchWBS2 >= '0' " +
                    //"AND W.vchWBS2 <= '9' " +
                    //"AND H.numPostedWeight > 0 ";

                    //lCmd.CommandText = lSQL;
                    //lCmd.Connection = cnNDS;
                    //lCmd.CommandTimeout = 300;
                    //lRst = lCmd.ExecuteReader();
                    //if (lRst.HasRows)
                    //{
                    //    if (lRst.Read())
                    //    {
                    //        lWBS2b = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
                    //    }
                    //}

                    //lRst.Close();

                    lWBS2.Add(lWBS2b);

                    if (WBS2 != null && WBS2.Trim() != "")
                    {
                        lWBS2F = WBS2;
                    }
                    else
                    {
                        if (lWBS2b == "")
                        {
                            if (lWBS2.Count > 0)
                            {
                                lWBS2F = lWBS2[0];
                            }
                        }
                        else
                        {
                            lWBS2F = lWBS2b;
                        }
                    }

                    // get WBS3
                    for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                    {
                        var lWBS1t = lDs.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                        var lWBS2t = lDs.Tables[0].Rows[i].ItemArray[1].ToString().Trim();
                        var lWBS3t = lDs.Tables[0].Rows[i].ItemArray[2].ToString().Trim();

                        if (lWBS1t == lWBS1F && lWBS2t == lWBS2F)
                        {
                            lWBS3.Add(lWBS3t);
                        }
                    }

                }
                lProcess.CloseNDSConnection(ref cnNDS);
            }
            lDa = null;
            lCmd = null;
            lDs = null;
            cnNDS = null;

            //var lWBS1 = await (from p in db.WBSList
            //                   where p.ProjectCode == ProjectCode &&
            //                   (p.ProductType == "CAB" ||
            //                    p.ProductType == "MSH" ||
            //                    p.ProductType == "CAR" ||
            //                    p.ProductType == "PRC")
            //                   select p.WBS1)
            //               .Distinct()
            //               .OrderBy(x => x)
            //               .ToListAsync();

            //if (lWBS1 != null && lWBS1.Count > 0)
            //{
            //    int lFound = 0;
            //    if (WBS1 != "")
            //    {
            //        for (int i = 0; i < lWBS1.Count; i++)
            //        {
            //            if (lWBS1[i] == WBS1)
            //            {
            //                lFound = 1;
            //                break;
            //            }
            //        }
            //    }
            //    if (lFound == 0)
            //    {
            //        lSWBS1 = lWBS1[0];
            //    }
            //    else
            //    {
            //        lSWBS1 = WBS1;
            //    }
            //    lWBS2 = extractWBS2(ProjectCode, lSWBS1);

            //    if (lWBS2 != null && lWBS2.Count > 0)
            //    {
            //        lFound = 0;
            //        if (WBS2 != "")
            //        {
            //            for (int i = 0; i < lWBS2.Count; i++)
            //            {
            //                if (lWBS2[i] == WBS2)
            //                {
            //                    lFound = 1;
            //                    break;
            //                }
            //            }
            //        }
            //        if (lFound == 0)
            //        {
            //            lSWBS2 = lWBS2[lWBS2.Count - 1];
            //            if (lSWBS2 == "")
            //            {
            //                lSWBS2 = lWBS2[0];
            //            }
            //        }
            //        else
            //        {
            //            lSWBS2 = WBS2;
            //        }
            //        lWBS3 = extractWBS3(ProjectCode, lSWBS1, lSWBS2);
            //    }
            //}

            var lReturn = new
            {
                WBS1 = lWBS1,
                WBS2 = lWBS2,
                WBS3 = lWBS3
            };

            return Ok(lReturn);

            //}
            //else
            //{
            //    var content = await (from p in db.WBSList
            //                         where p.ProjectCode == ProjectCode && p.ProductType == "CAB"
            //                         select p.WBS1)
            //                   .Distinct()
            //                   .OrderBy(x => x)
            //                   .ToListAsync();
            //    content.Insert(0, " ");
            //    return Json(content, JsonRequestBehavior.AllowGet);
            //}
        }

        [HttpGet]
        [Route("/getWBS2Clone/{ProjectCode}/{WBS1}/{WBS2}")]
        public ActionResult getWBS2(string ProjectCode, string WBS1, string WBS2)
        {
            var lWBS2 = new List<string>();
            var lWBS3 = new List<string>();

            var lDa = new SqlDataAdapter();
            var lCmd = new SqlCommand();
            var lDs = new DataSet();
            var cnNDS = new SqlConnection();
            SqlDataReader lRst;
            string lWBS2F = "";
            var lWBS2b = "";

            var lProcess = new ProcessController();
            lProcess.OpenNDSConnection(ref cnNDS);
            if (cnNDS.State == ConnectionState.Open)
            {
                var lSQL = "SELECT " +
                "rtrim(ltrim(vchWBS2)) as WBS2, " +
                "rtrim(ltrim(vchWBS3)) as WBS3  " +
                "FROM dbo.WBSElements E, dbo.WBS W, " +
                "dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                "dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                //"dbo.ContractMaster C " +
                "WHERE W.intWBSId = E.intWBSId " +
                "AND P.intProjectId = W.intProjectId " +
                // "AND P.intContractID = C.intContractID " +
                "AND E.intWBSElementId = D.intWBSElementId " +
                "AND D.sitProductTypeID = T.sitProductTypeID " +
                "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                "AND vchProjectCode = '" + ProjectCode + "' " +
                "AND (vchProductType = 'CAB' " +
                "OR vchProductType = 'MSH' " +
                "OR vchProductType = 'CAR' " +
                "OR vchProductType = 'BPC' " +
                "OR vchProductType = 'PRC' OR vchProductType = 'CORE') " +
                "AND vchWBS1 = '" + WBS1 + " ' " +
                "AND E.tntStatusId = 1 " +
                "AND D.intConfirm = 1 " +
                "GROUP BY " +
                "vchWBS2, " +
                "vchWBS3  " +
                "ORDER BY " +
                "(case when PATINDEX('B[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                "PATINDEX('%[^0-9]%', substring(vchWBS2 + 'z', (PATINDEX('%[0-9]%', vchWBS2)), " +
                "len(vchWBS2)) + 'z') ) as int)    " +
                "else (case when PATINDEX('FDN[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
                "len(vchWBS2)) + 'z') ) as int) else  " +
                "case when(PATINDEX('B%', vchWBS2) > 0 OR PATINDEX('FDN%', vchWBS2) > 0) then 98  else 99 end " +
                "end) end), " +
                " " +
                "(case when PATINDEX('[^0-9]%',vchWBS2) > 0 then vchWBS2 " +
                "else '' end), " +
                " " +
                "(CASE WHEN PATINDEX('%[0-9]%',vchWBS2) > 0 THEN cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2) + 1), " +
                "len(vchWBS2)) + 'z') ) as int) " +
                "ELSE " +
                "cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), len(vchWBS2)), " +
                "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
                "len(vchWBS2)) + 'z') - 1) as int) " +
                "END),  " +
                "vchWBS2, " +
                "vchWBS3 ";

                lCmd.CommandType = CommandType.Text;
                lCmd.CommandText = lSQL;
                lCmd.Connection = cnNDS;
                lCmd.CommandTimeout = 300;
                lDa.SelectCommand = lCmd;
                lDs = new DataSet();
                lDa.Fill(lDs);
                if (lDs.Tables[0].Rows.Count > 0)
                {
                    int lFound = 0;
                    for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                    {
                        lFound = 0;
                        var lWBS2t = lDs.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                        var lWBS3t = lDs.Tables[0].Rows[i].ItemArray[1].ToString().Trim();

                        lFound = 0;
                        if (lWBS2.Count > 0)
                        {
                            for (int j = 0; j < lWBS2.Count; j++)
                            {
                                if (lWBS2t == lWBS2[j])
                                {
                                    lFound = 1;
                                    break;
                                }
                            }
                        }
                        if (lFound == 0)
                        {
                            lWBS2.Add(lWBS2t);
                        }

                    }

                    //reverse the sorting

                    if (lWBS2.Count() > 1)
                    {
                        var lWBS2a = lWBS2;
                        lWBS2 = new List<string>();

                        for (int i = lWBS2a.Count() - 1; i >= 0; i--)
                        {
                            lWBS2.Add(lWBS2a[i]);
                        }

                        //lSQL = "SELECT Max(W.vchWBS2) " +
                        //"FROM dbo.BBSPostHeader H, dbo.WBSElements W, dbo.ProjectMaster P, dbo.ProductTypeMaster T " +
                        //"WHERE P.intProjectId = H.intProjectId " +
                        //"AND H.intWBSElementId = W.intWBSElementId " +
                        //"AND T.sitProductTypeID = H.sitProductTypeId " +
                        //"AND vchProjectCode = '" + ProjectCode + "' " +
                        //"AND H.tntStatusId = 3 " +
                        //"AND W.vchWBS1 = '" + WBS1 + "' " +
                        //"AND W.vchWBS2 >= '0' " +
                        //"AND W.vchWBS2 <= '9' " +
                        //"AND H.numPostedWeight > 0 ";

                        //lCmd.CommandText = lSQL;
                        //lCmd.Connection = cnNDS;
                        //lCmd.CommandTimeout = 300;
                        //lRst = lCmd.ExecuteReader();
                        //if (lRst.HasRows)
                        //{
                        //    if (lRst.Read())
                        //    {
                        //        lWBS2b = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
                        //    }
                        //}
                        //lRst.Close();
                    }

                    if (lWBS2b == "")
                    {
                        if (lWBS2.Count > 0)
                        {
                            lWBS2F = lWBS2[0];
                        }
                    }
                    else
                    {
                        lWBS2F = lWBS2b;
                    }

                    lWBS2.Add(lWBS2b);

                    // get WBS3
                    for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                    {
                        lFound = 0;
                        var lWBS2t = lDs.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                        var lWBS3t = lDs.Tables[0].Rows[i].ItemArray[1].ToString().Trim();

                        if (lWBS2t == lWBS2F)
                        {
                            lWBS3.Add(lWBS3t);
                        }
                    }


                }
                lProcess.CloseNDSConnection(ref cnNDS);
            }
            lDa = null;
            lCmd = null;
            lDs = null;
            cnNDS = null;


            //var lWBS2 = extractWBS2(ProjectCode, WBS1);
            //var lWBS3 = new List<string>();
            //if (lWBS2 != null && lWBS2.Count > 0)
            //{
            //    string lSWBS2 = "";
            //    int lFound = 0;
            //    if (WBS2 != "")
            //    {
            //        for (int i = 0; i < lWBS2.Count; i++)
            //        {
            //            if (lWBS2[i] == WBS2)
            //            {
            //                lFound = 1;
            //                break;
            //            }
            //        }

            //    }
            //    if (lFound == 0 )
            //    {
            //        lSWBS2 = lWBS2[lWBS2.Count - 1];
            //        if (lSWBS2 == "")
            //        {
            //            lSWBS2 = lWBS2[0];
            //        }
            //    }
            //    else
            //    {
            //        lSWBS2 = WBS2;
            //    }
            //    lWBS3 = extractWBS3(ProjectCode, WBS1, lSWBS2);
            //}

            var lReturn = new
            {
                WBS2 = lWBS2,
                WBS3 = lWBS3
            };

            return Ok(lReturn);
        }

        [HttpPost]
        [Route("/getWBS3Clone")]
        public ActionResult getWBS3([FromBody] GetWBS3Dto getWBS3Dto)
        {
            string ProjectCode = getWBS3Dto.ProjectCode;
            string WBS1O = getWBS3Dto.WBS1O;
            string WBS2O = getWBS3Dto.WBS2O;
            string WBS3O = getWBS3Dto.WBS3O;
            string WBS1 = getWBS3Dto.WBS1;
            string WBS2FR = getWBS3Dto.WBS2FR;
            string WBS2TO = getWBS3Dto.WBS2TO;
            string ScheduledProd = getWBS3Dto.ScheduledProd;
            var lWBS3 = new List<string>();

            var lDa = new SqlDataAdapter();

            var lCmd = new SqlCommand();
            var lDs = new DataSet();
            var cnNDS = new SqlConnection();
            SqlDataReader lRst;

            var lSQL = "";

            int liWBS2FR = 0;
            int liWBS2TO = 0;
            int liWBS2O = 0;
            int.TryParse(WBS2FR, out liWBS2FR);
            int.TryParse(WBS2TO, out liWBS2TO);
            int.TryParse(WBS2O, out liWBS2O);


            var lProcess = new ProcessController();
            lProcess.OpenNDSConnection(ref cnNDS);
            if (cnNDS.State == ConnectionState.Open)
            {
                string lSE_ProdTye = "";
                if (getWBS3Dto.StructureEle.Count > 0)
                {
                    for (int i = 0; i < getWBS3Dto.StructureEle.Count; i++)
                    {
                        string lProdType = getWBS3Dto.ProductType[i];
                        if (getWBS3Dto.ProductType[i] == "CUT-TO-SIZE-MESH" || getWBS3Dto.ProductType[i] == "COLUMN-LINK-MESH" || getWBS3Dto.ProductType[i] == "STIRRUP-LINK-MESH")
                        {
                            lProdType = "MSH";
                        }
                        else if (getWBS3Dto.ProductType[i] == "PRE-CAGE")
                        {
                            lProdType = "PRC";
                        }

                        //else if (getWBS3Dto.ProductType[i] == "CARPET" || getWBS3Dto.ProductType[i] == "CORE-CAGE")
                        //{
                        //    lProdType = "CAR";
                        //}

                        else if (getWBS3Dto.ProductType[i] == "CARPET")
                        {
                            lProdType = "CAR";
                        }

                        else if (getWBS3Dto.ProductType[i] == "CORE-CAGE")
                        {
                            lProdType = "CORE";
                        }

                        if (ScheduledProd == "Y")
                        {
                            if (lSE_ProdTye == "")
                            {
                                lSE_ProdTye = lSE_ProdTye + " ( vchStructureElementType = '" + getWBS3Dto.StructureEle[i] + "' AND vchProductType = '" + lProdType + "') ";
                            }
                            else
                            {
                                lSE_ProdTye = lSE_ProdTye + " OR ( vchStructureElementType = '" + getWBS3Dto.StructureEle[i] + "' AND vchProductType = '" + lProdType + "') ";
                            }
                        }
                    }
                }

                if (lSE_ProdTye == "")
                {
                    lSE_ProdTye = "AND (1=1) ";
                }
                else
                {
                    lSE_ProdTye = "AND (" + lSE_ProdTye + ") ";
                }

                if (ScheduledProd == "Y")
                {
                    lSE_ProdTye = lSE_ProdTye +
                    "AND EXISTS(SELECT H.intPostHeaderid " +
                    "FROM dbo.BBSPostHeader H " +
                    "WHERE intWBSElementId = E.intWBSElementId AND " +
                    "intStructureElementTypeId = D.intStructureElementTypeId AND " +
                    "sitProductTypeId = D.sitProductTypeID " +
                    "AND NOT EXISTS(SELECT R.intPostHeaderid " +
                    "FROM dbo.BBSReleaseDetails R " +
                    "WHERE R.intPostHeaderid = H.intPostHeaderid " +
                    "AND R.tntStatusId = 12)" +
                    "And not exists (select sr.PostHeaderID from OESProjOrdersSE sr where sr.PostHeaderID = H.intPostHeaderid and sr.OrderStatus in('Submitted','Reviewed')))";
                }

                if (WBS1O == WBS1 && WBS2FR == WBS2O && WBS2TO == WBS2O)
                {
                    var lUprodType = getWBS3Dto.ProductType.Distinct().ToArray();
                    if (lUprodType.Length == 1 && lUprodType[0] == "CAB")
                    {
                        //lSQL = "SELECT " +
                        //"rtrim(ltrim(vchWBS3)) as WBS3  " +
                        //"FROM dbo.WBSElements E, dbo.WBS W, " +
                        //"dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                        //"dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                        ////"dbo.ContractMaster C " +
                        //"WHERE W.intWBSId = E.intWBSId " +
                        //"AND P.intProjectId = W.intProjectId " +
                        //// "AND P.intContractID = C.intContractID " +
                        //"AND E.intWBSElementId = D.intWBSElementId " +
                        //"AND D.sitProductTypeID = T.sitProductTypeID " +
                        //"AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                        //"AND vchProjectCode = '" + ProjectCode + "' " +
                        //lSE_ProdTye +
                        //"AND vchWBS1 = '" + WBS1 + "' " +
                        //"AND vchWBS2 = '" + WBS2FR + "' " +
                        //"AND E.tntStatusId = 1 " +
                        //"AND D.intConfirm = 1 " +
                        //"GROUP BY " +
                        //"vchWBS3  " +
                        //"ORDER BY " +
                        //"vchWBS3 ";

                        // ProjectMaster and ContractMaster changed to SAPProjectMaster and SAPContractMaster 

                        lSQL = "SELECT " +
                        "rtrim(ltrim(vchWBS3)) as WBS3  " +
                        "FROM dbo.WBSElements E, dbo.WBS W, " +
                        "dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                        "dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                        //    ", dbo.SAPContractMaster C, " +
                        //    "dbo.SAP_CONTRACT_PROJECT CP " +
                        "WHERE W.intWBSId = E.intWBSId " +
                        "AND P.intProjectId = W.intProjectId " +
                        //   "AND C.vchContractCode = CP.CONTR_NO  " +
                        //   "AND P.vchProjectCode= CP.PROJ_ID " +
                        "AND E.intWBSElementId = D.intWBSElementId " +
                        "AND D.sitProductTypeID = T.sitProductTypeID " +
                        "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                        "AND vchProjectCode = '" + ProjectCode + "' " +
                        lSE_ProdTye +
                        "AND vchWBS1 = '" + WBS1 + "' " +
                        "AND vchWBS2 = '" + WBS2FR + "' " +
                        "AND E.tntStatusId = 1 " +
                        "AND D.intConfirm = 1 " +
                        "GROUP BY " +
                        "vchWBS3  " +
                        "ORDER BY " +
                        "vchWBS3 ";
                    }
                    else
                    {
                        // lSQL = "SELECT " +
                        // "rtrim(ltrim(vchWBS3)) as WBS3  " +
                        // "FROM dbo.WBSElements E, dbo.WBS W, " +
                        // "dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                        // "dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                        // //"dbo.ContractMaster C " +
                        // "WHERE W.intWBSId = E.intWBSId " +
                        // "AND P.intProjectId = W.intProjectId " +
                        // // "AND P.intContractID = C.intContractID " +
                        // "AND E.intWBSElementId = D.intWBSElementId " +
                        // "AND D.sitProductTypeID = T.sitProductTypeID " +
                        // "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                        // "AND vchProjectCode = '" + ProjectCode + "' " +
                        // lSE_ProdTye +
                        // "AND vchWBS1 = '" + WBS1 + "' " +
                        // "AND vchWBS2 = '" + WBS2FR + "' " +
                        //// "AND vchWBS3 <> '" + WBS3O + "' " + //ajit
                        // "AND E.tntStatusId = 1 " +
                        // "AND D.intConfirm = 1 " +
                        // "GROUP BY " +
                        // "vchWBS3  " +
                        // "ORDER BY " +
                        // "vchWBS3 ";

                        // ProjectMaster and ContractMaster changed to SAPProjectMaster and SAPContractMaster 

                        lSQL = "SELECT " +
                       "rtrim(ltrim(vchWBS3)) as WBS3  " +
                       "FROM dbo.WBSElements E, dbo.WBS W, " +
                       "dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                       "dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                       //   ", dbo.SAPContractMaster C, " +
                       //   "dbo.SAP_CONTRACT_PROJECT CP " +
                       "WHERE W.intWBSId = E.intWBSId " +
                       "AND P.intProjectId = W.intProjectId " +
                       //   "AND C.vchContractCode = CP.CONTR_NO  " +
                       //   "AND P.vchProjectCode= CP.PROJ_ID " +
                       "AND E.intWBSElementId = D.intWBSElementId " +
                       "AND D.sitProductTypeID = T.sitProductTypeID " +
                       "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                       "AND vchProjectCode = '" + ProjectCode + "' " +
                       lSE_ProdTye +
                       "AND vchWBS1 = '" + WBS1 + "' " +
                       "AND vchWBS2 = '" + WBS2FR + "' " +
                       //"AND vchWBS3 <> '" + WBS3O + "' " +//ajit
                       "AND E.tntStatusId = 1 " +
                       "AND D.intConfirm = 1 " +
                       "GROUP BY " +
                       "vchWBS3  " +
                       "ORDER BY " +
                       "vchWBS3 ";
                    }
                }
                if (WBS1O == WBS1 && liWBS2FR > 0 && liWBS2TO > 0 && liWBS2O > 0)
                {
                    string lWBS2Con = "";
                    if (liWBS2FR < 10 && liWBS2TO < 10)
                    {
                        lWBS2Con = "vchWBS2 >= '" + WBS2FR + " ' " +
                        "AND vchWBS2 <= '" + WBS2TO + "' " +
                        "AND LEN(Ltrim(Rtrim(vchWBS2))) = 1 ";
                    }
                    else if (liWBS2FR >= 10 && liWBS2TO < 100)
                    {
                        lWBS2Con = "vchWBS2 >= '" + WBS2FR + " ' " +
                        "AND vchWBS2 <= '" + WBS2TO + "' " +
                        "AND LEN(Ltrim(Rtrim(vchWBS2))) = 2 ";
                    }
                    else if (liWBS2FR >= 100 && liWBS2TO < 1000)
                    {
                        lWBS2Con = "vchWBS2 >= '" + WBS2FR + "' " +
                        "AND vchWBS2 <= '" + WBS2TO + "' " +
                        "AND LEN(Ltrim(Rtrim(vchWBS2))) = 3 ";
                    }
                    else if (liWBS2FR < 10 && liWBS2TO >= 100)
                    {
                        lWBS2Con = "(vchWBS2 >= '" + WBS2FR + "' " +
                        "AND vchWBS2 <= '9' " +
                        "AND LEN(Ltrim(Rtrim(vchWBS2))) = 1) " +
                        "OR (vchWBS2 >= '10' " +
                        "AND vchWBS2 <= '99' " +
                        "AND LEN(Ltrim(Rtrim(vchWBS2))) = 2 ) " +
                        "OR (vchWBS2 >= '100' " +
                        "AND vchWBS2 <= '" + WBS2TO + "' " +
                        "AND LEN(Ltrim(Rtrim(vchWBS2))) = 3 ) ";
                    }
                    else if (liWBS2FR < 10 && liWBS2TO >= 10 && liWBS2TO < 100)
                    {
                        lWBS2Con = "(vchWBS2 >= '" + WBS2FR + "' " +
                        "AND vchWBS2 <= '9' " +
                        "AND LEN(Ltrim(Rtrim(vchWBS2))) = 1) " +
                        "OR (vchWBS2 >= '10' " +
                        "AND vchWBS2 <= '" + WBS2TO + "' " +
                        "AND LEN(Ltrim(Rtrim(vchWBS2))) = 2 ) ";
                    }
                    else if (liWBS2FR > 10 && liWBS2TO >= 100 && liWBS2TO < 1000)
                    {
                        lWBS2Con = "(vchWBS2 >= '" + WBS2FR + "' " +
                        "AND vchWBS2 <= '99' " +
                        "AND LEN(Ltrim(Rtrim(vchWBS2))) = 2 ) " +
                        "OR (vchWBS2 >= '100' " +
                        "AND vchWBS2 <= '" + WBS2TO + "' " +
                        "AND LEN(Ltrim(Rtrim(vchWBS2))) = 3 ) ";
                    }

                    if (lWBS2Con != "")
                    {
                        lWBS2Con = "AND (" + lWBS2Con + ") ";
                    }
                    else
                    {
                        lWBS2Con = "AND vchWBS2 >= '" + WBS2FR + "' " +
                        "AND vchWBS2 <= '" + WBS2TO + " ' ";
                    }
                    int lWBS3CT = 0;

                    if (liWBS2O >= liWBS2FR && liWBS2O <= liWBS2TO)
                    {
                        lSQL = "SELECT isNull(MAX(WBS3CT),0) FROM (SELECT " +
                        "COUNT(vchWBS3) as WBS3CT " +
                        "FROM ( " +
                        "SELECT vchWBS1, vchWBS2, vchWBS3 " +
                        "FROM dbo.WBSElements E, dbo.WBS W, " +
                        "dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                        "dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                        //"dbo.ContractMaster C " +
                        "WHERE W.intWBSId = E.intWBSId " +
                        "AND P.intProjectId = W.intProjectId " +
                        // "AND P.intContractID = C.intContractID " +
                        "AND E.intWBSElementId = D.intWBSElementId " +
                        "AND D.sitProductTypeID = T.sitProductTypeID " +
                        "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                        "AND vchProjectCode = '" + ProjectCode + "' " +
                        lSE_ProdTye +
                        "AND vchWBS1 = '" + WBS1 + " ' " +
                        lWBS2Con +
                        //"AND vchWBS3 <> '" + WBS3O + " ' " +
                        "AND E.tntStatusId = 1 " +
                        "AND D.intConfirm = 1 " +
                        "GROUP BY vchWBS1, vchWBS2, vchWBS3 ) B " +
                        "GROUP BY " +
                        "vchWBS3) A ";

                    }
                    else
                    {
                        //lSQL = "SELECT isNull(MAX(WBS3CT),0) FROM (SELECT " +
                        //"COUNT(vchWBS3) as WBS3CT " +
                        //"FROM ( " +
                        //"SELECT vchWBS1, vchWBS2, vchWBS3 " +
                        //"FROM dbo.WBSElements E, dbo.WBS W, " +
                        //"dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                        //"dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                        ////"dbo.ContractMaster C " +
                        //"WHERE W.intWBSId = E.intWBSId " +
                        //"AND P.intProjectId = W.intProjectId " +
                        ////"AND P.intContractID = C.intContractID " +
                        //"AND E.intWBSElementId = D.intWBSElementId " +
                        //"AND D.sitProductTypeID = T.sitProductTypeID " +
                        //"AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                        //"AND vchProjectCode = '" + ProjectCode + "' " +
                        //lSE_ProdTye +
                        //"AND vchWBS1 = '" + WBS1 + " ' " +
                        //lWBS2Con +
                        //"AND E.tntStatusId = 1 " +
                        //"AND D.intConfirm = 1 " +
                        //"GROUP BY vchWBS1, vchWBS2, vchWBS3 ) B " +
                        //"GROUP BY " +
                        //"vchWBS3) A ";

                        // PM & CM Chenged by chetan

                        lSQL = "SELECT isNull(MAX(WBS3CT),0) FROM (SELECT " +
                       "COUNT(vchWBS3) as WBS3CT " +
                       "FROM ( " +
                       "SELECT vchWBS1, vchWBS2, vchWBS3 " +
                       "FROM dbo.WBSElements E, dbo.WBS W, " +
                       "dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                       "dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                       // "dbo.SAPContractMaster C, " +
                       // "dbo.SAP_CONTRACT_PROJECT CP " +
                       "WHERE W.intWBSId = E.intWBSId " +
                       "AND P.intProjectId = W.intProjectId " +
                       //"AND C.vchContractCode = CP.CONTR_NO  " +
                       // "AND P.vchProjectCode= CP.PROJ_ID " +
                       "AND E.intWBSElementId = D.intWBSElementId " +
                       "AND D.sitProductTypeID = T.sitProductTypeID " +
                       "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                       "AND vchProjectCode = '" + ProjectCode + "' " +
                       lSE_ProdTye +
                       "AND vchWBS1 = '" + WBS1 + " ' " +
                       lWBS2Con +
                       "AND E.tntStatusId = 1 " +
                       "AND D.intConfirm = 1 " +
                       "GROUP BY vchWBS1, vchWBS2, vchWBS3 ) B " +
                       "GROUP BY " +
                       "vchWBS3) A ";

                    }

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.CommandTimeout = 1200;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        lRst.Read();
                        lWBS3CT = lRst.GetInt32(0);
                    }
                    lRst.Close();

                    if (liWBS2O >= liWBS2FR && liWBS2O <= liWBS2TO)
                    {
                        var lUprodType = getWBS3Dto.ProductType.Distinct().ToArray();
                        if (lUprodType.Length == 1 && lUprodType[0] == "CAB")
                        {
                            lSQL = "SELECT " +
                            "rtrim(ltrim(vchWBS3)) as WBS3 " +
                            "FROM ( " +
                            "SELECT vchWBS1, vchWBS2, vchWBS3 " +
                            "FROM dbo.WBSElements E, dbo.WBS W, " +
                            "dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                            "dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                            //"dbo.ContractMaster C " +
                            "WHERE W.intWBSId = E.intWBSId " +
                            "AND P.intProjectId = W.intProjectId " +
                            // "AND P.intContractID = C.intContractID " +
                            "AND E.intWBSElementId = D.intWBSElementId " +
                            "AND D.sitProductTypeID = T.sitProductTypeID " +
                            "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                            "AND vchProjectCode = '" + ProjectCode + "' " +
                            lSE_ProdTye +
                            "AND vchWBS1 = '" + WBS1 + "' " +
                            lWBS2Con +
                            "AND E.tntStatusId = 1 " +
                            "AND D.intConfirm = 1 " +
                            "GROUP BY  vchWBS1, vchWBS2, vchWBS3 ) B " +
                            "GROUP BY " +
                            "vchWBS3 HAVING COUNT(vchWBS3) >= " + lWBS3CT + " " +
                            "ORDER BY " +
                            "vchWBS3 ";

                            //     lSQL = "SELECT " +
                            //"rtrim(ltrim(vchWBS3)) as WBS3  " +
                            //"FROM dbo.WBSElements E, dbo.WBS W, " +
                            //"dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                            //"dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                            ////    ", dbo.SAPContractMaster C, " +
                            ////    "dbo.SAP_CONTRACT_PROJECT CP " +
                            //"WHERE W.intWBSId = E.intWBSId " +
                            //"AND P.intProjectId = W.intProjectId " +
                            ////   "AND C.vchContractCode = CP.CONTR_NO  " +
                            ////   "AND P.vchProjectCode= CP.PROJ_ID " +
                            //"AND E.intWBSElementId = D.intWBSElementId " +
                            //"AND D.sitProductTypeID = T.sitProductTypeID " +
                            //"AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                            //"AND vchProjectCode = '" + ProjectCode + "' " +
                            //lSE_ProdTye +
                            //"AND vchWBS1 = '" + WBS1 + "' " +
                            //"AND vchWBS2 = '" + WBS2FR + "' " +
                            //"AND E.tntStatusId = 1 " +
                            //"AND D.intConfirm = 1 " +
                            //"GROUP BY " +
                            //"vchWBS3  " +
                            //"ORDER BY " +
                            //"vchWBS3 ";



                        }
                        else
                        {
                            lSQL = "SELECT " +
                            "rtrim(ltrim(vchWBS3)) as WBS3 " +
                            "FROM ( " +
                            "SELECT vchWBS1, vchWBS2, vchWBS3 " +
                            "FROM dbo.WBSElements E, dbo.WBS W, " +
                            "dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                            "dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                            // "dbo.ContractMaster C " +
                            "WHERE W.intWBSId = E.intWBSId " +
                            "AND P.intProjectId = W.intProjectId " +
                            // "AND P.intContractID = C.intContractID " +
                            "AND E.intWBSElementId = D.intWBSElementId " +
                            "AND D.sitProductTypeID = T.sitProductTypeID " +
                            "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                            "AND vchProjectCode = '" + ProjectCode + "' " +
                            lSE_ProdTye +
                            "AND vchWBS1 = '" + WBS1 + "' " +
                            lWBS2Con +
                            // "AND vchWBS3 <> '" + WBS3O + "' " +//ajit
                            "AND E.tntStatusId = 1 " +
                            "AND D.intConfirm = 1 " +
                            "GROUP BY  vchWBS1, vchWBS2, vchWBS3 ) B " +
                            "GROUP BY " +
                            "vchWBS3 HAVING COUNT(vchWBS3) >= " + lWBS3CT + " " +
                            "ORDER BY " +
                            "vchWBS3 ";

                        }
                    }
                    else
                    {
                        lSQL = "SELECT " +
                        "rtrim(ltrim(vchWBS3)) as WBS3 " +
                        "FROM ( " +
                        "SELECT vchWBS1, vchWBS2, vchWBS3 " +
                        "FROM dbo.WBSElements E, dbo.WBS W, " +
                        "dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                        "dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                        //"dbo.ContractMaster C " +
                        "WHERE W.intWBSId = E.intWBSId " +
                        "AND P.intProjectId = W.intProjectId " +
                        // "AND P.intContractID = C.intContractID " +
                        "AND E.intWBSElementId = D.intWBSElementId " +
                        "AND D.sitProductTypeID = T.sitProductTypeID " +
                        "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                        "AND vchProjectCode = '" + ProjectCode + "' " +
                        lSE_ProdTye +
                        "AND vchWBS1 = '" + WBS1 + "' " +
                        lWBS2Con +
                        "AND E.tntStatusId = 1 " +
                        "AND D.intConfirm = 1 " +
                        "GROUP BY  vchWBS1, vchWBS2, vchWBS3 ) B " +
                        "GROUP BY " +
                        "vchWBS3 HAVING COUNT(vchWBS3) >= " + lWBS3CT + " " +
                        "ORDER BY " +
                        "vchWBS3 ";

                    }
                }
                else
                {
                    lSQL = "SELECT " +
                    "rtrim(ltrim(vchWBS3)) as WBS3  " +
                    "FROM dbo.WBSElements E, dbo.WBS W, " +
                    "dbo.SAPProjectMaster P, dbo.WBSElementsDetails D, " +
                    "dbo.ProductTypeMaster T, dbo.StructureElementMaster S " +
                    //"dbo.ContractMaster C " +
                    "WHERE W.intWBSId = E.intWBSId " +
                    "AND P.intProjectId = W.intProjectId " +
                    //  "AND P.intContractID = C.intContractID " +
                    "AND E.intWBSElementId = D.intWBSElementId " +
                    "AND D.sitProductTypeID = T.sitProductTypeID " +
                    "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                    "AND vchProjectCode = '" + ProjectCode + "' " +
                    lSE_ProdTye +
                    "AND vchWBS1 = '" + WBS1 + "' " +
                    "AND vchWBS2 = '" + WBS2FR + "' " +
                    // "AND vchWBS3 <> '" + WBS3O + "' " +//ajit
                    "AND E.tntStatusId = 1 " +
                    "AND D.intConfirm = 1 " +
                    "GROUP BY " +
                    "vchWBS3  " +
                    "ORDER BY " +
                    "vchWBS3 ";

                }

                lCmd.CommandType = CommandType.Text;
                lCmd.CommandText = lSQL;
                lCmd.Connection = cnNDS;
                lCmd.CommandTimeout = 300;
                lDa.SelectCommand = lCmd;
                lDs = new DataSet();
                lDa.Fill(lDs);
                if (lDs.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                    {
                        var lWBS3t = lDs.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                        lWBS3.Add(lWBS3t);
                    }
                }
                lProcess.CloseNDSConnection(ref cnNDS);
            }
            lDa = null;
            lCmd = null;
            lDs = null;
            cnNDS = null;

            //var content = extractWBS3(ProjectCode, WBS1, WBS2);

            return Ok(lWBS3);
        }


        [HttpPost]
        [Route("/setCloneOrderWBSClone/{CustomerCode}/{sProjectCode}/{dProjectCode}/{OrderNo}/{WBS1}/{WBS2From}/{WBS2To}/{CloneNo}/{StructureElement}/{UserName}")]
        public ActionResult setCloneOrderWBS([FromBody] GetWBS3SubmitDto getWBS3SubmitDto, string CustomerCode, string sProjectCode, string dProjectCode, int OrderNo,
    string WBS1, string WBS2From, string WBS2To, int CloneNo, string StructureElement, string UserName)
        {
            string lSQL;
            SqlCommand lCmd;
            SqlDataReader adoRst;
            SqlConnection lcnNDS = new SqlConnection();
            var lErrorMsg = "";
            int lNewOrderNo = 0;
            try
            {
                if (CloneNo > 0)
                {
                    var lProcess = new ProcessController();

                    lProcess.OpenNDSConnection(ref lcnNDS);
                    if (lcnNDS.State == ConnectionState.Open)
                    {

                        string lWBS2 = WBS2From;

                        var lHeader = db.OrderProject.Find(OrderNo);
                        if (lHeader == null || lHeader.CustomerCode == null ||
                            lHeader.CustomerCode != CustomerCode || lHeader.ProjectCode != sProjectCode)
                        {
                            lErrorMsg = "Invalid Order Number.";
                            return Ok(new { success = false, responseText = lErrorMsg });
                        }

                        var lHeaderSE = (from p in db.OrderProjectSE
                                         where p.OrderNumber == OrderNo
                                         select p).ToList();
                        if (lHeaderSE == null || lHeaderSE.Count == 0)
                        {
                            lErrorMsg = "Invalid Order Number.";
                            return Ok(new { success = false, responseText = lErrorMsg });
                        }

                        for (int i = 0; i < CloneNo; i++)
                        {

                            string lWBS3 = "";
                            for (int k = 0; k < getWBS3SubmitDto.WBS3.Count; k++)
                            {
                                lWBS3 = getWBS3SubmitDto.WBS3[k];

                                lNewOrderNo = 0;

                                int lWBSElementId = 0;

                                lSQL = "SELECT MAX(E.intWBSElementId) " +
                                "FROM dbo.WBSElements E, dbo.WBS W, dbo.SAPProjectMaster P, dbo.ContractMaster C " +
                                "WHERE W.intWBSId = E.intWBSId " +
                                "AND P.intProjectId = W.intProjectId " +
                                // "AND P.intContractID = C.intContractID " +
                                "AND vchProjectCode = '" + dProjectCode + "' " +
                                "AND vchWBS1 = '" + WBS1 + "' " +
                                "AND vchWBS2 = '" + lWBS2 + "' " +
                                "AND vchWBS3 = '" + lWBS3 + "' " +
                                "AND E.tntStatusId = 1 ";


                                lCmd = new SqlCommand(lSQL, lcnNDS);
                                //lCmd.Transaction = osqlTransNDS;
                                lCmd.CommandTimeout = 1200;
                                adoRst = lCmd.ExecuteReader();
                                if (adoRst.HasRows)
                                {
                                    adoRst.Read();
                                    lWBSElementId = (int)adoRst.GetValue(0);
                                }
                                adoRst.Close();

                                if (lWBSElementId > 0)
                                {
                                    for (int j = 0; j < lHeaderSE.Count; j++)
                                    {
                                        //Get PostID for Scheduled Order
                                        string lStructureEle = lHeaderSE[j].StructureElement;
                                        if (lStructureEle != StructureElement)
                                        {
                                            lStructureEle = StructureElement;
                                        }
                                        int lPostHeaderID = 0;

                                        string lProdType = lHeaderSE[j].ProductType;
                                        if (lProdType == "CUT-TO-SIZE-MESH" || lProdType == "COLUMN-LINK-MESH" || lProdType == "STIRRUP-LINK-MESH")
                                        {
                                            lProdType = "MSH";
                                        }
                                        else if (lProdType == "PRE-CAGE")
                                        {
                                            lProdType = "PRC";
                                        }

                                        //Core Cage Product Type added at line 10611

                                        //else if (lProdType == "CARPET" || lProdType == "CORE-CAGE")
                                        //{
                                        //    lProdType = "CAR";
                                        //}

                                        else if (lProdType == "CARPET")
                                        {
                                            lProdType = "CAR";
                                        }
                                        else if (lProdType == "CORE-CAGE")
                                        {
                                            lProdType = "CORE";
                                        }




                                        if (lHeaderSE[j].ScheduledProd == "Y")
                                        {

                                            var lContractNo = getContractNo(CustomerCode, dProjectCode, lProdType, lcnNDS);

                                            int lProjectID = 0;

                                            lSQL = "SELECT P.intProjectId FROM dbo.SAPProjectMaster P, dbo.ContractMaster C " +
                                            "WHERE  P.vchProjectCode = '" + dProjectCode + "' " +
                                            "AND C.vchContractNumber = '" + lContractNo + "'";


                                            lCmd = new SqlCommand(lSQL, lcnNDS);
                                            //lCmd.Transaction = osqlTransNDS;
                                            lCmd.CommandTimeout = 1200;
                                            adoRst = lCmd.ExecuteReader();
                                            if (adoRst.HasRows)
                                            {
                                                adoRst.Read();
                                                lProjectID = (int)adoRst.GetValue(0);
                                            }
                                            adoRst.Close();

                                            lSQL = "SELECT H.intPostHeaderId,  H.tntStatusId, H.intWBSElementId, H.vchBBSNo, H.BBS_DESC " +
                                                "from dbo.BBSPostHeader H, dbo.WBSElements E, dbo.WBS W, dbo.WBSElementsDetails D, dbo.SAPProjectMaster P  " +
                                                "where e.intWBSId = W.intWBSId " +
                                                "and E.intWBSElementId = D.intWBSElementId " +
                                                "and W.intProjectid = P.intProjectid " +
                                                "and P.intProjectid = 0" + lProjectID + " " +
                                                "and D.intConfirm  = 1 " +
                                                "and H.intWBSElementId = E.intWBSElementId " +
                                                "AND NOT EXISTS ( SELECT R.intPostHeaderid FROM dbo.BBSReleaseDetails R  " +
                                                "WHERE R.intPostHeaderid = H.intPostHeaderId  " +
                                                "AND R.tntStatusId = 12 ) " +
                                                "and E.vchWBS1 = '" + WBS1 + "' " +
                                                "and E.vchWBS2 = '" + lWBS2 + "' " +
                                                "and E.vchWBS3 = '" + lWBS3 + "' " +
                                                "and D.sitProductTypeId in " +
                                                "(SELECT sitProductTypeID " +
                                                "FROM dbo.ProductTypeMaster " +
                                                "WHERE vchProductType = '" + lProdType + "' ) " +
                                                "and D.intStructureElementTypeId in " +
                                                "(SELECT intStructureElementTypeId " +
                                                "FROM dbo.StructureElementMaster " +
                                                "WHERE vchStructureElementType = '" + lStructureEle + "' ) ";

                                            lCmd = new SqlCommand(lSQL, lcnNDS);
                                            lCmd.CommandTimeout = 1200;
                                            adoRst = lCmd.ExecuteReader();
                                            if (adoRst.HasRows)
                                            {
                                                if (adoRst.Read())
                                                {
                                                    lPostHeaderID = adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetInt32(0);
                                                }
                                            }
                                            adoRst.Close();

                                            #region Taken out the WBS Creation
                                            //if (lPostHeaderID == 0)
                                            //{
                                            //    var lErrorMsg1 = GenerateWBS(CustomerCode, ProjectCode, lContractNo, lStructureEle, lProdType, WBS1, lWBS2, lWBS3, lcnNDS);
                                            //    if (lErrorMsg1 == "")
                                            //    {
                                            //        for (int m = 0; m < 50; m++)
                                            //        {
                                            //            lSQL = "SELECT H.intPostHeaderId,  H.tntStatusId, H.intWBSElementId, H.vchBBSNo, H.BBS_DESC " +
                                            //                "from dbo.BBSPostHeader H, dbo.WBSElements E, dbo.WBS W, dbo.WBSElementsDetails D, dbo.ProjectMaster P  " +
                                            //                "where e.intWBSId = W.intWBSId " +
                                            //                "and E.intWBSElementId = D.intWBSElementId " +
                                            //                "and W.intProjectid = P.intProjectid " +
                                            //                "and P.intProjectid = 0" + lProjectID + " " +
                                            //                "and D.intConfirm  = 1 " +
                                            //                "and H.intWBSElementId = E.intWBSElementId " +
                                            //                "AND NOT EXISTS ( SELECT R.intPostHeaderid FROM dbo.BBSReleaseDetails R  " +
                                            //                "WHERE R.intPostHeaderid = H.intPostHeaderId  " +
                                            //                "AND R.tntStatusId = 14 ) " +
                                            //                "and E.vchWBS1 = '" + WBS1 + "' " +
                                            //                "and E.vchWBS2 = '" + lWBS2 + "' " +
                                            //                "and E.vchWBS3 = '" + lWBS3 + "' " +
                                            //                "and D.sitProductTypeId in " +
                                            //                "(SELECT sitProductTypeID " +
                                            //                "FROM dbo.ProductTypeMaster " +
                                            //                "WHERE vchProductType = '" + lProdType + "' ) " +
                                            //                "and D.intStructureElementTypeId in " +
                                            //                "(SELECT intStructureElementTypeId " +
                                            //                "FROM dbo.StructureElementMaster " +
                                            //                "WHERE vchStructureElementType = '" + lStructureEle + "' ) ";

                                            //            lCmd = new SqlCommand(lSQL, lcnNDS);
                                            //            lCmd.CommandTimeout = 1200;
                                            //            adoRst = lCmd.ExecuteReader();
                                            //            if (adoRst.HasRows)
                                            //            {
                                            //                if (adoRst.Read())
                                            //                {
                                            //                    lPostHeaderID = adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetInt32(0);
                                            //                }
                                            //            }
                                            //            adoRst.Close();
                                            //            if (lPostHeaderID > 0)
                                            //            {
                                            //                break;
                                            //            }
                                            //        }
                                            //    }
                                            //    else
                                            //    {
                                            //        return Json(new { success = false, responseText = lErrorMsg1 }, JsonRequestBehavior.AllowGet);
                                            //    }
                                            //}
                                            #endregion

                                            if (lPostHeaderID == 0)
                                            {
                                                lErrorMsg = "Could not find the WBS (" + WBS1 + ", " + lWBS2 + ", " + lWBS3 + ") for copy. ";
                                                return Ok(new { success = false, responseText = lErrorMsg });
                                            }

                                        }

                                        string lStrutEle = lHeaderSE[j].StructureElement;
                                        if (lStrutEle != StructureElement)
                                        {
                                            lStrutEle = StructureElement;
                                        }
                                        if (lStrutEle == null)
                                        {
                                            lStrutEle = "";
                                        }
                                        lStrutEle = lStrutEle.Trim();
                                        if (lStrutEle.Length == 0)
                                        {
                                            lStrutEle = "NONWBS";
                                        }

                                        lProdType = lHeaderSE[j].ProductType;
                                        if (lProdType == null)
                                        {
                                            lProdType = "";
                                        }
                                        lProdType = lProdType.Trim();

                                        string lScheduledProd = lHeaderSE[j].ScheduledProd;
                                        if (lScheduledProd == null)
                                        {
                                            lScheduledProd = "N";
                                        }
                                        lScheduledProd = lScheduledProd.Trim();

                                        AddToCart obj = new AddToCart();
                                        obj.pCustomerCode = CustomerCode;
                                        obj.pProjectCode = dProjectCode;
                                        obj.pOrderType = lHeader.OrderType;
                                        obj.pOrderNo = lNewOrderNo;
                                        obj.pStructureElement = lStrutEle;
                                        obj.pProductType = lProdType;
                                        obj.pWBS1 = WBS1;
                                        obj.pWBS2 = lWBS2;
                                        obj.pWBS3 = lWBS3;
                                        obj.pPONo = lHeaderSE[j].PONumber;
                                        obj.pScheduledProd = lHeaderSE[j].ScheduledProd == null ? "N" : lHeaderSE[j].ScheduledProd.Trim();
                                        obj.pPostID = lPostHeaderID;
                                        int lCreatedOrder = AddToCart(
                                           obj
                                            );

                                        lNewOrderNo = lCreatedOrder;

                                        var lHeaderSENew = db.OrderProjectSE.Find(lNewOrderNo, lStrutEle, lProdType, lScheduledProd.Trim());

                                        if (lHeaderSENew != null && lHeaderSENew.OrderNumber == lNewOrderNo)
                                        {
                                            var lSENew = lHeaderSENew;
                                            if (lScheduledProd != "Y")
                                            {
                                                //Clone CAB
                                                if (lHeaderSE[j].CABJobID > 0 && lHeaderSENew.CABJobID > 0)
                                                {
                                                    cloneCAB(CustomerCode, sProjectCode, lHeaderSE[j].CABJobID, CustomerCode, dProjectCode, lHeaderSENew.CABJobID, UserName);
                                                }
                                                //Clone MESH
                                                if (lHeaderSE[j].MESHJobID > 0)
                                                {
                                                    cloneMeshProducts(CustomerCode, sProjectCode, lHeaderSE[j].MESHJobID, CustomerCode, dProjectCode, lHeaderSENew.MESHJobID, UserName);
                                                }
                                                //Clone BPC
                                                if (lHeaderSE[j].BPCJobID > 0)
                                                {
                                                    cloneBPC(CustomerCode, sProjectCode, lHeaderSE[j].BPCJobID, CustomerCode, dProjectCode, lHeaderSENew.BPCJobID, UserName);
                                                }
                                                //Clone CAGE
                                                if (lHeaderSE[j].CageJobID > 0)
                                                {
                                                    cloneCageProducts(CustomerCode, sProjectCode, lHeaderSE[j].CageJobID, CustomerCode, dProjectCode, lHeaderSENew.CageJobID);
                                                }
                                                //Clone Carpet
                                                if (lHeaderSE[j].CarpetJobID > 0)
                                                {

                                                }
                                                //Clone Standrad Bar
                                                if (lHeaderSE[j].StdBarsJobID > 0)
                                                {
                                                    cloneStdProducts(CustomerCode, sProjectCode, lHeaderSE[j].StdBarsJobID, CustomerCode, dProjectCode, lHeaderSENew.StdBarsJobID, UserName);
                                                }
                                                //Clone Standrad Mesh
                                                if (lHeaderSE[j].StdMESHJobID > 0)
                                                {
                                                    cloneStdProducts(CustomerCode, sProjectCode, lHeaderSE[j].StdMESHJobID, CustomerCode, dProjectCode, lHeaderSENew.StdMESHJobID, UserName);
                                                }
                                                //Clone Coil Products
                                                if (lHeaderSE[j].CoilProdJobID > 0)
                                                {
                                                    cloneStdProducts(CustomerCode, sProjectCode, lHeaderSE[j].CoilProdJobID, CustomerCode, dProjectCode, lHeaderSENew.CoilProdJobID, UserName);
                                                }
                                            }
                                            lSENew.OrderNumber = lNewOrderNo;
                                            lSENew.OrderStatus = "Created";
                                            //lSENew.PODate = lHeaderSE[j].PODate;
                                            lSENew.PODate = DateTime.Now;
                                            lSENew.PONumber = lHeaderSE[j].PONumber;

                                            var lItemReqDate = lHeaderSE[j].RequiredDate;
                                            lSENew.RequiredDate = lItemReqDate;
                                            lSENew.OrigReqDate = lItemReqDate;

                                            lSENew.TotalWeight = lHeaderSE[j].TotalWeight;
                                            lSENew.TotalPCs = lHeaderSE[j].TotalPCs;
                                            lSENew.TransportMode = lHeaderSE[j].TransportMode;
                                            lSENew.UpdateBy = UserName;//User.Identity.GetUserName();
                                            lSENew.UpdateDate = DateTime.Now;

                                            db.Entry(lHeaderSENew).CurrentValues.SetValues(lSENew);
                                        }

                                    }

                                    var lNewHeader = db.OrderProject.Find(lNewOrderNo);

                                    var lOrderNew = lNewHeader;

                                    lOrderNew.DeliveryAddress = lHeader.DeliveryAddress;
                                    lOrderNew.OrderSource = lHeader.OrderSource;
                                    lOrderNew.OrderStatus = "Created";
                                    lOrderNew.OrderType = lHeader.OrderType;
                                    //lOrderNew.PODate = lHeader.PODate;
                                    lOrderNew.PODate = DateTime.Now;
                                    lOrderNew.PONumber = lHeader.PONumber;
                                    lOrderNew.Remarks = lHeader.Remarks;

                                    var lHeaderReqDate = lHeader.RequiredDate;
                                    lOrderNew.RequiredDate = lHeaderReqDate;
                                    lOrderNew.OrigReqDate = lHeaderReqDate;

                                    lOrderNew.Scheduler_Email = lHeader.Scheduler_Email;
                                    lOrderNew.Scheduler_HP = lHeader.Scheduler_HP;
                                    lOrderNew.Scheduler_Name = lHeader.Scheduler_Name;
                                    lOrderNew.SiteEngr_Email = lHeader.SiteEngr_Email;
                                    lOrderNew.SiteEngr_HP = lHeader.SiteEngr_HP;
                                    lOrderNew.SiteEngr_Name = lHeader.SiteEngr_Name;
                                    lOrderNew.SubmitBy = lHeader.SubmitBy;
                                    lOrderNew.SubmitDate = lHeader.SubmitDate;
                                    lOrderNew.TotalWeight = lHeader.TotalWeight;
                                    lOrderNew.TransportMode = lHeader.TransportMode;
                                    lOrderNew.UpdateBy = UserName;//User.Identity.GetUserName();
                                    lOrderNew.UpdateDate = DateTime.Now;
                                    lOrderNew.WBS1 = WBS1;
                                    lOrderNew.WBS2 = lWBS2;
                                    lOrderNew.WBS3 = lWBS3;
                                    lOrderNew.GreenSteel = lHeader.GreenSteel;
                                    lOrderNew.Address = lHeader.Address;
                                    lOrderNew.Gate = lHeader.Gate;
                                    lOrderNew.AddressCode = lHeader.AddressCode;

                                    db.Entry(lNewHeader).CurrentValues.SetValues(lOrderNew);
                                    db.SaveChanges();
                                }
                            }

                            int lWBS2No = 0;
                            int.TryParse(lWBS2, out lWBS2No);
                            if (lWBS2No > 0)
                            {
                                lWBS2No = lWBS2No + 1;
                            }
                            lWBS2 = lWBS2No.ToString();

                        }
                    }
                    lProcess.CloseNDSConnection(ref lcnNDS);
                }
                return Ok(new { success = true, responseText = "Successfully saved." });
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            return Ok(new { success = false, responseText = lErrorMsg });
        }

        [HttpGet]
        [Route("/cloneCageProducts/{pOldCustomerCode}/{pOldProjectCode}/{pOldJobID}/{pNewCustomerCode}/{pNewProjectCode}/{pNewJobID}")]

        public int cloneCageProducts(string pOldCustomerCode, string pOldProjectCode, int pOldJobID, string pNewCustomerCode, string pNewProjectCode, int pNewJobID)
        {
            int lReturn = 0;

            var lJobNewOr = db.PRCJobAdvice.Find(pNewCustomerCode, pNewProjectCode, pNewJobID);
            var lJobNew = lJobNewOr;
            var lJobOld = db.PRCJobAdvice.Find(pOldCustomerCode, pOldProjectCode, pOldJobID);
            if (lJobOld != null && lJobNew != null)
            {
                lJobNew.OrderStatus = "New";
                //lJobNew.PODate = lJobOld.PODate;
                lJobNew.PODate = DateTime.Now;
                lJobNew.PONumber = lJobOld.PONumber;
                lJobNew.DeliveryAddress = lJobOld.DeliveryAddress;
                lJobNew.Remarks = lJobOld.Remarks;
                lJobNew.RequiredDate = lJobOld.RequiredDate;
                lJobNew.Scheduler_HP = lJobOld.Scheduler_HP;
                lJobNew.Scheduler_Name = lJobOld.Scheduler_Name;
                lJobNew.Scheduler_Tel = lJobOld.Scheduler_Tel;
                lJobNew.SiteEngr_HP = lJobOld.SiteEngr_HP;
                lJobNew.SiteEngr_Name = lJobOld.SiteEngr_Name;
                lJobNew.SiteEngr_Tel = lJobOld.SiteEngr_Tel;
                lJobNew.TotalPcs = lJobOld.TotalPcs;
                lJobNew.TotalWeight = lJobOld.TotalWeight;
                lJobNew.Transport = lJobOld.Transport;
                lJobNew.Model = lJobOld.Model;
                lJobNew.OrderSource = lJobOld.OrderSource;

                lJobNew.WBS1 = lJobOld.WBS1;
                lJobNew.WBS2 = lJobOld.WBS2;
                lJobNew.WBS3 = lJobOld.WBS3;

                lJobNew.UpdateBy = User.Identity.GetUserName();
                lJobNew.UpdateDate = DateTime.Now;
                db.Entry(lJobNewOr).CurrentValues.SetValues(lJobNew);
            }

            // clone BBS
            var lBBSDel = (from p in db.PRCBBS
                           where p.CustomerCode == pNewCustomerCode &&
                           p.ProjectCode == pNewProjectCode &&
                           p.JobID == pNewJobID
                           select p).ToList();
            if (lBBSDel != null && lBBSDel.Count > 0)
            {
                db.PRCBBS.RemoveRange(lBBSDel);
            }

            var lOldBBS = (from p in db.PRCBBS
                           where p.CustomerCode == pOldCustomerCode &&
                           p.ProjectCode == pOldProjectCode &&
                           p.JobID == pOldJobID
                           select p).ToList();
            if (lOldBBS != null && lOldBBS.Count > 0)
            {
                var lNewBBS = new List<PRCBBSModels>();
                for (int j = 0; j < lOldBBS.Count; j++)
                {

                    lNewBBS.Add(new PRCBBSModels
                    {
                        CustomerCode = pNewCustomerCode,
                        ProjectCode = pNewProjectCode,
                        JobID = (int)pNewJobID,
                        BBSID = lOldBBS[j].BBSID,
                        BBSOrder = lOldBBS[j].BBSOrder,
                        BBSStrucElem = lOldBBS[j].BBSStrucElem,
                        BBSAssembly = lOldBBS[j].BBSAssembly,
                        BBSDrawing = lOldBBS[j].BBSDrawing,
                        BBSWidth = lOldBBS[j].BBSWidth,
                        BBSDepth = lOldBBS[j].BBSDepth,
                        BBSLength = lOldBBS[j].BBSLength,
                        BBSQty = lOldBBS[j].BBSQty,
                        BBSCABPcs = lOldBBS[j].BBSCABPcs,
                        BBSCABWT = lOldBBS[j].BBSCABWT,
                        BBSBeamMESHPcs = lOldBBS[j].BBSBeamMESHPcs,
                        BBSBeamMESHWT = lOldBBS[j].BBSBeamMESHWT,
                        BBSColumnMESHPcs = lOldBBS[j].BBSColumnMESHPcs,
                        BBSColumnMESHWT = lOldBBS[j].BBSColumnMESHWT,
                        BBSCTSMESHPcs = lOldBBS[j].BBSCTSMESHPcs,
                        BBSCTSMESHWT = lOldBBS[j].BBSCTSMESHWT,
                        BBSTotalPcs = lOldBBS[j].BBSTotalPcs,
                        BBSTotalWT = lOldBBS[j].BBSTotalWT,
                        BBSCageMark = lOldBBS[j].BBSCageMark,
                        BBSRemarks = lOldBBS[j].BBSRemarks,
                        BBSNDSPostID = lOldBBS[j].BBSNDSPostID,
                        BBSNDSGroupMark = lOldBBS[j].BBSNDSGroupMark,
                        BBSNDSGroupMarkID = lOldBBS[j].BBSNDSGroupMarkID,
                        BBSSOR = "",
                        UpdateBy = User.Identity.GetUserName(),
                        UpdateDate = DateTime.Now
                    });
                }
                db.PRCBBS.AddRange(lNewBBS);
            }

            var lDelBeam = (from p in db.PRCBeamMESHDetails
                            where p.CustomerCode == pNewCustomerCode &&
                            p.ProjectCode == pNewProjectCode &&
                            p.JobID == pNewJobID
                            select p).ToList();
            if (lDelBeam != null && lDelBeam.Count > 0)
            {
                db.PRCBeamMESHDetails.RemoveRange(lDelBeam);
            }

            var lOldBeam = (from p in db.PRCBeamMESHDetails
                            where p.CustomerCode == pOldCustomerCode &&
                            p.ProjectCode == pOldProjectCode &&
                            p.JobID == pOldJobID
                            select p).ToList();
            if (lOldBeam != null && lOldBeam.Count > 0)
            {
                var lNewBeam = new List<PRCBeamMESHDetailsModels>();
                for (int j = 0; j < lOldBeam.Count; j++)
                {

                    lNewBeam.Add(new PRCBeamMESHDetailsModels
                    {
                        CustomerCode = pNewCustomerCode,
                        ProjectCode = pNewProjectCode,
                        JobID = (int)pNewJobID,

                        BBSID = lOldBeam[j].BBSID,
                        MeshID = lOldBeam[j].MeshID,
                        MeshSort = lOldBeam[j].MeshSort,
                        MeshMark = lOldBeam[j].MeshMark,
                        MeshWidth = lOldBeam[j].MeshWidth,
                        MeshDepth = lOldBeam[j].MeshDepth,
                        MeshSlope = lOldBeam[j].MeshSlope,
                        MeshProduct = lOldBeam[j].MeshProduct,
                        MeshShapeCode = lOldBeam[j].MeshShapeCode,
                        MeshTotalLinks = lOldBeam[j].MeshTotalLinks,
                        MeshSpan = lOldBeam[j].MeshSpan,
                        MeshMemberQty = lOldBeam[j].MeshMemberQty,
                        MeshCapping = lOldBeam[j].MeshCapping,
                        MeshCPProduct = lOldBeam[j].MeshCPProduct,
                        A = lOldBeam[j].A,
                        B = lOldBeam[j].B,
                        C = lOldBeam[j].C,
                        D = lOldBeam[j].D,
                        E = lOldBeam[j].E,
                        P = lOldBeam[j].P,
                        Q = lOldBeam[j].Q,
                        HOOK = lOldBeam[j].HOOK,
                        LEG = lOldBeam[j].LEG,
                        MeshTotalWT = lOldBeam[j].MeshTotalWT,
                        Remarks = lOldBeam[j].Remarks,
                        MWLength = lOldBeam[j].MWLength,
                        MWBOM = lOldBeam[j].MWBOM,
                        CWBOM = lOldBeam[j].CWBOM,

                        UpdateBy = User.Identity.GetUserName(),
                        UpdateDate = DateTime.Now
                    });
                }
                db.PRCBeamMESHDetails.AddRange(lNewBeam);
            }

            var lOlColumn = (from p in db.PRCColumnMESHDetails
                             where p.CustomerCode == pOldCustomerCode &&
                             p.ProjectCode == pOldProjectCode &&
                             p.JobID == pOldJobID
                             select p).ToList();
            if (lOlColumn != null && lOlColumn.Count > 0)
            {
                var lNewColumn = new List<PRCColumnMESHDetailsModels>();
                for (int j = 0; j < lOlColumn.Count; j++)
                {

                    lNewColumn.Add(new PRCColumnMESHDetailsModels
                    {
                        CustomerCode = pNewCustomerCode,
                        ProjectCode = pNewProjectCode,
                        JobID = (int)pNewJobID,

                        BBSID = lOlColumn[j].BBSID,
                        MeshID = lOlColumn[j].MeshID,
                        MeshSort = lOlColumn[j].MeshSort,
                        MeshMark = lOlColumn[j].MeshMark,
                        MeshWidth = lOlColumn[j].MeshWidth,
                        MeshLength = lOlColumn[j].MeshLength,
                        MeshProduct = lOlColumn[j].MeshProduct,
                        MeshShapeCode = lOlColumn[j].MeshShapeCode,
                        MeshTotalLinks = lOlColumn[j].MeshTotalLinks,
                        MeshHeight = lOlColumn[j].MeshHeight,
                        MeshMemberQty = lOlColumn[j].MeshMemberQty,
                        MeshCLinkRowsAtLen = lOlColumn[j].MeshCLinkRowsAtLen,
                        MeshCLinkProductAtLen = lOlColumn[j].MeshCLinkProductAtLen,
                        MeshCLinkRowsAtWidth = lOlColumn[j].MeshCLinkRowsAtWidth,
                        MeshCLinkProductAtWidth = lOlColumn[j].MeshCLinkProductAtWidth,
                        A = lOlColumn[j].A,
                        B = lOlColumn[j].B,
                        C = lOlColumn[j].C,
                        D = lOlColumn[j].D,
                        E = lOlColumn[j].E,
                        F = lOlColumn[j].F,
                        P = lOlColumn[j].P,
                        Q = lOlColumn[j].Q,
                        LEG = lOlColumn[j].LEG,
                        MeshTotalWT = lOlColumn[j].MeshTotalWT,
                        Remarks = lOlColumn[j].Remarks,
                        MWLength = lOlColumn[j].MWLength,
                        MWBOM = lOlColumn[j].MWBOM,
                        CWBOM = lOlColumn[j].CWBOM,

                        UpdateBy = User.Identity.GetUserName(),
                        UpdateDate = DateTime.Now
                    });
                }
                db.PRCColumnMESHDetails.AddRange(lNewColumn);
            }

            var lOldSlab = (from p in db.PRCCTSMESHDetails
                            where p.CustomerCode == pOldCustomerCode &&
                            p.ProjectCode == pOldProjectCode &&
                            p.JobID == pOldJobID
                            select p).ToList();
            if (lOldSlab != null && lOldSlab.Count > 0)
            {
                var lNewSlab = new List<PRCCTSMESHDetailsModels>();
                for (int j = 0; j < lOldSlab.Count; j++)
                {

                    lNewSlab.Add(new PRCCTSMESHDetailsModels
                    {
                        CustomerCode = pNewCustomerCode,
                        ProjectCode = pNewProjectCode,
                        JobID = (int)pNewJobID,

                        BBSID = lOldSlab[j].BBSID,
                        MeshID = lOldSlab[j].MeshID,
                        MeshSort = lOldSlab[j].MeshSort,
                        MeshMark = lOldSlab[j].MeshMark,
                        MeshProduct = lOldSlab[j].MeshProduct,
                        MeshMainLen = lOldSlab[j].MeshMainLen,
                        MeshCrossLen = lOldSlab[j].MeshCrossLen,
                        MeshMO1 = lOldSlab[j].MeshMO1,
                        MeshMO2 = lOldSlab[j].MeshMO2,
                        MeshCO1 = lOldSlab[j].MeshCO1,
                        MeshCO2 = lOldSlab[j].MeshCO2,
                        MeshMemberQty = lOldSlab[j].MeshMemberQty,
                        MeshShapeCode = lOldSlab[j].MeshShapeCode,

                        A = lOldSlab[j].A,
                        B = lOldSlab[j].B,
                        C = lOldSlab[j].C,
                        D = lOldSlab[j].D,
                        E = lOldSlab[j].E,
                        F = lOldSlab[j].F,
                        G = lOldSlab[j].G,
                        H = lOldSlab[j].H,
                        I = lOldSlab[j].I,
                        J = lOldSlab[j].J,
                        K = lOldSlab[j].K,
                        L = lOldSlab[j].L,
                        M = lOldSlab[j].M,
                        N = lOldSlab[j].N,
                        O = lOldSlab[j].O,
                        P = lOldSlab[j].P,
                        Q = lOldSlab[j].Q,
                        R = lOldSlab[j].R,
                        S = lOldSlab[j].S,
                        T = lOldSlab[j].T,
                        U = lOldSlab[j].U,
                        V = lOldSlab[j].V,
                        W = lOldSlab[j].W,
                        X = lOldSlab[j].X,
                        Y = lOldSlab[j].Y,
                        Z = lOldSlab[j].Z,

                        HOOK = lOldBeam[j].HOOK,
                        MeshTotalWT = lOldBeam[j].MeshTotalWT,
                        Remarks = lOldBeam[j].Remarks,
                        MWBOM = lOldBeam[j].MWBOM,
                        CWBOM = lOldBeam[j].CWBOM,

                        UpdateBy = User.Identity.GetUserName(),
                        UpdateDate = DateTime.Now
                    });
                }
                db.PRCCTSMESHDetails.AddRange(lNewSlab);
            }

            db.SaveChanges();
            return lReturn;
        }


        [HttpGet]
        [Route("/downloadOrderDoc/{OrderNumber}/{StructureElement}/{ProductType}/{ScheduledProd}/{DocID}")]
        public ActionResult downloadOrderDoc(int OrderNumber, string StructureElement, string ProductType, string ScheduledProd, int DocID)
        {
            StructureElement = StructureElement.Trim();
            ProductType = ProductType.Trim();
            ScheduledProd = ScheduledProd.Trim();
            var Content = new OrderDocsModels();
            try
            {
                Content = db.OrderDocs.Find(OrderNumber, StructureElement, ProductType, ScheduledProd, 0, DocID);
                if (Content == null)
                {
                    Content = new OrderDocsModels();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
            }

            string fileName = "abc.pdf";

            // Create a MemoryStream from the byte array
            using (MemoryStream memoryStream = new MemoryStream(Content.OrderDoc))
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(memoryStream);
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                return File(response.Content.ReadAsByteArrayAsync().Result, "application/pdf");
            }
            //var lReturn = Ok(Content);
            ////lReturn.MaxJsonLength = 50000000;
            //return lReturn;
        }


        [HttpGet]
        [Route("/ValidatePONumber_OS/{CustomerCode}/{ProjectCode}/{PONumber}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult ValidatePONumber(string CustomerCode, string ProjectCode, string PONumber)
        {
            var lErrorMsg = "";
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            string lReturn = "";

            try
            {
                lCmd.CommandText =
                "SELECT PO_NUM " +
                "FROM dbo.SAP_REQUEST_HDR " +
                "WHERE PROJ_ID = '" + ProjectCode + "' " +
                "AND PO_NUM = '" + PONumber + "' " +
                "AND STATUS <> 'X' " +
                "UNION " +
                "SELECT PONumber FROM dbo.OESProjOrder " +
                "WHERE CustomerCode = '" + CustomerCode + "' " +
                "AND ProjectCode = '" + ProjectCode + "' " +
                "AND PONumber = '" + PONumber + "' " +
                "AND OrderStatus <> 'Deleted' " +
                "AND OrderStatus <> 'New' " +
                "AND OrderStatus <> 'Created' " +
                "AND OrderStatus <> 'Cancelled' " +
                "UNION " +
                "SELECT S.PONumber " +
                "FROM dbo.OESProjOrder H, dbo.OESProjOrdersSE S " +
                "WHERE H.OrderNumber = S.OrderNumber " +
                "AND H.CustomerCode = '" + CustomerCode + "' " +
                "AND H.ProjectCode = '" + ProjectCode + "' " +
                "AND S.PONumber = '" + PONumber + "' " +
                "AND S.OrderStatus <> 'Deleted' " +
                "AND S.OrderStatus <> 'New' " +
                "AND S.OrderStatus <> 'Created' " +
                "AND S.OrderStatus <> 'Cancelled' " +
                "ORDER BY 1 DESC ";

                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lReturn = lRst.GetValue(0) == System.DBNull.Value ? "" : lRst.GetString(0).Trim();
                        }
                    }
                    lRst.Close();

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }
                lProcessObj = null;

                lCmd = null;
                lNDSCon = null;
                lRst = null;
                return Json(new { success = true, responseText = lReturn });
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                return Json(new { success = false, responseText = lErrorMsg });
            }
        }

        [HttpGet]
        [Route("/getContractNo_WBSExtension/{pCustomerCode}/{pProjectCode}/{pProdType}")]
        public string getContractNo_WBSExtension(string pCustomerCode, string pProjectCode, string pProdType)
        {
            SqlConnection lcnNDS = new SqlConnection();
            var lProcess = new ProcessController();
            string lContractNo = "";
            lProcess.OpenNDSConnection(ref lcnNDS);
            if (lcnNDS.State == ConnectionState.Open)
            {

                var lCmd = new SqlCommand();
                SqlDataReader lRst;
                string lSQL = "";

                lSQL = "SELECT isNull(MAX(C.vchContractCode), '')  " +
                "FROM dbo.SAPContractMaster C, dbo.CustomerMaster U, " +
                "dbo.ContractProductMapping M, dbo.SAPProjectMaster P " +
                "WHERE C.CUST_ID = U.vchCustomerNo " +
                "AND C.vchContractCode = M.VBELN " +
               // "AND C.intContractID = P.intContractId " +
               "AND U.vchCustomerNo = '" + pCustomerCode + "' " +
               "AND P.vchProjectCode = '" + pProjectCode + "' ";


                //lSQL = "SELECT isNull(MAX(C.vchContractNumber), '') " +
                //"FROM dbo.ContractMaster C, dbo.CustomerMaster U,  " +
                //"dbo.ContractProductMapping M, dbo.SAPProjectMaster P " +
                //"WHERE C.intCustomerCode = U.intCustomerCode " +
                //"AND C.vchContractNumber = M.VBELN " +
                //// "AND C.intContractID = P.intContractId " +
                //"AND U.vchCustomerNo = '" + pCustomerCode + "' " +
                //"AND P.vchProjectCode = '" + pProjectCode + "' ";

                if (pProdType == "4")
                {
                    lSQL = lSQL + "AND M.ytot_cab > 0 ";

                }
                else if (pProdType == "7")
                {
                    lSQL = lSQL + "AND M.ytot_mesh > 0 ";

                }
                else if (pProdType == "10")
                {
                    lSQL = lSQL + "AND M.ytot_precage > 0 ";

                }
                else if (pProdType == "9")
                {
                    lSQL = lSQL + "AND M.ytot_bpc > 0 ";

                }
                else if (pProdType == "14")
                {
                    lSQL = lSQL + "AND M.ytot_car > 0 ";

                }

                lCmd = new SqlCommand(lSQL, lcnNDS);
                lCmd.CommandTimeout = 1200;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    lRst.Read();
                    lContractNo = lRst.GetString(0);
                }
                else
                {
                    lContractNo = "";
                }
                lRst.Close();

                if (lContractNo.Length > 0)
                {
                    lContractNo = "11111111111";
                }
                else
                {
                    lContractNo = "22222222222";
                }
                return lContractNo.ToString();
            }
            else
            {

                return lContractNo.ToString();
            }
        }

        [HttpGet]
        [Route("/OrderReferenceNumber")]
        public int OrderReferenceNumber()
        {
            var lDa = new SqlDataAdapter();
            var lCmd = new SqlCommand();
            var lDs = new DataSet();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;

            var refNo = 0;

            var lProcess = new ProcessController();
            lProcess.OpenNDSConnection(ref lNDSCon);
            if (lNDSCon.State == ConnectionState.Open)
            {
                var lSQL = "select top 1 OrderReferenceNo from OESProjOrder order by OrderReferenceNo desc ";

                lCmd.CommandText = lSQL;
                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    if (lRst.Read())
                    {
                        if (!lRst.IsDBNull(0)) // Check if the value is not null
                        {
                            refNo = lRst.GetInt32(0);
                        }
                        else
                        {
                            // Handle the case where the value is null
                            // For example, assign a default value or log a message
                            refNo = 0; // Example default value
                            //Console.WriteLine("The value is null.");
                        }
                    }
                }
                lProcess.CloseNDSConnection(ref lNDSCon);
            }
            lDa = null;
            lCmd = null;
            lDs = null;
            lNDSCon = null;

            return refNo;
        }


        [HttpPost]
        [Route("/AddToCartUI")]
        public ActionResult AddToCartUI([FromBody] AddToCart addToCart)
        {
            int lReturnOrderNo = 0;
            int lReturnRefNo = 0;
            string lErrorMsg = "";
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;
            string lSQL = "";

            string lCustomerCode = addToCart.pCustomerCode;
            string lProjectCode = addToCart.pProjectCode;
            int lOrderNumber = 0;
            string lStruEle = addToCart.pStructureElement;
            string lProductType = addToCart.pProductType;
            string lScheduledProd = addToCart.pScheduledProd == null ? "N" : addToCart.pScheduledProd.Trim();

            bool? pGreenSteel = addToCart.GreenSteel;
            string AddressCode = addToCart.AddressCode;

            string lCustomerVer = "";
            string lProjectVer = "";
            int lOrderVer = 0;
            string lStruEleVer = "";
            string lWBSVer = "";

            if (addToCart.pOrderNo > 0)
            {
                lOrderNumber = addToCart.pOrderNo;
            }

            lReturnOrderNo = lOrderNumber;

            if (lCustomerCode != null && lProjectCode != null && lCustomerCode != "" && lProjectCode != "")
            {
                lCustomerCode = lCustomerCode.Trim();
                lProjectCode = lProjectCode.Trim();

                if (lCustomerCode.Length > 0 && lCustomerCode.Length > 0)
                {
                    try
                    {
                        var lProcessObj = new ProcessController();
                        if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                        {
                            if (lOrderNumber > 0)
                            {
                                var lHeader = db.OrderProject.Find(lOrderNumber);

                                if (lHeader.OrderStatus == null || lHeader.OrderStatus == ""
                                    || lHeader.OrderStatus == "New" || lHeader.OrderStatus == "Created"
                                    || lHeader.OrderStatus == "Reserved" || lHeader.OrderStatus == "Created*"
                                    || lHeader.OrderStatus == "Sent")
                                {
                                    // Check order number
                                    lSQL =
                                    "SELECT CustomerCode, ProjectCode, OrderJobID, OrderReferenceNo " +
                                    "FROM dbo.OESProjOrder " +
                                    "WHERE OrderNumber = " + lOrderNumber.ToString() + " ";

                                    lCmd.CommandText = lSQL;
                                    lCmd.Connection = lNDSCon;
                                    lCmd.CommandTimeout = 300;
                                    lRst = lCmd.ExecuteReader();
                                    if (lRst.HasRows)
                                    {
                                        if (lRst.Read())
                                        {
                                            lCustomerVer = lRst.GetString(0).Trim();
                                            lProjectVer = lRst.GetString(1).Trim();
                                            lOrderVer = lRst.GetInt32(2);
                                            lReturnRefNo = lRst.GetInt32(3);
                                        }
                                    }
                                    lRst.Close();
                                    if (lCustomerVer != lCustomerCode || lProjectVer != lProjectCode)
                                    {
                                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                                        lReturnOrderNo = -1;
                                        lErrorMsg = "Invalid order number.";
                                        Json(new { OrderNumber = lReturnOrderNo, Refnumber = lReturnRefNo });
                                    }
                                    else
                                    {
                                        lReturnOrderNo = lOrderNumber;
                                    }

                                    //update WBS
                                    lSQL =
                                    "UPDATE dbo.OESProjOrder " +
                                    "SET OrderType = '" + addToCart.pOrderType + "' " +
                                    ", WBS1 = '" + addToCart.pWBS1 + "' " +
                                    ", WBS2 = '" + addToCart.pWBS2 + "' " +
                                    ", WBS3 = '" + addToCart.pWBS3 + "' " +
                                    ", UpdateDate = getDate() " +
                                    ", UpdateBy = '" + addToCart.UpdateBy + "' " +
                                    ", GreenSteel = '" + pGreenSteel + "' " +
                                    ", AddressCode = '" + AddressCode + "' " +
                                    "WHERE OrderNumber = " + lOrderNumber.ToString() + " ";

                                    lCmd.CommandText = lSQL;
                                    lCmd.Connection = lNDSCon;
                                    lCmd.CommandTimeout = 300;
                                    lCmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // generate order Number
                                int lJobID = 0;
                                int lRefNo = addToCart.pRefNo;
                                if (lRefNo == 0)
                                {

                                    //lRefNo = OrderReferenceNumber();
                                    //lRefNo = lRefNo + 1;

                                    String tempRef = lProcessObj.GetDocNo("NSH", "ORN");
                                    lRefNo = int.Parse(tempRef);
                                }
                                lSQL =
                                "SELECT isNULL(MAX(OrderJobID),0) FROM dbo.OESProjOrder " +
                                "WHERE CustomerCode  = '" + lCustomerCode + "' " +
                                "AND  ProjectCode = '" + lProjectCode + "' ";
                                lCmd.CommandText = lSQL;
                                lCmd.Connection = lNDSCon;
                                lCmd.CommandTimeout = 300;
                                lRst = lCmd.ExecuteReader();
                                if (lRst.HasRows)
                                {
                                    if (lRst.Read())
                                    {
                                        lJobID = lRst.GetInt32(0);
                                    }
                                }
                                lRst.Close();

                                lJobID = lJobID + 1;


                                lReturnRefNo = lRefNo;

                                lSQL =
                                "INSERT INTO dbo.OESProjOrder " +
                                "(CustomerCode " +
                                ", ProjectCode " +
                                ", OrderJobID " +
                                ", OrderType " +
                                ", WBS1 " +
                                ", WBS2 " +
                                ", WBS3 " +
                                ", OrderStatus " +
                                ", OrderSource " +
                                ", OrderShared " +
                                ", UpdateDate " +
                                ", UpdateBy " +
                                ", OrderReferenceNo " +
                                ", GreenSteel " +
                                ", AddressCode) " +
                                "VALUES " +
                                "('" + lCustomerCode + "' " +
                                ",'" + lProjectCode + "' " +
                                "," + lJobID.ToString() + " " +
                                ",'" + addToCart.pOrderType + "' " +
                                ",'" + addToCart.pWBS1 + "' " +
                                ",'" + addToCart.pWBS2 + "' " +
                                ",'" + addToCart.pWBS3 + "' " +
                                ",'Created' " +
                                ",'UX' " +
                                ",0 " +
                                ",getDate() " +
                                ",'" + addToCart.UpdateBy + "' " +
                                ",'" + lRefNo + "' " +
                                ",'" + pGreenSteel + "' " +
                                ",'" + AddressCode + "' ) ";
                                lCmd.CommandText = lSQL;
                                lCmd.Connection = lNDSCon;
                                lCmd.CommandTimeout = 300;
                                lCmd.ExecuteNonQuery();

                                lSQL =
                                "SELECT OrderNumber  " +
                                "FROM dbo.OESProjOrder " +
                                "WHERE CustomerCode = '" + lCustomerCode + "' " +
                                "AND ProjectCode = '" + lProjectCode + "' " +
                                "AND OrderJobID = " + lJobID.ToString() + " ";

                                lCmd.CommandText = lSQL;
                                lCmd.Connection = lNDSCon;
                                lCmd.CommandTimeout = 300;
                                lRst = lCmd.ExecuteReader();
                                if (lRst.HasRows)
                                {
                                    if (lRst.Read())
                                    {
                                        lReturnOrderNo = lRst.GetInt32(0);
                                        lOrderNumber = lReturnOrderNo;
                                    }
                                }
                                lRst.Close();
                            }

                            // structure element process
                            if (lReturnOrderNo >= 0)
                            {
                                // check Structure Element
                                int lFound = 0;
                                int lCABJobID = 0;
                                int lMESHJobID = 0;
                                int lBPCJobID = 0;
                                int lCageJobID = 0;
                                int lCarpetJobID = 0;
                                int lStdBarsJobID = 0;
                                int lStdMESHJobID = 0;
                                int lCoilProdJobID = 0;
                                int lPostHeaderID = 0;

                                lSQL =
                                "SELECT CABJobID, MESHJobID, BPCJobID, " +
                                "CageJobID, CarpetJobID, " +
                                "StdBarsJobID, StdMESHJobID, CoilProdJobID, " +
                                "isNULL(PostHeaderID, 0) " +
                                "FROM dbo.OESProjOrdersSE " +
                                "WHERE OrderNumber = " + lOrderNumber.ToString() + " " +
                                "AND StructureElement = '" + lStruEle + "' " +
                                "AND ProductType = '" + lProductType + "' " +
                                "AND ScheduledProd = '" + lScheduledProd + "' ";

                                lCmd.CommandText = lSQL;
                                lCmd.Connection = lNDSCon;
                                lCmd.CommandTimeout = 300;
                                lRst = lCmd.ExecuteReader();
                                if (lRst.HasRows)
                                {
                                    if (lRst.Read())
                                    {
                                        lFound = 1;
                                        lCABJobID = lRst.GetInt32(0);
                                        lMESHJobID = lRst.GetInt32(1);
                                        lBPCJobID = lRst.GetInt32(2);
                                        lCageJobID = lRst.GetInt32(3);
                                        lCarpetJobID = lRst.GetInt32(4);
                                        lStdBarsJobID = lRst.GetInt32(5);
                                        lStdMESHJobID = lRst.GetInt32(6);
                                        lCoilProdJobID = lRst.GetInt32(7);
                                        lPostHeaderID = lRst.GetInt32(8);
                                    }
                                }
                                lRst.Close();

                                if (lFound == 0)
                                {
                                    lSQL =
                                    "INSERT INTO dbo.OESProjOrdersSE " +
                                    "(OrderNumber " +
                                    ", StructureElement " +
                                    ", ProductType " +
                                    ", ScheduledProd" +
                                    ", CABJobID " +
                                    ", MESHJobID " +
                                    ", BPCJobID " +
                                    ", CageJobID " +
                                    ", CarpetJobID " +
                                    ", StdBarsJobID " +
                                    ", StdMESHJobID " +
                                    ", CoilProdJobID " +
                                    ", PostHeaderID " +
                                    ", OrderStatus " +
                                    ", TotalWeight " +
                                    ", UpdateDate " +
                                    ", UpdateBy) " +
                                    "VALUES " +
                                    "(" + lOrderNumber.ToString() + " " +
                                    ",'" + addToCart.pStructureElement.Trim() + "' " +
                                    ",'" + addToCart.pProductType.Trim() + "' " +
                                    ",'" + addToCart.pScheduledProd.Trim() + "' " +
                                    ",0 " +
                                    ",0 " +
                                    ",0 " +
                                    ",0 " +
                                    ",0 " +
                                    ",0 " +
                                    ",0 " +
                                    ",0 " +
                                    "," + addToCart.pPostID + " " +
                                    ",'Created' " +
                                    ",0 " +
                                    ",getDate() " +
                                    ",'" + addToCart.UpdateBy + "') ";

                                    lCmd.CommandText = lSQL;
                                    lCmd.Connection = lNDSCon;
                                    lCmd.CommandTimeout = 300;
                                    lCmd.ExecuteNonQuery();
                                }
                                if (addToCart.pScheduledProd != "Y")
                                {
                                    if (lProductType == "CAB")
                                    {
                                        if (lCABJobID == 0)
                                        {
                                            lCABJobID = createCABJobAdvice(lCustomerCode, lProjectCode, lStruEle, addToCart.pPONo, addToCart.UpdateBy);

                                            if (lCABJobID > 0)
                                            {
                                                lSQL =
                                                "UPDATE dbo.OESProjOrdersSE " +
                                                "SET CABJobID = " + lCABJobID + ", " +
                                                "UpdateDate = getDate(), " +
                                                "UpdateBy = '" + addToCart.UpdateBy + "' " +
                                                "WHERE OrderNumber = " + lOrderNumber.ToString() + " " +
                                                "AND StructureElement = '" + addToCart.pStructureElement + "' " +
                                                "AND ProductType = '" + addToCart.pProductType + "' " +
                                                "AND ScheduledProd = '" + addToCart.pScheduledProd.Trim() + "' ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lNDSCon;
                                                lCmd.CommandTimeout = 300;
                                                lCmd.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                lProcessObj.CloseNDSConnection(ref lNDSCon);
                                                lReturnOrderNo = -1;
                                                lErrorMsg = "Error on CAB job ID generation.";
                                            }
                                        }
                                    }
                                    else if (lProductType == "STANDARD-BAR")
                                    {
                                        if (lStdBarsJobID == 0)
                                        {
                                            lStdBarsJobID = createStdProductJobAdvice(lCustomerCode, lProjectCode, addToCart.UpdateBy);

                                            if (lStdBarsJobID > 0)
                                            {
                                                lSQL =
                                                "UPDATE dbo.OESProjOrdersSE " +
                                                "SET StdBarsJobID = " + lStdBarsJobID + ", " +
                                                "UpdateDate = getDate(), " +
                                                "UpdateBy = '" + addToCart.UpdateBy + "' " +
                                                "WHERE OrderNumber = " + lOrderNumber.ToString() + " " +
                                                "AND StructureElement = '" + addToCart.pStructureElement + "' " +
                                                "AND ProductType = '" + addToCart.pProductType + "' " +
                                                "AND ScheduledProd = '" + addToCart.pScheduledProd.Trim() + "' ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lNDSCon;
                                                lCmd.CommandTimeout = 300;
                                                lCmd.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                lProcessObj.CloseNDSConnection(ref lNDSCon);
                                                lReturnOrderNo = -1;
                                                lErrorMsg = "Error on CAB job ID generation.";
                                            }
                                        }
                                    }
                                    else if (lProductType == "CUT-TO-SIZE-MESH" || lProductType == "COLUMN-LINK-MESH" || lProductType == "STIRRUP-LINK-MESH")
                                    {
                                        if (lMESHJobID == 0)
                                        {
                                            lMESHJobID = createMeshJobAdvice(lCustomerCode, lProjectCode, addToCart.UpdateBy);

                                            if (lMESHJobID > 0)
                                            {
                                                lSQL =
                                                "UPDATE dbo.OESProjOrdersSE " +
                                                "SET MESHJobID = " + lMESHJobID + ", " +
                                                "UpdateDate = getDate(), " +
                                                "UpdateBy = '" + addToCart.UpdateBy + "' " +
                                                "WHERE OrderNumber = " + lOrderNumber.ToString() + " " +
                                                "AND StructureElement = '" + addToCart.pStructureElement + "' " +
                                                "AND ProductType = '" + addToCart.pProductType + "' " +
                                                "AND ScheduledProd = '" + addToCart.pScheduledProd.Trim() + "' ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lNDSCon;
                                                lCmd.CommandTimeout = 300;
                                                lCmd.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                lProcessObj.CloseNDSConnection(ref lNDSCon);
                                                lReturnOrderNo = -1;
                                                lErrorMsg = "Error on MESH job ID generation.";
                                            }
                                        }
                                    }
                                    else if (lProductType == "BPC")
                                    {
                                        if (lBPCJobID == 0)
                                        {
                                            lBPCJobID = createBPCJobAdvice(lCustomerCode, lProjectCode, addToCart.UpdateBy);

                                            if (lBPCJobID > 0)
                                            {
                                                lSQL =
                                                "UPDATE dbo.OESProjOrdersSE " +
                                                "SET BPCJobID = " + lBPCJobID + ", " +
                                                "UpdateDate = getDate(), " +
                                                "UpdateBy = '" + addToCart.UpdateBy + "' " +
                                                "WHERE OrderNumber = " + lOrderNumber.ToString() + " " +
                                                "AND StructureElement = '" + addToCart.pStructureElement + "' " +
                                                "AND ProductType = '" + addToCart.pProductType + "' " +
                                                "AND ScheduledProd = '" + addToCart.pScheduledProd.Trim() + "' ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lNDSCon;
                                                lCmd.CommandTimeout = 300;
                                                lCmd.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                lProcessObj.CloseNDSConnection(ref lNDSCon);
                                                lReturnOrderNo = -1;
                                                lErrorMsg = "Error on CAB job ID generation.";
                                            }
                                        }
                                    }
                                    else if (lProductType == "PRE-CAGE" || lProductType == "CORE-CAGE")
                                    {
                                        if (lCageJobID == 0)
                                        {
                                            lCageJobID = createCageJobAdvice(lCustomerCode, lProjectCode, addToCart.UpdateBy);

                                            if (lCageJobID > 0)
                                            {
                                                lSQL =
                                                "UPDATE dbo.OESProjOrdersSE " +
                                                "SET CageJobID = " + lCageJobID + ", " +
                                                "UpdateDate = getDate(), " +
                                                "UpdateBy = '" + addToCart.UpdateBy + "' " +
                                                "WHERE OrderNumber = " + lOrderNumber.ToString() + " " +
                                                "AND StructureElement = '" + addToCart.pStructureElement + "' " +
                                                "AND ProductType = '" + addToCart.pProductType + "' " +
                                                "AND ScheduledProd = '" + addToCart.pScheduledProd.Trim() + "' ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lNDSCon;
                                                lCmd.CommandTimeout = 300;
                                                lCmd.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                lProcessObj.CloseNDSConnection(ref lNDSCon);
                                                lReturnOrderNo = -1;
                                                lErrorMsg = "Error on MESH job ID generation.";
                                            }
                                        }
                                    }
                                    else if (lProductType == "STANDARD-MESH")
                                    {
                                        if (lStdMESHJobID == 0)
                                        {
                                            lStdMESHJobID = createStdProductJobAdvice(lCustomerCode, lProjectCode, addToCart.UpdateBy);

                                            if (lStdMESHJobID > 0)
                                            {
                                                lSQL =
                                                "UPDATE dbo.OESProjOrdersSE " +
                                                "SET StdMESHJobID = " + lStdMESHJobID + ", " +
                                                "UpdateDate = getDate(), " +
                                                "UpdateBy = '" + addToCart.UpdateBy + "' " +
                                                "WHERE OrderNumber = " + lOrderNumber.ToString() + " " +
                                                "AND StructureElement = '" + addToCart.pStructureElement + "' " +
                                                "AND ProductType = '" + addToCart.pProductType + "' " +
                                                "AND ScheduledProd = '" + addToCart.pScheduledProd.Trim() + "' ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lNDSCon;
                                                lCmd.CommandTimeout = 300;
                                                lCmd.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                lProcessObj.CloseNDSConnection(ref lNDSCon);
                                                lReturnOrderNo = -1;
                                                lErrorMsg = "Error on Standard Product job ID generation.";
                                            }
                                        }
                                    }
                                    else if (lProductType == "COIL")
                                    {
                                        if (lCoilProdJobID == 0)
                                        {
                                            lCoilProdJobID = createStdProductJobAdvice(lCustomerCode, lProjectCode, addToCart.UpdateBy);

                                            if (lCoilProdJobID > 0)
                                            {
                                                lSQL =
                                                "UPDATE dbo.OESProjOrdersSE " +
                                                "SET CoilProdJobID = " + lCoilProdJobID + ", " +
                                                "UpdateDate = getDate(), " +
                                                "UpdateBy = '" + addToCart.UpdateBy + "' " +
                                                "WHERE OrderNumber = " + lOrderNumber.ToString() + " " +
                                                "AND StructureElement = '" + addToCart.pStructureElement + "' " +
                                                "AND ProductType = '" + addToCart.pProductType + "' " +
                                                "AND ScheduledProd = '" + addToCart.pScheduledProd.Trim() + "' ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lNDSCon;
                                                lCmd.CommandTimeout = 300;
                                                lCmd.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                lProcessObj.CloseNDSConnection(ref lNDSCon);
                                                lReturnOrderNo = -1;
                                                lErrorMsg = "Error on Standard Product job ID generation.";
                                            }
                                        }

                                    }
                                    else if (lProductType == "COUPLER")
                                    {
                                        if (lCoilProdJobID == 0)
                                        {
                                            lCoilProdJobID = createStdProductJobAdvice(lCustomerCode, lProjectCode, addToCart.UpdateBy);

                                            if (lCoilProdJobID > 0)
                                            {
                                                lSQL =
                                                "UPDATE dbo.OESProjOrdersSE " +
                                                "SET CoilProdJobID = " + lCoilProdJobID + ", " +
                                                "UpdateDate = getDate(), " +
                                                "UpdateBy = '" + addToCart.UpdateBy + "' " +
                                                "WHERE OrderNumber = " + lOrderNumber.ToString() + " " +
                                                "AND StructureElement = '" + addToCart.pStructureElement + "' " +
                                                "AND ProductType = '" + addToCart.pProductType + "' " +
                                                "AND ScheduledProd = '" + addToCart.pScheduledProd.Trim() + "' ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lNDSCon;
                                                lCmd.CommandTimeout = 300;
                                                lCmd.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                lProcessObj.CloseNDSConnection(ref lNDSCon);
                                                lReturnOrderNo = -1;
                                                lErrorMsg = "Error on Standard Product job ID generation.";
                                            }
                                        }

                                    }
                                    else if (lProductType == "CARPET")
                                    {
                                    }
                                }
                            }
                            lProcessObj.CloseNDSConnection(ref lNDSCon);

                        }
                    }
                    catch (Exception ex)
                    {
                        lReturnOrderNo = -1;
                        lErrorMsg = ex.Message;
                    }
                }
                else
                {
                    lErrorMsg = "Invalid customer code or project code";
                }
            }
            else
            {
                lErrorMsg = "Invalid customer code or project code";
            }

            lCmd = null;
            lNDSCon = null;

            return Json(new { OrderNumber = lReturnOrderNo, Refnumber = lReturnRefNo });
        }


        [HttpGet]
        [Route("/GetReferenceNoByOrderNo/{OrderNumber}/{routeFlag}")]
        //[ValidateAntiForgeryHeader]
        public async Task<ActionResult> GetReferenceNoByOrderNo(string OrderNumber, bool routeFlag)
        {
            string lReturnRefNo = "";
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lReturn = (new[]{ new
            {
                CustomerCode = "",
                ProjectCode = "",
                    OrderNo = 0,
                    StructureElement="",
                    ProductType="",
                    ScheduledProd="",
                    WBS1="",
                    WBS2="",
                    WBS3="",
                      // TotalWeight=0,
                       TotalPCs=0,
                    PostHeaderId=0,


            }}).ToList();

            if (lReturn.Count > 0)
            {

                lReturn.RemoveAt(0);
            }
            if (routeFlag)

            {

                var refNumber = OrderReferenceNumber2(OrderNumber);

                if (refNumber == 0)
                {
                    lCmd.CommandText = "SELECT P.CustomerCode, P.ProjectCode, P.OrderNumber, " +
                    "S.StructureElement, S.ProductType, S.ScheduledProd, P.WBS1,  P.WBS2,  " +
                    "P.WBS3, S.TotalWeight, S.TotalPCs,  S.PostHeaderID FROM  OESProjOrder as P, " +
                    "OESProjOrdersSE as S WHERE P.OrderNumber = S.OrderNumber " +
                    "AND (P.OrderStatus = 'Created' OR P.OrderStatus = 'Created*') AND " +
                    "P.OrderNumber = '" + OrderNumber + "'";
                }
                else
                {
                    lCmd.CommandText = "SELECT P.CustomerCode, P.ProjectCode, P.OrderNumber, " +
                    "S.StructureElement, S.ProductType, S.ScheduledProd, P.WBS1,  P.WBS2,  " +
                    "P.WBS3, S.TotalWeight, S.TotalPCs,  S.PostHeaderID FROM  " +
                    "OESProjOrder as P, OESProjOrdersSE as S WHERE P.OrderNumber=S.OrderNumber " +
                    "AND (P.OrderStatus = 'Created' OR P.OrderStatus = 'Created*') AND  " +
                    "P.OrderReferenceNo=(SELECT OrderReferenceNo FROM " +
                    "OESProjOrder WHERE OrderNumber='" + OrderNumber + "')";
                }
            }
            else
            {
                lCmd.CommandText = "SELECT P.CustomerCode, P.ProjectCode, P.OrderNumber, " +
                    "S.StructureElement, S.ProductType,S.ScheduledProd, P.WBS1,  P.WBS2,  " +
                    "P.WBS3, S.TotalWeight, S.TotalPCs,  S.PostHeaderID FROM  OESProjOrder as P, " +
                    "OESProjOrdersSE as S WHERE P.OrderNumber = S.OrderNumber AND " +
                    "P.OrderNumber = '" + OrderNumber + "'";
            }

            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = await lCmd.ExecuteReaderAsync();
                if (lRst.HasRows)
                {

                    while (lRst.Read())
                    {

                        if (!lRst.IsDBNull(0)) // Check if the value is not null
                        {
                            //lReturnRefNo = lRst.GetString(0);
                            lReturn.Add(new
                            {
                                CustomerCode = (lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim()),
                                ProjectCode = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim()),
                                OrderNo = lRst.GetValue(2) == DBNull.Value ? 0 : lRst.GetInt32(2),
                                StructureElement = lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim(),
                                ProductType = lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetString(4).Trim(),
                                ScheduledProd = lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim(),
                                WBS1 = lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetString(6).Trim(),
                                WBS2 = lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetString(7).Trim(),
                                WBS3 = lRst.GetValue(8) == DBNull.Value ? "" : lRst.GetString(8).Trim(),
                                // TotalWeight= decimal.TryParse(lRst[9].ToString()),
                                //TotalPCs = lRst.GetInt32(10)== DBNull.Value ? "" ,
                                TotalPCs = lRst.GetValue(10) == DBNull.Value ? 0 : lRst.GetInt32(10),
                                PostHeaderId = lRst.GetValue(11) == DBNull.Value ? 0 : lRst.GetInt32(11)
                            });
                        }
                        else
                        {
                            // Handle the case where the value is null
                            // For example, assign a default value or log a message
                            lReturnRefNo = ""; // Example default value
                            //Console.WriteLine("The value is null.");
                        }
                    }
                    lRst.Close();

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }
                lProcessObj = null;

                lCmd = null;
                lNDSCon = null;
                lRst = null;

            }
            return Ok(lReturn);
        }


        [HttpGet]
        [Route("/OrderReferenceNumber2")]
        public int OrderReferenceNumber2(string pOrderNumber)
        {
            var lDa = new SqlDataAdapter();
            var lCmd = new SqlCommand();
            var lDs = new DataSet();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;

            var refNo = 0;

            var lProcess = new ProcessController();
            lProcess.OpenNDSConnection(ref lNDSCon);
            if (lNDSCon.State == ConnectionState.Open)
            {
                var lSQL = "select top 1 OrderReferenceNo from OESProjOrder where OrderNumber = '" + pOrderNumber + "'  order by OrderReferenceNo desc ";

                lCmd.CommandText = lSQL;
                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    if (lRst.Read())
                    {
                        if (!lRst.IsDBNull(0)) // Check if the value is not null
                        {
                            refNo = lRst.GetInt32(0);
                        }
                        else
                        {
                            // Handle the case where the value is null
                            // For example, assign a default value or log a message
                            refNo = 0; // Example default value
                            //Console.WriteLine("The value is null.");
                        }
                    }
                }
                lProcess.CloseNDSConnection(ref lNDSCon);
            }
            lDa = null;
            lCmd = null;
            lDs = null;
            lNDSCon = null;

            return refNo;
        }

        [HttpGet]
        [Route("/getAccessRight/{pCustomerCode}/{pProjectCode}/{UserName}")]
        public ActionResult getAccessRight(string pCustomerCode, string pProjectCode, string UserName)
        {
            var lSubmission = "No";
            var lEditable = "No";

            var lReturn = new
            {
                Submission = "No",
                Editable = "No"
            };

            try
            {
                if (pCustomerCode == null)
                {
                    pCustomerCode = "";
                }
                if (pProjectCode == null)
                {
                    pProjectCode = "";
                }
                pCustomerCode = pCustomerCode.Trim();
                pProjectCode = pProjectCode.Trim();
                if (pCustomerCode.Length > 0 && pProjectCode.Length > 0)
                {

                    string lUserType = "";
                    UserAccessController lUa = new UserAccessController();
                    lUserType = lUa.getUserType(UserName);
                    lUa = null;
                    if (lUserType == "CU" || lUserType == "CA" || lUserType == "CM")
                    {
                        var lAccess = db.UserAccess.Find(UserName, pCustomerCode, pProjectCode);
                        if (lAccess != null)
                        {
                            lSubmission = lAccess.OrderSubmission.Trim();
                            lEditable = lAccess.OrderCreation.Trim();
                        }
                    }
                    else
                    {
                        if (lUserType == "TE" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
                        {
                            lSubmission = "Yes";
                            lEditable = "Yes";
                        }
                    }

                    lReturn = new
                    {
                        Submission = lSubmission,
                        Editable = lEditable
                    };
                }
            }
            catch (Exception e)
            {
                var lErrMsg = e.Message;
            }

            return Json(lReturn);
        }


        [HttpGet]
        [Route("/checkOrderDocs_2/{OrderNumber}/{StructureElement}/{ProductType}/{ScheduledProd}")]
        public ActionResult checkOrderDocs_2(int OrderNumber, string StructureElement, string ProductType, string ScheduledProd)
        {
            StructureElement = StructureElement.Trim();
            ProductType = ProductType.Trim();
            ScheduledProd = ScheduledProd.Trim();
            var lReturn = (new[]{ new
            {
                OrderNumber = 0,
                StructureElement = "",
                ProductType = "",
                ScheduledProd = "",
                DrawingID = 0,
                DrawingNo = "",
                FileName = "",
                Revision = 0,
                Remarks = "",
                UpdatedDate = DateTime.Now,
                UpdatedBy = "",
                Source = "",
                Comments_User="",
                Approved_Status= false,
                Comment_Customer =""
            }}).ToList();
            if (lReturn.Count > 0)
            {
                lReturn.RemoveAt(0);
            }

            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            lCmd.CommandText =
            "SELECT DocID, " +
            "DocumentNo, " +
            "FileName, " +
            "RevID, " +
            "Remarks, " +
            "UpdatedDate, " +
            "UpdatedBy " +
            "FROM dbo.OESOrderDocs " +
            "WHERE OrderNumber = " + OrderNumber.ToString() + " " +
            "AND StructureElement = '" + StructureElement + "' " +
            "AND ProductType = '" + ProductType + "' " +
            "AND ScheduledProd = '" + ScheduledProd + "' " +
            "AND ItemID = 0 " +
            "ORDER BY DocID ";
            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        var lRecord = new
                        {
                            OrderNumber = OrderNumber,
                            StructureElement = StructureElement,
                            ProductType = ProductType,
                            ScheduledProd = ScheduledProd,
                            DrawingID = lRst.GetInt32(0),
                            DrawingNo = lRst.GetValue(1) == System.DBNull.Value ? "" : lRst.GetString(1).Trim(),
                            FileName = lRst.GetString(2),
                            Revision = lRst.GetInt32(3),
                            Remarks = lRst.GetValue(4) == System.DBNull.Value ? "" : lRst.GetString(4).Trim(),
                            UpdatedDate = lRst.GetDateTime(5),
                            UpdatedBy = lRst.GetString(6),
                            Source = "DB",
                            Comments_User = "",
                            Approved_Status = false,
                            Comment_Customer = ""
                        };
                        lReturn.Add(lRecord);
                    }
                }
                lRst.Close();
                lCmd.CommandText =
                "SELECT W.DrawingID, " +
                "W.DrawingNo, " +
                "W.FileName, " +
                "O.Revision, " +
                "W.Remarks, " +
                // "W.UpdatedDate, " +
                // "W.UpdatedBy " +
                "O.UpdatedDate," +
                "O.UpdatedBy, " +
                "W.Comments_User, " +
                "W.Approved_Status, " +
                "W.Comment_Customer " +
                "FROM dbo.OESDrawings_posting W, dbo.OESDrawingsOrder_posting O " +
                "WHERE W.DrawingID = O.DrawingID " +
                "AND O.BBSPostHeader = " + OrderNumber.ToString() + " " +
                "AND O.StructureElement = '" + StructureElement + "' " +
                "AND O.ProductType = '" + ProductType + "' " +
                "AND O.ScheduledProd = '" + ScheduledProd + "' " +
                "ORDER BY W.DrawingID ";
                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();

                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        var lRecord = new
                        {
                            OrderNumber = OrderNumber,
                            StructureElement = StructureElement,
                            ProductType = ProductType,
                            ScheduledProd = ScheduledProd,
                            DrawingID = lRst.GetInt32(0),
                            DrawingNo = lRst.GetValue(1) == System.DBNull.Value ? "" : lRst.GetString(1).Trim(),
                            FileName = lRst.GetString(2),
                            Revision = lRst.GetInt32(3),
                            Remarks = lRst.GetValue(4) == System.DBNull.Value ? "" : lRst.GetString(4).Trim(),
                            UpdatedDate = lRst.GetDateTime(5),
                            UpdatedBy = lRst.GetString(6),
                            Source = "SP",
                            Comments_User = lRst.GetValue(7) == System.DBNull.Value ? "" : lRst.GetString(7).Trim(),
                            Approved_Status = lRst.GetValue(8) == System.DBNull.Value ? false : (bool)lRst.GetValue(8),
                            Comment_Customer = lRst.GetValue(9) == System.DBNull.Value ? "" : lRst.GetString(9).Trim()
                        };
                        lReturn.Add(lRecord);
                    }
                }
                lRst.Close();
                lProcessObj.CloseNDSConnection(ref lNDSCon);
            }
            lProcessObj = null;

            lCmd = null;
            lNDSCon = null;
            lRst = null;
            return Ok(lReturn);
        }

        [HttpGet]
        [Route("/CheckTransportModeCABOrderSummary/{OrderNumber}")]
        public IActionResult CheckTransportModeCABOrderSummary(string OrderNumber)
        {
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            try
            {

                lCmd.CommandText = "SELECT TOP 1 " +
                    "pj.CustomerCode AS CustomerCode, " +
                    "pj.ProjectCode AS ProjectCode, " +
                    "pj.OrderNumber AS OrderNumber, " +
                    "ja.TransportLimit AS TransportLimit, " +
                    "pj.TransportMode AS TransportMode, " +
                    "oe.shapeTransport as ShapeTransport " +
                    "from OESProjOrder pj  " +
                    "INNER JOIN OESProjOrdersSE se ON pj.OrderNumber=se.OrderNumber " +
                    "INNER JOIN OESJobAdvice ja ON pj.CustomerCode = ja.CustomerCode AND pj.ProjectCode = ja.ProjectCode AND se.CABJobID = ja.JobID " +
                    "INNER JOIN OESOrderDetails oe " +
                    "ON pj.CustomerCode = oe.CustomerCode " +
                    "AND oe.ProjectCode=pj.ProjectCode " +
                    "AND se.CABJobID = oe.JobID " +
                    "AND (oe.Cancelled IS NULL OR oe.Cancelled != 1) " +
                    "And(oe.shapeTransport != 3 or oe.shapeTransport is null) " +
                    "WHERE pj.OrderNumber = '" + OrderNumber + "' order by oe.shapeTransport DESC";

                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon))
                {
                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        while (lRst.Read())
                        {
                            string transportLimit = lRst["TransportLimit"].ToString();
                            string transportMode = lRst["TransportMode"].ToString();
                            string ShapeTransport = lRst["ShapeTransport"] == DBNull.Value ? "" : lRst["ShapeTransport"].ToString();

                            bool areSame = false;

                            if (transportLimit == "Normal" && transportMode == "TR40/24" && (ShapeTransport == "" || ShapeTransport == "0" || ShapeTransport == "3"))
                            {
                                areSame = true;
                            }
                            else if (transportLimit == "Low Bed" && transportMode == "LB30" && ShapeTransport == "1")
                            {
                                areSame = true;
                            }
                            else if (transportLimit == "Police Escort" && transportMode == "LBE" && ShapeTransport == "2")
                            {
                                areSame = true;
                            }
                            lRst.Close();
                            lProcessObj.CloseNDSConnection(ref lNDSCon);
                            //return Ok(areSame); // Return true or false
                            string lTransport = "";
                            if (areSame == false)
                            {
                                if ((ShapeTransport == "" || ShapeTransport == "0" || ShapeTransport == "3"))
                                {
                                    lTransport = "TR40/24";
                                }
                                else if (ShapeTransport == "1")
                                {
                                    lTransport = "LB30";
                                }
                                else if (ShapeTransport == "2")
                                {
                                    lTransport = "LBE";
                                }

                            }
                            return Ok(new { result = areSame, transport = lTransport });
                        }
                    }
                    lRst.Close();
                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }
                //return Ok(false); // Default if no rows found
                return Ok(new { result = true, transport = " " }); // Default if no rows found
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }


        [HttpPost]
        [Route("/ResetOrderRefNo")]
        public IActionResult ResetOrderRefNo(List<int> orderNos)
        {
            var lNDSCon = new SqlConnection();
            var lProcess = new ProcessController();
            if (orderNos == null || orderNos.Count == 0)
                return BadRequest("No order numbers provided.");

            try
            {
                using (var lCmd = new SqlCommand())
                {

                    lProcess.OpenNDSConnection(ref lNDSCon); 
                    if (lNDSCon.State == ConnectionState.Open)
                    {
                        lCmd.Connection = lNDSCon;
                        var paramNames = orderNos.Select((o, i) => $"@p{i}").ToList();
                        lCmd.CommandText =
                            $"UPDATE OESPROJORDER SET OrderReferenceNo=NULL WHERE ORDERNUMBER IN ({string.Join(",", paramNames)})";

                        for (int i = 0; i < orderNos.Count; i++)
                        {
                            lCmd.Parameters.AddWithValue(paramNames[i], orderNos[i]);
                        }

                        lCmd.ExecuteNonQuery();

                        lProcess.CloseNDSConnection(ref lNDSCon);
                        lNDSCon = null;
                        return Ok(new { success = true });
                    }
                    else
                    {
                        return Ok(new { success = false });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log ex here (Serilog, NLog, ILogger, etc.)
                return Ok(new { success = false });

            }
            finally
            {
                if (lNDSCon != null)
                    lProcess.CloseNDSConnection(ref lNDSCon);
            }
        }

        //[HttpGet]
        //[Route("/GetGreenType/{CustomerCode}/{ProjectCode}")]
        //public ActionResult GetGreenType(string CustomerCode, string ProjectCode)
        //{
        //    var content = new List<string>();
        //    var lDa = new SqlDataAdapter();
        //    var lCmd = new SqlCommand();
        //    var lDs = new DataSet();
        //    var cnNDS = new SqlConnection();

        //    var LOW_CARBON_RATE = "";
        //    var lReturn = "";
        //    try
        //    {
        //        var lSQL = "SELECT CM.CONTRACT_NO, CM.LOW_CARBON_RATE FROM HMIContractMaster CM " +
        //            "INNER JOIN HMIContractProject CP ON CP.CONTRACT_NO = CM.CONTRACT_NO " +
        //            "WHERE CM.CUST_ID = '" + CustomerCode + "' AND CP.PROJECT_CODE = '" + ProjectCode + "' AND CON_END_DATE>GETDATE() ";

        //        var lProcessObj = new ProcessController();
        //        if (lProcessObj.OpenNDSConnection(ref cnNDS))
        //        {
        //            lCmd.CommandType = CommandType.Text;
        //            lCmd.CommandText = lSQL;
        //            lCmd.Connection = cnNDS;
        //            lDa.SelectCommand = lCmd;
        //            lDs = new DataSet();
        //            lDa.Fill(lDs);

        //            if (lDs.Tables[0].Rows.Count > 0)
        //            {
        //                bool hasZero = false;
        //                bool hasHundred = false;

        //                foreach (DataRow row in lDs.Tables[0].Rows)
        //                {
        //                    string rate = row["LOW_CARBON_RATE"].ToString().Trim();

        //                    if (rate == "0") hasZero = true;
        //                    else if (rate == "100") hasHundred = true;
        //                }

        //                if (hasZero && hasHundred)
        //                    lReturn = "50"; // Mixed
        //                else if (hasZero)
        //                    lReturn = "0";
        //                else if (hasHundred)
        //                    lReturn = "100";
        //                else
        //                    lReturn = "50"; // Default to mixed if neither found
        //            }

        //            lProcessObj.CloseNDSConnection(ref cnNDS);
        //        }
        //        lDa = null;
        //        lCmd = null;
        //        lDs = null;
        //        cnNDS = null;

        //        // Here the value of lReturn does not specify the actual carbon rate of a paticular contract,
        //        // instead it is a numerical representation of the combined result for all the contracts available against the given Customer & project.
        //        // IF VALUE = 0 --> NON-GREEN
        //        // IF VALUE = 100 --> GREEN
        //        // IF VALUE = 50 --> GREEN & NON-GREEN (MIX)

        //        return Ok(lReturn);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        [HttpPost]
        [Route("/GetGreenSteelFlag")]
        public IActionResult GetGreenSteelFlag(List<int> orderNos)
        {
            var lProcess = new ProcessController();
            var lNDSCon = new SqlConnection();

            if (orderNos == null || orderNos.Count == 0)
                return BadRequest("No order numbers provided.");

            try
            {
                using (var lCmd = new SqlCommand())
                {
                    bool lGreenSteel = false;

                    // Open database connection
                    lProcess.OpenNDSConnection(ref lNDSCon);
                    if (lNDSCon.State == ConnectionState.Open)
                    {
                        lCmd.Connection = lNDSCon;

                        // Build parameterized query
                        var paramNames = orderNos.Select((o, i) => $"@p{i}").ToList();
                        lCmd.CommandText =
                            $"SELECT GreenSteel FROM OESPROJORDER WHERE OrderNumber IN ({string.Join(",", paramNames)})";

                        for (int i = 0; i < orderNos.Count; i++)
                        {
                            lCmd.Parameters.AddWithValue(paramNames[i], orderNos[i]);
                        }

                        lCmd.CommandTimeout = 300;

                        using (var lRst = lCmd.ExecuteReader())
                        {
                            if (lRst.HasRows)
                            {
                                while (lRst.Read())
                                {
                                    lGreenSteel = lRst.IsDBNull(0) ? false : lRst.GetBoolean(0);
                                    if (lGreenSteel) break; // Stop early if any true found
                                }
                            }
                        }

                        lProcess.CloseNDSConnection(ref lNDSCon);
                        lNDSCon = null;
                        return Ok(new { GreenSteel = lGreenSteel });
                    }
                    return Ok(new { GreenSteel = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An unexpected error occurred while checking GreenSteel flag.");
                // TODO: Log the exception (Serilog, NLog, ILogger, etc.)
            }
            finally
            {
                if (lNDSCon != null)
                    lProcess.CloseNDSConnection(ref lNDSCon);
            }
        }

        [HttpGet]
        [Route("/UpdateGreenSteelFlag/{OrderNumber}/{GreenSteel}")]
        public IActionResult UpdateGreenSteelFlag(int OrderNumber, bool GreenSteel)
        {
            var lProcess = new ProcessController();
            var lNDSCon = new SqlConnection();

            try
            {
                using (var lCmd = new SqlCommand())
                {
                    var lUpdateValue = GreenSteel==true ? 1 : 0;
                    // Open database connection
                    lProcess.OpenNDSConnection(ref lNDSCon);
                    if (lNDSCon.State == ConnectionState.Open)
                    {
                        lCmd.Connection = lNDSCon;

                        // Build parameterized query
                        lCmd.CommandText = $"UPDATE OESPROJORDER SET GreenSteel = {lUpdateValue} WHERE OrderNumber = {OrderNumber} ";
                        lCmd.CommandTimeout = 300;
                        lCmd.ExecuteNonQuery();

                        lProcess.CloseNDSConnection(ref lNDSCon);
                        lNDSCon = null;
                        return Ok(new { success = true });
                    }
                    return Ok(new { success = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An unexpected error occurred while checking GreenSteel flag.");
                // TODO: Log the exception (Serilog, NLog, ILogger, etc.)
            }
            finally
            {
                if (lNDSCon != null)
                    lProcess.CloseNDSConnection(ref lNDSCon);
            }
        }


        [HttpGet]
        [Route("/IsGreenSteelFlagVal/{OrderNumber}")]
        public IActionResult IsGreenSteelFlagVal(int OrderNumber)
        {
            var lProcess = new ProcessController();
            var lNDSCon = new SqlConnection();

            try
            {
                using (var lCmd = new SqlCommand())
                {
                    bool lGreenSteel = false;

                    // Open database connection
                    lProcess.OpenNDSConnection(ref lNDSCon);
                    if (lNDSCon.State == ConnectionState.Open)
                    {
                        lCmd.Connection = lNDSCon;

                        // Build parameterized query
                      
                        lCmd.CommandText =
                            $"select GreenSteel from OESProjOrder where OrderNumber='"+ OrderNumber + "'";


                        using (var lRst = lCmd.ExecuteReader())
                        {
                            if (lRst.HasRows)
                            {
                                while (lRst.Read())
                                {
                                    lGreenSteel = lRst.IsDBNull(0) ? false : lRst.GetBoolean(0);
                                }
                            }
                        }

                        lProcess.CloseNDSConnection(ref lNDSCon);
                        lNDSCon = null;
                        return Ok(new { GreenSteel = lGreenSteel });
                    }
                    return Ok(new { GreenSteel = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An unexpected error occurred while checking GreenSteel flag.");
                // TODO: Log the exception (Serilog, NLog, ILogger, etc.)
            }
            finally
            {
                if (lNDSCon != null)
                    lProcess.CloseNDSConnection(ref lNDSCon);
            }
        }
    }
}
