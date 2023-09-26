using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ToktersPlayground.Controls;
using ToktersPlayground.Controls.SceneGraph;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground.Components
{
    public class PlaygroundComponent : IPlaygroundComponent, INotifyPropertyChanged, ICanBeLoadedSaved
    {
        private ViewModelBase? _viewModel = null;
        public string Type { get; private set; }

        public SketchControl? Sketch { get; set; }

        public PlaygroundComponent()
        {
            var attr = this.GetType().GetCustomAttribute<PlaygroundComponentAttribute>();
            if (attr != null)
            {
                Type = attr.Type;
            }
            else
            {
                throw new Exception("PlaygroundComponentAttribute not found!");
            }
        }

        [Property("(Name)")]
        public string Name { get; set; } = "New Component";

        public ViewModelBase ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    _viewModel = CreateViewModel();
                }
                return _viewModel;
            }
        }

        protected virtual ViewModelBase CreateViewModel()
        {
            throw new NotImplementedException();
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SaveTo(XmlWriter writer, LoadSaveOptions options)
        {
            writer.WriteStartElement("Component");
            writer.WriteAttributeString("Name", Name);
            writer.WriteAttributeString("Type", Type);
            OnSave(writer, options);
            if (Sketch != null)
            {
                foreach (var node in Sketch.Scene.Root.Children)
                {
                    if (node is ICanBeLoadedSaved loadSave)
                    {
                        loadSave.SaveTo(writer, options);
                    }
                }
            }
            writer.WriteEndElement();
        }

        protected virtual void OnSave(XmlWriter writer, LoadSaveOptions options)
        {
        }

        public void LoadFrom(XmlElement element, LoadSaveOptions options)
        {
            this.Name = element.GetAttribute("Name");
            OnLoad(element, options);
            if (Sketch != null)
            {
                foreach (XmlElement node in element.ChildNodes)
                {
                    var sceneNode = Sketch.Scene.Root.Children.FirstOrDefault(n => n.Type == node.Name && n.Name == node.GetAttribute("Name"));
                    if (sceneNode is ICanBeLoadedSaved loadSave)
                    {
                        loadSave.LoadFrom(node, options);
                    }
                    else throw new Exception($"Node {node.Name} does not implement ICanBeLoadedSaved!");
                }
            }
        }

        protected virtual void OnLoad(XmlElement element, LoadSaveOptions options)
        {
        }
    }
}
