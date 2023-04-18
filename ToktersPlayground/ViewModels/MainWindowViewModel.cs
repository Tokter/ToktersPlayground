using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ToktersPlayground.Components;
using ToktersPlayground.Components.LiftDragCurve;

namespace ToktersPlayground.ViewModels
{
    public interface IPlayground
    {
        ObservableCollection<IPlaygoundComponent> Components { get; }
        IPlaygoundComponent? SelectedComponent { get; set; }
    }

    public class MainWindowViewModel : ViewModelBase, IPlayground
    {
        public string Greeting => "Welcome to Avalonia!";
        public IList<MenuViewModel>? MainMenu { get; set; }
        public IList<MenuViewModel>? ComponentsMenu { get; set; }
        public IList<MenuViewModel>? ComponentTools { get; set; }
        public ObservableCollection<IPlaygoundComponent> Components { get; set; } = new ObservableCollection<IPlaygoundComponent>();
        public ObservableCollection<PropertyBase> Properties { get; set; } = new ObservableCollection<PropertyBase>();

        public MainWindowViewModel()
        {
            MainMenu = BuildMenuItems(PlaygroundCommandLocation.MainMenu).Items;
            ComponentsMenu = BuildMenuItems(PlaygroundCommandLocation.ComponentsMenu).Items;
            ComponentTools = BuildMenuItems(PlaygroundCommandLocation.ComponentsTools).Items;
        }

        private IPlaygoundComponent? _selectedComponent;
        public IPlaygoundComponent? SelectedComponent
        {
            get => _selectedComponent;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedComponent, value);
                this.RaisePropertyChanged(nameof(ComponentViewModel));
                RefreshCommands();
                BuildPropertyList();
            }
        }

        public ViewModelBase? ComponentViewModel => _selectedComponent?.ViewModel;

        private MenuViewModel BuildMenuItems(PlaygroundCommandLocation location)
        {
            MenuViewModel root = new MenuViewModel(this);
            var type = typeof(IPlaygroundCommand);
            var commands = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => type.IsAssignableFrom(p));

            foreach (var command in commands)
            {
                var attribute = command.GetCustomAttributes(typeof(PlaygroundCommandAttribute), false).FirstOrDefault() as PlaygroundCommandAttribute;
                if (attribute != null && attribute.Location == location)
                {
                    var commandInstance = Activator.CreateInstance(command) as IPlaygroundCommand;
                    if (commandInstance != null)
                    {
                        var menuItem = new MenuViewModel(this)
                        {
                            Name = attribute.Name,
                            Command = commandInstance,
                            Order = attribute.Order,
                        };

                        if (!string.IsNullOrEmpty(attribute.Icon))
                        {
                            menuItem.Icon = Geometry.Parse(attribute.Icon);
                        }

                        //Find the place where to insert the menu item
                        var parts = menuItem.Name.Split("/");
                        MenuViewModel current = root;
                        for (int i = 0; i < parts.Length; i++)
                        {
                            //If it's the last item, we insert the created menu item
                            if (i == parts.Length - 1)
                            {
                                menuItem.Name = parts[i];
                                if (current.Items == null) current.Items = new List<MenuViewModel>();
                                var insertIndex = current.Items?.TakeWhile(p => p.Order < menuItem.Order).Count() ?? 0;
                                current.Items?.Insert(insertIndex, menuItem);
                            }
                            else //Create a parent menu item
                            {
                                var parent = current.Items?.FirstOrDefault(p => p.Name == parts[i]);
                                if (parent == null)
                                {
                                    parent = new MenuViewModel(this);
                                    parent.Name = parts[i];
                                    parent.Order = menuItem.Order;
                                    var insertIndex = current.Items?.TakeWhile(p => p.Order < menuItem.Order).Count() ?? 0;
                                    if (current.Items == null) current.Items = new List<MenuViewModel>();
                                    current.Items.Insert(insertIndex, parent);
                                }
                                current = parent!;
                            }
                        }
                    }
                }
            }

            return root;
        }

        private void RefreshCommands()
        {
            RefreshCommands(MainMenu!);
            RefreshCommands(ComponentsMenu!);
            RefreshCommands(ComponentTools!);
        }

        private void RefreshCommands(IList<MenuViewModel> items)
        {
            foreach (var item in items)
            {
                item.TriggerRefresh();
            }
        }

        private void BuildPropertyList()
        {
            Properties.Clear();
            if (SelectedComponent != null)
            {
                foreach(var property in SelectedComponent.GetType().GetProperties())
                {
                    var propertyAttribute = property.GetCustomAttributes(typeof(PropertyAttribute), false).FirstOrDefault() as PropertyAttribute;
                    if (propertyAttribute != null)
                    {
                        switch (property.PropertyType.Name)
                        {
                            case "String":
                                var stringProperty = new PropertyStringViewModel();
                                stringProperty.Name = propertyAttribute.DisplayName ?? property.Name;
                                stringProperty.Property = property;
                                stringProperty.Item = SelectedComponent;
                                Properties.Add(stringProperty);
                                break;

                            case "Single":
                                var floatProperty = new PropertyFloatViewModel();
                                floatProperty.Name = propertyAttribute.DisplayName ?? property.Name;
                                floatProperty.Property = property;
                                floatProperty.Item = SelectedComponent;
                                floatProperty.StringFormat = propertyAttribute.StringFormat;
                                Properties.Add(floatProperty);
                                break;
                        }
                    }
                }
            }
        }

    }
}