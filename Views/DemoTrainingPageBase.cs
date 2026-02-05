using DemoTraining.Business.Rendering;
using EPiServer.ServiceLocation;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DemoTraining.Views
{
    public abstract class DemoTrainingPageBase<TModel> : RazorPage<TModel> where TModel : class
    {
        private readonly ContentAreaItemRenderer _contentAreaItemRenderer;

        public abstract override Task ExecuteAsync();

        public DemoTrainingPageBase() : this(ServiceLocator.Current.GetInstance<ContentAreaItemRenderer>())
        {
        }

        public DemoTrainingPageBase(ContentAreaItemRenderer contentAreaItemRenderer)
        {
            _contentAreaItemRenderer = contentAreaItemRenderer;
        }

        protected void OnItemRendered(ContentAreaItem contentAreaItem, TagHelperContext context, TagHelperOutput output)
        {
            _contentAreaItemRenderer.RenderContentAreaItemCss(contentAreaItem, context, output);
        }
    }
}
