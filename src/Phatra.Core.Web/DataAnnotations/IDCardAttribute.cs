using System.Collections.Generic;
using System.Web.Mvc;

namespace System.ComponentModel.DataAnnotations
{
    public class IDCardAttribute : ValidationAttribute, IClientValidatable
    {
        //private RequiredAttribute _innerAttribute = new RequiredAttribute();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            //if (!_innerAttribute.IsValid(value))
            //{
            //    // validation failed - return an error
            //    return new ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
            //}

            //implement later.
            //Validate Thai ID card.

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = string.IsNullOrEmpty(ErrorMessage) ? FormatErrorMessage(metadata.DisplayName) : ErrorMessage,
                ValidationType = "idcard"
            };
        }
    }
}
