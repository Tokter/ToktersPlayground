using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground.Components.ParagliderWingSimulation.ViewModels
{
    public class ParagliderWingSimulationViewModel : ViewModelBase
    {
        private ParagliderWingSimulation _simulation;

        public ParagliderWingSimulationViewModel(ParagliderWingSimulation simulation)
        {
            _simulation = simulation;
        }
    }
}
