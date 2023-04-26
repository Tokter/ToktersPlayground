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
        public override bool CanExecute(object? parameter)
        {
            return true;
        }

        public override Task ExecuteAsync(object? parameter)
        {
            if (parameter is IPlayground playground)
            {
                var c = new LiftDragCurve();
                playground.Components.Add(c);
                playground.SelectedComponent = c;
            }
            return Task.CompletedTask;
        }
    }
}
