using System.Collections.Generic;
using System.Web.Mvc;

namespace System.ComponentModel.DataAnnotations
{
    public class CustomMinimumCurrentDateAttribute : ValidationAttribute, IClientValidatable
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime objValue = (DateTime)value;

            var mindate = DateTime.Now;
            mindate = mindate.AddYears(Year);
            mindate = mindate.AddMonths(Month);
            mindate = mindate.AddDays(Day);

            if (objValue <= mindate.Date) return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "dategreaterthancurrent",
            };

            rule.ValidationParameters.Add("addyear", Year);
            rule.ValidationParameters.Add("addmonth", Month);
            rule.ValidationParameters.Add("addday", Day);

            yield return rule;
        }


    }
}