namespace ShapeCodeService.Dtos
{
    public class SearchResultDto<T>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Required by automapper")]
        public List<T> Results { get; set; } = new List<T>();

        public int Total { get; set; }
    }
}
