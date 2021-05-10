using System;
using System.Linq;
using Core.CustomAttributes.Validation.Base;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.CustomAttributes.Validation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class OnlyOneInSceneAttribute : ClassValidationAttribute
    {
        private Type _type;

        public OnlyOneInSceneAttribute(bool showError = false) : base(showError)
        {
        }

        public override bool Validate(Object instance)
        {
            bool isValid;

            try
            {
                isValid = !ValidState(instance);
                _error = ErrorText();
            }
            catch (Exception e)
            {
                _error = e.Message;
                isValid = false;
            }
            return isValid;
        }

        private protected override bool ValidState(object obj)
        {
            var mono = Object.FindObjectsOfType<MonoBehaviour>();
            _type = obj.GetType();
            var count = mono.Count(type => obj.GetType() == type.GetType());
            return count > 1;
        }

        private protected override string ErrorText()
        {
            return $"Should be only one instance of <i>{_type}</i> in scene";
        }
    }
}
