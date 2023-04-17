namespace ProvaPub.Models
{
    public class Pagination
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public bool HasNext { get; set; }
    }
}
