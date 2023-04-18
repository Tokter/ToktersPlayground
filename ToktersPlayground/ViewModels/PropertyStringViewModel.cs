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
        public string Value
        {
            get
            {
                return Property?.GetValue(Item)?.ToString() ?? string.Empty;
            }
            set
            {
                Property?.SetValue(Item, value);
                this.RaisePropertyChanged();
            }
        }
    }
}
