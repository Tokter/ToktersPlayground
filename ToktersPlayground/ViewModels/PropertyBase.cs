using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.ViewModels
{
    public class PropertyBase : ViewModelBase, INotifyDataErrorInfo
    {
        private PropertyEditorViewModel _propertyEditor;
        private PropertyDetail _details;
        private Dictionary<string, string> _errors = new Dictionary<string, string>();

        public string? Name { get; set; }
        public bool HasErrors => _errors.Count > 0;
        public PropertyEditorViewModel Editor => _propertyEditor;
        public PropertyDetail Details => _details;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public PropertyBase(PropertyEditorViewModel propertyEditor, PropertyDetail details)
        {
            _propertyEditor = propertyEditor;
            _details = details;
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName) && _errors.ContainsKey(propertyName))
            {
                yield return _errors[propertyName];
            }
        }

        protected void SetError(string error, [CallerMemberName] string propertyName = null!)
        {
            if (_errors.ContainsKey(propertyName)) _errors.Remove(propertyName);
            _errors.Add(propertyName, error);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void ClearError([CallerMemberName] string propertyName = null!)
        {
            if (_errors.ContainsKey(propertyName)) _errors.Remove(propertyName);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public virtual void Update()
        {
            throw new NotImplementedException(nameof(Update));
        }
    }
}
