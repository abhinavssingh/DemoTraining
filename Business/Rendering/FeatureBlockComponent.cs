using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace DemoTraining.Business.Rendering
{

    public abstract class FeatureBlockComponent<T> : BlockComponent<T> where T : BlockData
    {
        protected IViewComponentResult FeatureView(string featureFolder, T model)
            => View($"~/Features/Components/{featureFolder}/Views/Index.cshtml", model);
    }

}
