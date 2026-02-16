namespace DemoTraining.Features.Search.Models
{
    public class SearchQuery
    {
        public string Q { get; set; }
        public List<string> Sections { get; set; } = new();
        public List<string> Types { get; set; } = new();
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

    }
}
