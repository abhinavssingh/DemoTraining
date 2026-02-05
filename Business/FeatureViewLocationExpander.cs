using Microsoft.AspNetCore.Mvc.Razor;

namespace DemoTraining.Business
{
    public class FeatureViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            // see: https://stackoverflow.com/questions/36802661/what-is-iviewlocationexpander-populatevalues-for-in-asp-net-core-mvc
            context.Values["action_displayname"]
                = context.ActionContext.ActionDescriptor.DisplayName;
        }

        public IEnumerable<string> ExpandViewLocations(
            ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (viewLocations == null)
                throw new ArgumentNullException(nameof(viewLocations));

            var controllerDescriptor = context.ActionContext.ActionDescriptor;
            var viewName = GetViewName(context.ViewName);
            var featureName = controllerDescriptor.Properties.ContainsKey("feature")
                ? controllerDescriptor.Properties["feature"] as string
                : "";

            foreach (var location in viewLocations)
            {
                yield return location
                    .Replace("{feature}", featureName)
                    .Replace("{0}", viewName);
            }
        }

        private string GetViewName(string viewName)
        {
            // Workaround: remove Block part from view name because folder structure for blocks used without Block suffix
            if (viewName.Contains("Components") && viewName.Contains("Block"))
            {
                var splitedParts = viewName.Split("/");
                splitedParts[1] = splitedParts[1].Replace("Block", string.Empty);
                return string.Join('/', splitedParts);
            }

            return viewName;
        }
    }
}
