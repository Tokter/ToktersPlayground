using Avalonia.Media;
using Avalonia.Metadata;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private IPlayground _playground;

        public IPlaygroundCommand? Command { get; set; } = null;
        public string Name { get; set; } = "Menu Item";
        public int Order { get; set; } = 100;
        public Geometry? Icon { get; set; }
        public IList<MenuViewModel>? Items { get; set; } = null;

        public MenuViewModel(IPlayground playgound)
        {
            _playground = playgound;
        }

        public void Run(object parameter)
        {
            Command?.Execute(_playground);
        }

        [DependsOn("Refresh")]
        bool CanRun(object parameter)
        {
            return Command?.CanExecute(_playground) ?? true;
        }


        public void TriggerRefresh()
        {
            this.RaisePropertyChanged("Refresh");
        }
    }
}
