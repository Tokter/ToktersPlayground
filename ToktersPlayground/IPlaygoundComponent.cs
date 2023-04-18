using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground
{
    public interface IPlaygoundComponent
    {
        string Type { get; }
        string Name { get; set; }
        ViewModelBase ViewModel { get; }
    }
}
