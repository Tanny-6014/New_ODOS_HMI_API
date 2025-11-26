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
using OrderService.Repositories;
using Azure;
using OfficeOpenXml.Style;
using System;
using OfficeOpenXml;
using System.Net.Http.Headers;
using iText.StyledXmlParser.Node;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DraftOrdersController : Controller
    {
        private readonly IOrder _OrderRepository;
        private readonly IMapper _mapper;

        private DBContextModels db = new DBContextModels();


        public DraftOrdersController(IOrder orderService, IMapper mapper)
        {
            _OrderRepository = orderService;
            _mapper = mapper;

        }

        //List of raised PO
        [HttpPost]
        [Route("/getDraftOrdersController")]
        public async Task<ActionResult> getDraftOrders(DraftOrdersDto draftOrdersDto)
        {
            string CustomerCode = draftOrdersDto.CustomerCode;
            string ProjectCode = draftOrdersDto.ProjectCode;
            List<string> AddressCodes = draftOrdersDto.AddressCode;
            bool AllProjects = draftOrdersDto.AllProjects;
            bool AllAddress = draftOrdersDto.AllProjects;
            string UserName=draftOrdersDto.UserName;
            string lPODateFrom = "";
            string lPODateTo = "";
            string lRDateFrom = "";
            string lRDateTo = "";
            string lUserName = UserName;//"vishalw_ttl@natsteel.com.sg";

            UserAccessController lUa = new UserAccessController();
            //var lUserType = lUa.getUserType(User.Identity.GetUserName());
            var lUserType = lUa.getUserType(lUserName);
            var lGroupName = lUa.getGroupName(UserName);

            //ViewBag.UserType = lUserType;

            //string lUserName = User.Identity.GetUserName();
            
            lUa = null;


            var lSubmission = "No";
            var lEditable = "No";

            //get Access right;
            if (lUserType == "CU" || lUserType == "CA" || lUserType == "CM")
            {
                //var lAccess = db.UserAccess.Find(User.Identity.Name, CustomerCode, ProjectCode);
                var lAccess = db.UserAccess.Find(lUserName, CustomerCode, ProjectCode);
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

            //if (PODate == null) PODate = "";
            //if (PODate.Trim().Length == 0 || PODate.IndexOf("to") <= 0)
            //{
            //    lPODateFrom = "2000-01-01 00:00:00";
            //    lPODateTo = "2200-01-01 00:00:00";
            //}
            //else
            //{
            //    lPODateFrom = PODate.Substring(0, PODate.IndexOf("to")).Trim();
            //    lPODateTo = PODate.Substring(PODate.IndexOf("to") + 2).Trim();
            //}

            lPODateFrom = lPODateFrom + " 00:00:00";
            lPODateTo = lPODateTo + " 23:59:59";

            DateTime lDateV = new DateTime();
            if (DateTime.TryParse(lPODateFrom, out lDateV) != true)
            {
                lPODateFrom = "2000-01-01 00:00:00";
            }
            if (DateTime.TryParse(lPODateTo, out lDateV) != true)
            {
                lPODateTo = "2200-01-01 00:00:00";
            }

            //if (RDate == null) RDate = "";
            //if (RDate.Trim().Length == 0 || RDate.IndexOf("to") <= 0)
            //{
            //    lRDateFrom = "2000-01-01 00:00:00";
            //    lRDateTo = "2200-01-01 23:59:59";
            //}
            //else
            //{
            //    lRDateFrom = RDate.Substring(0, RDate.IndexOf("to")).Trim();
            //    lRDateTo = RDate.Substring(RDate.IndexOf("to") + 2).Trim();
            //}

            lRDateFrom = lRDateFrom + " 00:00:00";
            lRDateTo = lRDateTo + " 00:00:00";

            lDateV = new DateTime();
            if (DateTime.TryParse(lRDateFrom, out lDateV) != true)
            {
                lRDateFrom = "2000-01-01 00:00:00";
            }
            if (DateTime.TryParse(lRDateTo, out lDateV) != true)
            {
                lRDateTo = "2200-01-01 23:59:59";
            }

            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lReturn = (new[]{ new
            {
                SSNNo = 0,
                OrderNo = 0,
                WBS1 = "",
                WBS2 = "",
                WBS3 = "",
                StructureElement = "",
                ProdType = "",
                PONo = "",
                UpdateDate = "",
                RequiredDate = "",
                OrderWeight = "",
                UpdateBy = "",
                OrderShared = false,
                ScheduledProd = "N",
                BBSNo = "",
                BBSDesc = "",
                CustomerCode = "",
                ProjectCode = "",
                Address = "",       // <-- added
                Gate = "",          // <-- added
                ProjectTitle = "",
                lUserType="",
                lSubmission="",
                lEditable=""

            }}).ToList();

            if (lReturn.Count > 0)
            {
                lReturn.RemoveAt(0);
            }

            if (CustomerCode != null && ProjectCode != null)
            {
                if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
                {
                    var lProjectState = "";
                    var lAddressState = "";
                    if (AllProjects == true)
                    {
                        SharedAPIController lBackEnd = new SharedAPIController();

                        var lProjects = lBackEnd.getProject(CustomerCode, lUserType, lGroupName);
                        lBackEnd = null;

                        if (lProjects != null && lProjects.Count > 0)
                        {
                            for (int i = 0; i < lProjects.Count; i++)
                            {
                                if (lProjects[i] != null && lProjects[i].ProjectCode != null && lProjects[i].ProjectCode.Trim().Length > 0)
                                {
                                    if (lProjectState == "")
                                    {
                                        lProjectState = "M.ProjectCode = '" + lProjects[i].ProjectCode + "' ";
                                    }
                                    else
                                    {
                                        lProjectState = lProjectState + " OR M.ProjectCode = '" + lProjects[i].ProjectCode + "' ";
                                    }
                                }
                            }
                        }

                        if (lProjectState == "")
                        {
                            lProjectState = "AND 1 = 2 ";
                        }
                        else
                        {
                            lProjectState = "AND (" + lProjectState + ") ";
                        }

                    }
                    else
                    {
                        lProjectState = "AND M.ProjectCode = '" + ProjectCode + "' ";
                    }

                    //if (AddressCodes.Count() != 0)
                    //{
                    //    for (int k = 0 ; k < AddressCodes.Count(); k++)
                    //    {
                    //        if (AddressCodes[k] != "")
                    //        {
                    //            if (lAddressState == "")
                    //            {
                    //                lAddressState = "AND M.AddressCode IN ('" + AddressCodes[k] + "' ";
                    //            }
                    //            else
                    //            {
                    //                lAddressState = lAddressState + ", '" + AddressCodes[k] + "' ";
                    //            }
                    //        }
                    //    }
                    //    if (lAddressState != "")
                    //    {
                    //        lAddressState = lAddressState + " ) ";
                    //    }
                    //}

                    if (AddressCodes != null && AddressCodes.Any())
                    {
                        var validCodes = AddressCodes
                            .Where(code => !string.IsNullOrWhiteSpace(code))
                            .Distinct() // optional: remove duplicates
                            .ToList();

                        if (validCodes.Any())
                        {
                            lAddressState = "AND M.AddressCode IN ('" + string.Join("', '", validCodes) + "')";
                        }
                    }



                    //lCmd.CommandText =
                    //    "SELECT " +
                    //    "M.OrderNumber, " +
                    //    "M.WBS1, " +
                    //    "M.WBS2, " +
                    //    "M.WBS3, " +
                    //    "(STUFF( " +
                    //    "(SELECT ',' + left(StructureElement + ProductType, len(StructureElement)) " +
                    //    "FROM dbo.OESProjOrdersSE " +
                    //    "WHERE OrderNumber = M.OrderNumber " +
                    //    "GROUP BY StructureElement, ProductType " +
                    //    "FOR XML PATH('')), 1, 1, ''))   " +
                    //    "AS StructureElement, " +
                    //    "(STUFF( " +
                    //    "(SELECT ',' + left(ProductType + StructureElement, len(ProductType)) " +
                    //    "FROM dbo.OESProjOrdersSE " +
                    //    "WHERE OrderNumber = M.OrderNumber " +
                    //    "GROUP BY ProductType, StructureElement " +
                    //    "FOR XML PATH('')), 1, 1, ''))   " +
                    //    "AS ProductType, " +
                    //    "(STUFF( " +
                    //    "(SELECT ',' + PONumber " +
                    //    "FROM dbo.OESProjOrdersSE " +
                    //    "WHERE OrderNumber = M.OrderNumber " +
                    //    "GROUP BY PONumber " +
                    //    "FOR XML PATH('')), 1, 1, ''))   " +
                    //    "AS PONumber, " +
                    //    "M.UpdateDate,  " +
                    //    "(STUFF( " +
                    //    "(SELECT ',' + left(convert(varchar(10), RequiredDate, 120) + ProductType + StructureElement, 10) " +
                    //    "FROM dbo.OESProjOrdersSE " +
                    //    "WHERE OrderNumber = M.OrderNumber " +
                    //    "GROUP BY convert(varchar(10), RequiredDate, 120), ProductType, StructureElement " +
                    //    "FOR XML PATH('')), 1, 1, ''))   " +
                    //    "AS RequiredDate, " +
                    //    "M.TotalWeight, " +
                    //    "M.UpdateBy, " +
                    //    "M.OrderShared, " +
                    //    "MIN(S.ScheduledProd), " +
                    //    "(STUFF( " +
                    //    "(SELECT ',' + rtrim(ltrim(BBSNo)) " +
                    //    "FROM dbo.OESBBS " +
                    //    "WHERE CustomerCode = M.CustomerCode " +
                    //    "AND ProjectCode = M.ProjectCode AND JobID > 0 " +
                    //    "AND JobID in (select CABJobID from dbo.OESProjOrdersSE " +
                    //    "WHERE OrderNumber = S.OrderNumber group by CABJobID) " +
                    //    "GROUP BY BBSNo " +
                    //    "FOR XML PATH('')), 1, 1, ''))   " +
                    //    "AS BBSNo, " +
                    //    "(STUFF( " +
                    //    "(SELECT ',' + rtrim(ltrim(BBSDesc)) " +
                    //    "FROM dbo.OESBBS " +
                    //    "WHERE CustomerCode = M.CustomerCode " +
                    //    "AND ProjectCode = M.ProjectCode AND JobID > 0 " +
                    //    "AND JobID in (select CABJobID from dbo.OESProjOrdersSE " +
                    //    "WHERE OrderNumber = S.OrderNumber group by CABJobID) " +
                    //    "GROUP BY BBSDesc " +
                    //    "FOR XML PATH('')), 1, 1, ''))   " +
                    //    "AS BBSDesc, " +
                    //    "M.CustomerCode, " +
                    //    "M.ProjectCode, " +
                    //    "(SELECT ProjectTitle FROM dbo.OESProject " +
                    //    "WHERE CustomerCode = M.CustomerCode " +
                    //    "AND ProjectCode = M.ProjectCode) as ProjectTitle " +
                    //    "FROM dbo.OESProjOrder M, dbo.OESProjOrdersSE S " +
                    //    "WHERE M.OrderNumber = S.OrderNumber " +
                    //    "AND M.CustomerCode = '" + CustomerCode + "' " +
                    //    "" + lProjectState + " " +
                    //    "AND (M.OrderStatus is NULL " +
                    //    "OR M.OrderStatus = 'New' " +
                    //    "OR M.OrderStatus = 'Created' " +
                    //    "OR M.OrderStatus = 'Reserved') " +
                    //    //  "AND M.UpdateDate >'2024-03-08 00:00:00.000' " + 
                    //    //"AND ((S.UpdateDate >= '" + lPODateFrom + "' " +
                    //    //"AND DATEADD(d,-1,S.UpdateDate) < '" + lPODateTo + "') " +
                    //    //"OR (S.UpdateDate is null AND '" + lPODateTo + "' = '2200-01-01 00:00:00' )) " +
                    //    //"AND ((S.RequiredDate >= '" + lRDateFrom + "' " +
                    //    //"AND DATEADD(d,-1,S.RequiredDate) < '" + lRDateTo + "') " +
                    //    //"OR (S.RequiredDate is null AND '" + lRDateTo + "' = '2200-01-01 23:59:59' )) " +
                    //    //"AND (S.PONumber like '%" + PONumber + "%' " +
                    //    //"OR  (S.PONumber is null AND '" + PONumber + "' = '' )) " +
                    //    //"AND (M.WBS1 like '%" + WBS1 + "%' " +
                    //    //"OR (M.WBS1 is null AND '" + WBS1 + "' = '' )) " +
                    //    //"AND (M.WBS2 like '%" + WBS2 + "%' " +
                    //    //"OR (M.WBS2 is null AND '" + WBS2 + "' = '' )) " +
                    //    //"AND (M.WBS3 like '%" + WBS3 + "%' " +
                    //    //"OR (M.WBS3 is null AND '" + WBS3 + "' = '' )) " +
                    //    "AND (M.UpdateBy = '" + UserName + "' " +
                    //    "OR M.OrderShared = 1 ) " +
                    //    ////"AND M.UpdateDate >= '2019-07-01' " +
                    //    "GROUP BY " +
                    //    "M.OrderNumber, " +
                    //    "S.OrderNumber, " +
                    //    "M.WBS1, " +
                    //    "M.WBS2, " +
                    //    "M.WBS3, " +
                    //    "M.UpdateDate, " +
                    //    "M.TotalWeight, " +
                    //    "M.UpdateBy, " +
                    //    "M.OrderShared, " +
                    //    "M.CustomerCode, " +
                    //    "M.ProjectCode " +
                    //    "ORDER BY " +
                    //    "M.OrderNumber DESC ";


                    lCmd.CommandText =
                        "SELECT " +
                        "M.OrderNumber, " +
                        "M.WBS1, " +
                        "M.WBS2, " +
                        "M.WBS3, " +
                        "(STUFF( " +
                        "(SELECT ',' + left(StructureElement + ProductType, len(StructureElement)) " +
                        "FROM dbo.OESProjOrdersSE " +
                        "WHERE OrderNumber = M.OrderNumber " +
                        "GROUP BY StructureElement, ProductType " +
                        "FOR XML PATH('')), 1, 1, '')) AS StructureElement, " +
                        "(STUFF( " +
                        "(SELECT ',' + left(ProductType + StructureElement, len(ProductType)) " +
                        "FROM dbo.OESProjOrdersSE " +
                        "WHERE OrderNumber = M.OrderNumber " +
                        "GROUP BY ProductType, StructureElement " +
                        "FOR XML PATH('')), 1, 1, '')) AS ProductType, " +
                        "(STUFF( " +
                        "(SELECT ',' + PONumber " +
                        "FROM dbo.OESProjOrdersSE " +
                        "WHERE OrderNumber = M.OrderNumber " +
                        "GROUP BY PONumber " +
                        "FOR XML PATH('')), 1, 1, '')) AS PONumber, " +
                        "M.UpdateDate,  " +
                        "(STUFF( " +
                        "(SELECT ',' + left(convert(varchar(10), RequiredDate, 120) + ProductType + StructureElement, 10) " +
                        "FROM dbo.OESProjOrdersSE " +
                        "WHERE OrderNumber = M.OrderNumber " +
                        "GROUP BY convert(varchar(10), RequiredDate, 120), ProductType, StructureElement " +
                        "FOR XML PATH('')), 1, 1, '')) AS RequiredDate, " +
                        "M.TotalWeight, " +
                        "M.UpdateBy, " +
                        "M.OrderShared, " +
                        "MIN(S.ScheduledProd) AS MinScheduledProd, " +
                        "(STUFF( " +
                        "(SELECT ',' + rtrim(ltrim(BBSNo)) " +
                        "FROM dbo.OESBBS " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode AND JobID > 0 " +
                        "AND JobID in (select CABJobID from dbo.OESProjOrdersSE " +
                        "WHERE OrderNumber = S.OrderNumber group by CABJobID) " +
                        "GROUP BY BBSNo " +
                        "FOR XML PATH('')), 1, 1, '')) AS BBSNo, " +
                        "(STUFF( " +
                        "(SELECT ',' + rtrim(ltrim(BBSDesc)) " +
                        "FROM dbo.OESBBS " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode AND JobID > 0 " +
                        "AND JobID in (select CABJobID from dbo.OESProjOrdersSE " +
                        "WHERE OrderNumber = S.OrderNumber group by CABJobID) " +
                        "GROUP BY BBSDesc " +
                        "FOR XML PATH('')), 1, 1, '')) AS BBSDesc, " +
                        "M.CustomerCode, " +
                        "M.ProjectCode, " +
                        "M.Address, " +               // <-- added
                        "M.Gate, " +                  // <-- added
                        "(SELECT ProjectTitle FROM dbo.OESProject " +
                        " WHERE CustomerCode = M.CustomerCode " +
                        " AND ProjectCode = M.ProjectCode) AS ProjectTitle " +
                        "FROM dbo.OESProjOrder M, dbo.OESProjOrdersSE S " +
                        "WHERE M.OrderNumber = S.OrderNumber " +
                        "AND M.CustomerCode = '" + CustomerCode + "' " +
                        "" + lProjectState + " " +
                        "" + lAddressState + " " +
                        "AND (M.OrderStatus is NULL " +
                        " OR M.OrderStatus = 'New' " +
                        " OR M.OrderStatus = 'Created' " +
                        " OR M.OrderStatus = 'Reserved') " +
                        "AND (M.UpdateBy = '" + UserName + "' " +
                        " OR M.OrderShared = 1 ) " +
                        "GROUP BY " +
                        "M.OrderNumber, " +
                        "S.OrderNumber, " +
                        "M.WBS1, " +
                        "M.WBS2, " +
                        "M.WBS3, " +
                        "M.UpdateDate, " +
                        "M.TotalWeight, " +
                        "M.UpdateBy, " +
                        "M.OrderShared, " +
                        "M.CustomerCode, " +
                        "M.ProjectCode, " +
                        "M.Address, " +           // <-- included in GROUP BY
                        "M.Gate " +               // <-- included in GROUP BY
                        "ORDER BY " +
                        "M.OrderNumber DESC ";


                    var lProcessObj = new ProcessController();
                    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                    {
                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = await lCmd.ExecuteReaderAsync();
                        if (lRst.HasRows)
                        {
                            var lSNo = 0;
                            while (lRst.Read())
                            {
                                lSNo = lSNo + 1;
                                int lOrderNo = lRst.GetInt32(0);
                                decimal lOrderWT = lRst.GetValue(9) == DBNull.Value ? 0 : (decimal)(lRst.GetDecimal(9) / 1000);
                                if (lOrderWT == 0)
                                {
                                   lOrderWT = UpdateOrderWT(CustomerCode, ProjectCode, lOrderNo) / 1000; //..by CSS
                                }

                                if (lOrderWT > 0 && lOrderWT < (decimal)0.001)
                                {
                                    lOrderWT = (decimal)0.001;
                                }

                                lReturn.Add(new
                                {
                                    SSNNo = lSNo,
                                    OrderNo = lRst.GetInt32(0),
                                    WBS1 = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim()),
                                    WBS2 = (lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim()),
                                    WBS3 = (lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim()),
                                    StructureElement = (lRst.GetValue(4) == DBNull.Value ? "" : (lRst.GetString(4).Trim() == "NONWBS" ? "" : lRst.GetString(4).Trim())),
                                    ProdType = (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim()),
                                    PONo = (lRst.GetValue(6) == DBNull.Value ? "" : (lRst.GetString(6).Trim().Length > 0 && lRst.GetString(6).Trim().Substring(0, 1) == "," ? lRst.GetString(6).Trim().Substring(1) : lRst.GetString(6).Trim())),
                                    UpdateDate = (lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetDateTime(7).ToString("yyyy-MM-dd")),
                                    RequiredDate = (lRst.GetValue(8) == DBNull.Value ? "" : (lRst.GetString(8).Trim().Length > 0 && lRst.GetString(8).Substring(0, 1) == "," ? lRst.GetString(8).Substring(1) : lRst.GetString(8))),
                                    OrderWeight = lOrderWT.ToString("###,###,##0.000;(###,##0.000);"),
                                    UpdateBy = (lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim()),
                                    OrderShared = (lRst.GetValue(11) == DBNull.Value ? false : (bool)lRst.GetBoolean(11)),
                                    ScheduledProd = (lRst.GetValue(12) == DBNull.Value ? "N" : lRst.GetString(12).Trim()),
                                    BBSNo = (lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetString(13).Trim()),
                                    BBSDesc = WebUtility.HtmlDecode(lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetString(14).Trim()),
                                    CustomerCode = (lRst.GetValue(15) == DBNull.Value ? "" : lRst.GetString(15).Trim()),
                                    ProjectCode = (lRst.GetValue(16) == DBNull.Value ? "" : lRst.GetString(16).Trim()),
                                    Address = (lRst.GetValue(17) == DBNull.Value ? "" : lRst.GetString(17).Trim()),   // <-- new index 17
                                    Gate = (lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetString(18).Trim()),      // <-- new index 18
                                    ProjectTitle = (lRst.GetValue(19) == DBNull.Value ? "" : lRst.GetString(19).Trim()), // <-- new index 19
                                    lUserType = lUserType.ToString(),
                                    lSubmission = lSubmission.ToString(),
                                    lEditable = lEditable.ToString()
                                });
                            }
                        }
                        lRst.Close();

                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                    }
                    lProcessObj = null;
                }
            }
            lCmd = null;
            lNDSCon = null;
            lRst = null;



            return Ok(lReturn);
        }

        decimal UpdateOrderWT(string pCustomerCode, string pProjectCode, int pOrderNo)
        {
            decimal lTotalWT = 0;

            var lOrderSE = (from p in db.OrderProjectSE
                            where p.OrderNumber == pOrderNo
                            select p).ToList();

            for (int i = 0; i < lOrderSE.Count; i++)
            {
                if (lOrderSE[i].CABJobID > 0)
                {
                    int lJobID = lOrderSE[i].CABJobID;
                    var lProdWT = (from p in db.OrderDetails
                                   where p.CustomerCode == pCustomerCode &&
                                   p.ProjectCode == pProjectCode &&
                                   p.JobID == lJobID &&
                                   p.Cancelled == null
                                   select p.BarWeight)
                               .DefaultIfEmpty(0)
                               .Sum();

                    if (lProdWT == null) lProdWT = 0;

                    //save WT back to SE
                    if (lOrderSE[i].TotalWeight != lProdWT)
                    {
                        var lNewCart = lOrderSE[i];
                        lNewCart.TotalWeight = lProdWT;
                        db.Entry(lOrderSE[i]).CurrentValues.SetValues(lNewCart);
                    }
                    lTotalWT = lTotalWT + (decimal)lProdWT;
                }
                else if (lOrderSE[i].MESHJobID > 0)
                {
                    int lJobID = lOrderSE[i].MESHJobID;
                    var lProdWT = (from p in db.CTSMESHOthersDetails
                                   where p.CustomerCode == pCustomerCode &&
                                   p.ProjectCode == pProjectCode &&
                                   p.JobID == lJobID
                                   select p.MeshTotalWT)
                               .DefaultIfEmpty(0)
                               .Sum();

                    if (lProdWT == null) lProdWT = 0;

                    //save WT back to SE
                    if (lOrderSE[i].TotalWeight != lProdWT)
                    {
                        var lNewCart = lOrderSE[i];
                        lNewCart.TotalWeight = lProdWT;
                        db.Entry(lOrderSE[i]).CurrentValues.SetValues(lNewCart);
                    }
                    lTotalWT = lTotalWT + (decimal)lProdWT;

                }
                else if (lOrderSE[i].BPCJobID > 0)
                {
                    int lJobID = lOrderSE[i].BPCJobID;
                    var lProdWT = (from p in db.BPCJobAdvice
                                   where p.CustomerCode == pCustomerCode &&
                                   p.ProjectCode == pProjectCode &&
                                   p.JobID == lJobID
                                   select p.TotalWeight)
                               .DefaultIfEmpty(0)
                               .Sum();

                    //save WT back to SE
                    if (lOrderSE[i].TotalWeight != lProdWT)
                    {
                        var lNewCart = lOrderSE[i];
                        lNewCart.TotalWeight = lProdWT;
                        db.Entry(lOrderSE[i]).CurrentValues.SetValues(lNewCart);
                    }
                    lTotalWT = lTotalWT + (decimal)lProdWT;
                }
                else if (lOrderSE[i].CageJobID > 0)
                {
                    int lJobID = lOrderSE[i].CageJobID;
                    var lProdWT = (from p in db.PRCJobAdvice
                                   where p.CustomerCode == pCustomerCode &&
                                   p.ProjectCode == pProjectCode &&
                                   p.JobID == lJobID
                                   select p.TotalWeight)
                               .DefaultIfEmpty(0)
                               .Sum();

                    //save WT back to SE
                    if (lOrderSE[i].TotalWeight != lProdWT)
                    {
                        var lNewCart = lOrderSE[i];
                        lNewCart.TotalWeight = lProdWT;
                        db.Entry(lOrderSE[i]).CurrentValues.SetValues(lNewCart);
                    }
                    lTotalWT = lTotalWT + (decimal)lProdWT;
                }
                else if (lOrderSE[i].CoilProdJobID > 0)
                {
                    int lJobID = lOrderSE[i].CoilProdJobID;
                    var lProdWT = (from p in db.StdProdDetails
                                   where p.CustomerCode == pCustomerCode &&
                                   p.ProjectCode == pProjectCode &&
                                   p.JobID == lJobID &&
                                   p.ProdType != "BAR12" &&
                                   p.ProdType != "BAR14" &&
                                   p.ProdType != "BAR15" &&
                                   p.ProdType != "500M" &&
                                   p.ProdType != "BAR6"
                                   select p.order_wt)
                               .DefaultIfEmpty(0)
                               .Sum();

                    //save WT back to SE
                    if (lOrderSE[i].TotalWeight != lProdWT)
                    {
                        var lNewCart = lOrderSE[i];
                        lNewCart.TotalWeight = lProdWT;
                        db.Entry(lOrderSE[i]).CurrentValues.SetValues(lNewCart);
                    }
                    lTotalWT = lTotalWT + (decimal)lProdWT;
                }
                else if (lOrderSE[i].StdBarsJobID > 0)
                {
                    int lJobID = lOrderSE[i].StdBarsJobID;
                    var lProdWT = (from p in db.StdProdDetails
                                   where p.CustomerCode == pCustomerCode &&
                                   p.ProjectCode == pProjectCode &&
                                   p.JobID == lJobID &&
                                   (p.ProdType == "BAR12" ||
                                   p.ProdType == "BAR14" ||
                                   p.ProdType == "BAR15" ||
                                   p.ProdType == "500M" ||
                                   p.ProdType == "BAR6")
                                   select p.order_wt)
                               .DefaultIfEmpty(0)
                               .Sum();

                    //save WT back to SE
                    if (lOrderSE[i].TotalWeight != lProdWT)
                    {
                        var lNewCart = lOrderSE[i];
                        lNewCart.TotalWeight = lProdWT;
                        db.Entry(lOrderSE[i]).CurrentValues.SetValues(lNewCart);
                    }
                    lTotalWT = lTotalWT + (decimal)lProdWT;
                }
                else if (lOrderSE[i].StdMESHJobID > 0)
                {
                    int lJobID = lOrderSE[i].StdMESHJobID;

                    var lProdWT = (from p in db.StdSheetDetails
                                   where p.CustomerCode == pCustomerCode &&
                                   p.ProjectCode == pProjectCode &&
                                   p.JobID == lJobID
                                   select p.order_wt)
                               .DefaultIfEmpty(0)
                               .Sum();

                    //save WT back to SE
                    if (lOrderSE[i].TotalWeight != lProdWT)
                    {
                        var lNewCart = lOrderSE[i];
                        lNewCart.TotalWeight = lProdWT;
                        db.Entry(lOrderSE[i]).CurrentValues.SetValues(lNewCart);
                    }
                    lTotalWT = lTotalWT + (decimal)lProdWT;
                }
            }

            if (lTotalWT > 0)
            {
                var lUXHeader = db.OrderProject.Find(pOrderNo);
                if (lUXHeader != null && lUXHeader.OrderNumber > 0)
                {
                    if (lUXHeader.TotalWeight != lTotalWT)
                    {
                        var lNewHd = lUXHeader;
                        lNewHd.TotalWeight = lTotalWT;
                        db.Entry(lUXHeader).CurrentValues.SetValues(lNewHd);
                    }
                }
                db.SaveChanges();
            }
            return lTotalWT;

        }

        [HttpGet]
        [Route("/getDraftOrdersController/{CustomerCode}/{ProjectCode}/{orderNumber}/{pOrderStatus}/{UserName}")]
        public ActionResult BatchChangeStatus(string CustomerCode, string ProjectCode, string orderNumber, string pOrderStatus,string UserName)
        {
            //OrderStatus 1. Submitted, 2. Sent 
            //Process Shared or Exclusive request

            string[] pCustomerCode = CustomerCode.Split(',');
            string[] OrderNo = orderNumber.Split(',');
            string[] pProjectCode = ProjectCode.Split(',');
            int[] pOrderNo;
            pOrderNo = Array.ConvertAll<string, int>(OrderNo, int.Parse);

            
            try
            {
                if (pOrderStatus == "Shared" || pOrderStatus == "Exclusive")
                {
                    if (pOrderNo.Length > 0)
                    {
                        bool lShared = true;
                        if (pOrderStatus == "Exclusive")
                        {
                            lShared = false;
                        }
                        for (int i = 0; i < pOrderNo.Length; i++)
                        {
                            var lHeader = db.OrderProject.Find(pOrderNo[i]);
                            var lNewHeader = lHeader;
                            lNewHeader.OrderShared = lShared;
                            lNewHeader.UpdateDate = DateTime.Now;
                            lNewHeader.UpdateBy = UserName;//User.Identity.GetUserName();
                            db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);
                            db.SaveChanges();

                            if (pOrderStatus == "Shared")
                            {
                                //var lUserID = User.Identity.GetUserName();
                                //if (lUserID.Split('@')[1].ToLower() != "natsteel.com.sg" &&
                                //lUserID.Split('@')[1].ToLower() != "easteel-services.com")
                                {
                                    //var lEmailObj = new SendGridEmail();
                                    //lEmailObj.sendOrderActionEmail(lHeader.CustomerCode, lHeader.ProjectCode, lHeader.OrderNumber, pOrderStatus, lHeader.OrderStatus, lUserID, 1, "", "", "");
                                    //lEmailObj = null;

                                }

                            }
                        }
                    }
                }
                else
                {
                    if (pOrderStatus == "Submitted" || pOrderStatus == "Sent")
                    {
                        if (pOrderNo.Length > 0)
                        {
                            for (int i = 0; i < pOrderNo.Length; i++)
                            {
                                int lOrderNo = pOrderNo[i];
                                var lSE = (from p in db.OrderProjectSE
                                           where p.OrderNumber == lOrderNo
                                           select p).ToList();
                                if (lSE != null && lSE.Count > 0)
                                {
                                    for (int j = 0; j < lSE.Count; j++)
                                    {
                                        if (lSE[j].ProductType != "BPC" && lSE[j].ScheduledProd == "N" && lSE[j].TotalWeight == 0)
                                        {
                                            //return Json(new
                                            //{
                                            //    success = false,
                                            //    responseText = "Incompleted order has been found (Ref No:" + pOrderNo + "). Please enter the order detail before submit it. \n\n发现未完成的订单. 请输入订单的数据再提交."
                                            //}, System.Web.Mvc.JsonRequestBehavior.AllowGet);
                                            string Message = "Error during orders processing: " + "Incompleted order has been found (Ref No:" + pOrderNo + "). Please enter the order detail before submit it. \n\n发现未完成的订单. 请输入订单的数据再提交.";
                                            return Ok(new { Message, response = "failure" });

                                            
                                        }
                                    }
                                }
                            }
                        }

                        if (pOrderNo.Length > 0)
                        {
                            for (int i = 0; i < pOrderNo.Length; i++)
                            {

                                int lCAB = 0;
                                int lOrderNo = pOrderNo[i];
                                var lSE = (from p in db.OrderProjectSE
                                           where p.OrderNumber == lOrderNo
                                           select p).ToList();
                                if (lSE != null && lSE.Count > 0)
                                {
                                    for (int j = 0; j < lSE.Count; j++)
                                    {
                                        if (lSE[j].ProductType == "CAB")
                                        {
                                            lCAB = 1;
                                            break;
                                        }
                                    }
                                }
                                if (lCAB == 1)
                                {
                                    var lSharedAPI = new SharedAPIController();
                                    string lCheckResult = lSharedAPI.checkCABDetails(lOrderNo, UserName);
                                    if (lCheckResult != "")
                                    {
                                        //return Json(new
                                        //{
                                        //    success = false,
                                        //    responseText = "Please settle Cut & Bend Order (Ref No:" + pOrderNo[i] + ") issue before submit the order: \n\n" + lCheckResult
                                        //}, System.Web.Mvc.JsonRequestBehavior.AllowGet);
                                        string Message = "Error during orders processing: " + "Please settle Cut & Bend Order (Ref No:" + pOrderNo[i] + ") issue before submit the order: \n\n" + lCheckResult;
                                        return Ok(new { Message, response = "failure" });

                                    }
                                }
                            }
                        }
                    }

                    OrderProcessAPIController lOrderProc = new OrderProcessAPIController();

                    if (pOrderNo.Length > 0)
                    {
                        for (int i = 0; i < pOrderNo.Length; i++)
                        {
                            var lReturnMsg = lOrderProc.StatusProcess(pCustomerCode[i], pProjectCode[i], pOrderNo[i], pOrderStatus, UserName);

                            if (lReturnMsg != "")
                            {
                                //return Json(new
                                //{
                                //    success = false,
                                //    responseText = lReturnMsg
                                //}, System.Web.Mvc.JsonRequestBehavior.AllowGet);

                                string Message = "Error during orders processing: " + lReturnMsg;
                                return Ok(new { Message, response = "failure" });
                                

                            }
                        }
                    }
                    lOrderProc = null;
                }
            }
            catch (Exception ex)
            {
                //return Json(new
                //{
                //    success = false,
                //    responseText = "Error:" + ex.Message
                //}, System.Web.Mvc.JsonRequestBehavior.AllowGet);
                return Ok("Error:" + ex.Message);
            }

            //if (!response)
            //{
            //    //lReturn = -1;
            //    alert("Error during orders processing: " + response);
            //}
            //else
            //{

                if (pOrderStatus == "Delete")
                {
                string Message = "The selected orders have been deleted successfully.";
                return Ok(new { Message, response = "success" });
            }
                if (pOrderStatus == "Sent")
                {
                string Message = "The selected orders have been submitted for approval successfully.";
                return Ok(new { Message, response = "success" });
            }
                if (pOrderStatus == "Submitted")
                {
                string Message = "The selected orders have been submitted to NatSteel successfully.";
                return Ok(new { Message, response = "success" });
            }
                if (pOrderStatus == "Shared")
                {
                string Message = "selected orders have been shared to other users in the project";
                return Ok(new { Message, response = "success" });
                }

                if (pOrderStatus == "Exclusive")
                {
                string Message = "The selected orders have been hold by you exclusively.";
                return Ok(new { Message, response = "success" });
            }
            //}
            
            return Ok(true);
            //return Json(new { success = true }, System.Web.Mvc.JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [Route("/exportDraftOrdersToExcel")]
        public async Task<ActionResult> exportDraftOrdersToExcel()
        {
            //set kookie for customer and project
            string CustomerCode = Request.Form["CustomerCode"];
            string temp = Request.Form["ProjectCodes"];
            string tempAddress = Request.Form["AddressCodes"];
            string lUserName = Request.Form["UserName"];
            List<string> ProjectCodes = temp.Split(',').ToList();
            List<string> AddressCodes = tempAddress.Split(',').ToList();
            string RDate = "";
            string AllProjectsFlag = Request.Form["AllProjectsFlag"];
            bool AllProjects = AllProjectsFlag == "Y" ? true : false;


            string PONumber = ""; string PODate = ""; string DONumber = "";
            string WBS1 = ""; string WBS2 = ""; string WBS3 = "";

            string lPODateFrom = "";
            string lPODateTo = "";
            string lRDateFrom = "";
            string lRDateTo = "";

            UserAccessController lUa = new UserAccessController();
            var lUserType = lUa.getUserType(lUserName);
            var lGroupName = lUa.getGroupName(lUserName);

            ViewBag.UserType = lUserType;


            lUa = null;

            if (PODate == null) PODate = "";
            if (PODate.Trim().Length == 0 || PODate.IndexOf("to") <= 0)
            {
                lPODateFrom = "2000-01-01 00:00:00";
                lPODateTo = "2200-01-01 23:59:59";
            }
            else
            {
                lPODateFrom = PODate.Substring(0, PODate.IndexOf("to")).Trim();
                lPODateTo = PODate.Substring(PODate.IndexOf("to") + 2).Trim();
            }

            lPODateFrom = lPODateFrom + " 00:00:00";
            lPODateTo = lPODateTo + " 23:59:59";

            DateTime lDateV = new DateTime();
            if (DateTime.TryParse(lPODateFrom, out lDateV) != true)
            {
                lPODateFrom = "2000-01-01 00:00:00";
            }
            if (DateTime.TryParse(lPODateTo, out lDateV) != true)
            {
                lPODateTo = "2200-01-01 23:59:59";
            }

            if (RDate == null) RDate = "";
            if (RDate.Trim().Length == 0 || RDate.IndexOf("to") <= 0)
            {
                lRDateFrom = "2000-01-01 00:00:00";
                lRDateTo = "2200-01-01 23:59:59";
            }
            else
            {
                lRDateFrom = RDate.Substring(0, RDate.IndexOf("to")).Trim();
                lRDateTo = RDate.Substring(RDate.IndexOf("to") + 2).Trim();
            }

            lRDateFrom = lRDateFrom + " 00:00:00";
            lRDateTo = lRDateTo + " 00:00:00";

            lDateV = new DateTime();
            if (DateTime.TryParse(lRDateFrom, out lDateV) != true)
            {
                lRDateFrom = "2000-01-01 00:00:00";
            }
            if (DateTime.TryParse(lRDateTo, out lDateV) != true)
            {
                lRDateTo = "2200-01-01 23:59:59";
            }


            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet ws = package.Workbook.Worksheets.Add("Saved Order List");

            int lRowNo = 1;
            //ws.Column(1).Width = 5;         //"SNo\n序号";
            ws.Column(1).Width = 12;        //"Order No\n加工表号";
            ws.Column(2).Width = 10;        //"WBS1\n楼座";
            ws.Column(3).Width = 10;        //"WBS2\n楼座";
            ws.Column(4).Width = 7;         //"WBS3\n楼座";
            ws.Column(5).Width = 14;        //"Structure Element";
            ws.Column(6).Width = 14;        //"Prod Type";
            ws.Column(7).Width = 14;        //"PO Number";
            ws.Column(8).Width = 14;        //"BBS Number";
            ws.Column(9).Width = 20;        //"BBS Desc";
            ws.Column(10).Width = 11;        //"Update Date\n订货日期";
            ws.Column(11).Width = 15;        //"Required Date\n到场日期";
            ws.Column(12).Width = 16;       //"Order WT\n料单重量";
            ws.Column(13).Width = 17;       //"Address\n料单重量";
            ws.Column(14).Width = 18;       //"Gate\n料单重量";

            ws.Row(1).Height = 30;
            ws.Cells[lRowNo, 1].Value = "SNo\n序号";
            //ws.Cells[lRowNo, 2].Value = "Order No\n订单号码";
            ws.Cells[lRowNo, 2].Value = "WBS1\n楼座";
            ws.Cells[lRowNo, 3].Value = "WBS2\n楼层";
            ws.Cells[lRowNo, 4].Value = "WBS3\n分部";
            ws.Cells[lRowNo, 5].Value = "Structure Element\n构件";
            ws.Cells[lRowNo, 6].Value = "Product Type\n产品类型";
            ws.Cells[lRowNo, 7].Value = "PO Number\n订单号码";
            ws.Cells[lRowNo, 8].Value = "BBS Number\n钢筋加工号码";
            ws.Cells[lRowNo, 9].Value = "BBS Description\n钢筋加工描述";
            ws.Cells[lRowNo, 10].Value = "Date Created\n创建日期";
            ws.Cells[lRowNo, 11].Value = "Required Date\n到场日期";
            ws.Cells[lRowNo, 12].Value = "Order Weight\n料单重量 (MT)";
            ws.Cells[lRowNo, 13].Value = "Address\n地址";
            ws.Cells[lRowNo, 14].Value = "Gate\n门";

            ws.Cells[lRowNo, 1].Style.WrapText = true;
            ws.Cells[lRowNo, 2].Style.WrapText = true;
            ws.Cells[lRowNo, 3].Style.WrapText = true;
            ws.Cells[lRowNo, 4].Style.WrapText = true;
            ws.Cells[lRowNo, 5].Style.WrapText = true;
            ws.Cells[lRowNo, 6].Style.WrapText = true;
            ws.Cells[lRowNo, 7].Style.WrapText = true;
            ws.Cells[lRowNo, 8].Style.WrapText = true;
            ws.Cells[lRowNo, 9].Style.WrapText = true;
            ws.Cells[lRowNo, 10].Style.WrapText = true;
            ws.Cells[lRowNo, 11].Style.WrapText = true;
            ws.Cells[lRowNo, 12].Style.WrapText = true;
            ws.Cells[lRowNo, 13].Style.WrapText = true;
            ws.Cells[lRowNo, 14].Style.WrapText = true;

            ws.Cells[lRowNo, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 13].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            ws.Cells[lRowNo, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[lRowNo, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[lRowNo, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[lRowNo, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[lRowNo, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[lRowNo, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[lRowNo, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[lRowNo, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[lRowNo, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[lRowNo, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[lRowNo, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[lRowNo, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[lRowNo, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[lRowNo, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[lRowNo, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;

            ws.Cells[lRowNo, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            ws.Cells[lRowNo, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            ws.Cells[lRowNo, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            ws.Cells[lRowNo, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            ws.Cells[lRowNo, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            ws.Cells[lRowNo, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            ws.Cells[lRowNo, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            ws.Cells[lRowNo, 8].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            ws.Cells[lRowNo, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            ws.Cells[lRowNo, 10].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            ws.Cells[lRowNo, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            ws.Cells[lRowNo, 12].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            ws.Cells[lRowNo, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            ws.Cells[lRowNo, 14].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            //ws.Cells[lRowNo, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));

            lRowNo = 2;
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();

            if (CustomerCode != null && ProjectCodes != null)
            {

                for (int x = 0; x < ProjectCodes.Count(); x++)
                {
                    string ProjectCode = ProjectCodes[x];
                    var j = 0;
                    if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
                    {
                        var lProjectState = "";
                        var lAddressState = "";
                        if (AllProjects == true)
                        {
                            SharedAPIController lBackEnd = new SharedAPIController();

                            var lProjects = lBackEnd.getProject(CustomerCode, lUserType, lGroupName);
                            lBackEnd = null;

                            if (lProjects != null && lProjects.Count > 0)
                            {
                                for (int i = 0; i < lProjects.Count; i++)
                                {
                                    if (lProjects[i] != null && lProjects[i].ProjectCode != null && lProjects[i].ProjectCode.Trim().Length > 0)
                                    {
                                        if (lProjectState == "")
                                        {
                                            lProjectState = "M.ProjectCode = '" + lProjects[i].ProjectCode + "' ";
                                        }
                                        else
                                        {
                                            lProjectState = lProjectState + " OR M.ProjectCode = '" + lProjects[i].ProjectCode + "' ";
                                        }
                                    }
                                }
                            }

                            if (lProjectState == "")
                            {
                                lProjectState = "AND 1 = 2 ";
                            }
                            else
                            {
                                lProjectState = "AND (" + lProjectState + ") ";
                            }

                        }
                        else
                        {
                            lProjectState = "AND M.ProjectCode = '" + ProjectCode + "' ";
                        }
                        if (AddressCodes != null && AddressCodes.Any())
                        {
                            var validCodes = AddressCodes
                                .Where(code => !string.IsNullOrWhiteSpace(code))
                                .Distinct() // optional: remove duplicates
                                .ToList();

                            if (validCodes.Any())
                            {
                                lAddressState =
                                    "AND M.AddressCode IN ('" + string.Join("', '", validCodes) + "') ";
                            }
                        }

                        //                    lCmd.CommandText =
                        //                    "SELECT " +
                        //                    "M.OrderNumber, " +
                        //                    "M.WBS1, " +
                        //                    "M.WBS2, " +
                        //                    "M.WBS3, " +
                        //                    "(STUFF( " +
                        //                    "(SELECT ',' + StructureElement " +
                        //                    "FROM dbo.OESProjOrdersSE " +
                        //                    "WHERE OrderNumber = M.OrderNumber " +
                        //                    "GROUP BY StructureElement " +
                        //                    "FOR XML PATH('')), 1, 1, ''))   " +
                        //                    "AS StructureElement, " +
                        //                    "(STUFF( " +
                        //                    "(SELECT ',' + ProductType " +
                        //                    "FROM dbo.OESProjOrdersSE " +
                        //                    "WHERE OrderNumber = M.OrderNumber " +
                        //                    "GROUP BY ProductType " +
                        //                    "FOR XML PATH('')), 1, 1, ''))   " +
                        //                    "AS ProductType, " +
                        //                    "(STUFF( " +
                        //                    "(SELECT ',' + PONumber " +
                        //                    "FROM dbo.OESProjOrdersSE " +
                        //                    "WHERE OrderNumber = M.OrderNumber " +
                        //                    "GROUP BY PONumber " +
                        //                    "FOR XML PATH('')), 1, 1, ''))   " +
                        //                    "AS PONumber, " +
                        //                    "M.UpdateDate,  " +
                        //                    "(STUFF( " +
                        //                    "(SELECT ',' + convert(varchar(10), RequiredDate, 120) " +
                        //                    "FROM dbo.OESProjOrdersSE " +
                        //                    "WHERE OrderNumber = M.OrderNumber " +
                        //                    "GROUP BY convert(varchar(10), RequiredDate, 120) " +
                        //                    "FOR XML PATH('')), 1, 1, ''))   " +
                        //                    "AS RequiredDate, " +
                        //                    "M.TotalWeight, " +
                        //                    "M.UpdateBy, " +
                        //                    "M.OrderShared, " +
                        //                    "MIN(S.ScheduledProd), " +
                        //                    "(STUFF( " +
                        //                    "(SELECT ',' + rtrim(ltrim(BBSNo)) " +
                        //                    "FROM dbo.OESBBS " +
                        //                    "WHERE CustomerCode = M.CustomerCode " +
                        //                    "AND ProjectCode = M.ProjectCode AND JobID > 0 " +
                        //                    "AND JobID in (select CABJobID from dbo.OESProjOrdersSE " +
                        //                    "WHERE OrderNumber = S.OrderNumber group by CABJobID) " +
                        //                    "GROUP BY BBSNo " +
                        //                    "FOR XML PATH('')), 1, 1, ''))   " +
                        //                    "AS BBSNo, " +
                        //                    "(STUFF( " +
                        //                    "(SELECT ',' + rtrim(ltrim(BBSDesc)) " +
                        //                    "FROM dbo.OESBBS " +
                        //                    "WHERE CustomerCode = M.CustomerCode " +
                        //                    "AND ProjectCode = M.ProjectCode AND JobID > 0 " +
                        //                    "AND JobID in (select CABJobID from dbo.OESProjOrdersSE " +
                        //                    "WHERE OrderNumber = S.OrderNumber group by CABJobID) " +
                        //                    "GROUP BY BBSDesc " +
                        //                    "FOR XML PATH('')), 1, 1, ''))   " +
                        //                    "AS BBSDesc, " +
                        //// ✅ Newly added columns
                        //"ISNULL(M.Address, '') AS Address, " +
                        //"ISNULL(M.Gate, '') AS Gate " +
                        //                    "FROM dbo.OESProjOrder M, dbo.OESProjOrdersSE S " +
                        //                    "WHERE M.OrderNumber = S.OrderNumber " +
                        //                    "AND M.CustomerCode = '" + CustomerCode + "' " +
                        //                    "" + lProjectState + " " +
                        //                    "AND (M.OrderStatus is NULL " +
                        //                    "OR M.OrderStatus = 'New' " +
                        //                    "OR M.OrderStatus = 'Created' " +
                        //                    "OR M.OrderStatus = 'Reserved') " +
                        //                    "AND ((S.UpdateDate >= '" + lPODateFrom + "' " +
                        //                    "AND DATEADD(d,-1,S.UpdateDate) < '" + lPODateTo + "') " +
                        //                    "OR (S.UpdateDate is null AND '" + lPODateTo + "' = '2200-01-01 00:00:00' )) " +
                        //                    "AND ((S.RequiredDate >= '" + lRDateFrom + "' " +
                        //                    "AND DATEADD(d,-1,S.RequiredDate) < '" + lRDateTo + "') " +
                        //                    "OR (S.RequiredDate is null AND '" + lRDateTo + "' = '2200-01-01 23:59:59' )) " +
                        //                    "AND (S.PONumber like '%" + PONumber + "%' " +
                        //                    "OR  (S.PONumber is null AND '" + PONumber + "' = '' )) " +
                        //                    "AND (M.WBS1 like '%" + WBS1 + "%' " +
                        //                    "OR (M.WBS1 is null AND '" + WBS1 + "' = '' )) " +
                        //                    "AND (M.WBS2 like '%" + WBS2 + "%' " +
                        //                    "OR (M.WBS2 is null AND '" + WBS2 + "' = '' )) " +
                        //                    "AND (M.WBS3 like '%" + WBS3 + "%' " +
                        //                    "OR (M.WBS3 is null AND '" + WBS3 + "' = '' )) " +
                        //                    "AND (M.UpdateBy = '" + lUserName + "' " +
                        //                    "OR M.OrderShared = 1 ) " +
                        //                    //"AND M.UpdateDate >= '2019-07-01' " +
                        //                    "GROUP BY " +
                        //                    "M.OrderNumber, " +
                        //                    "S.OrderNumber, " +
                        //                    "M.WBS1, " +
                        //                    "M.WBS2, " +
                        //                    "M.WBS3, " +
                        //                    "M.UpdateDate, " +
                        //                    "M.TotalWeight, " +
                        //                    "M.UpdateBy, " +
                        //                    "M.OrderShared, " +
                        //                    "M.CustomerCode, " +
                        //                    "M.ProjectCode, " +
                        //                        "M.Address, " +     // ✅ added
                        //"M.Gate " +         // ✅ added
                        //                    "ORDER BY " +
                        //                    "M.OrderNumber DESC ";

                        lCmd.CommandText =
    "SELECT " +
    "M.OrderNumber, " +
    "M.WBS1, " +
    "M.WBS2, " +
    "M.WBS3, " +
    "(STUFF( " +
    "(SELECT ',' + left(StructureElement + ProductType, len(StructureElement)) " +
    "FROM dbo.OESProjOrdersSE " +
    "WHERE OrderNumber = M.OrderNumber " +
    "GROUP BY StructureElement, ProductType " +
    "FOR XML PATH('')), 1, 1, '')) AS StructureElement, " +
    "(STUFF( " +
    "(SELECT ',' + left(ProductType + StructureElement, len(ProductType)) " +
    "FROM dbo.OESProjOrdersSE " +
    "WHERE OrderNumber = M.OrderNumber " +
    "GROUP BY ProductType, StructureElement " +
    "FOR XML PATH('')), 1, 1, '')) AS ProductType, " +
    "(STUFF( " +
    "(SELECT ',' + PONumber " +
    "FROM dbo.OESProjOrdersSE " +
    "WHERE OrderNumber = M.OrderNumber " +
    "GROUP BY PONumber " +
    "FOR XML PATH('')), 1, 1, '')) AS PONumber, " +
    "M.UpdateDate,  " +
    "(STUFF( " +
    "(SELECT ',' + left(convert(varchar(10), RequiredDate, 120) + ProductType + StructureElement, 10) " +
    "FROM dbo.OESProjOrdersSE " +
    "WHERE OrderNumber = M.OrderNumber " +
    "GROUP BY convert(varchar(10), RequiredDate, 120), ProductType, StructureElement " +
    "FOR XML PATH('')), 1, 1, '')) AS RequiredDate, " +
    "M.TotalWeight, " +
    "M.UpdateBy, " +
    "M.OrderShared, " +
    "MIN(S.ScheduledProd) AS MinScheduledProd, " +
    "(STUFF( " +
    "(SELECT ',' + rtrim(ltrim(BBSNo)) " +
    "FROM dbo.OESBBS " +
    "WHERE CustomerCode = M.CustomerCode " +
    "AND ProjectCode = M.ProjectCode AND JobID > 0 " +
    "AND JobID in (select CABJobID from dbo.OESProjOrdersSE " +
    "WHERE OrderNumber = S.OrderNumber group by CABJobID) " +
    "GROUP BY BBSNo " +
    "FOR XML PATH('')), 1, 1, '')) AS BBSNo, " +
    "(STUFF( " +
    "(SELECT ',' + rtrim(ltrim(BBSDesc)) " +
    "FROM dbo.OESBBS " +
    "WHERE CustomerCode = M.CustomerCode " +
    "AND ProjectCode = M.ProjectCode AND JobID > 0 " +
    "AND JobID in (select CABJobID from dbo.OESProjOrdersSE " +
    "WHERE OrderNumber = S.OrderNumber group by CABJobID) " +
    "GROUP BY BBSDesc " +
    "FOR XML PATH('')), 1, 1, '')) AS BBSDesc, " +
    "M.CustomerCode, " +
    "M.ProjectCode, " +
    "M.Address, " +               // <-- added
    "M.Gate, " +                  // <-- added
    "(SELECT ProjectTitle FROM dbo.OESProject " +
    " WHERE CustomerCode = M.CustomerCode " +
    " AND ProjectCode = M.ProjectCode) AS ProjectTitle " +
    "FROM dbo.OESProjOrder M, dbo.OESProjOrdersSE S " +
    "WHERE M.OrderNumber = S.OrderNumber " +
    "AND M.CustomerCode = '" + CustomerCode + "' " +
    "" + lProjectState + " " +
    "" + lAddressState + " " +
    "AND (M.OrderStatus is NULL " +
    " OR M.OrderStatus = 'New' " +
    " OR M.OrderStatus = 'Created' " +
    " OR M.OrderStatus = 'Reserved') " +
    "AND (M.UpdateBy = '" + lUserName + "' " +
    " OR M.OrderShared = 1 ) " +
    "GROUP BY " +
    "M.OrderNumber, " +
    "S.OrderNumber, " +
    "M.WBS1, " +
    "M.WBS2, " +
    "M.WBS3, " +
    "M.UpdateDate, " +
    "M.TotalWeight, " +
    "M.UpdateBy, " +
    "M.OrderShared, " +
    "M.CustomerCode, " +
    "M.ProjectCode, " +
    "M.Address, " +           // <-- included in GROUP BY
    "M.Gate " +               // <-- included in GROUP BY
    "ORDER BY " +
    "M.OrderNumber DESC ";

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
                                    //ws.Cells[j + lRowNo, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 13].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                                    //ws.Cells[j + lRowNo, 1].Value = j + 1;
                                    ws.Cells[j + lRowNo, 1].Value = (lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetInt32(0).ToString().Trim()); //"Order No\n加工表号";
                                    ws.Cells[j + lRowNo, 2].Value = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim()); //"WBS1\n楼座";
                                    ws.Cells[j + lRowNo, 3].Value = (lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim()); //"WBS2\n楼层";
                                    ws.Cells[j + lRowNo, 4].Value = (lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim()); //"WBS3\n分部";
                                    ws.Cells[j + lRowNo, 5].Value = (lRst.GetValue(4) == DBNull.Value ? "" : (lRst.GetString(4).Trim() == "NONWBS" ? "" : lRst.GetString(4).Trim())); //"Structure Element\n分部

                                    ws.Cells[j + lRowNo, 6].Value = (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5)); //"Product Type\n订货日期";
                                    ws.Cells[j + lRowNo, 7].Value = lRst.GetValue(6) == DBNull.Value ? "" : (lRst.GetString(6).Trim().Length > 0 && lRst.GetString(6).Trim().Substring(0, 1) == "," ? lRst.GetString(6).Trim().Substring(1) : lRst.GetString(6).Trim()); //"Product No\n订货日期";

                                    ws.Cells[j + lRowNo, 8].Value = (lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetString(13).Trim());
                                    ws.Cells[j + lRowNo, 9].Value = WebUtility.HtmlDecode(lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetString(14).Trim());

                                    ws.Cells[j + lRowNo, 10].Value = (lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetDateTime(7).ToString("yyyy-MM-dd")); //"Update Date\n订货日期";

                                    ws.Cells[j + lRowNo, 11].Value = (lRst.GetValue(8) == DBNull.Value ? "" : (lRst.GetString(8).Substring(0, 1) == "," ? lRst.GetString(8).Substring(1) : lRst.GetString(8))); //"Required Date\n到场日期";
                                    ws.Cells[j + lRowNo, 12].Value = (lRst.GetValue(9) == DBNull.Value ? (decimal)0 : (lRst.GetDecimal(9) / 1000));//"Total weight\n订货日期";
                                    ws.Cells[j + lRowNo, 12].Style.Numberformat.Format = "#,###,##0.000";
                                    ws.Cells[j + lRowNo, 13].Value = (lRst.GetValue(17) == DBNull.Value ? "" : lRst.GetString(17).Trim()); //"WBS3\n分部";
                                    ws.Cells[j + lRowNo, 14].Value = (lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetString(18).Trim()); //"WBS3\n分部";
                                    j = j + 1;

                                }
                            }
                            lRst.Close();

                            lProcessObj.CloseNDSConnection(ref lNDSCon);
                        }
                        lProcessObj = null;
                    }
                }

            }
            lCmd = null;
            lNDSCon = null;
            lRst = null;

            //MemoryStream ms = new MemoryStream();
            //package.SaveAs(ms);

            //var bExcel = new byte[ms.Length];
            //ms.Position = 0;
            //ms.Read(bExcel, 0, bExcel.Length);

            ////bPDF = ms.GetBuffer();
            //ms.Flush();
            //ms.Dispose();

            //return Json(bExcel, JsonRequestBehavior.AllowGet);

            MemoryStream ms = new MemoryStream();
            package.SaveAs(ms);

            var bExcel = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(bExcel, 0, bExcel.Length);

            //bPDF = ms.GetBuffer();
            ms.Flush();
            ms.Dispose();

            var fileName = "abc";
            using (MemoryStream memoryStream = new MemoryStream(bExcel)) // Replace bExcelData with your Excel data byte array
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(memoryStream);
                // Set content disposition with the appropriate file name and extension for Excel
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName + ".xlsx"; // Change the file extension to .xlsx for Excel
                                                                                           // Set the content type to Excel
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"); // Use the appropriate MIME type for Excel
                return File(response.Content.ReadAsByteArrayAsync().Result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"); // Use the appropriate MIME type for Excel
            }
        }

        [HttpGet]
        [Route("/CheckTransportModeCAB/{OrderNumber}")]
        public IActionResult CheckTransportModeCAB(string OrderNumber)
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
                    "And (oe.shapeTransport != 3 or oe.shapeTransport is null) "+
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
                            return Ok(areSame); // Return true or false
                        }
                    }
                    lRst.Close();
                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                    return Ok(true); // Default if no rows found
                }
                return StatusCode(500, new { error = "Unable to establish a connection to the server." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
