using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.Components.LiftDragCurve.ViewModels;
using ToktersPlayground.Components.LiftDragCurve;
using ToktersPlayground.Controls.SceneGraph.EditStates;
using ToktersPlayground.Controls.SceneGraph;
using ToktersPlayground.Controls;
using ToktersPlayground.Components.ParagliderLayout.ViewModels;
using ToktersPlayground.Components.ParagliderLayout.SceneGraph;

namespace ToktersPlayground.Components.ParagliderLayout
{
    public class ParagliderLayoutControl : SketchControl
    {
        private ParagliderLayoutNode _plNode;

        public ParagliderLayoutControl()
        {
            _plNode = new ParagliderLayoutNode();
            Scene.Root.Add(new SimpleGrid());
            Scene.Root.Add(_plNode);
            Scene.RegisterEditState(new Pan());
            Scene.RegisterEditState(new Zoom());
            Scene.RegisterEditState(new Drag());
            Scene.RegisterEditState(new Select());
            Scene.RegisterEditState(new InsertVertex());

            DataContextChanged += ParagliderLayoutControl_DataContextChanged;
        }

        private void ParagliderLayoutControl_DataContextChanged(object? sender, EventArgs e)
        {
            if (DataContext is ParagliderLayoutViewModel vm)
            {
                _plNode.Layout = vm.Layout;
                vm.Layout.Sketch = this;
            }
        }
    }
}
