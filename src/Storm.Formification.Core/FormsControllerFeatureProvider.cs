using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Storm.Formification.Core
{
    public class FormsControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly IFormLocator locator;

        public FormsControllerFeatureProvider(IFormLocator locator)
        {
            this.locator = locator;
        }

        public FormsControllerFeatureProvider(params Assembly[] assemblies) : this(new FormLocator(assemblies))
        {

        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            foreach (var type in locator.All())
            {
                var typeName = type.Name + "Controller";
                if (!feature.Controllers.Any(t => t.Name == typeName))
                {
                    var controllerType = typeof(Forms.FormsController<>).MakeGenericType(type).GetTypeInfo();
                    feature.Controllers.Add(controllerType);
                }
            }
        }
    }
}
