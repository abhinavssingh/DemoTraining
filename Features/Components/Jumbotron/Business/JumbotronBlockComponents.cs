using DemoTraining.Business.Rendering;
using DemoTraining.Features.Components.Jumbotron.Models;
using EPiServer.Framework.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DemoTraining.Features.Components.Jumbotron.Business
{
    [TemplateDescriptor(ModelType = typeof(JumbotronBlock))]
    public class JumbotronBlockComponents : FeatureBlockComponent<JumbotronBlock>
    {
        protected override IViewComponentResult InvokeComponent(JumbotronBlock currentBlock)
               => FeatureView("Jumbotron", currentBlock);
    }
}
