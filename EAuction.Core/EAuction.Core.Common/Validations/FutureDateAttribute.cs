using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EAuction.Core.Common.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class FutureDate : ValidationAttribute
    {
        public FutureDate()
        {

        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Date is required");
            }
            if (Convert.ToDateTime(value).Date <= DateTime.Now.Date)
            {
                return new ValidationResult("Date should be future date");
            }
            return base.IsValid(value, validationContext);
        }
    }
}
