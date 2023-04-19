using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.Components
{
    public enum EditorType
    {
        Default,
        FileLocation,
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PropertyAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public string StringFormat { get; set; }
        public EditorType EditorType { get; set; } = EditorType.Default;

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

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is PropertyAttribute attribute)
            {
                return DisplayName == attribute.DisplayName && StringFormat == attribute.StringFormat && EditorType == attribute.EditorType;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DisplayName, StringFormat, EditorType);
        }
    }
}
