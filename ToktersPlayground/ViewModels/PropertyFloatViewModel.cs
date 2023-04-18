using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.ViewModels
{
    public class PropertyFloatViewModel : PropertyBase
    {
        public string StringFormat { get; set; } = string.Empty;

        public float Value
        {
            get
            {
                return (float?)Property?.GetValue(Item) ?? 0.0f;
            }
            set
            {
                Property?.SetValue(Item, value);
                this.RaisePropertyChanged();
            }
        }

        private string? _valueText = null;
        public string ValueText
        {
            get
            {
                if (_valueText != null) return _valueText;
                if (!string.IsNullOrEmpty(StringFormat))
                {
                    return Value.ToString(StringFormat);
                }
                else
                {
                    return Value.ToString();
                }
            }
            set
            {
                if (float.TryParse(value, out float result))
                {
                    Value = result;
                    _valueText = null;
                    ClearError();
                }
                else
                {
                    _valueText = value;
                    SetError($"{_valueText} is not a valid float!");
                }
                this.RaisePropertyChanged();
            }
        }
    }
}
