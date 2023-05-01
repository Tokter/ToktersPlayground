using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.ViewModels
{
    public class PropertyIntViewModel : PropertyBase
    {
        public PropertyIntViewModel(PropertyEditorViewModel propertyEditor, PropertyDetail details) : base(propertyEditor, details)
        {
        }

        public int Value
        {
            get => (int)Editor.GetValue(Details, 0);
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
                if (int.TryParse(value, out int result))
                {
                    Value = result;
                    _valueText = null;
                    ClearError();
                }
                else
                {
                    _valueText = value;
                    SetError($"{_valueText} is not a valid int!");
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
