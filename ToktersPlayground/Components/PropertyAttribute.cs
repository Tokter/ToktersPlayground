using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.Components
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PropertyAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public string StringFormat { get; set; }

        public PropertyAttribute()
        {
            DisplayName = string.Empty;
            StringFormat = string.Empty;
        }

        public PropertyAttribute(string name)
        {
            DisplayName = name;
            StringFormat = string.Empty;
        }

        public PropertyAttribute(string name, string stringFormat)
        {
            DisplayName = name;
            StringFormat = stringFormat;
        }
    }
}
