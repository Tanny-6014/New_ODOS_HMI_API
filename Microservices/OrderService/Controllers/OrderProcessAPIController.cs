using System;
using System.Collections.Generic;
using System.Linq;

//using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.Identity;

using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;

using System.Security.Cryptography;
using OrderService.Controllers;
using OrderService.Models;
using OrderService.Repositories;
using MongoDB.Driver.Linq;
using OrderService.Interfaces;
using System.Data;

namespace OrderService.Controllers
{
    //[Authorize]
    public class OrderProcessAPIController : Controller
    {
        private DBContextModels db = new DBContextModels();

       
        public  string StatusProcess(string pCustomerCode, string pProjectCode, int pOrderNo, string pOrderStatus, string UserID)
        {
            //OrderStatus 1. Submitted, 2. Sent 3. Withdraw; 4. Delete 
            //UserID = "vishalw_ttl@natsteel.com.sg";
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            string lSQL = "";
            string lOriStatus = "";

            string lResponseText = "";
            try
            {
                UserAccessController lUa = new UserAccessController();
                var lUserType = lUa.getUserType(UserID);
                var lGroupName = lUa.getGroupName(UserID);

                var lSubmission = "No";
                var lEditable = "No";

                string lUserName = UserID;
                if (lUserName.IndexOf("@") > 0)
                {
                    lUserName = lUserName.Substring(0, lUserName.IndexOf("@"));
                }

                lUa = null;

                //get Access right;
                if (lUserType == "CU" || lUserType == "CA" || lUserType == "CM")
                {
                    var lAccess = db.UserAccess.Find(UserID, pCustomerCode, pProjectCode);
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

                if ((pOrderStatus == "Submitted") && lSubmission != "Yes")
                {
                    lResponseText = "Insufficient rights to submit order. ";
                    return lResponseText;
                }

                if (pOrderNo == 0)
                {
                    lResponseText = "Invlid Order Number.";
                    return lResponseText;
                }
              
                var lHeader = db.OrderProject.Find(pOrderNo);
                if (lHeader == null || lHeader.OrderNumber == 0)
                {
                    lResponseText = "Invlid Order Number.";
                    return lResponseText;
                }

                lOriStatus = lHeader.OrderStatus;

                //validation
                if (pOrderStatus == "Withdraw")
                {
                    if (lSubmission == "Yes" && lHeader.OrderStatus != "Submitted" && lHeader.OrderStatus != "Submitted*" && lHeader.OrderStatus != "Created*")
                    {
                        lResponseText = "Order had been processed already. Please contact NatSteel coordinator to withdraw the order.";
                        return lResponseText;
                    }
                    if (lSubmission != "Yes" && lHeader.OrderStatus != "Sent" && lHeader.OrderStatus != "Submitted" && lHeader.OrderStatus != "Submitted*" && lHeader.OrderStatus != "Created*")
                    {
                        lResponseText = "Order had been approved already. Please contact approver for the the order withdraw.";
                        return lResponseText;
                    }
                }

                if (pOrderStatus == "Submitted" && lHeader.OrderStatus != "New" && lHeader.OrderStatus != "Created" && lHeader.OrderStatus != "Created*" && lHeader.OrderStatus != "Sent" && lHeader.OrderStatus != "Reserved" && lHeader.OrderStatus != "Submitted*")
                {
                    lResponseText = "Order had been submitted already.";
                    return lResponseText;
                }

                if (pOrderStatus == "Submitted*" && lHeader.OrderStatus != "New" && lHeader.OrderStatus != "Created" && lHeader.OrderStatus != "Created*" && lHeader.OrderStatus != "Sent" && lHeader.OrderStatus != "Reserved")
                {
                    lResponseText = "Order had been submitted already.";
                    return lResponseText;
                }

                if (pOrderStatus == "Recover" && lHeader.OrderStatus != "Deleted")
                {
                    lResponseText = "Order had been recovered already.";
                    return lResponseText;
                }

                if (pOrderStatus == "Delete" && lHeader.OrderStatus != "New" && lHeader.OrderStatus != "Created" && lHeader.OrderStatus != "Reserved")
                {
                    lResponseText = "Order had been submitted already.";
                    return lResponseText;
                }

                if (pOrderStatus == "Sent" && lHeader.OrderStatus != "New" && lHeader.OrderStatus != "Created" && lHeader.OrderStatus != "Reserved")
                {
                    lResponseText = "Order had been sent for approval already.";
                    return lResponseText;
                }

                if (pOrderStatus == "Reject" && lHeader.OrderStatus != "Sent")
                {
                    if (lHeader.OrderStatus == "Created")
                    {
                        lResponseText = "Invalid action. Order had been withdrawn already.";
                    }
                    else
                    {
                        lResponseText = "Invalid action. Order had been Approved already.";
                    }
                    return lResponseText;
                }

                //check for Advance Order & required Date
                if (pOrderStatus == "Submitted" || pOrderStatus == "Send" || pOrderStatus == "Created*")
                {
                    var lProcessObj = new ProcessController();
                    lProcessObj.OpenNDSConnection(ref lNDSCon);

                    var lCarts = (from p in db.OrderProjectSE
                                  where p.OrderNumber == pOrderNo
                                  select p).ToList();
                    if (lCarts == null || lCarts.Count == 0)
                    {
                        lResponseText = "Invalid order cart. Order submission failed.";
                        return lResponseText;
                    }
                    for (int i = 0; i < lCarts.Count; i++)
                    {
                        var lNewSE = lCarts[i];

                        if (pOrderStatus == "Submitted" && lNewSE.RequiredDate == null)
                        {
                            lProcessObj.CloseNDSConnection(ref lNDSCon);
                            lProcessObj = null;
                            lResponseText = "There is no required date for product " + lNewSE.ProductType + ", Please check required date for the order.";
                            return lResponseText;

                        }

                        if (lNewSE.ScheduledProd == "Y")
                        {
                            int lFount = 0;
                            lSQL = "SELECT tntStatusId FROM dbo.BBSReleaseDetails WHERE intPostHeaderid = " + lNewSE.PostHeaderID + " AND tntStatusId = 12 ";

                            lCmd.CommandText = lSQL;
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lRst = lCmd.ExecuteReader();
                            if (lRst.HasRows)
                            {
                                if (lRst.Read())
                                {
                                    lFount = 1;
                                }
                            }
                            lRst.Close();

                            if (lFount == 0)
                            {
                                lSQL = "SELECT S.OrderNumber FROM dbo.OESProjOrdersSE S, dbo.OESProjOrder M " +
                                "WHERE S.OrderNumber = M.OrderNumber " +
                                "AND S.PostHeaderID = " + lNewSE.PostHeaderID + " AND S.OrderStatus is NOT NULL " +
                                "AND S.OrderStatus <> '' AND S.OrderStatus <> 'New' " +
                                "AND S.OrderStatus <> 'Created' AND  S.OrderStatus <> 'Reserved' " +
                                "AND S.OrderStatus <> 'Sent' AND S.OrderStatus <> 'Deleted' " +
                                "AND S.OrderStatus <> 'Cancelled' AND M.OrderStatus <> 'Deleted'  ";

                                lCmd.CommandText = lSQL;
                                lCmd.Connection = lNDSCon;
                                lCmd.CommandTimeout = 300;
                                lRst = lCmd.ExecuteReader();
                                if (lRst.HasRows)
                                {
                                    if (lRst.Read())
                                    {
                                        lFount = 1;
                                    }
                                }
                                lRst.Close();

                            }

                            if (lFount == 1)
                            {
                                lProcessObj.CloseNDSConnection(ref lNDSCon);
                                lProcessObj = null;
                                lResponseText = "The product " + lNewSE.ProductType + " has been ordered by another person already, Please re-select product.";
                                return lResponseText;
                            }
                        }
                    }

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                    lProcessObj = null;
                }

                //withdraw process
                if (pOrderStatus == "Withdraw")
                {
                    if (lHeader.OrderStatus == "Submitted")
                    {
                        //Recover the Split Various Bar
                        RecoverSplitBBS(pOrderNo);
                    }

                    var lCarts = (from p in db.OrderProjectSE
                                  where p.OrderNumber == pOrderNo
                                  select p).ToList();

                    var lNewHeader = lHeader;
                    if (UserID.Split('@')[1].ToLower() == "natsteel.com.sg")
                    {
                        int lFound = 0;
                        if (lCarts != null && lCarts.Count != 0)
                        {
                            for (int i = 0; i < lCarts.Count; i++)
                            {
                                if (lCarts[i].ScheduledProd == "Y")
                                {
                                    lFound = 1;
                                    break;
                                }
                            }
                        }
                        if (lFound == 0)
                        {
                            if (lNewHeader.OrderStatus == "Created*" || lNewHeader.OrderStatus == "Submitted*")
                            {
                                lNewHeader.OrderStatus = "Created";
                            }
                            else
                            {
                                if (lHeader.OrderStatus == "Sent")
                                {
                                    lNewHeader.OrderStatus = "Created";
                                }
                                else
                                {
                                    lNewHeader.OrderStatus = "Created*";
                                }
                            }
                        }
                        else
                        {
                            lNewHeader.OrderStatus = "Created";
                        }
                        lNewHeader.UpdateBy = UserID;
                        lNewHeader.UpdateDate = DateTime.Now;
                    }
                    else
                    {
                        if ((lHeader.OrderStatus == "Submitted" || lHeader.OrderStatus == "Submitted*") && lHeader.SubmitBy != null && lHeader.SubmitBy != "" && lHeader.SubmitBy.Trim().Length > 0)
                        {
                            lNewHeader.OrderStatus = "Sent";
                            lNewHeader.UpdateBy = lNewHeader.SubmitBy;
                            lNewHeader.UpdateDate = lNewHeader.SubmitDate == null ? DateTime.Now : (DateTime)lNewHeader.SubmitDate;
                        }
                        else
                        {
                            lNewHeader.OrderStatus = "Created";
                            lNewHeader.UpdateBy = UserID;
                            lNewHeader.UpdateDate = DateTime.Now;
                        }
                    }
                    db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);

                    if (lCarts == null || lCarts.Count == 0)
                    {
                        lResponseText = "Invalid order cart. Order submission failed.";
                        return lResponseText;
                    }
                    for (int i = 0; i < lCarts.Count; i++)
                    {
                        var lNewSE = lCarts[i];
                        if (UserID.Split('@')[1].ToLower() == "natsteel.com.sg")
                        {
                            if (lNewSE.ScheduledProd == "Y")
                            {
                                lNewSE.OrderStatus = "Created";
                            }
                            else
                            {
                                if (lNewSE.OrderStatus == "Created*")
                                {
                                    lNewSE.OrderStatus = "Created";
                                }
                                else
                                {
                                    lNewSE.OrderStatus = "Created*";
                                }
                            }
                        }
                        else
                        {
                            lNewSE.OrderStatus = "Created";
                        }
                        db.Entry(lCarts[i]).CurrentValues.SetValues(lNewSE);

                        if (lCarts[i].CABJobID > 0)
                        {
                            int lJobID = lCarts[i].CABJobID;
                            var lCAB = db.JobAdvice.Find(pCustomerCode, pProjectCode, lJobID);
                            var lNewCAB = lCAB;
                            lNewCAB.OrderStatus = "Created";
                            lNewCAB.UpdateBy = UserID;
                            lNewCAB.UpdateDate = DateTime.Now;

                            db.Entry(lCAB).CurrentValues.SetValues(lNewCAB);
                        }
                        if (lCarts[i].MESHJobID > 0)
                        {
                            int lJobID = lCarts[i].MESHJobID;
                            var lMESH = db.CTSMESHJobAdvice.Find(pCustomerCode, pProjectCode, lJobID);
                            var lNewMESH = lMESH;
                            lNewMESH.OrderStatus = "Created";
                            lNewMESH.UpdateBy = UserID;
                            lNewMESH.UpdateDate = DateTime.Now;

                            db.Entry(lMESH).CurrentValues.SetValues(lNewMESH);
                        }
                        if (lCarts[i].BPCJobID > 0)
                        {
                            int lJobID = lCarts[i].BPCJobID;
                            var lBPC = db.BPCJobAdvice.Find(pCustomerCode, pProjectCode, false, lJobID);
                            var lNewBPC = lBPC;
                            lNewBPC.OrderStatus = "Created";
                            lNewBPC.UpdateBy = UserID;
                            lNewBPC.UpdateDate = DateTime.Now;

                            db.Entry(lBPC).CurrentValues.SetValues(lNewBPC);
                        }
                        if (lCarts[i].CageJobID > 0)
                        {
                            int lJobID = lCarts[i].CageJobID;
                            var lPRC = db.PRCJobAdvice.Find(pCustomerCode, pProjectCode, lJobID);

                            var lNewPRC = lPRC;
                            lNewPRC.OrderStatus = "Created";
                            lNewPRC.UpdateBy = UserID;
                            lNewPRC.UpdateDate = DateTime.Now;

                            db.Entry(lPRC).CurrentValues.SetValues(lNewPRC);
                        }

                        if (lCarts[i].StdBarsJobID > 0 || lCarts[i].StdMESHJobID > 0 || lCarts[i].CoilProdJobID > 0)
                        {
                            int lJobID = 0;
                            if (lCarts[i].StdBarsJobID > 0)
                            {
                                lJobID = lCarts[i].StdBarsJobID;
                            }
                            if (lCarts[i].StdMESHJobID > 0)
                            {
                                lJobID = lCarts[i].StdMESHJobID;
                            }
                            if (lCarts[i].CoilProdJobID > 0)
                            {
                                lJobID = lCarts[i].CoilProdJobID;
                            }

                            var lStd = db.StdSheetJobAdvice.Find(pCustomerCode, pProjectCode, lJobID);
                            var lNewStd = lStd;
                            //lNewStd.OrderSource = "UX";
                            lNewStd.OrderStatus = "Created";
                            lNewStd.UpdateBy = UserID;
                            lNewStd.UpdateDate = DateTime.Now;

                            db.Entry(lStd).CurrentValues.SetValues(lNewStd);
                        }

                    }

                    db.SaveChanges();
                }

                if (pOrderStatus == "Sent")
                {
                    var lNewHeader = lHeader;
                    lNewHeader.OrderStatus = "Sent";
                    lNewHeader.SubmitDate = DateTime.Now;
                    lNewHeader.SubmitBy = UserID;
                    db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);

                    var lCarts = (from p in db.OrderProjectSE
                                  where p.OrderNumber == pOrderNo
                                  select p).ToList();
                    if (lCarts == null || lCarts.Count == 0)
                    {
                        lResponseText = "Invalid order cart. Order submission failed.";
                        return lResponseText;
                    }
                    for (int i = 0; i < lCarts.Count; i++)
                    {
                        var lNewSE = lCarts[i];
                        lNewSE.OrderStatus = "Sent";
                        db.Entry(lCarts[i]).CurrentValues.SetValues(lNewSE);

                        if (lCarts[i].CABJobID > 0)
                        {
                            int lJobID = lCarts[i].CABJobID;
                            var lCAB = db.JobAdvice.Find(pCustomerCode, pProjectCode, lJobID);
                            var lNewCAB = lCAB;
                            lNewCAB.OrderStatus = "Sent";
                            lNewCAB.UpdateBy = UserID;
                            lNewCAB.UpdateDate = DateTime.Now;

                            db.Entry(lCAB).CurrentValues.SetValues(lNewCAB);
                        }
                        if (lCarts[i].MESHJobID > 0)
                        {
                            int lJobID = lCarts[i].MESHJobID;
                            var lMESH = db.CTSMESHJobAdvice.Find(pCustomerCode, pProjectCode, lJobID);
                            var lNewMESH = lMESH;
                            lNewMESH.OrderStatus = "Sent";
                            lNewMESH.UpdateBy = UserID;
                            lNewMESH.UpdateDate = DateTime.Now;

                            db.Entry(lMESH).CurrentValues.SetValues(lNewMESH);
                        }
                        if (lCarts[i].BPCJobID > 0)
                        {
                            int lJobID = lCarts[i].BPCJobID;
                            var lBPC = db.BPCJobAdvice.Find(pCustomerCode, pProjectCode, false, lJobID);
                            var lNewBPC = lBPC;
                            lNewBPC.OrderStatus = "Sent";
                            lNewBPC.UpdateBy = UserID;
                            lNewBPC.UpdateDate = DateTime.Now;

                            db.Entry(lBPC).CurrentValues.SetValues(lNewBPC);
                        }
                        if (lCarts[i].CageJobID > 0)
                        {
                            int lJobID = lCarts[i].CageJobID;
                            var lPRC = db.PRCJobAdvice.Find(pCustomerCode, pProjectCode, lJobID);

                            var lNewPRC = lPRC;
                            lNewPRC.OrderStatus = "Sent";
                            lNewPRC.UpdateBy = UserID;
                            lNewPRC.UpdateDate = DateTime.Now;

                            db.Entry(lPRC).CurrentValues.SetValues(lNewPRC);
                        }

                        if (lCarts[i].StdBarsJobID > 0 || lCarts[i].StdMESHJobID > 0 || lCarts[i].CoilProdJobID > 0)
                        {
                            int lJobID = 0;
                            if (lCarts[i].StdBarsJobID > 0)
                            {
                                lJobID = lCarts[i].StdBarsJobID;
                            }
                            if (lCarts[i].StdMESHJobID > 0)
                            {
                                lJobID = lCarts[i].StdMESHJobID;
                            }
                            if (lCarts[i].CoilProdJobID > 0)
                            {
                                lJobID = lCarts[i].CoilProdJobID;
                            }

                            var lStd = db.StdSheetJobAdvice.Find(pCustomerCode, pProjectCode, lJobID);
                            var lNewStd = lStd;
                            //lNewStd.OrderSource = "UX";
                            lNewStd.OrderStatus = "Sent";
                            lNewStd.UpdateBy = UserID;
                            lNewStd.UpdateDate = DateTime.Now;

                            db.Entry(lStd).CurrentValues.SetValues(lNewStd);
                        }

                    }
                    db.SaveChanges();
                }

                if (pOrderStatus == "Delete")
                {
                    var lNewHeader = lHeader;
                    lNewHeader.OrderStatus = "Deleted";
                    lNewHeader.UpdateDate = DateTime.Now;
                    lNewHeader.UpdateBy = UserID;
                    db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);

                    var lCarts = (from p in db.OrderProjectSE
                                  where p.OrderNumber == pOrderNo
                                  select p).ToList();
                    if (lCarts == null || lCarts.Count == 0)
                    {
                        lResponseText = "Invalid order cart. Order delete failed.";
                        return lResponseText;
                    }
                    for (int i = 0; i < lCarts.Count; i++)
                    {
                        var lNewSE = lCarts[i];
                        lNewSE.OrderStatus = "Deleted";
                        lNewSE.UpdateDate = DateTime.Now;
                        lNewSE.UpdateBy = UserID;
                        db.Entry(lCarts[i]).CurrentValues.SetValues(lNewSE);
                    }

                    db.SaveChanges();
                    var lOrderNo = lHeader.OrderNumber;
                    CheckForUpcomingOrders(lOrderNo);

                }

                if (pOrderStatus == "Recover")
                {
                    var lNewHeader = lHeader;
                    lNewHeader.OrderStatus = "Created";
                    lNewHeader.UpdateDate = DateTime.Now;
                    lNewHeader.UpdateBy = UserID;
                    db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);

                    var lCarts = (from p in db.OrderProjectSE
                                  where p.OrderNumber == pOrderNo
                                  select p).ToList();
                    if (lCarts == null || lCarts.Count == 0)
                    {
                        lResponseText = "Invalid order cart. Order delete failed.";
                        return lResponseText;
                    }
                    for (int i = 0; i < lCarts.Count; i++)
                    {
                        var lNewSE = lCarts[i];
                        lNewSE.OrderStatus = "Created";
                        lNewSE.UpdateDate = DateTime.Now;
                        lNewSE.UpdateBy = UserID;
                        db.Entry(lCarts[i]).CurrentValues.SetValues(lNewSE);
                    }

                    db.SaveChanges();
                }

                if (pOrderStatus == "Reject")
                {
                    var lNewHeader = lHeader;
                    lNewHeader.OrderStatus = "Created";
                    if (lNewHeader.SubmitBy != null && lNewHeader.SubmitBy != "")
                    {
                        lNewHeader.UpdateBy = lNewHeader.SubmitBy;
                    }
                    db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);
                    db.SaveChanges();

                    var lCarts = (from p in db.OrderProjectSE
                                  where p.OrderNumber == pOrderNo
                                  select p).ToList();
                    if (lCarts != null && lCarts.Count > 0)
                    {
                        for (int i = 0; i < lCarts.Count; i++)
                        {
                            var lNewSE = lCarts[i];

                            lNewSE.OrderStatus = "Created";

                            db.Entry(lCarts[i]).CurrentValues.SetValues(lNewSE);

                            if (lCarts[i].CABJobID > 0)
                            {
                                int lJobID = lCarts[i].CABJobID;
                                var lCAB = db.JobAdvice.Find(pCustomerCode, pProjectCode, lJobID);
                                var lNewCAB = lCAB;
                                lNewCAB.OrderStatus = "Created";
                                lNewCAB.UpdateBy = UserID;
                                lNewCAB.UpdateDate = DateTime.Now;

                                db.Entry(lCAB).CurrentValues.SetValues(lNewCAB);
                            }
                            if (lCarts[i].MESHJobID > 0)
                            {
                                int lJobID = lCarts[i].MESHJobID;
                                var lMESH = db.CTSMESHJobAdvice.Find(pCustomerCode, pProjectCode, lJobID);
                                var lNewMESH = lMESH;
                                lNewMESH.OrderStatus = "Created";
                                lNewMESH.UpdateBy = UserID;
                                lNewMESH.UpdateDate = DateTime.Now;

                                db.Entry(lMESH).CurrentValues.SetValues(lNewMESH);
                            }
                            if (lCarts[i].BPCJobID > 0)
                            {
                                int lJobID = lCarts[i].BPCJobID;
                                var lBPC = db.BPCJobAdvice.Find(pCustomerCode, pProjectCode, false, lJobID);
                                var lNewBPC = lBPC;
                                lNewBPC.OrderStatus = "Created";
                                lNewBPC.UpdateBy = UserID;
                                lNewBPC.UpdateDate = DateTime.Now;

                                db.Entry(lBPC).CurrentValues.SetValues(lNewBPC);
                            }
                            if (lCarts[i].CageJobID > 0)
                            {
                                int lJobID = lCarts[i].CageJobID;
                                var lPRC = db.PRCJobAdvice.Find(pCustomerCode, pProjectCode, lJobID);

                                var lNewPRC = lPRC;
                                lNewPRC.OrderStatus = "Created";
                                lNewPRC.UpdateBy = UserID;
                                lNewPRC.UpdateDate = DateTime.Now;

                                db.Entry(lPRC).CurrentValues.SetValues(lNewPRC);
                            }

                            if (lCarts[i].StdBarsJobID > 0 || lCarts[i].StdMESHJobID > 0 || lCarts[i].CoilProdJobID > 0)
                            {
                                int lJobID = 0;
                                if (lCarts[i].StdBarsJobID > 0)
                                {
                                    lJobID = lCarts[i].StdBarsJobID;
                                }
                                if (lCarts[i].StdMESHJobID > 0)
                                {
                                    lJobID = lCarts[i].StdMESHJobID;
                                }
                                if (lCarts[i].CoilProdJobID > 0)
                                {
                                    lJobID = lCarts[i].CoilProdJobID;
                                }

                                var lStd = db.StdSheetJobAdvice.Find(pCustomerCode, pProjectCode, lJobID);
                                var lNewStd = lStd;
                                //lNewStd.OrderSource = "UX";
                                lNewStd.OrderStatus = "Created";
                                lNewStd.UpdateBy = UserID;
                                lNewStd.UpdateDate = DateTime.Now;

                                db.Entry(lStd).CurrentValues.SetValues(lNewStd);
                            }

                        }

                        db.SaveChanges();

                    }
                }

                if (pOrderStatus == "Submitted")
                {
                    var lCarts = (from p in db.OrderProjectSE
                                  where p.OrderNumber == pOrderNo
                                  select p).ToList();
                    if (lCarts == null || lCarts.Count == 0)
                    {
                        lResponseText = "Invalid order cart. Order submission failed.";
                        return lResponseText;
                    }
                    //check the Submitted* status
                    int lSubmittedS = 0;
                    for (int i = 0; i < lCarts.Count; i++)
                    {
                        //if (lCarts[i].ScheduledProd != "Y" && lCarts[i].ProductType == "BPC" && lCarts[i].TotalWeight == 0 && lCarts[i].TotalPCs == 0)
                        if ((lCarts[i].ScheduledProd != "Y" && lCarts[i].ProductType == "BPC" && UserID.Split('@')[1].ToUpper().Trim() != "NATSTEEL.COM.SG") ||
                            (lCarts[i].ScheduledProd != "Y" && lCarts[i].ProductType == "BPC" && UserID.Split('@')[1].ToUpper().Trim() == "NATSTEEL.COM.SG" && lCarts[i].TotalWeight == 0 && lCarts[i].TotalPCs == 0))
                        {
                            lSubmittedS = 1;
                        }
                        else
                        {
                            lSubmittedS = 0;
                        }
                    }

                    var lNewHeader = lHeader;
                    if (lSubmittedS == 1)
                    {
                        lNewHeader.OrderStatus = "Submitted*";
                        lNewHeader.UpdateBy = UserID;
                    }
                    else
                    {
                        lNewHeader.OrderStatus = "Submitted";
                        if (lNewHeader.UpdateBy == null ||
                            lNewHeader.UpdateBy == "" ||
                            lNewHeader.UpdateBy.Split('@').Length < 2 ||
                            lNewHeader.UpdateBy.Split('@')[1].ToUpper().Trim() == "NATSTEEL.COM.SG" ||
                            UserID.Split('@')[1].ToUpper().Trim() != "NATSTEEL.COM.SG")
                        {
                            lNewHeader.UpdateBy = UserID;
                        }
                    }

                    lNewHeader.UpdateDate = DateTime.Now;
                    if (UserID != null && UserID.Split('@').Length == 2 && UserID.Split('@')[1].ToUpper().Trim() != "NATSTEEL.COM.SG")
                    {
                        lNewHeader.PODate = DateTime.Now;
                    }
                    db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);

                    //Update Structure level
                    for (int i = 0; i < lCarts.Count; i++)
                    {
                        // set PO Date for customer
                        if (UserID != null && UserID.Split('@').Length == 2 && UserID.Split('@')[1].ToUpper().Trim() != "NATSTEEL.COM.SG")
                        {
                            lCarts[i].PODate = DateTime.Now;
                        }

                        var lNewSE = lCarts[i];

                        if (lSubmittedS == 1)
                        {
                            lNewSE.OrderStatus = "Submitted*";
                        }
                        else
                        {
                            lNewSE.OrderStatus = "Submitted";
                        }
                        lNewSE.SAPSOR = "";
                        lNewSE.UpdateBy = UserID;
                        lNewSE.UpdateDate = DateTime.Now;
                        if (lNewSE.OrigReqDate == null)
                        {
                            lNewSE.OrigReqDate = lNewSE.RequiredDate;
                        }
                        db.Entry(lCarts[i]).CurrentValues.SetValues(lNewSE);

                        if (lCarts[i].CABJobID > 0)
                        {
                            int lJobID = lCarts[i].CABJobID;
                            var lCAB = db.JobAdvice.Find(pCustomerCode, pProjectCode, lJobID);
                            if (lCAB == null || lCAB.JobID == 0)
                            {
                                lNewHeader.OrderStatus = lHeader.OrderStatus;
                                db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);

                                lResponseText = "Invalid order cart. Order submission failed.";
                                return lResponseText;
                            }
                            var lNewCAB = lCAB;
                            if (lNewCAB.OrderSource == null || (lNewCAB.OrderSource != "UX" && lNewCAB.OrderSource != "UXE"))
                            {
                                lNewCAB.OrderSource = "UX";
                            }
                            lNewCAB.DeliveryAddress = lHeader.DeliveryAddress;
                            lNewCAB.OrderStatus = "Submitted";
                            lNewCAB.PODate = (DateTime)lCarts[i].PODate;
                            lNewCAB.PONumber = lCarts[i].PONumber;
                            lNewCAB.Remarks = lHeader.Remarks;
                            lNewCAB.RequiredDate = (DateTime)lCarts[i].RequiredDate;
                            lNewCAB.Scheduler_HP = lHeader.Scheduler_HP;
                            lNewCAB.Scheduler_Name = lHeader.Scheduler_Name;
                            lNewCAB.Scheduler_Tel = lHeader.SiteEngr_Email;
                            lNewCAB.SiteEngr_HP = lHeader.SiteEngr_HP;
                            lNewCAB.SiteEngr_Name = lHeader.SiteEngr_Name;
                            lNewCAB.SiteEngr_Tel = lHeader.SiteEngr_Name;
                            lNewCAB.TransportMode = lCarts[i].TransportMode;
                            lNewCAB.UpdateBy = UserID;
                            lNewCAB.UpdateDate = DateTime.Now;
                            lNewCAB.WBS1 = lHeader.WBS1;
                            lNewCAB.WBS2 = lHeader.WBS2;
                            lNewCAB.WBS3 = lHeader.WBS3;

                            db.Entry(lCAB).CurrentValues.SetValues(lNewCAB);
                        }
                        if (lCarts[i].MESHJobID > 0)
                        {
                            int lJobID = lCarts[i].MESHJobID;
                            var lMESH = db.CTSMESHJobAdvice.Find(pCustomerCode, pProjectCode, lJobID);
                            if (lMESH == null || lMESH.JobID == 0)
                            {
                                lNewHeader.OrderStatus = lHeader.OrderStatus;
                                db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);

                                lResponseText = "Invalid order cart. Order submission failed.";
                                return lResponseText;
                            }
                            var lNewMESH = lMESH;
                            lNewMESH.OrderSource = "UX";
                            lNewMESH.DeliveryAddress = lHeader.DeliveryAddress;
                            lNewMESH.OrderStatus = "Submitted";
                            lNewMESH.PODate = (DateTime)lCarts[i].PODate;
                            lNewMESH.PONumber = lCarts[i].PONumber;
                            lNewMESH.Remarks = lHeader.Remarks;
                            lNewMESH.RequiredDate = (DateTime)lCarts[i].RequiredDate;
                            lNewMESH.Scheduler_HP = lHeader.Scheduler_HP;
                            lNewMESH.Scheduler_Name = lHeader.Scheduler_Name;
                            lNewMESH.Scheduler_Tel = lHeader.SiteEngr_Email;
                            lNewMESH.SiteEngr_HP = lHeader.SiteEngr_HP;
                            lNewMESH.SiteEngr_Name = lHeader.SiteEngr_Name;
                            lNewMESH.SiteEngr_Tel = lHeader.SiteEngr_Name;
                            lNewMESH.Transport = lCarts[i].TransportMode;
                            lNewMESH.UpdateBy = UserID;
                            lNewMESH.UpdateDate = DateTime.Now;
                            lNewMESH.WBS1 = lHeader.WBS1;
                            lNewMESH.WBS2 = lHeader.WBS2;
                            lNewMESH.WBS3 = lHeader.WBS3;

                            db.Entry(lMESH).CurrentValues.SetValues(lNewMESH);
                        }
                        if (lCarts[i].BPCJobID > 0)
                        {
                            int lJobID = lCarts[i].BPCJobID;
                            var lBPC = db.BPCJobAdvice.Find(pCustomerCode, pProjectCode, false, lJobID);
                            if (lBPC == null || lBPC.JobID == 0)
                            {
                                lNewHeader.OrderStatus = lHeader.OrderStatus;
                                db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);

                                lResponseText = "Invalid order cart. Order submission failed.";
                                return lResponseText;
                            }
                            var lNewBPC = lBPC;
                            lNewBPC.OrderSource = "UX";
                            lNewBPC.DeliveryAddress = lHeader.DeliveryAddress;
                            lNewBPC.OrderStatus = "Submitted";
                            lNewBPC.PODate = (DateTime)lCarts[i].PODate;
                            lNewBPC.PONumber = lCarts[i].PONumber;
                            lNewBPC.Remarks = lHeader.Remarks;
                            lNewBPC.RequiredDate = (DateTime)lCarts[i].RequiredDate;
                            lNewBPC.Scheduler_HP = lHeader.Scheduler_HP;
                            lNewBPC.Scheduler_Name = lHeader.Scheduler_Name;
                            lNewBPC.Scheduler_Tel = lHeader.SiteEngr_Email;
                            lNewBPC.SiteEngr_HP = lHeader.SiteEngr_HP;
                            lNewBPC.SiteEngr_Name = lHeader.SiteEngr_Name;
                            lNewBPC.SiteEngr_Tel = lHeader.SiteEngr_Name;
                            lNewBPC.Transport = lCarts[i].TransportMode;
                            lNewBPC.UpdateBy = UserID;
                            lNewBPC.UpdateDate = DateTime.Now;

                            db.Entry(lBPC).CurrentValues.SetValues(lNewBPC);
                        }
                        if (lCarts[i].CageJobID > 0)
                        {
                            int lJobID = lCarts[i].CageJobID;
                            var lPRC = db.PRCJobAdvice.Find(pCustomerCode, pProjectCode, lJobID);
                            if (lPRC == null || lPRC.JobID == 0)
                            {
                                lNewHeader.OrderStatus = lHeader.OrderStatus;
                                db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);

                                lResponseText = "Invalid order cart. Order submission failed.";
                                return lResponseText;
                            }
                            var lNewPRC = lPRC;
                            lNewPRC.OrderSource = "UX";
                            lNewPRC.DeliveryAddress = lHeader.DeliveryAddress;
                            lNewPRC.OrderStatus = "Submitted";
                            lNewPRC.PODate = (DateTime)lCarts[i].PODate;
                            lNewPRC.PONumber = lCarts[i].PONumber;
                            lNewPRC.Remarks = lHeader.Remarks;
                            lNewPRC.RequiredDate = (DateTime)lCarts[i].RequiredDate;
                            lNewPRC.Scheduler_HP = lHeader.Scheduler_HP;
                            lNewPRC.Scheduler_Name = lHeader.Scheduler_Name;
                            lNewPRC.Scheduler_Tel = lHeader.SiteEngr_Email;
                            lNewPRC.SiteEngr_HP = lHeader.SiteEngr_HP;
                            lNewPRC.SiteEngr_Name = lHeader.SiteEngr_Name;
                            lNewPRC.SiteEngr_Tel = lHeader.SiteEngr_Name;
                            lNewPRC.Transport = lCarts[i].TransportMode;
                            lNewPRC.UpdateBy = UserID;
                            lNewPRC.UpdateDate = DateTime.Now;

                            db.Entry(lPRC).CurrentValues.SetValues(lNewPRC);
                        }

                        if (lCarts[i].StdBarsJobID > 0 || lCarts[i].StdMESHJobID > 0 || lCarts[i].CoilProdJobID > 0)
                        {
                            int lJobID = 0;
                            if (lCarts[i].StdBarsJobID > 0)
                            {
                                lJobID = lCarts[i].StdBarsJobID;
                            }
                            if (lCarts[i].StdMESHJobID > 0)
                            {
                                lJobID = lCarts[i].StdMESHJobID;
                            }
                            if (lCarts[i].CoilProdJobID > 0)
                            {
                                lJobID = lCarts[i].CoilProdJobID;
                            }

                            var lStd = db.StdSheetJobAdvice.Find(pCustomerCode, pProjectCode, lJobID);
                            if (lStd == null || lStd.JobID == 0)
                            {
                                lNewHeader.OrderStatus = lHeader.OrderStatus;
                                db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);

                                lResponseText = "Invalid order cart. Order submission failed.";
                                return lResponseText;
                            }
                            var lNewStd = lStd;
                            //lNewStd.OrderSource = "UX";
                            lNewStd.DeliveryAddress = lHeader.DeliveryAddress;
                            lNewStd.OrderStatus = "Submitted";
                            lNewStd.PODate = (DateTime)lCarts[i].PODate;
                            lNewStd.PONumber = lCarts[i].PONumber;
                            lNewStd.Remarks = lHeader.Remarks;
                            lNewStd.RequiredDate = (DateTime)lCarts[i].RequiredDate;
                            lNewStd.Scheduler_HP = lHeader.Scheduler_HP;
                            lNewStd.Scheduler_Name = lHeader.Scheduler_Name;
                            lNewStd.Scheduler_Tel = lHeader.SiteEngr_Email;
                            lNewStd.SiteEngr_HP = lHeader.SiteEngr_HP;
                            lNewStd.SiteEngr_Name = lHeader.SiteEngr_Name;
                            lNewStd.SiteEngr_Tel = lHeader.SiteEngr_Name;
                            lNewStd.Transport = lCarts[i].TransportMode;
                            lNewStd.UpdateBy = UserID;
                            lNewStd.UpdateDate = DateTime.Now;

                            db.Entry(lStd).CurrentValues.SetValues(lNewStd);
                        }
                    }
                    db.SaveChanges();
                }

                if (pOrderStatus == "Created*")
                {
                    var lNewHeader = lHeader;
                    lNewHeader.OrderStatus = "Created*";
                    db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);

                    var lCarts = (from p in db.OrderProjectSE
                                  where p.OrderNumber == pOrderNo
                                  select p).ToList();
                    if (lCarts == null || lCarts.Count == 0)
                    {
                        lResponseText = "Invalid order cart. Order submission failed.";
                        return lResponseText;
                    }
                    for (int i = 0; i < lCarts.Count; i++)
                    {
                        var lNewSE = lCarts[i];
                        lNewSE.OrderStatus = "Created*";
                        db.Entry(lCarts[i]).CurrentValues.SetValues(lNewSE);

                        if (lCarts[i].CABJobID > 0)
                        {
                            int lJobID = lCarts[i].CABJobID;
                            var lCAB = db.JobAdvice.Find(pCustomerCode, pProjectCode, lJobID);
                            if (lCAB == null || lCAB.JobID == 0)
                            {
                                lNewHeader.OrderStatus = lHeader.OrderStatus;
                                db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);

                                lResponseText = "Invalid order cart. Order submission failed.";
                                return lResponseText;
                            }
                            var lNewCAB = lCAB;
                            lNewCAB.OrderSource = "UX";
                            lNewCAB.DeliveryAddress = lHeader.DeliveryAddress;
                            lNewCAB.OrderStatus = "Created";
                            lNewCAB.PODate = (DateTime)lCarts[i].PODate;
                            lNewCAB.PONumber = lCarts[i].PONumber;
                            lNewCAB.Remarks = lHeader.Remarks;
                            lNewCAB.RequiredDate = (DateTime)lCarts[i].RequiredDate;
                            lNewCAB.Scheduler_HP = lHeader.Scheduler_HP;
                            lNewCAB.Scheduler_Name = lHeader.Scheduler_Name;
                            lNewCAB.Scheduler_Tel = lHeader.SiteEngr_Email;
                            lNewCAB.SiteEngr_HP = lHeader.SiteEngr_HP;
                            lNewCAB.SiteEngr_Name = lHeader.SiteEngr_Name;
                            lNewCAB.SiteEngr_Tel = lHeader.SiteEngr_Name;
                            lNewCAB.TransportMode = lCarts[i].TransportMode;
                            lNewCAB.UpdateBy = UserID;
                            lNewCAB.UpdateDate = DateTime.Now;
                            lNewCAB.WBS1 = lHeader.WBS1;
                            lNewCAB.WBS2 = lHeader.WBS2;
                            lNewCAB.WBS3 = lHeader.WBS3;

                            db.Entry(lCAB).CurrentValues.SetValues(lNewCAB);
                        }
                        if (lCarts[i].MESHJobID > 0)
                        {
                            int lJobID = lCarts[i].MESHJobID;
                            var lMESH = db.CTSMESHJobAdvice.Find(pCustomerCode, pProjectCode, lJobID);
                            if (lMESH == null || lMESH.JobID == 0)
                            {
                                lNewHeader.OrderStatus = lHeader.OrderStatus;
                                db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);

                                lResponseText = "Invalid order cart. Order submission failed.";
                                return lResponseText;
                            }
                            var lNewMESH = lMESH;
                            lNewMESH.OrderSource = "UX";
                            lNewMESH.DeliveryAddress = lHeader.DeliveryAddress;
                            lNewMESH.OrderStatus = "Created";
                            lNewMESH.PODate = (DateTime)lCarts[i].PODate;
                            lNewMESH.PONumber = lCarts[i].PONumber;
                            lNewMESH.Remarks = lHeader.Remarks;
                            lNewMESH.RequiredDate = (DateTime)lCarts[i].RequiredDate;
                            lNewMESH.Scheduler_HP = lHeader.Scheduler_HP;
                            lNewMESH.Scheduler_Name = lHeader.Scheduler_Name;
                            lNewMESH.Scheduler_Tel = lHeader.SiteEngr_Email;
                            lNewMESH.SiteEngr_HP = lHeader.SiteEngr_HP;
                            lNewMESH.SiteEngr_Name = lHeader.SiteEngr_Name;
                            lNewMESH.SiteEngr_Tel = lHeader.SiteEngr_Name;
                            lNewMESH.Transport = lCarts[i].TransportMode;
                            lNewMESH.UpdateBy = UserID;
                            lNewMESH.UpdateDate = DateTime.Now;
                            lNewMESH.WBS1 = lHeader.WBS1;
                            lNewMESH.WBS2 = lHeader.WBS2;
                            lNewMESH.WBS3 = lHeader.WBS3;

                            db.Entry(lMESH).CurrentValues.SetValues(lNewMESH);
                        }
                        if (lCarts[i].BPCJobID > 0)
                        {
                            int lJobID = lCarts[i].BPCJobID;
                            var lBPC = db.BPCJobAdvice.Find(pCustomerCode, pProjectCode, false, lJobID);
                            if (lBPC == null || lBPC.JobID == 0)
                            {
                                lNewHeader.OrderStatus = lHeader.OrderStatus;
                                db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);

                                lResponseText = "Invalid order cart. Order submission failed.";
                                return lResponseText;
                            }
                            var lNewBPC = lBPC;
                            lNewBPC.OrderSource = "UX";
                            lNewBPC.DeliveryAddress = lHeader.DeliveryAddress;
                            lNewBPC.OrderStatus = "Created";
                            lNewBPC.PODate = (DateTime)lCarts[i].PODate;
                            lNewBPC.PONumber = lCarts[i].PONumber;
                            lNewBPC.Remarks = lHeader.Remarks;
                            lNewBPC.RequiredDate = (DateTime)lCarts[i].RequiredDate;
                            lNewBPC.Scheduler_HP = lHeader.Scheduler_HP;
                            lNewBPC.Scheduler_Name = lHeader.Scheduler_Name;
                            lNewBPC.Scheduler_Tel = lHeader.SiteEngr_Email;
                            lNewBPC.SiteEngr_HP = lHeader.SiteEngr_HP;
                            lNewBPC.SiteEngr_Name = lHeader.SiteEngr_Name;
                            lNewBPC.SiteEngr_Tel = lHeader.SiteEngr_Name;
                            lNewBPC.Transport = lCarts[i].TransportMode;
                            lNewBPC.UpdateBy = UserID;
                            lNewBPC.UpdateDate = DateTime.Now;

                            db.Entry(lBPC).CurrentValues.SetValues(lNewBPC);
                        }
                        if (lCarts[i].CageJobID > 0)
                        {
                            int lJobID = lCarts[i].CageJobID;
                            var lPRC = db.PRCJobAdvice.Find(pCustomerCode, pProjectCode, lJobID);
                            if (lPRC == null || lPRC.JobID == 0)
                            {
                                lNewHeader.OrderStatus = lHeader.OrderStatus;
                                db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);

                                lResponseText = "Invalid order cart. Order submission failed.";
                                return lResponseText;
                            }
                            var lNewPRC = lPRC;
                            lNewPRC.OrderSource = "UX";
                            lNewPRC.DeliveryAddress = lHeader.DeliveryAddress;
                            lNewPRC.OrderStatus = "Created";
                            lNewPRC.PODate = (DateTime)lCarts[i].PODate;
                            lNewPRC.PONumber = lCarts[i].PONumber;
                            lNewPRC.Remarks = lHeader.Remarks;
                            lNewPRC.RequiredDate = (DateTime)lCarts[i].RequiredDate;
                            lNewPRC.Scheduler_HP = lHeader.Scheduler_HP;
                            lNewPRC.Scheduler_Name = lHeader.Scheduler_Name;
                            lNewPRC.Scheduler_Tel = lHeader.SiteEngr_Email;
                            lNewPRC.SiteEngr_HP = lHeader.SiteEngr_HP;
                            lNewPRC.SiteEngr_Name = lHeader.SiteEngr_Name;
                            lNewPRC.SiteEngr_Tel = lHeader.SiteEngr_Name;
                            lNewPRC.Transport = lCarts[i].TransportMode;
                            lNewPRC.UpdateBy = UserID;
                            lNewPRC.UpdateDate = DateTime.Now;

                            db.Entry(lPRC).CurrentValues.SetValues(lNewPRC);
                        }

                        if (lCarts[i].StdBarsJobID > 0 || lCarts[i].StdMESHJobID > 0 || lCarts[i].CoilProdJobID > 0)
                        {
                            int lJobID = 0;
                            if (lCarts[i].StdBarsJobID > 0)
                            {
                                lJobID = lCarts[i].StdBarsJobID;
                            }
                            if (lCarts[i].StdMESHJobID > 0)
                            {
                                lJobID = lCarts[i].StdMESHJobID;
                            }
                            if (lCarts[i].CoilProdJobID > 0)
                            {
                                lJobID = lCarts[i].CoilProdJobID;
                            }

                            var lStd = db.StdSheetJobAdvice.Find(pCustomerCode, pProjectCode, lJobID);
                            if (lStd == null || lStd.JobID == 0)
                            {
                                lNewHeader.OrderStatus = lHeader.OrderStatus;
                                db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);

                                lResponseText = "Invalid order cart. Order submission failed.";
                                return lResponseText;
                            }
                            var lNewStd = lStd;
                            //lNewStd.OrderSource = "UX";
                            lNewStd.DeliveryAddress = lHeader.DeliveryAddress;
                            lNewStd.OrderStatus = "Created";
                            lNewStd.PODate = (DateTime)lCarts[i].PODate;
                            lNewStd.PONumber = lCarts[i].PONumber;
                            lNewStd.Remarks = lHeader.Remarks;
                            lNewStd.RequiredDate = (DateTime)lCarts[i].RequiredDate;
                            lNewStd.Scheduler_HP = lHeader.Scheduler_HP;
                            lNewStd.Scheduler_Name = lHeader.Scheduler_Name;
                            lNewStd.Scheduler_Tel = lHeader.SiteEngr_Email;
                            lNewStd.SiteEngr_HP = lHeader.SiteEngr_HP;
                            lNewStd.SiteEngr_Name = lHeader.SiteEngr_Name;
                            lNewStd.SiteEngr_Tel = lHeader.SiteEngr_Name;
                            lNewStd.Transport = lCarts[i].TransportMode;
                            lNewStd.UpdateBy = UserID;
                            lNewStd.UpdateDate = DateTime.Now;

                            db.Entry(lStd).CurrentValues.SetValues(lNewStd);
                        }
                    }
                    db.SaveChanges();
                }

                if (pOrderStatus == "Submitted" ||
                    (pOrderStatus == "Withdraw" && lOriStatus != "Created*") ||
                    pOrderStatus == "Reject" ||
                    pOrderStatus == "Sent"
                    )
                {
                    //No email sending if natsteel staff created and processed Standard products.
                    var lNeedEmail = 1;
                    var lStandardProd = 0;
                    if (lUserType != "CU" && lUserType != "CA" && lUserType != "CM")
                    {
                        var lOrderSE = (from p in db.OrderProjectSE
                                        where p.OrderNumber == pOrderNo
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
                        if (UserID.Split('@')[1].ToLower() == "natsteel.com.sg"
                            && lHeader.UpdateBy.Split('@')[1].ToLower() == "natsteel.com.sg"
                            && lStandardProd == 1)
                        {
                            lNeedEmail = 0;
                        }
                    }

                    //send prompting email
                    if (lNeedEmail == 1)
                    {
                        var lEmailObj = new SendGridEmail();
                        //lEmailObj.sendOrderActionEmail(pCustomerCode, pProjectCode, pOrderNo, pOrderStatus, lHeader.OrderStatus, UserID, 1, "", "", "");commented by ajit
                        lEmailObj = null;
                    }
                }

            }
            catch (Exception ex)
            {
                //SaveErrorMsg(ex.Message, ex.StackTrace, UserID);
                lResponseText = "Error:" + ex.Message;
            }

            return lResponseText;

        }

        private int RecoverSplitBBS(int pOrderNo)
        {
            int lReturn = 0;
            int RecoverCount = 0;

            var lHeader = db.OrderProject.Find(pOrderNo);

            if (lHeader == null || lHeader.OrderNumber == 0)
            {
                lReturn = -1;
                return lReturn;
            }

            var lCarts = (from p in db.OrderProjectSE
                          where p.OrderNumber == pOrderNo
                          select p).ToList();

            if (lCarts == null || lCarts.Count == 0)
            {
                lReturn = -1;
                return lReturn;
            }
            for (int i = 0; i < lCarts.Count; i++)
            {
                if (lCarts[i].ProductType == "CAB" && lCarts[i].CABJobID > 0)
                {
                    var lCustomerCode = lHeader.CustomerCode;
                    var lProjectCode = lHeader.ProjectCode;
                    var lJobID = lCarts[i].CABJobID;

                    var lBBSs = (from p in db.BBS
                                 where p.CustomerCode == lCustomerCode &&
                                 p.ProjectCode == lProjectCode &&
                                 p.JobID == lJobID
                                 orderby p.BBSID
                                 select p).ToList();

                    if (lBBSs != null && lBBSs.Count > 0)
                    {
                        for (int j = lBBSs.Count - 1; j >= 0; j--)
                        {
                            if (lBBSs[j].BBSCopiedFrom == "VARIOUS BAR")
                            {
                                var lBBSNo = lBBSs[j].BBSNo;
                                if (lBBSNo != null && lBBSNo.Length > 2)
                                {
                                    var lFound = -1;
                                    lBBSNo = lBBSNo.Substring(0, lBBSNo.Trim().Length - 2);
                                    for (int k = 0; k < lBBSs.Count; k++)
                                    {
                                        if (lBBSs[k].BBSNo == lBBSNo)
                                        {
                                            lFound = k;
                                            break;
                                        }
                                    }
                                    // check whehter Old BBS No is cut from start
                                    if (lFound == -1)
                                    {
                                        for (int k = 0; k < lBBSs.Count; k++)
                                        {
                                            if (lBBSs[k].BBSNo.EndsWith(lBBSNo))
                                            {
                                                lFound = k;
                                                break;
                                            }
                                        }
                                    }

                                    if (lFound >= 0)
                                    {
                                        // Remove cancelled indictor and BBS No in Remarks field
                                        var lBBSID = lBBSs[lFound].BBSID;
                                        var lBBSNoSplit = lBBSs[j].BBSNo;
                                        decimal lWT = 0;

                                        var lOrderDetails = (from p in db.OrderDetails
                                                             where p.CustomerCode == lCustomerCode &&
                                                             p.ProjectCode == lProjectCode &&
                                                             p.JobID == lJobID &&
                                                             p.BBSID == lBBSID &&
                                                             p.Cancelled == true &&
                                                             p.Remarks == lBBSNoSplit
                                                             select p).ToList();

                                        if (lOrderDetails != null && lOrderDetails.Count > 0)
                                        {
                                            lWT = 0;
                                            for (int m = 0; m < lOrderDetails.Count; m++)
                                            {
                                                lWT = lWT + (decimal)(lOrderDetails[m].BarWeight == null ? 0 : lOrderDetails[m].BarWeight);

                                                var lBarID = lOrderDetails[m].BarID;
                                                var oldBar = db.OrderDetails.Find(lCustomerCode, lProjectCode, lJobID, lBBSID, lBarID);
                                                if (oldBar != null)
                                                {
                                                    var lNewbar = oldBar;
                                                    lNewbar.Cancelled = null;
                                                    lNewbar.Remarks = "";
                                                    db.Entry(oldBar).CurrentValues.SetValues(lNewbar);
                                                }

                                            }

                                            var lOriBBS = db.BBS.Find(lCustomerCode, lProjectCode, lJobID, lBBSID);
                                            if (lOriBBS != null)
                                            {
                                                var lNewBBS = lOriBBS;
                                                lNewBBS.BBSOrderCABWT = lOriBBS.BBSOrderCABWT + lWT;
                                                lNewBBS.BBSTotalWT = lOriBBS.BBSTotalWT + lWT;
                                                lNewBBS.BBSCancelledWT = lOriBBS.BBSCancelledWT - lWT;

                                                if (lNewBBS.BBSCancelledWT < 0)
                                                {
                                                    lNewBBS.BBSCancelledWT = 0;
                                                }

                                                db.Entry(lOriBBS).CurrentValues.SetValues(lNewBBS);
                                            }

                                        }

                                        // remove split BBS
                                        var lBBSIDSplit = lBBSs[j].BBSID;
                                        var lOrderDetailsSplit = from p in db.OrderDetails
                                                                 where p.CustomerCode == lCustomerCode &&
                                                                 p.ProjectCode == lProjectCode &&
                                                                 p.JobID == lJobID &&
                                                                 p.BBSID == lBBSIDSplit
                                                                 select p;

                                        if (lOrderDetailsSplit != null)
                                        {
                                            db.OrderDetails.RemoveRange(lOrderDetailsSplit);
                                        }
                                        var oldBBS = db.BBS.Find(lCustomerCode, lProjectCode, lJobID, lBBSIDSplit);
                                        if (oldBBS != null)
                                        {
                                            db.BBS.Remove(oldBBS);
                                        }

                                        RecoverCount = RecoverCount + 1;

                                    }
                                }
                            }
                        }

                        if (RecoverCount > 0)
                        {
                            db.SaveChanges();
                        }

                    }

                }
            }

            return lReturn;

        }

        //private int SaveErrorMsg(string pErrorMsg, string pTrack, string UserID)
        //{
        //    int lReturn = 0;
        //    var lCmd = new SqlCommand();
        //    var lNDSCon = new SqlConnection();
        //    string lSQL = "";

        //    string lVar = pErrorMsg.Trim() + " / " + pTrack.Trim();

        //    if (lVar == null)
        //    {
        //        lVar = "";
        //    }
        //    lVar = lVar.Trim();
        //    if (lVar.Length > 8000)
        //    {
        //        lVar = lVar.Substring(0, 8000);
        //    }

        //    var lSourceIP = "";
        //    var lRequest = Request;

        //    if (lRequest != null && lRequest.GetOwinContext() != null)
        //    {
        //        lSourceIP = lRequest.GetOwinContext().Request.RemoteIpAddress;
        //    }

        //    var lProcessObj = new ProcessController();
        //    lProcessObj.OpenNDSConnection(ref lNDSCon);

        //    lSQL = "INSERT INTO dbo.OESProjOrdersError " +
        //       "(ErrorMassage " +
        //       ", UpdateDate " +
        //       ", SourceIP " +
        //       ", UpdateBy) " +
        //       "VALUES " +
        //       "('" + lVar + "' " +
        //       ",getDate() " +
        //       ",'" + lSourceIP + "' " +
        //       ",'" + UserID + "') ";

        //    lCmd.CommandText = lSQL;
        //    lCmd.Connection = lNDSCon;
        //    lCmd.CommandTimeout = 300;
        //    lCmd.ExecuteNonQuery();

        //    lProcessObj.CloseNDSConnection(ref lNDSCon);
        //    lProcessObj = null;

        //    return lReturn;
        //}

        private void CheckForUpcomingOrders(int pOrderNo)
        {
            var lDa = new SqlDataAdapter();
            var lCmd = new SqlCommand();
            var lDs = new DataSet();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;

       

            var lProcess = new ProcessController();
            lProcess.OpenNDSConnection(ref lNDSCon);
            if (lNDSCon.State == ConnectionState.Open)
            {
                var lSQL = "UPDATE OESUPCOMINGORDER SET ConvertedOrderNo=null ,ConvertOrderDate = NULL, ConvertedOrderBy = NULL WHERE ConvertedOrderNo= '" + pOrderNo + "' ";

                lCmd.CommandText = lSQL;
                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
               
                lProcess.CloseNDSConnection(ref lNDSCon);
                lDa = null;
                lCmd = null;
                lDs = null;
                lNDSCon = null;
            }
            
        }
    }
}
