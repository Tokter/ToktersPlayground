using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.ViewModels
{
    public class PropertyByteViewModel : PropertyBase
    {
        public PropertyByteViewModel(PropertyEditorViewModel propertyEditor, PropertyDetail details) : base(propertyEditor, details)
        {
        }

        public byte Value
        {
            get => (byte)Editor.GetValue(Details, 0);
            set
            {
                Editor.SetValue(Details, value);
                this.RaisePropertyChanged();
            }
        }

        private string? _valueText = null;
        public string ValueText
        {
            get
            {
                if (_valueText != null) return _valueText;
                return Value.ToString();
            }
            set
            {
                if (byte.TryParse(value, out byte result))
                {
                    Value = result;
                    _valueText = null;
                    ClearError();
                }
                else
                {
                    _valueText = value;
                    SetError($"{_valueText} is not a valid byte!");
                }
                this.RaisePropertyChanged();
            }
        }

        public override void Update()
        {
            this.RaisePropertyChanged(nameof(Value));
            this.RaisePropertyChanged(nameof(ValueText));
        }
    }
}
