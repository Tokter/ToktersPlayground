using Avalonia.Media;
using Avalonia.Metadata;
using Microsoft.VisualBasic;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private IPlayground _playground;
        public IPlaygroundCommand? Command { get; set; }
        public IPlayground Playground => _playground;

        public string Name { get; set; } = "Menu Item";
        public int Order { get; set; } = 100;
        public Geometry? Icon { get; set; }
        public IList<MenuViewModel>? Items { get; set; } = null;

        public MenuViewModel(IPlayground playground)
        {
            _playground = playground;
        }

        public void Sort()
        {
            if (Items != null)
            {
                var sorted = Items.OrderBy(x => x.Order).ToList();
                Items.Clear();
                foreach (var item in sorted)
                {
                    Items.Add(item);
                    item.Sort();
                }
            }
        }
    }
}
