using System.Collections.Generic;
using System.Web.Mvc;

namespace System.ComponentModel.DataAnnotations
{
    public class EnforceTrueAttribute : ValidationAttribute, IClientValidatable
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //Not implement yet.
            //use attribute for Client validation.
            //if (value == null) 
            //    return new ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });

            //if (value.GetType() != typeof(bool)) throw new InvalidOperationException("can only be used on boolean properties.");
            //return (bool)value == true;
            return ValidationResult.Success;

        }

        public override string FormatErrorMessage(string name)
        {
            return "The " + name + " field must be checked in order to continue.";
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = string.IsNullOrEmpty(ErrorMessage) ? FormatErrorMessage(metadata.DisplayName) : ErrorMessage,
                ValidationType = "enforcetrue"
            };
        }
    }
}
