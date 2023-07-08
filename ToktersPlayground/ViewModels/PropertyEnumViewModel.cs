using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.ViewModels
{
    public class PropertyEnumViewModel : PropertyBase
    {
        private Type? _enumType = null;

        public List<string> Values { get; set; } = new List<string>();

        public PropertyEnumViewModel(PropertyEditorViewModel propertyEditor, PropertyDetail details) : base(propertyEditor, details)
        {
        }

        public Type? EnumType
        {
            get => _enumType;
            set
            {
                if (_enumType != value)
                {
                    _enumType = value;
                    GenerateEnumNames();
                    this.RaisePropertyChanged();
                }
            }
        }


        public string SelectedValue
        {
            get => Editor.GetValue(Details, Values.First()).ToString()!;
            set
            {
                if (EnumType == null || value == null) return;
                Editor.SetValue(Details, Enum.Parse(EnumType, value));
                this.RaisePropertyChanged();
            }
        }

        public void GenerateEnumNames()
        {
            if (EnumType == null) return;
            Values.Clear();
            Values.AddRange(Enum.GetNames(EnumType));
        }

        public override void Update()
        {
            this.RaisePropertyChanged(nameof(SelectedValue));
        }
    }
}
