namespace DemoTraining.Features.Search.Models
{
    public class FacetBucket
    {
        public string Name { get; set; }
        public IList<FacetItem> Items { get; set; } = new List<FacetItem>();
    }
}
