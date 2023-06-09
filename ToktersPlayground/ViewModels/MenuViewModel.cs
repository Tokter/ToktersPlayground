﻿using Avalonia.Media;
using Avalonia.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly IPlayground _playground;
        public IPlaygroundCommand? Command { get; set; }
        public IPlayground Playground => _playground;
        public IPlaygroundComponent? Component => _playground.SelectedComponent;

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
