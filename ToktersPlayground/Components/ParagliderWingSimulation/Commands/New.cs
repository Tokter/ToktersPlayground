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
    [PlaygroundCommand("Components/Paragliding/Wing Simulation", PlaygroundCommandLocation.ComponentsMenu)]
    public class New : PlaygroundCommand
    {
        public override void Execute(IPlayground playgound)
        {
            var sim = new ParagliderWingSimulation();
            playgound.Components.Add(sim);
            playgound.SelectedComponent = sim;
        }

        public override bool CanExecute(IPlayground playgound)
        {
            return true;
        }
    }
}
