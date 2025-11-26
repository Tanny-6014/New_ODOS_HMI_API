using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using OrderService.Constants;
using OrderService.Interfaces;
using OrderService.Models;
using OrderService.Dtos;
using OrderService.Repositories;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Net.Http.Headers;

namespace OrderService.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class OrderAssignmentController: Controller
    {
        private readonly IOrderAssignment _IOrder;

        public OrderAssignmentController(IOrderAssignment order)
        {
            _IOrder = order?? throw new ArgumentNullException(nameof(order));
            // this._mapper = mapper;
        }

        [HttpGet]
        [Route("/SchnellOrderAssignment")]
        public async Task<IActionResult> GetTop1000CabOrders(DateTime? FromReqDelDate = null, DateTime? ToReqDelDate = null,string? Customercode=null,
            string? Project=null, string? Producttype = null, string? Status = null, string? OrderNo = null)
        {
            List<OrderDto> OrdersList = _IOrder.GetCabOrders(FromReqDelDate, ToReqDelDate, Customercode, Project, Producttype,Status,OrderNo);
            var result = OrdersList;
            return Ok(result);
        }


        [HttpGet]
        [Route("/SchnellOrderItemData/{OrderRequestNo}")]
        public async Task<IActionResult> GetOrderItemdata(string OrderRequestNo)
        {
            List<OrderItemDet> OrdersItemsList = _IOrder.GetOrderItems(OrderRequestNo);
            var result = OrdersItemsList;
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetCustomerOrdAssgn")]
        public async Task<IActionResult> GetCustomerOrdAssgn()
        {
            List<CustomerOrdAssgn> CustList = _IOrder.GetCustomersOrdAssgn();
            var result = CustList;
            return Ok(result);
        }

        //public List<CustomerOrdAssgn> GetCustomersOrdAssgn()
        //{
        //    IEnumerable<CustomerOrdAssgn> orderslist;

        //    using (var sqlConnection = new SqlConnection(connectionString))
        //    {
        //        sqlConnection.Open();

        //        orderslist = sqlConnection.Query<CustomerOrdAssgn>(SystemConstant.GetCustomer_OrdAssgn, commandType: CommandType.StoredProcedure);
        //        sqlConnection.Close();
        //        return orderslist.ToList();

        //    }
        //}

        [HttpPost]
        [Route("/SaveOrderAssignment")]
        public ActionResult SaveOrderAssignment([FromBody] OrderAssignmentRequestDto request)
        {
            var content = _IOrder.SaveOrderAssignment(request.OrderRequestNo, request.AssignedTo);
            //var content = _IOrder.SaveOrderAssignment(request.OrderRequestNo, request.AssignedTo,request.AssignedBy);
            //return Json(content, JsonRequestBehavior.AllowGet);
            return Ok(content);
        }

        [HttpPost]
        [Route("/ExportOrderAssignmentToExcel")]
        public async Task<ActionResult> ExportOrderAssignmentToExcel()
        {
            //set kookie for customer and project
            DateTime? FromReqDelDate = null;
            DateTime? ToReqDelDate = null;

            if (DateTime.TryParse(Request.Form["FromReqDelDate"], out var parsedDate))
            {
                FromReqDelDate = parsedDate;
            }
            if (DateTime.TryParse(Request.Form["ToReqDelDate"], out var parsedDate1))
            {
                ToReqDelDate = parsedDate1;
            }
            string? Customercode = Request.Form["Customercode"].FirstOrDefault(); ;
            string? Project = Request.Form["Project"];
            string? Producttype = Request.Form["Producttype"];
            string? Status = Request.Form["Status"];
            //List<string> ProjectCodes = temp.Split(',').ToList();

            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet ws = package.Workbook.Worksheets.Add("Order Assignment List");

            int lRowNo = 1;
            //ws.Column(1).Width = 5;       
            ws.Column(1).Width = 6;        //"SNo\n序号";
            ws.Column(2).Width = 17;        //"Order Request No";
            ws.Column(3).Width = 12;        //Order No";
            ws.Column(4).Width = 15;         //"Customer Code";
            ws.Column(5).Width = 30;        //"Customer Name";
            ws.Column(6).Width = 12;        //"Project No";
            ws.Column(7).Width = 25;        //"Project Name";
            ws.Column(8).Width = 20;        //"Customer PO No
            ws.Column(9).Width = 12;        //Product Type
            ws.Column(10).Width = 13;       //"Assignment Status
            ws.Column(11).Width = 11;       //"Assigned To
            ws.Column(12).Width = 11;       //"No. Of Cuts";
            ws.Column(13).Width = 12;        //No. Of Pieces
            ws.Column(14).Width = 10;        //"S/L Bendings
            ws.Column(15).Width = 8;        //S/L Pieces
            ws.Column(16).Width = 10;         //"SCB M-run
            ws.Column(17).Width = 8;        //"SCB Pieces";
            ws.Column(18).Width = 8;        //SBC Pieces";
            ws.Column(19).Width = 12;        //"Coupler Bar Pieces";
            ws.Column(20).Width = 8;        //"Coupler Ends
            ws.Column(21).Width = 13;        //Contract No
            ws.Column(22).Width = 15;       //"Customer Order Date
            ws.Column(23).Width = 17;       //"Required Delivery Date From
            ws.Column(24).Width = 15;       //"Required Delivery Date To
            ws.Column(25).Width = 12;        //Project Segment
            ws.Column(26).Width = 15;        //Project SubSegment
            ws.Column(27).Width = 12;        //Created By
            ws.Column(28).Width = 12;       //"Created Date
            ws.Column(29).Width = 11;       //"Score


            ws.Row(1).Height = 30;
            ws.Cells[lRowNo, 1].Value = "Sr No";
            //ws.Cells[lRowNo, 2].Value = "Order No\n订单号码";
            ws.Cells[lRowNo, 2].Value = "Order Request No";
            ws.Cells[lRowNo, 3].Value = "Order No";
            ws.Cells[lRowNo, 4].Value = "Customer Code";
            ws.Cells[lRowNo, 5].Value = "Customer Name";
            ws.Cells[lRowNo, 6].Value = "Project No";
            ws.Cells[lRowNo, 7].Value = "Project Name";
            ws.Cells[lRowNo, 8].Value = "Customer PO No";
            ws.Cells[lRowNo, 9].Value = "Product Type";
            ws.Cells[lRowNo, 10].Value = "Assignment Status";
            ws.Cells[lRowNo, 11].Value = "Assigned To";
            ws.Cells[lRowNo, 12].Value = "No. Of Cuts";
            ws.Cells[lRowNo, 13].Value = "No. Of Pieces";
            ws.Cells[lRowNo, 14].Value = "S/L Bendings";
            ws.Cells[lRowNo, 15].Value = "S/L Pieces";
            ws.Cells[lRowNo, 16].Value = "SCB M-run";
            ws.Cells[lRowNo, 17].Value = "SCB Pieces";
            ws.Cells[lRowNo, 18].Value = "SBC Pieces";
            ws.Cells[lRowNo, 19].Value = "Coupler Bar Pieces";
            ws.Cells[lRowNo, 20].Value = "Coupler Ends";
            ws.Cells[lRowNo, 21].Value = "Contract No";
            ws.Cells[lRowNo, 22].Value = "Customer Order Date";
            ws.Cells[lRowNo, 23].Value = "Required Delivery Date From";
            ws.Cells[lRowNo, 24].Value = "Required Delivery Date To";
            ws.Cells[lRowNo, 25].Value = "Project Segment";
            ws.Cells[lRowNo, 26].Value = "Project SubSegment";
            ws.Cells[lRowNo, 27].Value = "Created By";
            ws.Cells[lRowNo, 28].Value = "Created Date";
            ws.Cells[lRowNo, 29].Value = "Score(%)";

            var range = ws.Cells["A1:AC1"];

            range.Style.WrapText = true;

            //ws.Cells[lRowNo, 1].Style.WrapText = true;
            //ws.Cells[lRowNo, 2].Style.WrapText = true;
            //ws.Cells[lRowNo, 3].Style.WrapText = true;
            //ws.Cells[lRowNo, 4].Style.WrapText = true;
            //ws.Cells[lRowNo, 5].Style.WrapText = true;
            //ws.Cells[lRowNo, 6].Style.WrapText = true;
            //ws.Cells[lRowNo, 7].Style.WrapText = true;
            //ws.Cells[lRowNo, 8].Style.WrapText = true;
            //ws.Cells[lRowNo, 9].Style.WrapText = true;
            //ws.Cells[lRowNo, 10].Style.WrapText = true;
            //ws.Cells[lRowNo, 11].Style.WrapText = true;
            //ws.Cells[lRowNo, 12].Style.WrapText = true;
            //ws.Cells[lRowNo, 13].Style.WrapText = true;
            //ws.Cells[lRowNo, 14].Style.WrapText = true;
            //ws.Cells[lRowNo, 15].Style.WrapText = true;
            //ws.Cells[lRowNo, 16].Style.WrapText = true;
            //ws.Cells[lRowNo, 17].Style.WrapText = true;
            //ws.Cells[lRowNo, 18].Style.WrapText = true;
            //ws.Cells[lRowNo, 19].Style.WrapText = true;
            //ws.Cells[lRowNo, 20].Style.WrapText = true;
            //ws.Cells[lRowNo, 21].Style.WrapText = true;
            //ws.Cells[lRowNo, 22].Style.WrapText = true;
            //ws.Cells[lRowNo, 23].Style.WrapText = true;
            //ws.Cells[lRowNo, 24].Style.WrapText = true;
            //ws.Cells[lRowNo, 25].Style.WrapText = true;
            //ws.Cells[lRowNo, 26].Style.WrapText = true;
            //ws.Cells[lRowNo, 27].Style.WrapText = true;
            //ws.Cells[lRowNo, 28].Style.WrapText = true;
            //ws.Cells[lRowNo, 29].Style.WrapText = true;
            //ws.Cells[lRowNo, 30].Style.WrapText = true;

            range.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            //ws.Cells[lRowNo, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);


            range.Style.Fill.PatternType = ExcelFillStyle.Solid;

            //ws.Cells[lRowNo, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[lRowNo, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[lRowNo, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[lRowNo, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[lRowNo, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[lRowNo, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[lRowNo, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[lRowNo, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[lRowNo, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[lRowNo, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[lRowNo, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[lRowNo, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[lRowNo, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;

            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));

            //ws.Cells[lRowNo, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            //ws.Cells[lRowNo, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            //ws.Cells[lRowNo, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            //ws.Cells[lRowNo, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            //ws.Cells[lRowNo, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            //ws.Cells[lRowNo, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            //ws.Cells[lRowNo, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            //ws.Cells[lRowNo, 8].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            //ws.Cells[lRowNo, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            //ws.Cells[lRowNo, 10].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            //ws.Cells[lRowNo, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            //ws.Cells[lRowNo, 12].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            //ws.Cells[lRowNo, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));

            lRowNo = 2;
            List<OrderDto> OrdersList = _IOrder.GetCabOrders(FromReqDelDate, ToReqDelDate, Customercode, Project, Producttype, Status);
            
            if (OrdersList.Count>0)
                            {
                                var range1 = ws.Cells[lRowNo, 1, OrdersList.Count+1, 29];
                                range1.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                var b = range1.Style.Border;
                                b.Top.Style = ExcelBorderStyle.Thin;
                                b.Bottom.Style = ExcelBorderStyle.Thin;
                                b.Left.Style = ExcelBorderStyle.Thin;
                                b.Right.Style = ExcelBorderStyle.Thin;
                                for (int j=0;j<OrdersList.Count;j++)
                                {
                                    
                                    //ws.Cells[j + lRowNo, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    //ws.Cells[j + lRowNo, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    //ws.Cells[j + lRowNo, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    //ws.Cells[j + lRowNo, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    //ws.Cells[j + lRowNo, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    //ws.Cells[j + lRowNo, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    //ws.Cells[j + lRowNo, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    //ws.Cells[j + lRowNo, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    //ws.Cells[j + lRowNo, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    //ws.Cells[j + lRowNo, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    //ws.Cells[j + lRowNo, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    //ws.Cells[j + lRowNo, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                                    ws.Cells[j + lRowNo, 1].Value = j + 1;
                                    ws.Cells[j + lRowNo, 2].Value = OrdersList[j].OrderRequestNo == null ? "" : OrdersList[j].OrderRequestNo.ToString().Trim(); //"Order No\n加工表号";
                                    ws.Cells[j + lRowNo, 3].Value = OrdersList[j].OrderNo == null ? "" : OrdersList[j].OrderNo.ToString().Trim();
                                    ws.Cells[j + lRowNo, 4].Value = OrdersList[j].CustCode == null ? "" : OrdersList[j].CustCode.ToString().Trim();
                                    ws.Cells[j + lRowNo, 5].Value =  OrdersList[j].CustName == null ? "" : OrdersList[j].CustName.ToString().Trim();
                                    ws.Cells[j + lRowNo, 6].Value =  OrdersList[j].ProjNo == null ? "" : OrdersList[j].ProjNo.ToString().Trim();

                                    ws.Cells[j + lRowNo, 7].Value = OrdersList[j].ProjName == null ? "" : OrdersList[j].ProjName.ToString().Trim();
                                    ws.Cells[j + lRowNo, 8].Value = OrdersList[j].CustPoNo == null ? "" : OrdersList[j].CustPoNo.ToString().Trim();

                                    ws.Cells[j + lRowNo, 9].Value = OrdersList[j].ProductType == null ? "" : OrdersList[j].ProductType.ToString().Trim();
                                    ws.Cells[j + lRowNo, 10].Value = OrdersList[j].AssignmentStatus == null ? "" : OrdersList[j].AssignmentStatus.ToString().Trim();

                                    ws.Cells[j + lRowNo, 11].Value = OrdersList[j].AssignedTo == null ? "" : OrdersList[j].AssignedTo.ToString().Trim();

                                   // ws.Cells[j + lRowNo, 11].Value = (lRst.GetValue(8) == DBNull.Value ? "" : (lRst.GetString(8).Substring(0, 1) == "," ? lRst.GetString(8).Substring(1) : lRst.GetString(8))); //"Required Date\n到场日期";
                                    ws.Cells[j + lRowNo, 12].Value = OrdersList[j].NoofCuts == null ? "" : OrdersList[j].NoofCuts.ToString().Trim();
                                    ws.Cells[j + lRowNo, 13].Value = OrdersList[j].NoofPieces == null ? "" : OrdersList[j].NoofPieces.ToString().Trim();
                                    ws.Cells[j + lRowNo, 14].Value = OrdersList[j].SLBendings == null ? "" : OrdersList[j].SLBendings.ToString().Trim(); //"Order No\n加工表号";
                                    ws.Cells[j + lRowNo, 15].Value = OrdersList[j].SLPieces == null ? "" : OrdersList[j].SLPieces.ToString().Trim();
                                    ws.Cells[j + lRowNo, 16].Value = OrdersList[j].SCBMRun == null ? "" : OrdersList[j].SCBMRun.ToString().Trim();
                                    ws.Cells[j + lRowNo, 17].Value = OrdersList[j].SCBPieces == null ? "" : OrdersList[j].SCBPieces.ToString().Trim();
                                    ws.Cells[j + lRowNo, 18].Value = OrdersList[j].SBCPieces == null ? "" : OrdersList[j].SBCPieces.ToString().Trim();
                                    
                                    ws.Cells[j + lRowNo, 19].Value = OrdersList[j].CouplrBrPeices == null ? "" : OrdersList[j].CouplrBrPeices.ToString().Trim();

                                    ws.Cells[j + lRowNo, 20].Value = OrdersList[j].CouplrEnds == null ? "" : OrdersList[j].CouplrEnds.ToString().Trim();
                                    ws.Cells[j + lRowNo, 21].Value = OrdersList[j].ContractNo == null ? "" : OrdersList[j].ContractNo.ToString().Trim();

                                    ws.Cells[j + lRowNo, 22].Value = OrdersList[j].CustOrdDate == null ? "" : OrdersList[j].CustOrdDate.ToString().Trim();
                                    ws.Cells[j + lRowNo, 23].Value = OrdersList[j].ReqDelDatefr == null ? "" : OrdersList[j].ReqDelDatefr.ToString().Trim();

                                    ws.Cells[j + lRowNo, 24].Value = OrdersList[j].ReqDelDateto == null ? "" : OrdersList[j].ReqDelDateto.ToString().Trim();

                                    // ws.Cells[j + lRowNo, 11].Value = (lRst.GetValue(8) == DBNull.Value ? "" : (lRst.GetString(8).Substring(0, 1) == "," ? lRst.GetString(8).Substring(1) : lRst.GetString(8))); //"Required Date\n到场日期";
                                    ws.Cells[j + lRowNo, 25].Value = OrdersList[j].ProjSeg == null ? "" : OrdersList[j].ProjSeg.ToString().Trim();
                                    ws.Cells[j + lRowNo, 26].Value = OrdersList[j].ProjSubSeg == null ? "" : OrdersList[j].ProjSubSeg.ToString().Trim();
                                    ws.Cells[j + lRowNo, 27].Value = OrdersList[j].CreateBy == null ? "" : OrdersList[j].CreateBy.ToString().Trim(); //"Order No\n加工表号";
                                    ws.Cells[j + lRowNo, 28].Value = OrdersList[j].CreateDate == null ? "" : OrdersList[j].CreateDate.ToString().Trim();
                                    ws.Cells[j + lRowNo, 29].Value = OrdersList[j].Score == null ? "" : OrdersList[j].Score.ToString().Trim();

                }
                            }
                            
         
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


        [HttpPost]
        [Route("/SaveOrderWithdraw")]
        //public ActionResult SaveOrderWithdraw([FromBody] OrderAssignmentWD request)
        public ActionResult SaveOrderWithdraw([FromBody] List<string> orderRequestNo)
        {
            //var content = _IOrder.SaveOrderWithdraw(request.OrderRequestNo, request.WithdrawBy);
            var content = _IOrder.SaveOrderWithdraw(orderRequestNo);
            //return Json(content, JsonRequestBehavior.AllowGet);
            return Ok(content);
        }

        

        [HttpGet]
        [Route("/OutsourceOrderAssignment")]
        public async Task<IActionResult> GetOrdersOutsource(DateTime? FromReqDelDate = null, DateTime? ToReqDelDate = null, string? Customercode = null,
            string? Project = null, string? Producttype = null, string? Status = null, string? OrderNo = null)
        {
            List<OrderDto> OrdersList = _IOrder.GetOrdersOutsource(FromReqDelDate, ToReqDelDate, Customercode, Project, Producttype, Status, OrderNo);
            var result = OrdersList;
            return Ok(result);
        }

        [HttpPost]
        [Route("/getFullOrdersDetails")]
        public async Task<IActionResult> GetFullOrdersDetails([FromBody] List<string> orderRequestNos)
        {
            var content = await _IOrder.GetFullOrdersDetails(orderRequestNos);
            return Ok(content);
        }

        //[HttpGet]
        //[Route("/OrderAssignmentSummary")]
        //public async Task<IActionResult> GetOrderAssignmntSummary(DateTime? FromReqDelDate = null, DateTime? ToReqDelDate = null)
        //{
        //    List<OrdAssgnSummary> OrdersList = _IOrder.GetOrderAssgnmntSummary(FromReqDelDate, ToReqDelDate);
        //    var result = OrdersList;
        //    return Ok(result);
        //}

        [HttpPost]
        [Route("/GenerateBatch")]
        public async Task<IActionResult> GenerateBatches([FromBody]GenerateBatchRequest request)
        {
            if (request.SoNums == null || !request.SoNums.Any())
                return BadRequest(new { success = false, message = "No order numbers provided" });

            var result = await _IOrder.GenerateBatchForOneAsync(request);

            if(result.IsSuccess)
            return Ok(new
            {
                success = result.IsSuccess,
                message = result.Message
            });
            else
            {
                return Ok(new
                {
                    success = result.IsSuccess,
                    message = result.Message
                });
            }
        }


        [HttpPost]
        [Route("/assignOrderOutSource")]
        public async Task<IActionResult> AssignOrderOutsource([FromBody] GenerateBatchRequest req)
        {
            if (req.SoNums == null || !req.SoNums.Any())
                return BadRequest(new { success = false, message = "No order numbers provided" });

            var result = await _IOrder.AssignOrderOutsource(req);

            return Ok(new
            {
                success = result.IsSuccess,
                message = result.Message
            });
        }

        [HttpPost]
        [Route("/DeleteOutsourceData")]
        public async Task<IActionResult> Delete_OutsourceData([FromBody] List<string> orderNo)
        {
            //dynamic content;
            //content =null;
            (bool Success, string Message) result = (true, "No orders processed");
            if (orderNo != null && orderNo.Count > 0)
            {
                for (int i = 0; i < orderNo.Count; i++)
                {
                    result = await _IOrder.Delete_OutsourceData(orderNo[i]);
                    if (!result.Success)
                    {
                        return Ok(new { success = result.Success, message = result.Message });
                    }
                }
            }
            return Ok(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        [Route("/PostSupplierData")]
        public async Task<IActionResult> PostSupplierData([FromQuery] string? productType)
        {
            try
            {
                var result = await _IOrder.PostSupplierDataAsync(productType);

                if (!result.IsSuccess)
                {
                    return Ok(new { code = 400, success = false, message = result.Message });
                }

                return Ok(new
                {
                    code = 200,
                    success = true,
                    message = "Data processed successfully.",
                    data = result.Data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = 500,
                    success = false,
                    message = "Internal Server Error",
                    error = ex.Message
                });
            }
        }
        
        [HttpGet]
        [Route("/GetSupplier_BatchPrinting")]
        public async Task<IActionResult> GetSupplier_BatchPrinting()
        {
            IEnumerable<dynamic> result = await _IOrder.GetSupplier_BatchPrinting();
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetSONO_BatchPrinting/{NoofDays}/{Vendorcode}")]
        public async Task<IActionResult> GetSONO_BatchPrinting(int NoofDays, string Vendorcode)
        {
            IEnumerable<dynamic> result = await _IOrder.GetSONO_BatchPrinting(NoofDays, Vendorcode);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetBatch_BatchPrinting/{SONo}")]
        public async Task<IActionResult> GetBatch_BatchPrinting(string SONo)
        {
            IEnumerable<dynamic> result = await _IOrder.GetBatch_BatchPrinting(SONo);
            return Ok(result);
        }
    }
}
 