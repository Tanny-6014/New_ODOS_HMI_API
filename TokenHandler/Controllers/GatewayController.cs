using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using OrderService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
//using Microsoft.IdentityModel.Tokens;

namespace TokenHandler.Controllers
{

    public class CustomerData
    {
        public string CustomerSAPID { get; set; }
        public List<string> ProjectSAPIDs { get; set; }
    }

    [ApiController]
    [Route("api")]
    public class GatewayController : ControllerBase
    {
        private DBContextModels db = new DBContextModels();
        private readonly ILogger<GatewayController> _logger;

        public GatewayController(ILogger<GatewayController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("/IndexUMP")]
        public IActionResult IndexUMP()
        {
            var lSSOUrl = "https://ODOSSAP.natsteel.com.sg";

           
            string lToken = Request.Form["token"];// HttpContext.Request.Form["token"];

            //var httpRequest = HttpContext.Current.Request;
            //string lToken = httpRequest.Form["token"];

            bool lValidUser = false;




            var lServerURL = Request.Headers["Referer"].ToString();// System.Web.HttpContext.Current.Request.UrlReferrer;

            var lServername = "";

            //var referrerUri = new Uri(lServerURL);


            //if (lServerURL != null)
            //{
            //    lServername = referrerUri.AbsoluteUri;
            //}

            //if (lServername == "")
            //{
            //lServername = "https://ODOSSAP.natsteel.com.sg";
            // }

            //if (lServername.Length > 27)
            //{
            //    lServername = lServername.Substring(0, 27);
            //}

            //if (lToken == null || lToken == "" || (lServername != "https://ODOSSAP.natsteel.com.sg"))
            if (lToken == null || lToken == "")
                {
                //return Redirect(lSSOUrl);
                return BadRequest("UnAuthorize");

            }

            var lJWTHandler = new JwtSecurityTokenHandler();

            IdentityModelEventSource.ShowPII = true;

            SecurityToken lValidatedToken;

            var lParam = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.FromMinutes(1),
                ValidIssuer = "ump.natsteel.com.sg",
                ValidAudience = "ODOSSAP.natsteel.com.sg",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ODOS.Natsteel.com.sg@89273829NatSteel"))
            };

            try
            {
                var handler = new JwtSecurityTokenHandler();

                var lClaims = lJWTHandler.ValidateToken(lToken, lParam, out lValidatedToken);

                //if (lClaims != null)
                //{
                //    lValidUser = true;
                //    var jsonToken = handler.ReadToken(lToken) as JwtSecurityToken;
                //    return Ok(jsonToken);
                //}
                //else
                //{
                //    return BadRequest("UnAuthorize");
                //}




                #region Remove this  region while actual implementaion

                #endregion

                //Uncomment this code while actual implementaion

                if (lClaims != null)
                {
                    var jsonToken = handler.ReadToken(lToken) as JwtSecurityToken;

                    string lUserID = "";
                    string lLoginID = "";
                    string lUserRole = "";

                    var lCustomerData = new List<CustomerData>();

                    var lClaimsList = lClaims.Claims.ToList();
                    if (lClaimsList.Count > 0)
                    {
                        for (int i = 0; i < lClaimsList.Count; i++)
                        {
                            if (lClaimsList[i].Properties.Count > 0 && lClaimsList[i].Properties.ToList()[0].Value.ToLower() == "emailaddress")
                            {
                                lUserID = lClaimsList[i].Value;
                            }
                            if (lClaimsList[i].Properties.Count > 0 && lClaimsList[i].Properties.ToList()[0].Value.ToLower() == "email")
                            {
                                lUserID = lClaimsList[i].Value;
                            }
                            if (lClaimsList[i].Type.ToLower() == "loginid")
                            {
                                lLoginID = lClaimsList[i].Value;
                            }
                        }
                    }

                    if (lLoginID == null || lLoginID == "" || lLoginID.Trim().Length == 0 || lLoginID.Split('@').Length != 2)
                    {
                        return BadRequest("UnAuthorize");
                        //return Redirect(lSSOUrl);
                    }

                    if (lLoginID.Split('@')[1] != null && lLoginID.Split('@')[1].ToLower() == "natsteel.com.sg")
                    {
                        lUserID = lLoginID;
                    }


                    if (lUserID == null || lUserID == "" || lUserID.Trim().Length == 0)
                    {
                        return BadRequest("UnAuthorize");
                        //return Redirect(lSSOUrl);
                    }

                    if (lUserID != null && lUserID != "" && lLoginID != null && lLoginID != "" &&
                        lUserID.Split('@').Length == 2 && lLoginID.Split('@').Length == 2)
                    {
                        if (lUserID.Split('@')[1].ToLower().Trim() == "natsteel.com.sg" &&
                            lLoginID.Split('@')[1].ToLower().Trim() != "natsteel.com.sg")
                        {
                            return BadRequest("UnAuthorize");
                            //return Redirect(lSSOUrl);
                        }

                        //var lAcc = new AccountController(HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(), HttpContext.GetOwinContext().Get<ApplicationSignInManager>());
                        //var lLogin = lAcc.LoginUX(lUserID, "");
                        //lAcc = null;

                        var lLogin = true;
                        if (lLogin == true)
                        {
                            var lAccessGroup = db.UserAccess.Find(lUserID, "0000000000", "0000000000");

                            if (lAccessGroup == null || lAccessGroup.UserName == null ||
                                lAccessGroup.UserName.ToLower().Trim() != lUserID.ToLower().Trim() ||
                                lAccessGroup.UserType == null || lAccessGroup.UserType.Trim().Length == 0 ||
                                ((lAccessGroup.UserType == "CU" || lAccessGroup.UserType == "CA" || lAccessGroup.UserType == "CM") &&
                                lUserID.Split('@')[1].ToLower().Trim() != "natsteel.com.sg"))
                            {
                                string lCustomerCode = "";
                                string lProjectCode = "";

                                if (lClaimsList.Count > 0)
                                {
                                    for (int i = 0; i < lClaimsList.Count; i++)
                                    {
                                        if (lClaimsList[i].Type.ToLower() == "companycode")
                                        {
                                            lCustomerCode = lClaimsList[i].Value;
                                        }
                                        if (lClaimsList[i].Type.ToLower() == "projectcode")
                                        {
                                            lProjectCode = lClaimsList[i].Value;
                                        }
                                        if (lClaimsList[i].Type.ToLower() == "usergroup")
                                        {
                                            lUserRole = lClaimsList[i].Value;
                                        }
                                        if (lClaimsList[i].Type.ToLower() == "customerdata")
                                        {
                                            lCustomerData = JsonSerializer.Deserialize<List<CustomerData>>(lClaimsList[i].Value);
                                        }
                                    }

                                }

                                if (lCustomerData != null && lCustomerData.Count > 0)
                                {
                                    if (lUserID.Split('@')[1].ToLower().Trim() != "natsteel.com.sg")
                                    {
                                        //delete all access right for customer
                                        var lDeleteAcc = (from p in db.UserAccess
                                                          where p.UserName == lUserID
                                                          select p).ToList();
                                        if (lDeleteAcc != null && lDeleteAcc.Count > 0)
                                        {
                                            db.UserAccess.RemoveRange(lDeleteAcc);
                                        }

                                    }

                                    for (int j = 0; j < lCustomerData.Count; j++)
                                    {
                                        lCustomerCode = lCustomerData[j].CustomerSAPID;
                                        var lProjectList = lCustomerData[j].ProjectSAPIDs;

                                        for (int i = 0; i < lProjectList.Count; i++)
                                        {
                                            lProjectCode = lProjectList[i];

                                            // validate Customer and Project code

                                            var lProject = db.Project.Find(lCustomerCode, lProjectCode);
                                            if (lProject != null && lProject.CustomerCode == lCustomerCode && lProject.ProjectCode == lProjectCode)
                                            {
                                                var lNewAccess = new UserAccessModels();
                                                lNewAccess.UserName = lUserID;
                                                lNewAccess.CustomerCode = lCustomerCode;
                                                lNewAccess.ProjectCode = lProjectCode;
                                                lNewAccess.OrderCreation = "No";
                                                if (lUserRole == "CA" || lUserRole == "CU" || lUserRole == "CM" || lUserRole == "SU1" || lUserRole == "SU2" || lUserRole == "SU3" || lUserRole == "SU4" || lUserRole == "ODOSUAT")
                                                {
                                                    lNewAccess.OrderCreation = "Yes";
                                                }

                                                lNewAccess.OrderSubmission = "No";
                                                if (lUserRole == "CA" || lUserRole == "CM" || lUserRole == "SU1" || lUserRole == "SU2" || lUserRole == "SU4" || (lUserRole == "ODOSUAT" && lNewAccess.UserName == "Ajit.Kamble@tatatechnologies.com"))
                                                {
                                                    lNewAccess.OrderSubmission = "Yes";
                                                }

                                                lNewAccess.APICreation = "No";
                                                lNewAccess.APISubmission = "No";
                                                lNewAccess.CreatedBy = "UX";
                                                lNewAccess.DateCreated = DateTime.Now;
                                                lNewAccess.UserType = "CU";
                                                if (lUserRole == "CA" || lUserRole == "CU" || lUserRole == "CM")
                                                {
                                                    lNewAccess.UserType = lUserRole;
                                                }

                                                db.UserAccess.Add(lNewAccess);

                                            }

                                        }
                                    }
                                    var lNewAccessH = new UserAccessModels();
                                    lNewAccessH.UserName = lUserID;
                                    lNewAccessH.CustomerCode = "0000000000";
                                    lNewAccessH.ProjectCode = "0000000000";
                                    lNewAccessH.OrderCreation = "No";
                                    if (lUserRole == "CA" || lUserRole == "CU" || lUserRole == "CM" || lUserRole == "SU1" || lUserRole == "SU2" || lUserRole == "SU3" || lUserRole == "SU4" || lUserRole == "ODOSUAT")
                                    {
                                        lNewAccessH.OrderCreation = "Yes";
                                    }

                                    lNewAccessH.OrderSubmission = "No";
                                    if (lUserRole == "CA" || lUserRole == "CM" || lUserRole == "SU1" || lUserRole == "SU2" || lUserRole == "SU4" || (lUserRole == "ODOSUAT" && lNewAccessH.UserName == "Ajit.Kamble@tatatechnologies.com"))
                                    {
                                        lNewAccessH.OrderSubmission = "Yes";
                                    }

                                    lNewAccessH.APICreation = "No";
                                    lNewAccessH.APISubmission = "No";
                                    lNewAccessH.CreatedBy = "UX";
                                    lNewAccessH.DateCreated = DateTime.Now;
                                    lNewAccessH.UserType = "CU";
                                    if (lUserRole == "CA" || lUserRole == "CU" || lUserRole == "CM")
                                    {
                                        lNewAccessH.UserType = lUserRole;
                                    }

                                    db.UserAccess.Add(lNewAccessH);

                                    db.SaveChanges();

                                    lValidUser = true;
                                }
                                else
                                {
                                    if (lCustomerCode != "" && lProjectCode != "")
                                    {
                                        // Project Code comma separated string
                                        var lProjectList = lProjectCode.Split(',').ToList();

                                        if (lUserID.Split('@')[1].ToLower().Trim() != "natsteel.com.sg")
                                        {
                                            //delete all access right for customer
                                            var lDeleteAcc = (from p in db.UserAccess
                                                              where p.UserName == lUserID &&
                                                              ((p.CustomerCode != lCustomerCode &&
                                                              p.CustomerCode != "0000000000") ||
                                                              (p.CustomerCode == lCustomerCode &&
                                                              (!lProjectList.Contains(p.ProjectCode))))
                                                              select p).ToList();
                                            if (lDeleteAcc != null && lDeleteAcc.Count > 0)
                                            {
                                                db.UserAccess.RemoveRange(lDeleteAcc);
                                            }

                                        }

                                        for (int i = 0; i < lProjectList.Count; i++)
                                        {
                                            lProjectCode = lProjectList[i];

                                            // validate User access right 
                                            var lAccess = db.UserAccess.Find(lUserID, lCustomerCode, lProjectCode);

                                            if (lAccess != null && lAccess.UserName != null && lAccess.UserName.ToLower().Trim() == lUserID.ToLower().Trim())
                                            {
                                                if ((lUserRole == "SU3" && (lAccess.OrderCreation != "Yes" || lAccess.OrderSubmission == "Yes")) ||
                                                ((lUserRole == "CA" || lUserRole == "CM" || lUserRole == "SU1" || lUserRole == "SU2" || lUserRole == "SU4") &&
                                                (lAccess.OrderCreation != "Yes" || lAccess.OrderSubmission != "Yes")))
                                                {
                                                    var lAccessUpdate = lAccess;

                                                    if (lUserRole == "CA" || lUserRole == "CU" || lUserRole == "CM" || lUserRole == "SU1" || lUserRole == "SU2" || lUserRole == "SU3" || lUserRole == "SU4" || lUserRole == "SU5" || lUserRole == "SU6")
                                                    {
                                                        lAccessUpdate.OrderCreation = "Yes";
                                                    }

                                                    lAccessUpdate.OrderSubmission = "No";
                                                    if (lUserRole == "CA" || lUserRole == "CM" || lUserRole == "SU1" || lUserRole == "SU2" || lUserRole == "SU4")
                                                    {
                                                        lAccessUpdate.OrderSubmission = "Yes";
                                                    }

                                                    //lAccessUpdate.APICreation = "No";
                                                    //lAccessUpdate.APISubmission = "No";
                                                    lAccessUpdate.CreatedBy = "UX";
                                                    lAccessUpdate.DateCreated = DateTime.Now;
                                                    lAccessUpdate.UserType = "CU";
                                                    if (lUserRole == "CA" || lUserRole == "CU" || lUserRole == "CM")
                                                    {
                                                        lAccessUpdate.UserType = lUserRole;
                                                    }

                                                    db.Entry(lAccess).CurrentValues.SetValues(lAccessUpdate);
                                                }


                                                lValidUser = true;
                                            }
                                            else
                                            {
                                                // validate Customer and Project code

                                                var lProject = db.Project.Find(lCustomerCode, lProjectCode);
                                                if (lProject != null && lProject.CustomerCode == lCustomerCode && lProject.ProjectCode == lProjectCode)
                                                {
                                                    var lNewAccess = new UserAccessModels();
                                                    lNewAccess.UserName = lUserID;
                                                    lNewAccess.CustomerCode = lCustomerCode;
                                                    lNewAccess.ProjectCode = lProjectCode;
                                                    lNewAccess.OrderCreation = "No";
                                                    if (lUserRole == "CA" || lUserRole == "CU" || lUserRole == "CM" || lUserRole == "SU1" || lUserRole == "SU2" || lUserRole == "SU3" || lUserRole == "SU4" || lUserRole == "ODOSUAT")
                                                    {
                                                        lNewAccess.OrderCreation = "Yes";
                                                    }

                                                    lNewAccess.OrderSubmission = "No";
                                                    if (lUserRole == "CA" || lUserRole == "CM" || lUserRole == "SU1" || lUserRole == "SU2" || lUserRole == "SU4" || (lUserRole == "ODOSUAT" && lNewAccess.UserName == "Ajit.Kamble@tatatechnologies.com"))
                                                    {
                                                        lNewAccess.OrderSubmission = "Yes";
                                                    }

                                                    lNewAccess.APICreation = "No";
                                                    lNewAccess.APISubmission = "No";
                                                    lNewAccess.CreatedBy = "UX";
                                                    lNewAccess.DateCreated = DateTime.Now;
                                                    lNewAccess.UserType = "CU";
                                                    if (lUserRole == "CA" || lUserRole == "CU" || lUserRole == "CM")
                                                    {
                                                        lNewAccess.UserType = lUserRole;
                                                    }

                                                    db.UserAccess.Add(lNewAccess);

                                                }
                                            }
                                        }

                                        if (lAccessGroup == null || lAccessGroup.UserName == null ||
                                        lAccessGroup.UserName.ToLower().Trim() != lUserID.ToLower().Trim())
                                        {
                                            var lNewAccess = new UserAccessModels();
                                            lNewAccess.UserName = lUserID;
                                            lNewAccess.CustomerCode = "0000000000";
                                            lNewAccess.ProjectCode = "0000000000";
                                            lNewAccess.OrderCreation = "No";
                                            if (lUserRole == "CA" || lUserRole == "CU" || lUserRole == "CM" || lUserRole == "SU1" || lUserRole == "SU2" || lUserRole == "SU3" || lUserRole == "SU4" || lUserRole == "ODOSUAT")
                                            {
                                                lNewAccess.OrderCreation = "Yes";
                                            }

                                            lNewAccess.OrderSubmission = "No";
                                            if (lUserRole == "CA" || lUserRole == "CM" || lUserRole == "SU1" || lUserRole == "SU2" || lUserRole == "SU4" || (lUserRole == "ODOSUAT" && lNewAccess.UserName == "Ajit.Kamble@tatatechnologies.com"))
                                            {
                                                lNewAccess.OrderSubmission = "Yes";
                                            }

                                            lNewAccess.APICreation = "No";
                                            lNewAccess.APISubmission = "No";
                                            lNewAccess.CreatedBy = "UX";
                                            lNewAccess.DateCreated = DateTime.Now;
                                            lNewAccess.UserType = "CU";
                                            if (lUserRole == "CA" || lUserRole == "CU" || lUserRole == "CM")
                                            {
                                                lNewAccess.UserType = lUserRole;
                                            }

                                            db.UserAccess.Add(lNewAccess);
                                        }

                                        db.SaveChanges();

                                        lValidUser = true;

                                    }
                                }
                            }
                            else
                            {
                                lValidUser = true;
                            }
                        }
                    }
                    if (lValidUser == true)
                    {
                        return Ok(jsonToken);
                    }
                    else
                    {
                        return BadRequest("UnAuthorize");
                    }
                }
                else
                {
                    return BadRequest("UnAuthorize");
                }

                //Uncomment this code while actual implemention

                //return Ok(jsonToken);

                //if (lValidUser == true)
                //{
                //    //return Redirect("/SavedOrders/Index");
                //    return lValidUser;
                //}
                //else
                //{
                //    return Redirect("/Home/RegNeed");
                //}
            }
            catch (Exception ex)
            {
                var lErrorMsg = ex.Message;
                return BadRequest("UnAuthorize");

                //return BadRequest(lSSOUrl);
            }
        }


        


    }


}
