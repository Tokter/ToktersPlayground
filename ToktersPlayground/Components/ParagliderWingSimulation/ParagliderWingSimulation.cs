﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.Components.LiftDragCurve.ViewModels;
using ToktersPlayground.Components.ParagliderWingSimulation.ViewModels;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground.Components.ParagliderWingSimulation
{
    [PlaygroundComponent("Wing Simulation")]
    public class ParagliderWingSimulation : PlaygroundComponent
    {
        public ParagliderWingSimulation()
        {
            Name = "Paraglider Wing Simulation";
        }

        protected override ViewModelBase CreateViewModel()
        {
            return new ParagliderWingSimulationViewModel(this);
        }
    }      
}
