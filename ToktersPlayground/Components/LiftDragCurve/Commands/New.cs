using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground.Components.LiftDragCurve.Commands
{
    /// <summary>
    /// Create a new lift drag curve component
    /// </summary>
    [PlaygroundCommand("Add Component/Paragliding/Lift Drag Curve", PlaygroundCommandLocation.ComponentsMenu)]
    public class New : PlaygroundCommand
    {
        public override void Execute(IPlayground playgound)
        {
            var c = new LiftDragCurve();
            playgound.Components.Add(c);
            playgound.SelectedComponent = c;
        }

        public override bool CanExecute(IPlayground playground)
        {
            return true;
        }
    }
}
