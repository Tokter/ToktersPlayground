using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.Components.LiftDragCurve.ViewModels;
using ToktersPlayground.Controls;
using ToktersPlayground.Controls.SceneGraph;
using ToktersPlayground.Controls.SceneGraph.EditStates;

namespace ToktersPlayground.Components.LiftDragCurve
{
    public class LiftDragCurveControl : SketchControl
    {
        private LiftDragCurveNode _ldNode;

        public LiftDragCurveControl()
        {
            _ldNode = new LiftDragCurveNode();
            Scene.Root.Add(new SimpleGrid());
            Scene.Root.Add(_ldNode);
            Scene.RegisterEditState(new Pan());
            Scene.RegisterEditState(new Zoom());

            DataContextChanged += LiftDragCurveControl_DataContextChanged;
        }

        private void LiftDragCurveControl_DataContextChanged(object? sender, EventArgs e)
        {
            if (DataContext is LiftDragCurveViewModel vm)
            {
                _ldNode.Curve = vm.Curve;
            }
        }
    }
}
