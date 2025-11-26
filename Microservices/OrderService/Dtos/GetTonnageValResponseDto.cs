namespace OrderService.Dtos
{
    public class GetTonnageValResponseDto
    {
        public bool success { get; set; }
        
        public decimal tonnage { get; set; }

        public string errorMessage { get; set; }

    }
}
