using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Razor;

namespace DemoTraining.Business
{
    public class FeatureViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context) { }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var featureLocations = new[] {
                // With Views subfolder (matches project layout)
                "/Features/{1}/Views/{0}.cshtml",
                "/Features/Components/{1}/Views/{0}.cshtml",
                "/Features/Blocks/{1}/Views/{0}.cshtml",
                // Fallbacks without Views folder
                "/Features/{1}/{0}.cshtml",
                "/Features/Components/{1}/{0}.cshtml",
                "/Features/Blocks/{1}/{0}.cshtml",
            };

            // Preserve the original view locations and prepend feature locations so they are searched first
            return featureLocations.Concat(viewLocations);
        }
    }
}
