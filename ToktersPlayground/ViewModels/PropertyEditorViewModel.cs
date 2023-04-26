using DynamicData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.Components;

namespace ToktersPlayground.ViewModels
{
    public class PropertyEditorViewModel : ViewModelBase
    {
        private List<object> _selectedObjects = new List<object>();
        private PropertyDetails _propertyDetails = new PropertyDetails();
        public ObservableCollection<PropertyBase> Properties { get; set; } = new ObservableCollection<PropertyBase>();

        public void SelectObjects(IEnumerable<object> objects)
        {
            _selectedObjects.Clear();
            _selectedObjects.AddRange(objects);
            BuildPropertyList();
        }

        public void SelectObject(object? obj)
        {
            _selectedObjects.Clear();
            if (obj != null)
            {
                _selectedObjects.Add(obj);
            }
            BuildPropertyList();
        }

        public void ClearSelection()
        {
            _selectedObjects.Clear();
            BuildPropertyList();
        }

        private void BuildPropertyList()
        {
            _propertyDetails.Clear();
            Properties.Clear();

            foreach (var obj in _selectedObjects)
            {
                var type = obj.GetType();
                var properties = type.GetProperties();
                var details = new List<PropertyDetail>();
                foreach (var property in properties)
                {
                    var propertyAttribute = property.GetCustomAttributes(typeof(PropertyAttribute), false).FirstOrDefault() as PropertyAttribute;
                    if (propertyAttribute != null)
                    {
                        details.Add(new PropertyDetail(property, propertyAttribute));
                    }
                }
                _propertyDetails.Add(details);
            }

            foreach (var pd in _propertyDetails.OrderBy(p=>p.Attribute.DisplayName))
            {
                switch (pd.Attribute.EditorType)
                {
                    case EditorType.Default:
                        switch (pd.Property.PropertyType.Name)
                        {
                            case "String":
                                var stringProperty = new PropertyStringViewModel(this, pd);
                                stringProperty.Name = pd.Attribute.DisplayName ?? pd.Property.Name;
                                Properties.Add(stringProperty);
                                break;

                            case "Single":
                                var floatProperty = new PropertyFloatViewModel(this, pd);
                                floatProperty.Name = pd.Attribute.DisplayName ?? pd.Property.Name;
                                floatProperty.StringFormat = pd.Attribute.StringFormat;
                                Properties.Add(floatProperty);
                                break;
                        }
                        break;

                    case EditorType.FileLocation:
                        break;
                }
            }
        }

        public void SetValue(PropertyDetail property, object value)
        {
            foreach (var obj in _selectedObjects)
            {
                property.Property.SetValue(obj, value);
            }
        }
        
        public object GetValue(PropertyDetail property, object defaultValue)
        {
            //If all selected objects have the same value, return that value, otherwise return the default value
            object? value = null;
            foreach (var obj in _selectedObjects)
            {
                var objValue = property.Property.GetValue(obj);
                if (value == null)
                {
                    value = objValue;
                }
                else if (!value.Equals(objValue))
                {
                    return defaultValue;
                }
            }
            return value ?? defaultValue;
        }
    }

    public class PropertyDetail
    {
        public PropertyInfo Property { get; set; }

        public PropertyAttribute Attribute { get; set; }

        public PropertyDetail(PropertyInfo info, PropertyAttribute attribute)
        {
            Property = info;
            Attribute = attribute;
        }

        public override bool Equals(object? obj)
        {
            if (obj is PropertyDetail detail)
            {
                return detail.Attribute == Attribute
                    && detail.Property == Property;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Property, Attribute);
        }
    }

    /// <summary>
    /// Holds a list of all common properties for the selected objects
    /// </summary>
    public class PropertyDetails : Collection<PropertyDetail>
    {
        /// <summary>
        /// If our list is empty, we have no common properties and can just add all properties
        /// If we contain properties, we want to remove any properties that are not common to all selected objects
        /// </summary>
        /// <param name="items">List of properties to add</param>
        public void Add(IEnumerable<PropertyDetail> items)
        {
            if (Count == 0)
            {
                this.AddRange(items);
            }
            else
            {
                var toRemove = new List<PropertyDetail>();
                foreach (var item in this)
                {
                    if (!items.Contains(item))
                    {
                        toRemove.Add(item);
                    }
                }
                foreach (var item in toRemove)
                {
                    Remove(item);
                }
            }
        }
    }
}
