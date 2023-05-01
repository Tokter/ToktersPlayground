using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.ViewModels
{
    public class PropertyStringViewModel : PropertyBase
    {
        public PropertyStringViewModel(PropertyEditorViewModel propertyEditor, PropertyDetail details) : base(propertyEditor, details)
        {            
        }

        public string Value
        {
            get => Editor.GetValue(Details, string.Empty).ToString() ?? string.Empty;
            set
            {
                Editor.SetValue(Details, value);
                this.RaisePropertyChanged();
            }
        }

        public override void Update()
        {
            this.RaisePropertyChanged(nameof(Value));
        }
    }
}
