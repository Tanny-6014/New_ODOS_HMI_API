using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;
using OrderService.Models;
using System.Data.SqlClient;
using Microsoft.Exchange.WebServices.Data;
using OrderService.Controllers;
using Microsoft.Identity.Client;
using System.Configuration;
using System.IdentityModel;
//using Microsoft.Online.SharePoint.TenantAdministration;
//using Microsoft.Owin.Security.Twitter.Messages;
using OrderService.Controllers;
using OrderService.Models;
using System.Net.Mail;
using Oracle.ManagedDataAccess.Client;

namespace OrderService.Repositories
{
    public class SendGridEmail
    {
        private const string sLoginID = "DigiOSNoReply@natsteel.com.sg";
        private const string sLoginPwd = "NatSteel@123B";

        //public async Task Execute(string EmailFromAddress, string EmailFromName, string EmailTo, string EmailCc, string Subject, string Type, string Content)
        public void SendEmailWithAttachments(string EmailTo, string EmailCc, string Subject, string Content, List<string> AttFileName, List<byte[]> AttFileContent)
        {
            //Microsoft Exchange Reast API
            //Web Credential does not work since 2023-04-06 8:00AM
            //ExchangeService myservice = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
            //myservice.Credentials = new WebCredentials(sLoginID, sLoginPwd);

            // Using Microsoft.Identity.Client 4.22.0
            //var cca = ConfidentialClientApplicationBuilder
            //    //.Create(ConfigurationManager.AppSettings["appId"])
            //    //.WithClientSecret(ConfigurationManager.AppSettings["clientSecret"])
            //    //.WithTenantId(ConfigurationManager.AppSettings["tenantId"])
            //    .Create("3975ae35-567f-47f2-b270-52ebad3e92a0")
            //    .WithClientSecret("OPl8Q~Zb5pBtiTf3MR.kB6ZSz4EVQHmREHIRXaKu")
            //    .WithTenantId("e678edbd-9359-4ccb-9dfa-1c7641b172d8")
            //    .Build();

            var pcaOptions = new PublicClientApplicationOptions
            {
                ClientId = "3975ae35-567f-47f2-b270-52ebad3e92a0",
                TenantId = "e678edbd-9359-4ccb-9dfa-1c7641b172d8"
            };

            var pca = PublicClientApplicationBuilder
                .CreateWithApplicationOptions(pcaOptions).Build();

            var ewsScopes = new string[] { "https://outlook.office365.com/.default" };

            try
            {
                //var authResult = await cca.AcquireTokenForClient(ewsScopes).ExecuteAsync();

                IConfidentialClientApplication app;

                app = ConfidentialClientApplicationBuilder.Create("3975ae35-567f-47f2-b270-52ebad3e92a0")
                           .WithClientSecret("4Zm8Q~kon_tyqDBiAhl_1o54F1~RybB.rsrpvc-s")
                           .WithAuthority(new Uri($"https://login.microsoftonline.com/e678edbd-9359-4ccb-9dfa-1c7641b172d8"))
                .Build();

                //var lAC = await app.GetAccountsAsync();

                var result = app.AcquireTokenForClient(ewsScopes)
                                  .ExecuteAsync().Result;

                // Configure the ExchangeService with the access token

                var ewsClient = new ExchangeService();
                ewsClient.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");
                //ewsClient.Credentials = new OAuthCredentials(authResult.AccessToken);
                ewsClient.Credentials = new OAuthCredentials(result.AccessToken);
                ewsClient.ImpersonatedUserId =
                    new ImpersonatedUserId(ConnectingIdType.SmtpAddress, sLoginID);
                //new ImpersonatedUserId(ConnectingIdType.SmtpAddress, "meganb@contoso.onmicrosoft.com");

                //Include x-anchormailbox header
                ewsClient.HttpHeaders.Add("X-AnchorMailbox", sLoginID);
                //ewsClient.HttpHeaders.Add("X-AnchorMailbox", "meganb@contoso.onmicrosoft.com");

                string lEmailBK = "";
                //string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";

                //myservice.Url = new Uri(serviceUrl);

                EmailMessage emailMessage = new EmailMessage(ewsClient);
                emailMessage.Subject = Subject;
                emailMessage.Body = new MessageBody(Content);

                EmailTo = EmailTo.Trim();
                EmailCc = EmailCc.Trim();

                if (EmailTo.Length > 0 || EmailCc.Length > 0)
                {
                    if (EmailTo.Length == 0 && EmailCc.Length > 0)
                    {
                        EmailTo = EmailCc;
                        EmailCc = "";
                    }

                    var laTo = EmailTo.Split(';').Distinct().ToArray();
                    if (laTo.Length > 0)
                    {
                        for (int i = 0; i < laTo.Length; i++)
                        {
                            laTo[i] = laTo[i].Replace(" ", "");
                            if (laTo[i].Length > 0 && laTo[i].IndexOf("@") > 0 && (lEmailBK == "" || lEmailBK.IndexOf(laTo[i]) < 0))
                            {
                                emailMessage.ToRecipients.Add(laTo[i].Trim());
                                lEmailBK = lEmailBK + ";" + laTo[i].Trim();
                            }
                        }
                    }

                    if (EmailCc.Length > 0)
                    {
                        var laCC = EmailCc.Split(';').Distinct().ToArray();
                        if (laCC.Length > 0)
                        {
                            for (int i = 0; i < laCC.Length; i++)
                            {
                                laCC[i] = laCC[i].Replace(" ", "");
                                if (laCC[i].Length > 0 && laCC[i].IndexOf("@") > 0 && (lEmailBK == "" || lEmailBK.IndexOf(laCC[i]) < 0))
                                {
                                    emailMessage.CcRecipients.Add(laCC[i].Trim());
                                    lEmailBK = lEmailBK + ";" + laCC[i].Trim();
                                }
                            }
                        }
                    }

                    if (AttFileName != null && AttFileName.Count > 0 && AttFileContent != null && AttFileContent.Count > 0)
                    {
                        for (int i = 0; i < AttFileContent.Count; i++)
                        {
                            if (AttFileContent[i] != null && AttFileContent[i].Length > 0 && AttFileName[i] != null && AttFileName[i].Length > 0)
                            {
                                emailMessage.Attachments.AddFileAttachment(AttFileName[i], AttFileContent[i]);
                            }
                        }
                    }

                    if (emailMessage.ToRecipients.Count > 0 || emailMessage.CcRecipients.Count > 0)
                    {
                        //emailMessage.Send();
                        emailMessage.SendAndSaveCopy();
                    }
                }

            }
            catch (Exception exception)
            {
                string lExchMsg = "Mail cannot be sent(AutodiscoverRemoteException):";
                lExchMsg += exception.Message;
                throw new Exception(lExchMsg);
            }

        }

        public async System.Threading.Tasks.Task Execute(string EmailFromAddress, string EmailFromName, string EmailTo, string EmailCc, string Subject, string Type, string Content)
        {
            #region GridMail - No more use
            ////string apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY", EnvironmentVariableTarget.User);
            //string apiKey = "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";
            //var sg = new SendGridClient(apiKey);
            //string lEmailBK = "";
            ////Email from = new Email("test@example.com");
            ////string subject = "Hello World from the SendGrid CSharp Library!";
            ////Email to = new Email("test@example.com");
            ////Content content = new Content("text/plain", "Hello, Email!");

            ////Attachment attachment = new Attachment();
            ////attachment.Content = "TG9yZW0gaXBzdW0gZG9sb3Igc2l0IGFtZXQsIGNvbnNlY3RldHVyIGFkaXBpc2NpbmcgZWxpdC4gQ3JhcyBwdW12";
            ////attachment.Type = "application/pdf";
            ////attachment.Filename = "balance_001.pdf";
            ////attachment.Disposition = "attachment";
            ////attachment.ContentId = "Balance Sheet";
            ////mail.AddAttachment(attachment);


            ////attachment = new Attachment();
            ////attachment.Content = "BwdW";
            ////attachment.Type = "image/png";
            ////attachment.Filename = "banner.png";
            ////attachment.Disposition = "inline";
            ////attachment.ContentId = "Banner";
            ////mail.AddAttachment(attachment);

            ////Email from = new Email(EmailFromAddress);
            ////from.Name = EmailFromName;

            ////string subject = Subject;
            ////Content content = new Content(Type, Content);
            ////Email to = new Email(EmailTo);

            ////var laTo = EmailTo.Split(';').Distinct().ToArray();
            ////if (laTo.Length > 0)
            ////{
            ////    for (int i = 0; i < laTo.Length; i++)
            ////    {
            ////        Email lTo = new Email(laTo[i]);
            ////        Mail mail = new Mail(from, Subject, lTo, content);
            ////        dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
            ////    }
            ////}

            //var msg = new SendGridMessage();
            //msg.SetFrom(new EmailAddress(EmailFromAddress, EmailFromName));

            //msg.SetSubject(Subject);
            //if (Type == "text/html")
            //{
            //    msg.AddContent(MimeType.Html, Content);
            //}
            //else
            //{
            //    msg.AddContent(MimeType.Text, Content);
            //}

            //if (EmailTo == null)
            //{
            //    EmailTo = "";
            //}
            //if (EmailCc == null)
            //{
            //    EmailCc = "";
            //}

            //EmailTo = EmailTo.Trim();
            //EmailCc = EmailCc.Trim();

            //if (EmailTo.Length > 0 || EmailCc.Length > 0)
            //{
            //    if (EmailTo.Length == 0 && EmailCc.Length > 0)
            //    {
            //        EmailTo = EmailCc;
            //        EmailCc = "";
            //    }

            //    var laTo = EmailTo.Split(';').Distinct().ToArray();
            //    if (laTo.Length > 0)
            //    {
            //        for (int i = 0; i < laTo.Length; i++)
            //        {
            //            if (laTo[i].Length > 0 && (lEmailBK == "" || lEmailBK.IndexOf(laTo[i]) < 0))
            //            {
            //                msg.AddTo(new EmailAddress(laTo[i]));
            //                lEmailBK = lEmailBK + ";" + laTo[i];
            //            }
            //        }
            //    }

            //    if (EmailCc.Length > 0)
            //    {
            //        var laCC = EmailCc.Split(';').Distinct().ToArray();
            //        if (laCC.Length > 0)
            //        {
            //            for (int i = 0; i < laCC.Length; i++)
            //            {
            //                if (laCC[i].Length > 0 && (lEmailBK == "" || lEmailBK.IndexOf(laCC[i]) < 0))
            //                {
            //                    msg.AddCc(new EmailAddress(laCC[i]));
            //                    lEmailBK = lEmailBK + ";" + laCC[i];
            //                }
            //            }
            //        }

            //    }
            //    var response = await sg.SendEmailAsync(msg);
            //    var lReturnCode = response.StatusCode;
            //}

            #endregion

            //Microsoft Exchange Reast API
            //Web Credential does not work since 2023-04-06 8:00AM
            //ExchangeService myservice = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
            //myservice.Credentials = new WebCredentials(sLoginID, sLoginPwd);

            // Using Microsoft.Identity.Client 4.22.0
            //var cca = ConfidentialClientApplicationBuilder
            //    //.Create(ConfigurationManager.AppSettings["appId"])
            //    //.WithClientSecret(ConfigurationManager.AppSettings["clientSecret"])
            //    //.WithTenantId(ConfigurationManager.AppSettings["tenantId"])
            //    .Create("3975ae35-567f-47f2-b270-52ebad3e92a0")
            //    .WithClientSecret("9IL8Q~AxyT53gtCSEoVS2IXZH_5yIfxPiBqRCaHD")
            //    .WithTenantId("e678edbd-9359-4ccb-9dfa-1c7641b172d8")
            //    .Build();

            // Configure the MSAL client to get tokens
            var pcaOptions = new PublicClientApplicationOptions
            {
                ClientId = "3975ae35-567f-47f2-b270-52ebad3e92a0",
                TenantId = "e678edbd-9359-4ccb-9dfa-1c7641b172d8"
            };

            var pca = PublicClientApplicationBuilder
                .CreateWithApplicationOptions(pcaOptions).Build();

            var ewsScopes = new string[] { "https://outlook.office365.com/.default" };
            //var ewsScopes = new string[] { "https://outlook.office365.com/EWS.AccessAsUser.All" };
            //var ewsScopes = new List<string> { "https://graph.microsoft.com/Mail.Read" };

            try
            {
                //var authResult = await cca.AcquireTokenForClient(ewsScopes).ExecuteAsync();
                //var authResult = await pca.AcquireTokenInteractive(ewsScopes).ExecuteAsync();

                IConfidentialClientApplication app;

                app = ConfidentialClientApplicationBuilder.Create("3975ae35-567f-47f2-b270-52ebad3e92a0")
                           .WithClientSecret("4Zm8Q~kon_tyqDBiAhl_1o54F1~RybB.rsrpvc-s")
                           .WithAuthority(new Uri($"https://login.microsoftonline.com/e678edbd-9359-4ccb-9dfa-1c7641b172d8"))
                .Build();

                //var lAC = await app.GetAccountsAsync();

                var result = app.AcquireTokenForClient(ewsScopes)
                                  .ExecuteAsync().Result;

                //var client = PublicClientApplicationBuilder.Create("3975ae35-567f-47f2-b270-52ebad3e92a0")
                //.WithClientSecret("9IL8Q~AxyT53gtCSEoVS2IXZH_5yIfxPiBqRCaHD")
                //.WithAuthority(new Uri($"https://login.microsoftonline.com/e678edbd-9359-4ccb-9dfa-1c7641b172d8"))
                //.Build();

                //var request = client.AcquireTokenByUsernamePassword(ewsScopes, sLoginID, sLoginPwd);
                //var result = request.ExecuteAsync().Result;

                // Configure the ExchangeService with the access token
                var ewsClient = new ExchangeService();
                ewsClient.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");

                ewsClient.Credentials = new OAuthCredentials(result.AccessToken);

                ewsClient.ImpersonatedUserId =
                    new ImpersonatedUserId(ConnectingIdType.SmtpAddress, sLoginID);

                ////Include x-anchormailbox header
                ewsClient.HttpHeaders.Add("X-AnchorMailbox", sLoginID);

                //Include Token header
                //ewsClient.HttpHeaders.Add("Authorization", "bearer " + result.AccessToken);
                //ewsClient.HttpHeaders.Add("appid", "3975ae35-567f-47f2-b270-52ebad3e92a0");
                //ewsClient.HttpHeaders.Add("roles", "3975ae35-567f-47f2-b270-52ebad3e92a0");

                string lEmailBK = "";

                //string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
                //myservice.Url = new Uri(serviceUrl);

                EmailMessage emailMessage = new EmailMessage(ewsClient);

                emailMessage.Subject = Subject;
                emailMessage.Body = new MessageBody(Content);

                EmailTo = EmailTo.Trim();
                EmailCc = EmailCc.Trim();

                if (EmailTo.Length > 0 || EmailCc.Length > 0)
                {
                    if (EmailTo.Length == 0 && EmailCc.Length > 0)
                    {
                        EmailTo = EmailCc;
                        EmailCc = "";
                    }

                    var laTo = EmailTo.Split(';').Distinct().ToArray();
                    if (laTo.Length > 0)
                    {
                        for (int i = 0; i < laTo.Length; i++)
                        {
                            laTo[i] = laTo[i].Trim();
                            if (laTo[i].Length > 5 && laTo[i].IndexOf("@") > 0 && laTo[i].IndexOf(".") > 0
                                && laTo[i].Substring(0, 1) != "@"
                                && laTo[i].Substring(0, 1) != "."
                                && laTo[i].Substring(laTo[i].Length - 1, 1) != "@"
                                && laTo[i].Substring(laTo[i].Length - 1, 1) != "."
                                && (lEmailBK == "" || lEmailBK.IndexOf(laTo[i]) < 0))
                            {
                                emailMessage.ToRecipients.Add(laTo[i]);
                                lEmailBK = lEmailBK + ";" + laTo[i];
                            }
                        }
                    }

                    if (EmailCc.Length > 0)
                    {
                        var laCC = EmailCc.Split(';').Distinct().ToArray();
                        if (laCC.Length > 0)
                        {
                            for (int i = 0; i < laCC.Length; i++)
                            {
                                laCC[i] = laCC[i].Trim();
                                if (laCC[i].Length > 5 && laCC[i].IndexOf("@") > 0 && laCC[i].IndexOf(".") > 0
                                    && laCC[i].Substring(0, 1) != "@"
                                    && laCC[i].Substring(0, 1) != "."
                                    && laCC[i].Substring(laCC[i].Length - 1, 1) != "@"
                                    && laCC[i].Substring(laCC[i].Length - 1, 1) != "."
                                    && (lEmailBK == "" || lEmailBK.IndexOf(laCC[i]) < 0))
                                {
                                    emailMessage.CcRecipients.Add(laCC[i]);
                                    lEmailBK = lEmailBK + ";" + laCC[i];
                                }
                            }
                        }
                    }
                    if (emailMessage.ToRecipients.Count > 0 || emailMessage.CcRecipients.Count > 0)
                    {
                        //emailMessage.Send();
                        emailMessage.SendAndSaveCopy();
                    }
                }

            }
            catch (Exception exception)
            {
                string lExchMsg = "Mail cannot be sent(AutodiscoverRemoteException):";
                lExchMsg += exception.Message;
                //throw new Exception(lExchMsg);
            }

        }

        public bool sendOrderActionEmail(string CustomerCode, string ProjectCode, int OrderNumber, string OrderAction, string OrderStatus, string UserID, int AllItems, string StructureElement, string ProductType, string ScheduledProd)
        {
            var lReturn = true;

            var lEmailContent = "";
            var lEmailFrom = "";
            var lEmailTo = "";
            var lEmailCc = "";
            var lEmailSubject = "";
            string lVar1 = "";
            string lProducts = "";
            string lPOs = "";
            string lServer = "DEV";
            string lUpdatedBy = "";
            int lDetailingProd = 0;

            var lProcessObj = new ProcessController();
            lServer = lProcessObj.strServer;
            lProcessObj = null;

            DBContextModels db = new DBContextModels();

            var JobContent = db.OrderProject.Find(OrderNumber);
            if (JobContent != null)
            {
                if (JobContent.CustomerCode == null) JobContent.CustomerCode = "";
                else JobContent.CustomerCode = JobContent.CustomerCode.Trim();

                if (JobContent.OrderStatus == null) JobContent.OrderStatus = "";
                else JobContent.OrderStatus = JobContent.OrderStatus.Trim();

                if (JobContent.PONumber == null) JobContent.PONumber = "";
                else JobContent.PONumber = JobContent.PONumber.Trim();

                if (JobContent.ProjectCode == null) JobContent.ProjectCode = "";
                else JobContent.ProjectCode = JobContent.ProjectCode.Trim();

                if (JobContent.DeliveryAddress == null) JobContent.DeliveryAddress = "";
                else JobContent.DeliveryAddress = JobContent.DeliveryAddress.Trim();

                if (JobContent.Remarks == null) JobContent.Remarks = "";
                else JobContent.Remarks = JobContent.Remarks.Trim();

                if (JobContent.Scheduler_HP == null) JobContent.Scheduler_HP = "";
                else JobContent.Scheduler_HP = JobContent.Scheduler_HP.Trim();

                if (JobContent.Scheduler_Name == null) JobContent.Scheduler_Name = "";
                else JobContent.Scheduler_Name = JobContent.Scheduler_Name.Trim();

                if (JobContent.Scheduler_Email == null) JobContent.Scheduler_Email = "";
                else JobContent.Scheduler_Email = JobContent.Scheduler_Email.Trim();

                if (JobContent.SiteEngr_HP == null) JobContent.SiteEngr_HP = "";
                else JobContent.SiteEngr_HP = JobContent.SiteEngr_HP.Trim();

                if (JobContent.SiteEngr_Name == null) JobContent.SiteEngr_Name = "";
                else JobContent.SiteEngr_Name = JobContent.SiteEngr_Name.Trim();

                if (JobContent.SiteEngr_Email == null) JobContent.SiteEngr_Email = "";
                else JobContent.SiteEngr_Email = JobContent.SiteEngr_Email.Trim();

                if (JobContent.TransportMode == null) JobContent.TransportMode = "";
                else JobContent.TransportMode = JobContent.TransportMode.Trim();

                if (JobContent.WBS1 == null) JobContent.WBS1 = "";
                else JobContent.WBS1 = JobContent.WBS1.Trim();

                if (JobContent.WBS2 == null) JobContent.WBS2 = "";
                else JobContent.WBS2 = JobContent.WBS2.Trim();

                if (JobContent.WBS3 == null) JobContent.WBS3 = "";
                else JobContent.WBS3 = JobContent.WBS3.Trim();

                lUpdatedBy = JobContent.UpdateBy == null ? "" : JobContent.UpdateBy;
            }

            if (lUpdatedBy.ToLower().IndexOf("natsteel.com.sg") > 0 && OrderAction == "Submitted")
            {
                return lReturn;
            }
            lEmailContent = "Serial No.: " + JobContent.OrderNumber.ToString() + "<br/>";

            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
            lEmailContent = lEmailContent + "<td width=20%>" + "Customer (客户名称)" + "</td>";

            CustomerModels lCustomer = db.Customer.Find(JobContent.CustomerCode);
            string lVar = "";
            if (lCustomer != null) lVar = lCustomer.CustomerName.Trim() + " (" + JobContent.CustomerCode.Trim() + ")";
            lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr>";

            lEmailContent = lEmailContent + "<tr><td>" + "Project (工程项目)" + "</td>";

            var lProject = (from p in db.Project
                            where p.ProjectCode == ProjectCode
                            orderby p.ProjectTitle descending
                            select p).First();
            lVar = "";
            if (lProject != null && lProject.ProjectTitle != null && JobContent.ProjectCode != null) lVar = lProject.ProjectTitle.Trim() + " (" + JobContent.ProjectCode.Trim() + ")";
            lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr></table>";

            if (JobContent.OrderType == "WBS")
            {
                lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                lEmailContent = lEmailContent + "<td width='20%'>" + "Block (WBS1) (座号/大牌)" + "</td>";
                lEmailContent = lEmailContent + "<td width=15%><strong>" + JobContent.WBS1.Trim() + "</strong></td>";
                lEmailContent = lEmailContent + "<td width='17%'>" + "Storey (WBS2) (楼层)" + "</td>";
                lEmailContent = lEmailContent + "<td width=14%><strong>" + JobContent.WBS2.Trim() + "</strong></td>";
                lEmailContent = lEmailContent + "<td width=16%>" + "Part (WBS3) (分部)" + "</td>";
                lEmailContent = lEmailContent + "<td><strong>" + JobContent.WBS3.Trim() + "</strong></td></tr></table>";
            }


            var lOrderSE = (from p in db.OrderProjectSE
                            where p.OrderNumber == OrderNumber
                            select p).ToList();

            if (AllItems == 0)
            {
                lOrderSE = (from p in db.OrderProjectSE
                            where p.OrderNumber == OrderNumber &&
                            p.StructureElement == StructureElement &&
                            p.ProductType == ProductType &&
                            p.ScheduledProd == ScheduledProd
                            select p).ToList();
            }

            if (lOrderSE.Count > 0)
            {
                for (int i = 0; i < lOrderSE.Count; i++)
                {
                    if (lProducts == "")
                    {
                        lProducts = lOrderSE[i].ProductType.Trim();
                    }
                    else
                    {
                        if (lProducts.IndexOf(lOrderSE[i].ProductType.Trim()) < 0)
                        {
                            lProducts = lProducts + "/" + lOrderSE[i].ProductType.Trim();
                        }
                    }

                    if (lPOs == "")
                    {
                        lPOs = lOrderSE[i].PONumber;
                        if (lPOs == null)
                        {
                            lPOs = "";
                        }
                        lPOs = lPOs.Trim();
                    }
                    else
                    {
                        if (lPOs.IndexOf(lOrderSE[i].PONumber.Trim()) < 0)
                        {
                            lPOs = lPOs + "/" + lOrderSE[i].PONumber.Trim();
                        }
                    }

                    lEmailContent = lEmailContent + "<table border='1' width=100%>";
                    if (JobContent.OrderType == "WBS")
                    {
                        lEmailContent = lEmailContent + "<tr>";
                        lEmailContent = lEmailContent + "<td width='20%'>" + "Structure Element (构件)" + "</td>";
                        lEmailContent = lEmailContent + "<td>" + lOrderSE[i].StructureElement + "</td>";
                        lEmailContent = lEmailContent + "</tr>";
                    }
                    lEmailContent = lEmailContent + "<tr>";
                    lEmailContent = lEmailContent + "<td width='20%'> " + "Product Type (产品类型)" + "</td>";
                    lEmailContent = lEmailContent + "<td>" + lOrderSE[i].ProductType + "</td>";
                    lEmailContent = lEmailContent + "</tr>";
                    lEmailContent = lEmailContent + "<tr>";
                    lEmailContent = lEmailContent + "<td>" + "Order Weight (KG) (订单重量)" + "</td>";
                    lEmailContent = lEmailContent + "<td><strong>" + (lOrderSE[i].TotalWeight == null ? "0.000" : ((decimal)lOrderSE[i].TotalWeight).ToString("###,##0.000")) + "</strong></td>";
                    lEmailContent = lEmailContent + "</tr>";
                    lEmailContent = lEmailContent + "<tr>";
                    lEmailContent = lEmailContent + "<td>" + "PO Number (订单号码)" + "</td>";
                    lEmailContent = lEmailContent + "<td>" + lOrderSE[i].PONumber + "</td>";
                    lEmailContent = lEmailContent + "</tr>";
                    lEmailContent = lEmailContent + "<tr>";
                    lEmailContent = lEmailContent + "<td>" + "Required Date (到场日期)" + "</td>";
                    lEmailContent = lEmailContent + "<td>" + (lOrderSE[i].RequiredDate == null ? "" : ((DateTime)lOrderSE[i].RequiredDate).ToString("yyyy-MM-dd")) + "</td>";
                    lEmailContent = lEmailContent + "</tr>";
                    lEmailContent = lEmailContent + "<tr>";
                    lEmailContent = lEmailContent + "<td>" + "Transport (运输)" + "</td>";
                    lEmailContent = lEmailContent + "<td>" + lOrderSE[i].TransportMode + "</td>";
                    lEmailContent = lEmailContent + "</tr>";
                    if (lOrderSE[i].SAPSOR != null || lOrderSE[i].SAPSOR != "")
                    {
                        lEmailContent = lEmailContent + "<tr>";
                        lEmailContent = lEmailContent + "<td>" + "Order Ref No. (订单参号)" + "</td>";
                        lEmailContent = lEmailContent + "<td>" + lOrderSE[i].SAPSOR + "</td>";
                        lEmailContent = lEmailContent + "</tr>";
                    }

                    lEmailContent = lEmailContent + "<tr><td colspan=2>";
                    if (lOrderSE[i].ProductType == "CAB")
                    {
                        var lJobID = lOrderSE[i].CABJobID;
                        var JobCABContent = db.JobAdvice.Find(CustomerCode, ProjectCode, lJobID);
                        if (JobCABContent.TotalCABWeight > 0)
                        {
                            var lBBSContent = (from p in db.BBS
                                               where p.CustomerCode == CustomerCode &&
                                               p.ProjectCode == ProjectCode &&
                                               p.JobID == lJobID &&
                                               p.BBSOrderCABWT + p.BBSOrderSTDWT > 0
                                               orderby p.BBSID
                                               select p).ToList();

                            if (lBBSContent.Count > 0)
                            {
                                var lPrintSOR = 0;
                                for (int j = 0; j < lBBSContent.Count; j++)
                                {
                                    if ((lBBSContent[j].BBSSOR != null && lBBSContent[j].BBSSOR != "") ||
                                        (lBBSContent[j].BBSSORCoupler != null && lBBSContent[j].BBSSORCoupler != "") ||
                                        (lBBSContent[j].BBSSAPSO != null && lBBSContent[j].BBSSAPSO != ""))
                                    {
                                        lPrintSOR = 1;
                                        break;
                                    }
                                }

                                lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                                lEmailContent = lEmailContent + "<td width='20%'>" + "BBS No. (钢筋加工表)" + "</td>";
                                lEmailContent = lEmailContent + "<td width='25%'>" + "BBS Description (具体描述)" + "</td>";
                                lEmailContent = lEmailContent + "<td width='18%'>" + "CAB Weight (KG) (加工铁重量)" + "</td>";
                                lEmailContent = lEmailContent + "<td width='18%'>" + "SB Weight (KG) (标准直铁重量)" + "</td>";
                                lEmailContent = lEmailContent + "<td>" + "Total Weight (KG) (总重量)" + "</td>";
                                if (lPrintSOR == 1)
                                {
                                    lEmailContent = lEmailContent + "<td>" + "Order Ref No. (订单参号)" + "</td>";
                                }
                                lEmailContent = lEmailContent + "</tr>";

                                for (int j = 0; j < lBBSContent.Count; j++)
                                {
                                    lEmailContent = lEmailContent + "<tr><td> <font color='blue'>" + lBBSContent[j].BBSNo + "</font></td>";
                                    lEmailContent = lEmailContent + "<td>" + lBBSContent[j].BBSDesc + "</td>";
                                    if (lBBSContent[j].BBSOrderCABWT == 0) lVar = ""; else lVar = lBBSContent[j].BBSOrderCABWT.ToString("F3");
                                    lEmailContent = lEmailContent + "<td align='left'>" + lVar + "</td>";
                                    if (lBBSContent[j].BBSOrderSTDWT == 0) lVar = ""; else lVar = lBBSContent[j].BBSOrderSTDWT.ToString("F3");
                                    lEmailContent = lEmailContent + "<td align='left'>" + lVar + "</td>";
                                    lEmailContent = lEmailContent + "<td align='left'>" + (lBBSContent[j].BBSOrderCABWT + lBBSContent[j].BBSOrderSTDWT).ToString("F3") + "</td>";
                                    if (lPrintSOR == 1)
                                    {
                                        string lSOR = "";
                                        if (lBBSContent[j].BBSSOR != null && lBBSContent[j].BBSSOR != "")
                                        {
                                            if (lSOR == "")
                                            {
                                                lSOR = lBBSContent[j].BBSSOR;
                                            }
                                            else
                                            {
                                                lSOR = lSOR + ", " + lBBSContent[j].BBSSOR;
                                            }
                                        }
                                        if (lBBSContent[j].BBSSORCoupler != null && lBBSContent[j].BBSSORCoupler != "")
                                        {
                                            if (lSOR == "")
                                            {
                                                lSOR = lBBSContent[j].BBSSORCoupler;
                                            }
                                            else
                                            {
                                                lSOR = lSOR + ", " + lBBSContent[j].BBSSORCoupler;
                                            }
                                        }
                                        if (lBBSContent[j].BBSSAPSO != null && lBBSContent[j].BBSSAPSO != "")
                                        {
                                            if (lSOR == "")
                                            {
                                                lSOR = lBBSContent[j].BBSSAPSO;
                                            }
                                            else
                                            {
                                                lSOR = lSOR + ", " + lBBSContent[j].BBSSAPSO;
                                            }
                                        }
                                        lEmailContent = lEmailContent + "<td align='left'>" + lSOR + "</td>";
                                    }

                                    lEmailContent = lEmailContent + "</tr>";
                                }

                                lEmailContent = lEmailContent + "</table>";
                            }
                        }
                        else
                        {
                            var lBBSSO = (from p in db.BBS
                                          where p.CustomerCode == CustomerCode &&
                                          p.ProjectCode == ProjectCode &&
                                          p.JobID == lJobID &&
                                          p.BBSOrderCABWT + p.BBSOrderSTDWT > 0
                                          orderby p.BBSID
                                          select p).ToList();
                            string lSORef = "";
                            if (lBBSSO != null && lBBSSO.Count > 0)
                            {
                                lSORef = lBBSSO[0].BBSSAPSO;
                            }

                            lEmailContent = lEmailContent + "<table border='1' width=100%>";
                            lEmailContent = lEmailContent + "<tr><td width='5%'>" + "S/N\n序号" + "</td>";
                            lEmailContent = lEmailContent + "<td width='20%' align='ceneter'>" + "Bar Type\n(钢筋型号)" + "</td>";
                            lEmailContent = lEmailContent + "<td width='20%' align='ceneter'>" + "Size\n(直径)" + "</td>";
                            lEmailContent = lEmailContent + "<td width='20%' align='ceneter'>" + "Bar Length\n(长度)" + "</td>";
                            lEmailContent = lEmailContent + "<td width='20%' align='ceneter'>" + "No. of Bundles\n(捆数)" + "</td>";
                            lEmailContent = lEmailContent + "<td  align='ceneter'>" + "Total Weight\n(总重量)" + "</td>";
                            if (lSORef != null && lSORef != "")
                            {
                                lEmailContent = lEmailContent + "<td  align='ceneter'>" + "Order Ref No.\n(订单参号)" + "</td>";
                            }
                            lEmailContent = lEmailContent + "</tr>";

                            var lBBSContent = (from p in db.OrderDetails
                                               where p.CustomerCode == CustomerCode &&
                                               p.ProjectCode == ProjectCode &&
                                               p.JobID == lJobID &&
                                               p.BarSTD == true
                                               orderby p.BBSID
                                               select p).ToList();

                            if (lBBSContent.Count > 0)
                            {
                                for (int j = 0; j < lBBSContent.Count; j++)
                                {
                                    lEmailContent = lEmailContent + "<tr><td align='ceneter'>" + (j + 1).ToString() + "</td>";
                                    lEmailContent = lEmailContent + "<td  align='ceneter'>" + lBBSContent[j].BarType + "</td>";
                                    lEmailContent = lEmailContent + "<td  align='ceneter'>" + lBBSContent[j].BarSize + "</td>";
                                    lEmailContent = lEmailContent + "<td  align='ceneter'>" + lBBSContent[j].BarLength + "</td>";
                                    lEmailContent = lEmailContent + "<td  align='ceneter'>" + lBBSContent[j].BarMemberQty + "</td>";
                                    lEmailContent = lEmailContent + "<td  align='ceneter'>" + ((decimal)lBBSContent[j].BarWeight).ToString("F0") + "</td>";
                                    if (lSORef != null && lSORef != "")
                                    {
                                        lEmailContent = lEmailContent + "<td  align='ceneter'>" + lSORef + "</td>";
                                    }
                                    lEmailContent = lEmailContent + "</tr>";
                                }
                            }
                            lEmailContent = lEmailContent + "</table>";
                        }
                    }
                    else if (lOrderSE[i].ProductType == "STANDARD-BAR")
                    {
                        var lJobID = lOrderSE[i].StdBarsJobID;
                        var lStdContent = (from p in db.StdProdDetails
                                           where p.CustomerCode == CustomerCode &&
                                           p.ProjectCode == ProjectCode &&
                                           p.JobID == lJobID
                                           orderby p.SSID
                                           select p).ToList();

                        if (lStdContent.Count > 0)
                        {
                            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                            lEmailContent = lEmailContent + "<td width='5%'>" + "S/N<br/>序号" + "</td>";
                            lEmailContent = lEmailContent + "<td width='15%'>" + "Product Code<br/>产品代码" + "</td>";
                            lEmailContent = lEmailContent + "<td width='30%'>" + "Product Description<br/>产品概述" + "</td>";
                            lEmailContent = lEmailContent + "<td width='10%'>" + "Grade<br/>类型" + "</td>";
                            lEmailContent = lEmailContent + "<td width='10%'>" + "Diameter<br/>直径" + "</td>";
                            lEmailContent = lEmailContent + "<td width='10%'>" + "Unit Weight<br/>单位重量" + "</td>";
                            lEmailContent = lEmailContent + "<td width='10%'>" + "Order Qty<br/>订购件数" + "</td>";
                            lEmailContent = lEmailContent + "<td>" + "Total Weight<br/>总重量" + "</td>";
                            lEmailContent = lEmailContent + "</tr>";


                            for (int j = 0; j < lStdContent.Count; j++)
                            {
                                lEmailContent = lEmailContent + "<tr><td align='center'> <font color='blue'>" + (j + 1).ToString() + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'><strong>" + lStdContent[j].ProdCode + "</strong></font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lStdContent[j].ProdDesc + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lStdContent[j].Grade + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lStdContent[j].Diameter.ToString() + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lStdContent[j].UnitWT.ToString("F0") + "</font></td>";
                                if (lStdContent[j].order_pcs == 0) lVar = ""; else lVar = lStdContent[j].order_pcs.ToString();
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'><strong>" + lVar + "</strong></font></td>";
                                if (lStdContent[j].order_wt == 0) lVar = ""; else lVar = lStdContent[j].order_wt.ToString("F0");
                                lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lVar + "</font></td>";
                                lEmailContent = lEmailContent + "</tr>";
                            }
                            lEmailContent = lEmailContent + "</table>";
                        }

                    }
                    else if (lOrderSE[i].ProductType == "STANDARD-MESH")
                    {
                        var lJobID = lOrderSE[i].StdMESHJobID;
                        var lBBSContent = (from p in db.StdSheetDetails
                                           where p.CustomerCode == CustomerCode &&
                                           p.ProjectCode == ProjectCode &&
                                           p.JobID == lJobID
                                           orderby p.SheetSort
                                           select p).ToList();

                        if (lBBSContent.Count > 0)
                        {
                            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                            lEmailContent = lEmailContent + "<td colspan='11'>" + "MESH Standard Sheet (Class A)(标准铁网)" + "</td></tr><tr>";
                            lEmailContent = lEmailContent + "<td width='5%'>" + "S/N<br/>序号" + "</td>";
                            lEmailContent = lEmailContent + "<td width='10%'>" + "Product Code<br/>产品代码" + "</td>";
                            lEmailContent = lEmailContent + "<td width='8%'>" + "Main Length<br/>主筋长" + "</td>";
                            lEmailContent = lEmailContent + "<td width='8%'>" + "Cross Length<br/>副筋长" + "</td>";
                            lEmailContent = lEmailContent + "<td width='9%'>" + "MW Size<br/>主筋直径" + "</td>";
                            lEmailContent = lEmailContent + "<td width='10%'>" + "MW Spacing<br/>主筋间距" + "</td>";
                            lEmailContent = lEmailContent + "<td width='9%'>" + "CW Size<br/>副筋直径" + "</td>";
                            lEmailContent = lEmailContent + "<td width='10%'>" + "CW Spacing<br/>副筋间距" + "</td>";
                            lEmailContent = lEmailContent + "<td width='10%'>" + "Unit Weight<br/>单片重量" + "</td>";
                            lEmailContent = lEmailContent + "<td width='10%'>" + "Order Qty<br/>订购件数" + "</td>";
                            lEmailContent = lEmailContent + "<td>" + "Total Weight<br/>总重量" + "</td>";
                            lEmailContent = lEmailContent + "</td></tr>";


                            for (int j = 0; j < lBBSContent.Count; j++)
                            {
                                lEmailContent = lEmailContent + "<tr><td align='center'> <font color='blue'>" + (j + 1).ToString() + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[j].sheet_name + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[j].mw_length.ToString() + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[j].cw_length.ToString() + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[j].mw_size.ToString() + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[j].mw_spacing.ToString() + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[j].cw_size.ToString() + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[j].cw_spacing.ToString() + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[j].unit_weight.ToString("F3") + "</font></td>";
                                if (lBBSContent[j].order_pcs == 0) lVar = ""; else lVar = lBBSContent[j].order_pcs.ToString();
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'><strong>" + lVar + "</strong></font></td>";
                                if (lBBSContent[j].order_wt == 0) lVar = ""; else lVar = lBBSContent[j].order_wt.ToString("F3");
                                lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lVar + "</font></td>";
                                lEmailContent = lEmailContent + "</tr>";
                            }
                            lEmailContent = lEmailContent + "</table>";
                        }

                    }
                    else if (lOrderSE[i].ProductType == "COIL")
                    {
                        var lJobID = lOrderSE[i].CoilProdJobID;
                        var lStdContent = (from p in db.StdProdDetails
                                           where p.CustomerCode == CustomerCode &&
                                           p.ProjectCode == ProjectCode &&
                                           p.JobID == lJobID
                                           orderby p.SSID
                                           select p).ToList();

                        if (lStdContent.Count > 0)
                        {
                            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                            lEmailContent = lEmailContent + "<td width='5%'>" + "S/N<br/>序号" + "</td>";
                            lEmailContent = lEmailContent + "<td width='15%'>" + "Product Code<br/>产品代码" + "</td>";
                            lEmailContent = lEmailContent + "<td width='30%'>" + "Product Description<br/>产品概述" + "</td>";
                            lEmailContent = lEmailContent + "<td width='10%'>" + "Diameter<br/>直径" + "</td>";
                            lEmailContent = lEmailContent + "<td width='10%'>" + "Grade<br/>类型" + "</td>";
                            lEmailContent = lEmailContent + "<td width='10%'>" + "Unit Weight<br/>单位重量" + "</td>";
                            lEmailContent = lEmailContent + "<td width='10%'>" + "Order Qty<br/>订购件数" + "</td>";
                            lEmailContent = lEmailContent + "<td>" + "Total Weight<br/>总重量" + "</td>";
                            lEmailContent = lEmailContent + "</td></tr>";


                            for (int j = 0; j < lStdContent.Count; j++)
                            {
                                lEmailContent = lEmailContent + "<tr><td align='center'> <font color='blue'>" + (j + 1).ToString() + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'><strong>" + lStdContent[j].ProdCode + "</strong></font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lStdContent[j].ProdDesc + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lStdContent[j].Diameter.ToString() + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lStdContent[j].Grade + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lStdContent[j].UnitWT.ToString("F0") + "</font></td>";
                                if (lStdContent[j].order_pcs == 0) lVar = ""; else lVar = lStdContent[j].order_pcs.ToString();
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'><strong>" + lVar + "</strong></font></td>";
                                if (lStdContent[j].order_wt == 0) lVar = ""; else lVar = lStdContent[j].order_wt.ToString("F0");
                                lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lVar + "</font></td>";
                                lEmailContent = lEmailContent + "</tr>";
                            }
                            lEmailContent = lEmailContent + "</table>";
                        }
                    }
                    else if (lOrderSE[i].ProductType == "CUT-TO-SIZE-MESH")
                    {
                        lDetailingProd = 1;
                        if (lOrderSE[i].ScheduledProd == "Y")
                        {
                        }
                    }
                    else if (lOrderSE[i].ProductType == "STIRRUP-LINK-MESH")
                    {
                        lDetailingProd = 1;
                        if (lOrderSE[i].ScheduledProd == "Y")
                        {
                        }
                    }
                    else if (lOrderSE[i].ProductType == "COLUMN-LINK-MESH")
                    {
                        lDetailingProd = 1;
                        if (lOrderSE[i].ScheduledProd == "Y")
                        {
                        }
                    }
                    else if (lOrderSE[i].ProductType == "PRE-CAGE")
                    {
                        if (lOrderSE[i].ScheduledProd == "Y")
                        {
                            lDetailingProd = 1;
                        }
                        else
                        {
                        }
                    }
                    else if (lOrderSE[i].ProductType == "CORE-CAGE")
                    {
                        if (lOrderSE[i].ScheduledProd == "Y")
                        {
                            lDetailingProd = 1;
                        }
                        else
                        {
                        }
                    }
                    else if (lOrderSE[i].ProductType == "CARPET")
                    {
                        if (lOrderSE[i].ScheduledProd == "Y")
                        {
                            lDetailingProd = 1;
                        }
                        else
                        {
                        }
                    }
                    else if (lOrderSE[i].ProductType == "BPC")
                    {
                        lDetailingProd = 1;
                        var lJobID = lOrderSE[i].BPCJobID;
                        lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                        lEmailContent = lEmailContent + "<td width='3%'>" + "S/N<br/>序号" + "</td>";
                        lEmailContent = lEmailContent + "<td width='6%'>" + "Pile Diameter<br/>桩直径" + "</td>";
                        lEmailContent = lEmailContent + "<td width='12%'>" + "Cage Type<br/>铁笼类型" + "</td>";
                        lEmailContent = lEmailContent + "<td width='9%'>" + "No of Main Bars<br/>主筋数量" + "</td>";
                        lEmailContent = lEmailContent + "<td width='7%'>" + "Main Bar Shape<br/>主筋图形" + "</td>";
                        lEmailContent = lEmailContent + "<td width='5%'>" + "Cage Length<br/>铁笼长" + "</td>";
                        lEmailContent = lEmailContent + "<td width='5%'>" + "Lap Length<br/>顶端重叠长" + "</td>";
                        lEmailContent = lEmailContent + "<td width='14%'>" + "Link Dia-Spacing-Length<br/>弧状链的直径,间距与长度" + "</td>";
                        lEmailContent = lEmailContent + "<td width='7%'>" + "End Length<br/>底端预留长度" + "</td>";
                        lEmailContent = lEmailContent + "<td width='5%'>" + "Cage Qty<br/>铁笼件数" + "</td>";
                        lEmailContent = lEmailContent + "<td width='11%'>" + "Combination of Cages<br/>上端, 中间还是低端铁笼" + "</td>";
                        lEmailContent = lEmailContent + "<td width='6%'>" + "Weight(MT)<br/>重量(吨)" + "</td>";
                        lEmailContent = lEmailContent + "<td width='5%'>" + "Per Set<br/>每组笼数" + "</td>";
                        lEmailContent = lEmailContent + "<td width='5%'>" + "Cage Mark<br/>铁笼号码" + "</td>";
                        lEmailContent = lEmailContent + "<td>" + "Remarks<br/>备注" + "</td>";
                        lEmailContent = lEmailContent + "</td></tr>";

                        var lBBSContent = (from p in db.BPCDetails
                                           where p.CustomerCode == CustomerCode &&
                                           p.ProjectCode == ProjectCode &&
                                           p.Template == false &&
                                           p.JobID == lJobID
                                           orderby p.cage_id
                                           select p).ToList();

                        if (lBBSContent.Count > 0)
                        {
                            for (int j = 0; j < lBBSContent.Count; j++)
                            {
                                lEmailContent = lEmailContent + "<tr><td align='center'> <font color='blue'>" + (j + 1).ToString() + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[j].pile_dia.ToString() + "</font></td>";
                                //Cage Type
                                lVar = "";
                                var lPileType = lBBSContent[j].pile_type;
                                var lMainBarArrange = lBBSContent[j].main_bar_arrange;
                                var lMainBarType = lBBSContent[j].main_bar_type;
                                if (lPileType == "Single-Layer")
                                {
                                    if (lMainBarType == "Single")
                                    {
                                        if (lMainBarArrange == "Single")
                                        {
                                            lVar = "Single Layer";
                                        }
                                        else if (lMainBarArrange == "Side-By-Side")
                                        {
                                            lVar = "Single Layer<br/>Side-By-Side Bundled Bars";
                                        }
                                        else if (lMainBarArrange == "In-Out")
                                        {
                                            lVar = "Single Layer<br/>In-Out Bundled Bars";
                                        }
                                        else
                                        {
                                            lVar = "Single Layer<br/>Complex Bundled Bars";
                                        }
                                    }
                                    if (lMainBarType == "Mixed")
                                    {
                                        if (lMainBarArrange == "Single")
                                        {
                                            lVar = "Single Layer<br/>Mixed Bars";
                                        }
                                        else if (lMainBarArrange == "Side-By-Side")
                                        {
                                            lVar = "Single Layer<br/>Side By Side Bundled<br/>Mixed Bars";
                                        }
                                        else if (lMainBarArrange == "In-Out")
                                        {
                                            lVar = "Single Layer<br/>In-Out Bundled<br/>Mixed Bars";
                                        }
                                        else
                                        {
                                            lVar = "Single Layer<br/>Complex Bundled<br/>Mixed Bars";
                                        }
                                    }
                                }
                                else
                                {
                                    if (lMainBarArrange == "Single")
                                    {
                                        lVar = "Double Layer";
                                    }
                                    else if (lMainBarArrange == "Side-By-Side")
                                    {
                                        lVar = "Double Layer<br/>Side By Side Bundled Bars";
                                    }
                                    else
                                    {
                                        lVar = "Double Layer<br/>Complex Bundled Bars";
                                    }

                                }

                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lVar + "</font></td>";

                                lVar = "";
                                var lBarCTArr = lBBSContent[j].main_bar_ct.Split(',');
                                var lBarDiaArr = lBBSContent[j].main_bar_dia.Split(',');
                                var lBarType = lBBSContent[j].main_bar_grade.Trim();
                                if (lBarCTArr.Length > 0 && lBarDiaArr.Length > 0)
                                {
                                    lVar = lBarCTArr[0].Trim() + lBarType + lBarDiaArr[0].Trim();
                                }
                                if (lBarCTArr.Length > 1 && lBarDiaArr.Length > 1)
                                {
                                    lVar = lVar + "<br/>" + lBarCTArr[1].Trim() + lBarType + lBarDiaArr[1].Trim();
                                }

                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lVar + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[j].main_bar_shape + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[j].cage_length.ToString() + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[j].lap_length.ToString() + "</font></td>";

                                lVar = "";
                                var lSLType = "";
                                if (lBBSContent[j].spiral_link_type.Length >= 5)
                                {
                                    if (lBBSContent[j].spiral_link_type.Substring(0, 5) == "Others")
                                    {
                                        lSLType = "";
                                    }
                                    else if (lBBSContent[j].spiral_link_type.Substring(0, 4) == "Twin")
                                    {
                                        lSLType = "2";
                                    }
                                }
                                var lSLSpacing = lBBSContent[j].spiral_link_spacing.Split(',');
                                var lSLGrade = lBBSContent[j].spiral_link_grade.Trim();
                                if (lSLSpacing.Length > 0 && lSLSpacing.Length > 0)
                                {
                                    lVar = lSLType + lSLGrade + lBBSContent[j].sl1_dia + "-" + lSLSpacing[0] + "-" + lBBSContent[j].sl1_length;
                                }
                                if (lSLSpacing.Length > 1 && lSLSpacing.Length > 1)
                                {
                                    lVar = lVar + "<br/>" + lSLType + lSLGrade + lBBSContent[j].sl2_dia + "-" + lSLSpacing[1] + "-" + lBBSContent[j].sl2_length;
                                }
                                if (lSLSpacing.Length > 2 && lSLSpacing.Length > 2)
                                {
                                    lVar = lVar + "<br/>" + lSLType + lSLGrade + lBBSContent[j].sl3_dia + "-" + lSLSpacing[2] + "-" + lBBSContent[j].sl3_length;
                                }

                                lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lVar + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[j].end_length.ToString() + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[j].cage_qty.ToString() + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[j].cage_location + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[j].cage_weight.ToString("F3") + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[j].per_set.ToString() + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[j].bbs_no + "</font></td>";
                                lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[j].cage_remarks + "</font></td>";
                                lEmailContent = lEmailContent + "</tr>";
                            }
                        }
                        lEmailContent = lEmailContent + "</table>";

                    }

                    lEmailContent = lEmailContent + "</td></tr>";
                    lEmailContent = lEmailContent + "</table>";
                }

            }

            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
            lEmailContent = lEmailContent + "<td width=20%>" + "Special Remarks/Delivery Address (特别备注/送货地址)" + "</td>";

            lVar = "&nbsp;";
            if (JobContent.Remarks != null) lVar = JobContent.Remarks.Trim();
            lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr>";

            lEmailContent = lEmailContent + "<tr><td>" + "BBS Description (BBS说明)" + "</td>";

            lVar = "&nbsp;";
            if (JobContent.DeliveryAddress != null) lVar = JobContent.DeliveryAddress.Trim();
            lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr></table>";


            lEmailContent = lEmailContent + "<table border='1' width=100%>";

            lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Site Contact (工地联系人)" + "</td>";
            lEmailContent = lEmailContent + "<td width='15%'>" + JobContent.Scheduler_Name.Trim() + "</td>";
            lEmailContent = lEmailContent + "<td width='13%'>" + "H / P\n(手机号码)" + " </td>";
            lEmailContent = lEmailContent + "<td width='16%'>" + JobContent.Scheduler_HP.Trim() + "</td>";
            lEmailContent = lEmailContent + "<td width='13%'>" + "Email\n(电邮地址)" + "</td>";
            lEmailContent = lEmailContent + "<td>" + JobContent.Scheduler_Email.Trim() + "</td></tr>";

            lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Good Receiver\n(收货人)" + "</td>";
            lEmailContent = lEmailContent + "<td width='15%'>" + JobContent.SiteEngr_Name.Trim() + "</td>";
            lEmailContent = lEmailContent + "<td width='13%'>" + "H / P\n(手机号码)" + " </td>";
            lEmailContent = lEmailContent + "<td width='16%'>" + JobContent.SiteEngr_HP.Trim() + "</td>";
            lEmailContent = lEmailContent + "<td width='13%'>" + "Email\n(电邮地址)" + "</td>";
            lEmailContent = lEmailContent + "<td>" + JobContent.SiteEngr_Email.Trim() + "</td></tr>";

            lEmailContent = lEmailContent + "</table>";

            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";

            lEmailContent = lEmailContent + "<td colspan='3'>" + "NatSteel Contacts (大众钢铁联系人) (Fax:62619133)" + "</td></tr>";

            lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Name (姓名)" + "</td>";
            lEmailContent = lEmailContent + "<td width='15%'>" + "Contact Numbers (联系电话)" + "</td>";
            lEmailContent = lEmailContent + "<td width='13%'>" + "Email Address (电邮地址)" + " </td></tr>";


            var lProjContent = (from p in db.Project
                                where p.ProjectCode == ProjectCode
                                orderby p.ProjectTitle descending
                                select p).First();

            if (lProjContent.Contact1 != null)
            {
                if (lProjContent.Contact1.Trim().Length > 0)
                {
                    lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact1.Trim() + "</td>";
                    lVar1 = "";
                    if (lProjContent.Tel1 != null) if (lProjContent.Tel1.Trim().Length > 0) lVar1 = lProjContent.Tel1.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

                    lVar1 = "";
                    if (lProjContent.Email1 != null) if (lProjContent.Email1.Trim().Length > 0) lVar1 = lProjContent.Email1.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
                    if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
                }
            }

            if (lProjContent.Contact2 != null)
            {
                if (lProjContent.Contact2.Trim().Length > 0)
                {
                    lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact2.Trim() + "</td>";
                    lVar1 = "";
                    if (lProjContent.Tel2 != null) if (lProjContent.Tel2.Trim().Length > 0) lVar1 = lProjContent.Tel2.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

                    lVar1 = "";
                    if (lProjContent.Email2 != null) if (lProjContent.Email2.Trim().Length > 0) lVar1 = lProjContent.Email2.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
                    if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
                }
            }

            if (lProjContent.Contact3 != null)
            {
                if (lProjContent.Contact3.Trim().Length > 0)
                {
                    lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact3.Trim() + "</td>";
                    lVar1 = "";
                    if (lProjContent.Tel3 != null) if (lProjContent.Tel3.Trim().Length > 0) lVar1 = lProjContent.Tel3.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

                    lVar1 = "";
                    if (lProjContent.Email3 != null) if (lProjContent.Email3.Trim().Length > 0) lVar1 = lProjContent.Email3.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
                    if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
                }
            }

            if (lProjContent.Contact4 != null)
            {
                if (lProjContent.Contact4.Trim().Length > 0)
                {
                    lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact4.Trim() + "</td>";
                    lVar1 = "";
                    if (lProjContent.Tel4 != null) if (lProjContent.Tel4.Trim().Length > 0) lVar1 = lProjContent.Tel4.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

                    lVar1 = "";
                    if (lProjContent.Email4 != null) if (lProjContent.Email4.Trim().Length > 0) lVar1 = lProjContent.Email4.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
                    if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
                }
            }

            if (lProjContent.Contact5 != null)
            {
                if (lProjContent.Contact5.Trim().Length > 0)
                {
                    lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact5.Trim() + "</td>";
                    lVar1 = "";
                    if (lProjContent.Tel5 != null) if (lProjContent.Tel5.Trim().Length > 0) lVar1 = lProjContent.Tel5.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

                    lVar1 = "";
                    if (lProjContent.Email5 != null) if (lProjContent.Email5.Trim().Length > 0) lVar1 = lProjContent.Email5.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
                    if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
                }
            }

            if (lProjContent.Contact6 != null)
            {
                if (lProjContent.Contact6.Trim().Length > 0)
                {
                    lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact6.Trim() + "</td>";
                    lVar1 = "";
                    if (lProjContent.Tel6 != null) if (lProjContent.Tel6.Trim().Length > 0) lVar1 = lProjContent.Tel6.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

                    lVar1 = "";
                    if (lProjContent.Email6 != null) if (lProjContent.Email6.Trim().Length > 0) lVar1 = lProjContent.Email6.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
                    if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
                }
            }

            #region email to Technical ... Moved to Processed
            //if (OrderAction == "Submitted")
            //{
            //    if (lDetailingProd == 1)
            //    {
            //        SqlCommand lCmd;
            //        SqlDataReader lRst;
            //        SqlConnection lNDSCn = new SqlConnection();

            //        lProcessObj = new ProcessController();
            //        var lSQL = "SELECT DetailingIncharge FROM dbo.OESProjectIncharges WHERE ProjectCode = '" + ProjectCode + "' ";

            //        if (lProcessObj.OpenNDSConnection(ref lNDSCn) == true)
            //        {
            //            lCmd = new SqlCommand(lSQL, lNDSCn);
            //            lCmd.CommandTimeout = 1200;
            //            lRst = lCmd.ExecuteReader();
            //            if (lRst.HasRows)
            //            {
            //                if (lRst.Read())
            //                {
            //                    var lDetailer = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0);
            //                    var lDetailerA = lDetailer.Split(',').ToList();
            //                    if (lDetailerA.Count > 0 )
            //                    {
            //                        for (int i = 0; i < lDetailerA.Count; i++)
            //                        {
            //                            if (lDetailerA[i] != null && lDetailerA[i].Trim() != "" &&
            //                            lDetailerA[i].IndexOf(".") < 0 && lDetailerA[i].IndexOf("@") < 0)
            //                            {
            //                                if (lEmailTo == "")
            //                                {
            //                                    lEmailTo = lDetailerA[i].Trim() + "@natsteel.com.sg";
            //                                }
            //                                else
            //                                {
            //                                    lEmailTo = lEmailTo + ";" + lDetailerA[i].Trim() + "@natsteel.com.sg";
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            lRst.Close();

            //            lProcessObj.CloseNDSConnection(ref lNDSCn);
            //        }
            //        lProcessObj = null;
            //    }
            //}
            #endregion

            lEmailContent = lEmailContent + "</table>";

            // No email to PMD group for created by natsteel staff
            if (lUpdatedBy.ToLower().IndexOf("natsteel.com.sg") > 0 && OrderAction != "Cancelled" && OrderAction != "Withdraw")
            {
                lEmailTo = "";
            }

            if (OrderAction == "Received" || OrderAction == "Cancelled")
            {
                lEmailCc = lEmailTo;
                lEmailTo = "";
            }

            #region Header & email address for send,withdraw (sent)
            if (OrderAction == "Received" || OrderAction == "Cancelled" || OrderAction == "Submitted" || (OrderAction == "Withdraw" && OrderStatus != "Sent"))
            {
                //Title
                if (OrderAction == "Submitted")
                {
                    lEmailContent = "<p align='center'>DIGITAL PURCHASE ORDER ADVICE (数码订单通知) - " + UserID + "</p>" + lEmailContent;
                }
                else if (OrderAction == "Withdraw")
                {
                    lEmailContent = "<p align='center'>WITHDRAW DIGITAL PURCHASE ORDER ADVICE (撤回数码订单通知) - " + UserID + "</p>" + lEmailContent;
                }
                else if (OrderAction == "Received")
                {
                    lEmailContent = "<p align='center'>RECEIVED DIGITAL PURCHASE ORDER ADVICE (数码订单已验收) - " + UserID + "</p>" + lEmailContent;
                }
                else if (OrderAction == "Cancelled")
                {
                    lEmailContent = "<p align='center'>CANCELLED DIGITAL PURCHASE ORDER ADVICE (取消数码订单) - " + UserID + "</p>" + lEmailContent;
                }

                // Scheduler email
                if (JobContent != null)
                {
                    lVar1 = JobContent.Scheduler_Email.Trim();
                    lVar1 = lVar1.Replace(" ", "");
                    if (OrderAction == "Received" || OrderAction == "Cancelled")
                    {
                        if (lVar1.Length > 0 && lEmailTo.IndexOf(lVar1.ToLower()) < 0)
                        {

                            if (lEmailTo == "")
                            {
                                lEmailTo = lVar1.ToLower();
                            }
                            else
                            {
                                lEmailTo = lEmailTo + ";" + lVar1.ToLower();
                            }

                        }
                    }
                    else
                    {
                        if (lVar1.Length > 0 && lEmailCc.IndexOf(lVar1.ToLower()) < 0)
                        {

                            if (lEmailCc == "")
                            {
                                lEmailCc = lVar1.ToLower();
                            }
                            else
                            {
                                lEmailCc = lEmailCc + ";" + lVar1.ToLower();
                            }
                        }
                    }

                    lVar1 = JobContent.SiteEngr_Email.Trim();
                    lVar1 = lVar1.Replace(" ", "");
                    if (OrderAction == "Received" || OrderAction == "Cancelled")
                    {
                        if (lVar1.Length > 0 && lEmailTo.IndexOf(lVar1.ToLower()) < 0)
                        {
                            if (lEmailTo == "")
                            {
                                lEmailTo = lVar1.ToLower();
                            }
                            else
                            {
                                lEmailTo = lEmailTo + ";" + lVar1.ToLower();
                            }
                        }
                    }
                    else
                    {
                        if (lVar1.Length > 0 && lEmailCc.IndexOf(lVar1.ToLower()) < 0)
                        {
                            if (lEmailCc == "")
                            {
                                lEmailCc = lVar1.ToLower();
                            }
                            else
                            {
                                lEmailCc = lEmailCc + ";" + lVar1.ToLower();
                            }
                        }
                    }

                    if (JobContent.UpdateBy != null)
                    {
                        lVar1 = JobContent.UpdateBy.Trim();
                        lVar1 = lVar1.Replace(" ", "");
                        if (OrderAction == "Received" || OrderAction == "Cancelled")
                        {
                            if (lVar1 != "" && lEmailTo.IndexOf(lVar1.ToLower()) < 0)
                            {
                                if (lEmailTo == "") { lEmailTo = lVar1.ToLower(); }
                                else { lEmailTo = lEmailTo + ";" + lVar1.ToLower(); }
                            }
                        }
                        else
                        {
                            if (lVar1 != "" && lEmailCc.IndexOf(lVar1.ToLower()) < 0)
                            {
                                if (lEmailCc == "") { lEmailCc = lVar1.ToLower(); }
                                else { lEmailCc = lEmailCc + ";" + lVar1.ToLower(); }
                            }
                        }
                    }

                    if (JobContent.SubmitBy != null)
                    {
                        lVar1 = JobContent.SubmitBy.Trim();
                        lVar1 = lVar1.Replace(" ", "");
                        if (OrderAction == "Received" || OrderAction == "Cancelled")
                        {
                            if (lVar1 != "" && lEmailTo.IndexOf(lVar1.ToLower()) < 0)
                            {
                                if (lEmailTo == "") { lEmailTo = lVar1.ToLower(); }
                                else { lEmailTo = lEmailTo + ";" + lVar1.ToLower(); }
                            }
                        }
                        else
                        {
                            if (lVar1 != "" && lEmailCc.IndexOf(lVar1.ToLower()) < 0)
                            {
                                if (lEmailCc == "") { lEmailCc = lVar1.ToLower(); }
                                else { lEmailCc = lEmailCc + ";" + lVar1.ToLower(); }
                            }
                        }
                    }
                }

                if (lProjContent.EmailDistribution != null)
                {
                    lVar1 = lProjContent.EmailDistribution.Trim();
                    if (lVar1.Length > 0 && lEmailCc.IndexOf(lVar1.ToLower()) < 0)
                    {
                        if (lEmailCc == "")
                        {
                            lEmailCc = lVar1.ToLower();
                        }
                        else
                        {
                            lEmailCc = lEmailCc + ";" + lVar1.ToLower();
                        }
                    }

                }

                // No email to PMD group for created by natsteel staff
                if (lUpdatedBy.ToLower().IndexOf("natsteel.com.sg") > 0)
                {
                    lEmailTo = "";
                }

                lVar = "";
                if (lCustomer != null) lVar = lCustomer.CustomerName.Trim();

                lEmailSubject = lVar + " - " + lProducts + " - " + lPOs;

                if (OrderAction == "Withdraw")
                {
                    lEmailSubject = lEmailSubject + " - Withdraw ";
                }
                if (OrderAction == "Received" || OrderAction == "Cancelled" || OrderAction == "Withdraw")
                {
                    // CC to Planning_mesh, Technical


                    if (lServer == "DEV")
                    {
                        if (lEmailCc.Length > 0)
                        {
                            lEmailCc = lEmailCc + ";Ajit.Kamble@tatatechnologies.com";
                        }
                        else
                        {
                            lEmailCc = "Ajit.Kamble@tatatechnologies.com";
                        }
                    }
                    else
                    {
                        if (lEmailCc.Length > 0)
                        {
                            if (lProducts.IndexOf("Rebar") >= 0 || lProducts.IndexOf("CAB") >= 0 || lProducts.IndexOf("STANDARD-BAR") >= 0)
                            {
                                //lEmailCc = lEmailCc + ";planning_cab@natsteel.com.sg";
                            }
                            else if (lProducts.IndexOf("BPC") >= 0 || lProducts.IndexOf("PRE-CAGE") >= 0 || lProducts.IndexOf("CAGE") >= 0 || lProducts.IndexOf("CORE-CAGE") >= 0 || lProducts.IndexOf("CARPET") >= 0)
                            {
                                // Removed the email requested by Kelvin Lim Wei Liang dd 2022-05-05
                                //lEmailCc = lEmailCc + ";planning_cage@natsteel.com.sg";
                            }
                            //if (lProducts.IndexOf("Standard MESH") >= 0 || lProducts.IndexOf("MESH") >= 0 || lProducts.IndexOf("STIRRUP-LINK-MESH") >= 0 ||
                            //    lProducts.IndexOf("COLUMN-LINK-MESH") >= 0 || lProducts.IndexOf("CUT-TO-SIZE-MESH") >= 0 || lProducts.IndexOf("STANDARD-MESH") >= 0)
                            else if (lProducts.IndexOf("COIL") >= 0)
                            {
                                lEmailCc = "Planning_WP@natsteel.com.sg;ajit.kamble@tatatechnologies.com";
                            }
                            else
                            {
                                lEmailCc = "planning_mesh@natsteel.com.sg";
                            }
                            if (lProducts.IndexOf("STANDARD-MESH") >= 0)
                            {
                                lEmailCc = lEmailCc + ";ltc@natsteel.com.sg";
                            }

                        }
                        else
                        {
                            if (lProducts.IndexOf("Rebar") >= 0 || lProducts.IndexOf("CAB") >= 0 || lProducts.IndexOf("STANDARD-BAR") >= 0)
                            {
                                //lEmailCc = "planning_cab@natsteel.com.sg";
                            }
                            else if (lProducts.IndexOf("BPC") >= 0 || lProducts.IndexOf("PRE-CAGE") >= 0 || lProducts.IndexOf("CAGE") >= 0 || lProducts.IndexOf("CORE-CAGE") >= 0 || lProducts.IndexOf("CARPET") >= 0)
                            {
                                // Removed the email requested by Kelvin Lim Wei Liang dd 2022-05-05
                                //lEmailCc = "planning_cage@natsteel.com.sg";
                            }
                            //if (lProducts.IndexOf("Standard MESH") >= 0 || lProducts.IndexOf("MESH") >= 0 || lProducts.IndexOf("STIRRUP-LINK-MESH") >= 0 ||
                            //    lProducts.IndexOf("COLUMN-LINK-MESH") >= 0 || lProducts.IndexOf("CUT-TO-SIZE-MESH") >= 0 || lProducts.IndexOf("STANDARD-MESH") >= 0)
                                else
                            {
                                lEmailCc = "planning_mesh@natsteel.com.sg";
                            }
                            if (lProducts.IndexOf("STANDARD-MESH") >= 0)
                            {
                                lEmailCc = lEmailCc + ";ltc@natsteel.com.sg";
                            }

                        }

                        //Technical
                        if (lDetailingProd == 1)
                        {
                            SqlCommand lCmd;
                            SqlDataReader lRst;
                            SqlConnection lNDSCn = new SqlConnection();

                            lProcessObj = new ProcessController();
                            var lSQL = "SELECT DetailingIncharge FROM dbo.OESProjectIncharges WHERE ProjectCode = '" + ProjectCode + "' ";

                            if (lProcessObj.OpenNDSConnection(ref lNDSCn) == true)
                            {
                                lCmd = new SqlCommand(lSQL, lNDSCn);
                                lCmd.CommandTimeout = 1200;
                                lRst = lCmd.ExecuteReader();
                                if (lRst.HasRows)
                                {
                                    if (lRst.Read())
                                    {
                                        var lDetailer = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0);
                                        var lDetailerA = lDetailer.Split(',').ToList();
                                        if (lDetailerA.Count > 0)
                                        {
                                            for (int i = 0; i < lDetailerA.Count; i++)
                                            {
                                                if (lDetailerA[i] != null && lDetailerA[i].Trim() != "" &&
                                                lDetailerA[i].IndexOf(".") < 0 && lDetailerA[i].IndexOf("@") < 0)
                                                {
                                                    if (lEmailCc == "")
                                                    {
                                                        lEmailCc = lDetailerA[i].Trim() + "@natsteel.com.sg";
                                                    }
                                                    else
                                                    {
                                                        lEmailCc = lEmailCc + ";" + lDetailerA[i].Trim() + "@natsteel.com.sg";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                lRst.Close();

                                lProcessObj.CloseNDSConnection(ref lNDSCn);
                            }
                            lProcessObj = null;
                        }

                    }

                    if (OrderAction == "Received")
                    {
                        lEmailSubject = lEmailSubject + " - Received ";
                    }
                    else if (OrderAction == "Cancelled")
                    {
                        lEmailSubject = lEmailSubject + " - Cancelled ";
                    }
                }
            }
            else if (OrderAction == "Sent" || OrderAction == "Shared" || (OrderAction == "Withdraw" && OrderStatus == "Sent"))
            {
                if (OrderAction == "Sent")
                {
                    lEmailContent = "<p align='center'>APPROVAL REQUEST FOR DIGITAL PURCHASE ORDER (数码订单申请) - " + UserID + "</p>" + lEmailContent;
                }
                else if (OrderAction == "Shared")
                {
                    lEmailContent = "<p align='center'>SHARED DIGITAL PURCHASE ORDER (共享数码订单) - " + UserID + "</p>" + lEmailContent;
                }
                else
                {
                    lEmailContent = "<p align='center'>WITHDRAW REQUEST FOR DIGITAL PURCHASE ORDER (撤回数码订单申请) - " + UserID + "</p>" + lEmailContent;
                }

                lEmailTo = "";

                var lApprovers = (from p in db.UserAccess
                                  where p.CustomerCode == CustomerCode &&
                                  p.ProjectCode == ProjectCode &&
                                  p.OrderSubmission == "Yes"
                                  select p.UserName).ToList();

                if (lApprovers != null && lApprovers.Count > 0)
                {
                    for (int i = 0; i < lApprovers.Count; i++)
                    {
                        lVar1 = lApprovers[i] == null ? "" : lApprovers[i].Trim();
                        lVar1 = lVar1.Replace(" ", "");
                        if (lVar1 != "" && lEmailTo.IndexOf(lVar1.ToLower()) < 0)
                        {
                            if (lEmailTo == "") { lEmailTo = lVar1.ToLower(); }
                            else { lEmailTo = lEmailTo + ";" + lVar1.ToLower(); }
                        }
                    }
                }

                if (JobContent.SubmitBy != null)
                {
                    lVar1 = JobContent.SubmitBy.Trim();
                    lVar1 = lVar1.Replace(" ", "");
                    if (lVar1 != "" && lEmailCc.IndexOf(lVar1.ToLower()) < 0)
                    {
                        if (lEmailCc == "") { lEmailCc = lVar1.ToLower(); }
                        else { lEmailCc = lEmailCc + ";" + lVar1.ToLower(); }
                    }
                }

                // Scheduler email
                if (JobContent != null)
                {
                    lVar1 = JobContent.Scheduler_Email.Trim();
                    lVar1 = lVar1.Replace(" ", "");
                    if (lVar1.Length > 0 && lEmailCc.IndexOf(lVar1.ToLower()) < 0)
                    {
                        if (lEmailCc == "")
                        {
                            lEmailCc = lVar1.ToLower();
                        }
                        else
                        {
                            lEmailCc = lEmailCc + ";" + lVar1.ToLower();
                        }
                    }

                    lVar1 = JobContent.SiteEngr_Email.Trim();
                    lVar1 = lVar1.Replace(" ", "");
                    if (lVar1.Length > 0 && lEmailCc.IndexOf(lVar1.ToLower()) < 0)
                    {
                        if (lEmailCc == "")
                        {
                            lEmailCc = lVar1.ToLower();
                        }
                        else
                        {
                            lEmailCc = lEmailCc + ";" + lVar1.ToLower();
                        }
                    }
                }

                // Email distribution
                if (lProjContent.EmailDistribution != null)
                {
                    lVar1 = lProjContent.EmailDistribution.Trim();
                    if (lVar1.Length > 0 && lEmailCc.IndexOf(lVar1.ToLower()) < 0)
                    {
                        if (lEmailCc == "")
                        {
                            lEmailCc = lVar1.ToLower();
                        }
                        else
                        {
                            lEmailCc = lEmailCc + ";" + lVar1.ToLower();
                        }
                    }

                }

                if (OrderAction == "Sent")
                {
                    lEmailSubject = "Approval Request for Digial Purchase Order - " + lProducts + (lPOs == null || lPOs == "" ? "" : " - " + lPOs);
                }
                else if (OrderAction == "Shared")
                {
                    lEmailSubject = "Shared Digial Purchase Order - " + lProducts + (lPOs == null || lPOs == "" ? "" : " - " + lPOs);
                }
                else
                {
                    lEmailSubject = "Withdraw Request for Digial Purchase Order - " + lProducts + (lPOs == null || lPOs == "" ? "" : " - " + lPOs);
                }
            }
            else if (OrderAction == "Reject")
            {
                lEmailContent = "<p align='center'>REJECT REQUEST FOR DIGITAL PURCHASE ORDER (数码订单申请被拒)</p>" + lEmailContent;

                lEmailTo = "";

                if (JobContent.SubmitBy != null)
                {
                    lVar1 = JobContent.SubmitBy.Trim();
                    lVar1 = lVar1.Replace(" ", "");
                    if (lVar1 != "" && lEmailTo.IndexOf(lVar1.ToLower()) < 0)
                    {
                        if (lEmailTo == "") { lEmailTo = lVar1.ToLower(); }
                        else { lEmailTo = lEmailTo + ";" + lVar1.ToLower(); }
                    }
                }

                lEmailCc = "";
                var lApprovers = (from p in db.UserAccess
                                  where p.CustomerCode == CustomerCode &&
                                  p.ProjectCode == ProjectCode &&
                                  p.OrderSubmission == "Yes"
                                  select p.UserName).ToList();

                if (lApprovers != null && lApprovers.Count > 0)
                {
                    for (int i = 0; i < lApprovers.Count; i++)
                    {
                        lVar1 = lApprovers[i] == null ? "" : lApprovers[i].Trim();
                        if (lVar1 != "" && lEmailCc.IndexOf(lVar1.ToLower()) < 0)
                        {
                            if (lEmailCc == "") { lEmailCc = lVar1.ToLower(); }
                            else { lEmailCc = lEmailCc + ";" + lVar1.ToLower(); }
                        }
                    }
                }

                lEmailSubject = "Reject Request for Digial Purchase Order - " + lProducts + (lPOs == null || lPOs == "" ? "" : " - " + lPOs);
            }
            #endregion

            string lEmailFromAddress = "eprompt@natsteel.com.sg";
            string lEmailFromName = "Digital Ordering Email Services";

            //dynamic response = lOESEmail.Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent);
            Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent).Wait();//commented by ajit

            return lReturn;
        }


     
    }

}