using Microsoft.AspNetCore.Mvc;
using OrderService.Dtos;
using OrderService.Interfaces;
using OrderService.Models;

namespace OrderService.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class InterfaceController : Controller
    {
        private readonly IInterface _IInterface;
        public InterfaceController(IInterface Interface)
        {
            _IInterface= Interface ?? throw new ArgumentNullException(nameof(Interface));
        }

        [HttpPost]
        [Route("/Delete_OrderAssignmentData")]
        public async Task<IActionResult> Delete_OrderAssignmentData([FromBody] List<string> orderRequestNo)
        {
            //dynamic content;
            //content =null;
            (bool Success, string Message) result = (true, "No orders processed");
            if (orderRequestNo != null && orderRequestNo.Count > 0)
            {
                for (int i = 0; i < orderRequestNo.Count; i++)
                {
                    result = await _IInterface.Delete_OrderAssignmentData(orderRequestNo[i]);
                    if (!result.Success)
                    {
                        return Ok(new { success = result.Success, message = result.Message });
                    }
                }
            }
            return Ok(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        [Route("/Delete_ODOS_Scnhell_Data")]
        public async Task<IActionResult> Delete_ODOS_Scnhell_Data([FromBody] List<string> orderRequestNo)
        {
            //dynamic content;
            //content = null;
            (bool Success, string Message) result = (true, "No orders processed");
            if (orderRequestNo != null && orderRequestNo.Count > 0)
            {
                for (int i = 0; i < orderRequestNo.Count; i++)
                {
                    //content = _IInterface.Delete_ODOS_Scnhell_Data(orderRequestNo[i]);
                    result = await _IInterface.Delete_ODOS_Scnhell_Data(orderRequestNo[i]);
                    if (!result.Success)
                    {
                        return Ok(new { success = result.Success, message = result.Message });
                    }
                }
            }
            return Ok(new { success = result.Success, message = result.Message });
        }

        [HttpPut]
        [Route("/Update_OrderAssignmentData")]
        public async Task<IActionResult> Update_OrderAssignmentData(string orderNo, int LayerNo, string LoadNo)
        {
            try
            {
                var result = await _IInterface.Update_OrderAssignmentData(orderNo,LayerNo,LoadNo);

                if (!result.IsSuccess)
                {
                    return Ok(new { success = false, message = result.Message });
                }

                return Ok(new { success = true, message = result.Message });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPut]
        [Route("/Update_OrderAssignmentData_H")]
        public async Task<IActionResult> Update_OrderAssignmentData(string OrderRequestNo)
        {
            try
            {
                var result = await _IInterface.Update_OrderAssignmentData(OrderRequestNo);

                if (!result.IsSuccess)
                {
                    return Ok(new { success = false, message = result.Message });
                }

                return Ok(new { success = true, message = result.Message });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPut]
        [Route("/Update_OrderAssignmentData_Schedule")]
        public async Task<IActionResult> Update_OrderAssignmentData_Schedule()
        {
            try
            {
                var result = await _IInterface.Update_OrderAssignmentData_Schedule();

                if (!result.IsSuccess)
                {
                    return Ok(new { success = false, message = result.Message });
                }

                return Ok(new { success = true, message = result.Message });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("/Insert_OrderAssignmentData")]
        public async Task<IActionResult> POST_OrderAssignmentData()
        {
            try
            {
                var result = await _IInterface.POST_OrderAssignmentData();

                if (!result.IsSuccess)
                {
                    return Ok(new { success = false, message = result.Message });
                }

                return Ok(new { success = true, message = result.Message });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }


    }
}
