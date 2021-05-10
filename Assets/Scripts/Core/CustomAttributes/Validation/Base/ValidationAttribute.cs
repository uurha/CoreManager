using System;

namespace Core.CustomAttributes.Validation.Base
{
    public abstract class ValidationAttribute : Attribute
    {
        private protected string _error;

        public string ErrorMessage => _error;

        public bool ShowError { get; }

        protected ValidationAttribute(bool showError = false)
        {
            ShowError = showError;
        }

        private protected abstract bool ValidState(object obj);

        private protected abstract string ErrorText();
    }

}
