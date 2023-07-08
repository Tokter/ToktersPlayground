using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tmds.DBus.SourceGenerator;
using ToktersPlayground.Components;

namespace ToktersPlayground.ViewModels
{
    public class PropertyEditorViewModel : ViewModelBase
    {
        private readonly List<object> _selectedObjects = new();
        private readonly List<Subscription> _subscriptions = new();
        private readonly PropertyDetails _propertyDetails = new();
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

        public void SubscribedPropertyChanged(string propertyName)
        {
            foreach (var p in Properties.Where(p => p.Details.Property.Name == propertyName))
            {
                p.Update();
            }
        }

        private void BuildPropertyList()
        {
            _propertyDetails.Clear();
            Properties.Clear();
            ClearSubscriptions();

            foreach (var obj in _selectedObjects)
            {
                var type = obj.GetType();
                var properties = type.GetProperties();
                var details = new List<PropertyDetail>();
                foreach (var property in properties)
                {
                    if (property.GetCustomAttributes(typeof(PropertyAttribute), false).FirstOrDefault() is PropertyAttribute propertyAttribute)
                    {
                        details.Add(new PropertyDetail(property, propertyAttribute));
                    }
                }
                _propertyDetails.Add(details);
            }

            foreach(var obj in _selectedObjects)
            {
                var sub = new Subscription(obj, _propertyDetails.Select(pd => pd.Property.Name), SubscribedPropertyChanged);
                if (sub.Object != null) _subscriptions.Add(sub);
            }

            foreach (var pd in _propertyDetails.OrderBy(p=>p.Attribute.DisplayName))
            {
                switch (pd.Attribute.EditorType)
                {
                    case EditorType.Default:
                        switch (pd.Property.PropertyType.Name)
                        {
                            case "String":
                                var stringProperty = new PropertyStringViewModel(this, pd)
                                {
                                    Name = pd.Attribute.DisplayName ?? pd.Property.Name
                                };
                                Properties.Add(stringProperty);
                                break;

                            case "Single":
                                var floatProperty = new PropertyFloatViewModel(this, pd)
                                {
                                    Name = pd.Attribute.DisplayName ?? pd.Property.Name,
                                    StringFormat = pd.Attribute.StringFormat
                                };
                                Properties.Add(floatProperty);
                                break;

                            case "Byte":
                                var byteProperty = new PropertyByteViewModel(this, pd)
                                {
                                    Name = pd.Attribute.DisplayName ?? pd.Property.Name
                                };
                                Properties.Add(byteProperty);
                                break;

                            case "Int":
                            case "Int32":
                                var intProperty = new PropertyIntViewModel(this, pd)
                                {
                                    Name = pd.Attribute.DisplayName ?? pd.Property.Name
                                };
                                Properties.Add(intProperty);
                                break;

                            case "Boolean":
                                var boolProperty = new PropertyBoolViewModel(this, pd)
                                {
                                    Name = pd.Attribute.DisplayName ?? pd.Property.Name
                                };
                                Properties.Add(boolProperty);
                                break;

                            case "Vector2":
                                var vector2Property = new PropertyVector2ViewModel(this, pd)
                                {
                                    Name = pd.Attribute.DisplayName ?? pd.Property.Name
                                };
                                Properties.Add(vector2Property);
                                break;

                            default:
                                switch (pd.Property.PropertyType.BaseType?.Name)
                                {
                                    case "Enum":
                                        var enumProperty = new PropertyEnumViewModel(this, pd)
                                        {
                                            Name = pd.Attribute.DisplayName ?? pd.Property.Name,
                                            EnumType = pd.Property.PropertyType
                                        };
                                        Properties.Add(enumProperty);
                                        break;
                                }
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

        private void ClearSubscriptions()
        {
            foreach(var sub in _subscriptions)
            {
                sub.Unsubscribe();
            }
            _subscriptions.Clear();
        }
    }

    public class Subscription
    {
        public HashSet<string> Properties { get; set; } = new HashSet<string>();
        public object? Object { get; set; }
        public Action<string>? Action { get; set; }

        public Subscription(object obj, IEnumerable<string> properties, Action<string> action)
        {
            if (obj is INotifyPropertyChanged notifyPropertyChanged)
            {
                Object = obj;
                foreach(var p in properties)
                {
                    Properties.Add(p);
                }
                Action = action;

                notifyPropertyChanged.PropertyChanged += PropertyChanged;
            }
        }

        public void Unsubscribe()
        {
            if (Object is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged -= PropertyChanged;
            }
        }

        public void PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null && Properties.Contains(e.PropertyName))
            {
                Action?.Invoke(e.PropertyName);
            }
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

        private void AddRange(IEnumerable<PropertyDetail> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }
    }
}
