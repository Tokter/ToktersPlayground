using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground.Components
{
    public class PlaygroundComponent : IPlaygoundComponent, INotifyPropertyChanged
    {
        private string _name = "New Component";
        private ViewModelBase? _viewModel = null;

        public string Type { get; protected set; } = "Playground Component";

        [Property("Name")]
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
