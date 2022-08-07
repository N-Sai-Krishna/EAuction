using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Seller.Core.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class IsValidCategory: ValidationAttribute
    {
        private readonly IList<string> values;

        public IsValidCategory()
        {
            this.values = new List<string>() { "Painting", "Ornament", "Scluptor"};
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult($"Value should exists & fall in any one of the following category - {String.Join(',',values)} ");
            }
            if (!this.values.Any(s => s.Equals(value.ToString(),StringComparison.InvariantCultureIgnoreCase)))
            {
                return new ValidationResult($"Value should fall in any one of the following category - {String.Join(',', values)} ");
            }
            return null;
        }
    }
}
