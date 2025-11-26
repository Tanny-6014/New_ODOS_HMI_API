using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrderService.Dtos
{
    public class AmendmentDto
    {
        public SelectList lProjectSelection { get; set; }
        public SelectList lCustomerSelection { get; set; }

    }
}
