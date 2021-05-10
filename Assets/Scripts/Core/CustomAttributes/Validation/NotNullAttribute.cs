using System;
using Core.CustomAttributes.Validation.Base;

namespace Core.CustomAttributes.Validation
{
    [AttributeUsage(AttributeTargets.Field)]
    public class NotNullAttribute : FieldValidationAttribute
    {
        public NotNullAttribute(bool showError = false) : base(showError)
        {
        }

        private protected override bool ValidState(object obj)
        {
            return obj != null;
        }

        private protected override string ErrorText()
        {
            return "cannot be null";
        }
    }
}
