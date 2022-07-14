﻿using System.Collections.Generic;
using System.Web.Mvc;

namespace System.ComponentModel.DataAnnotations
{
    public class CustomCompareDateAttribute : ValidationAttribute, IClientValidatable
    {
        public CustomCompareDateAttribute(string startDate, string endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var containerType = validationContext.ObjectInstance.GetType();
            var startDate = containerType.GetProperty(StartDate);
            var endDate = containerType.GetProperty(EndDate);

            if (startDate != null && endDate != null)
            {
                DateTime startDateValue = (DateTime)startDate.GetValue(validationContext.ObjectInstance, null);
                DateTime endDateValue = (DateTime)endDate.GetValue(validationContext.ObjectInstance, null);

                if (startDateValue.AddYears(5) >= endDateValue)
                {
                    return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
                }              
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "comparedategreaterthanfiveyears",
            };

            rule.ValidationParameters.Add("startdate", StartDate);
            rule.ValidationParameters.Add("enddate", EndDate);

            yield return rule;
        }

    }
}
