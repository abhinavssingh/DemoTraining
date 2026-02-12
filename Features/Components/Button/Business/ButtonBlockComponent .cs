using DemoTraining.Business.Rendering;
using DemoTraining.Features.Components.Button.Models;
using EPiServer.Framework.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DemoTraining.Features.Components.Button.Business
{
    [TemplateDescriptor(ModelType = typeof(ButtonBlock))]
    public class ButtonBlockComponent : FeatureBlockComponent<ButtonBlock>
    {
        protected override IViewComponentResult InvokeComponent(ButtonBlock currentBlock)
                => FeatureView("Button", currentBlock);
    }
}
