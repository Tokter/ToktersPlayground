using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.Components.LiftDragCurve.ViewModels;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground.Components.LiftDragCurve
{
    public class LiftDragCurve : PlaygroundComponent
    {
        public LiftDragCurve()
        {
            Name = "New Curve";
            Type = "Lift/Drag Curve";
        }

        [Property("AOA Min", "n0")]
        public float AngleOfAttackMin { get; set; } = 0.0f;

        [Property("AOA Max", "n0")]
        public float AngleOfAttackMax { get; set; } = 25.0f;

        [Property("CoL Min", "n1")]
        public float LiftCoefficientMin { get; set; } = 0.0f;

        [Property("CoL Max", "n1")]
        public float LiftCoefficientMax { get; set; } = 2.0f;

        [Property("CoD Min", "n2")]
        public float DragCoefficientMin { get; set; } = 0.0f;

        [Property("CoD Max", "n2")]
        public float DragCoefficientMax { get; set; } = 0.2f;

        protected override ViewModelBase CreateViewModel()
        {
            return new LiftDragCurveViewModel(this);
        }
    }
}
