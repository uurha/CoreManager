using System;
using System.Collections;
using System.Reflection;
using Object = UnityEngine.Object;

namespace Core.CustomAttributes.Validation.Base
{
    public abstract class FieldValidationAttribute : ValidationAttribute
    {
        protected FieldValidationAttribute(bool showError = false) : base(showError)
        {
        }

        public virtual bool Validate(FieldInfo field, Object instance)
        {
            bool isValid;

            try
            {
                var value = field.GetValue(instance);

                if (value is IEnumerable list)
                {
                    isValid = true;
                    var index = -1;

                    foreach (var item in list)
                    {
                        index++;

                        if (!item.Equals(null) &&
                            ValidState(item))
                            continue;
                        isValid = false;

                        _error =
                            $"<b>Element[{index}]</b> {ErrorText()}\ninto Field: <i>{field.Name}</i>\non GameObject: {instance.name}";
                        break;
                    }
                }
                else
                {
                    _error =
                        $"Object <b>\"{((Object) value)?.name}\"</b> {ErrorText()}\ninto Field: <i>{field.Name}</i> on GameObject: {instance.name}";
                    isValid = value != null && ValidState(value);
                }
            }
            catch (Exception e)
            {
                _error = e.Message;
                isValid = false;
            }
            return isValid;
        }
    }
}
