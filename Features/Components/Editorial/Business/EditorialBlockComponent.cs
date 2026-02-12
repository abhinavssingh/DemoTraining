using DemoTraining.Business.Rendering;
using DemoTraining.Features.Components.Editorial.Models;
using EPiServer.Framework.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DemoTraining.Features.Components.Editorial.Business
{
    [TemplateDescriptor(ModelType = typeof(EditorialBlock))]
    public class EditorialBlockComponent : FeatureBlockComponent<EditorialBlock>
    {
        protected override IViewComponentResult InvokeComponent(EditorialBlock currentBlock)
               => FeatureView("Editorial", currentBlock);
    }
}
