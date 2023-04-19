using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground.Components.ParagliderLayout.ViewModels
{
    public class ParagliderLayoutViewModel : ViewModelBase
    {
        public ParagliderLayout Layout{ get; private set; }

        public ParagliderLayoutViewModel(ParagliderLayout layout)
        {
            Layout = layout;
        }
    }
}
