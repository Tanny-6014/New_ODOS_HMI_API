using OrderService.Models;

namespace OrderService.Dtos
{
    public class SaveOrderDetailsDto
    {
        public OrderProjectModels pOrderHeader { get; set; }
        public List<OrderProjectSEModels> pOrderCart { get; set; }

    }
}
