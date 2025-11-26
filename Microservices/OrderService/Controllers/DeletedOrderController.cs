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


namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeletedOrderController : Controller
    {
        private readonly IOrder _OrderRepository;
        private readonly IMapper _mapper;

        private DBContextModels db = new DBContextModels();


        public DeletedOrderController(IOrder orderService, IMapper mapper)
        {
            _OrderRepository = orderService;
            _mapper = mapper;

        }

        //List of raised PO
        [HttpGet]
        public async Task<ActionResult> getDeleteddOrders(string CustomerCode, string ProjectCode,bool AllProjects, string UserName)
        {
            string lPODateFrom = "";
            string lPODateTo = "";
            string lRDateFrom = "";
            string lRDateTo = "";

            UserAccessController lUa = new UserAccessController();
            var lUserType = lUa.getUserType(UserName);
            var lGroupName = lUa.getGroupName(UserName);

          //  ViewBag.UserType = lUserType;

            string lUserName = User.Identity.GetUserName();

            lUa = null;

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
            //    lRDateTo = "2200-01-01 00:00:00";
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
                lRDateTo = "2200-01-01 00:00:00";
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
                CustomerCode = "",
                ProjectCode = "",
                ProjectTitle = ""
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


                    lCmd.CommandText =
                    "SELECT " +
                    "M.OrderNumber, " +
                    "M.WBS1, " +
                    "M.WBS2, " +
                    "M.WBS3, " +
                    "(STUFF( " +
                    "(SELECT ',' + StructureElement " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = M.OrderNumber " +
                    "GROUP BY StructureElement " +
                    "FOR XML PATH('')), 1, 1, ''))   " +
                    "AS StructureElement, " +
                    "(STUFF( " +
                    "(SELECT ',' + ProductType " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = M.OrderNumber " +
                    "GROUP BY ProductType " +
                    "FOR XML PATH('')), 1, 1, ''))   " +
                    "AS ProductType, " +
                    "(STUFF( " +
                    "(SELECT ',' + PONumber " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = M.OrderNumber " +
                    "GROUP BY PONumber " +
                    "FOR XML PATH('')), 1, 1, ''))   " +
                    "AS PONumber, " +
                    "M.UpdateDate,  " +
                    "(STUFF( " +
                    "(SELECT ',' + convert(varchar(10), RequiredDate, 120) " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = M.OrderNumber " +
                    "GROUP BY convert(varchar(10), RequiredDate, 120) " +
                    "FOR XML PATH('')), 1, 1, ''))   " +
                    "AS RequiredDate, " +
                    "M.TotalWeight, " +
                    "M.CustomerCode, " +
                    "M.ProjectCode, " +
                    "(SELECT ProjectTitle FROM dbo.OESProject " +
                    "WHERE CustomerCode = M.CustomerCode " +
                    "AND ProjectCode = M.ProjectCode) as ProjectTitle " +
                    "FROM dbo.OESProjOrder M, dbo.OESProjOrdersSE S " +
                    "WHERE M.OrderNumber = S.OrderNumber " +
                    "AND M.CustomerCode = '" + CustomerCode + "' " +
                    "" + lProjectState + " " +
                    "AND M.OrderStatus = 'Deleted' " +
                    //"AND ((S.UpdateDate >= '" + lPODateFrom + "' " +
                    //"AND DATEADD(d,-1,S.UpdateDate) < '" + lPODateTo + "') " +
                    //"OR (S.UpdateDate is null AND '" + lPODateTo + "' = '2200-01-01 00:00:00' )) " +
                    //"AND ((S.RequiredDate >= '" + lRDateFrom + "' " +
                    //"AND DATEADD(d,-1,S.RequiredDate) < '" + lRDateTo + "') " +
                    //"OR (S.RequiredDate is null AND '" + lRDateTo + "' = '2200-01-01 00:00:00' )) " +
                    //"AND (S.PONumber like '%" + PONumber + "%' " +
                    //"OR  (S.PONumber is null AND '" + PONumber + "' = '' )) " +
                    //"AND (M.WBS1 like '%" + WBS1 + "%' " +
                    //"OR (M.WBS1 is null AND '" + WBS1 + "' = '' )) " +
                    //"AND (M.WBS2 like '%" + WBS2 + "%' " +
                    //"OR (M.WBS2 is null AND '" + WBS2 + "' = '' )) " +
                    //"AND (M.WBS3 like '%" + WBS3 + "%' " +
                    //"OR (M.WBS3 is null AND '" + WBS3 + "' = '' )) " +
                    "AND M.UpdateBy = '" + UserName + "' " +
                    "AND M.UpdateDate >= DateAdd(dd, -2, getDate()) " +
                    "GROUP BY " +
                    "M.OrderNumber, " +
                    "M.WBS1, " +
                    "M.WBS2, " +
                    "M.WBS3, " +
                    "M.UpdateDate, " +
                    "M.TotalWeight, " +
                    "M.CustomerCode, " +
                    "M.ProjectCode " +
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
                                    RequiredDate = (lRst.GetValue(8) == DBNull.Value ? "" : (lRst.GetString(8).Substring(0, 1) == "," ? lRst.GetString(8).Substring(1) : lRst.GetString(8))),
                                    OrderWeight = (lRst.GetValue(9) == DBNull.Value ? "0.000" : (lRst.GetDecimal(9) / 1000).ToString("###,###,##0.000;(###,##0.000);")),
                                    CustomerCode = (lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim()),
                                    ProjectCode = (lRst.GetValue(11) == DBNull.Value ? "" : lRst.GetString(11).Trim()),
                                    ProjectTitle = (lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetString(12).Trim())
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

    }
}
