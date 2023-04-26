using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground.Components.ParagliderWingSimulation.Commands
{
    /// <summary>
    /// Create a new paraglider wing simulation component
    /// </summary>
    [PlaygroundCommand("Add Component/Paragliding/Wing Simulation", PlaygroundCommandLocation.ComponentsMenu)]
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
                var sim = new ParagliderWingSimulation();
                playground.Components.Add(sim);
                playground.SelectedComponent = sim;
            }

            return Task.CompletedTask;
        }
    }
}
