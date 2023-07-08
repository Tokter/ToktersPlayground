using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground.Components.LiftDragCurve.ViewModels
{
    public class LiftDragCurveViewModel : ViewModelBase
    {
        public LiftDragCurve Curve { get; private set; }

        public LiftDragCurveViewModel(LiftDragCurve curve)
        {
            Curve = curve;
        }
    }
}
