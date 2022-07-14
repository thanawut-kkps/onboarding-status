using System.Collections.Generic;
using System.Web.Mvc;

namespace System.ComponentModel.DataAnnotations
{
    public class RequiredItemClientOnlyAttribute : ValidationAttribute, IClientValidatable
    {
        private int _minimumItem;

        public RequiredItemClientOnlyAttribute()
        {
            _minimumItem = 1;
            ErrorMessage = "Please select item at least 1 item";
        }

        public int MinimumItem
        {
            get { return _minimumItem; }
            set { _minimumItem = value; }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = String.IsNullOrEmpty(ErrorMessage) ? FormatErrorMessage(metadata.DisplayName) : ErrorMessage,
                ValidationType = "requireditemclientonly"
            };
        }
    }
}
