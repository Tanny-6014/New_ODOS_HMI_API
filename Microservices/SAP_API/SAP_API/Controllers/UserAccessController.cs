
using Microsoft.AspNet.Identity;
using SAP_API.Models;
using SAP_API.Modelss;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace SAP_API.Controllers
{
    public class UserAccessController : Controller
    {
        private DBContextModels db = new DBContextModels();

        [Route("Index")]
        [HttpGet]
        public ActionResult Index()
        {
            var lErrorMsg = "";

            try
            {
                var lUserType = getUserType(User.Identity.GetUserName());
                ViewBag.UserType = lUserType;

                if (lUserType != "AD")
                {
                    return RedirectToAction("Index", "Home");
                }

                var content = db.Customer.ToList();

                content = content.OrderBy(o => o.CustomerName).ToList();

                var lCustID = new List<string>(content.Select(h => h.CustomerCode));
                var lCustName = new List<string>(content.Select(h => h.CustomerName));
                var lOptions = "";
                for (int i = 0; i < lCustID.Count; i++)
                {
                    if (lOptions == "")
                    {
                        lOptions = lCustName[i].Replace("'", "").Trim() + " (" + lCustID[i].Trim() + ")";
                    }
                    else
                    {
                        lOptions = lOptions + "," + lCustName[i].Replace("'", "").Trim() + " (" + lCustID[i].Trim() + ")";
                    }
                }
                ViewBag.CustomerOptions = lOptions;

                var content1 = db.UserName.ToList();

                content1 = content1.OrderBy(o => o.UserName).ToList();

                ViewBag.UserSelection = new SelectList(new List<SelectListItem>(content1.Select(h => new SelectListItem
                {
                    Value = h.UserName,
                    Text = h.UserName
                })), "Value", "Text");

                if (content1.Count() == 0)
                {
                    content1 = new List<UserNameModels> {
                new UserNameModels
                {
                    UserName = ""
                } };
                }
                return View(content1.First());
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            return Json(false);
        }

        [HttpPost]
        //[ValidateAntiForgeryHeader]
        public ActionResult getAccess(string UserName)
        {
            var lErrorMsg = "";
            try
            {
                var lCustomerCodeBK = "";
                var lCustomerNameBK = "";

                var content = (from p in db.UserAccess where p.UserName == UserName orderby p.CustomerCode select p).ToList();
                if (content.Count > 0)
                {
                    for (int i = 0; i < content.Count; i++)
                    {
                        if (content[i].CustomerCode != "0000000000")
                        {
                            var lCustomerCode = content[i].CustomerCode;
                            var lProjectCode = content[i].ProjectCode;

                            if (lCustomerCode == lCustomerCodeBK)
                            {
                                content[i].CustomerCode = lCustomerNameBK + " (" + lCustomerCode + ")";
                            }
                            else
                            {
                                var lCustomer = (from p in db.Customer where p.CustomerCode == lCustomerCode select p).ToList();
                                if (lCustomer.Count > 0)
                                {
                                    content[i].CustomerCode = lCustomer[0].CustomerName + " (" + lCustomerCode + ")";
                                    lCustomerCodeBK = lCustomerCode;
                                    lCustomerNameBK = lCustomer[0].CustomerName;
                                }
                            }

                            var lProject = (from p in db.ProjectList where p.CustomerCode == lCustomerCode && p.ProjectCode == lProjectCode select p).ToList();
                            if (lProject.Count > 0)
                            {
                                content[i].ProjectCode = lProject[0].ProjectTitle + " (" + lProjectCode + ")";
                            }
                            content[i].OrderSubmission = content[i].OrderSubmission.Trim(); ;
                            content[i].OrderCreation = content[i].OrderCreation.Trim();
                            content[i].APISubmission = content[i].APISubmission.Trim();
                            content[i].APICreation = content[i].APICreation.Trim();
                            content[i].UserName = content[i].UserName.Trim();
                            content[i].UserType = content[i].UserType.Trim();
                        }
                    }
                    content = content.OrderBy(o => o.CustomerCode).ThenBy(o => o.ProjectCode).ToList();
                }
                 return Json(content, System.Web.Mvc.JsonRequestBehavior.AllowGet);
                //return Ok(content);

            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            return Json(false);
        }

        [HttpPost]
        //[ValidateAntiForgeryHeader]
        public ActionResult getProject(string CustomerCode)
        {
            var lErrorMsg = "";

            try
            {
                if (CustomerCode != null)
                {
                    //if (CustomerCode.IndexOf("(0") > 0)
                    //{
                    //    CustomerCode = CustomerCode.Substring(CustomerCode.LastIndexOf("(0") + 1, CustomerCode.LastIndexOf("(0") + 11);
                    //}
                    var content = (from p in db.Project where p.CustomerCode == CustomerCode orderby p.ProjectTitle select p).ToList();
                    if (content.Count > 0)
                    {
                        for (var i = 0; i < content.Count; i++)
                        {
                            if (content[i].ProjectTitle != null)
                            {
                                content[i].ProjectTitle = content[i].ProjectTitle.Replace("'", "");
                            }
                        }
                    }
                     return Json(content, System.Web.Mvc.JsonRequestBehavior.AllowGet);
                    //return Ok(content);

                }
                return Json(true);
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            return Json(false);
        }

        [HttpPost]
        //[ValidateAntiForgeryHeader]
        public ActionResult getCustomer()
        {
            var lErrorMsg = "";
            var lCmd = new SqlCommand();
            var lCmdUpdate = new SqlCommand();

            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();

            var lCustomers = (new[]
            {
                new
                {
                    CustomerCode = "",
                    CustomerName = ""
                }

            }).ToList();

            try
            {
                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    lCmd.CommandText = "SELECT C.CustomerCode, C.CustomerName " +
                    "FROM dbo.OESCustomerMaster C, dbo.OESProject P " +
                    "WHERE C.CustomerCode = P.CustomerCode " +
                    "GROUP BY C.CustomerCode, C.CustomerName " +
                    "ORDER BY C.CustomerName ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        while (lRst.Read())
                        {
                            lCustomers.Add(new
                            {
                                CustomerCode = lRst.GetString(0).Trim(),
                                CustomerName = lRst.GetString(1).Trim()
                            });

                        }
                    }

                    lCustomers.RemoveAt(0);

                    lProcessObj.CloseNDSConnection(ref lNDSCon);

                    //var content = (from p in db.Project
                    //               join c in db.Customer on p.CustomerCode equals c.CustomerCode
                    //               group new { c.CustomerCode, c.CustomerName } by new { c.CustomerCode, c.CustomerName } into newg
                    //               orderby newg.Key.CustomerName
                    //               select new
                    //               {
                    //                   newg.Key.CustomerCode,
                    //                   newg.Key.CustomerName
                    //               }).ToList();

                    lProcessObj = null;

                    return Json(lCustomers);
                }
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            return Json(lCustomers);
        }

        [HttpPost]
        //[ValidateAntiForgeryHeader]
        public string getUserType(string UserName)
        {
            var lErrorMsg = "";
            var lUserName = "";
            // UserName = "vishalw_ttl";
            try
            {
                var lUserType = "";
                var lEmpty = 0;
                //try
                //{
                //    lUserType = (string)System.Web.HttpContext.Current.Session[UserName];
                //}
                //catch
                //{
                //    lEmpty = 1;
                //}

                if (UserName != null && UserName != "")
                {
                    if (lUserType == null || lUserType == "" || lEmpty != 0)
                    {
                        var content = db.UserAccess.Find(new object[] { UserName, "0000000000", "0000000000" });
                        if (content != null)
                        {
                            lUserType = content.UserType.Trim();
                            if (lUserType == "P1")
                            {
                                lUserName = "PMG1@natsteel.com.sg";
                                content = db.UserAccess.Find(new object[] { lUserName, "0000000000", "0000000000" });
                                lUserType = content.UserType.Trim();
                            }
                            else if (lUserType == "P2")
                            {
                                lUserName = "PMG2@natsteel.com.sg";
                                content = db.UserAccess.Find(new object[] { lUserName, "0000000000", "0000000000" });
                                lUserType = content.UserType.Trim();
                            }
                            else if (lUserType == "P3")
                            {
                                lUserName = "PMG3@natsteel.com.sg";
                                content = db.UserAccess.Find(new object[] { lUserName, "0000000000", "0000000000" });
                                lUserType = content.UserType.Trim();
                            }
                            else if (lUserType == "P4")
                            {
                                lUserName = "PMG4@natsteel.com.sg";
                                content = db.UserAccess.Find(new object[] { lUserName, "0000000000", "0000000000" });
                                lUserType = content.UserType.Trim();
                            }

                            //  System.Web.HttpContext.Current.Session[UserName] = lUserType.Trim();
                        }

                    }
                }
                return lUserType;
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            return "";

        }

        public string getGroupName(string UserName)
        {
            var lErrorMsg = "";
            var lGroupName = UserName;
            var lUserType = "";
            try
            {
                var content = db.UserAccess.Find(new object[] { UserName, "0000000000", "0000000000" });
                if (content != null)
                {
                    lUserType = content.UserType.Trim();
                    if (lUserType == "P1")
                    {
                        lGroupName = "PMG1@natsteel.com.sg";
                    }
                    else if (lUserType == "P2")
                    {
                        lGroupName = "PMG2@natsteel.com.sg";
                    }
                    else if (lUserType == "P3")
                    {
                        lGroupName = "PMG3@natsteel.com.sg";
                    }
                    else if (lUserType == "P4")
                    {
                        lGroupName = "PMG4@natsteel.com.sg";
                    }

                }
                return lGroupName;
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            return "";

        }

        [HttpPost]
        //[ValidateAntiForgeryHeader]
        public ActionResult Create(UserAccessModels UserAccess)
        {
            var lErrorMsg = "";
            try
            {
                if (UserAccess != null)
                {
                    if (UserAccess.CustomerCode.IndexOf("(") > 0)
                    {
                        var lCustomer = UserAccess.CustomerCode;
                        lCustomer = lCustomer.Substring(lCustomer.LastIndexOf("(") + 1, lCustomer.LastIndexOf(")") - lCustomer.LastIndexOf("(") - 1);
                        UserAccess.CustomerCode = lCustomer;
                    }

                    if (UserAccess.ProjectCode.IndexOf("(") > 0)
                    {
                        var lProject = UserAccess.ProjectCode;
                        lProject = lProject.Substring(lProject.LastIndexOf("(") + 1, lProject.LastIndexOf(")") - lProject.LastIndexOf("(") - 1);
                        UserAccess.ProjectCode = lProject;
                    }

                    UserAccess.DateCreated = DateTime.Now;
                    UserAccess.CreatedBy = User.Identity.GetUserName();
                    var oldAccess = db.UserAccess.Find(UserAccess.UserName, UserAccess.CustomerCode, UserAccess.ProjectCode);
                    if (oldAccess == null)
                    {
                        db.Entry(UserAccess).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        //String case may be different, Make them same. 
                        UserAccess.UserName = oldAccess.UserName;
                        db.Entry(oldAccess).CurrentValues.SetValues(UserAccess);
                    }
                    db.SaveChanges();
                }
                return Json(true);
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            return Json(false);
        }

        // POST: UserAccess/Delete/
        [HttpPost]
        //[ValidateAntiForgeryHeader]
        public ActionResult Delete(string UserName, string CustomerCode, string ProjectCode)
        {
            var lErrorMsg = "";
            try
            {
                var lCustomer = CustomerCode;
                if (lCustomer.IndexOf("(") > 0)
                {
                    lCustomer = lCustomer.Substring(lCustomer.LastIndexOf("(") + 1, lCustomer.LastIndexOf(")") - lCustomer.LastIndexOf("(") - 1);
                }

                var lProject = ProjectCode;
                if (lProject.IndexOf("(") > 0)
                {
                    lProject = lProject.Substring(lProject.LastIndexOf("(") + 1, lProject.LastIndexOf(")") - lProject.LastIndexOf("(") - 1);
                }

                UserAccessModels content = db.UserAccess.Find(UserName, lCustomer, lProject);

                if (content != null)
                {
                    db.UserAccess.Remove(content);
                    db.SaveChanges();
                }
                return Json(true);
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            return Json(false);
        }


        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

    }
}
