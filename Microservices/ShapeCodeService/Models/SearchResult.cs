namespace ShapeCodeService.Models
{
    public class SearchResult<T>
    {
        public SearchResult(IEnumerable<T> results, int total)
        {
            Results = results;
            Total = total;
        }

        public IEnumerable<T> Results { get; }

        public int Total { get; }
    }
}
