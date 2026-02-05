using EPiServer.Shell.ObjectEditing;

namespace DemoTraining.Features.CmsFieldTypes.Business
{
    public class DayOfWeekSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var list = new List<SelectItem>();
            foreach (var item in Enum.GetValues(typeof(DayOfWeek)))
            {
                list.Add(new SelectItem { Value = item, Text = Enum.GetName(typeof(DayOfWeek), item) });
            }
            return list;
        }
    }
}
