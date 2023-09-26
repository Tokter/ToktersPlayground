using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using ToktersPlayground.Components;
using ToktersPlayground.Components.LiftDragCurve;
using ToktersPlayground.Controls.SceneGraph;

namespace ToktersPlayground.ViewModels
{
    public interface IPlayground
    {
        ObservableCollection<IPlaygroundComponent> Components { get; }
        IPlaygroundComponent? SelectedComponent { get; set; }
        IPlaygroundComponent? CreateComponent(string type);
        SceneNode? CreateSceneNode(string type);
        string ProjectFileName { get; set; }
        void Clear();
    }

    public class MainWindowViewModel : ViewModelBase, IPlayground
    {
        public static string Greeting => "Welcome to Avalonia!";
        public IList<MenuViewModel>? MainMenu { get; set; }
        public IList<MenuViewModel>? ComponentsMenu { get; set; }
        public IList<MenuViewModel>? ComponentTools { get; set; }
        public ObservableCollection<IPlaygroundComponent> Components { get; set; } = new ObservableCollection<IPlaygroundComponent>();
        public ObservableCollection<PropertyBase> Properties { get; set; } = new ObservableCollection<PropertyBase>();
        public PropertyEditorViewModel PropertyEditor { get; set; } = new PropertyEditorViewModel();

        public MainWindowViewModel(Control view)
        {
            View = view;
            MainMenu = BuildMenuItems(PlaygroundCommandLocation.MainMenu).Items;
            ComponentsMenu = BuildMenuItems(PlaygroundCommandLocation.ComponentsMenu).Items;
            Components.CollectionChanged += (s, e) =>
            {
                RefreshCommands();
            };
        }

        public void Clear()
        {
            SelectedComponent = null;
            Components.Clear();
        }

        private int _selectedTab = 0;
        public int SelectedTab
        {
            get => _selectedTab;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedTab, value);
                UpdatePropertyEditor();
            }
        }

        private IPlaygroundComponent? _selectedComponent;
        public IPlaygroundComponent? SelectedComponent
        {
            get => _selectedComponent;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedComponent, value);
                this.RaisePropertyChanged(nameof(ComponentViewModel));
                if (_selectedComponent != null)
                {
                    ComponentTools = BuildMenuItems(PlaygroundCommandLocation.ComponentsTools, _selectedComponent.GetType()).Items;
                    this.RaisePropertyChanged(nameof(ComponentTools));
                }
                RefreshCommands();
                UpdatePropertyEditor();
                this.RaisePropertyChanged(nameof(Nodes));
            }
        }

        public SceneNodeList? Nodes
        {
            get => _selectedComponent?.Sketch?.Scene.Root.Children;
        }
        
        private SceneNode? _selectedNode;
        public SceneNode? SelectedNode
        {
            get => _selectedNode;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedNode, value);
                UpdatePropertyEditor();
            }
        }

        private void UpdatePropertyEditor()
        {
            switch (_selectedTab)
            {
                case 0:
                    PropertyEditor.SelectObject(_selectedComponent);
                    break;

                case 1:
                    PropertyEditor.SelectObject(_selectedNode);
                    break;
            }
        }

        public IPlaygroundComponent? CreateComponent(string type)
        {
            var typesWithMyAttribute = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(typeof(PlaygroundComponentAttribute)));
            foreach(var t in typesWithMyAttribute)
            {
                if (t.GetCustomAttribute<PlaygroundComponentAttribute>()?.Type == type)
                {
                    if (Activator.CreateInstance(t) is IPlaygroundComponent component)
                    {
                        Components.Add(component);
                        SelectedComponent = component;
                        return component;
                    }
                }
            }
            return null;
        }

        public SceneNode? CreateSceneNode(string typeName)
        {
            //Find all types that inherit from SceneNode
            var sceneNodeType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.IsSubclassOf(typeof(SceneNode)) && t.Name == typeName);
            if (sceneNodeType != null)
            {
                return Activator.CreateInstance(sceneNodeType) as SceneNode;
            }
            else
            {
                throw new Exception($"SceneNode type {typeName} not found!");
            }
        }

        public string ProjectFileName { get; set; } = "New.playground";

        public ViewModelBase? ComponentViewModel => _selectedComponent?.ViewModel;
        public IPlayground Playground => this;

        private MenuViewModel BuildMenuItems(PlaygroundCommandLocation location, Type? componentType = null)
        {
            MenuViewModel root = new(this);
            var type = typeof(IPlaygroundCommand);
            var commands = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => type.IsAssignableFrom(p));

            foreach (var command in commands)
            {
                if (command.GetCustomAttributes(typeof(PlaygroundCommandAttribute), false).FirstOrDefault() is PlaygroundCommandAttribute attribute && attribute.Location == location && (attribute.ComponentType == null || attribute.ComponentType == componentType))
                {
                    if (Activator.CreateInstance(command) is IPlaygroundCommand commandInstance)
                    {
                        var menuItem = new MenuViewModel(this)
                        {
                            Name = attribute.Name,
                            Command = commandInstance,
                            Order = attribute.Order,
                        };

                        commandInstance.StorageProvider = TopLevel.GetTopLevel(this.View)?.StorageProvider;


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
                                current.Items ??= new List<MenuViewModel>();
                                var insertIndex = current.Items?.TakeWhile(p => p.Order < menuItem.Order).Count() ?? 0;
                                current.Items?.Insert(insertIndex, menuItem);
                            }
                            else //Create a parent menu item
                            {
                                var parent = current.Items?.FirstOrDefault(p => p.Name == parts[i]);
                                if (parent == null)
                                {
                                    parent = new MenuViewModel(this)
                                    {
                                        Name = parts[i],
                                        Order = menuItem.Order
                                    };
                                    var insertIndex = current.Items?.TakeWhile(p => p.Order < menuItem.Order).Count() ?? 0;
                                    current.Items ??= new List<MenuViewModel>();
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
            RefreshCommands(MainMenu);
            RefreshCommands(ComponentsMenu);
            RefreshCommands(ComponentTools);
        }

        private void RefreshCommands(IList<MenuViewModel>? items)
        {
            if (items == null) return;
            foreach (var item in items)
            {
                item.Command?.RaiseCanExecuteChanged();
                if (item.Items!= null) RefreshCommands(item.Items);
            }
        }
    }
}