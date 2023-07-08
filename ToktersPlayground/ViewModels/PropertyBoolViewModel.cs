using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.ViewModels
{
    public class PropertyBoolViewModel : PropertyBase
    {
        public PropertyBoolViewModel(PropertyEditorViewModel propertyEditor, PropertyDetail details) : base(propertyEditor, details)
        {
        }

        public bool Value
        {
            get => (bool)Editor.GetValue(Details, false);
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
