using UnityEngine;

namespace Core.CustomAttributes.Validation.Base
{
    public abstract class ClassValidationAttribute : ValidationAttribute
    {
        protected ClassValidationAttribute(bool showError = false) : base(showError)
        {
        }

        public abstract bool Validate(Object instance);
    }
}
