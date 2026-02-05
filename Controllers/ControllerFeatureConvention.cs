using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace DemoTraining.Controllers
{

    public class ControllerFeatureConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var name = DeriveFeatureFolderName(controller);
            controller.Properties.Add("feature", name);
        }

        private string DeriveFeatureFolderName(ControllerModel model)
        {
            var @namespace = model.ControllerType.Namespace;
            var result = @namespace.Split('.')
                .SkipWhile(s => s != "Features")
                .Aggregate("", Path.Combine);

            return result;
        }
    }
}
