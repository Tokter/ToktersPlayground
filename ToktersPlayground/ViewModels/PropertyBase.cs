﻿using System;
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
        public string? Name { get; set; }
        public PropertyInfo? Property { get; set; }
        public object? Item { get; set; }

        private Dictionary<string, string> _errors = new Dictionary<string, string>();

        public bool HasErrors => _errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

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
    }
}