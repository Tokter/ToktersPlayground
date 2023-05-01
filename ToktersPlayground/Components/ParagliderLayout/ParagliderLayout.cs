using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.Components.LiftDragCurve.ViewModels;
using ToktersPlayground.Components.ParagliderLayout.SceneGraph;
using ToktersPlayground.Components.ParagliderLayout.ViewModels;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground.Components.ParagliderLayout
{
    public class ParagliderLayout : PlaygroundComponent
    {
        public ParagliderLayoutControl? LayoutControl => this.Sketch as ParagliderLayoutControl;

        public ParagliderLayout()
        {
            Name = "New Paraglider Layout";
            Type = "Paraglider Layout";
        }

        [Property("Layout Image", EditorType = EditorType.FileLocation)]
        public string LayoutImage { get; set; } = "";

        protected override ViewModelBase CreateViewModel()
        {
            return new ParagliderLayoutViewModel(this);
        }
    }
}
