using DemoTraining.Features.PageValidation.Models;
using EPiServer.Framework.Serialization;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;

namespace DemoTraining.Features.PageValidation.Business
{
    [PropertyDefinitionTypePlugIn(DisplayName = "List of people i.e. IList<Person>",
       Description = "An editable list of Person instances.")]
    public class PropertyPersonList : PropertyList<Person>
    {
        public PropertyPersonList()
        {
            _objectSerializer = _objectSerializerFactory.Service.GetSerializer(KnownContentTypes.Json);
        }

        private Injected<IObjectSerializerFactory> _objectSerializerFactory;

        private IObjectSerializer _objectSerializer;

        protected override Person ParseItem(string value)
        {
            return _objectSerializer.Deserialize<Person>(value);
        }
    }
}
