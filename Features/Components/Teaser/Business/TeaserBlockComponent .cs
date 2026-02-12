using DemoTraining.Business.Rendering;
using DemoTraining.Features.Components.Teaser.Models;
using EPiServer.Framework.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DemoTraining.Features.Components.Teaser.Business
{
    [TemplateDescriptor(ModelType = typeof(TeaserBlock))]
    public class TeaserBlockComponent : FeatureBlockComponent<TeaserBlock>
    {
        protected override IViewComponentResult InvokeComponent(TeaserBlock currentBlock)
                        => FeatureView("Teaser", currentBlock);
    }
}
