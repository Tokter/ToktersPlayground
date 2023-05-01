using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.Controls;
using ToktersPlayground.Controls.SceneGraph;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground.Components
{
    public class PlaygroundComponent : IPlaygroundComponent, INotifyPropertyChanged
    {
        private string _name = "New Component";
        private ViewModelBase? _viewModel = null;
        public string Type { get; protected set; } = "Playground Component";

        public SketchControl? Sketch { get; set; }

        public PlaygroundComponent()
        {
            var attr = this.GetType().GetCustomAttribute<PlaygroundComponentAttribute>();
            if (attr != null)
            {
                Type = attr.Type;
            }
        }

        [Property("(Name)")]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

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
    }
}
