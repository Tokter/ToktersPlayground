using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground
{
    public class ViewLocator : IDataTemplate
    {
        public Control? Build(object? data)
        {
            if (data is ViewModelBase vm)
            {
                if (vm.View == null)
                {
                    var name = data.GetType().FullName!.Replace("ViewModel", "View");
                    var type = Type.GetType(name);

                    if (type != null)
                    {
                        vm.View = (Control)Activator.CreateInstance(type)!;
                    }
                    else
                    {
                        vm.View = new TextBlock { Text = name };
                    }
                }
                return vm.View;
            }
            return null;
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}
