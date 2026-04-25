using DemoTraining.Features.PageValidation.Models;
using EPiServer.DataAnnotations;
using EPiServer.Framework.Serialization;
using EPiServer.ServiceLocation;

namespace DemoTraining.Features.PageValidation.Business
{
    // TODO CMS13: PropertyDefinitionTypePlugInAttribute is obsolete, use PropertyDefinitionTypeAttribute instead
    [PropertyDefinitionType(DisplayName = "List of people i.e. IList<Person>",
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
