using System;
using System.Linq;
using Core.CustomAttributes.Validation.Base;
using UnityEngine;

namespace Core.CustomAttributes.Validation
{
    [AttributeUsage(AttributeTargets.Field)]
    public class HasComponentAttribute : FieldValidationAttribute
    {
        private readonly Type _requiredType;

        public HasComponentAttribute(Type requiredType, bool showError = false) : base(showError)
        {
            _requiredType = requiredType ?? throw new ArgumentNullException(nameof(requiredType));
        }

        private protected override string ErrorText()
        {
            return $"should have component with type: <i>\"{_requiredType}\"</i>";
        }

        private protected override bool ValidState(object obj)
        {
            return ((GameObject) obj).GetComponents<MonoBehaviour>().Select(x => x.GetType()).Any(type => _requiredType.IsAssignableFrom(type));
        }
    }
}
