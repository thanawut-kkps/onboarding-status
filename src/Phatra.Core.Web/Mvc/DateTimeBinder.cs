using System;
using System.Web.Mvc;
using Phatra.Core.Web.Constants;

namespace Phatra.Core.Web.Mvc
{
    public class DateTimeBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (bindingContext.ModelMetadata.DataTypeName == DateTimeConst.Name)
            {
                if (string.IsNullOrEmpty(value.AttemptedValue)) return null;
                DateTime d;
                var date = DateTime.TryParseExact(value.AttemptedValue, DateTimeConst.Format, new System.Globalization.CultureInfo("en-us"), System.Globalization.DateTimeStyles.None, out d);
                if (date) return d;
                bindingContext.ModelState.AddModelError(bindingContext.ModelMetadata.PropertyName, "The value " + bindingContext.ModelMetadata.PropertyName + " is not valid.");
            }
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}
