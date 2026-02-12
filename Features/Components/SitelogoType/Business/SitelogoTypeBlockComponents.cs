using DemoTraining.Business.Rendering;
using DemoTraining.Features.Components.SitelogoType.Models;
using EPiServer.Framework.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DemoTraining.Features.Components.SitelogoType.Business
{
    [TemplateDescriptor(ModelType = typeof(SiteLogotypeBlock))]
    public class SitelogoTypeBlockComponents : FeatureBlockComponent<SiteLogotypeBlock>
    {
        protected override IViewComponentResult InvokeComponent(SiteLogotypeBlock currentBlock)
               => FeatureView("SiteLogotype", currentBlock);
    }
}
