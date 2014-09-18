using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DashboardSite.Model
{
    public class DifferFromAttribute : ValidationAttribute
    {
        public DifferFromAttribute(params string[] propertyNames)
        {
            this.PropertyNames = propertyNames;
        }

        public string[] PropertyNames { get; private set; }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherProperties = this.PropertyNames.Select(validationContext.ObjectType.GetProperty);            
            var otherValues = otherProperties.Select(p => p.GetValue(validationContext.ObjectInstance, null)); //.OfType<string>();

            var thisValue = value;
            foreach(var item in otherValues)
            {
                if (item.Equals(thisValue))
                    return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }
    }
}