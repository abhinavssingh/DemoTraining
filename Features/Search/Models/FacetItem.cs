namespace DemoTraining.Features.Search.Models
{
    public class FacetItem
    {
        public string Term { get; set; }
        public string Display { get; set; }
        public int Count { get; set; }
        public bool Selected { get; set; }
    }
}
