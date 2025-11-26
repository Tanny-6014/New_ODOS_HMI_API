namespace DrainService.Dtos
{
    public  class Response_New<T>
    {
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }




    }
}
