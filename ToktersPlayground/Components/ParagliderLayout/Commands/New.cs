using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground.Components.ParagliderLayout.Commands
{

    /// <summary>
    /// Create a new paraglider layout component
    /// </summary>
    [PlaygroundCommand("Add Component/Paragliding/Paraglider Layout", PlaygroundCommandLocation.ComponentsMenu, icon: "m 107.61796,112.85129 -1.60301,25.12334 m -19.336349,-25.65787 0.400754,28.59783 m -19.236157,-28.0633 0.901695,26.45969 m -27.050846,-24.5888 c 31.192215,-3.39186 61.611783,-4.07844 90.670433,0 -12.76504,34.84055 -86.457931,35.83049 -90.670433,0 z")]
    public class New : PlaygroundCommand
    {
        public override void Execute(IPlayground playgound)
        {
            var c = new ParagliderLayout();
            playgound.Components.Add(c);
            playgound.SelectedComponent = c;
        }

        public override bool CanExecute(IPlayground playground)
        {
            return true;
        }
    }
}
