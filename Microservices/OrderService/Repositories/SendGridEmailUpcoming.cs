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
using OrderService.Dtos;

namespace OrderService.Repositories
{
    public class SendGridEmailUpcoming
    {
        private const string sLoginID = "DigiOSNoReply@natsteel.com.sg";
        private const string sLoginPwd = "NatSteel@123B";


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

        public bool SendOrderActionEmail1(string EmailTo,string CustomerCode, string ProjectCode, List<int> OrderNumber)
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

            //var JobContent = db.OrderProject.Find(OrderNumber[0]);
            //var JobContent= db.OrderProject
            //       .Where(op => OrderNumber.Contains(op.OrderNumber))
            //       .ToList();


            

            //lEmailContent = "Serial No.: " + JobContent.OrderNumber.ToString() + "<br/>";

            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
            lEmailContent = lEmailContent + "<td width=20%>" + "Customer (客户名称)" + "</td>";

            CustomerModels lCustomer = db.Customer.Find(CustomerCode);
            string lVar = "";
            lVar = "";
            if (lCustomer != null) lVar = lCustomer.CustomerName;

            lEmailSubject = lVar + " - " + "Upcoming Orders";
            if (lCustomer != null) lVar = lCustomer.CustomerName.Trim() + " (" + CustomerCode.Trim() + ")";
            lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr>";


            #region Project loop

            lEmailContent = lEmailContent + "<tr><td>" + "Project (工程项目)" + "</td>";

            var lProject = (from p in db.Project
                            where p.ProjectCode == ProjectCode
                            orderby p.ProjectTitle descending
                            select p).First();
            lVar = "";
            if (lProject != null && lProject.ProjectTitle != null && ProjectCode != null) lVar = lProject.ProjectTitle.Trim() + " (" + ProjectCode.Trim() + ")";
            lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr></table>";


            //var lOrderUC = (from p in db.OESUpcomingOrders
            //                where p.OrderNumber == OrderNumber
            //                select p).ToList();

            var lOrderUC = (from p in db.OESUpcomingOrders
             where OrderNumber.Contains(p.OrderNumber)
             select p).ToList();

            //for (int i = 0; i < lOrderUC.Count; i++)
            //{
               
                lEmailContent = lEmailContent + "<table border='1' width=100%>";
            // lEmailContent = lEmailContent + "<tr><td colspan=2>";
            lEmailContent = lEmailContent + "<tr><td>" + "Order No (订单号)" + "</td>";
            lEmailContent = lEmailContent + "<td>" + "Block (WBS1) (座号/大牌)" + "</td>";
                lEmailContent = lEmailContent + "<td>" + "Storey (WBS2) (楼层)" + "</td>";
            lEmailContent = lEmailContent + "<td>" + "Part (WBS3) (分部)" + " </td>";
            lEmailContent = lEmailContent + "<td>" + "Product type" + " </td>";
            lEmailContent = lEmailContent + "<td>" + "Structure element" + " </td>";
            lEmailContent = lEmailContent + "<td>" + "Forecast Date" + " </td>";

                lEmailContent = lEmailContent + "</tr>";
                var lProjContent = (from p in db.Project
                                        where p.ProjectCode == ProjectCode
                                        orderby p.ProjectTitle descending
                                        select p).First();



                for (int j = 0; j < lOrderUC.Count; j++)
                {
                lEmailContent = lEmailContent + "<tr><td>" + lOrderUC[j].OrderNumber + "</td>";
                lEmailContent = lEmailContent + "<td>" + lOrderUC[j].WBS1 + "</td>";
                    lEmailContent = lEmailContent + "<td>" + lOrderUC[j].WBS2 + "</td>";
                    lEmailContent = lEmailContent + "<td>" + lOrderUC[j].WBS3 + "</td>";
                    lEmailContent = lEmailContent + "<td>" + lOrderUC[j].ProductType + "</td>";
                    lEmailContent = lEmailContent + "<td>" + lOrderUC[j].StructureElement + "</td>";
                    lEmailContent = lEmailContent + "<td>" + lOrderUC[j].ForecastDate + "</td>";
                    lEmailContent = lEmailContent + "</tr>";
                }



                lEmailContent = lEmailContent + "</table>";

                
            //}

            #endregion
            string lEmailFromAddress = "eprompt@natsteel.com.sg";
            string lEmailFromName = "Digital Ordering Email Services";

                    //dynamic response = lOESEmail.Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent);
           Execute(lEmailFromAddress, lEmailFromName, EmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent).Wait();//commented by ajit

                    
               
            //}
            return lReturn;
        }
    }

}